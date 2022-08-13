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
    public class rptSampleInvoice_F2
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
        int _PageHeight = 700;
        string _sMessage = "";
        string _sWaterMark = "";
        int _nCount = 0;
        double _nTotalAmount = 0;
        double _nTotalQty = 0;

        double _nGrandTotalAmount = 0;
        double _nGrandTotalQty = 0;
        double _nTotalAmountCRate = 0;
        double _nGrandTotalCRate = 0;
        float nUsagesHeight = 0;
        string _sMunit = "";
        int _nTitleType = 0;
       
        #endregion

        public byte[] PrepareReport(SampleInvoice oSampleInvoice, Company oCompany, List<DyeingOrderDetail> oDyeingOrderDetails, BusinessUnit oBusinessUnit, bool bIsDebitNote, SampleInvoiceSetup oSampleInvoiceSetup, int nTitleType)
        {
            _oSampleInvoice = oSampleInvoice;
            _oSampleInvoiceSetup = oSampleInvoiceSetup;
            _oDyeingOrderDetails = oDyeingOrderDetails;
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
                this.PrintHead_DebitNote();
                this.PrintWaterMark(30f, 30f, 30f, 30f);
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
                this.PrintHead_Invoice();
                if (_oDyeingOrderDetails.Count > 0)
                {
                    //this.PrintBody();
                    this.SetDyeingOrderDetail();
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

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
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
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 10f));
            oPdfPTable.AddCell(this.SetCellValue(_oBusinessUnit.PringReportHead, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 10f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 10f));

            AddTable(ref _oPdfPTable, oPdfPTable);
            #endregion

            #region ReportHeader
            _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 10f));
            #endregion
        }
        #endregion

        #region Report Body
        //private void PrintBody()
        //{
        //    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
        //    _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, 1);
        //    _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, 4);

        //    #region Balank Space
        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();
        //    #endregion

        //    #region Short Info
        //    _oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail());
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();

        //    #endregion

        //    #region Balank Space
        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();
        //    #endregion          
        //}
        #endregion

        private void PrintHead_Invoice()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, 1);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, 4);

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 100, 140f, 100f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            oPdfPTable.AddCell(this.SetCellValue("DATE: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue(_oSampleInvoice.SampleInvoiceDateST, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));
            oPdfPTable.AddCell(this.SetCellValue("SAMPLE INVOICE NO: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue(_oSampleInvoice.InvoiceNo, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));
            oPdfPTable.AddCell(this.SetCellValue("BUYER NAME: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            oPdfPTable.AddCell(this.SetCellValue(_oSampleInvoice.ContractorName, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            oPdfPTable.AddCell(this.SetCellValue("REQUIREMENT DATE: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue(_oSampleInvoice.RequirementDateST, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));
            oPdfPTable.AddCell(this.SetCellValue("CONCERN PERSON: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue(_oSampleInvoice.MKTPName, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));
            oPdfPTable.AddCell(this.SetCellValue("MODE OF PAYMENT: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
           
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
            oPdfPTable.AddCell(this.SetCellValue(sTemp + " " + sTempTwo, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));
            AddTable(ref _oPdfPTable, oPdfPTable);
        }
        private void ReporttHeader()
        {
            #region Proforma Invoice Heading Print
            _oPdfPCell = new PdfPCell();
            if (_oSampleInvoice.ApproveBy == 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoiceSetup.PrintName + "(Unauthorised)", FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD)));
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
      
            #endregion
        }

        private void SetDyeingOrderDetail()
        {
            _nTotalAmount = 0;
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(8);
            oPdfPTable.SetWidths(new float[] { 100f, 120f, 100f, 50, 70f, 60f, 70f, 70f });
            int nProductID = 0;
            if (_oDyeingOrderDetails.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 3);
                oPdfPTable.AddCell(this.SetCellValue("Product Information", 0, 8, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 1, 10f));
                _oDyeingOrderDetails = _oDyeingOrderDetails.OrderBy(o => o.ProductID).ToList();

                foreach (DyeingOrderDetail oDyeingOrderDetail in _oDyeingOrderDetails)
                {
                    if (nProductID != oDyeingOrderDetail.ProductID)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        if (nProductID > 0)
                        {
                            #region Total
                            oPdfPTable.AddCell(this.SetCellValue("Total: ", 0, 4, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                            oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                            oPdfPTable.AddCell(this.SetCellValue("$" + Global.MillionFormat(_nTotalAmount), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                          
                            if (_nTotalAmountCRate > 0)
                            {
                                oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_nTotalAmountCRate), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                            }
                            else
                            {
                                oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                            }
                            #endregion

                            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, 1);

                            oPdfPTable = new PdfPTable(8);
                            oPdfPTable.SetWidths(new float[] { 100f, 120f, 100f, 50, 70f, 60f, 70f, 70f });

                            nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

                            if (nUsagesHeight > 740)
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

                        }
                        #region Header
                        oPdfPTable.AddCell(this.SetCellValue("Yarn Type: " + oDyeingOrderDetail.ProductNameCode, 0, 8, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                        oPdfPTable.AddCell(this.SetCellValue("SAMPLE NO", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                        oPdfPTable.AddCell(this.SetCellValue("COLOR", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                        oPdfPTable.AddCell(this.SetCellValue("COLOR NO", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                        oPdfPTable.AddCell(this.SetCellValue("SHADE", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                        oPdfPTable.AddCell(this.SetCellValue("QTY(LBS)", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                        oPdfPTable.AddCell(this.SetCellValue("U/P($)", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                        oPdfPTable.AddCell(this.SetCellValue("Amount($)", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));

                        if (_oSampleInvoice.ConversionRate <= 1)
                        {
                            oPdfPTable.AddCell(this.SetCellValue("REMARKS", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                        }
                        else
                        {
                            oPdfPTable.AddCell(this.SetCellValue("Amount(" + _oSampleInvoice.CurrencySymbol + ")", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                        }

                        oPdfPTable.CompleteRow();
                        #endregion
                        _nTotalQty = 0;
                        _nTotalAmount = 0;
                        _nTotalAmountCRate = 0;
                        _nCount = 0;
                    }

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                    #region PrintDetail
                    _nCount++;
                    oPdfPTable.AddCell(this.SetCellValue(oDyeingOrderDetail.OrderNo, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                    oPdfPTable.AddCell(this.SetCellValue(oDyeingOrderDetail.ColorName, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                    oPdfPTable.AddCell(this.SetCellValue(oDyeingOrderDetail.ColorNo, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                    oPdfPTable.AddCell(this.SetCellValue(oDyeingOrderDetail.ShadeSt, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oDyeingOrderDetail.Qty), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oDyeingOrderDetail.UnitPrice), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
         
                    if (_oSampleInvoice.ConversionRate <= 1)
                    {
                        oPdfPTable.AddCell(this.SetCellValue(oDyeingOrderDetail.Note, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                    }
                    else
                    {
                        _nTotalAmountCRate = _nTotalAmountCRate + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice * _oSampleInvoice.ConversionRate);
                        _nGrandTotalCRate = _nGrandTotalCRate + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice * _oSampleInvoice.ConversionRate);
                        oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice * _oSampleInvoice.ConversionRate), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                    }
                    _nTotalAmount = _nTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                    _nTotalQty = _nTotalQty + oDyeingOrderDetail.Qty;
                    _nGrandTotalAmount = _nGrandTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                    _nGrandTotalQty = _nGrandTotalQty + oDyeingOrderDetail.Qty;
                      
                    oPdfPTable.CompleteRow();
                    #endregion

                    nProductID = oDyeingOrderDetail.ProductID;
                }

                oPdfPTable = new PdfPTable(8);
                oPdfPTable.SetWidths(new float[] { 100f, 120f, 100f, 50, 70f, 60f, 70f, 70f });

                #region Total
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                oPdfPTable.AddCell(this.SetCellValue("Total: ", 0, 4, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("$" + Global.MillionFormat(_nTotalAmount), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));

                if (_nTotalAmountCRate > 0)
                {
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_nTotalAmountCRate), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                }
                else
                {
                    oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                }
                #endregion
                #region Grand Total
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                oPdfPTable.AddCell(this.SetCellValue("Grand Total: ", 0, 4, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_nGrandTotalQty), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("$" + Global.MillionFormat(_nTotalAmount), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));

                if (_nGrandTotalCRate > 0)
                {
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_nGrandTotalCRate), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                }
                else
                {
                    oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                }

                #endregion
                #region Grand Total in Words
                if (_nGrandTotalCRate > 0)
                {
                    oPdfPTable.AddCell(this.SetCellValue("Amount In Words: " + Global.TakaWords(_nGrandTotalCRate), 0, 8, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                }
                else
                {
                    oPdfPTable.AddCell(this.SetCellValue("Amount In Words: " + Global.DollarWords(_nGrandTotalCRate), 0, 8, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                }
                #endregion

                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, 1);
            }
            //return oPdfPTable;
        }

        private void PrintFooter()
        {
            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 148f, 148f, 148f, 148f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            oPdfPTable.AddCell(this.SetCellValue("Prepared By", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue("Accounts", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue("Marketing Manager", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue("Managing Director", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 45f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 45f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 45f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 45f));

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
            _oFontStyle = FontFactory.GetFont("Tahoma",8f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, 1);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, 4);

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 100, 140f, 100f });


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            oPdfPTable.AddCell(this.SetCellValue("DATE: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue(_oSampleInvoice.SampleInvoiceDateST, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));
            oPdfPTable.AddCell(this.SetCellValue("DEBIT NOTE NO: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue(_oSampleInvoice.InvoiceNo, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));
            oPdfPTable.AddCell(this.SetCellValue("BUYER NAME: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            oPdfPTable.AddCell(this.SetCellValue(_oSampleInvoice.ContractorName, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));
            
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            oPdfPTable.AddCell(this.SetCellValue("CONCERN PERSON: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue(_oSampleInvoice.MKTPName, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));
            oPdfPTable.AddCell(this.SetCellValue("MODE OF PAYMENT: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
           
            
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
            oPdfPTable.AddCell(this.SetCellValue(sTemp + " " + sTempTwo, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));
           
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void Print_Body_SampleInvoice()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, 1);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, 4);

            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 30f, 90f, 180f, 75f, 70f, 75f, 75f });
            _nTotalQty = 0;
            _nTotalAmount = 0;
            _nCount = 0;

            if (_oSampleInvoice.SampleInvoiceDetails .Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                oPdfPTable.AddCell(this.SetCellValue("SL#", 0, 8, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("SAMPLE NO", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("Product Description", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("Qty", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));

                string sUnitPrice = _oSampleInvoice.RateUnit > 1 ? "U/P(" + _oSampleInvoice.CurrencySymbol + ")/" + _oSampleInvoice.RateUnit : "U/P(" + _oSampleInvoice.CurrencySymbol + ")";
                oPdfPTable.AddCell(this.SetCellValue(sUnitPrice, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("Amount(" + _oSampleInvoice.CurrencySymbol + ")", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
               
               
                if (_oSampleInvoice.ConversionRate <= 1)
                {
                    oPdfPTable.AddCell(this.SetCellValue("REMARKS", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                }
                else
                {
                    oPdfPTable.AddCell(this.SetCellValue("Amount(" + _oSampleInvoice.CurrencySymbol + ")", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                }


                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                foreach (SampleInvoiceDetail oItem in _oSampleInvoice.SampleInvoiceDetails)
                {
                    _nCount++;
                    #region PrintDetail
                    oPdfPTable.AddCell(this.SetCellValue(_nCount.ToString(), 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(oItem.OrderNo, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(oItem.ProductName, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oItem.Qty), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oItem.UnitPrice), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oItem.Qty * (oItem.UnitPrice / _oSampleInvoice.RateUnit)), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oItem.Qty * (oItem.UnitPrice / _oSampleInvoice.RateUnit) * _oSampleInvoice.ConversionRate), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));      
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
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                oPdfPTable.AddCell(this.SetCellValue("Total: ", 0, 3, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("$" + Global.MillionFormat(_nTotalAmount), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_nTotalAmount * _oSampleInvoice.ConversionRate), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                #endregion
                #region Total in World
                if (_oSampleInvoice.ConversionRate > 1)
                {
                    oPdfPTable.AddCell(this.SetCellValue("Amount In Words: " + Global.TakaWords(_nTotalAmount * _oSampleInvoice.ConversionRate), 0, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                }
                else
                {
                    oPdfPTable.AddCell(this.SetCellValue("Amount In Words: " + Global.DollarWords(_nTotalAmount), 0, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                }
                #endregion
            }
            if (!string.IsNullOrEmpty(_oSampleInvoice.Remark))
            {
                oPdfPTable.AddCell(this.SetCellValue("Remark: " + _oSampleInvoice.Remark, 0, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
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

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                oPdfPTable.AddCell(this.SetCellValue("SL#", 0, 8, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("SAMPLE NO", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("YARN TYPE", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("Qty(LBS)", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("U/P(" + _oSampleInvoice.CurrencySymbol + ")", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("Amount(" + _oSampleInvoice.CurrencySymbol + ")", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));

                if (_oSampleInvoice.ConversionRate <= 1)
                {
                    oPdfPTable.AddCell(this.SetCellValue("REMARKS", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                }
                else
                {
                    oPdfPTable.AddCell(this.SetCellValue("Amount(" + _oSampleInvoice.ExchangeCurrencySymbol + ")", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                }

                foreach (DyeingOrderDetail oItem in _oDyeingOrderDetails_Product)
                {
                    _nCount++;
                    #region PrintDetail

                    oPdfPTable.AddCell(this.SetCellValue(_nCount.ToString(), 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(oItem.OrderNo, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(oItem.ProductName, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oItem.Qty), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oItem.UnitPrice), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oItem.Qty * oItem.UnitPrice), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oItem.Qty * oItem.UnitPrice * _oSampleInvoice.ConversionRate), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
         
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
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                oPdfPTable.AddCell(this.SetCellValue("Total: ", 0, 3, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("$" + Global.MillionFormat(_nTotalAmount), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_nTotalAmount * _oSampleInvoice.ConversionRate), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                #endregion
                #region Total in World
                if (_oSampleInvoice.ConversionRate > 1)
                {
                    oPdfPTable.AddCell(this.SetCellValue("Amount In Words: " + Global.TakaWords(_nTotalAmount * _oSampleInvoice.ConversionRate), 0, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                }
                else
                {
                    oPdfPTable.AddCell(this.SetCellValue("Amount In Words: " + Global.DollarWords(_nTotalAmount), 0, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                }
                #endregion
            }
          
            if (!string.IsNullOrEmpty(_oSampleInvoice.Remark))
            {
                oPdfPTable.AddCell(this.SetCellValue("Remark: " + Global.DollarWords(_nTotalAmount), 0, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f)); 
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
                this.PrintWaterMark(30f, 30f, 30f, 30f);
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
                    _oPdfPCell = new PdfPCell(new Phrase("Waiting For Payment Settle", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC)));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Unauthorised Invoice", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC)));
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, 1);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, 4);

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 120, 140f, 80f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            oPdfPTable.AddCell(this.SetCellValue("DATE: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue(_oSampleInvoice.SampleInvoiceDateST, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));
            oPdfPTable.AddCell(this.SetCellValue("SAMPLE ADJUSTMENT NOTE NO: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue(_oSampleInvoice.InvoiceNo, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));
            oPdfPTable.AddCell(this.SetCellValue("BUYER NAME: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            oPdfPTable.AddCell(this.SetCellValue(_oSampleInvoice.ContractorName, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            oPdfPTable.AddCell(this.SetCellValue("P/I No: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue(_oExportSCDO.PINo_Full, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));
            oPdfPTable.AddCell(this.SetCellValue("L/C No: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue(_oExportSCDO.ELCNo, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));

            oPdfPTable.AddCell(this.SetCellValue("CONCERN PERSON: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
            oPdfPTable.AddCell(this.SetCellValue(_oSampleInvoice.MKTPName, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10));
           
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
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                oPdfPTable.AddCell(this.SetCellValue("SL#", 0, 8, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("SAMPLE NO", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("YARN TYPE", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("Qty(LBS)", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("U/P($)", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("Amount($)", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));

                if (_oSampleInvoice.ConversionRate <= 1)
                {
                    oPdfPTable.AddCell(this.SetCellValue("REMARKS", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                }
                else
                {
                    oPdfPTable.AddCell(this.SetCellValue("Amount(" + _oSampleInvoice.ExchangeCurrencySymbol + ")", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                }

                foreach (SampleInvoiceDetail oItem in _oSampleInvoice.SampleInvoiceDetails)
                {
                    _nCount++;

                    #region PrintDetail

                    oPdfPTable.AddCell(this.SetCellValue(_nCount.ToString(), 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(oItem.OrderNo, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(oItem.ProductName, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oItem.Qty), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oItem.UnitPrice), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oItem.Qty * oItem.UnitPrice), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));

                    if (_oSampleInvoice.ConversionRate <= 1)
                    {
                        oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    }
                    else
                    {
                        oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(oItem.Qty * oItem.UnitPrice * _oSampleInvoice.ConversionRate), 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 30f));
                    }


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
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                oPdfPTable.AddCell(this.SetCellValue("Total: ", 0, 3, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue("$" + Global.MillionFormat(_nTotalAmount), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_nTotalAmount * _oSampleInvoice.ConversionRate), 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.GRAY, 1, 10f));
                #endregion
                #region Total in World
                if (_oSampleInvoice.ConversionRate > 1)
                {
                    oPdfPTable.AddCell(this.SetCellValue("Amount In Words: " + Global.TakaWords(_nTotalAmount * _oSampleInvoice.ConversionRate), 0, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                }
                else
                {
                    oPdfPTable.AddCell(this.SetCellValue("Amount In Words: " + Global.DollarWords(_nTotalAmount), 0, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
                }
                #endregion
            }
            if (!string.IsNullOrEmpty(_oSampleInvoice.Remark))
            {
                oPdfPTable.AddCell(this.SetCellValue("Remark: " + Global.DollarWords(_nTotalAmount), 0, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 10f));
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
        private PdfPTable DyeingBillTable_F2()
        {
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
            return oPdfPTable;
        }
        public byte[] PrepareReport_DyeingBill(SampleInvoice oSampleInvoice, Company oCompany, BusinessUnit oBusinessUnit, List<DyeingOrderReport> oDyeingOrderReports, SampleInvoiceSetup oSampleInvoiceSetup, int nTitleType, int nPageHeight)
        {
            _oSampleInvoice = oSampleInvoice;
            _oDyeingOrderReports = oDyeingOrderReports;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oSampleInvoiceSetup = oSampleInvoiceSetup;
            _nTitleType = nTitleType;
            _PageHeight = nPageHeight;
            #region Page Setup
            //_oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595

            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(PageSize.A4);

            _oDocument.SetMargins(36f, 10f, 10f, 40f);
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
            this.PrintWaterMark(37f, 10f, 10f, 40f);
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
            oPdfPTable.SetWidths(new float[] { 60f, 180f, 70f, 130f });

            //// 1st Row
            ESimSolPdfHelper.FontStyle = _oFontStyle;
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "BUYER NAME", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            
            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSampleInvoice.ContractorName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);

            ESimSolPdfHelper.FontStyle = _oFontStyle;
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Bill No", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);

            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSampleInvoiceSetup.Code + "-" + _oSampleInvoice.InvoiceNo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
             
            /// 2nd Row
            ESimSolPdfHelper.FontStyle = _oFontStyle;
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Concern Person", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " " + _oSampleInvoice.ContractorPersopnnalName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "PI No", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSampleInvoice.ExportPINo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);

            //// 3nd Row 
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Delivery Place", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 3, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSampleInvoice.ContractorAddress, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 3, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Date", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSampleInvoice.SampleInvoiceDateST, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);

            //// 4th Row
            //// Use Rowspan
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Payment Term", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSampleInvoice.PaymentTypeSt, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
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
        private void Print_ColumnHeader(bool bIsColSpan)
        {
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable = DyeingBillTable_F2();
            #endregion

            #region Header Row
            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Order No", Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);

            if (bIsColSpan)
            {
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Buyer Ref", Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 10);
            }
            else 
            {
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Buyer Ref", Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Merchandiser", Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            }
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Color", Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Yarn Type", Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Shade", Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Qty " + "(" + _sMunit + ")", Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "U.P.", Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Value", Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, 1);
            #endregion
        }
        private bool IsColSpan(List<DyeingOrderReport> oDyeingOrderReports)
        {
            foreach (var oItem in oDyeingOrderReports) {
                if (!oItem.CPName.Equals(_oSampleInvoice.ContractorPersopnnalName))
                    return false;
            }
            return true;
        }

        private void PrintBody_DyeingBill(List<DyeingOrderReport> oDyeingOrderReports)
        {
            //_oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable = DyeingBillTable_F2();
            #endregion

            #region Header Name
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable,oDyeingOrderReports[0].OrderTypeSt, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, 9, 0);
            #endregion

            #region Insert Into Main Table
            if (CalculatePdfPTableHeight(_oPdfPTable) > _PageHeight)
            {
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
                this.PrintHeader();
                //this.ReporttHeader();
                nUsagesHeight = 0;
            }
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, 9);
            #endregion

            #region Initialize Table
            oPdfPTable = new PdfPTable(9);
            oPdfPTable = DyeingBillTable_F2();
            #endregion

            int nDyeingOrderID = 0; int nCount = 0; 
            double nQty=0,nAmount=0, nTotalAmount = 0, nTotalQty = 0;
            
            foreach (DyeingOrderReport oItem in oDyeingOrderReports)
            {
                bool bIsColSpan = this.IsColSpan(oDyeingOrderReports.Where(x=>x.ProductID==oItem.ProductID).ToList());

                if(nCount==0)
                    this.Print_ColumnHeader(bIsColSpan);

                if (nDyeingOrderID != oItem.DyeingOrderID)
                {
                    #region Product Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(9);
                        oPdfPTable = DyeingBillTable_F2();
                        #endregion

                        #region Order Wise Sub Total
                        ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                        //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 4, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total :", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + "" + Global.MillionFormat(nAmount), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table

                        if (CalculatePdfPTableHeight(_oPdfPTable) > _PageHeight)
                        {
                            _oDocument.Add(_oPdfPTable);
                            _oDocument.NewPage();
                            _oPdfPTable.DeleteBodyRows();

                            this.PrintHeader();
                            this.ReporttHeader();
                            this.Print_ColumnHeader(bIsColSpan);
                           
                            nUsagesHeight = 0;
                            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                        }
                        nCount = 0; nQty = 0; nAmount = 0;

                        ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, 1);
                        #endregion
                    }
                    #endregion

                    #region Header
                    //this.Print_ColumnHeader();
                    #endregion
                }

                #region Initialize Table
                oPdfPTable = new PdfPTable(9);
                oPdfPTable = DyeingBillTable_F2();
                #endregion

                nCount++;

                ESimSolPdfHelper.FontStyle = _oFontStyle;
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.OrderNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                if (bIsColSpan)
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (!String.IsNullOrEmpty(oItem.StyleNo) ? oItem.BuyerRef + ", " + oItem.StyleNo : oItem.BuyerRef), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 10);
                }
                else
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (!String.IsNullOrEmpty(oItem.StyleNo) ? oItem.BuyerRef + ", " + oItem.StyleNo : oItem.BuyerRef), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.CPName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                }
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.ColorName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                ESimSolPdfHelper.FontStyle = _oFontStyle;

                if (oItem.RGB == "" || oItem.RGB == null)
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.ColorNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                }
                else
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.RGB, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                }
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + "" + System.Math.Round(oItem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + "" + Global.MillionFormatActualDigit(oItem.Qty * oItem.UnitPrice), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
         
                nQty += oItem.Qty;
                nAmount += oItem.Qty * oItem.UnitPrice;

                #region Insert Into Main Table

                if (CalculatePdfPTableHeight(_oPdfPTable) > _PageHeight)
                {
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                    this.PrintHeader();
                    this.ReporttHeader();
                    this.Print_ColumnHeader(bIsColSpan);

                    nUsagesHeight = 0;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                    _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                }

                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, 1);
                #endregion

                nDyeingOrderID = oItem.DyeingOrderID;
                nTotalQty = nTotalQty + oItem.Qty;
                nTotalAmount += (oItem.Qty * oItem.UnitPrice);
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(9);
            oPdfPTable = DyeingBillTable_F2();
            #endregion

            #region Order Wise Sub Total
            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 4, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total :", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + "" + Global.MillionFormat(nAmount), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 4, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " Total :", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(Math.Round(nTotalQty, 2)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + "" + Global.MillionFormat(nTotalAmount), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, 1);
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
                                                    65f, //Color
                                                    90f,  //Type of count                                          
                                                    28f,  //Color Category                                              
                                                    43f,  //Qty
                                                    28f,  //U/P
                                                    43f,  //Value
                                                   
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
                                                    65f, //Color
                                                    90f,  //Type of count                                          
                                                    28f,  //Color Category                                              
                                                    43f,  //Qty
                                                    28f,  //U/P
                                                    43f,  //Value
                                                   
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

                        _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + "" + Global.MillionFormat(nAmount), _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol+ "" + System.Math.Round(oItem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                //_oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + "" + Global.MillionFormatActualDigit(oItem.UnitPrice), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; 
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + "" + Global.MillionFormatActualDigit(oItem.Qty * oItem.UnitPrice), _oFontStyle));
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

                //nUsagesHeight = CalculatePdfPTableHeight(oPdfPTable);

                //if (nUsagesHeight > _PageHeight)
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
                                                    65f, //Color
                                                    90f,  //Type of count                                          
                                                    28f,  //Color Category                                              
                                                    43f,  //Qty
                                                    28f,  //U/P
                                                    43f,  //Value
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

            _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + "" + Global.MillionFormat(nAmount), _oFontStyleBold));
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

            _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + "" + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
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

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
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
            if (_oSampleInvoice.Charge > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Add Charge", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + " " + _sMunit, _oFontStyle));
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

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + " " + _sMunit, _oFontStyle));
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
                _nTotalAmount = _nTotalAmount - _oSampleInvoice.Discount + _oSampleInvoice.Charge;
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
            oPdfPCell.FixedHeight = 10f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10f;
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


            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
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
        private void AddTable(ref PdfPTable oMainTable, PdfPTable oPdfPTable) 
        {
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oMainTable.AddCell(_oPdfPCell);
            oMainTable.CompleteRow();
        }
        private PdfPCell SetCellValue(string sName, int nRowSpan, int nColumnSpan, int halign, int valign, BaseColor color, int border, float height)
        {
            PdfPCell oPdfPCell = new PdfPCell(new Phrase(sName, _oFontStyle));
            oPdfPCell.Rowspan = (nRowSpan > 0) ? nRowSpan : 1;
            oPdfPCell.Colspan = (nColumnSpan > 0) ? nColumnSpan : 1;
            oPdfPCell.HorizontalAlignment = halign;
            oPdfPCell.VerticalAlignment = valign;
            oPdfPCell.BackgroundColor = color;
            if (border == 0)
                oPdfPCell.Border = 0;
            if (height > 0)
                oPdfPCell.MinimumHeight = height;
            //oPdfPCell.BorderWidth = iTextSharp.text.;
            return oPdfPCell;
        }
        private PdfPCell SetCellValue(string sName, int nRowSpan, int nColumnSpan, int halign, int valign, BaseColor color, int border, float height, string[] BorderWidth)
        {
            PdfPCell oPdfPCell = new PdfPCell(new Phrase(sName, _oFontStyle));
            oPdfPCell.Rowspan = (nRowSpan > 0) ? nRowSpan : 1;
            oPdfPCell.Colspan = (nColumnSpan > 0) ? nColumnSpan : 1;
            oPdfPCell.HorizontalAlignment = halign;
            oPdfPCell.VerticalAlignment = valign;
            oPdfPCell.BackgroundColor = color;
            if (border == 0)
                oPdfPCell.Border = 0;
            if (height > 0)
                oPdfPCell.MinimumHeight = height;

            foreach (string oItem in BorderWidth)
            {
                switch (oItem.ToUpper())
                {
                    case "TOP": oPdfPCell.BorderWidthTop = 1; break;
                    case "BOTTOM": oPdfPCell.BorderWidthBottom = 1; break;
                    case "LEFT": oPdfPCell.BorderWidthLeft = 1; break;
                    case "RIGHT": oPdfPCell.BorderWidthRight = 1; break;
                }
            }

            return oPdfPCell;
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
            
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
            PdfContentByte cb = PDFWriter.DirectContent;
            _oDocument.NewPage();
        }
    }
}
