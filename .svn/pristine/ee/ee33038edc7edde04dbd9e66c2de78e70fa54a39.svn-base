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

namespace ESimSol.Reports
{


    public class rptComprehensiveIncomeStatement
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        int _nColumns = 6;
        PdfPTable _oPdfPTable = new PdfPTable(6);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        IncomeStatement _oIncomeStatement = new IncomeStatement();

        List<CIStatementSetup> _oCIStatementSetups = new List<CIStatementSetup>();
        Company _oCompany = new Company();
        #endregion

        public byte[] PrepareReport(IncomeStatement oIncomeStatement, Company oCompany)
        {
            _oIncomeStatement = oIncomeStatement;
            _oCIStatementSetups = oIncomeStatement.CIStatementSetups;
            _oCompany = oCompany;
            
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 40f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 39f, 14f, 15f,15f,5f, 15f });//535
            #endregion

            this.PrintHeader();
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
                //_oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }


       
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Statement of Comprehensive Income", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.FixedHeight = 30f;
            //_oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("For the year ended " + _oIncomeStatement.EndDateFullSt, _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 30f;
            //_oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Base Currency Messge
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);

    
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oIncomeStatement.SessionDate + " " + _oCompany.CurrencyName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;_oPdfPCell.BackgroundColor = BaseColor.WHITE;_oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oIncomeStatement.PriviosSessoinName!=""?_oIncomeStatement.PriviosSessoinName + " " + _oCompany.CurrencyName:"", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("Particulars", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Notes", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            int nCount = 0; bool IsFirst = true;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            foreach (CIStatementSetup oItem in _oCIStatementSetups)
            {
                string sACCName = oItem.AccountHeadName.ToUpper();
                bool bIsTotal = (sACCName == "NET TURNOVER" || sACCName == "GROSS PROFIT" || sACCName == "PROFIT FROM OPERATIONS" || sACCName == "PROFIT AFTER FINANCIAL COST" || sACCName == "PROFIT BEFORE WPPF" || sACCName == "PROFIT BEFORE TAX" || sACCName == "PROFIT AFTER TAX" || sACCName == "PROFIT FOR THE YEAR" || sACCName == "TOTAL COMPHREHENSIVE INCOME FOR THE YEAR");

                if (oItem.Label==3)
                {                    
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadName, _oFontStyle));
                    _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                    if (sACCName == "TURNOVER")
                    {
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    }
                } 
                else if (oItem.Label == 2)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("  "+oItem.AccountHeadName, _oFontStyle));
                    _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                }
                else
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oPdfPCell = new PdfPCell(new Phrase("  "+oItem.AccountHeadName, _oFontStyle));
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                if (oItem.Label==1)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); 
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadValueSt, _oFontStyle));
                    _oPdfPCell.BorderColor = BaseColor.BLACK;

                    if (IsFirst)
                    {
                        _oPdfPCell.BorderWidthLeft = 1f;
                        _oPdfPCell.BorderWidthRight = 1f;
                        _oPdfPCell.BorderWidthTop = 1f;
                        _oPdfPCell.BorderWidthBottom = 0f;
                        //IsFirst = false;
                    }
                    else
                    {
                        _oPdfPCell.BorderWidthLeft = 1f;
                        _oPdfPCell.BorderWidthRight = 1f;
                        _oPdfPCell.BorderWidthTop = 0f;
                        _oPdfPCell.BorderWidthBottom = 0f;
                    }
                    if (_oCIStatementSetups[nCount+1].Label == 3 || _oCIStatementSetups[nCount+1].Label == 2)
                    {
                        _oPdfPCell.BorderWidthLeft = 1f;
                        _oPdfPCell.BorderWidthRight = 1f;

                        if (_oPdfPCell.BorderWidthTop!=1f)
                        _oPdfPCell.BorderWidthTop = 0f;
                        _oPdfPCell.BorderWidthBottom = 1f;
                        //IsFirst = true;
                    }
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);


                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadValue_ForSessionSt, _oFontStyle));
                    _oPdfPCell.BorderColor = BaseColor.BLACK;

                    if (IsFirst)
                    {
                        _oPdfPCell.BorderWidthLeft = 1f;
                        _oPdfPCell.BorderWidthRight = 1f;
                        _oPdfPCell.BorderWidthTop = 1f;
                        _oPdfPCell.BorderWidthBottom = 0f;
                        IsFirst = false;
                    }
                    else
                    {
                        _oPdfPCell.BorderWidthLeft = 1f;
                        _oPdfPCell.BorderWidthRight = 1f;
                        _oPdfPCell.BorderWidthTop = 0f;
                        _oPdfPCell.BorderWidthBottom = 0f;
                    }
                    if (_oCIStatementSetups[nCount + 1].Label == 3 || _oCIStatementSetups[nCount + 1].Label == 2)
                    {
                        _oPdfPCell.BorderWidthLeft = 1f;
                        _oPdfPCell.BorderWidthRight = 1f;

                        if (_oPdfPCell.BorderWidthTop != 1f)
                            _oPdfPCell.BorderWidthTop = 0f;
                        _oPdfPCell.BorderWidthBottom = 1f;
                        IsFirst = true;
                    }
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    
                }
                else if (oItem.Label==2)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); 
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadValueSt, _oFontStyle));
                    _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadValue_ForSessionSt, _oFontStyle));
                    _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //_oPdfPCell.Border = 0;_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE;  _oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadValueSt, _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    if (sACCName == "TURNOVER")
                    {
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    }
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadValue_ForSessionSt, _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    if (sACCName == "TURNOVER")
                    {
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    }
                    _oPdfPTable.AddCell(_oPdfPCell);
                }
                _oPdfPTable.CompleteRow();
                nCount++;
            }
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;_oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region Signature
            //#region Date Print
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase("Dated, Dhaka: " + DateTime.Today.ToString("dd MMMM yyyy"), _oFontStyle));
            //_oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion

            //#region Signature Print
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase("Managing Director", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Director", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Prepare By", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion
            #endregion

        }
        #endregion




    }


}
