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

    public class rptKnittingOrderRegisters
    {
        #region Declaration
        int count, num;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        //PdfPTable _oPdfPTableDetail = new PdfPTable(5);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        KnittingOrderRegister _oKnittingOrderRegister = new KnittingOrderRegister();
        List<KnittingOrderRegister> _oKnittingOrderRegisters = new List<KnittingOrderRegister>();
        Company _oCompany = new Company();
        double SubTotalStyleQty = 0, SubTotalFabricQty = 0, SubTotalFabricAmount = 0, SubTotalFabricRcvQty = 0, SubTotalFabricYetToRcvQty = 0, SubTotalYarnQty = 0, SubTotalYarnBalance = 0, SubTotalYarnConsumptionQty = 0, SubTotalYarnReturnQty = 0, SubTotalYarnProcessLossQty = 0;
        double GrandTotalStyleQty = 0, GrandTotalFabricQty = 0, GrandTotalFabricAmount = 0, GrandTotalFabricRcvQty = 0, GrandTotalFabricYetToRcvQty = 0, GrandTotalYarnQty = 0, GrandTotalYarnBalance = 0, GrandTotalYarnConsumptionQty = 0, GrandTotalYarnReturnQty = 0, GrandTotalYarnProcessLossQty = 0;        
        string sKnittingOrderNo = "", sStyleNo = "";
        string sReportHeader = "";
        EnumReportLayout _eReportLayout = EnumReportLayout.None;
        string _sDateRange = "";
        #endregion

        public byte[] PrepareReport(List<KnittingOrderRegister> oKnittingOrderRegisters, Company oCompany, EnumReportLayout eReportLayout, string sDateRange)
        {
            _oKnittingOrderRegisters = oKnittingOrderRegisters;
            _oCompany = oCompany;
            _eReportLayout = eReportLayout;
            _sDateRange = sDateRange;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.LEGAL.Rotate());
            _oDocument.SetMargins(25f, 25f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);

            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            ESimSolFooter PageEventHandler = new ESimSolFooter();

            //PageEventHandler.signatures = signatureList;
            PageEventHandler.nFontSize = 9;
            PdfWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 1008f });
            //_oPdfPTableDetail.SetWidths(new float[] { 25f, 100f, 80f, 80f, 80f});
            #endregion

            if (eReportLayout == EnumReportLayout.Order_Status_Wise)
                sReportHeader = "Order Status Wise Knitting Order";
            else if (eReportLayout == EnumReportLayout.DateWise)
                sReportHeader = "Knitting Order Register (Date Wise)";
            else if (eReportLayout == EnumReportLayout.PartyWise)
                sReportHeader = "Knitting Order Register (Party Wise)";
            else if (eReportLayout == EnumReportLayout.Style_Wise)
                sReportHeader = "Knitting Order Register (Style Wise)";
            
            this.PrintHeader(sReportHeader);
            this.PrintBody();            
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(string sReportHeader)
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

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_sDateRange, _oFontStyle));
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
        private void PrintBody()
        {
            if (_eReportLayout == EnumReportLayout.Order_Status_Wise)
            {
                this.GetOrderStatusWiseTable(_oKnittingOrderRegisters);
                _oPdfPTable.HeaderRows = 5;
            }
            else if (_eReportLayout == EnumReportLayout.DateWise)
            {
                _oPdfPCell = new PdfPCell(GetDateWiseTable(_oKnittingOrderRegisters)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; //_oPdfPCell.FixedHeight = 150;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.HeaderRows = 3;
            }
            else if (_eReportLayout == EnumReportLayout.PartyWise)
            {
                _oPdfPCell = new PdfPCell(GetPartyWiseTable(_oKnittingOrderRegisters)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; //_oPdfPCell.FixedHeight = 150;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.HeaderRows = 3;
            }
            else if (_eReportLayout == EnumReportLayout.Style_Wise)
            {
                //_oPdfPCell = new PdfPCell(GetStyleWiseTable(_oKnittingOrderRegisters)); _oPdfPCell.Border = 0;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; //_oPdfPCell.FixedHeight = 150;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.HeaderRows = 2;

                this.GetStyleWiseTable(_oKnittingOrderRegisters);
                _oPdfPTable.HeaderRows = 4;
            }
        }
        #endregion

        #region order status wise
        private PdfPTable TableInitialize()
        {
            #region Initialize table detail
            PdfPTable oOrderStatusWiseTable = new PdfPTable(21);
            oOrderStatusWiseTable.WidthPercentage = 100;
            oOrderStatusWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oOrderStatusWiseTable.SetWidths(new float[] {                                                         
                                                                60f,    //KO no/Order dt
                                                                60f,    //Business session/factory
                                                                60f,    //start dt/approx dt
                                                                
                                                                60f,    //style no
                                                                60f,     //buyer
                                                                50f,    //style qty
                                                                70f,     //Fabric
                                                                50f,    //color
                                                                40f,    //MUint
                                                                50f,    //Qty
                                                                40f,    //U price
                                                                50f,    //amount
                                                                50f,    //Rcv Qty
                                                                50f,    //yet to Qty

                                                                70f,    //Yarn Name
                                                                40f,     //MUnit
                                                                //40f,     //Unit price
                                                                50f,    //Qty
                                                                50f,    //Consuption Qty
                                                                50f,    //Return qty
                                                                50f,    //process loss
                                                                50f    //Balance
                                                                
                                                          });
            #endregion
            return oOrderStatusWiseTable;
        }

        private void GetOrderStatusWiseTable(List<KnittingOrderRegister> oKnittingOrderRegisters)
        {
            PdfPTable oOrderStatusWiseTable = null;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            oOrderStatusWiseTable = this.TableInitialize();

            #region top header
            _oPdfPCell = new PdfPCell(new Phrase("Knittiong Order Info", _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Fabric Receive Info", _oFontStyle)); _oPdfPCell.Colspan = 11;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Yarn Challan Info", _oFontStyle)); _oPdfPCell.Colspan = 8;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            oOrderStatusWiseTable.CompleteRow();
            #endregion

            #region header
            _oPdfPCell = new PdfPCell(new Phrase("KO No/ Order Dt", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Session/ Factory", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Start Dt/ Approx Dt", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Fabric", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M. Unit", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("U. Price", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Rcv Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Due Rcv", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Yarn", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M. Unit", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("U. Price", _oFontStyle));
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Consumption Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Loose Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Process Loss", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Balance", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderStatusWiseTable.AddCell(_oPdfPCell);
            oOrderStatusWiseTable.CompleteRow();
            #endregion

            #region Inseret Into Main Table
            _oPdfPCell = new PdfPCell(oOrderStatusWiseTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.6f, iTextSharp.text.Font.NORMAL);
            if (oKnittingOrderRegisters != null && oKnittingOrderRegisters.Count > 0)
            {                
                int nKnittingOrderID = 0, nKnittingOrderDetailID = 0;
                GrandTotalStyleQty = 0; GrandTotalFabricQty = 0; GrandTotalFabricAmount = 0; GrandTotalFabricRcvQty = 0; GrandTotalFabricYetToRcvQty = 0; GrandTotalYarnQty = 0; GrandTotalYarnBalance = 0; GrandTotalYarnConsumptionQty = 0; GrandTotalYarnReturnQty = 0; GrandTotalYarnProcessLossQty = 0;
                oOrderStatusWiseTable = this.TableInitialize();
                foreach (KnittingOrderRegister oItem in oKnittingOrderRegisters)
                {
                    if (oItem.KnittingOrderID != nKnittingOrderID)
                    {
                        #region Inseret Into Main Table
                        if (oOrderStatusWiseTable.Rows != null && oOrderStatusWiseTable.Rows.Count > 0)
                        {
                            _oPdfPCell = new PdfPCell(oOrderStatusWiseTable);
                            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        #endregion

                        if (SubTotalStyleQty > 0)
                        {
                            oOrderStatusWiseTable = this.TableInitialize();
                            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                            _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 5;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(SubTotalStyleQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 3;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(SubTotalFabricQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(SubTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(SubTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(SubTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 2;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(SubTotalYarnQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(SubTotalYarnConsumptionQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(SubTotalYarnReturnQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(SubTotalYarnProcessLossQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(SubTotalYarnBalance.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);
                            oOrderStatusWiseTable.CompleteRow();

                            #region Inseret Into Main Table
                            _oPdfPCell = new PdfPCell(oOrderStatusWiseTable);
                            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                            #endregion

                            SubTotalStyleQty = 0; SubTotalFabricQty = 0; SubTotalFabricAmount = 0; SubTotalFabricRcvQty = 0; SubTotalFabricYetToRcvQty = 0; SubTotalYarnQty = 0; SubTotalYarnBalance = 0; SubTotalYarnConsumptionQty = 0; SubTotalYarnReturnQty = 0; SubTotalYarnProcessLossQty = 0;
                        }
                        

                        oOrderStatusWiseTable = this.TableInitialize();
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7.6f, iTextSharp.text.Font.NORMAL);
                        int rowCount = oKnittingOrderRegisters.Count(x => x.KnittingOrderID == oItem.KnittingOrderID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.KnittingOrderNo + "\n" + oItem.OrderDateInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.BusinessSessionName + "\n" + oItem.FactoryName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.StartDateInString + "\n" + oItem.ApproxCompleteDateInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);
                    }

                    if (oItem.KnittingOrderDetailID != nKnittingOrderDetailID)
                    {
                        int rowCount = oKnittingOrderRegisters.Count(x => x.KnittingOrderID == oItem.KnittingOrderID && x.KnittingOrderDetailID == oItem.KnittingOrderDetailID);
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7.6f, iTextSharp.text.Font.NORMAL);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricStyleQtyInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);
                        SubTotalStyleQty += oItem.FabricStyleQty;
                        GrandTotalStyleQty += oItem.FabricStyleQty;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricMUnitSymbol, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricQtyInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);
                        SubTotalFabricQty += oItem.FabricQty;
                        GrandTotalFabricQty += oItem.FabricQty;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricUnitPriceInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricAmountInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);
                        SubTotalFabricAmount += oItem.FabricAmount;
                        GrandTotalFabricAmount += oItem.FabricAmount;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricRecvQtyInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);
                        SubTotalFabricRcvQty += oItem.FabricRecvQty;
                        GrandTotalFabricRcvQty += oItem.FabricRecvQty;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricYetRecvQtyInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);
                        SubTotalFabricYetToRcvQty += oItem.FabricYetRecvQty;
                        GrandTotalFabricYetToRcvQty += oItem.FabricYetRecvQty;
                    }

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.YarnName, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.YarnMUnitSymbol, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.YarnChallanQtyInString, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);
                    SubTotalYarnQty += oItem.YarnChallanQty;
                    GrandTotalYarnQty += oItem.YarnChallanQty;

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.YarnConsumptionQtyInString, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);
                    SubTotalYarnConsumptionQty += oItem.YarnConsumptionQty;
                    GrandTotalYarnConsumptionQty += oItem.YarnConsumptionQty;

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.YarnReturnQtyInString, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);
                    SubTotalYarnReturnQty += oItem.YarnReturnQty;
                    GrandTotalYarnReturnQty += oItem.YarnReturnQty;

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.YarnProcessLossQtyInString, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);
                    SubTotalYarnProcessLossQty += oItem.YarnProcessLossQty;
                    GrandTotalYarnProcessLossQty += oItem.YarnProcessLossQty;

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.YarnBalanceQtyInString, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);
                    SubTotalYarnBalance += oItem.YarnBalanceQty;
                    GrandTotalYarnBalance += oItem.YarnBalanceQty;
                    oOrderStatusWiseTable.CompleteRow();

                    nKnittingOrderID = oItem.KnittingOrderID;
                    nKnittingOrderDetailID = oItem.KnittingOrderDetailID;
                }

                #region Inseret Into Main Table
                _oPdfPCell = new PdfPCell(oOrderStatusWiseTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion
                
                oOrderStatusWiseTable = this.TableInitialize();
                #region subtotal
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 5;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(SubTotalStyleQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(SubTotalFabricQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(SubTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(SubTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(SubTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(SubTotalYarnQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(SubTotalYarnConsumptionQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(SubTotalYarnReturnQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(SubTotalYarnProcessLossQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(SubTotalYarnBalance.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                oOrderStatusWiseTable.CompleteRow();
                #endregion

                #region grandtotal
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 5;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalStyleQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalYarnQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalYarnConsumptionQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalYarnReturnQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalYarnProcessLossQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalYarnBalance.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderStatusWiseTable.AddCell(_oPdfPCell);
                oOrderStatusWiseTable.CompleteRow();
                #endregion


                #region Inseret Into Main Table
                _oPdfPCell = new PdfPCell(oOrderStatusWiseTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion
            }
            #endregion
        }

        #endregion

        #region Date Wise

        private PdfPTable GetDateWiseTable(List<KnittingOrderRegister> oKnittingOrderRegisters)
        {
            #region table detail
            PdfPTable oDateWiseTable = new PdfPTable(15);
            oDateWiseTable.WidthPercentage = 100;
            oDateWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oDateWiseTable.SetWidths(new float[] {                                                         
                                                                60f,    //KO no/Order type
                                                                60f,    //Business session/factory
                                                                60f,    //start dt/approx dt                                                                
                                                                60f,    //style no
                                                                30f,    //PAM
                                                                60f,     //buyer
                                                                50f,    //style qty
                                                                70f,     //Fabric
                                                                50f,    //color
                                                                40f,    //MUint
                                                                50f,    //Qty
                                                                40f,    //U price
                                                                50f,    //amount
                                                                50f,    //Rcv Qty
                                                                50f,    //yet to Qty
                                                          });
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 15; _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);
            oDateWiseTable.CompleteRow();

            #region header

            _oPdfPCell = new PdfPCell(new Phrase("KO No/Order Type", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("B. Session/Factory", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Start Date/Approx Date", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PAM", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Fabric", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M. Unit", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("U. Price", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Receive Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Yet To Receive", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            oDateWiseTable.CompleteRow();
            #endregion

            #region data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.6f, iTextSharp.text.Font.NORMAL);

            if (oKnittingOrderRegisters.Count > 0)
            {
                var data = oKnittingOrderRegisters.GroupBy(x => new { x.OrderDateInString }, (key, grp) => new
                {
                    OrderDate = key.OrderDateInString,
                    SubTotalStyleQty = grp.Select(x=>x.FabricStyleQty).Sum(),
                    SubTotalFabricQty = grp.Select(x => x.FabricQty).Sum(),
                    SubTotalFabricAmount = grp.Select(x => x.FabricAmount).Sum(),
                    SubTotalFabricRcvQty = grp.Select(x => x.FabricRecvQty).Sum(),
                    SubTotalFabricYetToRcvQty = grp.Select(x => x.FabricQty).Sum(),
                    Results = grp.ToList() //All data
                });

                #region body
                GrandTotalStyleQty = 0; GrandTotalFabricQty = 0; GrandTotalFabricAmount = 0; GrandTotalFabricRcvQty = 0; GrandTotalFabricYetToRcvQty = 0;
                foreach (var oData in data)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Knitting Order Date : @ " + oData.OrderDate, _oFontStyle)); _oPdfPCell.Colspan = 15;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);
                    oDateWiseTable.CompleteRow();

                    count = 0; num = 0;
                    //SubTotalStyleQty = 0; SubTotalFabricQty = 0; SubTotalFabricAmount = 0; SubTotalFabricRcvQty = 0; SubTotalFabricYetToRcvQty = 0;
                    //string sKONO = "";
                    sStyleNo = ""; sKnittingOrderNo = "";
                    foreach (var oItem in oData.Results)
                    {
                        count++; 
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7.6f, iTextSharp.text.Font.NORMAL);

                        #region data
                        if (sKnittingOrderNo != oItem.KnittingOrderNo)
                        {
                            sStyleNo = ""; 

                            num++;
                            int rowCount = oData.Results.Count(x => x.KnittingOrderNo == oItem.KnittingOrderNo);
                            //_oPdfPCell = new PdfPCell(new Phrase(num.ToString(), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.KnittingOrderNo + "\n" + oItem.OrderTypeInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.BusinessSessionName + "\n" + oItem.FactoryName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.StartDateInString + "\n" + oItem.ApproxCompleteDateInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        }

                        if (sStyleNo != oItem.StyleNo)
                        {
                            int rowCount = oData.Results.Count(x => x.KnittingOrderNo == oItem.KnittingOrderNo && x.StyleNo == oItem.StyleNo);
                            _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.PAM, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricStyleQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);
                            GrandTotalStyleQty += oItem.FabricStyleQty;
                        }

                        //_oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase(oItem.PAM, _oFontStyle)); 
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle)); 
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase(oItem.FabricStyleQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);
                        //SubTotalStyleQty += oItem.FabricStyleQty;
                        //GrandTotalStyleQty += oItem.FabricStyleQty;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricName, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricMUnitSymbol, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);
                        GrandTotalFabricQty += oItem.FabricQty;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricUnitPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);
                        GrandTotalFabricAmount += oItem.FabricAmount;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricRecvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);
                        GrandTotalFabricRcvQty += oItem.FabricRecvQty;

                        _oPdfPCell = new PdfPCell(new Phrase((oItem.FabricQty - oItem.FabricRecvQty).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);
                        GrandTotalFabricYetToRcvQty += (oItem.FabricQty - oItem.FabricRecvQty);

                        #endregion

                        oDateWiseTable.CompleteRow();
                        sKnittingOrderNo = oItem.KnittingOrderNo;
                        sStyleNo = oItem.StyleNo;
                    }
                    #region subtotal
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 6;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.SubTotalStyleQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 3;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.SubTotalFabricQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.SubTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.SubTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.SubTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    oDateWiseTable.CompleteRow();
                    #endregion
                }
                #region grandtotal
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 6;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalStyleQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                oDateWiseTable.CompleteRow();
                #endregion
                #endregion

            }

            #endregion

            return oDateWiseTable;
        }

        #endregion

        #region Party Wise

        private PdfPTable GetPartyWiseTable(List<KnittingOrderRegister> oKnittingOrderRegisters)
        {
            #region table detail
            PdfPTable oPartyWiseTable = new PdfPTable(15);
            oPartyWiseTable.WidthPercentage = 100;
            oPartyWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPartyWiseTable.SetWidths(new float[] {                                                         
                                                                60f,    //KO no/Order dt
                                                                60f,    //Business session/Order type
                                                                60f,    //start dt/approx dt                                                                
                                                                60f,    //style no
                                                                30f,    //PAM
                                                                60f,     //buyer
                                                                50f,    //style qty
                                                                70f,     //Fabric
                                                                50f,    //color
                                                                40f,    //MUint
                                                                50f,    //Qty
                                                                40f,    //U price
                                                                50f,    //amount
                                                                50f,    //Rcv Qty
                                                                50f,    //yet to Qty
                                                          });
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 15; _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
            oPartyWiseTable.CompleteRow();

            #region header

            _oPdfPCell = new PdfPCell(new Phrase("KO No/Order Date", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("B. Session/Order Type", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Start Date/Approx Date", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PAM", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Fabric", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M. Unit", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("U. Price", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Receive Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Yet To Receive", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            oPartyWiseTable.CompleteRow();
            #endregion

            #region data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.6f, iTextSharp.text.Font.NORMAL);

            if (oKnittingOrderRegisters.Count > 0)
            {
                var data = oKnittingOrderRegisters.GroupBy(x => new { x.FactoryName }, (key, grp) => new
                {
                    FactoryName = key.FactoryName,
                    SubTotalStyleQty = grp.Select(x => x.FabricStyleQty).Sum(),
                    SubTotalFabricQty = grp.Select(x => x.FabricQty).Sum(),
                    SubTotalFabricAmount = grp.Select(x => x.FabricAmount).Sum(),
                    SubTotalFabricRcvQty = grp.Select(x => x.FabricRecvQty).Sum(),
                    SubTotalFabricYetToRcvQty = grp.Select(x => x.FabricQty).Sum(),
                    Results = grp.ToList() //All data
                });

                #region body
                GrandTotalStyleQty = 0; GrandTotalFabricQty = 0; GrandTotalFabricAmount = 0; GrandTotalFabricRcvQty = 0; GrandTotalFabricYetToRcvQty = 0;
                foreach (var oData in data)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Party : @ " + oData.FactoryName, _oFontStyle)); _oPdfPCell.Colspan = 15;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                    oPartyWiseTable.CompleteRow();

                    count = 0; num = 0;
                    sKnittingOrderNo = ""; sStyleNo = "";
                    foreach (var oItem in oData.Results)
                    {
                        count++; 
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7.6f, iTextSharp.text.Font.NORMAL);

                        #region data
                        if (sKnittingOrderNo != oItem.KnittingOrderNo)
                        {
                            num++; sStyleNo = "";
                            int rowCount = oData.Results.Count(x => x.KnittingOrderNo == oItem.KnittingOrderNo);
                            //_oPdfPCell = new PdfPCell(new Phrase(num.ToString(), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.KnittingOrderNo + "\n" + oItem.OrderDateInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.BusinessSessionName + "\n" + oItem.OrderTypeInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.StartDateInString + "\n" + oItem.ApproxCompleteDateInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        }

                        if (sStyleNo != oItem.StyleNo)
                        {
                            int rowCount = oData.Results.Count(x => x.KnittingOrderNo == oItem.KnittingOrderNo && x.StyleNo == oItem.StyleNo);
                            _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.PAM, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricStyleQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                            GrandTotalStyleQty += oItem.FabricStyleQty;
                        }

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricName, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricMUnitSymbol, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                        GrandTotalFabricQty += oItem.FabricQty;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricUnitPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                        GrandTotalFabricAmount += oItem.FabricAmount;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricRecvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                        GrandTotalFabricRcvQty += oItem.FabricRecvQty;

                        _oPdfPCell = new PdfPCell(new Phrase((oItem.FabricQty - oItem.FabricRecvQty).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);                        
                        GrandTotalFabricYetToRcvQty += (oItem.FabricQty - oItem.FabricRecvQty);

                        #endregion

                        oPartyWiseTable.CompleteRow();
                        sKnittingOrderNo = oItem.KnittingOrderNo;
                        sStyleNo = oItem.StyleNo;
                    }
                    #region subtotal
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 6;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.SubTotalStyleQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 3;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.SubTotalFabricQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.SubTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.SubTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.SubTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    oPartyWiseTable.CompleteRow();
                    #endregion
                }
                #region grandtotal
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 6;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalStyleQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                oPartyWiseTable.CompleteRow();
                #endregion
                #endregion

            }

            #endregion

            return oPartyWiseTable;
        }

        #endregion

        #region Style Wise

        private void GetStyleWiseTable(List<KnittingOrderRegister> oKnittingOrderRegisters)
        {
            #region table detail
            PdfPTable oStyleWiseTable = new PdfPTable(14);
            oStyleWiseTable.WidthPercentage = 100;
            oStyleWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oStyleWiseTable.SetWidths(new float[] {                                                         
                                                                60f,    //KO no/Order dt
                                                                60f,    //Business session/Factory
                                                                60f,    //start dt/approx dt                                                                
                                                                60f,    //style no
                                                                60f,     //buyer
                                                                50f,    //style qty
                                                                70f,     //Fabric
                                                                50f,    //color
                                                                40f,    //MUint
                                                                50f,    //Qty
                                                                40f,    //U price
                                                                50f,    //amount
                                                                50f,    //Rcv Qty
                                                                50f,    //yet to Qty
                                                          });
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 14; _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);
            oStyleWiseTable.CompleteRow();

            #region header

            _oPdfPCell = new PdfPCell(new Phrase("KO No/Order Date", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oStyleWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("B. Session/Factory", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oStyleWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Start Date/Approx Date", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oStyleWiseTable.AddCell(_oPdfPCell);


            //_oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle));
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oStyleWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oStyleWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oStyleWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Fabric", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oStyleWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oStyleWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M. Unit", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oStyleWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oStyleWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("U. Price", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oStyleWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oStyleWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Receive Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oStyleWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Yet To Receive", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oStyleWiseTable.AddCell(_oPdfPCell);

            oStyleWiseTable.CompleteRow();
            #endregion

            #region Inseret Into Main Table
            _oPdfPCell = new PdfPCell(oStyleWiseTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region table detail
            oStyleWiseTable = new PdfPTable(14);
            oStyleWiseTable.WidthPercentage = 100;
            oStyleWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oStyleWiseTable.SetWidths(new float[] {                                                         
                                                                60f,    //KO no/Order dt
                                                                60f,    //Business session/Factory
                                                                60f,    //start dt/approx dt                                                                
                                                                60f,    //style no
                                                                60f,     //buyer
                                                                50f,    //style qty
                                                                70f,     //Fabric
                                                                50f,    //color
                                                                40f,    //MUint
                                                                50f,    //Qty
                                                                40f,    //U price
                                                                50f,    //amount
                                                                50f,    //Rcv Qty
                                                                50f,    //yet to Qty
                                                          });
            #endregion

            #region data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.6f, iTextSharp.text.Font.NORMAL);

            if (oKnittingOrderRegisters.Count > 0)
            {
                var data = oKnittingOrderRegisters.GroupBy(x => new { x.StyleNo, x.PAM, }, (key, grp) => new
                {
                    StyleNo = key.StyleNo,
                    PAM = key.PAM,
                    SubTotalStyleQty = grp.Select(x => x.FabricStyleQty).Sum(),
                    SubTotalFabricQty = grp.Select(x => x.FabricQty).Sum(),
                    SubTotalFabricAmount = grp.Select(x => x.FabricAmount).Sum(),
                    SubTotalFabricRcvQty = grp.Select(x => x.FabricRecvQty).Sum(),
                    SubTotalFabricYetToRcvQty = grp.Select(x => x.FabricQty).Sum(),
                    Results = grp.ToList() //All data
                });

                #region body
                GrandTotalStyleQty = 0; GrandTotalFabricQty = 0; GrandTotalFabricAmount = 0; GrandTotalFabricRcvQty = 0; GrandTotalFabricYetToRcvQty = 0;
                foreach (var oData in data)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Style : @ " + oData.StyleNo + " With PAM :" + oData.PAM, _oFontStyle)); _oPdfPCell.Colspan = 14;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);
                    oStyleWiseTable.CompleteRow();

                    count = 0; num = 0;
                    sKnittingOrderNo = "";
                    foreach (var oItem in oData.Results)
                    {
                        count++; 
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7.6f, iTextSharp.text.Font.NORMAL);

                        #region data
                        if (sKnittingOrderNo != oItem.KnittingOrderNo)
                        {
                            num++;
                            int rowCount = oData.Results.Count(x => x.KnittingOrderNo == oItem.KnittingOrderNo);
                            //_oPdfPCell = new PdfPCell(new Phrase(num.ToString(), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.KnittingOrderNo + "\n" + oItem.OrderDateInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.BusinessSessionName + "\n" + oItem.FactoryName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.StartDateInString + "\n" + oItem.ApproxCompleteDateInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);
                        }

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle)); _oPdfPCell.Colspan = 2;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricStyleQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); 
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);
                        GrandTotalStyleQty += oItem.FabricStyleQty;


                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricName, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricMUnitSymbol, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);
                        GrandTotalFabricQty += oItem.FabricQty;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricUnitPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);
                        GrandTotalFabricAmount += oItem.FabricAmount;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricRecvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);
                        SubTotalFabricRcvQty += oItem.FabricRecvQty;
                        GrandTotalFabricRcvQty += oItem.FabricRecvQty;

                        _oPdfPCell = new PdfPCell(new Phrase((oItem.FabricQty - oItem.FabricRecvQty).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);
                        GrandTotalFabricYetToRcvQty += (oItem.FabricQty - oItem.FabricRecvQty);

                        #endregion

                        oStyleWiseTable.CompleteRow();
                        sKnittingOrderNo = oItem.KnittingOrderNo;
                        sStyleNo = oItem.StyleNo;
                    }
                    #region Inseret Into Main Table
                    _oPdfPCell = new PdfPCell(oStyleWiseTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    #endregion

                    #region table detail
                    oStyleWiseTable = new PdfPTable(14);
                    oStyleWiseTable.WidthPercentage = 100;
                    oStyleWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oStyleWiseTable.SetWidths(new float[] {                                                         
                                                                60f,    //KO no/Order dt
                                                                60f,    //Business session/Factory
                                                                60f,    //start dt/approx dt                                                                
                                                                60f,    //style no
                                                                60f,     //buyer
                                                                50f,    //style qty
                                                                70f,     //Fabric
                                                                50f,    //color
                                                                40f,    //MUint
                                                                50f,    //Qty
                                                                40f,    //U price
                                                                50f,    //amount
                                                                50f,    //Rcv Qty
                                                                50f,    //yet to Qty
                                                          });
                    #endregion
                }
                #region grandtotal
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 5;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalStyleQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalFabricYetToRcvQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStyleWiseTable.AddCell(_oPdfPCell);

                oStyleWiseTable.CompleteRow();
                #endregion
                #endregion
                #region Inseret Into Main Table
                _oPdfPCell = new PdfPCell(oStyleWiseTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion
            }

            #endregion

            //return oStyleWiseTable;
        }

        #endregion

    }
}
