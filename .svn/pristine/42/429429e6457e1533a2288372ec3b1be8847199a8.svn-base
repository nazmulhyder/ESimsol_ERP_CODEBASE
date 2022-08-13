using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace ESimSol.Reports
{
    public class rptDUDeliveryOrderWiseSummary
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        string sHeaderString = "";
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<DUDeliverySummaryRPT> _oDUDeliverySummaryRPTs = new List<DUDeliverySummaryRPT>();
        Company _oCompany = new Company();
        string _sMessage = "";
        int _nReportType = 0;
        int _nColumns = 0;
        #endregion
        public byte[] PrepareReport(List<DUDeliverySummaryRPT> oDUDeliverySummaryRPTs, Company oCompany,int nReportType, string sMessage)
        {
            _oDUDeliverySummaryRPTs = oDUDeliverySummaryRPTs;
            _oCompany = oCompany;
            _sMessage = sMessage;
            _nReportType = nReportType;
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(25f, 25f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oPdfPTable.SetWidths(new float[] { 595f });
            _oDocument.Open();
            this.PrintHeader();
            this.PrintBody();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

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
                _oImag.ScaleAbsolute(70f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region report  header
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(printHeaderReport(_nReportType), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 13f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region date range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_sMessage, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 12f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region reporting date
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Reporting Time: "+DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion
        #region Report Body
        private void PrintBody()
        {

            if (_nReportType == 1)
            {
                PrintDayMonthWise(_oDUDeliverySummaryRPTs);
            }
            if (_nReportType == 2)
            {
                PrintDayMonthWise(_oDUDeliverySummaryRPTs);
            }
            if (_nReportType == 3)
            {
                PrintProductWise(_oDUDeliverySummaryRPTs);
            }
            if (_nReportType == 4)
            {
                PrintContractorWise(_oDUDeliverySummaryRPTs);
            }
            if (_nReportType == 5)
            {
                PrintMktPersonWise(_oDUDeliverySummaryRPTs);
            }
                      
        }
        #endregion
        public void PrintMktPersonWise(List<DUDeliverySummaryRPT> oDUDeliverySummaryRPTs)
        {
            var oDUDSRPTs = oDUDeliverySummaryRPTs.GroupBy(x => new { x.OrderType, x.OrderTypeSt }, (key, grp) =>
                               new
                               {
                                   OrderType = key.OrderType,
                                   OrderTypeName = key.OrderTypeSt
                               }).OrderBy(y => y.OrderType).ToList();

            var _oDUDSRPTs = oDUDeliverySummaryRPTs.GroupBy(x => new {x.RefID, x.RefName}, (key, grp) =>new{
                                     RefID = key.RefID,
                                     RefName = key.RefName,
                                     Results = grp.OrderBy(y => y.OrderType).ToList()
                                 });

            _nColumns = (oDUDSRPTs.Count*2) + 4;
            PdfPTable oTopTable = new PdfPTable(_nColumns);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;

            float[] tablecolumns = new float[_nColumns];
            tablecolumns[0] = 25f;
            tablecolumns[1] = 100f;

            for (int i = 2; i < _nColumns; i++)
            {
                tablecolumns[i] = 60f;
            }

            oTopTable = new PdfPTable(_nColumns);
            oTopTable.SetWidths(tablecolumns);

            #region Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MKT Person", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            foreach(var oItem in oDUDSRPTs)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderTypeName, _oFontStyle)); _oPdfPCell.Colspan = 2;
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);


            foreach (var oItem in oDUDSRPTs)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);           
            }

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);          
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            #region DATA
            int nCount = 1; 
            int _refID = 0;
            int _OrderType = 0;
            double _nTotalQty = 0;
            double _nTotalAmount = 0;
            double _nSubTotalQty = 0;
            double _nSubTotalAmount = 0;

            foreach (var oItem2 in _oDUDSRPTs)
            {
                oTopTable = new PdfPTable(_nColumns);
                oTopTable.WidthPercentage = 100;
                oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
                tablecolumns = new float[_nColumns];
                tablecolumns[0] = 25f;
                tablecolumns[1] = 100f;
                for (int i = 2; i < _nColumns; i++)
                    tablecolumns[i] = 60f;
                oTopTable = new PdfPTable(_nColumns);
                oTopTable.SetWidths(tablecolumns);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem2.RefName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _nTotalQty = 0; _nTotalAmount = 0;
                foreach (var oItem in oDUDSRPTs)
                {
                    bool isExist = false;
                    foreach (var res in oItem2.Results)
                    {
                        if (oItem.OrderType == res.OrderType)
                        {
                            _nSubTotalQty = res.QtyIn + res.QtyOut;
                            _oPdfPCell = new PdfPCell(new Phrase(_nSubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                            _nSubTotalAmount = res.AmountIn + res.AmountOut;
                            _oPdfPCell = new PdfPCell(new Phrase(_nSubTotalAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                            _nTotalQty += _nSubTotalQty;
                            _nTotalAmount += _nSubTotalAmount;
                            isExist = true;
                        }
   
                    }
                    if (!isExist)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    }

                }

                 _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_nTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_nTotalAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                nCount++;
                oTopTable.CompleteRow();
                #region push into main table
                _oPdfPCell = new PdfPCell(oTopTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }
            #endregion

        }
        public void PrintContractorWise(List<DUDeliverySummaryRPT> oDUDeliverySummaryRPTs)
        {
            var oDUDSRPTs = oDUDeliverySummaryRPTs.GroupBy(x => new { x.OrderType, x.OrderTypeSt }, (key, grp) =>
                               new
                               {
                                   OrderType = key.OrderType,
                                   OrderTypeName = key.OrderTypeSt
                               }).OrderBy(y => y.OrderType).ToList();

            var _oDUDSRPTs = oDUDeliverySummaryRPTs.GroupBy(x => new { x.RefID, x.RefName }, (key, grp) => new
            {
                RefID = key.RefID,
                RefName = key.RefName,
                Results = grp.OrderBy(y => y.OrderType).ToList()
            });

            _nColumns = (oDUDSRPTs.Count * 2) + 4;
            PdfPTable oTopTable = new PdfPTable(_nColumns);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;

            float[] tablecolumns = new float[_nColumns];
            tablecolumns[0] = 25f;
            tablecolumns[1] = 100f;

            for (int i = 2; i < _nColumns; i++)
            {
                tablecolumns[i] = 60f;
            }

            oTopTable = new PdfPTable(_nColumns);
            oTopTable.SetWidths(tablecolumns);

            #region Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Customer", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            foreach (var oItem in oDUDSRPTs)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderTypeName, _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);


            foreach (var oItem in oDUDSRPTs)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            #region DATA
            int nCount = 1;
            int _refID = 0;
            int _OrderType = 0;
            double _nTotalQty = 0;
            double _nTotalAmount = 0;
            double _nSubTotalQty = 0;
            double _nSubTotalAmount = 0;

            foreach (var oItem2 in _oDUDSRPTs)
            {
                oTopTable = new PdfPTable(_nColumns);
                oTopTable.WidthPercentage = 100;
                oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
                tablecolumns = new float[_nColumns];
                tablecolumns[0] = 25f;
                tablecolumns[1] = 100f;
                for (int i = 2; i < _nColumns; i++)
                    tablecolumns[i] = 60f;
                oTopTable = new PdfPTable(_nColumns);
                oTopTable.SetWidths(tablecolumns);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem2.RefName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _nTotalQty = 0; _nTotalAmount = 0;
                foreach (var oItem in oDUDSRPTs)
                {
                    bool isExist = false;
                    foreach (var res in oItem2.Results)
                    {
                        if (oItem.OrderType == res.OrderType)
                        {
                            _nSubTotalQty = res.QtyIn + res.QtyOut;
                            _oPdfPCell = new PdfPCell(new Phrase(_nSubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                            _nSubTotalAmount = res.AmountIn + res.AmountOut;
                            _oPdfPCell = new PdfPCell(new Phrase(_nSubTotalAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                            _nTotalQty += _nSubTotalQty;
                            _nTotalAmount += _nSubTotalAmount;
                            isExist = true;
                        }

                    }
                    if (!isExist)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    }

                }

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_nTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_nTotalAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                nCount++;
                oTopTable.CompleteRow();
                #region push into main table
                _oPdfPCell = new PdfPCell(oTopTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }
            #endregion

        }
        public void PrintProductWise(List<DUDeliverySummaryRPT> oDUDeliverySummaryRPTs)
        {
            var oDUDSRPTs = oDUDeliverySummaryRPTs.GroupBy(x => new { x.OrderType, x.OrderTypeSt }, (key, grp) =>
                               new
                               {
                                   OrderType = key.OrderType,
                                   OrderTypeName = key.OrderTypeSt
                               }).OrderBy(y => y.OrderType).ToList();

            var _oDUDSRPTs = oDUDeliverySummaryRPTs.GroupBy(x => new { x.RefID, x.RefName }, (key, grp) => new
            {
                RefID = key.RefID,
                RefName = key.RefName,
                Results = grp.OrderBy(y => y.OrderType).ToList()
            });

            _nColumns = (oDUDSRPTs.Count * 2) + 4;
            PdfPTable oTopTable = new PdfPTable(_nColumns);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;

            float[] tablecolumns = new float[_nColumns];
            tablecolumns[0] = 25f;
            tablecolumns[1] = 100f;

            for (int i = 2; i < _nColumns; i++)
            {
                tablecolumns[i] = 60f;
            }

            oTopTable = new PdfPTable(_nColumns);
            oTopTable.SetWidths(tablecolumns);

            #region Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            foreach (var oItem in oDUDSRPTs)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderTypeName, _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);


            foreach (var oItem in oDUDSRPTs)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            #region DATA
            int nCount = 1;
            int _refID = 0;
            int _OrderType = 0;
            double _nTotalQty = 0;
            double _nTotalAmount = 0;
            double _nSubTotalQty = 0;
            double _nSubTotalAmount = 0;

            foreach (var oItem2 in _oDUDSRPTs)
            {
                oTopTable = new PdfPTable(_nColumns);
                oTopTable.WidthPercentage = 100;
                oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
                tablecolumns = new float[_nColumns];
                tablecolumns[0] = 25f;
                tablecolumns[1] = 100f;
                for (int i = 2; i < _nColumns; i++)
                    tablecolumns[i] = 60f;
                oTopTable = new PdfPTable(_nColumns);
                oTopTable.SetWidths(tablecolumns);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem2.RefName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _nTotalQty = 0; _nTotalAmount = 0;
                foreach (var oItem in oDUDSRPTs)
                {
                    bool isExist = false;
                    foreach (var res in oItem2.Results)
                    {
                        if (oItem.OrderType == res.OrderType)
                        {
                            _nSubTotalQty = res.QtyIn + res.QtyOut;
                            _oPdfPCell = new PdfPCell(new Phrase(_nSubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                            _nSubTotalAmount = res.AmountIn + res.AmountOut;
                            _oPdfPCell = new PdfPCell(new Phrase(_nSubTotalAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                            _nTotalQty += _nSubTotalQty;
                            _nTotalAmount += _nSubTotalAmount;
                            isExist = true;
                        }

                    }
                    if (!isExist)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    }

                }

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_nTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_nTotalAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                nCount++;
                oTopTable.CompleteRow();
                #region push into main table
                _oPdfPCell = new PdfPCell(oTopTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }
            #endregion

        }
        public void PrintDayMonthWise(List<DUDeliverySummaryRPT> oDUDeliverySummaryRPTs)
        {
            var oDUDSRPTs = oDUDeliverySummaryRPTs.GroupBy(x => new { x.OrderType, x.OrderTypeSt }, (key, grp) =>
                               new
                               {
                                   OrderType = key.OrderType,
                                   OrderTypeName = key.OrderTypeSt
                               }).OrderBy(y => y.OrderType).ToList();

            var _oDUDSRPTs = oDUDeliverySummaryRPTs.GroupBy(x => new {x.RefID,x.RefName }, (key,grp) => new
            {
                RefName = key.RefName,
                Results = grp.OrderBy(y => y.OrderType).ToList()
            });

            _nColumns = (oDUDSRPTs.Count * 2) + 4;
            PdfPTable oTopTable = new PdfPTable(_nColumns);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;

            float[] tablecolumns = new float[_nColumns];
            tablecolumns[0] = 25f;
            tablecolumns[1] = 100f;

            for (int i = 2; i < _nColumns; i++)
            {
                tablecolumns[i] = 60f;
            }

            oTopTable = new PdfPTable(_nColumns);
            oTopTable.SetWidths(tablecolumns);

            #region Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            if (_nReportType == 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Month", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            }
            if (_nReportType == 2)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Day", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            }
            foreach (var oItem in oDUDSRPTs)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderTypeName, _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);


            foreach (var oItem in oDUDSRPTs)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            #region DATA
            int nCount = 1;
            int _refID = 0;
            int _OrderType = 0;
            double _nTotalQty = 0;
            double _nTotalAmount = 0;
            double _nSubTotalQty = 0;
            double _nSubTotalAmount = 0;

            foreach (var oItem2 in _oDUDSRPTs)
            {
                oTopTable = new PdfPTable(_nColumns);
                oTopTable.WidthPercentage = 100;
                oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
                tablecolumns = new float[_nColumns];
                tablecolumns[0] = 25f;
                tablecolumns[1] = 100f;
                for (int i = 2; i < _nColumns; i++)
                    tablecolumns[i] = 60f;
                oTopTable = new PdfPTable(_nColumns);
                oTopTable.SetWidths(tablecolumns);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem2.RefName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _nTotalQty = 0; _nTotalAmount = 0;
                foreach (var oItem in oDUDSRPTs)
                {
                    bool isExist = false;
                    foreach (var res in oItem2.Results)
                    {
                        if (oItem.OrderType == res.OrderType)
                        {
                            _nSubTotalQty = res.QtyIn + res.QtyOut;
                            _oPdfPCell = new PdfPCell(new Phrase(_nSubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                            _nSubTotalAmount = res.AmountIn + res.AmountOut;
                            _oPdfPCell = new PdfPCell(new Phrase(_nSubTotalAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                            _nTotalQty += _nSubTotalQty;
                            _nTotalAmount += _nSubTotalAmount;
                            isExist = true;
                        }

                    }
                    if (!isExist)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    }

                }

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_nTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_nTotalAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                nCount++;
                oTopTable.CompleteRow();
                #region push into main table
                _oPdfPCell = new PdfPCell(oTopTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }
            #endregion

        }
        public string printHeaderReport(int nReportType)
        {
            if (nReportType == 1)
            {
                return "Month Wise Report";
            }
            if (nReportType == 2)
            {
                return "Day Wise Report";
            }
            if (nReportType == 3)
            {
                return "Product Wise Report";
            }
            if (nReportType == 4)
            {
                return "Contractor Wise Report";
            }
            if (nReportType == 5)
            {
                return "MKT Person Wise Report";
            }
           
            return "";
            
        }

    }
}
