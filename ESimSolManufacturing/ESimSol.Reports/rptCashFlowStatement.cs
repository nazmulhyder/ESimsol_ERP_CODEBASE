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
   public class rptCashFlowStatement
   {
       #region Declaration
       Document _oDocument;
       iTextSharp.text.Font _oFontStyle;
       int _nColumns = 6;
       PdfPTable _oPdfPTable = new PdfPTable(6);
       PdfPCell _oPdfPCell;
       iTextSharp.text.Image _oImag;
       MemoryStream _oMemoryStream = new MemoryStream();
       CashFlowSetup _oCashFlowSetup = new CashFlowSetup();

       List<CashFlowSetup> _oCashFlowSetups = new List<CashFlowSetup>();
       Company _oCompany = new Company();
       #endregion

       public byte[] PrepareReport(CashFlowSetup oCashFlowSetup, Company oCompany)
       {
           _oCashFlowSetup = oCashFlowSetup;
           _oCashFlowSetups = oCashFlowSetup.CashFlowSetups;
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
           _oPdfPCell = new PdfPCell(new Phrase("For the Year Ended " + _oCashFlowSetup.EndDateSt, _oFontStyle));
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
           _oPdfPCell = new PdfPCell(new Phrase(_oCashFlowSetup.SessionDate + " " + _oCompany.CurrencyName, _oFontStyle));
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
           CashFlowSetup oNetIncreaseFromCashflow = new CashFlowSetup();
           CashFlowSetup oOpeningBalance = new CashFlowSetup();
           CashFlowSetup oClosingBalance = new CashFlowSetup();
           int nCFTransactionCategoryInInt=0;
           for (int i=0; i< _oCashFlowSetups.Count;i++)
           {
              
                if(_oCashFlowSetups[i].CashFlowSetupID>1003)
                {
                if(_oCashFlowSetups[i].CashFlowSetupID==1004)
                {
                    oNetIncreaseFromCashflow = _oCashFlowSetups[i];
                }
                else if(_oCashFlowSetups[i].CashFlowSetupID==1005)
                {
                    oOpeningBalance = _oCashFlowSetups[i];
                }
                else if(_oCashFlowSetups[i].CashFlowSetupID==1006)
                {
                    oClosingBalance = _oCashFlowSetups[i];
                }
            }
            else
            {
                if(nCFTransactionCategoryInInt!=_oCashFlowSetups[i].CFTransactionCategoryInInt)
                {
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);                   
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                 _oPdfPCell = new PdfPCell(new Phrase(_oCashFlowSetups[i].SubGroupName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
                _oPdfPCell = new PdfPCell(new Phrase(_oCashFlowSetups[i].DisplayCaption, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                 i=i+1;
                }
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                 _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
                if(_oCashFlowSetups[i].CashFlowSetupID==1000 || _oCashFlowSetups[i].CashFlowSetupID==1001 || _oCashFlowSetups[i].CashFlowSetupID==1002 || _oCashFlowSetups[i].CashFlowSetupID==1003)
                {
                     _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                }
                else
                {   
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                }
                    
                _oPdfPCell = new PdfPCell(new Phrase(_oCashFlowSetups[i].DisplayCaption, _oFontStyle)); 
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
  
                if(_oCashFlowSetups[i].CashFlowSetupID==1000 || _oCashFlowSetups[i].CashFlowSetupID==1001 || _oCashFlowSetups[i].CashFlowSetupID==1002 || _oCashFlowSetups[i].CashFlowSetupID==1003)
                {
                    
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase(_oCashFlowSetups[i].AmountSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 1; _oPdfPTable.AddCell(_oPdfPCell);

                }
                else
                {
                     _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oPdfPCell = new PdfPCell(new Phrase(_oCashFlowSetups[i].AmountSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPTable.AddCell(_oPdfPCell);
                    
                }     
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                nCFTransactionCategoryInInt =_oCashFlowSetups[i].CFTransactionCategoryInInt;
            }
           }

           #region net incease from cash flow
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.Colspan = 6; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();


           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
           _oPdfPCell = new PdfPCell(new Phrase(oNetIncreaseFromCashflow.DisplayCaption, _oFontStyle));
           _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(oNetIncreaseFromCashflow.AmountSt, _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion

           #region Opening Balance
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.Colspan = 6; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();


           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
           _oPdfPCell = new PdfPCell(new Phrase(oOpeningBalance.DisplayCaption, _oFontStyle));
           _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(oOpeningBalance.AmountSt, _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion

           #region Closing Balance
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.Colspan = 6; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();


           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
           _oPdfPCell = new PdfPCell(new Phrase(oClosingBalance.DisplayCaption, _oFontStyle));
           _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(oClosingBalance.AmountSt, _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthTop = 1; _oPdfPTable.AddCell(_oPdfPCell);

           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion


           #region Double Line
       
           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.Colspan = 6; _oPdfPCell.FixedHeight = 2f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();

           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.FixedHeight = 2f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPTable.AddCell(_oPdfPCell);

           _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.FixedHeight = 2f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
           #endregion

           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
           _oPdfPCell.Colspan = 6; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
           _oPdfPTable.CompleteRow();
       }
       #endregion




   }

}
