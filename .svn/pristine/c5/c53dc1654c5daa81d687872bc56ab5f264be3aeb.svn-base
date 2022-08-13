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
    public class rptDUStatement
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

        List<DyeingOrderReport> _oDyeingOrderReports = new List<DyeingOrderReport>();
        List<ExportPIDetail> _oExportPIDetails = new List<ExportPIDetail>();
        List<ExportSCDetailDO> _oExportSCDetailDO = new List<ExportSCDetailDO>();
        List<DUDeliveryChallan> _oDUDeliveryChallans = new List<DUDeliveryChallan>();
        List<DUDeliveryChallanRegister> _oDUDeliveryChallanRegisters = new List<DUDeliveryChallanRegister>();
        List<DUDeliveryChallanDetail> _oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
        List<DUDeliveryOrderDetail> _oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
        List<DUReturnChallanDetail> _oDUReturnChallanDetails = new List<DUReturnChallanDetail>();

        List<FabricDeliveryChallan> _oFabricDeliveryChallans = new List<FabricDeliveryChallan>();
        List<FabricDeliveryChallanDetail> _oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
        List<ExportBill> _oExportBills = new List<ExportBill>();
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
            this.ReporttHeader_DO("Delivery Statement");
            this.PrintReport_Header();

         
            _oExportPIDetails = _oDUStatement.ExportPIDetails;

            _oDyeingOrderReports = _oDUStatement.DyeingOrderReports;
            _oDUDeliveryOrderDetails = _oDUStatement.DUDeliveryOrderDetails;
            _oDUDeliveryChallanDetails = _oDUStatement.DUDeliveryChallanDetails.OrderBy(x => x.OrderID).ToList();
            _oDUReturnChallanDetails = _oDUStatement.DUReturnChallanDetails;
            //this.PrintBalance_DO();
            //this.SetDeliveryOrder_DO();

            this.SetDeliveryChallan_DU();
          
            //this.SetReturnChallan_DO();
            //this.SetDeliveryOrder_Claim_DO();
            //this.SetDeliveryChallan_Claim_DO();

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void ReporttHeader_DO(string sHeader)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLDITALIC);

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] {150f, 200f, 150f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sHeader, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUStatement.BusinessUnitType.ToString(), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle_UnLine);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);

            //#region Proforma Invoice Heading Print
            //_oPdfPCell = new PdfPCell(new Phrase(sHeader, FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD)));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion
        }
        private void PrintHead_DO()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.UNDERLINE);

            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);
            PdfPTable oPdfPTable = new PdfPTable(6);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 75f, 165f, 100f, 105f, 75f, 70f });
            for (int i = 1; i <= _oDUStatement.HeaderTable.Count; i++) //4rows*6Column
            {
                if (i % 2 == 0)
                    oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.HeaderTable[i].ToString(), _oFontStyle));
                else
                    oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.HeaderTable[i].ToString(), _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintReport_Header()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLDITALIC);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 60f, 170f, 60f, 120f, 60f, 150f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUStatement.Title, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":" + _oDUStatement.TitleNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable,":"+ _oDUStatement.TitleDate, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 2, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle_UnLine);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":" + _oDUStatement.BuyerName, 0,5, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);
            //oPdfPTable = new PdfPTable(4);
            //oPdfPTable.SetWidths(new float[] { 60f, 200f, 60f,200f });

            for (int i = 1; i <= _oDUStatement.HeaderTable.Count; i++) //4rows*6Column
            {
                if (i % 2 == 0)
                   // oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.HeaderTable[i].ToString(), _oFontStyle));
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":" + _oDUStatement.HeaderTable[i].ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
                else
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDUStatement.HeaderTable[i].ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                //oPdfPTable.CompleteRow();
            }
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);



        }
        private void PrintBalance_DO()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);
            PdfPTable oPdfPTable = new PdfPTable(8);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 60f, 88f, 60f, 86f, 60f, 86f, 60f, 86f });

            oPdfPCell = new PdfPCell(new Phrase("PI vs PO:", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUStatement.Qty_YetToDO + " " + _oDUStatement.MUName, _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.Colspan = 5;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("PI vs DO:", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + Global.MillionFormat((_oDUStatement.Qty_Total - _oDUStatement.Qty_DO)) + " " + _oDUStatement.MUName, _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.Colspan = 5;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("PI vs DC:", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + Global.MillionFormat((_oDUStatement.Qty_Total + _oDUStatement.Qty_Claim - _oDUStatement.Qty_DC)) + " " + _oDUStatement.MUName, _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("DO vs DC:", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + Global.MillionFormat(_oDUStatement.Qty_DO - _oDUStatement.Qty_DC) + " " + _oDUStatement.MUName, _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
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
        private void SetReturnChallan_DO()
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

            if (_oDUStatement.DUReturnChallanDetails.Count > 0)
            {
                _nTotalAmount = 0;
                _nTotalQty = 0;
                _nGrandTotalAmount = 0;
                _nGrandTotalQty = 0;

                oPdfPCell = new PdfPCell(new Phrase("Return Challan Info", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                _oDUDeliveryOrderDetails = _oDUDeliveryOrderDetails.OrderBy(o => o.ProductID).ToList();
                foreach (DUReturnChallanDetail oCODetail in _oDUStatement.DUReturnChallanDetails)
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                        oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyleBold));
                        //oPdfPCell.Colspan = 2;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold));
                        //oPdfPCell.Colspan = 2;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oDUStatement.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        //oPdfPCell = new PdfPCell(new Phrase("U/P(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //oPdfPTable.AddCell(oPdfPCell);


                        //oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Qty  ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Due DO ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Delivery Qty  ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Due D/C ", _oFontStyleBold));
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
                    oPdfPCell = new PdfPCell(new Phrase(oCODetail.RCDate, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oCODetail.RCNo, _oFontStyle));
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



                    //oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oCODetail.DOQty), _oFontStyle));
                    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //oPdfPTable.AddCell(oPdfPCell);

                    //oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oCODetail.Qty - oCODetail.DOQty), _oFontStyle));
                    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //oPdfPTable.AddCell(oPdfPCell);


                    //oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oCODetail.DOQty), _oFontStyle));
                    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //oPdfPTable.AddCell(oPdfPCell);

                    //oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oCODetail.DOQty), _oFontStyle));
                    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //oPdfPTable.AddCell(oPdfPCell);


                    //_nTotalYetToDC = _nTotalYetToDC + (oCODetail.Qty - oCODetail.DOQty);

                    //_nTotalQty = _nTotalQty + oCODetail.Qty;

                    //_nGrandTotalQty = _nGrandTotalQty + oCODetail.Qty;
                    //_nTotalDC = _nTotalDC + oCODetail.DOQty;
                    //_nGrandTotalDC = _nGrandTotalDC + oCODetail.DOQty;
                    //_nGrandTotalYetToDC = _nGrandTotalYetToDC + ((oCODetail.Qty - oCODetail.DOQty));

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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
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
        private void SetDeliveryOrder_DO()
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


                oPdfPCell = new PdfPCell(new Phrase("Delivery Order Info", FontFactory.GetFont("Tahoma", 10f, 3)));
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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


                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oDUStatement.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("U/P(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Delivery Qty  ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Due D/C ", _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
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
        private void SetDeliveryChallan_DO(int nDOtype)
        {
            #region Delivery challan
            int nColspan = 11;
            PdfPTable oPdfPTable = new PdfPTable(11);
            oPdfPTable.SetWidths(new float[] { 60f, 80f, 60f, 40f, 60f, 60f, 60f, 60f, 60f, 60f, 60f });

            if (nDOtype == 2) 
            {
                nColspan = 12;
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.SetWidths(new float[] { 55f, 80f, 55f, 40f, 55f, 55f, 55f, 50f, 50f, 50f, 50f, 55f });
            }

            PdfPCell oPdfPCell;
           
            int nOrderID = 0;

            _nTotalOrderQty = 0;
            _nTotalDyeingQty = 0;
            _nTotalAmount = 0;
            _nTotalQty = 0;
            _nGrandTotalAmount = 0;
            _nGrandTotalQty = 0;
            nOrderID = 0;
            if (_oDUDeliveryChallanDetails.Count > 0)
            {
                oPdfPCell = new PdfPCell(new Phrase("Delivery Challan Info", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = nColspan;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                foreach (DUDeliveryChallanDetail oDCDetail in _oDUDeliveryChallanDetails)
                {
                    var oDyeingOrderDetail = _oDyeingOrderReports.Where(x => x.DyeingOrderDetailID == oDCDetail.DyeingOrderDetailID).FirstOrDefault();

                    if (nOrderID != oDCDetail.OrderID)
                    {
                        #region Header

                        if (nOrderID > 0)
                        {
                            #region Total
                            _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                            _oPdfPCell.Colspan = 7;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalOrderQty), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            if (nDOtype == 2)
                            {
                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalDyeingQty), _oFontStyleBold));
                                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                oPdfPTable.AddCell(_oPdfPCell);
                            }

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            oPdfPTable.CompleteRow();
                            #endregion
                        }

                        oPdfPCell = new PdfPCell(new Phrase("Buyer Name: " + oDyeingOrderDetail.ContractorName, _oFontStyleBold));
                        oPdfPCell.Colspan = nColspan;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);
                        oPdfPTable.CompleteRow();

                        oPdfPCell = new PdfPCell(new Phrase("Order No ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Client Ref", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Shade", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Batch", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Order Qty (" + _oDUStatement.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        if (nDOtype == 2)
                        {
                            oPdfPCell = new PdfPCell(new Phrase("Dyeing Qty (" + _oDUStatement.MUName + ")", _oFontStyleBold));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            oPdfPTable.AddCell(oPdfPCell);
                        }

                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oDUStatement.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPTable.CompleteRow();
                        #endregion
                        _nTotalQty = 0;
                        _nTotalOrderQty = 0;
                        _nTotalDyeingQty = 0;
                        _nTotalAmount = 0;
                        _nCount = 0;
                    }
                    #region PrintDetail
                    //List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
                    //_oDyeingOrderDetails = _oDyeingOrderDetails.Where(o => o.ProductID == oChallan.ProductID).ToList();

                    //for (int i = 0; i < oDyeingOrderDetails.Count; i++)
                    //{
                    _nCount++;

                    oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.OrderNoFull, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.BuyerRef, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(((EnumShade)oDyeingOrderDetail.Shade).ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDCDetail.LotNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDCDetail.ChallanDate, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDCDetail.ChallanNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    if (nOrderID != oDCDetail.OrderID)
                    {
                        double nOrderQty = _oDyeingOrderReports.Where(x => x.DyeingOrderID == oDCDetail.OrderID).Sum(x => x.Qty);
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nOrderQty), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPCell.Colspan = _oDyeingOrderReports.Where(x => x.DyeingOrderID == oDCDetail.OrderID).Count();
                        oPdfPTable.AddCell(oPdfPCell);

                        _nTotalOrderQty = _nTotalOrderQty + nOrderQty;
                        _nGrandTotalOrderQty = _nGrandTotalOrderQty + nOrderQty;
                    }

                    if (nDOtype == 2)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(oPdfPCell);
                    }

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
                 
                    _nTotalDyeingQty = _nTotalDyeingQty + oDyeingOrderDetail.Qty;
                    _nGrandTotalAmount = _nGrandTotalAmount + (oDCDetail.Qty * oDCDetail.UnitPrice);
                    _nGrandTotalQty = _nGrandTotalQty + oDCDetail.Qty;
                    _nGrandTotalDyeingQty = _nGrandTotalDyeingQty + oDyeingOrderDetail.Qty;

                    #endregion

                    nOrderID = oDCDetail.OrderID;
                }

                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 7;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);


                //List<DUDeliveryChallanRegister> Total_Order = new List<DUDeliveryChallanRegister>();
                //Total_Order = _oDUDeliveryChallanRegisters.Where(x=>x.ProductID==nOrderID).GroupBy(x => new { x.OrderNoFull }, (key, grp) => new DUDeliveryChallanRegister
                //{
                //    Qty_Order = grp.Select(x => x.Qty_Order).FirstOrDefault()
                //}).ToList();
                //_nTotalOrderQty = (Total_Order.Count > 0 ? Total_Order.Sum(x => x.Qty_Order) : 0);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalOrderQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                if (nDOtype == 2)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalDyeingQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);
                }

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 7;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                //List<DUDeliveryChallanRegister> GrandTotal_Order = new List<DUDeliveryChallanRegister>();
                //GrandTotal_Order=  _oDUDeliveryChallanRegisters.GroupBy(x => new { x.OrderNoFull }, (key, grp) => new DUDeliveryChallanRegister
                //{
                //    Qty_Order = grp.Select(x=>x.Qty_Order).FirstOrDefault()
                //}).ToList();
                //_nGrandTotalOrderQty = (GrandTotal_Order.Count > 0 ? GrandTotal_Order.Sum(x=>x.Qty_Order) : 0);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalOrderQty) + "" + _oDUStatement.MUName, _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                if (nDOtype == 2)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalDyeingQty) + "" + _oDUStatement.MUName, _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);
                }

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty) + "" + _oDUStatement.MUName, _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.Colspan = nColspan; oPdfPCell.FixedHeight = 20f;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(oPdfPCell);


                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();


                #region Return Challan

                oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] { 180f, 70f, 70f, 70f, 70f, 70f, 70f});

                var oDyeingDetails = _oDyeingOrderReports.GroupBy(x => new { x.ProductID, x.ProductName}, (key, grp) => new 
                {
                    ProductID=key.ProductID,
                    ProductName=key.ProductName,
                    Qty_Order = grp.Sum(x => x.Qty),
                    Qty_DC = grp.Sum(x => x.Qty_DC),
                }).ToList();
                var oRCDetails = _oDUReturnChallanDetails.GroupBy(x => new { x.ProductID, x.ProductName }, (key, grp) => new
                {
                    ProductID = key.ProductID,
                    ProductName = key.ProductName,
                    Qty_RC = grp.Sum(x => x.Qty)
                }).ToList();
                var oDODetails = _oDyeingOrderReports.GroupBy(x => new { x.ProductID, x.ProductName }, (key, grp) => new
                {
                    ProductID = key.ProductID,
                    ProductName = key.ProductName,
                    Qty_DO = grp.Sum(x => x.Qty_DO)
                }).ToList();

                var oDCDetails = _oDUDeliveryChallanDetails.GroupBy(x => new { x.ProductID, x.ProductName }, (key, grp) => new
                {
                    ProductID = key.ProductID,
                    ProductName = key.ProductName,
                    Qty_DC = grp.Sum(x => x.Qty)
                }).ToList();

                if (oRCDetails.Count > 0)
                {
                    #region Header
                    oPdfPCell = new PdfPCell(new Phrase("Yarn", _oFontStyleBold));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyleBold));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase("DO Qty", _oFontStyleBold));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("Due DO", _oFontStyleBold));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("DC Qty", _oFontStyleBold));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("RC Qty", _oFontStyleBold));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("Due DC", _oFontStyleBold));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                    foreach (var oItem in oRCDetails)
                    {
                        _nCount++;
                        oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        double nDyeingQty = oDyeingDetails.Where(x => x.ProductID == oItem.ProductID).Select(x => x.Qty_Order).FirstOrDefault();
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nDyeingQty), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        double nDOQty = oDODetails.Where(x => x.ProductID == oItem.ProductID).Select(x => x.Qty_DO).FirstOrDefault();
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nDOQty), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nDyeingQty-nDOQty), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        double nDCQty = oDCDetails.Where(x => x.ProductID == oItem.ProductID).Select(x => x.Qty_DC).FirstOrDefault();
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nDCQty), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        double nRCQty = oRCDetails.Where(x => x.ProductID == oItem.ProductID).Select(x => x.Qty_RC).FirstOrDefault();
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nRCQty), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nDOQty - nDCQty + nRCQty), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                }
                #endregion Return Challan

            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion Delivery challan
        }
        private void SetDeliveryChallan_DU()
        {
            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);

            PdfPTable oPdfPTable = new PdfPTable(9);

            var oDyeingOrders = _oDyeingOrderReports.GroupBy(x => new { x.OrderNo, x.DyeingOrderID, x.NoCode ,x.OrderNoFull}, (key, grp) =>
                                  new
                                  {
                                      DyeingOrderID = key.DyeingOrderID,
                                      OrderNo = key.OrderNo,
                                      NoCode = key.NoCode,
                                      OrderNoFull = key.OrderNoFull
                                  }).ToList();


            #region Fabric Info
            oPdfPTable = new PdfPTable(8);
            oPdfPTable.SetWidths(new float[] { 150f,  110f, 90f, 65f, 68f, 65f, 68f, 65f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
          //  ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Count", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shade", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order Qty(" + _oDUStatement.MUName + ")", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery\nDate", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Challan No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery\nQty(" + _oDUStatement.MUName + ")", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion
            foreach (var oDyeingOrder in oDyeingOrders)
            {
                _nTotalDC = 0;
                var oDyeingOrderReports = _oDyeingOrderReports.Where(x => x.DyeingOrderID == oDyeingOrder.DyeingOrderID).ToList();

                oPdfPTable = new PdfPTable(8);
                oPdfPTable.SetWidths(new float[] { 150f, 110f, 90f, 65f, 68f, 65f, 68f, 65f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No:" + oDyeingOrder.OrderNoFull, 0, 9, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Count", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shade", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order Qty(" + _oDUStatement.MUName + ")", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery\nDate", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Challan No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery\nQty(" + _oDUStatement.MUName + ")", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


                foreach (var oItem in oDyeingOrderReports)
                {
                    oPdfPTable = new PdfPTable(8);
                    oPdfPTable.SetWidths(new float[] { 150f, 110f, 90f, 65f, 68f, 65f, 68f, 65f });

                    var oDUDelivertChallanDetails = _oDUStatement.DUDeliveryChallanRegisters.Where(x => x.DyeingOrderDetailID == oItem.DyeingOrderDetailID).ToList();
                    nCount = oDUDelivertChallanDetails.Count;

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ContractorName, (nCount <= 0) ? 0 : nCount, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.OrderNoFull, (nCount <= 0) ? 0 : nCount, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, (nCount <= 0) ? 0 : nCount, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ColorName, (nCount <= 0) ? 0 : nCount, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty), (nCount <= 0) ? 0 : nCount, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    if (nCount <= 0)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                        oPdfPTable.CompleteRow();
                        ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                    }
                    else
                    {
                        foreach (var oItemChallan in oDUDelivertChallanDetails)
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItemChallan.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItemChallan.ChallanDateSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItemChallan.ChallanNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItemChallan.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                            _nTotalDC = _nTotalDC + oItemChallan.Qty;
                            oPdfPTable.CompleteRow();
                           
                        }
                        ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                    }
                   
                }
                oPdfPTable = new PdfPTable(8);
                oPdfPTable.SetWidths(new float[] { 150f, 110f, 90f, 65f, 68f, 65f, 68f, 65f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTotalQty = oDyeingOrderReports.Where(x => x.DyeingOrderID == oDyeingOrder.DyeingOrderID).Select(c => c.Qty).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalDC), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
              
            }
            oPdfPTable = new PdfPTable(8);
            oPdfPTable.SetWidths(new float[] { 150f, 110f, 90f, 65f, 68f, 65f, 68f, 65f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Grand Total", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            _nTotalQty = _oDyeingOrderReports.Select(c => c.Qty).Sum();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalDC), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 50f, 200f, 70f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Order Qty", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            _nTotalQty = _oDyeingOrderReports.Select(c => c.Qty).Sum();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Process Loss:4%", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty * 4 / 100), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "To Be Delivered", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            _nTotalDC = _oDUStatement.DUDeliveryChallanRegisters.Select(c => c.Qty).Sum();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalDC), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            _nTotalYetToDC = _nTotalQty - (_nTotalQty * 4 / 100) - _nTotalDC;
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTotalYetToDC < 0) ? "Over  Delivery" : "Due Delivery", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalYetToDC), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

        }
     
        private void SetDeliveryOrder_Claim_DO()
        {
            #region Delivery Order Claim
            _nTotalAmount = 0;
            _nTotalQty = 0;
            PdfPTable oPdfPTable = new PdfPTable(8);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100f, 75f, 130f, 60, 60f, 75f, 70f, 70f });
            int nProductID = 0;

            if (_oDUStatement.DUDeliveryOrderDetails_Claim.Count > 0)
            {
                _nTotalAmount = 0;
                _nTotalQty = 0;
                _nGrandTotalAmount = 0;
                _nGrandTotalQty = 0;


                oPdfPCell = new PdfPCell(new Phrase("Claim Delivery Order Info", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                _oDUStatement.DUDeliveryOrderDetails_Claim = _oDUStatement.DUDeliveryOrderDetails_Claim.OrderBy(o => o.ProductID).ToList();
                foreach (DUDeliveryOrderDetail oDODetail in _oDUStatement.DUDeliveryOrderDetails_Claim)
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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


                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oDUStatement.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("U/P(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Delivery Qty  ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Due D/C ", _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
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
        private void SetDeliveryChallan_Claim_DO()
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
            if (_oDUStatement.DUDeliveryChallanDetails_Claim.Count > 0)
            {


                oPdfPCell = new PdfPCell(new Phrase("Claim Delivery Challan Info", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                _oDUStatement.DUDeliveryChallanDetails_Claim = _oDUStatement.DUDeliveryChallanDetails_Claim.OrderBy(o => o.ProductID).ToList();
                foreach (DUDeliveryChallanDetail oDCDetail in _oDUStatement.DUDeliveryChallanDetails_Claim)
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oDUStatement.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
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
                    oDUReturnChallanDetails = _oDUStatement.DUReturnChallanDetails_Claim.Where(o => o.DUDeliveryChallanDetailID == oDCDetail.DUDeliveryChallanDetailID).ToList();
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty) + "" + _oDUStatement.MUName, _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
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
        #endregion

        #region DYEING STATEMENT
        public byte[] PrepareReport_DU(DUStatement oDUStatement)
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
            this.ReporttHeader_DU();
            this.PrintHead();


            _oDyeingOrderReports = _oDUStatement.DyeingOrderReports;
            _oExportPIDetails = _oDUStatement.ExportPIDetails;
            //_oExportSCDetailDO = _oDUStatement.ExportSCDetailDOs;

            _oDUDeliveryChallans = _oDUStatement.DUDeliveryChallans;
            _oDUDeliveryChallanDetails = _oDUStatement.DUDeliveryChallanDetails;
            _oDUDeliveryOrderDetails = _oDUStatement.DUDeliveryOrderDetails;
            _oFabricDeliveryChallanDetails = _oDUStatement.FabricDeliveryChallanDetails;
            _oFabricDeliveryChallans = _oDUStatement.FabricDeliveryChallans;
            _oExportBills = _oDUStatement.ExportBills;
            this.PrintBalance();
            if (_oExportPIDetails.Count > 0)
            {
                this.SetExportBillInfo();
            }
            if (_oDUStatement.BusinessUnitType == EnumBusinessUnitType.Weaving || _oDUStatement.BusinessUnitType == EnumBusinessUnitType.Finishing)
            {
                this.SetExportPIInfo();
                this.SetDeliveryChallan_Fabric();
            }
            else
            {
                this.SetExportPI_LC();
                this.SetDyeingOrderTwo();
                this.SetDyeingOrderStatus();
                this.SetDeliveryOrder();
                this.SetDeliveryChallan();
                this.SetClaimOrder();
                this.SetDeliveryOrder_Claim();
                this.SetDeliveryChallan_Claim();
            }
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void ReporttHeader_DU()
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
        private void PrintHead()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.UNDERLINE);

            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);
            PdfPTable oPdfPTable = new PdfPTable(6);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 75f, 165f, 100f, 105f, 75f, 70f });
            if (_oDUStatement.HeaderTable != null && _oDUStatement.HeaderTable.Count > 0)
            {
                for (int i = 1; i <= _oDUStatement.HeaderTable.Count; i++) //4rows*6Column
                {
                    if (i % 2 == 0)
                        oPdfPCell = new PdfPCell(new Phrase(": " + _oDUStatement.HeaderTable[i].ToString(), _oFontStyleBold));
                    else
                        oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.HeaderTable[i].ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);
                }
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintBalance()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);
            PdfPTable oPdfPTable = new PdfPTable(8);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 60f, 88f, 60f, 86f, 60f, 86f, 60f, 86f });

            oPdfPCell = new PdfPCell(new Phrase("PI vs PO:", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + Global.MillionFormat(_oDUStatement.Qty_Total - _oDUStatement.Qty_PO) + " " + _oDUStatement.MUName, _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.Colspan = 5;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("PI vs DO:", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + Global.MillionFormat((_oDUStatement.Qty_Total - _oDUStatement.Qty_DO)) + " " + _oDUStatement.MUName, _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.Colspan = 5;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("PI vs DC:", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + Global.MillionFormat((_oDUStatement.Qty_Total + _oDUStatement.Qty_Claim - _oDUStatement.Qty_DC)) + " " + _oDUStatement.MUName, _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("DO vs DC:", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + Global.MillionFormat(_oDUStatement.Qty_DO - _oDUStatement.Qty_DC) + " " + _oDUStatement.MUName, _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
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
        private void SetExportPI()
        {
            _nTotalAmount = 0;
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(10);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 25f, 150f, 50f, 50f, 55f, 50f, 50f, 55f, 60f, 55f });
            int nCount = 0;
            if (_oExportPIDetails.Count > 0)
            {

                oPdfPCell = new PdfPCell(new Phrase("P/I  Product(s)", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 10;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();




                oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Qty(" + _oDUStatement.MUName + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("U/P(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Adj Qty", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("Adj Rate", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Adj Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Net Dyeing(" + _oDUStatement.MUName + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                foreach (ExportPIDetail oItem in _oExportPIDetails)
                {
                    #region PrintDetail

                    _nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);



                    oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(oItem.Qty * oItem.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);
                    if (oItem.AdjQty <= 0)
                    {
                        oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle));
                    }
                    else
                    {
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.AdjQty), _oFontStyle));
                    }
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    if (oItem.AdjRate <= 0)
                    {
                        oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle));
                    }
                    else
                    {
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.AdjRate), _oFontStyle));
                    }
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    if (oItem.AdjValue <= 0)
                    {
                        oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle));
                    }
                    else
                    {
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.AdjValue), _oFontStyle));
                    }

                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    if ((oItem.Qty - oItem.AdjQty) <= 0)
                    {
                        oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle));
                    }
                    else
                    {
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty - oItem.AdjQty), _oFontStyle));
                    }
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat((oItem.Qty * oItem.UnitPrice) - oItem.AdjValue), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    _nTotalQty = _nTotalQty + oItem.Qty;
                    _nTotalAmount = _nTotalAmount + (oItem.Qty * oItem.UnitPrice);
                    _nTotalAdjQty = _nTotalAdjQty + (oItem.AdjQty);
                    _nTotalAdjValue = _nTotalAdjValue + (oItem.AdjValue);
                    _nTotalNetDyeingQty = _nTotalNetDyeingQty + (oItem.Qty - oItem.AdjQty);
                    _nTotalActualValue = _nTotalActualValue + ((oItem.Qty * oItem.UnitPrice) - oItem.AdjValue);

                    oPdfPTable.CompleteRow();
                    #endregion
                }
                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 2;
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);


                if (_nTotalAdjQty <= 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyleBold));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalAdjQty), _oFontStyleBold));
                }
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);


                if (_nTotalAdjValue <= 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyleBold));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAdjValue), _oFontStyleBold));
                }
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);


                if (_nTotalNetDyeingQty <= 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyleBold));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalNetDyeingQty), _oFontStyleBold));
                }
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                if (_nTotalActualValue <= 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyleBold));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalActualValue), _oFontStyleBold));
                }
                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalActualValue), _oFontStyleBold));
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
        private void SetExportPI_LC()
        {
            _nTotalQty = 0;
            PdfPTable oPdfPTable = new PdfPTable(8);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 25f, 100f, 60f, 180f, 40f, 50f, 45f, 55f });
            int nCount = 0;
            if (_oExportPIDetails.Count > 0)
            {
                oPdfPCell = new PdfPCell(new Phrase("P/I   Product(s) Info", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("P/I Date", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Commodity", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("U/P(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                bool bFlag = true;
                List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
                foreach (ExportPI oItem in _oDUStatement.ExportPIs)
                {
                    #region PrintDetail
                    bFlag = true;
                    oExportPIDetails = _oExportPIDetails.Where(o => o.ExportPIID == oItem.ExportPIID).ToList();
                    foreach (ExportPIDetail oExportPIDetail in oExportPIDetails)
                    {
                        _nCount++;
                        oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);
                        if (bFlag)
                        {
                            oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.Rowspan = oExportPIDetails.Count;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase(oItem.IssueDate.ToString("dd MMM yy"), _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.Rowspan = oExportPIDetails.Count;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);
                        }
                        bFlag = false;
                        oPdfPCell = new PdfPCell(new Phrase(oExportPIDetail.ProductName, _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(oExportPIDetail.MUName, _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oExportPIDetail.Qty), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oExportPIDetail.UnitPrice), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(oExportPIDetail.Qty * oExportPIDetail.UnitPrice), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPTable.CompleteRow();
                    }
                    #endregion

                }
                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 5;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);
                _nTotalQty = _oExportPIDetails.Select(c => c.Qty).Sum();
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
                _nTotalAmount = _oExportPIDetails.Select(c => c.Qty * c.UnitPrice).Sum();
                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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
        private void SetExportBillInfo()
        {
            PdfPTable oPdfPTable = new PdfPTable(8);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 35f, 100f, 70f, 70f, 70f, 85f, 70f, 70f });

            oPdfPCell = new PdfPCell(new Phrase("Invoice Information", FontFactory.GetFont("Tahoma", 10f, 3)));
            oPdfPCell.Colspan = 8;
            //oPdfPCell.Bro = 8;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            if (_oExportBills.Count > 0)
            {


                oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Invoice#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Qty(" + _oDUStatement.MUName + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Doc. Comp Dt", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Doc. Sending Dt", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Doc. Rec Dt", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Bank Sub Dt", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                int nCount = 0;
                for (int i = 0; i < _oExportBills.Count; i++)
                {
                    nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(_oExportBills[i].ExportBillNoSt, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportBills[i].Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(_oExportBills[i].Currency + " " + Global.MillionFormat(_oExportBills[i].Amount), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(_oExportBills[i].StartDateSt, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(_oExportBills[i].SendToPartySt, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(_oExportBills[i].RecdFromPartySt, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(_oExportBills[i].SendToBankDateSt, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();

                    _nTotalAmount = _nTotalAmount + _oExportBills[i].Amount;
                    _nTotalQty = _nTotalQty + _oExportBills[i].Qty;

                }
                if (nCount > 0)
                {
                    #region Export Bill Total
                    _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
                    _oPdfPCell.Colspan = 2;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + _oDUStatement.MUName, _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion
                }


            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void SetExportPIInfo()
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 25f, 90f, 210f, 65f, 45f, 60f });

            oPdfPCell = new PdfPCell(new Phrase("PI Information", FontFactory.GetFont("Tahoma", 10f, 3)));
            oPdfPCell.Colspan = 8;
            //oPdfPCell.Bro = 8;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _nTotalAmount = 0;
            _nTotalQty = 0;
            string sTemp = "";
            if (_oExportPIDetails.Count > 0)
            {


                oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("PI No#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Product", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Qty(" + _oExportPIDetails[0].MUName + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("U/P(" + _oExportPIDetails[0].Currency + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPTable.CompleteRow();


                if (_oDUStatement.BusinessUnitType != EnumBusinessUnitType.Spinning)
                {
                    _oExportPIDetails.ForEach(o => o.ProductDescription = "");
                }
                _oExportPIDetails = _oExportPIDetails.GroupBy(x => new { x.ExportPIID, x.PINo, x.ProductID, x.ProductName, x.FabricNo, x.Construction, x.ProcessType, x.ProcessTypeName, x.FinishTypeName, x.FabricWeaveName, x.UnitPrice, x.MUName, x.ProductDescription }, (key, grp) =>
                                     new ExportPIDetail
                                     {
                                         ProductID = key.ProductID,
                                         ProcessType = key.ProcessType,
                                         ProductName = key.ProductName,
                                         ExportPIID = key.ExportPIID,
                                         MUName = key.MUName,
                                         Construction = key.Construction,
                                         PINo = key.PINo,
                                         FabricNo = key.FabricNo,
                                         ProcessTypeName = key.ProcessTypeName,
                                         FabricWeaveName = key.FabricWeaveName,
                                         FinishTypeName = key.FinishTypeName,
                                         UnitPrice = key.UnitPrice,
                                         Qty = grp.Sum(p => p.Qty),
                                         Amount = grp.Sum(p => p.Amount),
                                         ProductDescription = key.ProductDescription,

                                     }).ToList();


                _nCount = 0;
                for (int i = 0; i < _oExportPIDetails.Count; i++)
                {
                    _nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(_oExportPIDetails[i].PINo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    sTemp = _oExportPIDetails[i].ProductName;
                    if (!String.IsNullOrEmpty(_oExportPIDetails[i].FabricNo))
                    {
                        sTemp = sTemp + "" + _oExportPIDetails[i].FabricNo;
                    }
                    if (!String.IsNullOrEmpty(_oExportPIDetails[i].Construction))
                    {
                        sTemp = sTemp + " Con: " + _oExportPIDetails[i].Construction;
                    }
                    if (!String.IsNullOrEmpty(_oExportPIDetails[i].ProcessTypeName))
                    {
                        sTemp = sTemp + "," + _oExportPIDetails[i].ProcessTypeName;
                    }
                    if (!String.IsNullOrEmpty(_oExportPIDetails[i].FabricWeaveName))
                    {
                        sTemp = sTemp + "," + _oExportPIDetails[i].FabricWeaveName;
                    }

                    if (!String.IsNullOrEmpty(_oExportPIDetails[i].ProductDescription))
                    {
                        sTemp = sTemp + ", " + _oExportPIDetails[i].ProductDescription;
                    }

                    oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportPIDetails[i].Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportPIDetails[i].UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportPIDetails[i].Qty * _oExportPIDetails[i].UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    _nTotalAmount = _nTotalAmount + _oExportPIDetails[i].Qty * _oExportPIDetails[i].UnitPrice;
                    _nTotalQty = _nTotalQty + _oExportPIDetails[i].Qty;

                }
                if (_nCount > 1)
                {
                    #region Export Bill Total
                    _oPdfPCell = new PdfPCell(new Phrase("Total ", _oFontStyleBold));
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

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportPIDetails[0].Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                }


            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void SetExportSC()
        {
            double nTempQty = 0;
            _nTotalAmount = 0;
            _nTotalQty = 0;
            int nProductID = 0;
            //OrderBy(o => o.ProductID).ToList();
            _oExportSCDetailDO = _oExportSCDetailDO.OrderBy(o => o.ProductID).ToList();

            PdfPTable oPdfPTable = new PdfPTable(12);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 25f, 50f, 40f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f });
            if (_oExportPIDetails.Count > 0)
            {

                oPdfPCell = new PdfPCell(new Phrase("Convrted Product(s)", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 12;
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();


                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
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

                oPdfPCell = new PdfPCell(new Phrase(" Qty(" + _oDUStatement.MUName + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" U/P(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Claim Qty", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("BPO Qty", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPCell = new PdfPCell(new Phrase("YetTo(" + _oDUStatement.MUName + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("DO Qty", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPCell = new PdfPCell(new Phrase("YetTo(" + _oDUStatement.MUName + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("DC Qty", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPCell = new PdfPCell(new Phrase("YetTo(" + _oDUStatement.MUName + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("RC(" + _oDUStatement.MUName + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                _nCount = 0;

                foreach (ExportSCDetailDO oItem in _oExportSCDetailDO)
                {
                    #region PrintDetail

                    if (nProductID != oItem.ProductID)
                    {
                        _nCount++;
                        oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString() + ". Yarn: [" + oItem.ProductCode + "]" + oItem.ProductName, _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.Colspan = 12;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);
                        oPdfPTable.CompleteRow();
                    }


                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);


                    //2
                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);
                    //3
                    oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    //4
                    oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(oItem.Qty * oItem.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    //5 Claim Qty

                    nTempQty = (_oDUStatement.DUClaimOrderDetails.Where(c => c.ExportSCDetailID == oItem.ExportSCDetailID).Sum(x => x.Qty));

                    oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(nTempQty), _oFontStyle)); //_nTotalClaim
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);
                    _nTotalClaim = _nTotalClaim + nTempQty;

                    if (oItem.POQty <= 0)
                    {
                        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    }
                    else if (oItem.POQty > 0)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.POQty), _oFontStyle));
                    }
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.YetToPO), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);



                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DOQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.YetToDO), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    nTempQty = (_oDUStatement.DUDeliveryOrderDetails.Where(c => c.ProductID == oItem.ProductID).Sum(x => x.Qty_DC));
                    //nTempQty = nTempQty+(_oDUStatement.DUDeliveryOrderDetails_Claim.Where(c => c.ProductID == oItem.ProductID).Sum(x => x.Qty_DC));

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty - nTempQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    nTempQty = (_oDUStatement.DUReturnChallanDetails.Where(c => c.ProductID == oItem.ProductID).Sum(x => x.Qty));
                    _nTotalQty_RC = _nTotalQty_RC + nTempQty;
                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    _nTotalQty = _nTotalQty + oItem.Qty;
                    _nTotalAmount = _nTotalAmount + (oItem.Qty * oItem.UnitPrice);
                    _nTotalPOQty = _nTotalPOQty + oItem.POQty;
                    _nTotalDOQty = _nTotalDOQty + oItem.DOQty;

                    oPdfPTable.CompleteRow();
                    #endregion
                    nProductID = oItem.ProductID;
                }
                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("SUM", _oFontStyleBold));
                //_oPdfPCell.Colspan = 3;
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oDUStatement.Qty_Claim), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalPOQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty - _nTotalPOQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalDOQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty - _nTotalDOQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oDUStatement.Qty_DC), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty + _oDUStatement.Qty_Claim - _oDUStatement.Qty_DC), _oFontStyleBold));
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

            if (_oDUStatement.DUClaimOrderDetails.Count > 0)
            {
                _nTotalAmount = 0;
                _nTotalQty = 0;
                _nGrandTotalAmount = 0;
                _nGrandTotalQty = 0;

                oPdfPCell = new PdfPCell(new Phrase("Claim Order Info", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                _oDUDeliveryOrderDetails = _oDUDeliveryOrderDetails.OrderBy(o => o.ProductID).ToList();
                foreach (DUClaimOrderDetail oCODetail in _oDUStatement.DUClaimOrderDetails)
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oDUStatement.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        //oPdfPCell = new PdfPCell(new Phrase("U/P(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //oPdfPTable.AddCell(oPdfPCell);


                        //oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("DO Qty  ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Due DO ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Delivery Qty  ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Due D/C ", _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
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
            if (_oDyeingOrderReports.Count > 0)
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

                _oDyeingOrderReports = _oDyeingOrderReports.OrderBy(o => o.ProductID).ToList();
                foreach (DyeingOrderReport oDyeingOrderDetail in _oDyeingOrderReports)
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                        oPdfPCell = new PdfPCell(new Phrase("Yarn Type: " + oDyeingOrderDetail.ProductName, _oFontStyleBold));
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

                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oDUStatement.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty) + "" + _oDUStatement.MUName, _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
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
        private void SetDyeingOrderStatus() 
        {
            if (_oDUStatement.LotParents.Count > 0) 
            {
                var oDyeingOrderReports_Grp = _oDyeingOrderReports.GroupBy(x => new { x.DyeingOrderID, x.OrderNoFull }, (key, grp) =>
                                     new
                                     {
                                         DyeingOrderID = key.DyeingOrderID,
                                         OrderNo = key.OrderNoFull,
                                         DyeingOrderReports = grp
                                     }).ToList();

                double GT_Transfer_In = 0,
                       GT_Transfer_Out = 0;

                foreach (var oOrder in oDyeingOrderReports_Grp)
                {
                    List<LotParent> oLotParents_In = new List<LotParent>();
                    List<LotParent> oLotParents_Out = new List<LotParent>();
                    List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
                    List<DURequisitionDetail> oDURequisitionDetails_SRM = new List<DURequisitionDetail>();
                    List<DURequisitionDetail> oDURequisitionDetails_SRS = new List<DURequisitionDetail>();
                    List<DUProGuideLineDetail> DUProGuideLineDetails_Receive = new List<DUProGuideLineDetail>();
                    List<DUProGuideLineDetail> DUProGuideLineDetails_Return = new List<DUProGuideLineDetail>();

                    oLotParents_In = _oDUStatement.LotParents.Where(x => x.DyeingOrderID == oOrder.DyeingOrderID).ToList();
                    oLotParents_Out = _oDUStatement.LotParents.Where(x => x.DyeingOrderID_Out == oOrder.DyeingOrderID).ToList();
                    oDyeingOrderReports = oOrder.DyeingOrderReports.ToList();
                    oDURequisitionDetails_SRM = _oDUStatement.DURequisitionDetails.Where(x => x.RequisitionType == EnumInOutType.Disburse && x.DyeingOrderID == oOrder.DyeingOrderID).ToList();
                    oDURequisitionDetails_SRS = _oDUStatement.DURequisitionDetails.Where(x => x.RequisitionType == EnumInOutType.Receive && x.DyeingOrderID == oOrder.DyeingOrderID).ToList();
                    DUProGuideLineDetails_Return = _oDUStatement.DUProGuideLineDetails_Return.Where(x => x.DyeingOrderID == oOrder.DyeingOrderID).ToList();
                    DUProGuideLineDetails_Receive = _oDUStatement.DUProGuideLineDetails_Receive.Where(x => x.DyeingOrderID == oOrder.DyeingOrderID).ToList();

                    #region Summary Print V2
                    PdfPTable oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 25.5f, 125.5f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f }); //130

                    _oPdfPCell = new PdfPCell(new Phrase("Party Goods Receive Notes (" + oOrder.OrderNo + ")", FontFactory.GetFont("Tahoma", 9f, 3)));
                    _oPdfPCell.Colspan = 12;
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();

                    #region Header
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

                    #region 1st Row

                    _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Commodity", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Yarn Lot", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Received Status", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 4;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Issued Status", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 3;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Balance", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    #endregion

                    #region 2nd Row
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase("GRN Qty", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("SRM Qty", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Transfer In", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    //DUE, Y-Lot, I-S/W,T-OUT, RPQ, SB

                    _oPdfPCell = new PdfPCell(new Phrase("Received Due", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Issued to S/W", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Transfer Out", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Return Qty", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    #endregion

                    #endregion

                      _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                    int nProductID = 0, nLotID = 0, nCount = 0;
                    double _nQTy_Total_DUE = 0;

                    var oProducts = _oDUStatement.LotParents.Where(p => p.DyeingOrderID == oOrder.DyeingOrderID || p.DyeingOrderID_Out == oOrder.DyeingOrderID).GroupBy(x => new { x.DyeingOrderID, x.ProductID, x.ProductName }, (key, grp) =>
                                  new
                                  {
                                      DyeingOrderID = key.DyeingOrderID,
                                      ProductID = key.ProductID,
                                      ProductName = key.ProductName
                                  }).ToList();

                    foreach (var oItem in oProducts.OrderBy(x => x.ProductID))
                    {
                        if (oItem.ProductID != nProductID)
                        {
                            if (nCount > 1)
                            {
                                #region SUB TOTAL
                                _oPdfPCell = new PdfPCell(new Phrase("Sub Total", _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Colspan = 3;
                                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                                double nQty_Order = oDyeingOrderReports.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Order), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                                double nQty_GRN = DUProGuideLineDetails_Receive.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                                double nQty_SRM = oDURequisitionDetails_SRM.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                                double nQty_T_In = oLotParents_In.Where(x => x.ProductID == nProductID && x.DyeingOrderID_Out != 0).Sum(x => x.Qty);
                                double nQty_T_Out = oLotParents_Out.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                                double nQty_SW = oDURequisitionDetails_SRS.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                                double nQty_Return = DUProGuideLineDetails_Return.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);

                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_GRN), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SRM), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_In), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                                //DUE
                                double nBalance_DUE = nQty_Order - nQty_GRN - nQty_T_In;
                                nBalance_DUE = nBalance_DUE < 0 ? 0 : nBalance_DUE;

                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nBalance_DUE), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                                //I-S/W
                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SW), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                                //T-OUT
                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_Out), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                                //RETURN
                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Return), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                                //BALANCE
                                double nBalance_Store_SubTotal = (nQty_GRN + nQty_SRM + nQty_T_In) - nQty_SW - nQty_T_Out - nQty_Return;
                                nBalance_Store_SubTotal = nBalance_Store_SubTotal < 0 ? 0 : nBalance_Store_SubTotal;

                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nBalance_Store_SubTotal), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                                #endregion
                            }

                            #region Lot Wise Status

                            #region Group By Lot Wise
                            var dataGrpList = _oDUStatement.LotParents.Where(x => x.ProductID == oItem.ProductID).GroupBy(x => new { LotID = x.LotID }, (key, grp) => new LotParent
                            {
                                LotID = key.LotID,
                                LotNo = grp.First().LotNo,
                                ProductID = grp.First().ProductID,
                                ProductName = grp.First().ProductName,
                                DyeingOrderID = grp.First().DyeingOrderID,
                                DyeingOrderID_Out = grp.First().DyeingOrderID_Out,
                                Qty = grp.Sum(x => x.Qty)
                            });
                            #endregion

                            nCount = 0;
                            foreach (var oLotParent in dataGrpList.OrderBy(x => x.LotID))
                            {
                              
                                    #region DATA
                                    nCount++;
                                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    //Y-Lot
                                    _oPdfPCell = new PdfPCell(new Phrase(oLotParent.LotNo, _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    if (oItem.ProductID != nProductID)
                                    {
                                        double nQty_Order = oDyeingOrderReports.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty);
                                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Order), _oFontStyle));
                                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        _oPdfPCell.Rowspan = dataGrpList.Where(x => x.ProductID == oItem.ProductID).Count();
                                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                                    }

                                    double nBalance_Store = 0;
                                    double nQty_GRN = DUProGuideLineDetails_Receive.Where(x => x.ProductID == oItem.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty);
                                    double nQty_SRM = oDURequisitionDetails_SRM.Where(x => x.ProductID == oItem.ProductID && x.DestinationLotID == oLotParent.LotID).Sum(x => x.Qty);
                                    double nQty_T_In = oLotParents_In.Where(x => x.ProductID == oItem.ProductID && x.DyeingOrderID_Out != 0 && x.LotID == oLotParent.LotID).Sum(x => x.Qty);
                                    double nQty_T_Out = oLotParents_Out.Where(x => x.ProductID == oItem.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty);
                                    double nQty_SW = oDURequisitionDetails_SRS.Where(x => x.ProductID == oItem.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty);
                                    double nQty_Return = DUProGuideLineDetails_Return.Where(x => x.ProductID == oItem.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty);
                                    GT_Transfer_In  += nQty_T_In;
                                    GT_Transfer_Out += nQty_T_Out;

                                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_GRN), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SRM), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_In), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    #region BALANCE
                                    //DUE
                                    if (oItem.ProductID != nProductID)
                                    {
                                        double nQty_Order_Product = oDyeingOrderReports.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty);
                                        double nQty_Rcv = DUProGuideLineDetails_Receive.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty); //&& x.LotID == oLotParent.LotID
                                        double nQty_Rcv_In = oLotParents_In.Where(x => x.ProductID == oItem.ProductID && x.DyeingOrderID_Out != 0).Sum(x => x.Qty); //&& x.LotID == oLotParent.LotID
                                        double nQty_Rcv_Out = oLotParents_Out.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty); //&& x.LotID == oLotParent.LotID

                                        double nQty_Party_Return = DUProGuideLineDetails_Return.Where(x => x.ProductID == oItem.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty);

                                        double nQty_RSRS = oDURequisitionDetails_SRS.Where(x => x.ProductID == oItem.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty);
                                        double nQty_RSRM = oDURequisitionDetails_SRM.Where(x => x.ProductID == oItem.ProductID && x.DestinationLotID == oLotParent.LotID).Sum(x => x.Qty);

                                        double nBalance_DUE = nQty_Order_Product - nQty_Rcv - nQty_Rcv_In;
                                        nBalance_DUE = nBalance_DUE < 0 ? 0 : nBalance_DUE;

                                        _nQTy_Total_DUE += nBalance_DUE;

                                        nBalance_Store = nQty_Rcv + nQty_RSRM + nQty_Rcv_In - nQty_RSRS - nQty_Rcv_Out - nQty_Party_Return;

                                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nBalance_DUE), _oFontStyle));
                                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        _oPdfPCell.Rowspan = dataGrpList.Where(x => x.ProductID == oItem.ProductID).Count();
                                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                                    }
                                    #endregion

                                    //I-S/W
                                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SW), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    //T-OUT
                                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_Out), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    //RETURN
                                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Return), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    //BALANCE
                                    var nLot_Balance_Store = nQty_GRN + nQty_SRM + nQty_T_In - nQty_SW - nQty_T_Out - nQty_Return;
                                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nLot_Balance_Store), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    #endregion
                                
                                nLotID = oLotParent.LotID;
                                nProductID = oItem.ProductID;
                            }
                            #endregion
                        }
                    }

                     if (nCount > 1)
                     {
                         #region SUB TOTAL
                         _oPdfPCell = new PdfPCell(new Phrase("Sub Total", _oFontStyle));
                         _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Colspan = 3;
                         _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                         double nQty_Order = oDyeingOrderReports.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                         _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Order), _oFontStyle));
                         _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                         _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                         double nQty_GRN = DUProGuideLineDetails_Receive.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                         double nQty_SRM = oDURequisitionDetails_SRM.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                         double nQty_T_In = oLotParents_In.Where(x => x.ProductID == nProductID && x.DyeingOrderID_Out != 0).Sum(x => x.Qty);
                         double nQty_T_Out = oLotParents_Out.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                         double nQty_SW = oDURequisitionDetails_SRS.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                         double nQty_Return = DUProGuideLineDetails_Return.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);

                         _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_GRN), _oFontStyle));
                         _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                         _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                         _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SRM), _oFontStyle));
                         _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                         _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                         _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_In), _oFontStyle));
                         _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                         _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                         //DUE
                         double nBalance_DUE = nQty_Order - nQty_GRN - nQty_T_In;
                         nBalance_DUE = nBalance_DUE < 0 ? 0 : nBalance_DUE;

                         _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nBalance_DUE), _oFontStyle));
                         _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                         _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                         //I-S/W
                         _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SW), _oFontStyle));
                         _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                         _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                         //T-OUT
                         _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_Out), _oFontStyle));
                         _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                         _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                         //RETURN
                         _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Return), _oFontStyle));
                         _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                         _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                         //BALANCE
                         double nBalance_Store_SubTotal = (nQty_GRN + nQty_SRM + nQty_T_In) - nQty_SW - nQty_T_Out - nQty_Return;
                         nBalance_Store_SubTotal = nBalance_Store_SubTotal < 0 ? 0 : nBalance_Store_SubTotal;

                         _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nBalance_Store_SubTotal), _oFontStyle));
                         _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                         _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                         #endregion
                     }

                    #region Grand Total
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

                    //Count, OQ, GQ, SRM-Q, T-IN,        DUE, Y-Lot, I-S/W,T-OUT, RPQ, SB
                    _oPdfPCell = new PdfPCell(new Phrase("Grand Total", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Colspan = 3;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    double _Order = _oDyeingOrderReports.Sum(x => x.Qty);
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_Order), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    double _nQty_GRN = _oDUStatement.DUProGuideLineDetails_Receive.Sum(x => x.Qty);
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty_GRN), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    double _nQty_SRM =  _oDUStatement.DURequisitionDetails.Where(x => x.RequisitionType == EnumInOutType.Disburse).Sum(x => x.Qty);
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty_SRM), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(GT_Transfer_In), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQTy_Total_DUE), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    double _nQty_SRS = _oDUStatement.DURequisitionDetails.Where(x => x.RequisitionType == EnumInOutType.Receive).Sum(x => x.Qty);
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty_SRS), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(GT_Transfer_Out), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    double _nQty_Return = DUProGuideLineDetails_Return.Sum(x => x.Qty);
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty_Return), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    double nGrand_Balance = _nQty_GRN + _nQty_SRM + GT_Transfer_In - GT_Transfer_Out - _nQty_SRS - _nQty_Return;
                    nGrand_Balance = nGrand_Balance <= 0 ? 0 : nGrand_Balance;

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrand_Balance), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    #endregion

                    oPdfPTable.CompleteRow();
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                }
            }
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


                oPdfPCell = new PdfPCell(new Phrase("Delivery Order Info", FontFactory.GetFont("Tahoma", 10f, 3)));
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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


                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oDUStatement.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("U/P(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Delivery Qty  ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Due D/C ", _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
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


                oPdfPCell = new PdfPCell(new Phrase("Delivery Challan Info", FontFactory.GetFont("Tahoma", 10f, 3)));
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oDUStatement.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
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
                    oDUReturnChallanDetails = _oDUStatement.DUReturnChallanDetails.Where(o => o.DUDeliveryChallanDetailID == oDCDetail.DUDeliveryChallanDetailID).ToList();
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty) + "" + _oDUStatement.MUName, _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
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

            if (_oDUStatement.DUDeliveryOrderDetails_Claim.Count > 0)
            {
                _nTotalAmount = 0;
                _nTotalQty = 0;
                _nGrandTotalAmount = 0;
                _nGrandTotalQty = 0;


                oPdfPCell = new PdfPCell(new Phrase("Claim Delivery Order Info", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                _oDUStatement.DUDeliveryOrderDetails_Claim = _oDUStatement.DUDeliveryOrderDetails_Claim.OrderBy(o => o.ProductID).ToList();
                foreach (DUDeliveryOrderDetail oDODetail in _oDUStatement.DUDeliveryOrderDetails_Claim)
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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


                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oDUStatement.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("U/P(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Delivery Qty  ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Due D/C ", _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
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
            if (_oDUStatement.DUDeliveryChallanDetails_Claim.Count > 0)
            {


                oPdfPCell = new PdfPCell(new Phrase("Claim Delivery Challan Info", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                _oDUStatement.DUDeliveryChallanDetails_Claim = _oDUStatement.DUDeliveryChallanDetails_Claim.OrderBy(o => o.ProductID).ToList();
                foreach (DUDeliveryChallanDetail oDCDetail in _oDUStatement.DUDeliveryChallanDetails_Claim)
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oDUStatement.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oDUStatement.Currency + ")", _oFontStyleBold));
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
                    oDUReturnChallanDetails = _oDUStatement.DUReturnChallanDetails_Claim.Where(o => o.DUDeliveryChallanDetailID == oDCDetail.DUDeliveryChallanDetailID).ToList();
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty) + "" + _oDUStatement.MUName, _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
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
        private void SetDeliveryChallan_Fabric()
        {
            _nTotalAmount = 0;
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(8);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 30f, 75f, 72f, 186f, 70f, 50f, 70f, 70f });
            int nDOID = 0;


            if (_oFabricDeliveryChallans.Count > 0)
            {
                string sTemp = "";
                oPdfPCell = new PdfPCell(new Phrase("Delivery Information", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                _oFabricDeliveryChallans = _oFabricDeliveryChallans.OrderBy(o => o.FDOID).ToList();
                foreach (FabricDeliveryChallan oChallan in _oFabricDeliveryChallans)
                {

                    if (nDOID != oChallan.FDOID)
                    {
                        #region Header

                        if (nDOID > 0)
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIDetails[0].Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            oPdfPTable.CompleteRow();
                            #endregion
                        }

                        oPdfPCell = new PdfPCell(new Phrase(oChallan.FabricDONo, _oFontStyleBold));
                        oPdfPCell.Colspan = 8;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);
                        oPdfPTable.CompleteRow();

                        oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Challan Date", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Count", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oDUStatement.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oDUStatement.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPTable.CompleteRow();
                        #endregion


                        _nTotalQty = 0;
                        _nTotalAmount = 0;
                        _nCount = 0;
                    }
                    #region PrintDetail
                    List<FabricDeliveryChallanDetail> oFabricDCDetails = new List<FabricDeliveryChallanDetail>();
                    oFabricDCDetails = _oFabricDeliveryChallanDetails.Where(o => o.FDCID == oChallan.FDCID).ToList();
                    if (oFabricDCDetails.Any())
                    {
                        int nCount = 0;
                      //  var oresults = oFabricDCDetails.GroupBy(x => x.FEONo).Select(g => new { FEONo = g.Key, StyleNo = g.FirstOrDefault().StyleNo, ColorInfo = g.FirstOrDefault().ColorInfo, GoodsDesp = GetGoodsDescription(g.FirstOrDefault()), TotalRoll = g.Count(), YetToDeliveryChallanQty = g.Sum(p => (p.Qty * p.UnitPrice)), Qty = g.Sum(p => p.Qty) });

                        var oFDCDetails = oFabricDCDetails.GroupBy(x => new { x.FabricNo, x.ProductID, x.ExeNo, x.ColorInfo, x.ProductName, x.StyleNo, x.FabricWeave, x.Construction, x.FinishWidth, x.MUName, x.ProcessTypeName, x.FinishTypeName }, (key, grp) =>
                                       new
                                       {
                                           ProductID = key.ProductID,
                                           ProductName = key.ProductName,
                                           StyleNo = key.StyleNo,
                                           ColorInfo = key.ColorInfo,
                                           //Color_Grp = grp.GroupBy(p => new { p.ExeNo, p.StyleNo, p.BuyerRef, p.ColorInfo }, (color_key, color_grp) => new
                                           //{
                                           //    StyleNo = color_key.StyleNo,
                                           //    BuyerRef = color_key.BuyerRef,
                                           //    ExeNo = color_key.ExeNo,
                                           //    ColorInfo = color_key.ColorInfo,
                                           //    Qty = color_grp.Sum(p => p.Qty),
                                           //    NoRoll = color_grp.Count(),
                                           //    Amount = color_grp.Sum(p => (p.Qty * p.UnitPrice))
                                           //}).ToList(),
                                           Construction = key.Construction,
                                           FabricNo = key.FabricNo,
                                           FinishWidth = key.FinishWidth,
                                           FabricWeave = key.FabricWeave,
                                           FinishTypeName = key.FinishTypeName,// grp.Select(x => x.FinishType).FirstOrDefault(),
                                           Qty = grp.Sum(p => p.Qty),
                                           Amount = grp.Sum(p => p.Qty*p.UnitPrice),
                                           NoRoll = grp.Count(),
                                           MUName = key.MUName,
                                           ProcessTypeName = key.ProcessTypeName,
                                           ExeNo = key.ExeNo,
                                           //Weight = key.Weight
                                       }).ToList();


                        foreach (var oItem in oFDCDetails)
                        {
                            _nCount++;
                            oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase(oChallan.IssueDateSt, _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase(oChallan.ChallanNo, _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            oPdfPTable.AddCell(oPdfPCell);

                            sTemp = "";
                            if (!String.IsNullOrEmpty(oItem.FabricNo))
                            {
                                sTemp = sTemp + "" + oItem.FabricNo; //+"\n" + oItem.ProductName;
                            }
                            if (!String.IsNullOrEmpty(oItem.FabricWeave))
                            {
                                sTemp = sTemp + "Comp: " + oItem.ProductName + ", Weave: " + oItem.FabricWeave;
                            }
                            else
                            { sTemp = sTemp + "Comp:" + oItem.ProductName; }

                            if (!string.IsNullOrEmpty(oItem.Construction))
                            {
                                sTemp += " \nConst: " + oItem.Construction;
                            }
                            if (!string.IsNullOrEmpty(oItem.FinishWidth))
                            {
                                sTemp += " \nWidth : " + oItem.FinishWidth;
                            }
                            //if (!string.IsNullOrEmpty(oItem.Weight))
                            //{
                            //    sTemp += " Weight : " + oItem.Weight;
                            //}
                            if (!string.IsNullOrEmpty(oItem.ProcessTypeName))
                            {
                                sTemp += "\nProcess: " + oItem.ProcessTypeName;
                            }
                            if (!string.IsNullOrEmpty(oItem.FinishTypeName))
                            {
                                sTemp += ", Finish: " + oItem.FinishTypeName;
                            }
                            if (!string.IsNullOrEmpty(oItem.ColorInfo))
                            {
                                sTemp += ", Color: " + oItem.ColorInfo;
                            }
                            if (!string.IsNullOrEmpty(oItem.StyleNo))
                            {
                                sTemp += ", Style No: " + oItem.StyleNo;
                            }

                            oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount / oItem.Qty), _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase(oItem.ExeNo, _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            _nTotalAmount = _nTotalAmount + (oItem.Amount);
                            _nTotalQty = _nTotalQty + oItem.Qty;
                            _nGrandTotalAmount = _nGrandTotalAmount + (oItem.Amount);
                            _nGrandTotalQty = _nGrandTotalQty + oItem.Qty;


                        }
                    }
                    #endregion

                    nDOID = oChallan.FDOID;

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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(_oDUStatement.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
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
        #endregion

        #region
        private void SetDyeingOrderTwo()
        {
            var fontBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            var fontNormal = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC);
            var oFDCDetails = _oDyeingOrderReports.GroupBy(x => new { x.DyeingOrderID, x.OrderDateSt, x.OrderNoFull, x.SampleInvoiceNo }, (key, grp) =>
                                  new
                                  {
                                      DyeingOrderID = key.DyeingOrderID,
                                      OrderDateSt = key.OrderDateSt,
                                      OrderNoFull = key.OrderNoFull,
                                      SampleInvoiceNo = key.SampleInvoiceNo,

                                  }).ToList();
           // ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Greige Fabric Detail", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Production Order Detail", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 5f, _oFontStyleBold, true);
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 120f, 100f, 80f, 80f, 65f,50f });

            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Production Order Detail", 0, 6, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
           
            oPdfPTable.CompleteRow();

         
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion
            foreach (var oItem in oFDCDetails)
            {
                var oDyeingOrders = _oDyeingOrderReports.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).ToList();
                #region Fabric Info
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 150f, 110f, 80f, 60f,42f, 50f });
                if (oFDCDetails.Count > 0)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No:" + oItem.OrderNoFull, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date:" + oItem.OrderDateSt, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable,  "Bill/Invoice No:"+oItem.SampleInvoiceNo, 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    oPdfPTable.CompleteRow();
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Type", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shade", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty " + "(" + _oDUStatement.MUName + ")", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "U.P.", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Value", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                #endregion

                foreach (var oItem1 in oDyeingOrders)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ColorName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.RGB, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Qty), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatActualDigit(oItem1.UnitPrice), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable,oItem1.Currency+""+ Global.MillionFormat(oItem1.Qty * oItem1.UnitPrice), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    oPdfPTable.CompleteRow();
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTotalQty = oDyeingOrders.Select(c => c.Qty).Sum();
                _nTotalAmount= oDyeingOrders.Select(c => (c.Qty*c.UnitPrice)).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable,_oDUStatement.Currency+""+ Global.MillionFormat(_nTotalAmount), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
           
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
