using System;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.Reports
{
    public class rptRollDoffingCard
    {
        #region Declaration

        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        public iTextSharp.text.Image _oImag { get; set; }
        PdfPTable _oPdfPTable = new PdfPTable(2);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        FabricBatchProduction _oFBP = new FabricBatchProduction();

        Company _oCompany = new Company();

        #endregion



        public byte[] PrepareReport(FabricBatchProduction oFBP, Company oCompany)
        {

            _oFBP = oFBP;
            _oCompany = oCompany;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(250, 400), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(5f, 5f, 10f, 10f);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.SetWidths(new float[] {170f, 170f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            #endregion

            this.PrintHeader();
            this.PrintBody();

            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }


        #region Report Header
        private void PrintHeader()
        {

            #region CompanyHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

          
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Weaving Unit", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Loom No.", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


        }
        #endregion


        #region Report Body

        private void PrintBody()
        {

            #region Content part

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            #region Buyer and order
            _oPdfPCell = new PdfPCell(new Phrase("Buyer :" + _oFBP.BuyerName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order No :" + _oFBP.OrderNo, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Construction and weave
            _oPdfPCell = new PdfPCell(new Phrase("Const :" + _oFBP.Construction, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Weave :" + _oFBP.FabricWeave, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region TE and RC
            _oPdfPCell = new PdfPCell(new Phrase("TE :", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("RC :" , _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Bl and warp not
            _oPdfPCell = new PdfPCell(new Phrase("BL :", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Warp Lot :", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region GREY PICK and weft lot
            _oPdfPCell = new PdfPCell(new Phrase("Grey Check :", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Weft Lot :", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #region B No and R Date
            _oPdfPCell = new PdfPCell(new Phrase("B No: "+_oFBP.BatchNo, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("R Date: "+_oFBP.StartDateSt, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #region B No and R Date
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Roll Doffing Card", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
            #region Batch Man wise shift
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            PdfPTable oPdfPTable = new PdfPTable(5);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.SetWidths(new float []{
                                                60f,//DAte
                                                40f,//Time
                                                40f,//shift
                                                50f,//length
                                                60f,//Signature
                                                });
            #region heading
            _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Time", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shift", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Length", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Signature", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion
            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            if(_oFBP.FabricBatchProductionBatchMans.Count>0)
            {
                foreach(FabricBatchProductionBatchMan oItem in _oFBP.FabricBatchProductionBatchMans)
                {
                    nCount++;
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.FinishDateInString, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;_oPdfPCell.BackgroundColor = BaseColor.WHITE;oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ShiftEndTime.ToString("HH:mm"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ShiftName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty)+"(Y)", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
            }
            while(nCount<17)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 12; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 12; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 12; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 12; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 12; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 18; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Shift In-Charge", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

        }

        #endregion

    }
}
