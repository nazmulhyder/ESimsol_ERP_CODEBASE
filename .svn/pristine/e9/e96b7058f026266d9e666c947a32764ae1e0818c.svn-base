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
    public class rptFabricStickersNew
    {
        #region Declaration

        int _nTotalColumn = 3;

        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(3);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        FabricSticker _oFabricSticker = new FabricSticker();

        public byte[] PrepareReport(FabricSticker oFabricSticker)
        {
            _oFabricSticker = oFabricSticker;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(446.18f, 548.5f).Rotate(), 5f, 5f, 5f, 5f); //7.7" , 6" //739.2f, 595.2f  //
            _oDocument.SetMargins(5f, 5f, 5f, 5f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 100f, 30f, 100f });
            #endregion

            this.PrintBody();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            if (_oFabricSticker.PrintCount > 0)
            {
                _oPdfPCell = new PdfPCell();
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.FixedHeight = 10;
                _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                int nCount = 0;

                for (int i = 0; i < _oFabricSticker.PrintCount; i++)
                {
                    #region For Odd number of items
                    if ((_oFabricSticker.PrintCount % 2) != 0)
                    {
                        nCount++;

                        if (nCount > 1)
                        {
                            _oPdfPCell = new PdfPCell();
                            _oPdfPCell.Colspan = 3;
                            _oPdfPCell.FixedHeight = 30;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();
                        }


                        _oPdfPCell = new PdfPCell(MakeSingleSticker(_oFabricSticker));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell();
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        if (nCount == _oFabricSticker.PrintCount)
                        {
                            _oPdfPCell = new PdfPCell();
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(MakeSingleSticker(_oFabricSticker));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                            i++;
                            nCount++;
                        }
                        _oPdfPTable.CompleteRow();


                    }
                    #endregion
                    #region For Even Number of items
                    else
                    {
                        nCount++;

                        if (nCount > 1)
                        {
                            _oPdfPCell = new PdfPCell();
                            _oPdfPCell.Colspan = 3;
                            _oPdfPCell.FixedHeight = 30;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();

                        }

                        _oPdfPCell = new PdfPCell(MakeSingleSticker(_oFabricSticker));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell();
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(MakeSingleSticker(_oFabricSticker));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);
                        i++;

                        _oPdfPTable.CompleteRow();

                    }
                    #endregion
                }
            }
        }
        #endregion
        private PdfPTable MakeSingleSticker(FabricSticker oFabricSticker)
        {
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 50f, 50f });
            PdfPCell oPdfPCell;

            oPdfPCell = new PdfPCell(new Phrase(oFabricSticker.Title, _oFontStyle));
            oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            if (oFabricSticker.Title == "")
            {
                oPdfPCell.FixedHeight = 12;
            }
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Fabric Mill Name", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabricSticker.FabricMillName, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Fabric Article No.", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabricSticker.FabricArticleNo, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Composition", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabricSticker.ProductName, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabricSticker.Construction, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Weave Design", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabricSticker.FabricWeaveName, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Width", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabricSticker.Width, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Weight(GSM)", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabricSticker.Weight, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("FinishType", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabricSticker.FinishTypeName, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Price", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase((oFabricSticker.Price > 0 ? Global.MillionFormat(oFabricSticker.Price) : ""), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            if (!string.IsNullOrEmpty(oFabricSticker.Email) || !string.IsNullOrEmpty(oFabricSticker.Phone))
            {
                oPdfPCell = new PdfPCell(new Phrase(oFabricSticker.Email, _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(oFabricSticker.Phone, _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            return oPdfPTable;
        }
    }
}
