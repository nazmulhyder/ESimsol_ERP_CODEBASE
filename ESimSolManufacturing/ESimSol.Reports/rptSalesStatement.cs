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
   public class rptSalesStatement
    {
        #region Declaration

        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_Bold;
        string temp;
        public iTextSharp.text.Image _oImag { get; set; }
        //int _nTotalColumn = 12;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        SalesStatement oSalesStatement = new SalesStatement();
        BusinessUnit oBusinessUnit = new BusinessUnit();
        List<SalesStatement> _oSalesStatements = new List<SalesStatement>();
        Company _oCompany = new Company();
        List<SalesStatement> _oSalesStatement_EBills = new List<SalesStatement>();
        String _sTemp = string.Empty;
        #endregion

        public byte[] PrepareReport(List<SalesStatement> oSalesStatement_EBills, List<SalesStatement> oSalesStatements, Company oCompany, string sTemp, BusinessUnit businessUnit)
        {
            temp = sTemp;
            oBusinessUnit = businessUnit;
            _oSalesStatements = oSalesStatements;
            _oSalesStatement_EBills = oSalesStatement_EBills;
            _oCompany = oCompany;
            _sTemp = sTemp;
            #region Page Setup
            _oDocument = new Document(PageSize.A4.Rotate(), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(5f, 5f, 10f, 10f);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //ESimSolFooter PageEventHandler = new ESimSolFooter();
            //PageEventHandler.signatures = new List<string>(new string[] { "Pre.By", "Sr. QCO", "AM(QC)", "DM(QC)", "ED" });
            //PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            
            this.PrintBody_ExportBill();
            this.Print_Body_BillMaturity();
            this.Print_Body_ExportBillRealize();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            if (oBusinessUnit.BusinessUnitID > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(oBusinessUnit.Name, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.FactoryAddress, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            #endregion
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Sales Satement", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
         
            //DateTime dtStartDate = Convert.ToDateTime(_sTemp.Split('~')[1]);
            //DateTime dtEndDate = Convert.ToDateTime(_sTemp.Split('~')[2]);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase("Report on Date : " + dtStartDate.ToString("dd MMM yyyy") + " From " + dtEndDate.ToString("dd MMM yyyy"), _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
           
        }
        #endregion
        #region Report Body
        private void PrintBody()
        {
            //#region Blank Space
            //_oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion

            DateTime dtStartDate = Convert.ToDateTime(_sTemp.Split('~')[1]);
            DateTime dtEndDate = Convert.ToDateTime(_sTemp.Split('~')[2]);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Report on Date : " + dtStartDate.ToString("dd MMM yyyy") + " From " + dtEndDate.ToString("dd MMM yyyy"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            PdfPTable oPdfPTable = new PdfPTable(14);

            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 28f, 32f, 32f, 32f, 32f, 32f, 32f, 32f, 32f, 32f, 32f, 32f, 32f, 32f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);

            #region Table Header
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;   oPdfPTable.AddCell(oPdfPCell);
          
            oPdfPCell = new PdfPCell(new Phrase("Delivary", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;oPdfPCell.Colspan =5 ; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("PI Issued", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;oPdfPCell.Colspan =5 ; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("LC Received", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPCell.Colspan = 3; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(" Unit ", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("Sale Budget", _oFontStyle));
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Production ", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("In House Delivery ", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);
            
            oPdfPCell = new PdfPCell(new Phrase("Out Delivery ", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Total Delivery Qty", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Value(USD)", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("Value(BDT)", _oFontStyle));
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("No of P/I", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Quantity", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Value(USD)", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("Value(BDT)", _oFontStyle));
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Cash Sales", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Cash Sales Value", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("No of L/C", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Quantity", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);
           
            oPdfPCell = new PdfPCell(new Phrase("Value(USD)", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("Value(BDT)", _oFontStyle));
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion
            #region Table Body

            foreach (var oitem in _oSalesStatements)
            {

            oPdfPCell = new PdfPCell(new Phrase(oitem.BUName, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;  oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Amount_SaleBudget), _oFontStyle));
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;  oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Qty_Production), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Qty_Delivery_In), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Qty_Delivery_Out), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Qty_Delivery_In + oitem.Qty_Delivery_Out), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Amount_Delivery), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase((oitem.Count_PI + oitem.Count_Cash).ToString(), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Qty_PI), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Amount_PI), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Amount_PI*78), _oFontStyle));
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Qty_Cash), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Amount_Cash), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oitem.Count_LC.ToString(), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Qty_LC), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Amount_LC), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Amount_LC*78), _oFontStyle));
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            }
            #endregion
            #region Table Sum
            oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatements.Sum(p=>p.Amount_SaleBudget)), _oFontStyle_Bold));
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatements.Sum(p => p.Qty_Production)), _oFontStyle_Bold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatements.Sum(p => p.Qty_Delivery_In)), _oFontStyle_Bold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            
            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatements.Sum(p => p.Qty_Delivery_Out)), _oFontStyle_Bold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatements.Sum(p => (p.Qty_Delivery_Out + p.Qty_Delivery_In))), _oFontStyle_Bold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatements.Sum(p => p.Amount_Delivery)), _oFontStyle_Bold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatements.Sum(p => (p.Count_PI+p.Count_Cash))), _oFontStyle_Bold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatements.Sum(p => (p.Qty_PI))), _oFontStyle_Bold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatements.Sum(p => p.Amount_PI)), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatements.Sum(p => p.Amount_Cash)), _oFontStyle_Bold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatements.Sum(p => p.Count_LC)), _oFontStyle_Bold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatements.Sum(p => p.Qty_LC)), _oFontStyle_Bold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatements.Sum(p => p.Amount_LC)), _oFontStyle_Bold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();



        }
        private void PrintBody_ExportBill()
        {
            PdfPTable oPdfPTable = new PdfPTable(15);

            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 25f, 40f, 32f, 32f, 32f, 32f, 32f, 34f, 32f, 32f, 32f, 32f, 32f, 32f, 32f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.MinimumHeight = 10f; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.Colspan = 15; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Export L/C Status (Amount in USD)", _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.Colspan = 15; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

          

            #region Table Header
            oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle));
            oPdfPCell.Rowspan = 2; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Bank ", _oFontStyle));
            oPdfPCell.Rowspan = 2; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Back To Back Position", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPCell.Colspan = 4; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Bank Negotiation", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPCell.Colspan = 2; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Bank Accepted Bill", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPCell.Colspan = 3; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Payment Receive", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPCell.Colspan = 3; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            oPdfPCell.Rowspan = 2; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

          

            oPdfPCell = new PdfPCell(new Phrase("L/C In Hand", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("BOE in Hand ", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("In Party Hand ", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Acceptad Bill", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Nego Transit", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("NegotiatedBill", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Due Payment", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Over Due", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Total Due", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Discounted", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Payment Rec", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("B. FDD Rec", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);
           
          

           
            oPdfPTable.CompleteRow();
            #endregion
            #region Table Body
            string  nCurrency = "";
            //List<int> oUnit = new List<int>();
            //List<int> oBank = new List<int>();
            //List<ExportBill> oExportBills = new List<ExportBill>();
            //oBank = _oExportBills.Select(o => o.BankBranchID_Nego).Distinct().ToList();
            //oUnit = _oExportBills.Select(o => (int)o.TextileUnit).Distinct().ToList();
            //oExportBills = _oExportBills.Where(o => o.BankBranchID_Nego == oitemTwo && (int)o.TextileUnit == oitem).ToList();

            if (_oSalesStatement_EBills.Count > 0)
            {
                foreach (var oitem in _oSalesStatement_EBills)
                {

                        oPdfPCell = new PdfPCell(new Phrase(oitem.BUName, _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(oitem.BankName, _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Amount_LC), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat_Round(oitem.BOinHand), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.BOInCusHand), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat_Round(oitem.AcceptadBill), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.NegoTransit), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat_Round(oitem.NegotiatedBill), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Amount_Due), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat_Round(oitem.Amount_ODue), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Amount_Due + oitem.Amount_ODue), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.Discounted), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oitem.PaymentDone), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat_Round(oitem.BFDDRecd), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat_Round(oitem.Amount_LC+oitem.BOInCusHand + oitem.AcceptadBill + oitem.NegoTransit + oitem.NegotiatedBill + oitem.Amount_Due + oitem.Amount_ODue + oitem.PaymentDone + oitem.BFDDRecd), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                       nCurrency=oitem.Currency;
                        oPdfPTable.CompleteRow();
                    }
                
            }
            #endregion
            #region Table Sum
                        oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle_Bold));
                        oPdfPCell.Colspan = 2; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.Amount_LC)), _oFontStyle_Bold));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.BOinHand)), _oFontStyle_Bold));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.BOInCusHand)), _oFontStyle_Bold));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.AcceptadBill)), _oFontStyle_Bold));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.NegoTransit)), _oFontStyle_Bold));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.NegotiatedBill)), _oFontStyle_Bold));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.Amount_Due)), _oFontStyle_Bold));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.Amount_ODue)), _oFontStyle_Bold));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.Amount_Due + p.Amount_ODue)), _oFontStyle_Bold));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.Discounted)), _oFontStyle_Bold));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.PaymentDone)), _oFontStyle_Bold));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.BFDDRecd)), _oFontStyle_Bold));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => (p.Amount_LC + p.BOInCusHand + p.AcceptadBill + p.NegoTransit + p.NegotiatedBill + p.Amount_Due + p.Amount_ODue + p.PaymentDone + p.BFDDRecd))), _oFontStyle_Bold));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                        oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();



        }
        private void Print_Body_BillMaturity()
        {

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            PdfPTable oPdfPTable = new PdfPTable(1);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 595f });


            int nDateCompare = Convert.ToInt32(temp.Split('~')[0]);
            DateTime dtStartDate = Convert.ToDateTime(temp.Split('~')[1]);
            DateTime dtEndDate = Convert.ToDateTime(temp.Split('~')[2]);
            int BUID = Convert.ToInt32(temp.Split('~')[3]);
            dtEndDate = dtEndDate.AddMonths(1);
            dtStartDate = dtEndDate.AddYears(-1);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.MinimumHeight = 10f; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Bill Maturity", _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oSalesStatement_EBills = SalesStatement.Gets_BillMaturity(BUID, dtStartDate, dtEndDate, 0);

            //_nCount = 0;

            //var oDates = _oSalesStatement_EBills.GroupBy(x => new { x.StartDate }, (key, grp) =>
            //new
            //{
            //    StartDate = key.StartDate,
            //}).ToList().OrderBy(x => x.StartDate);

            //var oBanks = _oImportOutstandings_MonthWise.GroupBy(x => new { x.BankBranchID, x.BankName, x.LCPaymentType }, (key, grp) =>
            //new
            //{
            //    BankName = key.BankName,
            //    LCPaymentType = key.LCPaymentType,
            //    Amount = grp.Sum(x => x.Amount)
            //}).ToList();
            int minYear = 16;
            int minMonth = 2;

            if (_oSalesStatement_EBills.Count > 0)
            {
                minYear = _oSalesStatement_EBills.Min(x => x.StartDate.Year);
                minMonth = _oSalesStatement_EBills[0].StartDate.Month;
            }
            int column = 14;
            oPdfPTable = new PdfPTable(column);

            float[] widths = new float[column];
            widths[0] = 70f;

            for (int i = 1; i < column; i++)
            {
                widths[i] = 40f;
            }
            //widths[column+1] = 42f;

            oPdfPTable.SetWidths(widths);
            oPdfPCell = new PdfPCell(new Phrase("Bank", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            DateTime oDateTime = new DateTime();
            oDateTime = new DateTime(minYear, minMonth, 1);
            int m = 0;
            while (m < 12)
            {
                oPdfPCell = new PdfPCell(new Phrase(oDateTime.ToString("MMM yy"), _oFontStyle_Bold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oDateTime = oDateTime.AddMonths(1);
                m++;
            }
            oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            var summary = _oSalesStatement_EBills.GroupBy(x => x.Bank_Nego).Select(grp => new
            {
                BankName = grp.Key,
                Bill = grp.ToList().First()
                
            });

            foreach(var oitem in summary)
            {
                string bName = oitem.BankName;
                oPdfPCell = new PdfPCell(new Phrase(oitem.BankName, _oFontStyle));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);


                //var oList = _oImportOutstandings_MonthWise.Where(x => (x.BankName == oBank.BankName) && (x.LCPaymentType == oBank.LCPaymentType)).ToList();
                oDateTime = new DateTime(minYear, minMonth, 1);
                m = 0;
                double sum = 0.0;
                while (m < 12)
                {
                    var obj = _oSalesStatement_EBills.Where(x => (x.StartDate.Year == oDateTime.Year) && (x.StartDate.Month == oDateTime.Month) && (x.Bank_Nego == bName)).ToList();
                    var amount = (obj.Any()) ? obj.First().Amount : 0;
                    sum += amount;
                    if (amount > 0)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(amount).ToString(), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                    else
                    {
                        oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                    oDateTime = oDateTime.AddMonths(1);
                    m++;

                }

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(sum), _oFontStyle_Bold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            //foreach (var oBank in oBanks)
            //{
            //oPdfPCell = new PdfPCell(new Phrase("Payment Rec(USD)", _oFontStyle));
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //oPdfPTable.AddCell(oPdfPCell);


            ////var oList = _oImportOutstandings_MonthWise.Where(x => (x.BankName == oBank.BankName) && (x.LCPaymentType == oBank.LCPaymentType)).ToList();
            //oDateTime = new DateTime(minYear, minMonth, 1);
            //m = 0;
            //while (m < 12)
            //{
            //    var obj = _oSalesStatement_EBills.Where(x => (x.StartDate.Year == oDateTime.Year) && (x.StartDate.Month == oDateTime.Month)).ToList();
            //    var amount = (obj.Any()) ? obj.First().Amount : 0;
            //    if (amount > 0)
            //    {
            //        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(amount).ToString(), _oFontStyle));
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //        oPdfPTable.AddCell(oPdfPCell);
            //    }
            //    else
            //    {
            //        oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle));
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //        oPdfPTable.AddCell(oPdfPCell);
            //    }
            //    oDateTime = oDateTime.AddMonths(1);
            //    m++;

            //}

            //oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.Amount)), _oFontStyle_Bold));
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();


            /////// Convert To BDT
            //oDateTime = new DateTime(minYear, minMonth, 1);
            //oPdfPCell = new PdfPCell(new Phrase("Payment Rec(BDT)", _oFontStyle));
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //oPdfPTable.AddCell(oPdfPCell);

            //oDateTime = new DateTime(minYear, minMonth, 1);
            //m = 0;
            //while (m < 12)
            //{
            //    var obj = _oSalesStatement_EBills.Where(x => (x.StartDate.Year == oDateTime.Year) && (x.StartDate.Month == oDateTime.Month)).ToList();
            //    var amount = (obj.Any()) ? obj.First().Amount : 0;
            //    if (amount > 0)
            //    {
            //        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(amount * 78).ToString(), _oFontStyle));
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //        oPdfPTable.AddCell(oPdfPCell);
            //    }
            //    else
            //    {
            //        oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle));
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //        oPdfPTable.AddCell(oPdfPCell);
            //    }
            //    oDateTime = oDateTime.AddMonths(1);
            //    m++;

            //}
            //oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.Amount * 78)), _oFontStyle_Bold));
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();


            /////// Cash Receive
            //oPdfPCell = new PdfPCell(new Phrase("Payment Rec(Cash)", _oFontStyle));
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //oPdfPTable.AddCell(oPdfPCell);

            //oDateTime = new DateTime(minYear, minMonth, 1);
            //m = 0;
            //while (m < 12)
            //{
            //    var obj = _oSalesStatement_EBills.Where(x => (x.StartDate.Year == oDateTime.Year) && (x.StartDate.Month == oDateTime.Month)).ToList();
            //    var amount = (obj.Any()) ? obj.First().Amount_Cash : 0;
            //    if (amount > 0)
            //    {
            //        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(amount).ToString(), _oFontStyle));
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //        oPdfPTable.AddCell(oPdfPCell);
            //    }
            //    else
            //    {
            //        oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle));
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //        oPdfPTable.AddCell(oPdfPCell);
            //    }
            //    oDateTime = oDateTime.AddMonths(1);
            //    m++;

            //}
            //oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.Amount_Cash)), _oFontStyle_Bold));
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();

            ////// Total Cash +Export
            //oPdfPCell = new PdfPCell(new Phrase("Total(BDT) ", _oFontStyle));
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //oPdfPTable.AddCell(oPdfPCell);

            //oDateTime = new DateTime(minYear, minMonth, 1);
            //m = 0;
            //while (m < 12)
            //{
            //    var obj = _oSalesStatement_EBills.Where(x => (x.StartDate.Year == oDateTime.Year) && (x.StartDate.Month == oDateTime.Month)).ToList();
            //    var amount = (obj.Any()) ? (obj.First().Amount * 78 + obj.First().Amount_Cash) : 0;
            //    if (amount > 0)
            //    {
            //        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(amount).ToString(), _oFontStyle));
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //        oPdfPTable.AddCell(oPdfPCell);
            //    }
            //    else
            //    {
            //        oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle));
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //        oPdfPTable.AddCell(oPdfPCell);
            //    }
            //    oDateTime = oDateTime.AddMonths(1);
            //    m++;

            //}
            //oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => (p.Amount * 78 + p.Amount_Cash))), _oFontStyle_Bold));
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();

            ////// Total Cash +Export
            //oPdfPCell = new PdfPCell(new Phrase("Per Day Avarage", _oFontStyle));
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //oPdfPTable.AddCell(oPdfPCell);

            //oDateTime = new DateTime(minYear, minMonth, 1);
            //m = 0;
            //while (m < 12)
            //{
            //    var obj = _oSalesStatement_EBills.Where(x => (x.StartDate.Year == oDateTime.Year) && (x.StartDate.Month == oDateTime.Month)).ToList();
            //    var amount = (obj.Any()) ? (obj.First().Amount * 78 + obj.First().Amount_Cash) : 0;
            //    if (amount > 0)
            //    {
            //        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(amount / 30).ToString(), _oFontStyle));
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //        oPdfPTable.AddCell(oPdfPCell);
            //    }
            //    else
            //    {
            //        oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle));
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //        oPdfPTable.AddCell(oPdfPCell);
            //    }
            //    oDateTime = oDateTime.AddMonths(1);
            //    m++;

            //}
            //oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => (p.Amount * 78 + p.Amount_Cash)) / 12), _oFontStyle_Bold));
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

        }
        private void Print_Body_ExportBillRealize()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 595f });


            int nDateCompare = Convert.ToInt32(temp.Split('~')[0]);
            DateTime dtStartDate = Convert.ToDateTime(temp.Split('~')[1]);
            DateTime dtEndDate = Convert.ToDateTime(temp.Split('~')[2]);
            int BUID = Convert.ToInt32(temp.Split('~')[3]);
            dtEndDate = dtEndDate.AddMonths(1);
            dtStartDate = dtEndDate.AddYears(-1);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.MinimumHeight = 10f; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Payment Receive ", _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oSalesStatement_EBills = SalesStatement.Gets_BillRealize(BUID, dtStartDate, dtEndDate, 0);

            //_nCount = 0;

            //var oDates = _oSalesStatement_EBills.GroupBy(x => new { x.StartDate }, (key, grp) =>
            //new
            //{
            //    StartDate = key.StartDate,
            //}).ToList().OrderBy(x => x.StartDate);

            //var oBanks = _oImportOutstandings_MonthWise.GroupBy(x => new { x.BankBranchID, x.BankName, x.LCPaymentType }, (key, grp) =>
            //new
            //{
            //    BankName = key.BankName,
            //    LCPaymentType = key.LCPaymentType,
            //    Amount = grp.Sum(x => x.Amount)
            //}).ToList();
            int minYear = 16;
            int minMonth = 2;

            if (_oSalesStatement_EBills.Count > 0)
            {
                minYear = _oSalesStatement_EBills.Min(x => x.StartDate.Year);
                minMonth = _oSalesStatement_EBills[0].StartDate.Month;
            }
            int column = 14;
            oPdfPTable = new PdfPTable(column);

            float[] widths = new float[column];
            widths[0] = 70f;

            for (int i = 1; i < column; i++)
            {
                widths[i] = 40f;
            }
            //widths[column+1] = 42f;

            oPdfPTable.SetWidths(widths);
            oPdfPCell = new PdfPCell(new Phrase("Month", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            DateTime oDateTime = new DateTime();
            oDateTime = new DateTime(minYear, minMonth, 1);
            int m = 0;
            while (m < 12)
            {
                oPdfPCell = new PdfPCell(new Phrase(oDateTime.ToString("MMM yy"), _oFontStyle_Bold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oDateTime = oDateTime.AddMonths(1);
                m++;
            }
            oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            //foreach (var oBank in oBanks)
            //{
            oPdfPCell = new PdfPCell(new Phrase("Payment Rec(USD)", _oFontStyle));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);


            //var oList = _oImportOutstandings_MonthWise.Where(x => (x.BankName == oBank.BankName) && (x.LCPaymentType == oBank.LCPaymentType)).ToList();
            oDateTime = new DateTime(minYear, minMonth, 1);
            m = 0;
            while (m < 12)
            {
                var obj = _oSalesStatement_EBills.Where(x => (x.StartDate.Year == oDateTime.Year) && (x.StartDate.Month == oDateTime.Month)).ToList();
                var amount = (obj.Any()) ? obj.First().Amount : 0;
                if (amount > 0)
                {
                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(amount).ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                }
                oDateTime = oDateTime.AddMonths(1);
                m++;

            }

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.Amount)), _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            ///// Convert To BDT
            oDateTime = new DateTime(minYear, minMonth, 1);
            oPdfPCell = new PdfPCell(new Phrase("Payment Rec(BDT)", _oFontStyle));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);

            oDateTime = new DateTime(minYear, minMonth, 1);
            m = 0;
            while (m < 12)
            {
                var obj = _oSalesStatement_EBills.Where(x => (x.StartDate.Year == oDateTime.Year) && (x.StartDate.Month == oDateTime.Month)).ToList();
                var amount = (obj.Any()) ? obj.First().Amount : 0;
                if (amount > 0)
                {
                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(amount * 78).ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                }
                oDateTime = oDateTime.AddMonths(1);
                m++;

            }
            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.Amount * 78)), _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            ///// Cash Receive
            oPdfPCell = new PdfPCell(new Phrase("Payment Rec(Cash)", _oFontStyle));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);

            oDateTime = new DateTime(minYear, minMonth, 1);
            m = 0;
            while (m < 12)
            {
                var obj = _oSalesStatement_EBills.Where(x => (x.StartDate.Year == oDateTime.Year) && (x.StartDate.Month == oDateTime.Month)).ToList();
                var amount = (obj.Any()) ? obj.First().Amount_Cash : 0;
                if (amount > 0)
                {
                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(amount).ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                }
                oDateTime = oDateTime.AddMonths(1);
                m++;

            }
            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => p.Amount_Cash)), _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //// Total Cash +Export
            oPdfPCell = new PdfPCell(new Phrase("Total(BDT) ", _oFontStyle));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);

            oDateTime = new DateTime(minYear, minMonth, 1);
            m = 0;
            while (m < 12)
            {
                var obj = _oSalesStatement_EBills.Where(x => (x.StartDate.Year == oDateTime.Year) && (x.StartDate.Month == oDateTime.Month)).ToList();
                var amount = (obj.Any()) ? (obj.First().Amount * 78 + obj.First().Amount_Cash) : 0;
                if (amount > 0)
                {
                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(amount).ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                }
                oDateTime = oDateTime.AddMonths(1);
                m++;

            }
            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => (p.Amount * 78 + p.Amount_Cash))), _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //// Total Cash +Export
            oPdfPCell = new PdfPCell(new Phrase("Per Day Avarage", _oFontStyle));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);

            oDateTime = new DateTime(minYear, minMonth, 1);
            m = 0;
            while (m < 12)
            {
                var obj = _oSalesStatement_EBills.Where(x => (x.StartDate.Year == oDateTime.Year) && (x.StartDate.Month == oDateTime.Month)).ToList();
                var amount = (obj.Any()) ? (obj.First().Amount * 78 + obj.First().Amount_Cash) : 0;
                if (amount > 0)
                {
                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(amount / 30).ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                }
                oDateTime = oDateTime.AddMonths(1);
                m++;

            }
            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oSalesStatement_EBills.Sum(p => (p.Amount * 78 + p.Amount_Cash)) / 12), _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

        }
        #endregion
    }
}
