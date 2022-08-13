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
    public class rptSampleOutStandingMkt
    {
        #region Declaration
        PdfWriter _oWriter;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        iTextSharp.text.Font _oFontStyleBoldUnderLine;
    
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<SampleOutStanding> _oSampleOutStandings_Dr = new List<SampleOutStanding>();
        List<SampleOutStanding> _oSampleOutStandings_Cr = new List<SampleOutStanding>();
        List<SampleOutStanding> _oSampleOutStandings_Cr_SampleOrder = new List<SampleOutStanding>();
        List<SampleOutStanding> _oSampleOutStandings = new List<SampleOutStanding>();
        List<PaymentDetail> _oPaymentDetails = new List<PaymentDetail>();
        Company _oCompany = new Company();
        
        #endregion

        public byte[] PrepareReport(List<SampleOutStanding> oSampleOutStandings, List<SampleOutStanding> oSampleOutStandings_Dr, List<SampleOutStanding> oSampleOutStandings_Cr, List<SampleOutStanding> oSampleOutStandings_Cr_SampleOrder, List<PaymentDetail> oPaymentDetails, Company oCompany, string sDateRange)
        {
            _oSampleOutStandings_Dr = oSampleOutStandings_Dr;
            _oSampleOutStandings_Cr = oSampleOutStandings_Cr;
            _oSampleOutStandings_Cr_SampleOrder = oSampleOutStandings_Cr_SampleOrder;
            _oSampleOutStandings = oSampleOutStandings;
            _oPaymentDetails = oPaymentDetails;

         
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(10f, 10f, 5f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 802 });
            #endregion

            #region Report Body & Header

            this.PrintHeader("SAMPLE ORDER vs ADJUSTMENT", sDateRange);
            this.PrintBodyDr();
            if (_oSampleOutStandings_Cr.Count > 0 || _oSampleOutStandings_Cr_SampleOrder.Count > 0)
            {
                this.PrintBodyCr();
            }
            this.PrintBody_PaymentCR();
            //this.PrintBodyCr_Two();
            //this.PrintBodySampleOrder_CR();
            //this.PrintBody_PaymentCR();
            PrintBalance();
            SetSummaryPartyWise();
            _oPdfPTable.HeaderRows = 3;
            #endregion

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

   
        #region Report Header
        private void PrintHeader(string sReportHeader, string sDateRange)
        {
            #region Company & Report Header
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 450f, 250f });

            #region Company Name & Report Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(95f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase(sReportHeader, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Address & Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(sDateRange, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Phone Number
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }
        #endregion

        #region Report Body
    
        private void PrintBodyDr()
        {
            string sMunit="LBS";
            string sCurrency="$";

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            if(_oSampleOutStandings_Dr.Count>0)
            {
                _oSampleOutStandings_Dr = _oSampleOutStandings_Dr.OrderBy(o => o.ContractorName).ToList();
            }

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                      //30f,  //SL 
                                                    55f,  //Date
                                                    70f,  //Sample No
                                                   195f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    60f,  //Value
                                                    125f,  //Contact Person
                                                    80f,  //Payment                                                    
                                                    80f,  //Com
                                                   
                                             });
            #endregion
            #region For Sample Part
            _oPdfPCell = new PdfPCell(new Phrase("SAMPLE ORDER DETAILS:", FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            string sContractorName = ""; int nCount = 0; 
            double nTotalAmount = 0, nTotalQty = 0, nGrandTotalQty = 0, nGrandTotalAmount = 0;
            foreach (SampleOutStanding oItem in _oSampleOutStandings_Dr)
            {
                if (sContractorName != oItem.ContractorName)
                {
                    #region Supplier Wise Sub Total
                    if (sContractorName!="")
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(9);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                    55f,  //Date
                                                    70f,  //Sample No
                                                   195f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    60f,  //Value
                                                    125f,  //Contact Person
                                                    80f,  //Payment                                                    
                                                    80f,  //Com
                                             });
                        #endregion

                        #region Date Wise Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Party Wise Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat(nTotalQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0; nTotalQty = 0;
                        #region Blank Row
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion
                    }
                    #endregion

                    #region Header
                  

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(9);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                      55f,  //Date
                                                    70f,  //Sample No
                                                   195f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    60f,  //Value
                                                    125f,  //Contact Person
                                                    80f,  //Payment                                                    
                                                    80f,  //Com
                                                   
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oItem.ContractorName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 9; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    //_oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Sample No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty("+sMunit+")", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                   

                    _oPdfPCell = new PdfPCell(new Phrase("U/P("+sCurrency+')', _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                  

                    _oPdfPCell = new PdfPCell(new Phrase("Value("+sCurrency+')', _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                  


                    _oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Payment Mode", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Comments", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    
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
                                                   55f,  //Date
                                                    70f,  //Sample No
                                                   195f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    60f,  //Value
                                                    125f,  //Contact Person
                                                    80f,  //Payment                                                    
                                                    80f,  //Com
                                             });
                #endregion

                nCount++;
                //_oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderDate.ToString("dd MMM yyyy"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNoFull, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PaymentTypeSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            
                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sContractorName = oItem.ContractorName;
                //sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nTotalQty = nTotalQty + oItem.Qty;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
                nGrandTotalQty = nGrandTotalQty + oItem.Qty;
            }
            if (_oSampleOutStandings_Dr.Count > 0)
            {
                #region Initialize Table
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                   55f,  //Date
                                                    70f,  //Sample No
                                                   195f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    60f,  //Value
                                                    125f,  //Contact Person
                                                    80f,  //Payment                                                    
                                                    80f,  //Com
                                             });
                #endregion

                #region Date Wise Sub Total
                _oPdfPCell = new PdfPCell(new Phrase("Party Wise Sub Total :", _oFontStyleBold));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalQty), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat(nGrandTotalQty), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion
            }
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void PrintBodyCr()
        {
            string sMunit = "LBS";
            string sCurrency = "$";

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            if (_oSampleOutStandings_Cr.Count > 0)
            {
                _oSampleOutStandings_Cr = _oSampleOutStandings_Cr.OrderBy(o => o.ContractorName).ToList();
            }

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    50f,  //Date
                                                    75f,  //Sample No
                                                   180f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    57f,  //Value
                                                    80f,  //PI No
                                                    80f,  //L/C no                                                    
                                                    100f,  //CPerson
                                                    80f,  //Ad mode
                                                   
                                             });
            #endregion
            #region For Sample Part
            _oPdfPCell = new PdfPCell(new Phrase("SAMPLE ADJUSTMENT DETAILS:", FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.Colspan = 11; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            string sContractorName = ""; int nCount = 0;
            double nTotalAmount = 0, nTotalQty = 0, nGrandTotalQty = 0, nGrandTotalAmount = 0;
            #region Header

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(10);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    55f,  //Date
                                                    70f,  //Sample No
                                                   180f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    57f,  //Value
                                                    80f,  //PI No
                                                    80f,  //L/C no                                                    
                                                    100f,  //CPerson
                                                    80f,  //Ad mode
                                                   
                                             });
                    #endregion


                    #region Header Row
                    //_oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("SAN No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty(" + sMunit + ")", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("U/P(" + sCurrency + ')', _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Value(" + sCurrency + ')', _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);



                    _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Party Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Adjustment Mode", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
            foreach (SampleOutStanding oItem in _oSampleOutStandings_Cr)
            {
                //}
                #region Initialize Table
                oPdfPTable = new PdfPTable(10);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                        55f,  //Date
                                                    70f,  //Sample No
                                                   180f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    57f,  //Value
                                                    80f,  //PI No
                                                    80f,  //L/C no                                                    
                                                    100f,  //CPerson
                                                    80f,  //Ad mode
                                             });
                #endregion

                nCount++;
                //_oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SampleInvoiceDate.ToString("dd MMM yyyy"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SampleInvoiceNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.InvoiceTypeSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sContractorName = oItem.ContractorName;
                //sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nTotalQty = nTotalQty + oItem.Qty;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
                nGrandTotalQty = nGrandTotalQty + oItem.Qty;
            }

            foreach (SampleOutStanding oItem in _oSampleOutStandings_Cr_SampleOrder)
            {
                //}
                #region Initialize Table
                oPdfPTable = new PdfPTable(10);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                        55f,  //Date
                                                    70f,  //Sample No
                                                   180f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    57f,  //Value
                                                    80f,  //PI No
                                                    80f,  //L/C no                                                    
                                                    100f,  //CPerson
                                                    80f,  //Ad mode
                                             });
                #endregion

                nCount++;
                //_oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderDate.ToString("dd MMM yyyy"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNoFull, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount / oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.InvoiceTypeSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                //sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nTotalQty = nTotalQty + oItem.Qty;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
                nGrandTotalQty = nGrandTotalQty + oItem.Qty;
            }

           
            #region Initialize Table
            oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                     55f,  //Date
                                                    70f,  //Sample No
                                                   180f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    57f,  //Value
                                                    80f,  //PI No
                                                    80f,  //L/C no                                                    
                                                    100f,  //CPerson
                                                    80f,  //Ad mode
                                             });
            #endregion
           
            #region Date Wise Sub Total
                _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyleBold));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat(nTotalQty), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

          

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void PrintBodyCr_Two()
        {
            string sMunit = "LBS";
            string sCurrency = "$";

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            if (_oSampleOutStandings_Cr.Count > 0)
            {
                _oSampleOutStandings_Cr = _oSampleOutStandings_Cr.OrderBy(o => o.ContractorName).ToList();
            }

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    50f,  //Date
                                                    75f,  //Sample No
                                                   180f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    57f,  //Value
                                                    80f,  //PI No
                                                    80f,  //L/C no                                                    
                                                    100f,  //CPerson
                                                    80f,  //Ad mode
                                                   
                                             });
            #endregion
            #region For Sample Part
            _oPdfPCell = new PdfPCell(new Phrase("SAMPLE ADJUSTMENT DETAILS:", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.Colspan = 11; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            string sContractorName = ""; int nCount = 0;
            double nTotalAmount = 0, nTotalQty = 0, nGrandTotalQty = 0, nGrandTotalAmount = 0;
            foreach (SampleOutStanding oItem in _oSampleOutStandings_Cr)
            {
                if (sContractorName != oItem.ContractorName)
                {
                    #region Supplier Wise Sub Total
                    if (sContractorName != "")
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(10);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                    50f,  //Date
                                                    75f,  //Sample No
                                                   180f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    57f,  //Value
                                                    80f,  //PI No
                                                    80f,  //L/C no                                                    
                                                    100f,  //CPerson
                                                    80f,  //Ad mode
                                             });
                        #endregion

                        #region Date Wise Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Party Wise Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat(nTotalQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0; nTotalQty = 0;
                        #region Blank Row
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion
                    }
                    #endregion

                    #region Header
                 

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(10);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    50f,  //Date
                                                    75f,  //Sample No
                                                   180f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    57f,  //Value
                                                    80f,  //PI No
                                                    80f,  //L/C no                                                    
                                                    100f,  //CPerson
                                                    80f,  //Ad mode
                                                   
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oItem.ContractorName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 10; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    //_oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("SAN No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty(" + sMunit + ")", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("U/P(" + sCurrency + ')', _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Value(" + sCurrency + ')', _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);



                    _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Adjustment Mode", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

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
                oPdfPTable = new PdfPTable(10);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                      50f,  //Date
                                                    75f,  //Sample No
                                                   180f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    57f,  //Value
                                                    80f,  //PI No
                                                    80f,  //L/C no                                                    
                                                    100f,  //CPerson
                                                    80f,  //Ad mode
                                             });
                #endregion

                nCount++;
                //_oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SampleInvoiceDate.ToString("dd MMM yyyy"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SampleInvoiceNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.InvoiceTypeSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sContractorName = oItem.ContractorName;
                //sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nTotalQty = nTotalQty + oItem.Qty;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
                nGrandTotalQty = nGrandTotalQty + oItem.Qty;
            }
          
                #region Initialize Table
                oPdfPTable = new PdfPTable(10);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                    50f,  //Date
                                                    75f,  //Sample No
                                                   180f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    57f,  //Value
                                                    80f,  //PI No
                                                    80f,  //L/C no                                                    
                                                    100f,  //CPerson
                                                    80f,  //Ad mode
                                             });
                #endregion
           if (_oSampleOutStandings_Cr.Count > 0)
           {
                #region Date Wise Sub Total
                _oPdfPCell = new PdfPCell(new Phrase("Party Wise Sub Total :", _oFontStyleBold));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat(nTotalQty), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat(nGrandTotalQty), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion
            }
            else
            {
                #region Date Wise Sub Total
                _oPdfPCell = new PdfPCell(new Phrase("There have no Sample Adjustment in this date range for the selected Buyer(s)", _oFontStyleBold));
                _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion
            }

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void PrintBodySampleOrder_CR()
        {
            string sMunit = "LBS";
            string sCurrency = "$";

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            if (_oSampleOutStandings_Cr_SampleOrder.Count > 0)
            {
                _oSampleOutStandings_Cr_SampleOrder = _oSampleOutStandings_Cr_SampleOrder.OrderBy(o => o.ContractorName).ToList();
            }

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                      //30f,  //SL 
                                                    50f,  //Date
                                                    75f,  //Sample No
                                                   195f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    60f,  //Value
                                                    125f,  //Contact Person
                                                    80f,  //Payment                                                    
                                                    80f,  //Com
                                                   
                                             });
            #endregion
            #region For Sample Part
            _oPdfPCell = new PdfPCell(new Phrase("SAMPLE ORDER (Adjusted with L/C):", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            string sContractorName = ""; int nCount = 0;
            double nTotalAmount = 0, nTotalQty = 0, nGrandTotalQty = 0, nGrandTotalAmount = 0;
            foreach (SampleOutStanding oItem in _oSampleOutStandings_Cr_SampleOrder)
            {
                if (sContractorName != oItem.ContractorName)
                {
                    #region Supplier Wise Sub Total
                    if (sContractorName != "")
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(9);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                    50f,  //Date
                                                    75f,  //Sample No
                                                   195f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    60f,  //Value
                                                    125f,  //Contact Person
                                                    80f,  //Payment                                                    
                                                    80f,  //Com
                                             });
                        #endregion

                        #region Date Wise Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Party Wise Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0; nTotalQty = 0;
                        #region Blank Row
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion
                    }
                    #endregion

                    #region Header


                    #region Table Initialize
                    oPdfPTable = new PdfPTable(9);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    50f,  //Date
                                                    75f,  //Sample No
                                                   195f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    60f,  //Value
                                                    125f,  //Contact Person
                                                    80f,  //Payment                                                    
                                                    80f,  //Com
                                                   
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oItem.ContractorName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 9; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    //_oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Sample No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty(" + sMunit + ")", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("U/P(" + sCurrency + ')', _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Value(" + sCurrency + ')', _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);



                    _oPdfPCell = new PdfPCell(new Phrase("Contact Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Payment Mode", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Comments", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

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
                                                  50f,  //Date
                                                    75f,  //Sample No
                                                   195f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    60f,  //Value
                                                    125f,  //Contact Person
                                                    80f,  //Payment                                                    
                                                    80f,  //Com
                                             });
                #endregion

                nCount++;
                //_oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderDate.ToString("dd MMM yyyy"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNoFull, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PaymentTypeSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sContractorName = oItem.ContractorName;
                //sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nTotalQty = nTotalQty + oItem.Qty;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
                nGrandTotalQty = nGrandTotalQty + oItem.Qty;
            }
            if (_oSampleOutStandings_Cr_SampleOrder.Count > 0)
            {
                #region Initialize Table
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                  50f,  //Date
                                                    75f,  //Sample No
                                                   195f, //Yarn Type
                                                    50f,  //Qty                                                  
                                                    50f,  //U/P                                                   
                                                    60f,  //Value
                                                    125f,  //Contact Person
                                                    80f,  //Payment                                                    
                                                    80f,  //Com
                                             });
                #endregion

                #region Date Wise Sub Total
                _oPdfPCell = new PdfPCell(new Phrase("Party Wise Sub Total :", _oFontStyleBold));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalQty), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nGrandTotalQty), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion
            }
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void PrintBody_PaymentCR()
        {
            string sMunit = "LBS";
            string sCurrency = "$";

          

            if (_oPaymentDetails.Count > 0)
            {
                _oPaymentDetails = _oPaymentDetails.OrderBy(o => o.ContractorName).ToList();
            }

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                      //30f,  //SL 
                                                   50f,  //Date
                                                   75f,  //MR No
                                                   100f, //Value
                                                   80f,  //Value $  
                                                   100f,  //Payment Mode                                               
                                                   95f,  //Doc No                                                  
                                                   60f,  //Doc Date
                                                   125f,  //Bank Name
                                                   80f,  //Comments
                                                   
                                             });
            #endregion
            #region For Sample Part
            _oPdfPCell = new PdfPCell(new Phrase("Payment Received:", FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            string sContractorName = ""; int nCount = 0;
            double nTotalAmount = 0, nTotalQty = 0, nGrandTotalQty = 0, nGrandTotalAmount = 0;
            foreach (PaymentDetail oItem in _oPaymentDetails)
            {
                if (sContractorName != oItem.ContractorName)
                {
                    #region Supplier Wise Sub Total
                    if (sContractorName != "")
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(7);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                   70f,  //Date
                                                   80f,  //MR No
                                                   70f, //Value $
                                                   70f,  //Value TK 
                                                   100f,  //Invoice No                                               
                                                   100f,  //Invoice Date                                                
                                                   60f,  //Note
                                                  
                                             });
                        #endregion

                        #region Date Wise Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Party Wise Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0; nTotalQty = 0;
                        #region Blank Row
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion
                    }
                    #endregion

                    #region Header


                    #region Table Initialize
                    oPdfPTable = new PdfPTable(7);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                   70f,  //Date
                                                   80f,  //MR No
                                                   70f, //Value $
                                                   70f,  //Value TK 
                                                   100f,  //Invoice No                                               
                                                   100f,  //Invoice Date                                                
                                                   60f,  //Note
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oItem.ContractorName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 7; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    //_oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("MR Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("MR No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Value(" + sCurrency + ")", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Value(BDT)", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Invoice No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Invoice Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Note", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

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
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                   70f,  //Date
                                                   80f,  //MR No
                                                   70f, //Value $
                                                   70f,  //Value TK 
                                                   100f,  //Invoice No                                               
                                                   100f,  //Invoice Date                                                
                                                   60f,  //Note

                                             });
                #endregion

                nCount++;
                //_oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MRDate.ToString("dd MMM yyyy"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MRNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CurrencySymbol + " " + (oItem.PaymentAmount + oItem.DisCount)/oItem.CRate, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExchangeCurrencySymbol + " " + (oItem.PaymentAmount + oItem.DisCount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SampleInvoiceNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SampleInvoiceDateInString, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
              
                _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sContractorName = oItem.ContractorName;
                //sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + (oItem.PaymentAmount + oItem.DisCount) / oItem.CRate;
                //nTotalQty = nTotalQty + oItem.Qty;
                nGrandTotalAmount = nGrandTotalAmount + (oItem.PaymentAmount + oItem.DisCount)/oItem.CRate;
                //nGrandTotalQty = nGrandTotalQty + oItem.Qty;
            }
            if (_oPaymentDetails.Count > 0)
            {
                #region Initialize Table
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                    70f,  //Date
                                                   80f,  //MR No
                                                   70f, //Value $
                                                   70f,  //Value TK 
                                                   100f,  //Invoice No                                               
                                                   100f,  //Invoice Date                                                
                                                   60f,  //Note
                                             });
                #endregion

                #region Date Wise Sub Total
                _oPdfPCell = new PdfPCell(new Phrase("Party Wise Sub Total :", _oFontStyleBold));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
        }
        private void PrintBalance()
        {
            string sMunit = "LBS";
            string sCurrency = "$";
            double nValue = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                   300f,  
                                                   160f,  
                                                   120f,
                                                   300f,  
                                             });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Previous Due", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            nValue = _oSampleOutStandings.Select(c => c.Opening).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(sCurrency + "" + Global.MillionFormat(nValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

          
            _oPdfPCell = new PdfPCell(new Phrase("Total Due", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            nValue = _oSampleOutStandings.Select(c => c.Debit).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(sCurrency + "" + Global.MillionFormat(nValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
           
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        
             _oPdfPCell = new PdfPCell(new Phrase("Total Adjustment", _oFontStyleBold));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            nValue = _oSampleOutStandings.Select(c => c.Credit).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(sCurrency + "" + Global.MillionFormat(nValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        
            _oPdfPCell = new PdfPCell(new Phrase("Balance", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            nValue = _oSampleOutStandings.Select(c => c.Closing).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(sCurrency+" "+ Global.MillionFormat(nValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
          
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
           
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #region
        private void SetSummaryPartyWise()
        {

            //_oSampleOutStandings_Dr = oSampleOutStandings_Dr;
            //_oSampleOutStandings_Cr = oSampleOutStandings_Cr;
            //_oSampleOutStandings_Cr_SampleOrder = oSampleOutStandings_Cr_SampleOrder;
            //_oSampleOutStandings = oSampleOutStandings;
            //_oPaymentDetails = oPaymentDetails;
            double nTotalDR = 0;
            double nTotalCR = 0;
            double nValue = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            var oContractors = _oSampleOutStandings_Dr.GroupBy(x => new { x.ContractorName, x.ContractorID }, (key, grp) =>
                                  new
                                  {
                                      ContractorName = key.ContractorName,
                                      ContractorID = key.ContractorID,
                                  }).ToList();

            
            var oContractors_CR = _oSampleOutStandings_Cr.GroupBy(x => new { x.ContractorName, x.ContractorID }, (key, grp) =>
                                 new
                                 {
                                     ContractorName = key.ContractorName,
                                     ContractorID = key.ContractorID,
                                 }).ToList();
            oContractors_CR.ForEach(x => oContractors.Add(x));
            var oContractors_Cr_SampleOrder = _oSampleOutStandings_Cr_SampleOrder.GroupBy(x => new { x.ContractorName, x.ContractorID }, (key, grp) =>
                               new
                               {
                                   ContractorName = key.ContractorName,
                                   ContractorID = key.ContractorID,
                               }).ToList();
            var oContractors_Cr_Payment = _oPaymentDetails.GroupBy(x => new { x.ContractorName, x.ContractorID }, (key, grp) =>
                             new
                             {
                                 ContractorName = key.ContractorName,
                                 ContractorID = key.ContractorID,
                             }).ToList();

            oContractors_Cr_Payment.ForEach(x => oContractors.Add(x));

            oContractors = oContractors.GroupBy(x => new { x.ContractorName, x.ContractorID }, (key, grp) =>
                         new
                         {
                             ContractorName = key.ContractorName,
                             ContractorID = key.ContractorID,
                         }).ToList();

            //var oContractors = _oSampleOutStandings_Dr.GroupBy(x => new { x.ContractorName, x.ContractorID }, (key, grp) =>
            //                    new
            //                    {
            //                        ContractorName = key.ContractorName,
            //                        ContractorID = key.ContractorID,
            //                    }).ToList();

            // ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Greige Fabric Detail", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Production Order Detail", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 5f, _oFontStyleBold, true);
            //PdfPTable oPdfPTable = new PdfPTable(6);
            //oPdfPTable.SetWidths(new float[] { 120f, 90f, 80f, 80f, 80f, 40f });

            //#region PO Info
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Party Wise Summary", 0, 6, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);

            //oPdfPTable.CompleteRow();


            //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            //#endregion

                #region Fabric Info
                PdfPTable oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 150f, 110f, 80f, 60f, 42f, 50f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Party Wise Summary", 0, 6, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Previous Due", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Due", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Adjustment", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Balance", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                
                oPdfPTable.CompleteRow();
                #endregion

                foreach (var oItem1 in oContractors)
                {
                     nTotalDR = 0;
                     nTotalCR = 0;
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ContractorName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    nTotalDR = _oSampleOutStandings_Dr.Where(x => (x.ContractorID == oItem1.ContractorID)).ToList().Sum(x => x.Amount);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "$ " + Global.MillionFormat(nTotalDR), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    nTotalCR = _oSampleOutStandings_Cr.Where(x => (x.ContractorID == oItem1.ContractorID)).ToList().Sum(x => x.Amount);
                    nTotalCR = nTotalCR + _oSampleOutStandings_Cr_SampleOrder.Where(x => (x.ContractorID == oItem1.ContractorID)).ToList().Sum(x => x.Amount);

                    nTotalCR = nTotalCR + _oPaymentDetails.Where(x => (x.ContractorID == oItem1.ContractorID)).ToList().Sum(x => (x.PaymentAmount + x.DisCount)/x.CRate);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "$" + Global.MillionFormat(nTotalCR), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "$" + Global.MillionFormat(nTotalDR - nTotalCR), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                    nValue = nValue + nTotalDR;
                    oPdfPTable.CompleteRow();
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //nValue = _oSampleOutStandings.Select(c => c.Opening).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "$" + Global.MillionFormat(nValue), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                nValue = _oSampleOutStandings.Select(c => c.Debit).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "$" + Global.MillionFormat(nValue), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                nValue = _oSampleOutStandings.Select(c => c.Credit).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "$" + Global.MillionFormat(nValue), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                nValue = _oSampleOutStandings.Select(c => c.Closing).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "$" + Global.MillionFormat(nValue), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            
        }
        #endregion
       
        #endregion

        #region Report Mkt Wise Detail
        public byte[] PrepareReportMktDetail(List<SampleOutStanding> oSampleOutStandings, Company oCompany, string sDateRange)
        {
          
            _oSampleOutStandings = oSampleOutStandings;


            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(10f, 10f, 5f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 802 });
            #endregion

            #region Report Body & Header

            this.PrintHeader("SAMPLE ORDER vs ADJUSTMENT", sDateRange);
            //this.PrintBodyDr();
            //if (_oSampleOutStandings_Cr.Count > 0 || _oSampleOutStandings_Cr_SampleOrder.Count > 0)
            //{
            //    this.PrintBodyCr();
            //}
            //this.PrintBody_PaymentCR();
            ////this.PrintBodyCr_Two();
            ////this.PrintBodySampleOrder_CR();
            ////this.PrintBody_PaymentCR();
            PrintBody_MktDetail();
            _oPdfPTable.HeaderRows = 3;
            #endregion

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void PrintBody_MktDetail()
        {
            //_oSampleOutStandings_Dr = oSampleOutStandings_Dr;
            //_oSampleOutStandings_Cr = oSampleOutStandings_Cr;
            //_oSampleOutStandings_Cr_SampleOrder = oSampleOutStandings_Cr_SampleOrder;
            //_oSampleOutStandings = oSampleOutStandings;
            //_oPaymentDetails = oPaymentDetails;

            double nOpening = 0;
            double nRD = 0;
            double nCR = 0;
            string sTemp = "";
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            var oMkts = _oSampleOutStandings.GroupBy(x => new { x.MKTPName }, (key, grp) =>
                               new
                               {
                                   MKTPName = key.MKTPName
                               }).ToList();

            var oContractors = _oSampleOutStandings.GroupBy(x => new { x.ContractorName, x.ContractorID, x.Opening }, (key, grp) =>
                                  new
                                  {
                                      ContractorName = key.ContractorName,
                                      Opening = key.Opening,
                                      ContractorID = key.ContractorID,
                                  }).ToList();
           // oContractors = oContractors.Where(b => b.ContractorID == 736).ToList();
            //sTemp=oMkts.OrderByDescending
            sTemp = string.Join(",", oMkts.Select(x => x.MKTPName).ToList());

            #region Fabric Info
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 60f, 50f, 100f, 60f, 50f, 50f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sTemp, 0, 6, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Party Wise Summary", 0, 6, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            #endregion

            foreach (var oItem1 in oContractors)
            {
                nOpening = 0;
                var SampleOutStandings = _oSampleOutStandings.Where(p => p.ContractorID == oItem1.ContractorID).ToList();
                SampleOutStandings = SampleOutStandings.OrderBy(o => o.RefDate).ToList();


                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 6, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ContractorName, 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Opening", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "$ " + Global.MillionFormat(oItem1.Opening), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                nOpening = oItem1.Opening;
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Ref No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Describtion", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
           
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Debit", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Credit", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                foreach (var oItemTwo in SampleOutStandings)
                {
                    sTemp = "";
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItemTwo.RefNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItemTwo.RefDate.ToString("dd MMM yyyy"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                    if (oItemTwo.InvoiceType>0)
                    { sTemp =sTemp+ oItemTwo.InvoiceTypeSt; }
                    if (!String.IsNullOrEmpty(oItemTwo.Note))
                    { sTemp = sTemp + oItemTwo.Note; }

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, sTemp, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItemTwo.Debit > 0) ? "$" + Global.MillionFormat(oItemTwo.Debit) : ""), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItemTwo.Credit > 0) ? "$" + Global.MillionFormat(oItemTwo.Credit) : ""), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    nRD = nRD + oItemTwo.Debit;
                    nCR = nCR + oItemTwo.Credit;
                    oPdfPTable.CompleteRow();
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nRD > 0) ? "$" + Global.MillionFormat(nRD) : ""), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nCR > 0) ? "$" + Global.MillionFormat(nCR) : ""), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Colsing", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "$" + Global.MillionFormat(nOpening + nRD - nCR), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

            }
          
            
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

        }
        #endregion
    }
}

