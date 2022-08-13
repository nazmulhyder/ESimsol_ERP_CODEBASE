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
   public class rptCashFlowDmStatement
   {
       #region Declaration
       Document _oDocument;
       iTextSharp.text.Font _oFontStyle;
       int _nColumns = 6;
       PdfPTable _oPdfPTable = new PdfPTable(6);
       PdfPCell _oPdfPCell;
       iTextSharp.text.Image _oImag;
       MemoryStream _oMemoryStream = new MemoryStream();
       CashFlowDmSetup _oCashFlowDmSetup = new CashFlowDmSetup();

       List<CashFlowDmSetup> _oCashFlowDmSetups = new List<CashFlowDmSetup>();
       Company _oCompany = new Company();
       #endregion
       public byte[] PrepareReport(CashFlowDmSetup oCashFlowDmSetup, Company oCompany)
       {
           _oCashFlowDmSetup = oCashFlowDmSetup;
           _oCashFlowDmSetups = oCashFlowDmSetup.CashFlowDmSetups;
           _oCompany = oCompany;

           #region Page Setup
           _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
           _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
           _oDocument.SetMargins(30f, 30f, 30f, 30f);
           _oPdfPTable.WidthPercentage = 100;
           _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
           PdfWriter.GetInstance(_oDocument, _oMemoryStream);
           _oDocument.Open();

           _oPdfPTable.SetWidths(new float[] {6f,9f,141f, 60f, 78f,6f });//535
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
           _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.BaseColor.LIGHT_GRAY);
           _oPdfPCell = new PdfPCell(new Phrase("Statement of Cash Flows", _oFontStyle));
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
           _oPdfPCell = new PdfPCell(new Phrase("For the Year Ended " + _oCashFlowDmSetup.EndDateSt, _oFontStyle));
           _oPdfPCell.BackgroundColor = BaseColor.WHITE;
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
           _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
           _oPdfPCell.Colspan = _nColumns;
           _oPdfPCell.Border = 0;
           _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion

           #region Base Currency Messge
        
           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           //_oPdfPCell.Colspan = _nColumns;
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
           _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
           _oPdfPCell.Border = 0;
           _oPdfPCell.Colspan = 4; 
           _oPdfPCell.BackgroundColor = BaseColor.WHITE;
           _oPdfPCell.ExtraParagraphSpace = 5f;
           _oPdfPTable.AddCell(_oPdfPCell);

           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
           _oPdfPCell = new PdfPCell(new Phrase(_oCashFlowDmSetup.SessionDate + " " + _oCompany.CurrencyName, _oFontStyle));
           //_oPdfPCell.Colspan = _nColumns;
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
           _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER;
           //_oPdfPCell.Border = 0;
           _oPdfPCell.BackgroundColor = BaseColor.WHITE;
           _oPdfPCell.ExtraParagraphSpace = 5f;
           _oPdfPTable.AddCell(_oPdfPCell);

           
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           //_oPdfPCell.Colspan = _nColumns;
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
           _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
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
           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();

           //int nCount = 0;
           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
           CashFlowDmSetup oTempCashFlowDmSetup = new CashFlowDmSetup();
           List<CashFlowDmSetup> oTempCashFlowDmSetups = new  List<CashFlowDmSetup>();

            #region Blank Space
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
            #endregion

            #region Operating Activities
            #region Operating_Opening_Caption 
             oTempCashFlowDmSetup = GetSpecificList(2)[0]; //Operating_Opening_Caption = 2
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);                   
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oTempCashFlowDmSetup.DisplayCaption, _oFontStyle));
            _oPdfPCell.Colspan = 3;_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
           #endregion

            #region Operating_Activities
           oTempCashFlowDmSetups =  GetSpecificList(3);//Operating_Activities = 3,
           int nCount = 0;
           foreach(CashFlowDmSetup oItem in oTempCashFlowDmSetups)
           {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);                   
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                 _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
                _oPdfPCell = new PdfPCell(new Phrase(oItem.DisplayCaption, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0;
                if(nCount==0){_oPdfPCell.BorderWidthTop = 1;} if((nCount+1)==oTempCashFlowDmSetups.Count){_oPdfPCell.BorderWidthBottom = 1;}  _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1;  _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
               nCount++;
           }
            #endregion

            #region Operating_Closing_Caption
             oTempCashFlowDmSetup = GetSpecificList(4)[0]; //Operating_Closing_Caption = 4
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);                   
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 2;_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oTempCashFlowDmSetup.DisplayCaption, _oFontStyle));
            _oPdfPCell.Colspan = 2;_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
            _oPdfPCell = new PdfPCell(new Phrase(oTempCashFlowDmSetup.AmountSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
           #endregion

           #endregion

            #region Blank Space
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
            #endregion

            #region Invesing Activities
            #region Investing_Opening_Caption 
             oTempCashFlowDmSetup = GetSpecificList(5)[0]; //Investing_Opening_Caption = 5
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);                   
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oTempCashFlowDmSetup.DisplayCaption, _oFontStyle));
            _oPdfPCell.Colspan = 3;_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
           #endregion

            #region Investing_Activities
           oTempCashFlowDmSetups =  GetSpecificList(6);//Investing_Activities = 6,
            nCount = 0;
           foreach(CashFlowDmSetup oItem in oTempCashFlowDmSetups)
           {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);                   
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                 _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
                _oPdfPCell = new PdfPCell(new Phrase(oItem.DisplayCaption, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0;
                if(nCount==0){_oPdfPCell.BorderWidthTop = 1;} if((nCount+1)==oTempCashFlowDmSetups.Count){_oPdfPCell.BorderWidthBottom = 1;}  _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1;  _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
               nCount++;
           }
            #endregion

            #region Investing_Closing_Caption
             oTempCashFlowDmSetup = GetSpecificList(7)[0]; //Investing_Closing_Caption = 7
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);                   
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 2;_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oTempCashFlowDmSetup.DisplayCaption, _oFontStyle));
            _oPdfPCell.Colspan = 2;_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
            _oPdfPCell = new PdfPCell(new Phrase(oTempCashFlowDmSetup.AmountSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
           #endregion

           #endregion

            #region Blank Space
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
            #endregion

            #region Financing Activities
            #region Financing_Opening_Caption
                oTempCashFlowDmSetup = GetSpecificList(8)[0]; //Financing_Opening_Caption = 8
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);                   
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oTempCashFlowDmSetup.DisplayCaption, _oFontStyle));
            _oPdfPCell.Colspan = 3;_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Financing_Activities
            oTempCashFlowDmSetups =  GetSpecificList(9);//Financing_Activities = 9,
            nCount = 0;
            foreach(CashFlowDmSetup oItem in oTempCashFlowDmSetups)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);                   
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
                _oPdfPCell = new PdfPCell(new Phrase(oItem.DisplayCaption, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0;
                if(nCount==0){_oPdfPCell.BorderWidthTop = 1;} if((nCount+1)==oTempCashFlowDmSetups.Count){_oPdfPCell.BorderWidthBottom = 1;}  _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1;  _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                nCount++;
            }
            #endregion

            #region Financing_Closing_Caption
                oTempCashFlowDmSetup = GetSpecificList(10)[0]; //Financing_Closing_Caption = 10
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);                   
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 2;_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oTempCashFlowDmSetup.DisplayCaption, _oFontStyle));
            _oPdfPCell.Colspan = 2;_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
            _oPdfPCell = new PdfPCell(new Phrase(oTempCashFlowDmSetup.AmountSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion

            #region Net_Cash_Flow_Caption
                oTempCashFlowDmSetup = GetSpecificList(11)[0]; //Net_Cash_Flow_Caption = 11
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);                   
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 2;_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oTempCashFlowDmSetup.DisplayCaption, _oFontStyle));
            _oPdfPCell.Colspan = 2;_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
            _oPdfPCell = new PdfPCell(new Phrase(oTempCashFlowDmSetup.AmountSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Begaining_Cash_Caption
                oTempCashFlowDmSetup = GetSpecificList(12)[0]; //Begaining_Cash_Caption = 12
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);                   
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 2;_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oTempCashFlowDmSetup.DisplayCaption, _oFontStyle));
            _oPdfPCell.Colspan = 2;_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
            _oPdfPCell = new PdfPCell(new Phrase(oTempCashFlowDmSetup.AmountSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Closing_Cash_Caption
                oTempCashFlowDmSetup = GetSpecificList(13)[0]; //Closing_Cash_Caption = 13
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);                   
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 2;_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oTempCashFlowDmSetup.DisplayCaption, _oFontStyle));
            _oPdfPCell.Colspan = 2;_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
            _oPdfPCell = new PdfPCell(new Phrase(oTempCashFlowDmSetup.AmountSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region extra line
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);                   
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 3f; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight=3f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight =3f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
       }
       #endregion
       private List<CashFlowDmSetup> GetSpecificList(int nHeadType)
       {
           List<CashFlowDmSetup> oCashFlowDmSetups = new List<CashFlowDmSetup>();
           foreach(CashFlowDmSetup oItem in _oCashFlowDmSetups)
           {
               if(oItem.CashFlowHeadTypeInt==nHeadType)
               {
                   oCashFlowDmSetups.Add(oItem);
               }
           }
           return oCashFlowDmSetups;
       }
   }   
}
