using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Utility;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ESimSol.Reports
{
    public class rptMktPersonDetails
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle, _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);//number of columns
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        MktSaleTarget _oMktSaleTarget = new MktSaleTarget();
        List<MktSaleTarget> _oMktSaleTargets = new List<MktSaleTarget>();
        Company _oCompany = new Company();
        string _sMessage = "";
        string MktingHead = "";
        int _nYear = 0;
        int _ViewType = 0;
        List<ExportPI> _oExportPIs = new List<ExportPI>();
        #endregion

        public byte[] PrepareReport(List<MktSaleTarget> oMktSaleTargets, string MktAccountHead, Company oCompany, int ViewType, int sYear, string sMSG, List<ExportPI> oExportPIs)
        {
            _oMktSaleTargets = oMktSaleTargets;
             MktingHead = MktAccountHead;
            _oCompany = oCompany;
            _sMessage = sMSG;
            _nYear = sYear;
            _ViewType = ViewType;
            _oExportPIs = oExportPIs;
            #region Page Setup
            _oDocument = new Document(PageSize.LEGAL, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(20f, 20f, 20f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 800f });
            #endregion
            this.PrintHeader();
            this.PrintBody();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
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
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Projection Report Marketing Person wise for year "+_nYear, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        #region Report Body
        private void PrintBody()
        {
            GetMarketingProjectionTable();
            GetSummery();
        }
        #endregion
        private void GetMarketingProjectionTable()
        {
            PdfPTable oTopTable = new PdfPTable(9);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                                             
                                                 8f,//1  
                                                20f,//2  
                                                20f,//3  
                                                20f,//4  
                                                10f,//5  
                                                10f,//6  
                                                10f,//7  
                                                10f,//8  
                                                20f,//9  
                                            });

            //Header...
            var MktSaleTargets = _oMktSaleTargets.GroupBy(x => new { x.Month,x.Name })
                                                 .OrderBy(x => x.Key.Month)
                                                 .Select(x => new
                                                 {
                                                       Month = x.Key.Month,
                                                       MktPerson = x.Key.Name,
                                                      _MktSaleTargetsList = x.OrderBy(c => c.MarketingAccountID),
                                                       ApproxOrderQty = x.Sum(y => y.OrderQty),
                                                       DeliveryQty = x.Sum(y => y.ReceiveQty)
                                                 });

            foreach (var oData in MktSaleTargets)
            {
                #region initialize
                oTopTable = new PdfPTable(9);
                oTopTable.SetWidths(new float[] {                                             
                                                 8f,//1  
                                                20f,//2  
                                                20f,//3  
                                                20f,//4  
                                                10f,//5  
                                                10f,//6  
                                                10f,//7  
                                                10f,//8  
                                                20f,//9  
                                     
                                            });
                #endregion
                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Projection for month of "+GetMonthWithYear(oData.Month,_nYear), _oFontStyle)); _oPdfPCell.Colspan = 9;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                oTopTable.CompleteRow();
                #region push into main table
                _oPdfPCell = new PdfPCell(oTopTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
                #region initialize
                oTopTable = new PdfPTable(9);
                oTopTable.SetWidths(new float[] {                                             
                                                 8f,//1  
                                                20f,//2  
                                                20f,//3  
                                                20f,//4  
                                                10f,//5  
                                                10f,//6  
                                                10f,//7  
                                                10f,//8  
                                                20f,//9  
                                     
                                            });
                #endregion
                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(oData.MktPerson, _oFontStyle)); _oPdfPCell.Colspan = 9;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                oTopTable.CompleteRow();
                #region push into main table
                _oPdfPCell = new PdfPCell(oTopTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
                #region initialize
                oTopTable = new PdfPTable(9);
                oTopTable.SetWidths(new float[] {                                             
                                                8f,//1  
                                                20f,//2  
                                                20f,//3  
                                                20f,//4  
                                                10f,//5  
                                                10f,//6  
                                                10f,//7  
                                                10f,//8  
                                                20f,//9  
                                     
                                            });
                #endregion

                #region Header
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Composition", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Weave", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Finish", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Approx. Ord Qty", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Delivery Qty", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Tntv Sendling Month", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
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
                int nCount = 1;
                foreach (var oItem in oData._MktSaleTargetsList)
                {
                     #region initialize
                    oTopTable = new PdfPTable(9);
                    oTopTable.SetWidths(new float[] {                                             
                                                 8f,//1  
                                                20f,//2  
                                                20f,//3  
                                                20f,//4  
                                                10f,//5  
                                                10f,//6  
                                                10f,//7  
                                                10f,//8  
                                                20f,//9  
                                     
                                            });
                    #endregion
                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(Convert.ToInt32(nCount).ToString(), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(oItem.Construction, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(oItem.WeaveTypeName, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(oItem.FinishTypeName, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(oItem.EstOrderQtyST, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(oItem.ReceiveQtyST, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
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

                #region SUB TOTAL
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Sub Total", _oFontStyle)); _oPdfPCell.Colspan = 6;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oData.ApproxOrderQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oData.DeliveryQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                #endregion
            }
            #region GRAND TOTAL
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total", _oFontStyle)); _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oMktSaleTargets.Sum(x => x.OrderQty).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oMktSaleTargets.Sum(x => x.ReceiveQty).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            #endregion
        }

        public string GetMonthWithYear(int MonthInInt, int Year)
        {
            if (MonthInInt == 1)
                return "Jan, " + Year;
            if (MonthInInt == 2)
                return "Feb, " + Year;
            if (MonthInInt == 3)
                return "Mar, " + Year;
            if (MonthInInt == 4)
                return "Apr, " + Year;
            if (MonthInInt == 5)
                return "May, " + Year;
            if (MonthInInt == 6)
                return "Jun, " + Year;
            if (MonthInInt == 7)
                return "Jul, " + Year;
            if (MonthInInt == 8)
                return "Aug, " + Year;
            if (MonthInInt == 9)
                return "Sep, " + Year;
            if (MonthInInt == 10)
                return "Oct, " + Year;
            if (MonthInInt == 11)
                return "Nov, " + Year;
            if (MonthInInt == 12)
                return "Dec, " + Year;
            return "";
        }

        private void GetSummery()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 15f, 5f, 5f, 40f, 10f, 10f, 15f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 25, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 15f, 5f, 5f, 40f, 10f, 10f, 15f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Summery", 0, 5, Element.ALIGN_CENTER, BaseColor.WHITE, true, 10, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            #region Heder Info
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 15f, 5f, 5f, 40f, 10f, 10f, 15f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Month", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "No Of PI", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "PI No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Amount", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            #region data
            var data = _oExportPIs.GroupBy(x => new { x.OpeningDate.Month, x.OpeningDate.Year }, (key, grp) => new
            {
                Month = key.Month,
                Year = key.Year,
                NoOfPI = grp.ToList().Count(),
                PIsNo = string.Join(", ", grp.ToList().Select(y => y.PINo)),
                Qty = grp.ToList().Sum(y => y.Qty),
                Amount = grp.ToList().Sum(y => y.Amount)
            }).OrderBy(c => c.Year).ThenBy(c => c.Month);

            foreach (var oItem in data)
            {
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] { 15f, 5f, 5f, 40f, 10f, 10f, 15f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                DateTime dDate = new DateTime(oItem.Year, oItem.Month, 1);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, dDate.ToString("MMM yyyy"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.NoOfPI.ToString("#,##0;(#,##0)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.PIsNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Qty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Amount.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
            #endregion

            #region Total Info
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 15f, 5f, 5f, 40f, 10f, 10f, 15f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, data.Sum(x => x.NoOfPI).ToString("#,##0;(#,##0)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, data.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, data.Sum(x => x.Amount).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }
    }
}
