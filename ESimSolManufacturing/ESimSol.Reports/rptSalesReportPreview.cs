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
    public class rptSalesReportPreview
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(14);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<SalesReport> _oSalesReports = new List<SalesReport>();
        SalesReport _oSalesReport;
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit;
        int _nColspan = 14;
        #endregion
        public byte[] PrepareReport(List<SalesReport> SalesReports, SalesReport oSalesReport, Company oCompany,BusinessUnit oBusinessUnit, string sHeaderName)
        {
            _oSalesReports = SalesReports;
            _oSalesReport = oSalesReport;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //842*595
            _oDocument.SetMargins(20f, 20f, 10f, 20f);
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
                                                    52f,  //3
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
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase(((_oSalesReport.BUID>0)?_oBusinessUnit.BusinessUnitType.ToString():""), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));//_oBusinessUnit.PringReportHead
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColspan; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            if (_oSalesReport.ViewType == 2)
                sHeaderName += " " + _oSalesReport.Year;
            else
                sHeaderName += " " + (_oSalesReport.Year - 11) + " To " + _oSalesReport.Year;

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.Colspan = _nColspan; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLDITALIC);
            _oPdfPCell = new PdfPCell(new Phrase(_oSalesReport.Note, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Colspan =_nColspan; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            #region HEADER
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            if (_oSalesReport.ViewType == 2) //MonthWise
            {
                string[] MonthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                int nStartMonth = _oSalesReport.Month-1;
                int nEndMonth = nStartMonth + 12; int j = 0;
                int sNextYear = 0;
                for (int i = nStartMonth; i < nEndMonth; i++)
                {
                    if (i < 12)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(MonthNames[i].Substring(0, 3) + " " + _oSalesReport.Year.ToString().Substring(2, 2), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        if (j < 12)
                        {
                             string sYear = (_oSalesReport.Year+1).ToString();
                             _oPdfPCell = new PdfPCell(new Phrase(MonthNames[j].Substring(0, 3) + " " + sYear.Substring(2, 2), _oFontStyle));
                             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        j++;
                    }
                }
            }
            else //---------------------------------------YearWise
            {
                for (int i = 11; i >= 0; i--)
                    this.AddCell((_oSalesReport.Year - i).ToString(), "CENTER", true);
            }
            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);          
            List<SalesReport> oSalesReport_Dist = new List<SalesReport>();          
            oSalesReport_Dist = _oSalesReports.GroupBy(x => x.RefID).Select(g => g.First()).ToList();
            oSalesReport_Dist = oSalesReport_Dist.OrderBy(x => x.GroupName).ThenBy(x => x.RefName).ToList();
            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            string tempMKtPerson = "";
            foreach (SalesReport oItem in oSalesReport_Dist)
            {                 
                if (oItem.GroupName != "" && tempMKtPerson != oItem.GroupName)
                {                
                   _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.GroupName, _oFontStyle)); _oPdfPCell.Colspan = _nColspan;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);                  
                  
                }
             
                 this.AddCell(oItem.RefName, "LEFT", false);
                if (_oSalesReport.ViewType == 2) //MonthWise
                {
                    int nStartMonth = _oSalesReport.Month;
                    int nEndMonth = nStartMonth + 12; int j = 1;

                    for (int i = nStartMonth; i < nEndMonth; i++)
                    {
                          string nValue = "";
                          if (i < 13)
                         {
                            nValue = Global.MillionFormat_Round(_oSalesReports.Where(x => x.Month == i && x.RefID == oItem.RefID).Sum(x => x.Value));
                            if (_oSalesReports[0].Symbol == "") 
                            {
                                this.AddCell((nValue.Contains("-") ? "-" : nValue), "RIGHT", false);                           
                            }
                            else
                            {
                                this.AddCell((nValue.Contains("-") ? "-" : _oSalesReports[0].Symbol+""+nValue), "RIGHT", false);     
                            }
                            
                         }
                          else
                          {
                              if (j < 13)
                              {
                                  nValue = Global.MillionFormat_Round(_oSalesReports.Where(x => x.Month == j && x.RefID == oItem.RefID).Sum(x => x.Value));
                                  if (_oSalesReports[0].Symbol == "")
                                  {
                                      this.AddCell((nValue.Contains("-") ? "-" : nValue), "RIGHT", false);
                                  }
                                  else
                                  {
                                      this.AddCell((nValue.Contains("-") ? "-" : _oSalesReports[0].Symbol + "" + nValue), "RIGHT", false);
                                  }
                              }
                              j++;
                          }
                    
                    }

                }
                else if (_oSalesReport.ViewType == 3) //YearWise
                {
                    for (int i = 11; i >= 0; i--)
                    {
                        string nValue = "";
                        nValue = Global.MillionFormat_Round(_oSalesReports.Where(x => x.Year == (_oSalesReport.Year - i) && x.RefID == oItem.RefID).Select(x => x.Value).FirstOrDefault());
                        //this.AddCell((nValue.Contains("-") ? "-" : nValue), "RIGHT", false);
                        if (_oSalesReports[0].Symbol == "")
                        {
                            this.AddCell((nValue.Contains("-") ? "-" : nValue), "RIGHT", false);
                        }
                        else
                        {
                            this.AddCell((nValue.Contains("-") ? "-" : _oSalesReports[0].Symbol + "" + nValue), "RIGHT", false);
                        }
                    }
                }

                string nTValue = "";      
                nTValue = Global.MillionFormat_Round(_oSalesReports.Where(x => x.RefID == oItem.RefID).Sum(x => x.Value));
                //this.AddCell((nTValue.Contains("-") ? "-" : nTValue), "RIGHT", false);
                if (_oSalesReports[0].Symbol == "")
                {
                    this.AddCell((nTValue.Contains("-") ? "-" : nTValue), "RIGHT", false);
                }
                else
                {
                    this.AddCell((nTValue.Contains("-") ? "-" : _oSalesReports[0].Symbol + "" + nTValue), "RIGHT", false);
                }

                tempMKtPerson = oItem.GroupName;
                _oPdfPTable.CompleteRow();
            }
            #endregion


            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            this.AddCell("Total", "CENTER", false);
            if (_oSalesReport.ViewType == 2)
            {
                int nStartMonth = _oSalesReport.Month;
                int nEndMonth = nStartMonth + 12; int j = 1;
                for (int i = nStartMonth; i <nEndMonth; i++)
                {
                    double nValue = 0;
                     if (i < 13)
                     {
                          nValue = _oSalesReports.Where(x => x.Month == i).Sum(x => x.Value);
                          if (nValue>0)
                         {
                             if (_oSalesReports[0].Symbol == "")
                             {
                                 this.AddCell(Global.MillionFormat_Round(nValue), "RIGHT", false);
                             }
                             else
                             {
                                 this.AddCell(_oSalesReports[0].Symbol + "" + Global.MillionFormat_Round(nValue), "RIGHT", false);
                             }
                         }
                          else
                          {
                              this.AddCell("-", "RIGHT", false);
                          }                        
                     }
                     else
                     {
                         if (j < 13)
                         {
                             nValue = _oSalesReports.Where(x => x.Month == j).Sum(x => x.Value);
                             if (nValue>0)
                             {
                                 if (_oSalesReports[0].Symbol == "")
                                 {
                                     this.AddCell(Global.MillionFormat_Round(nValue), "RIGHT", false);
                                 }
                                 else
                                 {
                                     this.AddCell(_oSalesReports[0].Symbol + "" + Global.MillionFormat_Round(nValue), "RIGHT", false);
                                 }

                             }
                             else
                             {
                                 this.AddCell("-", "RIGHT", false);
                             }
                            
                         }
                         j++;
                     }                       
                }
            }
            else if (_oSalesReport.ViewType == 3)//--YearWiseTotal
            {
                for (int i = 11; i >= 0; i--)
                {
                    double nValue = _oSalesReports.Where(x => x.Year == (_oSalesReport.Year - i)).Sum(x => x.Value);
                    if (nValue > 0)
                    {
                        //this.AddCell(Global.MillionFormat_Round(nValue), "RIGHT", false); //--YearWiseTotal
                        if (_oSalesReports[0].Symbol == "")
                        {
                            this.AddCell(Global.MillionFormat_Round(nValue), "RIGHT", false);
                        }
                        else
                        {
                            this.AddCell(_oSalesReports[0].Symbol + ""+Global.MillionFormat_Round(nValue), "RIGHT", false);                            
                        }
                    }
                   
                    else
                    {
                        this.AddCell("-", "RIGHT", false);
                    }
                    
                }
            }


            #region Grand Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            double nLCValue = _oSalesReports.Sum(x => x.Value);
            if (nLCValue > 0)
            {
                //this.AddCell(Global.MillionFormat_Round(nLCValue), "RIGHT", false); //--GrandTotal
                 if (_oSalesReports[0].Symbol == "")
                {
                    this.AddCell(Global.MillionFormat_Round(nLCValue), "RIGHT", false);
                }
                else
                {
                    this.AddCell(_oSalesReports[0].Symbol + "" + Global.MillionFormat_Round(nLCValue), "RIGHT", false);                            
                }
            }
                
            else
              this.AddCell("-", "RIGHT", false);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion


        }

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

            if (isGray) {
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            }
            else
            {
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            }
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            
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
        public void PrintRow(string sHeader, string Align)
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
