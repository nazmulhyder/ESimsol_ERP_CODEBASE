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

    public class rptServiceInvoiceRegisters
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
        ServiceInvoiceRegister _oServiceInvoiceRegister = new ServiceInvoiceRegister();
        List<ServiceInvoiceRegister> _oServiceInvoiceRegisters = new List<ServiceInvoiceRegister>();
        Company _oCompany = new Company();
        double SubTotalQty = 0, SubTotalUnitPrice = 0, SubTotalTotalPrice = 0;
        double TotalQty = 0, TotalUnitPrice = 0, TotalTotalPrice = 0;
        double GrandTotalQty = 0, GrandTotalUnitPrice = 0, GrandTotalPrice = 0;
        string sQCNo = "";
        int nSIID = 0;
        string sReportHeader = "";
        EnumReportLayout _eReportLayout = EnumReportLayout.None;
        string _sDateRange = "";
        #endregion

        public byte[] PrepareReport(List<ServiceInvoiceRegister> oServiceInvoiceRegisters, Company oCompany, EnumReportLayout eReportLayout, string sDateRange)
        {
            _oServiceInvoiceRegisters = oServiceInvoiceRegisters;
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
            //_oPdfPTableDetail.SetWidths(new float[] { 25f, 100f, 80f, 80f, 80f});
            #endregion

            if (eReportLayout == EnumReportLayout.DateWise)
                sReportHeader = "Date Wise Service Invoice Register";
            else if (eReportLayout == EnumReportLayout.PartyWise)
                sReportHeader = "Party Wise Service Invoice Register";
            //else if (eReportLayout == EnumReportLayout.ProductWise)
            //    sReportHeader = "Product Wise Service Invoice Register(Details)";
            

            this.PrintHeader(sReportHeader);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 3;
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
            {
                _oPdfPCell = new PdfPCell(GetDateWiseTable(_oServiceInvoiceRegisters)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; //_oPdfPCell.FixedHeight = 150;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else if (_eReportLayout == EnumReportLayout.PartyWise)
            {
                _oPdfPCell = new PdfPCell(GetPartyWiseTable(_oServiceInvoiceRegisters)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; //_oPdfPCell.FixedHeight = 150;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            //else if (_eReportLayout == EnumReportLayout.ProductWise)
            //{
            //    _oPdfPCell = new PdfPCell(GetOrderWiseTable(_oServiceInvoiceRegisters)); _oPdfPCell.Border = 0;
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; //_oPdfPCell.FixedHeight = 150;
            //    _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //}
            
        }
        #endregion

        #region function

        #region date wise
        private PdfPTable GetDateWiseTable(List<ServiceInvoiceRegister> oServiceInvoiceRegisters)
        {
            PdfPTable oDateWiseTable = new PdfPTable(14);
            oDateWiseTable.WidthPercentage = 100;
            oDateWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oDateWiseTable.SetWidths(new float[] {               25f,                                                  
                                                                60f,    //part no
                                                                110f,     //product name
                                                                90f,    //party
                                                                70f,    //reg
                                                                70f,    //model
                                                                70f,    //engine                                                             
                                                                70f,    //chassis
                                                                70f,     //service type
                                                                40f,    //M Unit
                                                                45f,    //Qty
                                                                55f,    //Unit price
                                                                60f,    //total price
                                                                65f     //remarks
                                                          });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            #region header
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 14; _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);
            oDateWiseTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Part No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Party Name", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Reg. No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Model No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Engine No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Chassis No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Service Type", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M. Unit", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit Price", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Price", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oDateWiseTable.AddCell(_oPdfPCell);

            oDateWiseTable.CompleteRow();
            #endregion
            #region group by
            if (oServiceInvoiceRegisters.Count > 0)
            {
                var data = oServiceInvoiceRegisters.GroupBy(x => new { x.ServiceInvoiceDateInString }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
                {
                    ServiceInvoiceDate = key.ServiceInvoiceDateInString,
                    Results = grp.ToList() //All data
                });
            #endregion
                #region body
                GrandTotalQty = 0; GrandTotalUnitPrice = 0; GrandTotalPrice = 0;
                foreach (var oData in data)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Invoice Date : @ " + oData.ServiceInvoiceDate, _oFontStyle)); _oPdfPCell.Colspan = 14;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);
                    oDateWiseTable.CompleteRow();

                    count = 0; num = 0;
                    SubTotalQty = 0; SubTotalUnitPrice = 0; SubTotalTotalPrice = 0;
                    TotalQty = 0; TotalUnitPrice = 0; TotalTotalPrice = 0;
                    foreach (var oItem in oData.Results)
                    {
                        count++; //num++;
                        #region subtotal
                        //if (nSIID != 0)
                        //{
                        //    if (nSIID != oItem.ServiceInvoiceID && count > 1)
                        //    {
                        //        _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                        //        _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 10;
                        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        //        _oPdfPCell = new PdfPCell(new Phrase(SubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //SubTotalUnitPrice.ToString("#,##0.00;(#,##0.00)")
                        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        //        _oPdfPCell = new PdfPCell(new Phrase(SubTotalTotalPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        //        oDateWiseTable.CompleteRow();
                        //        SubTotalQty = 0; SubTotalUnitPrice = 0; SubTotalTotalPrice = 0;
                        //    }
                        //}
                        #endregion
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);

                        num++;
                        //if (nSIID != oItem.ServiceInvoiceID)
                        //{
                        //    num++;
                        //    int rowCount = oData.Results.Count(x => x.QCNo == oItem.QCNo);
                        //    _oPdfPCell = new PdfPCell(new Phrase(num.ToString(), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        //    _oPdfPCell = new PdfPCell(new Phrase(oItem.QCNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        //    _oPdfPCell = new PdfPCell(new Phrase(oItem.QCByName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        //    _oPdfPCell = new PdfPCell(new Phrase(oItem.StoreName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        //    _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        //}
                        _oPdfPCell = new PdfPCell(new Phrase(num.ToString(), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.PartsNo, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.PartsName, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.CustomerName, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.VehicleRegNo, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.VehicleModelNo, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.EngineNo, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ChassisNo, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ServiceOrderTypeSt, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.MUName, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);
                        SubTotalQty += oItem.Qty;
                        TotalQty += oItem.Qty;
                        GrandTotalQty += oItem.Qty;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);
                        SubTotalUnitPrice += oItem.UnitPrice;
                        TotalUnitPrice += oItem.UnitPrice;
                        GrandTotalUnitPrice += oItem.UnitPrice;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.TotalPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);
                        SubTotalTotalPrice += oItem.TotalPrice;
                        TotalTotalPrice += oItem.TotalPrice;
                        GrandTotalPrice += oItem.TotalPrice;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Remarks, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                        oDateWiseTable.CompleteRow();
                        nSIID = oItem.ServiceInvoiceID;
                    }
                    #region subtotal
                    //if (nSIID != 0)
                    //{
                    //    _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                    //    _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 10;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase(SubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));//SubTotalUnitPrice.ToString("#,##0.00;(#,##0.00)")
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase(SubTotalTotalPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    //    oDateWiseTable.CompleteRow();
                    //    SubTotalQty = 0; SubTotalUnitPrice = 0; SubTotalTotalPrice = 0;
                    //}
                    #endregion
                    #region total
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Date Wise Total : ", _oFontStyle)); _oPdfPCell.Colspan = 10;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(TotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));//TotalUnitPrice.ToString("#,##0.00;(#,##0.00)")
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(TotalTotalPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                    oDateWiseTable.CompleteRow();
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 14; _oPdfPCell.FixedHeight = 10; _oPdfPCell.Border = 0;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                }
                #region grandtotal
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 10;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));//GrandTotalUnitPrice.ToString("#,##0.00;(#,##0.00)")
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDateWiseTable.AddCell(_oPdfPCell);

                oDateWiseTable.CompleteRow();
                #endregion

            }
                #endregion
            return oDateWiseTable;
        }
        #endregion

        #region party wise
        private PdfPTable GetPartyWiseTable(List<ServiceInvoiceRegister> oServiceInvoiceRegisters)
        {
            PdfPTable oPartyWiseTable = new PdfPTable(14);
            oPartyWiseTable.WidthPercentage = 100;
            oPartyWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPartyWiseTable.SetWidths(new float[] {               25f,                                                  
                                                                60f,    //part no
                                                                110f,     //product name
                                                                60f,    //Invoice date
                                                                70f,    //reg
                                                                80f,    //model
                                                                70f,    //engine                                                             
                                                                90f,    //chassis
                                                                70f,     //service type
                                                                40f,    //M Unit
                                                                45f,    //Qty
                                                                55f,    //Unit price
                                                                60f,    //total price
                                                                65f     //remarks
                                                          });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            #region header
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 14; _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
            oPartyWiseTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Part No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Invoice Date", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Reg. No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Model No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Engine No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Chassis No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Service Type", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M. Unit", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit Price", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Price", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            oPartyWiseTable.CompleteRow();
            #endregion
            #region group by
            if (oServiceInvoiceRegisters.Count > 0)
            {
                var data = oServiceInvoiceRegisters.GroupBy(x => new { x.CustomerName }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
                {
                    CustomerName = key.CustomerName,
                    Results = grp.ToList() //All data
                });
            #endregion
                #region body
                GrandTotalQty = 0; GrandTotalUnitPrice = 0; GrandTotalPrice = 0;
                foreach (var oData in data)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Party : @ " + oData.CustomerName, _oFontStyle)); _oPdfPCell.Colspan = 14;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                    oPartyWiseTable.CompleteRow();

                    count = 0; num = 0;
                    SubTotalQty = 0; SubTotalUnitPrice = 0; SubTotalTotalPrice = 0;
                    TotalQty = 0; TotalUnitPrice = 0; TotalTotalPrice = 0;
                    foreach (var oItem in oData.Results)
                    {
                        count++; //num++;
                        #region subtotal
                        //if (nSIID != 0)
                        //{
                        //    if (nSIID != oItem.ServiceInvoiceID && count > 1)
                        //    {
                        //        _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                        //        _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 10;
                        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        //        _oPdfPCell = new PdfPCell(new Phrase(SubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //SubTotalUnitPrice.ToString("#,##0.00;(#,##0.00)")
                        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        //        _oPdfPCell = new PdfPCell(new Phrase(SubTotalTotalPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        //        oPartyWiseTable.CompleteRow();
                        //        SubTotalQty = 0; SubTotalUnitPrice = 0; SubTotalTotalPrice = 0;
                        //    }
                        //}
                        #endregion
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);

                        num++;
                        //if (nSIID != oItem.ServiceInvoiceID)
                        //{
                        //    num++;
                        //    int rowCount = oData.Results.Count(x => x.QCNo == oItem.QCNo);
                        //    _oPdfPCell = new PdfPCell(new Phrase(num.ToString(), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        //    _oPdfPCell = new PdfPCell(new Phrase(oItem.QCNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        //    _oPdfPCell = new PdfPCell(new Phrase(oItem.QCByName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        //    _oPdfPCell = new PdfPCell(new Phrase(oItem.StoreName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        //    _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        //}
                        _oPdfPCell = new PdfPCell(new Phrase(num.ToString(), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.PartsNo, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.PartsName, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ServiceInvoiceDateInString, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.VehicleRegNo, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.VehicleModelNo, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.EngineNo, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ChassisNo, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ServiceOrderTypeSt, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.MUName, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);


                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                        SubTotalQty += oItem.Qty;
                        TotalQty += oItem.Qty;
                        GrandTotalQty += oItem.Qty;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                        SubTotalUnitPrice += oItem.UnitPrice;
                        TotalUnitPrice += oItem.UnitPrice;
                        GrandTotalUnitPrice += oItem.UnitPrice;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.TotalPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                        SubTotalTotalPrice += oItem.TotalPrice;
                        TotalTotalPrice += oItem.TotalPrice;
                        GrandTotalPrice += oItem.TotalPrice;

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Remarks, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        oPartyWiseTable.CompleteRow();
                        nSIID = oItem.ServiceInvoiceID;
                    }
                    #region subtotal
                    //if (nSIID != 0)
                    //{
                    //    _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                    //    _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 10;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase(SubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));//SubTotalUnitPrice.ToString("#,##0.00;(#,##0.00)")
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase(SubTotalTotalPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    //    oPartyWiseTable.CompleteRow();
                    //    SubTotalQty = 0; SubTotalUnitPrice = 0; SubTotalTotalPrice = 0;
                    //}
                    #endregion
                    #region total
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Party Wise Total : ", _oFontStyle)); _oPdfPCell.Colspan = 10;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(TotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));//TotalUnitPrice.ToString("#,##0.00;(#,##0.00)")
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(TotalTotalPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                    oPartyWiseTable.CompleteRow();
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 14; _oPdfPCell.FixedHeight = 10; _oPdfPCell.Border = 0;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                }
                #region grandtotal
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 10;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));//GrandTotalUnitPrice.ToString("#,##0.00;(#,##0.00)")
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotalPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                oPartyWiseTable.CompleteRow();
                #endregion

            }
                #endregion
            return oPartyWiseTable;
        }
        #endregion

        #region order wise
        //private PdfPTable GetOrderWiseTable(List<ServiceInvoiceRegister> oServiceInvoiceRegisters)
        //{
        //    PdfPTable oOrderWiseTable = new PdfPTable(10);
        //    oOrderWiseTable.WidthPercentage = 100;
        //    oOrderWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
        //    oOrderWiseTable.SetWidths(new float[] {               25f,                                                  
        //                                                        60f,    //qc no
        //                                                        105f,     //qc by
        //                                                        60f,    //qc date
        //                                                        130f,    //Store
        //                                                        105f,    //buyer
        //                                                        107f,    //Style                                 
        //                                                        90f,    //Order qty
        //                                                        90f,    //QC qty
        //                                                        70f    //Reject qty
        //                                                  });

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);

        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 10; _oPdfPCell.Border = 0;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);
        //    oOrderWiseTable.CompleteRow();
        //    #region header
        //    _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderWiseTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("QC No", _oFontStyle));
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderWiseTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("QC By", _oFontStyle));
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderWiseTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("QC Date", _oFontStyle));
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderWiseTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("Store Name", _oFontStyle));
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderWiseTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyle));
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderWiseTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle));
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderWiseTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyle));
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderWiseTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("QC Pass Qty", _oFontStyle));
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderWiseTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("Reject Qty", _oFontStyle));
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oOrderWiseTable.AddCell(_oPdfPCell);

        //    oOrderWiseTable.CompleteRow();
        //    #endregion

        //    #region group by
        //    if (oServiceInvoiceRegisters.Count > 0)
        //    {
        //        var data = oServiceInvoiceRegisters.GroupBy(x => new { x.OrderRecapNo }, (key, grp) => new
        //        {
        //            PO = key.OrderRecapNo,
        //            Results = grp.ToList() //All data
        //        });
        //    #endregion

        //        #region body
        //        GrandTotalOrderQty = 0; GrandTotalQCPassQty = 0; GrandTotalRejectQty = 0;
        //        foreach (var oData in data)
        //        {
        //            _oPdfPCell = new PdfPCell(new Phrase("PO/Order No : @ " + oData.PO, _oFontStyle)); _oPdfPCell.Colspan = 10;
        //            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);
        //            oOrderWiseTable.CompleteRow();

        //            count = 0; num = 0;
        //            SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
        //            TotalOrderQty = 0; TotalQCPassQty = 0; TotalRejectQty = 0;

        //            foreach (var oItem in oData.Results)
        //            {
        //                count++; //num++;
        //                #region sub total
        //                //if (sQCNo != "")
        //                //{
        //                //    if (sQCNo != oItem.QCNo && count > 1)
        //                //    {
        //                //        _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
        //                //        _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 7;
        //                //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //                //        _oPdfPCell = new PdfPCell(new Phrase(SubTotalOrderQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
        //                //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //                //        _oPdfPCell = new PdfPCell(new Phrase(SubTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
        //                //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //                //        _oPdfPCell = new PdfPCell(new Phrase(SubTotalRejectQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
        //                //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //                //        oOrderWiseTable.CompleteRow();
        //                //        SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
        //                //    }
        //                //}
        //                #endregion
        //                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);

        //                _oPdfPCell = new PdfPCell(new Phrase(count.ToString(), _oFontStyle));
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase(oItem.QCNo, _oFontStyle));
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase(oItem.QCByName, _oFontStyle));
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase(oItem.QCDateInString, _oFontStyle));
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase(oItem.StoreName, _oFontStyle));
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase(oItem.TotalQuantity.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);
        //                SubTotalOrderQty += oItem.TotalQuantity;
        //                TotalOrderQty += oItem.TotalQuantity;
        //                GrandTotalOrderQty += oItem.TotalQuantity;

        //                _oPdfPCell = new PdfPCell(new Phrase(oItem.QCPassQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);
        //                SubTotalQCPassQty += oItem.QCPassQty;
        //                TotalQCPassQty += oItem.QCPassQty;
        //                GrandTotalQCPassQty += oItem.QCPassQty;

        //                _oPdfPCell = new PdfPCell(new Phrase(oItem.RejectQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);
        //                SubTotalRejectQty += oItem.RejectQty;
        //                TotalRejectQty += oItem.RejectQty;
        //                GrandTotalRejectQty += oItem.RejectQty;

        //                oOrderWiseTable.CompleteRow();
        //                sQCNo = oItem.QCNo;
        //            }
        //            #region subtotal
        //            //if (sQCNo != "")
        //            //{
        //            //    _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
        //            //    _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 7;
        //            //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //            //    _oPdfPCell = new PdfPCell(new Phrase(SubTotalOrderQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
        //            //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //            //    _oPdfPCell = new PdfPCell(new Phrase(SubTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
        //            //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //            //    _oPdfPCell = new PdfPCell(new Phrase(SubTotalRejectQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
        //            //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //            //    oOrderWiseTable.CompleteRow();
        //            //    SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
        //            //}
        //            #endregion
        //            #region total
        //            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
        //            _oPdfPCell = new PdfPCell(new Phrase("Order Wise Total : ", _oFontStyle)); _oPdfPCell.Colspan = 7;
        //            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase(TotalOrderQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
        //            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase(TotalQCPassQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
        //            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase(TotalRejectQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
        //            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //            oOrderWiseTable.CompleteRow();
        //            #endregion
        //            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 10; _oPdfPCell.FixedHeight = 10; _oPdfPCell.Border = 0;
        //            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //        }

        //        #region grand total
        //        _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
        //        _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 7;
        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(GrandTotalOrderQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(GrandTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(GrandTotalRejectQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oOrderWiseTable.AddCell(_oPdfPCell);

        //        oOrderWiseTable.CompleteRow();
        //        #endregion
        //    }
        //        #endregion
        //    return oOrderWiseTable;
        //}
        #endregion

        #endregion


    }
}
