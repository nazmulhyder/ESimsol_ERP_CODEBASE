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
   public class rptNotesOfTheComprehensiveIncome
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
       Chunk _sTemp_chunk_ac_Name = new Chunk();
       Chunk _sTemp_chunk_Balance = new Chunk();
       List<IncomeStatement> _Revenues = new List<IncomeStatement>();
       List<IncomeStatement> _Expenses = new List<IncomeStatement>();
       #endregion

       #region financial Position
       public byte[] PrepareReport(IncomeStatement oIncomeStatement, BusinessUnit oBusinessUnit)
       {
           _oIncomeStatement = oIncomeStatement;
           _oCompany = oIncomeStatement.Company;
           _oBusinessUnit = oBusinessUnit;

           #region Page Setup
           _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
           _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
           _oDocument.SetMargins(60f, 60f, 50f, 20f);
           _oPdfPTable.WidthPercentage = 100;
           _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
           PdfWriter.GetInstance(_oDocument, _oMemoryStream);
           _oDocument.Open();
           _oPdfPTable.SetWidths(new float[] { 230f, 105f, 115f });
           #endregion

           this.PrintHeader();
           this.PrintBody();
           _oPdfPTable.HeaderRows = 4;
           _oDocument.Close();
           return _oMemoryStream.ToArray();
       }

       #region Report Header
       private void PrintHeader()
       {
           #region CompanyHeader
           _oFontStyle = FontFactory.GetFont("italic", 12f, 1);
           if (_oBusinessUnit.BusinessUnitID > 0)
           {
               _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
           }
           else
           {
               _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
           }
           _oPdfPCell.Colspan = 3;
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
           _oPdfPCell.Border = 0;
           _oPdfPCell.BackgroundColor = BaseColor.WHITE;
           _oPdfPCell.ExtraParagraphSpace = 0;
           _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion

           #region ReportHeader
           _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
           _oPdfPCell = new PdfPCell(new Phrase("Notes to the Comprehensive Income", _oFontStyle));
           _oPdfPCell.Colspan = 3;
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
           _oPdfPCell.Border = 0;
           _oPdfPCell.BackgroundColor = BaseColor.WHITE;
           _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion

           #region Print Date & Currency
           _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
           _oPdfPCell = new PdfPCell(new Phrase("For the Year Ended " + _oIncomeStatement.EndDate.ToString("dd MMMM yyyy"), _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
           _oPdfPCell.Border = 0;
           _oPdfPCell.Colspan = 2;
           _oPdfPCell.BackgroundColor = BaseColor.WHITE;
           _oPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(_oIncomeStatement.EndDate.ToString("dd-MM-yyyy"), _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
           _oPdfPCell.BackgroundColor = BaseColor.WHITE;
           _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion
           #region Blank Space
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.FixedHeight = 10; _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.FixedHeight = 10; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion
       }
       #endregion

       #region Report Body
       private void PrintBody()
       {
           #region Revenues

           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
           int ncount = 0; string sPriviousSGInString = "";
           foreach (IncomeStatement oItem in _oIncomeStatement.Revenues)
           {
               if (oItem.AccountType == EnumAccountType.SubGroup || oItem.AccountType == EnumAccountType.Ledger)
               {
                   if (oItem.AccountType == EnumAccountType.SubGroup)
                   {

                       if (ncount != 0)
                       {
                           #region Line
                           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                           _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                           _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                           _oPdfPTable.CompleteRow();
                           #endregion
                           #region Total print of Subgroup
                           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                           _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                           _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                           _oPdfPCell = new PdfPCell(new Phrase(_oCompany.CurrencyName + " " + sPriviousSGInString, _oFontStyle));
                           _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                           _oPdfPTable.CompleteRow();
                           #endregion

                           #region Blank
                           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                           _oPdfPCell.FixedHeight = 2f; _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                           _oPdfPCell.FixedHeight = 2f; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                           _oPdfPTable.CompleteRow();
                           #endregion
                           #region Blank Space
                           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                           _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                           _oPdfPTable.CompleteRow();
                           #endregion
                           #region Date print
                           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                           _oPdfPCell.Border = 0;
                           _oPdfPCell.Colspan = 2;
                           _oPdfPTable.AddCell(_oPdfPCell);

                           _oPdfPCell = new PdfPCell(new Phrase(_oIncomeStatement.EndDate.ToString("dd-MM-yyyy"), _oFontStyle));
                           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                           _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                           _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                           _oPdfPCell.FixedHeight = 20f;
                           _oPdfPTable.AddCell(_oPdfPCell);
                           _oPdfPTable.CompleteRow();
                           #endregion
                       }
                       #region Blank Space
                       _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                       _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                       _oPdfPTable.CompleteRow();
                       #endregion
                       #region subgroup Print
                       _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
                       Phrase dynamicParse = new Phrase();
                       _sTemp_chunk_ac_Name = new Chunk(oItem.AccountCode + " " + oItem.AccountHeadName + "  ", _oFontStyle);
                       dynamicParse.Add(_sTemp_chunk_ac_Name);
                       _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                       _sTemp_chunk_Balance = new Chunk(_oCompany.CurrencyName + " " + oItem.CGSGBalanceInString, _oFontStyle);
                       dynamicParse.Add(_sTemp_chunk_Balance);
                       _oPdfPCell = new PdfPCell(dynamicParse);
                       _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                       _oPdfPTable.CompleteRow();
                       #endregion
                       #region Hard Code Text
                       _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                       _oPdfPCell = new PdfPCell(new Phrase("Details of " + oItem.AccountHeadName + " as at " + _oIncomeStatement.EndDate.ToString("dd MMMM yyyy") + " Are Shown in given :", _oFontStyle));
                       _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                       _oPdfPTable.CompleteRow();
                       #endregion
                       ncount++;
                       sPriviousSGInString = oItem.CGSGBalanceInString;
                   }
                   else
                   {
                       #region LEDger Print
                       _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                       _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadName, _oFontStyle));
                       _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                       _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                       _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                       _oPdfPCell = new PdfPCell(new Phrase(oItem.LedgerBalanceInString, _oFontStyle));
                       _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                       #endregion
                       _oPdfPTable.CompleteRow();
                   }

               }

           }

           #region Last total print
           #region Line
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion
           #region Total print of Subgroup
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

           _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
           _oPdfPCell = new PdfPCell(new Phrase(_oCompany.CurrencyName + " " + sPriviousSGInString, _oFontStyle));
           _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion

           #region Blank
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.FixedHeight = 2f; _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.FixedHeight = 2f; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion
           #region Blank Space
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion


           #endregion

           #endregion

           #region Blank Space
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.Colspan = 3;
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
           _oPdfPCell.Border = 0;
           _oPdfPCell.BackgroundColor = BaseColor.WHITE;
           _oPdfPCell.FixedHeight = 15;
           _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion

           #region Liability & Owner Equity



           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
           ncount = 0; sPriviousSGInString = "";
           foreach (IncomeStatement oItem in _oIncomeStatement.Expenses)
           {
               if (oItem.AccountType == EnumAccountType.SubGroup || oItem.AccountType == EnumAccountType.Ledger)
               {
                   if (oItem.AccountType == EnumAccountType.SubGroup)
                   {

                       if (ncount != 0)
                       {
                           #region Line
                           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                           _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                           _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                           _oPdfPTable.CompleteRow();
                           #endregion
                           #region Total print of Subgroup
                           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                           _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                           _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                           _oPdfPCell = new PdfPCell(new Phrase(_oCompany.CurrencyName + " " + sPriviousSGInString, _oFontStyle));
                           _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                           _oPdfPTable.CompleteRow();
                           #endregion

                           #region Blank
                           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                           _oPdfPCell.FixedHeight = 2f; _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                           _oPdfPCell.FixedHeight = 2f; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                           _oPdfPTable.CompleteRow();
                           #endregion
                           #region Blank Space
                           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                           _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                           _oPdfPTable.CompleteRow();
                           #endregion
                           #region Date print
                           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                           _oPdfPCell.Border = 0;
                           _oPdfPCell.Colspan = 2;
                           _oPdfPTable.AddCell(_oPdfPCell);

                           _oPdfPCell = new PdfPCell(new Phrase(_oIncomeStatement.EndDate.ToString("dd-MM-yyyy"), _oFontStyle));
                           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                           _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                           _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                           _oPdfPCell.FixedHeight = 20f;
                           _oPdfPTable.AddCell(_oPdfPCell);
                           _oPdfPTable.CompleteRow();
                           #endregion
                       }
                       #region Blank Space
                       _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                       _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                       _oPdfPTable.CompleteRow();
                       #endregion
                       #region subgroup Print
                       _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.BaseColor.LIGHT_GRAY);
                       Phrase dynamicParse = new Phrase();
                       _sTemp_chunk_ac_Name = new Chunk(oItem.AccountCode + " " + oItem.AccountHeadName + "  ", _oFontStyle);
                       dynamicParse.Add(_sTemp_chunk_ac_Name);
                       _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                       _sTemp_chunk_Balance = new Chunk(_oCompany.CurrencyName + " " + oItem.CGSGBalanceInString, _oFontStyle);
                       dynamicParse.Add(_sTemp_chunk_Balance);
                       _oPdfPCell = new PdfPCell(dynamicParse);
                       _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                       _oPdfPTable.CompleteRow();
                       #endregion
                       #region Hard Code Text
                       _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                       _oPdfPCell = new PdfPCell(new Phrase("Details of " + oItem.AccountHeadName + " as at " + _oIncomeStatement.EndDate.ToString("dd MMMM yyyy") + " Are Shown in given :", _oFontStyle));
                       _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                       _oPdfPTable.CompleteRow();
                       #endregion
                       ncount++;
                       sPriviousSGInString = oItem.CGSGBalanceInString;
                   }
                   else
                   {
                       #region Ledger Print
                       _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                       _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadName, _oFontStyle));
                       _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                       _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                       _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                       _oPdfPCell = new PdfPCell(new Phrase(oItem.LedgerBalanceInString, _oFontStyle));
                       _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                       #endregion
                       _oPdfPTable.CompleteRow();
                   }

               }

           }
           #region Last total print
           #region Line
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.FixedHeight = 1f; _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion
           #region Total print of Subgroup
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

           _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
           _oPdfPCell = new PdfPCell(new Phrase(_oCompany.CurrencyName + " " + sPriviousSGInString, _oFontStyle));
           _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion

           #region Blank
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.FixedHeight = 2f; _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.FixedHeight = 2f; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion
           #region Blank Space
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion

           #endregion
           #endregion


           _oDocument.Add(_oPdfPTable);
       }
       #endregion
       #endregion
   }
}
