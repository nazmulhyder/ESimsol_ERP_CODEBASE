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
using System.Linq;
using ICS.Core.Framework;

namespace ESimSol.Reports
{
    public class rptPrintPad
    {
        #region Declaration

        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(5);
        PdfPCell _oPdfPCell; 
        Paragraph _oPdfParagraph;
        MemoryStream _oMemoryStream = new MemoryStream();
        LetterSetupEmployee _oLetterSetupEmployee = new LetterSetupEmployee();
        LetterSetup _oLetterSetup = new LetterSetup();
        List<LetterSetupEmployee> _oLetterSetupEmployees = new List<LetterSetupEmployee>();
        Company _oCompany = new Company();
        DateTime _dstartDate = DateTime.Now;
        DateTime _dEndDate = DateTime.Now;

        #endregion

        public byte[] PrepareReport(LetterSetupEmployee oLetterSetupEmployee, LetterSetup oLS)
        {
            _oLetterSetupEmployee = oLetterSetupEmployee;
            _oLetterSetup = oLS;
            string pageSize = _oLetterSetup.PageSize;
            float width = (float)(Convert.ToDouble(pageSize.Split(',')[0]) * 71.95);
            float height = (float)(Convert.ToDouble(pageSize.Split(',')[1]) * 72.02737);


            #region Page Setup
            //_oDocument = new Document(new iTextSharp.text.Rectangle(842, 595), 0f, 0f, 0f, 0f);//LANDSCAPE
            _oDocument = new Document(new iTextSharp.text.Rectangle(width, height), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins((float)(_oLetterSetup.MarginLeft * 71.95), (float)(_oLetterSetup.MarginRight * 71.95), (float)(_oLetterSetup.MarginTop * 72.02737), (float)(_oLetterSetup.MarginBottom * 72.02737));
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { width / 4-25, 25, width / 4, width / 4, width / 4 });
            #endregion

            this.PrintBody();

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintBody()
        {
            List<string> partsOfLetter = new List<string>();
            bool isContailtau = _oLetterSetupEmployee.Body.Contains("~");
            int nCount = 0;
            if (isContailtau)
            {
                nCount = _oLetterSetupEmployee.Body.Split('~').Length;
                for (int i = 0; i < nCount; i++)
                {
                    string sPart = _oLetterSetupEmployee.Body.Split('~')[i];
                    partsOfLetter.Add(sPart);
                }
            }

            _oPdfPCell = new PdfPCell(new Phrase(_oLetterSetup.Name, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            if (nCount <= 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.NORMAL);

                //_oPdfParagraph = new Paragraph(new Phrase(_oLetterSetupEmployee.Body, _oFontStyle));
                //_oPdfParagraph.SetLeading(0f, 1.5f);
                //_oPdfParagraph.Alignment = Element.ALIGN_JUSTIFIED;
                //_oPdfPCell = new PdfPCell();
                //_oPdfPCell.AddElement(_oPdfParagraph);


                var oLines = _oLetterSetupEmployee.Body.Split('\n');

                foreach (var oItem in oLines) 
                {

                    _oPdfPCell = new PdfPCell(new Phrase(oItem, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPTable.AddCell(_oPdfPCell);

                }

                //_oPdfPCell = new PdfPCell(new Phrase(_oLetterSetupEmployee.Body, _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
            }
            else
            {
                PdfPTable oPdfPTable = new PdfPTable(4);
                _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.NORMAL);


                var oLines = _oLetterSetupEmployee.Body.Split('\n');
                bool bFlag = true;
                foreach (var oItem in oLines)
                {

                    if (bFlag && oItem.Contains("~"))
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = .5f; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("SalaryHead", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = .5f; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = .5f; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPTable.CompleteRow();

                        int nSL = 0;
                        double nTotal = 0.0;
                        for (int i = 1; i < partsOfLetter.Count-1; i+=2)
                        {
                            nSL++;
                            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.NORMAL);
                    
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = .5f; _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(partsOfLetter[i], _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = .5f; _oPdfPTable.AddCell(_oPdfPCell);

                            nTotal += Convert.ToDouble(partsOfLetter[i + 1]);
                            _oPdfPCell = new PdfPCell(new Phrase(Math.Round(Convert.ToDouble(partsOfLetter[i + 1])).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = .5f; _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPTable.CompleteRow();
                        }

                        _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = .5f; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = .5f; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Math.Round(nTotal).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = .5f; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPTable.CompleteRow();
                        bFlag = false;
                    }
                    else if (!oItem.Contains("~"))
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.NORMAL);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPTable.AddCell(_oPdfPCell);

                    }
                }

                //_oPdfPCell = new PdfPCell(new Phrase(_oLetterSetupEmployee.Body, _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfParagraph = new Paragraph(new Phrase(partsOfLetter[0], _oFontStyle));
                //_oPdfParagraph.SetLeading(0f, 1.5f);
                //_oPdfParagraph.Alignment = Element.ALIGN_JUSTIFIED;
                //_oPdfPCell = new PdfPCell();
                //_oPdfPCell.AddElement(_oPdfParagraph);

                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                

                //_oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.NORMAL);
                //_oPdfParagraph = new Paragraph(new Phrase(partsOfLetter[partsOfLetter.Count - 1], _oFontStyle));
                //_oPdfParagraph.SetLeading(0f, 1.5f);
                //_oPdfParagraph.Alignment = Element.ALIGN_JUSTIFIED;
                //_oPdfPCell = new PdfPCell();
                //_oPdfPCell.AddElement(_oPdfParagraph);

                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
            }
        }
    }
}
