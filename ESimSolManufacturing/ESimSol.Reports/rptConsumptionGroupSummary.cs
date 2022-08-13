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
using ESimSol.Reports;

namespace ESimSol.BusinessObjects
{
    public class rptConsumptionGroupSummary
    {
        #region Declaration
        PdfWriter _oWriter;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        iTextSharp.text.Font _oFontStyleBoldUnderLine;
        PdfPTable _oPdfPTable = new PdfPTable(7);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<ConsumptionReport> _oConsumptionReports = new List<ConsumptionReport>();
        Company _oCompany = new Company();
        EnumReportLayout _eEnumReportLayout;
        string _sShiftName = "";
        bool _bIsRateView = false;
        #endregion

        public byte[] PrepareReport(List<ConsumptionReport> oConsumptionReports, Company oCompany, string sDateRange, string ShiftName, int ReportLayout, bool bIsRateView)
        {
            _oConsumptionReports = oConsumptionReports;
            _sShiftName = ShiftName;
            _oCompany = oCompany;
            _eEnumReportLayout = (EnumReportLayout)ReportLayout;
            _bIsRateView = bIsRateView;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4); //595X842
            _oDocument.SetMargins(20f, 20f, 5f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                                30f,                   
                                                                55f,     //product code
                                                                210f,    //product name
                                                                30f,    //unit 
                                                                50f,    //unit price
                                                                60f,    //issue qty
                                                                80f    //consumption value  
            });
            #endregion

            #region Report Body & Header
            this.PrintHeader("Consumption Group Summary ", sDateRange);
            this.PrintBody(); 
            
            _oPdfPTable.HeaderRows = 3;
            #endregion
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }


        #region Report Header
        private void PrintHeader(string sReportHeader, string sDateRange)
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
                _oImag.ScaleAbsolute(55f, 25f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase(sReportHeader, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Address & Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(sDateRange, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Phone Number
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Colspan = 7;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 7;
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 6f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

     
            #endregion
        }
        #endregion

        #region body

        private void PrintBody()
        {
            #region head
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("U. Price", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(GetConsumptionWiseTable()); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; //_oPdfPCell.FixedHeight = 150;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }

        private PdfPTable GetConsumptionWiseTable()
        {
            PdfPTable oConsumptionWiseTable = new PdfPTable(7);
            oConsumptionWiseTable.WidthPercentage = 100;
            oConsumptionWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oConsumptionWiseTable.SetWidths(new float[] { 
                                                                30f,                   
                                                                55f,     //product code
                                                                210f,    //product name
                                                                30f,    //unit 
                                                                50f,    //unit price
                                                                60f,    //issue qty
                                                                80f    //consumption value  
            });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            
           #region group by
            if (_oConsumptionReports.Count > 0)
            {
                var data = _oConsumptionReports.GroupBy(x => new { x.CUGroupName }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
                {
                    CUGroupName = key.CUGroupName,
                    Results = grp.ToList() //All data
                });
            #endregion
           #region body



                int nConsumptionUnitID = -1; int nCount = 0; double nSubTotal = 0; double nGrandTotal = 0; double nSubTotalQty = 0; double nGrandTotalQty = 0;
                int nParentConsumptionUnitID = -1;



                foreach (var oData in data)
                {



                    #region subtotal
                    if (nSubTotal > 0)
                    {
                        //if (sQCNo != oItem.QCNo && count > 1)
                        //{
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 5;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nSubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nSubTotal.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase(SubTotalRejectQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                        oConsumptionWiseTable.CompleteRow();
                        nSubTotal = 0; nSubTotalQty = 0;
                        //}
                    }
                    #endregion


                    _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase(oData.CUGroupName, _oFontStyle)); _oPdfPCell.Colspan = 7;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);
                    oConsumptionWiseTable.CompleteRow();

                    int count = 0; int num = 0;
                   
                    foreach (var oItem in oData.Results)
                    {
                        count++; //num++;
                        
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);

                        _oPdfPCell = new PdfPCell(new Phrase(count.ToString(), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductCode, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                        
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.MUnitSymbol, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitPrice.ToString("###,0.00"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.IssueQty.ToString("###,0.00"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ConsumptionValue.ToString("###,0.00"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                        oConsumptionWiseTable.CompleteRow();

                        nSubTotal = nSubTotal + oItem.ConsumptionValue;
                        nSubTotalQty = nSubTotalQty + oItem.IssueQty;
                        nGrandTotal = nGrandTotal + oItem.ConsumptionValue;
                        nGrandTotalQty = nGrandTotalQty + oItem.IssueQty;
                        
                    }
                   
                    //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 7; _oPdfPCell.FixedHeight = 10; _oPdfPCell.Border = 0;
                    //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                }

                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 5;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTotal.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                oConsumptionWiseTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 5;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nGrandTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nGrandTotal.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConsumptionWiseTable.AddCell(_oPdfPCell);

                oConsumptionWiseTable.CompleteRow();



            }
            #endregion
            return oConsumptionWiseTable;
        }
        #endregion
    }
}
