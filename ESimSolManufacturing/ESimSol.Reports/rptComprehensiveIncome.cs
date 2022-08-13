using System;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ESimSol.Reports
{
    public class rptComprehensiveIncome
    {

        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(3);
        PdfPCell _oPdfPCell;

        MemoryStream _oMemoryStream = new MemoryStream();
        IncomeStatement _oIncomeStatement = new IncomeStatement();
        List<IncomeStatement> _oIncomeStatements = new List<IncomeStatement>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();

        List<IncomeStatement> _oRevenues = new List<IncomeStatement>();
        List<IncomeStatement> _oExpenses = new List<IncomeStatement>();
        List<CIStatementSetup> _oCIStatementSetups = new List<CIStatementSetup>();
        List<CIStatementSetup> _oTempCIStatementSetups = new List<CIStatementSetup>();
        CIStatementSetup _oCIStatementSetup = new CIStatementSetup();
        #endregion

        #region Balance Sheet Short
        public byte[] PrepareReport(IncomeStatement oIncomeStatement, BusinessUnit oBusinessUnit)
        {
            _oIncomeStatement = oIncomeStatement;
            _oCompany = oIncomeStatement.Company;
            _oBusinessUnit = oBusinessUnit;
            _oRevenues = oIncomeStatement.Revenues;
            _oExpenses = oIncomeStatement.Expenses;
            _oCIStatementSetups = oIncomeStatement.CIStatementSetups;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(60f, 60f, 25f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 230f, 115f, 115f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            if (_oBusinessUnit.BusinessUnitID > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            }
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.BaseColor.LIGHT_GRAY);
            _oPdfPCell = new PdfPCell(new Phrase("Statement of Comprehensive Income", _oFontStyle));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 20;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region DAte
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("For the Year Ended " + _oIncomeStatement.EndDate.ToString("dd MMMM yyyy"), _oFontStyle));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 20;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Print Date & Currency
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oIncomeStatement.SessionDate+ "  " + _oCompany.CurrencyName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            #region tittle
             _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
              _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Notes", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            #endregion
            double nClobalTotal = 0, nTempTotal = 0;
            #region TrunOver

            int nCount = _oCIStatementSetups.Where(x => (int)x.CIHeadType==1).Count();//turnover
            if(nCount>1)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
                _oPdfPCell = new PdfPCell(new Phrase("GROSS TURNOVER :", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                double nTurnOverAmount = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 1).Sum(x => x.AccountHeadValue);
                nClobalTotal = nTurnOverAmount;//set amount
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTurnOverAmount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                _oTempCIStatementSetups = new List<CIStatementSetup>();
                _oTempCIStatementSetups = _oCIStatementSetups.Where(x => (int)x.CIHeadType==1).ToList();
                foreach(CIStatementSetup oItem in _oTempCIStatementSetups)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.AccountHeadValue), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
            }
            else
            {
                _oCIStatementSetup = new CIStatementSetup();
                _oCIStatementSetup = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 1).ToList()[0];//turn over head
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.AccountHeadName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.Note, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oCIStatementSetup.AccountHeadValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                nClobalTotal = _oCIStatementSetup.AccountHeadValue;
            }

            
            #endregion

            #region liability value add tex
            _oTempCIStatementSetups = new List<CIStatementSetup>();
            _oTempCIStatementSetups = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 2).ToList();//liability
            foreach (CIStatementSetup oItem in _oTempCIStatementSetups)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oCIStatementSetup.AccountHeadValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                nClobalTotal = nClobalTotal - oItem.AccountHeadValue;
            }
            #endregion

            #region Net TurnOver
            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
            _oPdfPCell = new PdfPCell(new Phrase("NET TURNOVER", _oFontStyle));
            _oPdfPCell.FixedHeight = 6f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nClobalTotal), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Cost of Good sold

            nCount = _oCIStatementSetups.Where(x => (int)x.CIHeadType==3).Count();//Cost of good sold
            if(nCount>1)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
                _oPdfPCell = new PdfPCell(new Phrase("COST OF GOODS SOLD :", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                double nCostOFGoodSoldAmount = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 3).Sum(x => x.AccountHeadValue);
                nClobalTotal = nClobalTotal - nCostOFGoodSoldAmount;//set amount
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("("+Global.MillionFormat(nCostOFGoodSoldAmount)+")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                _oTempCIStatementSetups = new List<CIStatementSetup>();
                _oTempCIStatementSetups = _oCIStatementSetups.Where(x => (int)x.CIHeadType==3).ToList();//cost of good sold
                foreach(CIStatementSetup oItem in _oTempCIStatementSetups)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("("+Global.MillionFormat(oItem.AccountHeadValue)+")", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
            }
            else
            {
                _oCIStatementSetup = new CIStatementSetup();
                _oCIStatementSetup = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 3).ToList()[0];//cost of good sold
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.AccountHeadName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.Note, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("(" + Global.MillionFormat(_oCIStatementSetup.AccountHeadValue) + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                nClobalTotal = nClobalTotal- _oCIStatementSetup.AccountHeadValue;
            }

            
            #endregion

            #region Gross Profit
            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
            _oPdfPCell = new PdfPCell(new Phrase("GROSS PROFIT", _oFontStyle));
            _oPdfPCell.FixedHeight = 6f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nClobalTotal), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Operating Expense

            nCount = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 4).Count();//Operating exepense
            if (nCount > 1)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
                _oPdfPCell = new PdfPCell(new Phrase("OPERATING EXPENSES:", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                double nOperatingExpenseAmount = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 4).Sum(x => x.AccountHeadValue);//Operating exepense
                nClobalTotal = nClobalTotal - nOperatingExpenseAmount;//set amount
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("(" + Global.MillionFormat(nOperatingExpenseAmount) + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                _oTempCIStatementSetups = new List<CIStatementSetup>();
                _oTempCIStatementSetups = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 4).ToList();//Operating exepense
                foreach (CIStatementSetup oItem in _oTempCIStatementSetups)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("(" + Global.MillionFormat(oItem.AccountHeadValue) + ")", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
            }
            else
            {
                _oCIStatementSetup = new CIStatementSetup();
                _oCIStatementSetup = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 4).ToList()[0];//Operating exepense
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.AccountHeadName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.Note, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("(" + Global.MillionFormat(_oCIStatementSetup.AccountHeadValue) + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                nClobalTotal = nClobalTotal - _oCIStatementSetup.AccountHeadValue;
            }


            #endregion

            #region  Profit FROM Operation
            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
            _oPdfPCell = new PdfPCell(new Phrase("PROFIT FROM OPERATIONS", _oFontStyle));
            _oPdfPCell.FixedHeight = 6f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nClobalTotal), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Other Income

            nCount = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 5).Count();//Other Income
            if (nCount > 1)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
                _oPdfPCell = new PdfPCell(new Phrase("OTHER INCOME:", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                double nOtherIncomeAmount = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 5).Sum(x => x.AccountHeadValue);//Other Income
                nClobalTotal = nClobalTotal + nOtherIncomeAmount;//set amount
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat(nOtherIncomeAmount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                _oTempCIStatementSetups = new List<CIStatementSetup>();
                _oTempCIStatementSetups = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 5).ToList();//Other Income
                foreach (CIStatementSetup oItem in _oTempCIStatementSetups)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.AccountHeadValue), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
            }
            else
            {
                _oCIStatementSetup = new CIStatementSetup();
                _oCIStatementSetup = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 5).ToList()[0];//Other Income
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.AccountHeadName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.Note, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oCIStatementSetup.AccountHeadValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                nClobalTotal = nClobalTotal + _oCIStatementSetup.AccountHeadValue;
            }


            #endregion

            #region  Profit Before WPPF
            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
            _oPdfPCell = new PdfPCell(new Phrase("PROFIT BEFORE WPPF", _oFontStyle));
            _oPdfPCell.FixedHeight = 6f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nClobalTotal), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #region WPPF Allocation

            nCount = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 6).Count();//WPPF Allocation
            if (nCount > 1)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
                _oPdfPCell = new PdfPCell(new Phrase("ALLOCATION FOR WPPF:", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                double nWPPFAllocationAmount = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 6).Sum(x => x.AccountHeadValue);//WPPF Allocation
                nClobalTotal = nClobalTotal - nWPPFAllocationAmount;//set amount
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("(" + Global.MillionFormat(nWPPFAllocationAmount) + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                _oTempCIStatementSetups = new List<CIStatementSetup>();
                _oTempCIStatementSetups = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 6).ToList();//WPPF Allocation
                foreach (CIStatementSetup oItem in _oTempCIStatementSetups)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("(" + Global.MillionFormat(oItem.AccountHeadValue) + ")", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
            }
            else
            {
                _oCIStatementSetup = new CIStatementSetup();
                _oCIStatementSetup = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 6).ToList()[0];//WPPF Allocation
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.AccountHeadName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.Note, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("(" + Global.MillionFormat(_oCIStatementSetup.AccountHeadValue) + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                nClobalTotal = nClobalTotal - _oCIStatementSetup.AccountHeadValue;
            }


            #endregion

            #region  PROFIT BEPORE TAX
            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
            _oPdfPCell = new PdfPCell(new Phrase("PROFIT BEPORE TAX", _oFontStyle));
            _oPdfPCell.FixedHeight = 6f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nClobalTotal), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region INCOME TAX

            nCount = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 7).Count();//INCOME TAX
            if (nCount > 1)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
                _oPdfPCell = new PdfPCell(new Phrase("PROVISSION FOR INCOME TAX:", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                double nINCOMETAXAmount = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 7).Sum(x => x.AccountHeadValue);//INCOME TAX
                nClobalTotal = nClobalTotal - nINCOMETAXAmount;//set amount
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("(" + Global.MillionFormat(nINCOMETAXAmount) + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                _oTempCIStatementSetups = new List<CIStatementSetup>();
                _oTempCIStatementSetups = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 7).ToList();//INCOME TAX
                foreach (CIStatementSetup oItem in _oTempCIStatementSetups)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("(" + Global.MillionFormat(oItem.AccountHeadValue) + ")", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
            }
            else
            {
                _oCIStatementSetup = new CIStatementSetup();
                _oCIStatementSetup = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 7).ToList()[0];//INCOME TAX
                _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.AccountHeadName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.Note, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("(" + Global.MillionFormat(_oCIStatementSetup.AccountHeadValue) + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                nClobalTotal = nClobalTotal - _oCIStatementSetup.AccountHeadValue;
            }


            #endregion

            #region  PROFIT AFTER TAX
            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
            _oPdfPCell = new PdfPCell(new Phrase("PROFIT AFTER TAX", _oFontStyle));
            _oPdfPCell.FixedHeight = 6f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nClobalTotal), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Profit from ASSociate
            nCount = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 8).Count();//Profit from ASSociate
            if (nCount > 0)
            {
                if (nCount > 1)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
                    _oPdfPCell = new PdfPCell(new Phrase("PROFIT /(LOSS) FROM ASSOCIATED UNDERTAKINGS:", _oFontStyle));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    double nProfitFromAsscociatedAmount = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 8).Sum(x => x.AccountHeadValue);//Profit from ASSociate
                    nClobalTotal = nClobalTotal + nProfitFromAsscociatedAmount;//set amount
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nProfitFromAsscociatedAmount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    _oTempCIStatementSetups = new List<CIStatementSetup>();
                    _oTempCIStatementSetups = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 8).ToList();//Profit from ASSociate
                    foreach (CIStatementSetup oItem in _oTempCIStatementSetups)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.AccountHeadValue), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                    }
                }
                else
                {
                    _oCIStatementSetup = new CIStatementSetup();
                    _oCIStatementSetup = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 8).ToList()[0];//Profit from ASSociate
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.AccountHeadName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.Note, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oCIStatementSetup.AccountHeadValue), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    nClobalTotal = nClobalTotal + _oCIStatementSetup.AccountHeadValue;
                }
            }
            #endregion

            #region  Profit FOR THE YEAR
            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
            _oPdfPCell = new PdfPCell(new Phrase("PROFIT FOR THE YEAR", _oFontStyle));
            _oPdfPCell.FixedHeight = 6f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nClobalTotal), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region COMPHREHENSIVE INCOME

            nCount = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 9).Count();//COMPHREHENSIVE INCOME
            if (nCount > 0)
            {
                if (nCount > 1)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
                    _oPdfPCell = new PdfPCell(new Phrase("OTHER COMPHREHENSIVE INCOME:", _oFontStyle));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    double nProfitFromAsscociatedAmount = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 9).Sum(x => x.AccountHeadValue);//COMPHREHENSIVE INCOME
                    nClobalTotal = nClobalTotal + nProfitFromAsscociatedAmount;//set amount
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nProfitFromAsscociatedAmount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    _oTempCIStatementSetups = new List<CIStatementSetup>();
                    _oTempCIStatementSetups = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 9).ToList();//COMPHREHENSIVE INCOME
                    foreach (CIStatementSetup oItem in _oTempCIStatementSetups)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.AccountHeadValue), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                    }
                }
                else
                {
                    _oCIStatementSetup = new CIStatementSetup();
                    _oCIStatementSetup = _oCIStatementSetups.Where(x => (int)x.CIHeadType == 9).ToList()[0];//COMPHREHENSIVE INCOME
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.AccountHeadName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(_oCIStatementSetup.Note, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oCIStatementSetup.AccountHeadValue), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    nClobalTotal = nClobalTotal + _oCIStatementSetup.AccountHeadValue;
                }
            }


            #endregion

            #region  Profit FOR THE YEAR
            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
            _oPdfPCell = new PdfPCell(new Phrase("TOTAL COMPHREHENSIVE INCOME FOR THE YEAR", _oFontStyle));
            _oPdfPCell.FixedHeight = 6f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nClobalTotal), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Extra Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthTop =1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #endregion

            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region small note
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Attached notes form part of these Financial Statements", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("As per our annexed report of even date ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 45; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Signature Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Chairman                                                  Managing Director", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Company Secretary ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Chartered Accountants", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Date Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Dated, Dhaka: " + DateTime.Today.ToString("dd MMMM yyyy"), _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

           
           
            _oDocument.Add(_oPdfPTable);
        }
        #endregion
        #endregion
    }
}
