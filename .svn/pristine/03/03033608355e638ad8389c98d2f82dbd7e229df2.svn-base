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
   public class rptLoomCard
    {
        #region Declaration

        int _nTotalColumn = 3;

        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(3);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<FabricBatchProductionBeam> _oFBPBs = new List<FabricBatchProductionBeam>();
        List<FabricBatchRawMaterial> _oFBRMs = new List<FabricBatchRawMaterial>();
        public byte[] PrepareReport(List<FabricBatchProductionBeam> oFBPBs, List<FabricBatchRawMaterial> oFBRMs)
        {
            _oFBPBs = oFBPBs;
            _oFBRMs = oFBRMs;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(595f, 820f), 5f, 5f, 10f, 10f); //7.7" , 6" //739.2f, 595.2f  //
            _oDocument.SetMargins(5f, 5f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 225f, 60f, 225f });
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
            if (_oFBPBs.Count() > 0)
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

                for (int i = 0; i < _oFBPBs.Count(); i++)
                {
                    #region For Odd number of items
                    if ((_oFBPBs.Count() % 2) != 0)
                    {
                        nCount++;

                        if (nCount > 1)
                        {
                            _oPdfPCell = new PdfPCell();
                            _oPdfPCell.Colspan = 3;
                            _oPdfPCell.FixedHeight = 40;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();
                        }


                        _oPdfPCell = new PdfPCell(MakeLoomCardPrint(_oFBPBs[i]));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell();
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        if (nCount == _oFBPBs.Count())
                        {
                            _oPdfPCell = new PdfPCell();
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(MakeLoomCardPrint(_oFBPBs[i+1]));
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
                            _oPdfPCell.FixedHeight = 40;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();

                        }

                        _oPdfPCell = new PdfPCell(MakeLoomCardPrint(_oFBPBs[i]));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                       
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell();
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(MakeLoomCardPrint(_oFBPBs[i+1]));
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
        private PdfPTable MakeLoomCardPrint(FabricBatchProductionBeam oFBPB)
        {
            PdfPTable oPdfPTable = new PdfPTable(5);
            //oPdfPTable.SetWidths(new float[] { 50f, 75f,50f,75f });
            oPdfPTable.SetWidths(new float[] { 35f, 5f,65f, 50f, 75f });
            PdfPCell oPdfPCell;

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Colspan = 5;
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = 14;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, 1);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Colspan = 5;
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
           
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            oPdfPCell = new PdfPCell(new Phrase("Weaving Unit", _oFontStyle));
            oPdfPCell.Colspan = 5;
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = 14;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Colspan = 5;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = 12;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Loom No. "+oFBPB.MachineCode, _oFontStyle));
            oPdfPCell.Colspan = 5;
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = 14;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            oPdfPCell = new PdfPCell(new Phrase("BUYER ", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFBPB.BuyerName, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

           
            
            oPdfPCell = new PdfPCell(new Phrase("ORDER NO:", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFBPB.OrderNo, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("CONST ", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFBPB.Construction, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("WEAVE:", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFBPB.Weave, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("TE         ", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFBPB.TotalEnds.ToString(), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("RC:", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFBPB.LoomReedCount.ToString()+" "+((!string.IsNullOrEmpty(oFBPB.Dent))?"/"+oFBPB.Dent:""), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("BL         ", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFBPB.QtyInMtr.ToString(), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("WARP LOT:", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            string wraplot = "";
            if (_oFBRMs.Where(x => (int)x.WeavingProcess == 0 && x.FBID == oFBPB.FBID).ToList().Any())
            {
                wraplot = _oFBRMs.Where(x => (int)x.WeavingProcess == 0 && x.FBID == oFBPB.FBID).ToList().OrderByDescending(x => x.FBRMID).First().OnlyLotNo;
            }
            oPdfPCell = new PdfPCell(new Phrase(wraplot, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("GP        ", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("WEFT LOT:", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            string weftlot = "";
            if (_oFBRMs.Where(x => (int)x.WeavingProcess == 3 && x.FBID == oFBPB.FBID).ToList().Any())
            {
                weftlot = _oFBRMs.Where(x => (int)x.WeavingProcess == 3 && x.FBID == oFBPB.FBID).ToList().OrderByDescending(x => x.FBRMID).First().OnlyLotNo;
            }
            oPdfPCell = new PdfPCell(new Phrase(weftlot, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("B. NO    ", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFBPB.BeamNo, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("R DATE:", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFBPB.StartTimeStr, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Colspan = 5;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = 16;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

           

         
            return oPdfPTable;
        }
    }
}
