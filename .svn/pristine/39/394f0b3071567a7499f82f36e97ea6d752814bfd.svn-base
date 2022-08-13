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
    public class rptSampleRequestGatePass
    {
        #region Declaration
        //int _nTotalColumn = 8;
        float _nfixedHight = 5f;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle2;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        //PdfPTable _oPdfPTableDetail = new PdfPTable(5);
        PdfPCell _oPdfPCell;
        PdfPCell _oPdfPCellDetail;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        SampleRequest _oSampleRequest = new SampleRequest();
        SampleRequestDetail _oSampleRequestDetail = new SampleRequestDetail();
        List<SampleRequest> _oSampleRequests = new List<SampleRequest>();
        List<SampleRequestDetail> _oSampleRequestDetails = new List<SampleRequestDetail>();
        Company _oCompany = new Company();
        #endregion
        public byte[] PrepareReport(SampleRequest oSampleRequest, Company oCompany)
        {
            _oSampleRequest = oSampleRequest;
            _oSampleRequestDetails = oSampleRequest.SampleRequestDetails;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A5);//420*595
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
            this.PrintBody();
            this.PrintFooter();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintHeader()
        {
            #region CompanyHeader
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 75f, 310.5f, 75f });

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
            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
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
            _oPdfPCell = new PdfPCell(new Phrase("Gate Pass", FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            this.PrintSampleHeader();
        }

        private void PrintSampleHeader()
        {
            PdfPTable SampleHeaderPdfPTable = new PdfPTable(8);
            SampleHeaderPdfPTable.WidthPercentage = 100;
            SampleHeaderPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            SampleHeaderPdfPTable.SetWidths(new float[] { 73f, 74f, 73f, 74f, 73f, 74f, 73f, 74f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle2 = FontFactory.GetFont("Tahoma", 8f, 0);

            _oPdfPCell = new PdfPCell(new Phrase("Request No: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oSampleRequest.RequestNo, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Request Date: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oSampleRequest.RequestDateInString, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Type: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oSampleRequest.RequestTypeInString, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Request By: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oSampleRequest.RequestByName, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);

            SampleHeaderPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Request To: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oSampleRequest.RequestToName, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Party: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oSampleRequest.ContractorName, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Contact Person: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oSampleRequest.ContactPersonName, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);

            SampleHeaderPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Remarks: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oSampleRequest.Remarks, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 7; _oPdfPCell.BackgroundColor = BaseColor.WHITE; SampleHeaderPdfPTable.AddCell(_oPdfPCell);

            SampleHeaderPdfPTable.CompleteRow();

            #region Insert into Main Table
            _oPdfPCell = new PdfPCell(SampleHeaderPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 6; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        private void PrintBody()
        {
            int _nTotalQty = 0;
            PdfPTable oPdfPTable = new PdfPTable(5);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] {
                                        30f,  //SL
                                        100f, //Item
                                        
                                         60f, //color
                                        160f, //Qty
                                       
                                        70f//Note
            });
            int nProductID = 0;
            _nTotalQty = 0;
            int _nCount = 0;
            double nTableHeight = 0;
            if (_oSampleRequestDetails.Count > 0)
            {
                _oSampleRequestDetails = _oSampleRequestDetails.OrderBy(o => o.ProductID).ToList();
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyle));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Items", _oFontStyle));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);



                oPdfPCell = new PdfPCell(new Phrase("Color No", _oFontStyle));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Measurment & Qty", _oFontStyle));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                foreach (SampleRequestDetail oItem in _oSampleRequestDetails)
                {
                    _nCount++;
                    #region printdetails
                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);



                    oPdfPCell = new PdfPCell(new Phrase("Measurement Unit :" + oItem.MUnitName + "\n Qty :" + oItem.Quantity.ToString("#,###.##"), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Remarks, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();
                    _nTotalQty = _nTotalQty + (int)oItem.Quantity;
                    #endregion
                   
                }
                nTableHeight += CalculatePdfPTableHeight(oPdfPTable);
                _nfixedHight = 250 - (float)nTableHeight;
                if (_nfixedHight > 0)
                {
                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();

                    #region Total
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyle));
                    _oPdfPCell.Colspan = 3;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_nTotalQty.ToString("#,###.##"), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion


                    #region Total in Word
                    string sTemp = "";
                    sTemp = Global.DollarWords((float)_nTotalQty);
                    if (!String.IsNullOrEmpty(sTemp))
                    {
                        sTemp = sTemp.Replace("Dollar", "");
                        sTemp = sTemp.Replace("Only", "");
                        sTemp = sTemp.ToUpper();
                    }
                    _oPdfPCell = new PdfPCell(new Phrase("IN WORDS: " + sTemp + " ONLY", _oFontStyle));
                    _oPdfPCell.Colspan = 5;

                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                    #region ACKNOWLEDGEMENT
                    _oPdfPCell = new PdfPCell(new Phrase("GOODS RECEIVED IN GOOD ORDER, GOOD CONDITION & MENTIONED QUANTITY", _oFontStyle));
                    _oPdfPCell.Colspan = 5;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                }
                #region push into main table
                _oPdfPCell = new PdfPCell(oPdfPTable);

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;

                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 6; _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
        }
        private void PrintFooter()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 197f, 197f, 198f });

            oPdfPCell = new PdfPCell(new Phrase("RECEIVED BY", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("FACTORY MANAGER", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPCell = new PdfPCell(new Phrase("DELIVERY INCHARGE", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 9f, 0)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 35f;
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
    }
}
