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
    public class rptCustomerWiseBulkOrderReport
    {
        #region Declaration
        int _nTotalColumn = 5;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(5);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Fabric _oFabric = new Fabric();
        List<FabricExecutionOrder> _oFEOs = new List<FabricExecutionOrder>();
        Company _oCompany = new Company();
        string _sMessage = "";
        BaseColor _oBC = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EFF0EA"));
        #endregion

        public byte[] PrepareReport(List<FabricExecutionOrder> oFEOs, Company oCompany, string sMessage)
        {
            _oFEOs = oFEOs;
            _oCompany = oCompany;
            _sMessage = sMessage;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 40f, 200f, 100f, 100f, 100f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        public PdfPTable PrepareExcel(List<FabricExecutionOrder> oFEOs, Company oCompany, string sMessage)
        {
            _oFEOs = oFEOs;
            _oCompany = oCompany;
            _sMessage = sMessage;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 40f, 200f, 100f, 100f, 100f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oPdfPTable;
        }
        
        /*
        public MemoryStream PrepareExcel(List<FabricExecutionOrder> oFEOs, Company oCompany, string sMessage)
        {
            _oFEOs = oFEOs;
            _oCompany = oCompany;
            _sMessage = sMessage;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 40f, 200f, 100f, 100f, 100f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            //_oDocument.Close();

            return _oMemoryStream;
        } */

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(160f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.Colspan = _nTotalColumn;
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
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_sMessage, _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumn;
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
        private void PrintBody()
        {
            if (_oFEOs.Count > 0)
            {
                #region Table Header 1
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = _oBC;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = _oBC;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Unit in Mtr", _oFontStyle));
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = _oBC;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("dd MMM yyyy"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = _oBC;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Table Header 2
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

                _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = _oBC;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("CUSTOMER", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = _oBC;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("YARN DYED ORDER", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = _oBC;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("SOLID DYED ORDER", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = _oBC;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("TOTAL QTY", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = _oBC;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Table Body
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                string sBuyerName = "";
                double nYarnDyedOrder = 0,
                       nSolidDyedOrder = 0,
                       nTotalQty = 0,
                       nTotalYarnDyedOrder = 0,
                       nTotalSolidDyedOrder = 0,
                       nGrandTotalQty = 0;

                string[] sBuyerIDs = string.Join(",", _oFEOs.Select(o => o.BuyerID).Distinct()).Split(',');
                List<FabricExecutionOrder> oFEOs = new List<FabricExecutionOrder>();

                int nSL = 0;
                foreach (string sBuyerID in sBuyerIDs)
                {
                    #region Calculations
                    int nBuyerID = Convert.ToInt32(sBuyerID);
                    oFEOs = new List<FabricExecutionOrder>();
                    oFEOs = _oFEOs.Where(o => o.BuyerID == nBuyerID).ToList();
                    sBuyerName = oFEOs[0].BuyerName;
                    nYarnDyedOrder = oFEOs.Where(o => o.ProcessType == (int)EnumFabricProcess.Process && o.IsYarnDyed == true).Select(o => Global.GetMeter(o.Qty, 2)).Sum();
                    nSolidDyedOrder = oFEOs.Where(o => o.ProcessType == 2 && o.IsYarnDyed == false).Select(o => Global.GetMeter(o.Qty, 2)).Sum(); //2 = solid dyed
                    nTotalQty = nYarnDyedOrder + nSolidDyedOrder;

                    nTotalYarnDyedOrder += nYarnDyedOrder;
                    nTotalSolidDyedOrder += nSolidDyedOrder;
                    nGrandTotalQty += nTotalQty;
                    #endregion

                    nSL++;
                    _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(sBuyerName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nYarnDyedOrder), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nSolidDyedOrder), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                #endregion

                #region Total
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Grand Total = ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalYarnDyedOrder), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalSolidDyedOrder), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            else
            {
                #region No List Found
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("No List Found", _oFontStyle));
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
        }
        #endregion
    }
}
