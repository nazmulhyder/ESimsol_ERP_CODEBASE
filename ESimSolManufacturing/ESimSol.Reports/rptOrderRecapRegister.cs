using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Linq;

namespace ESimSol.Reports
{
    public class rptOrderRecapRegister
    {
        #region Declaration
        Document _oDocument;
        PdfWriter _oWriter;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(8);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        OrderRecap _oOrderRecap = new OrderRecap();
        List<ShipmentSchedule> _oShipmentSchedules = new List<ShipmentSchedule>();
        List<ShipmentScheduleDetail> _oShipmentScheduleDetailList = new List<ShipmentScheduleDetail>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit;
        int _nColspan = 8; float _fSizeWidth = 1;
        List<int> _nSizeIDs = new List<int>();
        List<string> _sColorList = new List<string>();
        int _nTotalColumn=8;
        #endregion

        #region Order Sheet
        public byte[] PrepareReport(OrderRecap oOrderRecap, List<ShipmentScheduleDetail> oShipmentScheduleDetailList,List<ShipmentSchedule> oShipmentSchedule, BusinessUnit oBusinessUnit, string sHeaderName)
        {
            _oOrderRecap = oOrderRecap;
            _oShipmentScheduleDetailList = oShipmentScheduleDetailList;
            _oShipmentSchedules = oShipmentSchedule;
            _oBusinessUnit = oBusinessUnit;
            _oCompany = oOrderRecap.Company;
            _sColorList = _oShipmentScheduleDetailList.Select(x => x.ColorName).Distinct().ToList();
            _oShipmentScheduleDetailList=_oShipmentScheduleDetailList.OrderBy(x => x.ColorID).ThenBy(x => x.SizeID).ToList();
            _nSizeIDs = _oShipmentScheduleDetailList.OrderBy(x => x.SizeName).Select(x => x.SizeID).Distinct().ToList();

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//595*842
            _oDocument.SetMargins(30f, 30f, 45f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);

            #region ESimSolFooter
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            #endregion

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    40f,  //TOD (ShipmentDate) 
                                                    25f,  //CutOff
                                                    35f,  //TOD (CutOffDate) 
                                                    45f,  //ShipmentMode 225  
                                                    60f, //Country
                                                    50f,  //Color 195                        
                                                    300f,  //Size
                                                    40f,  //Total
                                             });
            _fSizeWidth = 300f;
            #endregion
            this.PrintHeader(sHeaderName);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #endregion

        #region Report Header
        private void PrintHeader(string sHeaderName)
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
                _oImag.ScaleAbsolute(60f, 25f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));//_oBusinessUnit.PringReportHead
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColspan; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.Colspan = _nColspan; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private PdfPTable GetCutOffTable() 
        {
            PdfPTable oPdfPTable = new PdfPTable(_nTotalColumn-1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    25f,  //CutOff
                                                    35f,  //TOD (CutOffDate) 
                                                    45f,  //ShipmentMode 225  
                                                    60f, //Country
                                                    50f,  //Color 195                        
                                                    300f,  //Size
                                                    40f,  //Total
                                             });
            return oPdfPTable;
        } 
        private PdfPTable GetSizeTable() 
        {
            PdfPTable oPdfPTable = new PdfPTable(_nSizeIDs.Count+1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            float[] nWidths = new float[(_nSizeIDs.Count + 1)];
            for (int i = 0; i < (_nSizeIDs.Count + 1); i++)
            {
                nWidths[i] = _fSizeWidth / (_nSizeIDs.Count+1);
            }
            oPdfPTable.SetWidths(nWidths);
            return oPdfPTable;
        }
        private PdfPTable PrintCutOff(List<ShipmentSchedule> oSSCutOffWise) 
        {
            PdfPTable CutOffTable = GetCutOffTable();

            #region Cut Off Wise
            int nRowSpan = (oSSCutOffWise.Count() * _sColorList.Count()) + 1;
            _oPdfPCell = new PdfPCell(new Phrase(oSSCutOffWise[0].CutOffTypeST, _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = nRowSpan;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            CutOffTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oSSCutOffWise[0].CutOffDateSt, _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = nRowSpan;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            CutOffTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oSSCutOffWise[0].ShipmentModeST, _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = nRowSpan;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            CutOffTable.AddCell(_oPdfPCell);
            #endregion

            int nSL = 0, nCountryID=0;
            foreach (ShipmentSchedule oItem in oSSCutOffWise) 
            {
                bool isTotalPrint = true;
                if (nCountryID != oItem.CountryID)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.CountryShortName+" ("+oItem.CountryName+")", _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = _sColorList.Count();
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    CutOffTable.AddCell(_oPdfPCell);

                    #region Color And Size
                    foreach (var oColor in _sColorList)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oColor, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        CutOffTable.AddCell(_oPdfPCell);

                        #region Size BreakDwon
                        double nCutOffTotal = 0;
                        PdfPTable oSizeBreakDownTable = GetSizeTable();
                        foreach (var nSizeID in _nSizeIDs)
                        {
                            double Qty = _oShipmentScheduleDetailList.Where(x => x.ShipmentScheduleID == oItem.ShipmentScheduleID && x.SizeID == nSizeID && x.ColorName.Equals(oColor)).Select(x => x.Qty).FirstOrDefault();
                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Qty), _oFontStyle));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            oSizeBreakDownTable.AddCell(_oPdfPCell);
                            nCutOffTotal += Qty;
                        }

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nCutOffTotal), _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oSizeBreakDownTable.AddCell(_oPdfPCell);
                        #endregion

                        _oPdfPCell = new PdfPCell(oSizeBreakDownTable);
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        CutOffTable.AddCell(_oPdfPCell);

                        if (isTotalPrint)
                        {
                            double Qty = _oShipmentScheduleDetailList.Where(x => x.ShipmentScheduleID == oItem.ShipmentScheduleID).Sum(x => x.Qty);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Qty), _oFontStyle));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = nRowSpan;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            CutOffTable.AddCell(_oPdfPCell);
                            isTotalPrint = false;
                        }
                    }
                    #endregion
                    nCountryID = oItem.CountryID;
                }
            }
          
            nSL++;

            #region SubTotal
            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            CutOffTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            CutOffTable.AddCell(_oPdfPCell);

            #region Size BreakDwon
            double nCutOffSubTotal = 0;
            PdfPTable oSizeBreakDownSubTable = GetSizeTable();

            foreach (var nSizeID in _nSizeIDs)
            {
                double Qty = _oShipmentScheduleDetailList.Where(x => x.ShipmentScheduleID == oSSCutOffWise[0].ShipmentScheduleID && x.SizeID == nSizeID).Sum(x => x.Qty);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oSizeBreakDownSubTable.AddCell(_oPdfPCell);
                nCutOffSubTotal += Qty;
            }
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nCutOffSubTotal), _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oSizeBreakDownSubTable.AddCell(_oPdfPCell);
            #endregion

            _oPdfPCell = new PdfPCell(oSizeBreakDownSubTable);
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            CutOffTable.AddCell(_oPdfPCell);
            #endregion

            return CutOffTable;
        }
        private void PrintBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            
            #region HEADER
            _oPdfPCell = new PdfPCell(new Phrase("TOD(Po)", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Cut Off", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("TOD", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Mode of Transport", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Country Name", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Size", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Cut Off Total", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Size BreakDwon
            PdfPTable oSizeBreakDownTable = GetSizeTable();
            foreach(var oItem in _nSizeIDs)
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oShipmentScheduleDetailList.Where(x=>x.SizeID==oItem).Select(x=>x.SizeName).FirstOrDefault(), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oSizeBreakDownTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oSizeBreakDownTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oSizeBreakDownTable);
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion

            #endregion

            string sShipmentDateSt = "";

            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, iTextSharp.text.Font.NORMAL);
            foreach (ShipmentSchedule oItem in _oShipmentSchedules)
            {
                if (!sShipmentDateSt.Equals(oItem.ShipmentDateSt))
                {
                    List<ShipmentSchedule> oSSCutOffWise = _oShipmentSchedules.Where(x => x.ShipmentDateSt.Equals(oItem.ShipmentDateSt)).GroupBy(x => new { x.CutOffType, x.CutOffDate, x.CountryID,x.ShipmentScheduleID }, (key, grp) => new ShipmentSchedule
                    {
                        CutOffType = key.CutOffType,
                        CutOffDate = key.CutOffDate,
                        CountryID = key.CountryID,
                        CountryName = grp.First().CountryName,
                        CountryShortName = grp.First().CountryShortName,
                        CountryCode = grp.First().CountryShortName,
                        ShipmentScheduleID=key.ShipmentScheduleID,
                        ShipmentMode = grp.First().ShipmentMode
                    }).ToList();

                    this.AddCell(oItem.ShipmentDateSt, "CENTER", 0, oSSCutOffWise.Count());
                    int nCutOff = 0;
                    foreach (ShipmentSchedule oSS in oSSCutOffWise) 
                    {
                        if(nCutOff != (int)oSS.CutOffType) 
                        {
                            //PrintCutOff(oSSCutOffWise.Where(x => x.CutOffType == oItem.CutOffType).ToList());
                            _oPdfPCell = new PdfPCell(PrintCutOff(oSSCutOffWise.Where(x => x.CutOffType == oSS.CutOffType).ToList()));
                            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.Colspan = _nTotalColumn - 1;
                            _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();
                        }
                        nCutOff= (int)oSS.CutOffType;
                    }
                    sShipmentDateSt = oItem.ShipmentDateSt;
                }
            }
            #endregion

            #region Grand Total
            
            this.AddCell("Grand Total", "CENTER", 5, 0, "LIGHT_GRAY");
            this.AddCell("", "CENTER", "LIGHT_GRAY");

            #region Size BreakDwon
            double nTotal = 0;
            oSizeBreakDownTable = GetSizeTable();
            foreach (var nSizeID in _nSizeIDs)
            {
                double Qty = _oShipmentScheduleDetailList.Where(x => x.SizeID == nSizeID).Sum(x => x.Qty);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oSizeBreakDownTable.AddCell(_oPdfPCell);
                nTotal += Qty;
            }

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotal), _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oSizeBreakDownTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oSizeBreakDownTable);
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion
            this.AddCell("", "CENTER", "LIGHT_GRAY");
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region PDF HELPER
        public void AddCell(string sHeader, string sAlignment, int nColSpan, int nRowSpan, string sColor)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default
            if (sAlignment == "LEFT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "RIGHT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment == "CENTER")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            if (sColor.Equals("GRAY"))
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            if (sColor.Equals("LIGHT_GRAY"))
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;

            if (nColSpan > 0)
                _oPdfPCell.Colspan = nColSpan;
            if (nRowSpan > 0)
                _oPdfPCell.Rowspan = nRowSpan;

            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddCell(string sHeader, string sAlignment, int nColSpan, int nRowSpan)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default
            if (sAlignment == "LEFT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "RIGHT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment == "CENTER")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            if (nColSpan > 0)
                _oPdfPCell.Colspan = nColSpan;
            if (nRowSpan > 0)
                _oPdfPCell.Rowspan = nRowSpan;

            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddCell(string sHeader, string sAlignment)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default
            if (sAlignment == "LEFT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "RIGHT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment == "CENTER")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddCell(string sHeader, string sAlignment, string sColor)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default
            if (sAlignment == "LEFT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "RIGHT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment == "CENTER")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            if (sColor.Equals("GRAY"))
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            if (sColor.Equals("LIGHT_GRAY"))
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;

            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void PrintRow(string sHeader)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColspan;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion
    }
}