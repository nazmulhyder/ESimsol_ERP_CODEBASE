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

    public class rptShipmentRegisters
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
        ShipmentRegister _oShipmentRegister = new ShipmentRegister();
        List<ShipmentRegister> _oShipmentRegisters = new List<ShipmentRegister>();
        Company _oCompany = new Company();
        //double SubTotalOrderQty = 0, SubTotalQCPassQty = 0, SubTotalRejectQty = 0;
        //double TotalOrderQty = 0, TotalQCPassQty = 0, TotalRejectQty = 0;
        //double GrandTotalOrderQty = 0, GrandTotalQCPassQty = 0, GrandTotalRejectQty = 0;
        //int nRecapID = 0;
        string sReportHeader = "";
        string _sDateRange = "";
        EnumReportLayout _eReportLayout = EnumReportLayout.None;
        #endregion

        public byte[] PrepareReport(List<ShipmentRegister> oShipmentRegisters, Company oCompany, string sDateRange)    //, EnumReportLayout eReportLayout
        {
            _oShipmentRegisters = oShipmentRegisters;
            _oCompany = oCompany;
            //_eReportLayout = eReportLayout;
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

            sReportHeader = "Party Wise Shipment Register(Details)";
            
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
            _oPdfPCell = new PdfPCell(GetPartyWiseTable(_oShipmentRegisters)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; //_oPdfPCell.FixedHeight = 150;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            
        }
        #endregion

        #region function

        #region party wise
        private PdfPTable GetPartyWiseTable(List<ShipmentRegister> oShipmentRegisters)
        {
            PdfPTable oPartyWiseTable = new PdfPTable(12);
            oPartyWiseTable.WidthPercentage = 100;
            oPartyWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPartyWiseTable.SetWidths(new float[] {               //25f, 
                                                                75f,    //ship date                                 
                                                                80f,    //style
                                                                80f,     //order no
                                                                60f,    //country
                                                                50f,    //shpment qty
                                                                50f,    //ctn qty
                                                                50f,    //Total ctn 

                                                                60f,    //Chaallan no
                                                                55f,    //ship mode
                                                                70f,    //truck
                                                                80f,    //driver

                                                                80f,    //mobile no
                                                                
                                                          });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            #region header
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 16; _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
            oPartyWiseTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Ship. Date", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Country", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Ship. Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("CTN Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total CTN", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Ship. Mode", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Truck", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Driver", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Mobile No", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPartyWiseTable.AddCell(_oPdfPCell);

            oPartyWiseTable.CompleteRow();
            #endregion
            #region group by
            if (oShipmentRegisters.Count > 0)
            {
                var data = oShipmentRegisters.GroupBy(x => new { x.BuyerName }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
                {
                    BuyerName = key.BuyerName,
                    Results = grp.ToList() //All data
                });
            #endregion
                #region body
                //GrandTotalOrderQty = 0; GrandTotalQCPassQty = 0; GrandTotalRejectQty = 0;
                foreach (var oData in data)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Buyer : @ " + oData.BuyerName, _oFontStyle)); _oPdfPCell.Colspan = 16;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                    oPartyWiseTable.CompleteRow();

                    count = 0; num = 0;
                    string SDate = "",styleNo="",recapNo="",challanNo=""; int rowCount;
                    int TotalCTN = 0;
                    foreach (var oItem in oData.Results)
                    {
                        count++; 
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);

                        if (SDate != oItem.ShipmentDateInString)
                        {
                            rowCount = oData.Results.Count(x => x.ShipmentDateInString == oItem.ShipmentDateInString);
                            _oPdfPCell = new PdfPCell(new Phrase(oItem.ShipmentDateInString, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                        }

                        if (styleNo != oItem.StyleNo)
                        {
                            rowCount = oData.Results.Count(x => x.StyleNo == oItem.StyleNo);
                            _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                        }

                        if (recapNo != oItem.OrderRecapNo)
                        {
                            rowCount = oData.Results.Count(x => x.OrderRecapNo == oItem.OrderRecapNo);
                            _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderRecapNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                        }

                        
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.CountryShortName, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ShipmentQty.ToString("#,###.##;(#,###.##)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.CTNQty.ToString("#,###.##;(#,###.##)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                        
                        if (challanNo != oItem.ChallanNo)
                        {
                            rowCount = oData.Results.Count(x => x.ChallanNo == oItem.ChallanNo);
                            TotalCTN = oData.Results.Where(x => x.ChallanNo == oItem.ChallanNo).Sum(c => c.CTNQty);

                            _oPdfPCell = new PdfPCell(new Phrase(TotalCTN.ToString("#,###.##;(#,###.##)"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                            TotalCTN = 0;

                            _oPdfPCell = new PdfPCell(new Phrase(oItem.ChallanNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);
                        }

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ShipmentModeInString, _oFontStyle));  
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.TruckNo, _oFontStyle));  
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.DriverName, _oFontStyle));  
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.DriverMobileNo, _oFontStyle));  
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPartyWiseTable.AddCell(_oPdfPCell);

                        oPartyWiseTable.CompleteRow();
                        SDate = oItem.ShipmentDateInString;
                        styleNo = oItem.StyleNo;
                        recapNo = oItem.OrderRecapNo;
                        challanNo = oItem.ChallanNo;
                    }
                    
                }

            }
                #endregion
            return oPartyWiseTable;
        }
        #endregion

        #endregion


    }
}
