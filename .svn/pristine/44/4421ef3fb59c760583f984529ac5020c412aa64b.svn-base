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
    public class rptFabricReceiveRegister
    {
        #region Declaration
        int count, num;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        FabricReceiveRegister _oFabricReceiveRegister = new FabricReceiveRegister();
        List<FabricReceiveRegister> _oFabricReceiveRegisters = new List<FabricReceiveRegister>();
        Company _oCompany = new Company();
        double SubTotalQty = 0, SubtotalProcessLossQty = 0;
        double GrandTotalQty = 0, GrandTotalProcessLossQty = 0;
        string sReceiveNo = ""; int nStyleID = 0;
        string sReportHeader = "";
        EnumReportLayout _eReportLayout = EnumReportLayout.None;
        string _sDateRange = "";
        #endregion
        public byte[] PrepareReport(List<FabricReceiveRegister> oFabricReceiveRegisters, Company oCompany, EnumReportLayout eReportLayout, string sDateRange)
        {
            _oFabricReceiveRegisters = oFabricReceiveRegisters;
            _oCompany = oCompany;
            _eReportLayout = eReportLayout;
            _sDateRange = sDateRange;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
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
            _oPdfPTable.SetWidths(new float[] { 842f });
            #endregion

            if (eReportLayout == EnumReportLayout.DateWise)
                sReportHeader = "Date Wise Fabric Receive Register";
            else if (eReportLayout == EnumReportLayout.Style_Wise)
                sReportHeader = "Style Wise Fabric Receive Register";
            else if (eReportLayout == EnumReportLayout.Factorywise)
                sReportHeader = "Factory Wise Fabric Receive Register";

            this.PrintHeader(sReportHeader);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;
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
            if (_eReportLayout == EnumReportLayout.DateWise)
                this.GetDateWiseTable();
              else if (_eReportLayout == EnumReportLayout.Style_Wise)
            {
                this.GetStyleWiseTable();
            }
            else if (_eReportLayout == EnumReportLayout.Factorywise)
            {
                this.GetFactoryWiseTable();
            }
           


        }
        #endregion
        #region Functions
        private PdfPTable InitializeTable()
        {
            PdfPTable oTempTable = new PdfPTable(18);
            oTempTable.WidthPercentage = 100;
            oTempTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTempTable.SetWidths(new float[] {               
                                                    25f,                                                  
                                                    50f,   //challan no
                                                    60f,   //Knitting Order
                                                    50f,   //Style
                                                    30f,   //PAM                                                    
                                                    30f,//Finish Dia
                                                    50f,   //buyer
                                                    80f,  //factory
                                                    80f,   //Product
                                                    40f,   //GSM                                                            
                                                    30f,   //MicDia
                                                    40f,   //Lot no
                                                    60f, //Lot Balance
                                                    60f,   //Store
                                                    40f,   //m unit 
                                                    50f, //ApprovedBy
                                                    50f,   //qty
                                                    50f    //Bag Qty
                                              });
            return oTempTable;
        }
        #endregion
        #region Date wise
        private void GetDateWiseTable()
        {
            PdfPTable oTempTable = InitializeTable();
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);

            #region Header Rows
            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Receive No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PAM", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Finish Dia", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Factory", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("GSM", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MIC-DIA", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Lot Balance", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Store", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M Unit", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Approved BY", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Process Lose Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);
            oTempTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oTempTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            oTempTable = InitializeTable();
            int nKnittingFabricReceiveID = 0; int nRowSpan = 0; string sReceiveDate = "";
            foreach (FabricReceiveRegister oItem in _oFabricReceiveRegisters)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
                if (nKnittingFabricReceiveID != oItem.KnittingFabricReceiveID)
                {
                    #region SubTotal
                    if (SubTotalQty > 0)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 16;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(SubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(SubtotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
                        oTempTable.CompleteRow();

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oTempTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        oTempTable = InitializeTable();
                        #endregion

                        SubTotalQty = 0;
                        SubtotalProcessLossQty = 0;
                    }
                    #endregion

                    if (sReceiveDate != oItem.ReceiveDateInString)
                    {
                        #region Date Print
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                        _oPdfPCell = new PdfPCell(new Phrase("Receive Date : @ " + oItem.ReceiveDateInString, _oFontStyle)); _oPdfPCell.Colspan = 18;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
                        oTempTable.CompleteRow();
                        #endregion
                    }

                    num = num + 1;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
                    nRowSpan = _oFabricReceiveRegisters.Count(x => x.KnittingFabricReceiveID == oItem.KnittingFabricReceiveID);
                    _oPdfPCell = new PdfPCell(new Phrase(num.ToString(), _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ReceiveNo, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
                }

                _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.KnittingOrderNo, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PAM.ToString(), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.FinishDia, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.FactoryName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.GSM, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MICDia, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotBalance.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.OperationUnitName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUnitName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ApprovedByName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProcessLossQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
                oTempTable.CompleteRow();

                SubTotalQty = SubTotalQty + oItem.Qty;
                GrandTotalQty = GrandTotalQty + oItem.Qty;
                SubtotalProcessLossQty = SubtotalProcessLossQty + oItem.OrderQty;
                GrandTotalProcessLossQty = GrandTotalProcessLossQty + oItem.OrderQty;
                nKnittingFabricReceiveID = oItem.KnittingFabricReceiveID;
                sReceiveDate = oItem.ReceiveDateInString;
            }

            #region SubTotal
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 16;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(SubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(SubtotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
            oTempTable.CompleteRow();
            #endregion

            #region Grand Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 16;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(GrandTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(GrandTotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
            oTempTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oTempTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
        #region Factory wise
        private void GetFactoryWiseTable()
        {
            PdfPTable oTempTable = InitializeTable();
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);

            #region Header Rows
            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Receive No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PAM", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Finish Dia", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Factory", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("GSM", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MIC-DIA", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Lot Balance", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Store", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M Unit", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Approved BY", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Process Lose Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);
            oTempTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oTempTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            oTempTable = InitializeTable();
            int nKnittingFabricReceiveID = 0; int nRowSpan = 0; int nFactoryID = 0;
            foreach (FabricReceiveRegister oItem in _oFabricReceiveRegisters)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
                if (nKnittingFabricReceiveID != oItem.KnittingFabricReceiveID)
                {
                    #region SubTotal
                    if (SubTotalQty > 0)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 16;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(SubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(SubtotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
                        oTempTable.CompleteRow();

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oTempTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        oTempTable = InitializeTable();
                        #endregion

                        SubTotalQty = 0;
                        SubtotalProcessLossQty = 0;
                    }
                    #endregion

                    if (nFactoryID != oItem.FactoryID)
                    {
                        #region Factory Print
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                        _oPdfPCell = new PdfPCell(new Phrase("Factory Name : @ " + oItem.FactoryName, _oFontStyle)); _oPdfPCell.Colspan = 18;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
                        oTempTable.CompleteRow();
                        #endregion
                    }

                    num = num + 1;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
                    nRowSpan = _oFabricReceiveRegisters.Count(x => x.KnittingFabricReceiveID == oItem.KnittingFabricReceiveID);
                    _oPdfPCell = new PdfPCell(new Phrase(num.ToString(), _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ReceiveNo, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
                }

                _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.KnittingOrderNo, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PAM.ToString(), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.FinishDia, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.FactoryName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.GSM, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MICDia, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotBalance.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.OperationUnitName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUnitName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ApprovedByName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProcessLossQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
                oTempTable.CompleteRow();

                SubTotalQty = SubTotalQty + oItem.Qty;
                GrandTotalQty = GrandTotalQty + oItem.Qty;
                SubtotalProcessLossQty = SubtotalProcessLossQty + oItem.OrderQty;
                GrandTotalProcessLossQty = GrandTotalProcessLossQty + oItem.OrderQty;
                nKnittingFabricReceiveID = oItem.KnittingFabricReceiveID;
                nFactoryID = oItem.FactoryID;
            }

            #region SubTotal
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 16;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(SubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(SubtotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
            oTempTable.CompleteRow();
            #endregion

            #region Grand Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 16;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(GrandTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(GrandTotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
            oTempTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oTempTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
        #region Style Wise
        private void GetStyleWiseTable()
        {
            PdfPTable oTempTable = InitializeTable();
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);

            #region Header Rows
            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Receive No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PAM", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Finish Dia", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Factory", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("GSM", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MIC-DIA", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Lot Balance", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Store", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M Unit", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Approved BY", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Process Lose Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempTable.AddCell(_oPdfPCell);
            oTempTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oTempTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            oTempTable = InitializeTable();
            int nKnittingFabricReceiveID = 0; int nRowSpan = 0; int nStyleID = 0;
            foreach (FabricReceiveRegister oItem in _oFabricReceiveRegisters)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
                if (nKnittingFabricReceiveID != oItem.KnittingFabricReceiveID)
                {
                    #region SubTotal
                    if (SubTotalQty > 0)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 16;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(SubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(SubtotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
                        oTempTable.CompleteRow();

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oTempTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        oTempTable = InitializeTable();
                        #endregion

                        SubTotalQty = 0;
                        SubtotalProcessLossQty = 0;
                    }
                    #endregion

                    if (nStyleID != oItem.StyleID)
                    {
                        #region Style Print
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                        _oPdfPCell = new PdfPCell(new Phrase("Style Name : @ " + oItem.StyleNo, _oFontStyle)); _oPdfPCell.Colspan = 18;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
                        oTempTable.CompleteRow();
                        #endregion
                    }

                    num = num + 1;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
                    nRowSpan = _oFabricReceiveRegisters.Count(x => x.KnittingFabricReceiveID == oItem.KnittingFabricReceiveID);
                    _oPdfPCell = new PdfPCell(new Phrase(num.ToString(), _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ReceiveNo, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
                }

                _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.KnittingOrderNo, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PAM.ToString(), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.FinishDia, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.FactoryName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.GSM, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MICDia, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotBalance.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.OperationUnitName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUnitName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ApprovedByName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProcessLossQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
                oTempTable.CompleteRow();

                SubTotalQty = SubTotalQty + oItem.Qty;
                GrandTotalQty = GrandTotalQty + oItem.Qty;
                SubtotalProcessLossQty = SubtotalProcessLossQty + oItem.OrderQty;
                GrandTotalProcessLossQty = GrandTotalProcessLossQty + oItem.OrderQty;
                nKnittingFabricReceiveID = oItem.KnittingFabricReceiveID;
                nStyleID = oItem.StyleID;
            }

            #region SubTotal
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 16;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(SubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(SubtotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
            oTempTable.CompleteRow();
            #endregion

            #region Grand Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 16;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(GrandTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(GrandTotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempTable.AddCell(_oPdfPCell);
            oTempTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oTempTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
    }
}
