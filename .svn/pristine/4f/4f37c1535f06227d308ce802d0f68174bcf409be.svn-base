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
using ICS.Core.Framework;
using System.Linq;

namespace ESimSol.Reports
{
   public class rptCommissionBalaceBillWise
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(9);
        PdfPCell _oPdfPCell;
        int _nColumns = 9;
        public iTextSharp.text.Image _oImag { get; set; }
        MemoryStream _oMemoryStream = new MemoryStream();
        SalesCommissionPayable _oSalesCommissionPayable = new SalesCommissionPayable();
        List<SalesCommissionPayable> _oSalesCommissionPayables = new List<SalesCommissionPayable>();
        List<SalesCommissionPayable> _oTempSalesCommissionPayables = new List<SalesCommissionPayable>();
        Company _oCompany = new Company();
        int _nRow = 0;
        int _nRowCount = 0;
        #endregion


        public byte[] PrepareReport(List<SalesCommissionPayable> oSalesCommissionPayables, Company oCompany)
        {
            _oSalesCommissionPayables = oSalesCommissionPayables;
           
            _oCompany = oCompany;
           
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(PageSize.A4.Rotate());
            _oDocument.SetMargins(5f, 5f, 10f, 10f);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 98;
            _oPdfPTable.SetWidths(new float[] { 20f, 55f, 75f, 75f, 55f, 50f, 55f ,55f ,55f}); //height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            #endregion

            this.PrintHeader();
            //this.PrintBody();

            _oPdfPTable.HeaderRows = 3;
            PrintCommissionBalaceBillWise();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(160f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Commission Balance Report", _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body

       
        private void PrintCommissionBalaceBillWise()
        {
            if (_oSalesCommissionPayables.Count <= 0 || _oSalesCommissionPayables[0].ErrorMessage != "")
            {
                string masg = "Nothing to print";
                if (_oSalesCommissionPayables[0].ErrorMessage != "") { masg = ""; masg = _oSalesCommissionPayables[0].ErrorMessage; }
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(masg));
                _oPdfPCell.Colspan = 15;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
              
                _oSalesCommissionPayables = _oSalesCommissionPayables.OrderBy(x => x.ContractorName).ThenBy(x => x.CPName).ToList();
                _oSalesCommissionPayables.ForEach(x => _oTempSalesCommissionPayables.Add(x));
               // this.PrintHeader();
                this.PrintHeaderRowInBody(_oSalesCommissionPayables[0]);
              
                while (_oSalesCommissionPayables.Count > 0)
                {
                    var oResults = _oSalesCommissionPayables.Where(x => x.ContractorName == _oSalesCommissionPayables[0].ContractorName && x.CPName == _oSalesCommissionPayables[0].CPName).ToList();
                    _oSalesCommissionPayables.RemoveAll(x => x.ContractorName == oResults[0].ContractorName && x.CPName == oResults[0].CPName );
               
                    this.PrintBody(oResults);
                }

                PrintGT();
            
            }
        }
        private void PrintHeaderRowInBody(SalesCommissionPayable oSalesCommissionPayable)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("Factory Name :" + oSalesCommissionPayable.ContractorName, _oFontStyle)); _oPdfPCell.Colspan = 9; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Person Name :" + oSalesCommissionPayable.CPName, _oFontStyle)); _oPdfPCell.Colspan = 9; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/C Date", _oFontStyle)); 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("LDBC No", _oFontStyle)); 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Commission", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("C.Approved", _oFontStyle)); 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Adjustment", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Payable", _oFontStyle)); 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Paid Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            
        }

        int RowHeight = 20;
        private void PrintBody(List<SalesCommissionPayable> oSalesCommissionPayables)
        {
            var Currency = oSalesCommissionPayables[0].Currency;
            int nCount = 0;
            double TotalCommissionAmount = 0;
            double TotalMaturityAmount = 0;
            double TotalRealizeAmount = 0;
            double TotalAdjAdd = 0;
            double TotalAdiDUct = 0;
            foreach (SalesCommissionPayable oItem in oSalesCommissionPayables)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                nCount++;
              
               
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle)); 
                _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpeningDateStr, _oFontStyle)); 
                _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCNo, _oFontStyle));
                _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Currency+Global.MillionFormat(oItem.CommissionAmount).ToString(), _oFontStyle)); 
                _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; 
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(oItem.MaturityAmount).ToString() + Currency, _oFontStyle)); 
                _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; 
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(oItem.RealizeAmount).ToString() , _oFontStyle));
                _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(oItem.AdjAdd), _oFontStyle));
                _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(oItem.AdjDeduct + oItem.AdjPayable), _oFontStyle));
                _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                TotalCommissionAmount = TotalCommissionAmount + oItem.CommissionAmount;
                TotalMaturityAmount = TotalMaturityAmount + oItem.MaturityAmount;
                TotalRealizeAmount = TotalRealizeAmount + oItem.RealizeAmount;
             }
            _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight; _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalCommissionAmount).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



             _oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalCommissionAmount).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);




            _oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalRealizeAmount).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalAdjAdd).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);




            _oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalAdiDUct).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPTable.CompleteRow();

             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight; _oPdfPCell.Colspan = 4;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border=0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             _oPdfPTable.AddCell(_oPdfPCell);




             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);




             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPTable.CompleteRow();

             if (_oSalesCommissionPayables.Any()){
                 this.PrintHeaderRowInBody(_oSalesCommissionPayables[0]);
             }
        }
        private void PrintGT()
        {
            var Currency = _oTempSalesCommissionPayables[0].Currency;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            double TotalComAmount = _oTempSalesCommissionPayables.Sum(x => x.CommissionAmount);

            _oPdfPCell = new PdfPCell(new Phrase("Grand TOTAL", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight; _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalComAmount).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalComAmount).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



            double TotalGRealizeAmount = _oTempSalesCommissionPayables.Sum(x => x.RealizeAmount);
            _oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalGRealizeAmount).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            double TotalGAdjAmount = _oTempSalesCommissionPayables.Sum(x => x.AdjAdd);
            _oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalGAdjAmount).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



            double TotalGAdjDuctAmount = _oTempSalesCommissionPayables.Sum(x => (x.AdjDeduct+x.AdjPayable));
            _oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalGAdjDuctAmount).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

        }
           
        }

        

        #endregion


        
        
    }
