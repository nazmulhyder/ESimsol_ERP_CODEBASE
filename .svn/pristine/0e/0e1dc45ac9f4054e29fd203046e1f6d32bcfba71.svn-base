using System;
using System.Data;
using ESimSol.BusinessObjects.ReportingObject;
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
    public class rptGRNDetailProduct
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(14);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<GRNDetail> _oGRNDetailList = new List<GRNDetail>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit;
        int _nColspan = 14;

        int _nDateYear = 0;
        int _nLayout = 0;
        #endregion

        #region Order Sheet
        public byte[] PrepareReport(List<GRNDetail> oGRNDetailList, int[] nParam, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName)
        {
            _oGRNDetailList = oGRNDetailList;
           
            _oBusinessUnit = oBusinessUnit;
            _oCompany = oCompany;

            _nDateYear = nParam[0];
            _nLayout = nParam[1];

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //842*595
            _oDocument.SetMargins(30f, 30f, 45f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    166f,  //1 
                                                    52f,  //2
                                                    52f, //3
                                                    52f,  //4
                                                    52f,  //5                                                
                                                    52f,  //6                                               
                                                    52f,  //7
                                                    52f,  //8
                                                    52f,  //9
                                                    52f,  //10
                                                    52f,  //11
                                                    52f,  //12
                                                    52f,  //13
                                                    52f,  //14
                                             });
            #endregion

            this.PrintHeader(sHeaderName);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

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

            if (_nLayout == 1)
                sHeaderName += " " + _nDateYear;
            else
                sHeaderName += " " + (_nDateYear - 11) + " To " + _nDateYear;

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.Colspan = _nColspan; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            #region HEADER

        
                this.AddCell("Product Name", "CENTER", false);


            if (_nLayout == 1) //MonthWise
            {
                string[] MonthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                foreach (string month in MonthNames)
                {
                    if (!string.IsNullOrEmpty(month)) 
                    {
                        string Month = month.Substring(0, 3).ToUpper();
                        this.AddCell(Month + " " + (_nDateYear).ToString().Substring(2, 2), "CENTER", false);
                    }
                }
            }
            else //---------------------------------------YearWise
            {
                for (int i = 11; i >= 0; i--)
                    this.AddCell((_nDateYear-i).ToString(), "CENTER", false);
            }
            this.AddCell("Total", "CENTER", false);
            _oPdfPTable.CompleteRow();

            #endregion

            //oProductionSchedule.ProductionScheduleList.Where(x => x.MachineID > 0).Select(x => x.MachineID).Distinct().ToList())

            List<GRNDetail> oGRNDetail_Dist = new List<GRNDetail>();
            oGRNDetail_Dist = _oGRNDetailList.GroupBy(x => x.ProductID).Select(g => g.First()).ToList();
            
            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            foreach (GRNDetail oItem in oGRNDetail_Dist)
            {
                this.AddCell(oItem.ProductName, "LEFT", false);
               
                   
                if (_nLayout == 1) //MonthWise
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        string nQTy = "";
                        nQTy = Global.MillionFormat_Round(_oGRNDetailList.Where(x => x.DateMonth == i && x.ProductID == oItem.ProductID).Sum(x => x.ReceivedQty));
                        this.AddCell((nQTy.Contains("-") ? "-" : nQTy), "RIGHT", false);
                    }
                }
                else //YearWise
                {
                    for (int i = 11; i >=0; i--)
                    {
                        string nQTy = "";
                        nQTy = Global.MillionFormat_Round(_oGRNDetailList.Where(x => x.DateYear == (_nDateYear - i) && x.ProductID == oItem.ProductID).Sum(x => x.ReceivedQty));
                      
                        this.AddCell((nQTy.Contains("-") ? "-" : nQTy), "RIGHT", false);
                    }
                }

                string nTQTy = "";
                nTQTy = Global.MillionFormat_Round(_oGRNDetailList.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.ReceivedQty));
                   
                this.AddCell((nTQTy.Contains("-")?"-":nTQTy), "RIGHT", false);
                    _oPdfPTable.CompleteRow();
            }
            #endregion

            string sMUSymbol = "";// _oGRNDetailList.Select(o => o.MUSymbol).FirstOrDefault();
            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            this.AddCell("Total", "CENTER", false);
            if (_nLayout == 1)
            {
                for (int i = 1; i <= 12; i++)
                {
                    double nQTy = _oGRNDetailList.Where(x => x.DateMonth == i).Sum(x => x.ReceivedQty);
                    
                    if (nQTy > 0)
                        this.AddCell( sMUSymbol+ Global.MillionFormat_Round(nQTy), "RIGHT", false); //--MonthWiseTotal
                    else
                        this.AddCell("-", "RIGHT", false);
                }
            }
            else 
            {
                for (int i = 11; i >= 0; i--)
                {
                    double nQTy = _oGRNDetailList.Where(x => x.DateYear == (_nDateYear - i)).Sum(x => x.ReceivedQty);
                    if (nQTy > 0)
                        this.AddCell(sMUSymbol+Global.MillionFormat_Round(nQTy), "RIGHT", false); //--YearWiseTotal
                    else
                        this.AddCell("-", "RIGHT", false);
                }
            }
            double nLCQTy = _oGRNDetailList.Sum(x => x.ReceivedQty);
            if (nLCQTy > 0)
                this.AddCell(sMUSymbol+ Global.MillionFormat_Round(nLCQTy), "RIGHT", false); //--GrandTotal
            else
                this.AddCell("-", "RIGHT", false);
            _oPdfPTable.CompleteRow();
            #endregion

            this.PrintRow("*All Quantity Are In " + _oGRNDetailList.Select(o=>o.MUName).FirstOrDefault(),"LEFT");
        }

        #endregion

        #region PDF HELPER
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
        public void AddCell(string sHeader, string sAlignment, bool isGray)
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
        public void PrintRow(string sHeader,string Align)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            if (Align.Contains("LEFT"))
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColspan;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion

        #endregion
    }
}