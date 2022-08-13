using System;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using System.Linq;
using iTextSharp.text.pdf;
 
 

namespace ESimSol.Reports
{
  public class rptExportLCReport
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        int _nTablecolumn = 9;
        PdfPTable _oPdfPTable = new PdfPTable(9);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        ExportLCReport _oExportLCReport = new ExportLCReport();
        List<ExportLCReport> _oExportLCReports = new List<ExportLCReport>();
        Company _oCompany = new Company();
        #endregion

        #region Order Sheet List
        public byte[] PrepareReport(List<ExportLCReport> oExportLCReports, int LCReportLevel, Company oCompany, bool bPrintWithMLC)
        {
            _oExportLCReports = oExportLCReports;
            _oCompany = oCompany;
            
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(15f, 15f, 10f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            
            if (LCReportLevel == (int)EnumLCReportLevel.LCLevel)
            {
               
                if (!bPrintWithMLC)
                {
                    _oPdfPTable = new PdfPTable(9);
                    _nTablecolumn = 9;
                    _oPdfPTable.SetWidths(new float[] { 20f, //SL
                                                   50f, //LC no
                                                   75f, //buyer name
                                                   70f, //concern person
                                                   45f, //lc date
                                                   70f, //nego bnjk
                                                   75f, //issue bank
                                                   60f, //qty
                                                   60f //value
                });
                }
                else
                {
                    _oPdfPTable = new PdfPTable(10);
                    _nTablecolumn = 10;
                    _oPdfPTable.SetWidths(new float[] { 20f, //SL
                                                   50f, //LC no
                                                   50f, //Master LC no
                                                   75f, //buyer name
                                                   70f, //concern person
                                                   45f, //lc date
                                                   70f, //nego bnjk
                                                   75f, //issue bank
                                                   60f, //qty
                                                   60f //value
                });
                }
                    
            }
            else if (LCReportLevel == (int)EnumLCReportLevel.LCVersionLevel)
            {
              
                if (!bPrintWithMLC)
                {
                    _oPdfPTable = new PdfPTable(11);
                    _nTablecolumn = 11;
                    _oPdfPTable.SetWidths(new float[] { 20f, //SL
                                                   50f, //LC no
                                                   80f, //buyer name
                                                   70f, //concern person
                                                   50f, //lc date
                                                   40f, //Amendment No
                                                   50f, //Amendment Date
                                                   70f, //nego bnjk
                                                   75f, //issue bank
                                                   50f, //qty
                                                   60f //value
                });
                }
                else
                {
                    _oPdfPTable = new PdfPTable(12);
                    _nTablecolumn = 12;
                    _oPdfPTable.SetWidths(new float[] { 20f, //SL
                                                   50f, //LC no
                                                   50f, //MLC no
                                                   80f, //buyer name
                                                   70f, //concern person
                                                   50f, //lc date
                                                   40f, //Amendment No
                                                   50f, //Amendment Date
                                                   70f, //nego bnjk
                                                   75f, //issue bank
                                                   50f, //qty
                                                   60f //value
                });
                }
                _oPdfPTable.WidthPercentage = 100;
                _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else if (LCReportLevel == (int)EnumLCReportLevel.PILevel)
            {
         
                if (!bPrintWithMLC)
                {
                    _oPdfPTable = new PdfPTable(13);
                    _nTablecolumn = 13;
                    _oPdfPTable.SetWidths(new float[] { 20f, //SL
                                                   50f, //LC no
                                                   65f, //buyer name
                                                   65f, //concern person
                                                   45f, //lc date
                                                   40f, //Amendment No
                                                   50f, //Amendment Date
                                                   40f, //PI No
                                                   45f, //PI Date
                                                   70f, //nego bnjk
                                                   70f, //issue bank
                                                   45f, //qty
                                                   55f //value
                });
                }else
                {
                        _oPdfPTable = new PdfPTable(14);
                        _nTablecolumn = 14;
                        _oPdfPTable.SetWidths(new float[] { 20f, //SL
                                                       50f, //LC no
                                                       50f, //MLC no
                                                       65f, //buyer name
                                                       65f, //concern person
                                                       45f, //lc date
                                                       40f, //Amendment No
                                                       50f, //Amendment Date
                                                       40f, //PI No
                                                       45f, //PI Date
                                                       70f, //nego bnjk
                                                       70f, //issue bank
                                                       45f, //qty
                                                       55f //value
                    });
                }

                _oPdfPTable.WidthPercentage = 100;
                _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            }else
            {
              
               

                if (!bPrintWithMLC)
                {
                        _oPdfPTable = new PdfPTable(14);
                        _nTablecolumn = 14;
                        _oPdfPTable.SetWidths(new float[] { 20f, //SL
                                                       50f, //LC no
                                                       55f, //buyer name
                                                       60f, //concern person
                                                       45f, //lc date
                                                       40f, //Amendment No
                                                       50f, //Amendment Date
                                                       40f, //PI No
                                                       45f, //PI Date
                                                       55f, //Product Name
                                                       60f, //nego bnjk
                                                       55f, //issue bank
                                                       40f, //qty
                                                       50f //value
                    });
                }else
                {
                    _oPdfPTable = new PdfPTable(15);
                    _nTablecolumn = 15;
                    _oPdfPTable.SetWidths(new float[] { 20f, //SL
                                                       50f, //LC no
                                                       50f, //M LC no
                                                       55f, //buyer name
                                                       60f, //concern person
                                                       45f, //lc date
                                                       40f, //Amendment No
                                                       50f, //Amendment Date
                                                       40f, //PI No
                                                       45f, //PI Date
                                                       55f, //Product Name
                                                       60f, //nego bnjk
                                                       55f, //issue bank
                                                       40f, //qty
                                                       50f //value
                    });
                }
                _oPdfPTable.WidthPercentage = 100;
                _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            }
        
            #endregion

            this.PrintHeader(LCReportLevel);
            this.PrintBody(LCReportLevel, bPrintWithMLC);
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(int LCReportLevel)
        {
            #region CompanyHeader
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 100f, 335f, 100f });
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(95f, 28f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.FixedHeight = 28;
                _oPdfPCell.Rowspan = 2;
                _oPdfPCell.Border = 0;
                oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                oPdfPTable.AddCell(_oPdfPCell);
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            //insert in main table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTablecolumn; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("Export LC Report (" + LCReportLevelObj.GetLCReportLevelName((EnumLCReportLevel)LCReportLevel) + " )", _oFontStyle));
            _oPdfPCell = new PdfPCell(new Phrase("Export LC Report (" + EnumObject.jGet((EnumLCReportLevel)LCReportLevel) + " )", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.Colspan = _nTablecolumn; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


        }
        #endregion


        #region Report Body
        private void PrintBody(int LCReportLevel, bool bPrintWithMLC)
        {

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("LC No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            if (bPrintWithMLC)
            {
                _oPdfPCell = new PdfPCell(new Phrase("MasterLC No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("C. Person", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("LC Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            if (LCReportLevel == (int)EnumLCReportLevel.LCVersionLevel)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Amdt. No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amdt. Date", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }else  if (LCReportLevel == (int)EnumLCReportLevel.PILevel)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Amdt. No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amdt. Date", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PI Date", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else if (LCReportLevel == (int)EnumLCReportLevel.ProductLevel)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Amdt. No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amdt. Date", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PI Date", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Product", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("Nego. Bank", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Issue Bank", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();


            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, 0);
            foreach (ExportLCReport oItem in _oExportLCReports)
            {
                nCount++;

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                if (bPrintWithMLC)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.MasterLCNos, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPersonName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpenDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                if (LCReportLevel == (int)EnumLCReportLevel.LCVersionLevel)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AmendmentNoWithExportLCID.Split('~')[0].ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AmendmentDateSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else if (LCReportLevel == (int)EnumLCReportLevel.PILevel)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AmendmentNoWithExportLCID.Split('~')[0].ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AmendmentDateSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PIDateSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else if (LCReportLevel == (int)EnumLCReportLevel.ProductLevel)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AmendmentNoWithExportLCID.Split('~')[0].ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AmendmentDateSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PIDateSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }

               
                _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Issue, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            #region Total Print

            if (LCReportLevel == (int)EnumLCReportLevel.LCLevel)
            {

                _oPdfPCell = new PdfPCell(new Phrase("Total ", _oFontStyle)); if (bPrintWithMLC) { _oPdfPCell.Colspan = 8; } else{ _oPdfPCell.Colspan = 7; }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else if (LCReportLevel == (int)EnumLCReportLevel.LCVersionLevel)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Total ", _oFontStyle));
                if (bPrintWithMLC) { _oPdfPCell.Colspan = 10; } else { _oPdfPCell.Colspan = 9; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else if (LCReportLevel == (int)EnumLCReportLevel.PILevel)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Total ", _oFontStyle));
                if (bPrintWithMLC) { _oPdfPCell.Colspan = 12; } else { _oPdfPCell.Colspan = 11; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else if (LCReportLevel == (int)EnumLCReportLevel.ProductLevel)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Total ", _oFontStyle));
                if (bPrintWithMLC) { _oPdfPCell.Colspan = 13; } else { _oPdfPCell.Colspan = 12; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCReports.Sum(x=>x.Qty)) , _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCReports.Sum(x => x.Amount)), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            
            _oPdfPTable.CompleteRow();
            #endregion

        }

 
        #endregion
        #endregion

    }
}
