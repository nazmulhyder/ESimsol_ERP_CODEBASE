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
    public class rptDispoSummary
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
        DUStatement _oDUStatement = new DUStatement();
        SampleInvoice _oSampleInvoice=new SampleInvoice();
        SampleInvoiceSetup _oSampleInvoiceSetup=new SampleInvoiceSetup();
        Company _oCompany = new Company();

        FabricSCReport _oFabricSCReport = new FabricSCReport();
        List<DyeingOrderFabricDetail> _oDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();
        List<FabricDeliveryChallan> _oFabricDeliveryChallans = new List<FabricDeliveryChallan>();
        List<FabricDeliveryChallanDetail> _oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
  
        int _nCount = 0;
        double _nTotalAmount = 0;
        double _nTotalQty = 0;
        double _nTotalOrderQty = 0;
        double _nTotalDyeingQty = 0;
        double nUsagesHeight;
        double _nGrandTotalAmount = 0;
        double _nGrandTotalQty = 0;
        double _nGrandTotalOrderQty = 0;
        double _nGrandTotalDyeingQty = 0;

        double _nTotalClaim = 0;
        double _nTotalAdjQty = 0;
        double _nTotalAdjValue = 0;
        double _nTotalNetDyeingQty = 0;
        double _nTotalActualValue = 0;

        double _nTotalPOQty = 0;
        double _nTotalDOQty = 0;
        double _nTotalYetToDO = 0;
        double _nTotalYetToDC = 0;
        double _nGrandTotalYetToDC = 0;
        double _nTotalQty_RC = 0;
        double _nTotalDC = 0;
        double _nGrandTotalDC = 0;
        #endregion

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
            _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.BusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.BusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }
        #endregion
        
        #region BILL STATEMENT
        public byte[] PrepareBillStatementReport(DUStatement oDUStatement, SampleInvoiceSetup oSampleInvoiceSetup, int nTitleType)
        {
            _oDUStatement = oDUStatement;
            _oSampleInvoiceSetup = oSampleInvoiceSetup;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(37f, 10f, 10f, 30f);
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
            this.PrintHead_DyeingBill();
            foreach (SampleInvoice oSampleInvoice in _oDUStatement.SampleInvoices) 
            {
                if (_oDUStatement.SampleInvoices.Count > 1) 
                {
                    #region Blank
                    _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #region BILL NO
                    _oPdfPCell = new PdfPCell(new Phrase("BILL NO: " + _oSampleInvoiceSetup.Code + "-" + oSampleInvoice.InvoiceNo, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                }
                _oSampleInvoice = oSampleInvoice;
                this.PrintBody_DyeingBillOrderType(_oDUStatement.DyeingOrderReports.Where(x=>x.SampleInvoiceNo.Equals(oSampleInvoice.SampleInvoiceNo)).ToList());
            }
            //this.PrintHead_DyeingBill();
            this.PrintBody_DyeingBillSummary(_oDUStatement.DyeingOrderReports);
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 60f, 180f, 70f, 130f });

            for (int i = 1; i <= 12; i++) //3rows*4Column
            {
                string sData=(_oDUStatement.HeaderTable[i]==null?"":_oDUStatement.HeaderTable[i].ToString());
                if(i%2==0)
                    oPdfPCell = new PdfPCell(new Phrase(sData, _oFontStyle));
                else
                    oPdfPCell = new PdfPCell(new Phrase(sData, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            }
            
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
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
        private void PrintBody_DyeingBillOrderType(List<DyeingOrderReport> oDUReports)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            string _sMunit, _sCurrency;
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            oDyeingOrderReports = oDUReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.SaleOrder).ToList();
            if (oDUReports.Count > 0)
            {
                _sMunit = oDUReports[0].MUName;
                _sCurrency = oDUReports[0].Currency;
            }
            if (oDyeingOrderReports.Count > 0)
            {
                oDyeingOrderReports = oDyeingOrderReports.OrderBy(o => o.OrderNo).ThenBy(a => a.DyeingOrderID).ToList();
                this.PrintBody_DyeingBill(oDyeingOrderReports);
            }

            oDyeingOrderReports = oDUReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.SampleOrder).ToList();
            if (oDyeingOrderReports.Count > 0)
            {
                oDyeingOrderReports = oDyeingOrderReports.OrderBy(o => o.OrderNo).ThenBy(a => a.DyeingOrderID).ToList();
                this.PrintBody_DyeingBill(oDyeingOrderReports);
            }
        }
        private void PrintBody_DyeingBill(List<DyeingOrderReport> oDyeingOrderReports)
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

            string _sCurrency = oDyeingOrderReports.Where(x => !string.IsNullOrEmpty(x.Currency)).Select(x => x.Currency).FirstOrDefault(),
                _sMunit = oDyeingOrderReports.Where(x => !string.IsNullOrEmpty(x.MUName)).Select(x => x.MUName).FirstOrDefault();
            nUsagesHeight = 0;
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

                        _oPdfPCell = new PdfPCell(new Phrase(" Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(_sCurrency + "" + Global.MillionFormat(nAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();

                        #endregion

                        nCount = 0; nQty = 0; nAmount = 0;
                    }
                    #endregion

                    #region Header


                    #region Table Initialize
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

                    _oPdfPCell = new PdfPCell(new Phrase("Buyer Ref", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Merchandiser", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

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
                                                    65f, //Color
                                                    90f,  //Type of count                                          
                                                    28f,  //Color Category                                              
                                                    43f,  //Qty
                                                    28f,  //U/P
                                                    43f,  //Value
                                             });
                #endregion

                nCount++;

                //_oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (!String.IsNullOrEmpty(oItem.StyleNo))
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerRef + ", " + oItem.StyleNo, _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerRef, _oFontStyle));
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

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

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + "" + System.Math.Round(oItem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                //_oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + "" + Global.MillionFormatActualDigit(oItem.UnitPrice), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + "" + Global.MillionFormatActualDigit(oItem.Qty * oItem.UnitPrice), _oFontStyle));
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

            _oPdfPCell = new PdfPCell(new Phrase(_sCurrency + "" + Global.MillionFormat(nAmount), _oFontStyleBold));
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

            _oPdfPCell = new PdfPCell(new Phrase(_sCurrency + "" + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
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

            string _sCurrency = oDyeingOrderReports.Where(x => !string.IsNullOrEmpty(x.Currency)).Select(x => x.Currency).FirstOrDefault(),
                   _sMunit = oDyeingOrderReports.Where(x => !string.IsNullOrEmpty(x.MUName)).Select(x => x.MUName).FirstOrDefault();

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
            _nTotalQty = _oDUStatement.DyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SaleOrder).Sum(x => x.Qty);
            if (_nTotalQty > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Total Bulk Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + " " + _sMunit, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _nTotalAmount = _oDUStatement.DyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SaleOrder).Sum(x => (x.Qty * x.UnitPrice));
                _oPdfPCell = new PdfPCell(new Phrase(_sCurrency + " " + Global.MillionFormat(_nTotalAmount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            _nTotalQty = _oDUStatement.DyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SampleOrder).Sum(x => x.Qty);
            if (_nTotalQty > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Total Sample Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + " " + _sMunit, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _nTotalAmount = _oDUStatement.DyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SampleOrder).Sum(x => (x.Qty * x.UnitPrice));
                _oPdfPCell = new PdfPCell(new Phrase(_sCurrency + " " + Global.MillionFormat(_nTotalAmount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            _nTotalQty = _oDUStatement.DyeingOrderReports.Select(c => c.Qty).Sum();
            if (_nTotalQty > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Grand Total", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + " " + _sMunit, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _nTotalAmount = _oDUStatement.DyeingOrderReports.Select(c => (c.Qty * c.UnitPrice)).Sum();
                _oPdfPCell = new PdfPCell(new Phrase(_sCurrency + " " + Global.MillionFormat(_nTotalAmount), _oFontStyle));
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
                _nTotalAmount = _oDUStatement.DyeingOrderReports.Select(c => (c.Qty * c.UnitPrice)).Sum();
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalAmount) + "x" + _oSampleInvoice.ConversionRate.ToString("00.00"), _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.ExchangeCurrencySymbol + " " + Global.TakaFormat(Math.Round((_nTotalAmount * _oSampleInvoice.ConversionRate), 2)), _oFontStyle));
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

                _oPdfPCell = new PdfPCell(new Phrase(Global.TakaWords(Math.Round((_nTotalAmount * _oSampleInvoice.ConversionRate), 2)), _oFontStyle));
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


            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region BILL STATEMENT F2
        public byte[] PrepareBillStatementReport_F2(DUStatement oDUStatement, SampleInvoiceSetup oSampleInvoiceSetup, int nTitleType)
        {
            _oDUStatement = oDUStatement;
            _oSampleInvoiceSetup = oSampleInvoiceSetup;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(37f, 10f, 10f, 30f);
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
            this.PrintHead_DyeingBill();
            foreach (SampleInvoice oSampleInvoice in _oDUStatement.SampleInvoices)
            {
                if (_oDUStatement.SampleInvoices.Count > 1)
                {
                    #region Blank
                    _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #region BILL NO
                    ESimSolPdfHelper.FontStyle=FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, "BILL NO: " + _oSampleInvoiceSetup.Code + "-" + oSampleInvoice.InvoiceNo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                    #endregion
                }
                _oSampleInvoice = oSampleInvoice;
                this.PrintBody_DyeingBillOrderType_F2(_oDUStatement.DyeingOrderReports.Where(x => x.SampleInvoiceNo.Equals(oSampleInvoice.SampleInvoiceNo)).ToList());
            }
            //this.PrintHead_DyeingBill();
            this.PrintBody_DyeingBillSummary(_oDUStatement.DyeingOrderReports);
            this.PrintFooter_DyeingBill();

            _oPdfPTable.HeaderRows = 2;

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintBody_DyeingBillOrderType_F2(List<DyeingOrderReport> oDUReports)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            string _sMunit, _sCurrency;
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            oDyeingOrderReports = oDUReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.SaleOrder).ToList();
            if (oDUReports.Count > 0)
            {
                _sMunit = oDUReports[0].MUName;
                _sCurrency = oDUReports[0].Currency;
            }
            if (oDyeingOrderReports.Count > 0)
            {
                oDyeingOrderReports = oDyeingOrderReports.OrderBy(o => o.OrderNo).ThenBy(a => a.DyeingOrderID).ToList();
                this.PrintBody_DyeingBill_F2(oDyeingOrderReports);
            }

            oDyeingOrderReports = oDUReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.SampleOrder).ToList();
            if (oDyeingOrderReports.Count > 0)
            {
                oDyeingOrderReports = oDyeingOrderReports.OrderBy(o => o.OrderNo).ThenBy(a => a.DyeingOrderID).ToList();
                this.PrintBody_DyeingBill_F2(oDyeingOrderReports);
            }
        }
        private void PrintBody_DyeingBill_F2(List<DyeingOrderReport> oDyeingOrderReports)
        {
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable = DyeingBillTable_F2();
            #endregion

            #region Header Name
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, oDyeingOrderReports[0].OrderTypeSt, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER,0,9,0);
            #endregion

            #region Insert Into Main Table
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER,0,9);
            #endregion

            #region Initialize Table
            oPdfPTable = new PdfPTable(9);
            oPdfPTable = DyeingBillTable_F2();
            #endregion

            int nDyeingOrderID = 0; int nCount = 0;
            double nQty = 0, nAmount = 0, nTotalAmount = 0, nTotalQty = 0;

            string _sCurrency = oDyeingOrderReports.Where(x => !string.IsNullOrEmpty(x.Currency)).Select(x => x.Currency).FirstOrDefault(),
                   _sMunit    = oDyeingOrderReports.Where(x => !string.IsNullOrEmpty(x.MUName)).Select(x => x.MUName).FirstOrDefault();

            nUsagesHeight = 0;
            foreach (DyeingOrderReport oItem in oDyeingOrderReports)
            {
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
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total :", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nQty), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, _sCurrency + "" + Global.MillionFormat(nAmount), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, 1);
                        #endregion

                        nCount = 0; nQty = 0; nAmount = 0;
                    }
                    #endregion

                    #region Header


                    #region Initialize Table
                    oPdfPTable = new PdfPTable(9);
                    oPdfPTable = DyeingBillTable_F2();
                    #endregion

                    #region Header Row
                    ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "Order No", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "Buyer Ref", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "Merchandiser", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "Color", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "Yarn Type", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "Shade", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "Qty " + "(" + _sMunit + ")", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "U.P.", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "Value", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, 1);
                    #endregion

                    #endregion
                }

                #region Initialize Table
                oPdfPTable = new PdfPTable(9);
                oPdfPTable = DyeingBillTable_F2();
                #endregion

                nCount++;

                //_oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                ESimSolPdfHelper.FontStyle = _oFontStyle;
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.OrderNo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                    
                if (!String.IsNullOrEmpty(oItem.StyleNo))
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.BuyerRef + ", " + oItem.StyleNo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                }
                else
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.BuyerRef, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                }
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.CPName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.ColorName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                ESimSolPdfHelper.FontStyle =FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.ProductName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                ESimSolPdfHelper.FontStyle = _oFontStyle;

                if (oItem.RGB == "" || oItem.RGB == null)
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.ColorNo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                }
                else
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.RGB, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                }
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Currency + "" + System.Math.Round(oItem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Currency + "" + Global.MillionFormatActualDigit(oItem.Qty * oItem.UnitPrice), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
         
                nQty += oItem.Qty;
                nAmount += oItem.Qty * oItem.UnitPrice;

                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, 1);
                #endregion

                nDyeingOrderID = oItem.DyeingOrderID;
                nTotalQty = nTotalQty + oItem.Qty;
                nTotalAmount += (oItem.Qty * oItem.UnitPrice);

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
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(9);
            oPdfPTable = DyeingBillTable_F2();
            #endregion

            #region Order Wise Sub Total
            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total :", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nQty), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _sCurrency + "" + Global.MillionFormat(nAmount), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " Total :", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(Math.Round(nTotalQty, 2)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _sCurrency + "" + Global.MillionFormat(nTotalAmount), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, 1);

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
            #endregion
        }
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
        #endregion

        #region DELIVERY ORDER STATEMENT
        public byte[] PrepareReport_DO(DUStatement oDUStatement, int nDOtype)
        {
            _oDUStatement = oDUStatement;

            //_oDUReturnChallanDetails = oLCDeliveryReport.ReturnChallanDetails;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(10f, 10f, 20f, 20f);
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
            //this.ReporttHeader_DO("Delivery Statement");
            //this.PrintHead_DO();

         
            //_oExportPIDetails = _oDUStatement.ExportPIDetails;

            //_oDyeingOrderReports = _oDUStatement.DyeingOrderReports;
            //_oDUDeliveryOrderDetails = _oDUStatement.DUDeliveryOrderDetails;
            //_oDUDeliveryChallanDetails = _oDUStatement.DUDeliveryChallanDetails.OrderBy(x => x.OrderID).ToList();
            //_oDUReturnChallanDetails = _oDUStatement.DUReturnChallanDetails;
            //this.PrintBalance_DO();
            //this.SetDeliveryOrder_DO();

            //this.SetDeliveryChallan_DO(nDOtype);
          
            //this.SetReturnChallan_DO();
            //this.SetDeliveryOrder_Claim_DO();
            //this.SetDeliveryChallan_Claim_DO();

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintBody()
        {
         
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15f, _oFontStyle, true);
            PdfPTable oPdfPTable;
            var fontBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            var fontNormal = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            #region Fabric Info
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] {50f, 100f, 50f, 130f, 60f,130f });

            #endregion

            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No: ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.ExeNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Mkt Ref", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.FabricNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "PO No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.SCNoFull, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer Name: ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.ContractorName, 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Mkt Ref", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.FabricNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "PI No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.PINo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer Name: ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.BuyerName, 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Mkt Ref", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.FabricNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "L/C No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.PINo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Mkt Concern: ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.MKTPName+"["+_oFabricSCReport.MKTGroup+"]", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finish Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.FinishTypeName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weave:", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Process Type: ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.ProcessTypeName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finish Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.FinishTypeName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weave:", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.FabricWeaveName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            oPdfPTable.CompleteRow();
         
           
            #endregion

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
        }
        private void PrintBodyTwo()
        {

            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15f, _oFontStyle, true);
            PdfPTable oPdfPTable;
            var fontBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            var fontNormal = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            #region Fabric Info
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 100f, 100f, 50f, 130f, 60f, 130f });

            #endregion

            #region Warp
            if (_oDyeingOrderFabricDetails.Any())
            {
                foreach (DyeingOrderFabricDetail oItem in _oDyeingOrderFabricDetails)
                {
                 
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ColorName , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.BatchNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    oPdfPTable.CompleteRow();
                 
                }
           
            }
            #endregion

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
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
     
    }
}
