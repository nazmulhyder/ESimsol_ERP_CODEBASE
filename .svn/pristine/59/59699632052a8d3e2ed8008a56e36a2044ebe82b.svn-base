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
    public class rptFabricLotAssignReport
    {
        #region Declaration
        PdfWriter _oWriter;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        iTextSharp.text.Font _oFontStyleBoldUnderLine;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<FabricLotAssign> _oFabricLotAssigns = new List<FabricLotAssign>();
        List<SelectedField> _oSelectedFields = new List<SelectedField>();
        Company _oCompany = new Company();

        int _nColCount = 10;
        #endregion

        //PrepareReport(List<Object> oObjects, BusinessUnit oBusinessUnit, Company oCompany, string sHeader,List<SelectedField> oSelectedFields)
        public byte[] PrepareReport(List<FabricLotAssign> oFabricLotAssigns, BusinessUnit oBusinessUnit, Company oCompany, string sHeader, List<SelectedField> oSelectedFields)
        {
            _oFabricLotAssigns = oFabricLotAssigns;
            _oCompany = oCompany;
            _oSelectedFields = oSelectedFields;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(10f, 10f, 5f, 20f);

            _nColCount = oSelectedFields.Count();
            _oPdfPTable = new PdfPTable(1);
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
            _oPdfPTable.SetWidths(new float[] {842});
            #endregion

            #region Report Body & Header

            this.PrintHeader(sHeader, "");
            this.PrintBody(1);
            _oPdfPTable.HeaderRows = 2;

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
            oPdfPTable.SetWidths(new float[] { 100f, 400f, 300f });

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

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(sDateRange, _oFontStyle));
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


            #endregion
        }
        #endregion

        public PdfPTable GetDetailsTable(int eReportLayout) 
        {
            PdfPTable oPdfPTable = new PdfPTable(_nColCount);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(_oSelectedFields.Select(x => x.Width).ToArray());
            return oPdfPTable;
        }
      
        #region Report Body Sale Invoice
        private void PrintBody(int eReportLayout)
        {
            #region Layout Wise Header (Declaration)

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            string sHeader = "", sHeaderCoulmn = "";

            sHeader = "Lot Wise"; sHeaderCoulmn = "Lot No : ";

            #endregion

            #region Group By Layout Wise
            var data = _oFabricLotAssigns.GroupBy(x => new { x.LotNo, x.LotID,_nColCount }, (key, grp) => new
            {
                HeaderName = key.LotNo + "   Store : " + grp.Select(x => x.OperationUnitName).FirstOrDefault(),
                TotalQty = grp.Sum(x => x.Qty),
                TotalBalance = grp.Sum(x => x.Balance),
                Results = grp.ToList()
            });
            #endregion

            ESimSolPdfHelper.FontStyle = _oFontStyleBold;

            #region Header
            PdfPTable oPdfPTable = GetDetailsTable(eReportLayout);
           
            foreach(var oHead in _oSelectedFields)
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oHead.Caption, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
           
            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

            #endregion

            #region Print Details

            foreach (var oItem in data)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, sHeaderCoulmn+oItem.HeaderName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

                int nCount = 0, nRowSpan =0, ProductionSheetID = 0;
                
                #region Data
                foreach (var obj in oItem.Results)
                {

                    ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    oPdfPTable = new PdfPTable(10);
                    oPdfPTable = GetDetailsTable(eReportLayout);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (++nCount).ToString(), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.FabricLotAssignDateSt, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ExeNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.BuyerName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ColorName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    //ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.LotNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.Qty_Order), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.WarpWeftTypeSt, Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.Balance), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.Qty_Req), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.Qty_RS), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.Qty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.Qty_Consume), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    oPdfPTable.CompleteRow();
                    ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                }
                #endregion

                #region Sub Total (Layout Wise)
                oPdfPTable = new PdfPTable(11);
                oPdfPTable = GetDetailsTable(eReportLayout );
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, sHeader + " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 6, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Results.Sum(x => x.Qty_Order)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable,"", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Results.Sum(x => x.Balance)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Results.Sum(x => x.Qty_Req)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Results.Sum(x => x.Qty_RS)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Results.Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Results.Sum(x => x.Qty_Consume)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                oPdfPTable.CompleteRow();
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                #endregion
            }
            #region Grand Total
            PdfPTable oPdfPTable_GT = new PdfPTable(12);
            oPdfPTable_GT = GetDetailsTable(eReportLayout );
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, " Grand Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 6, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormat(_oFabricLotAssigns.Sum(x => x.Qty_Order)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, "", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormat(_oFabricLotAssigns.Sum(x => x.Balance)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormat(_oFabricLotAssigns.Sum(x => x.Qty_Req)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormat(_oFabricLotAssigns.Sum(x => x.Qty_RS)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormat(_oFabricLotAssigns.Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormat(_oFabricLotAssigns.Sum(x => x.Qty_Consume)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            oPdfPTable_GT.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable_GT);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            #endregion

            #endregion
        }
        #endregion
    }
}
