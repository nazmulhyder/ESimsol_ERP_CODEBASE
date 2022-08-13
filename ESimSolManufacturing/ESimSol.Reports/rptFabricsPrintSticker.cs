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
    public class rptFabricsPrintSticker
    {
        #region Declaration

        int _nTotalColumn = 3;

        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(3);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Fabric _oFabric = new Fabric();
        List<Fabric> _oFabrics = new List<Fabric>();
        Company _oCompany = new Company();
        public byte[] PrepareReport(Fabric oFabric, Company oCompany)
        {
            _oFabric = oFabric;
            _oFabrics = oFabric.Fabrics;
            _oCompany = oCompany;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 100f, 30f, 100f});
            #endregion

            this.PrintBody();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #endregion

        #region Report Body
        private void PrintBody() {
            if (_oFabrics.Count > 0) {

                _oPdfPCell = new PdfPCell();
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.FixedHeight = 30;
                _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                int nCount = 0;

                for (int i = 0; i < _oFabrics.Count; i++)
                {
                    #region For Odd number of items
                    if ((_oFabrics.Count % 2) != 0)
                    {
                        nCount++;
                        _oPdfPCell = new PdfPCell(MakeSingleSticker(_oFabrics[i]));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell();
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        if (nCount == _oFabrics.Count)
                        {
                            _oPdfPCell = new PdfPCell();
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(MakeSingleSticker(_oFabrics[i + 1]));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                            i++;
                            nCount++;
                        }
                        _oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell();
                        _oPdfPCell.Colspan = 3;
                        _oPdfPCell.FixedHeight = 40;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                    }
                    #endregion
                    #region For Even Number of items
                    else {
                        nCount++;
                        _oPdfPCell = new PdfPCell(MakeSingleSticker(_oFabrics[i]));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell();
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(MakeSingleSticker(_oFabrics[i + 1]));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);
                        i++;

                        _oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell();
                        _oPdfPCell.Colspan = 3;
                        _oPdfPCell.FixedHeight = 40;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                    }
                    #endregion
                }
            }
        }
        #endregion

        private PdfPTable MakeSingleSticker(Fabric oFabric)
        {
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 50f, 50f });
            PdfPCell oPdfPCell;

            oPdfPCell = new PdfPCell(new Phrase(_oFabric.StickerHeader, _oFontStyle));
            oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Fabric Mill Name", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Fabric Article No.", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Composition", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabric.ProductName, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabric.Construction, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Width", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabric.FabricWidth, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Weight", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("FinishType", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabric.FinishTypeName, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Price", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            return oPdfPTable;
        }
    }
}
