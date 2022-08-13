
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
using System.ComponentModel;

namespace ESimSol.Reports
{

    public class rptFabricBatchCard
    {
        #region Declaration

        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        public iTextSharp.text.Image _oImag { get; set; }
        PdfPTable _oPdfPTable = new PdfPTable(5);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        FabricBatchProduction _oWarping = new FabricBatchProduction();
        FabricBatchProduction _oSizing = new FabricBatchProduction();
        FabricBatchProduction _oWeaving = new FabricBatchProduction();

        DocPrintEngine _oDocPrintEngine = new DocPrintEngine();
        //List<FabricExecutionOrderDetail> _oFEODs = new List<FabricExecutionOrderDetail>();
        Company _oCompany = new Company();

        public enum EnumSizingSection
        {
            CURRENTDATE,

            PATTERNAME,
            PO,
            Program,
            Date,

            Construction,
            Shift,
            Customer,
            WarpMnf,

            SIZEMCNO,
            WarpCount,
            BeamNo,
            LotNo,

            SizePickup,
            Warplength,
            NoOfEnds,
            SizerName,

            GTAKNOTTEDBY,
            KNOTTEDDate,
            KNOTTEDTime,
            Chemical,
            //SHIFT_IN_CHARGE = "@SHIFT_IN_CHARGE"
        }

        #endregion

        public byte[] PrepareReport(FabricBatchProduction oWarping, FabricBatchProduction oSizing, FabricBatchProduction oWeaving, Company oCompany)
        {

            _oWarping = oWarping;
            _oSizing = oSizing;
            _oWeaving = oWeaving;
            _oCompany = oCompany;
            //_oFEODs = oFEODs;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(300, 560), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(5f, 5f, 10f, 10f);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.SetWidths(new float[] {     15f,//caption
                                                    57f,//title 1
                                                    55f,//title1 value
                                                    57f,//title 2
                                                    55f//title2 value
                                                    });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {

            #region CompanyHeader

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPCell.Colspan = 5;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Weaving Unit", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 5; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

          
        }
        #endregion

        #region Report Body

        private void PrintBody()
        {

            #region Warpig
            #region date and reed
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Warping " , _oFontStyle));
            _oPdfPCell.Rowspan = 14; _oPdfPCell.Rotation = 90; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
         
            _oPdfPCell = new PdfPCell(new Phrase(_oWarping.StartDateSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Reed", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Pgm and fto f (mm)
            _oPdfPCell = new PdfPCell(new Phrase("Pgm", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oWarping.BatchNo, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("F to F(mm)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Buyer and G/Y lot
            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oWarping.BuyerName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("G/Y lot", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            string LotNos = string.Join(",",_oWarping.FabricBatchRawMaterials.Select(o=>o.OnlyLotNo).Distinct());
            _oPdfPCell = new PdfPCell(new Phrase(LotNos, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Order and Wp length
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oWarping.FEONo, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Wp length(M)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oWarping.Qty * 0.9144), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Construction 
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Const.", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oWarping.Construction, _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);         
            _oPdfPTable.CompleteRow();
            #endregion

            #region Weave
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Weave", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oWarping.FabricWeave, _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Count and Beam No
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Count", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //string sProducts = string.Join(",", _oFEODs.Select(o => o.ProductName).Distinct());
            string sProducts = string.Join(",", "".Distinct());
            _oPdfPCell = new PdfPCell(new Phrase(sProducts, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Beam No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(string.Join(",", _oWarping.FBPBs.Select(o => o.BeamNo)), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region TE and Warp bks
            _oPdfPCell = new PdfPCell(new Phrase("TE", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oWarping.TotalEnds.ToString(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Warp bks", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oWarping.WarpCount.ToString(), _oFontStyle));
            _oPdfPCell = new PdfPCell(new Phrase(string.Join(",", _oWarping.FabricBatchProductionBatchMans.Select(o => o.TotalNoOfBreakage)), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region B.type and Shift
            _oPdfPCell = new PdfPCell(new Phrase("B.type", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shift", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Shade and Batch
            _oPdfPCell = new PdfPCell(new Phrase("Shade", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Batch", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shade", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Batch", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Shade and Batch value
            for (int i = 0; i < 3;i++ )//this is temporary change will be next
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 13f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 13f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 13f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 13f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Warper and Supervisor
            _oPdfPCell = new PdfPCell(new Phrase("Warper", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Supervisor", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 5; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Sizing
            #region date and Shift
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Sizing ", _oFontStyle));
            _oPdfPCell.Rowspan = (_oSizing.FabricBatchRawMaterials.Count + 4); _oPdfPCell.Rotation = 90; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oSizing.StartDateSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shift", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region S/N and Chemical
            _oPdfPCell = new PdfPCell(new Phrase("S/N", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Chemical", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Chemical value print
            int nCount = 0;
            if (_oSizing.FabricBatchRawMaterials.Count > 0)
            {
                foreach (FabricBatchRawMaterial oItem in _oSizing.FabricBatchRawMaterials)
                {
                    nCount++;
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.FixedHeight = 13f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCell.FixedHeight = 13f; _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
            }
            #endregion

            #region Beam No And Length
            _oPdfPCell = new PdfPCell(new Phrase("Beam No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Length", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oSizing.QtySt.ToString(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Sizer
            _oPdfPCell = new PdfPCell(new Phrase("Sizer", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 5; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Weaving
            #region Loom machine and date 
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Weaving " , _oFontStyle));
            _oPdfPCell.Rowspan = 11; _oPdfPCell.Rotation = 90; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Loom N-", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase( _oWeaving.FabricMachineName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Date- " + _oWeaving.StartDateSt, _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Wf lot and shift
            _oPdfPCell = new PdfPCell(new Phrase("Wf lot", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shift", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Shade and Batch
            _oPdfPCell = new PdfPCell(new Phrase("Shade", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Batch", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shade", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Batch", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Shade and Batch value
            for (int i = 0; i < 3;i++ )//this is temporary change will be next
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 13f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 13f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 13f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 13f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Blank
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Quality Check
            _oPdfPCell = new PdfPCell(new Phrase("Quality Check", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 2;_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            #endregion

            #region Shift In Charge
            _oPdfPCell = new PdfPCell(new Phrase("Shift In Charge", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 2;_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Runnig Time
            _oPdfPCell = new PdfPCell(new Phrase("Runnig Time", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            int nCountDays = 0;
            if (_oWeaving.StartTime != new DateTime(1900, 01, 01, 1, 1, 1))
            {
                if (_oWeaving.EndTime == new DateTime(1900, 01, 01, 1, 1, 1))
                {
                    _oWeaving.EndTime = DateTime.Now;
                }
                nCountDays = Convert.ToInt32((_oWeaving.EndTime - _oWeaving.StartTime).TotalDays);
            }

            _oPdfPCell = new PdfPCell(new Phrase(nCountDays.ToString(), _oFontStyle));
            _oPdfPCell.Colspan = 2;_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
        }

        #endregion

        public byte[] PrepareReport_Dynamic_Sizing(FabricBatchProduction oWarping, FabricBatchProduction oSizing, FabricBatchProduction oWeaving, Company oCompany, DocPrintEngine oDocPrintEngine)
        {
            _oCompany = oCompany;

            _oDocPrintEngine = oDocPrintEngine;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(300, 560), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(5f, 5f, 10f, 10f);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _oPdfPTable = new PdfPTable(1);
            
            float[] fPageSize = this.getSplits(_oDocPrintEngine.PageSize, ',', 2);
            float[] fMargin = this.getSplits(_oDocPrintEngine.Margin, ',', 4);

            _oDocument = new Document(new iTextSharp.text.Rectangle(fPageSize[0], fPageSize[1]), fMargin[0], fMargin[1], fMargin[2], fMargin[3]);
            _oPdfPTable.SetWidths(new float[] { fPageSize[0] });
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter oPdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            //_oDocument.PageSize.Border = 15;
            //_oDocument.PageSize.Border = 15;
            //_oDocument.PageSize.BorderWidth = 5;
            //_oDocument.PageSize.DisableBorderSide(0);
            //_oDocument.PageSize.EnableBorderSide(0);
            //_oDocument.PageSize.GrayFill = 15;

            _oDocument.Open();
            #endregion

            this.PrintDynamicTable(fPageSize[0], oSizing);
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        public byte[] PrepareReport_Dynamic_Warping(FabricBatchProduction oWarping, FabricBatchProduction oSizing, FabricBatchProduction oWeaving, Company oCompany, DocPrintEngine oDocPrintEngine)
        {
            _oWarping = oWarping;
            _oSizing = oSizing;
            _oWeaving = oWeaving;
            _oCompany = oCompany;

            _oDocPrintEngine = oDocPrintEngine;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(300, 560), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(5f, 5f, 10f, 10f);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _oPdfPTable = new PdfPTable(1);

            float[] fPageSize = this.getSplits(_oDocPrintEngine.PageSize, ',', 2);
            float[] fMargin = this.getSplits(_oDocPrintEngine.Margin, ',', 4);

            _oDocument = new Document(new iTextSharp.text.Rectangle(fPageSize[0], fPageSize[1]), fMargin[0], fMargin[1], fMargin[2], fMargin[3]);
            _oPdfPTable.SetWidths(new float[] { fPageSize[0] });
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter oPdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
         
            _oDocument.Open();
            #endregion

            this.PrintDynamicTable(fPageSize[0], oWarping);
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void PrintDynamicTable(float fWidth, FabricBatchProduction oFabricBatchProduction)
        {
            PdfPTable oPdfPTable_Main = new PdfPTable(1);
            oPdfPTable_Main.SetWidths(new float[] { fWidth });

            foreach (var oItem in _oDocPrintEngine.DocPrintEngineDetails)
            {
                float[] nColumnWidths = this.getSplits(oItem.SetWidths, ',', oItem.SetWidths.Split(',').Count());
                float[] nFonts = this.getSplits(oItem.FontSize, ',', oItem.FontSize.Split(',').Count());
                string[] sAligns = this.getSplits(oItem.SetAligns, ',');
                string[] sFieldDatas = this.getSplits(oItem.SetFields, '#');

                PdfPTable oPdfPTable = new PdfPTable(nColumnWidths.Length);
                oPdfPTable.SetWidths(nColumnWidths);
                oPdfPTable.DefaultCell.Border = 0;

                if (!string.IsNullOrEmpty(oItem.TableName) && !oItem.TableName.Contains("@ISNULL"))
                {
                    if (oItem.TableName.ToUpper().Contains("@COMPANYHEADER"))
                    {
                        ESimSolPdfHelper.PrintHeader_Sticker(ref oPdfPTable, _oCompany, oItem.SetFields, 0);
                    }
                    else if (oItem.TableName.Contains("@HEADER"))
                    {
                        foreach (var oFData in sFieldDatas)
                        {
                            ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.BOLD);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, oFData, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 0);
                        }
                    }
                    else if (oItem.TableName.Contains("@ISBOLD"))
                    {
                        foreach (var oFData in sFieldDatas)
                        {
                            ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.BOLD);

                            int nIndexOf = sFieldDatas.ToList().IndexOf(oFData);
                            var sCFData = this.ConvertData(oFData, oFabricBatchProduction);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData, (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[nIndexOf])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 0);
                        }
                    }
                    else if (oItem.TableName.Contains("@HASBORDER_DOTTED"))
                    {
                        foreach (var oFData in sFieldDatas)
                        {
                            ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.BOLD);

                            var sCFData = this.ConvertData(oFData, oFabricBatchProduction);

                            iTextSharp.text.pdf.draw.DottedLineSeparator sep = new iTextSharp.text.pdf.draw.DottedLineSeparator();

                            sep.Percentage = 100;
                            sep.LineWidth = 1;
                            sep.Gap = 2f;
                            sep.Offset = -2;
                            sep.Alignment = Element.ALIGN_BOTTOM;
                            Phrase oPhrase = new Phrase(sCFData, ESimSolPdfHelper.FontStyle);

                            PdfPCell oPdfPCell = new PdfPCell(oPhrase);

                            var nIndexOf = sFieldDatas.ToList().IndexOf(oFData) % 2;
                            if (nIndexOf != 0 && nIndexOf != -1)
                            {
                                oPhrase.Add(sep);
                            }

                            ESimSolPdfHelper.AddCell(ref oPdfPTable, oPdfPCell, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, (int)oItem.RowHeight, 0);
                        }
                    }
                }
                else if (oItem.SetFields.Contains("@ISNULL") || string.IsNullOrEmpty(oItem.SetFields))
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, nColumnWidths.Length, (int)oItem.RowHeight, 0);
                }
                else
                {
                    #region PRINT FIELD DATA
                    int nCount = 0;
                    foreach (var oFData in sFieldDatas)
                    {
                        ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, (nFonts.Length != nColumnWidths.Length ? nFonts[0] : nFonts[nCount]), iTextSharp.text.Font.NORMAL);

                        var sCFData = this.ConvertData(oFData, oFabricBatchProduction);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData,
                            (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[nCount])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, (int)oItem.RowHeight);
                        nCount++;
                    }
                    #endregion
                }
                oPdfPTable.CompleteRow();
                ESimSolPdfHelper.AddTable(ref oPdfPTable_Main, oPdfPTable, 0, 0, 0);
            }
            oPdfPTable_Main.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable_Main, 15, 0, 0);

            _oPdfPTable.CompleteRow();
        }

        #region Supprot Funtion
        private string ConvertData(string oFData, FabricBatchProduction oFabricBatchProduction)
        {
            if (oFData.Contains("@"))
            {
                foreach (string oItem in oFData.Split(' ')) 
                {
                    if (oItem.Contains("@"))
                    {
                        string sPropertyValue = "";
                        string sPropertyName = oItem.Replace("@", "");

                        // ======= GET PROPERTY VALUE ====== //
                        var oProperty = oFabricBatchProduction.GetType().GetProperty(sPropertyName);
                        if (oProperty != null)
                        {
                            var Data = oProperty.GetValue(oFabricBatchProduction, null);
                            if (Data != null && (Data.GetType() == typeof(double))) // || Data.GetType() == typeof(int)
                            {
                                sPropertyValue = Global.MillionFormat_Round(Convert.ToDouble(Data));
                            }
                            sPropertyValue = Data.ToString();
                        }
                        ESimSolPdfHelper.FindAndReplace(ref oFData, "@" + sPropertyName, sPropertyValue);
                    }
                }
                //ESimSolPdfHelper.FindAndReplace(ref oFData, "@" + EnumSizingSection.PO.ToString(), oFabricBatchProduction.FEONo);
                //ESimSolPdfHelper.FindAndReplace(ref oFData, "@" + EnumSizingSection.Date.ToString(), oFabricBatchProduction.StartDateSt);
                //ESimSolPdfHelper.FindAndReplace(ref oFData, "@" + EnumSizingSection.Shift.ToString(), oFabricBatchProduction.ShiftID.ToString());
                //ESimSolPdfHelper.FindAndReplace(ref oFData, "@" + EnumSizingSection.Construction.ToString(), oFabricBatchProduction.Construction);
                //ESimSolPdfHelper.FindAndReplace(ref oFData, "@" + EnumSizingSection.Customer.ToString(), oFabricBatchProduction.BuyerName);
                //ESimSolPdfHelper.FindAndReplace(ref oFData, "@" + EnumSizingSection.WarpCount.ToString(), Global.MillionFormat(oFabricBatchProduction.WarpCount));
                //ESimSolPdfHelper.FindAndReplace(ref oFData, "@" + EnumSizingSection.Warplength.ToString(), Global.MillionFormat(oFabricBatchProduction.WarpDoneQty));

                //ESimSolPdfHelper.FindAndReplace(ref oFData, "@" + EnumSizingSection.SIZEMCNO.ToString(), oFabricBatchProduction.MachineCode);
                //ESimSolPdfHelper.FindAndReplace(ref oFData, "@" + EnumSizingSection.BeamNo.ToString(), oFabricBatchProduction.BeamNo);
                //ESimSolPdfHelper.FindAndReplace(ref oFData, "@" + EnumSizingSection.LotNo.ToString(), oFabricBatchProduction.BatchNo);
                //ESimSolPdfHelper.FindAndReplace(ref oFData, "@" + EnumSizingSection.NoOfEnds.ToString(), Global.MillionFormat(oFabricBatchProduction.TotalEnds));
                //ESimSolPdfHelper.FindAndReplace(ref oFData, "@" + EnumSizingSection.Customer.ToString(), oFabricBatchProduction.BuyerName);
            }
            return oFData;
        }

        private int GetAlgin(string sAlign)
        {
            switch (sAlign.ToUpper())
            {
                case "LEFT":
                    return Element.ALIGN_LEFT;
                case "RIGHT":
                    return Element.ALIGN_RIGHT;
                case "TOP":
                    return Element.ALIGN_TOP;
                case "CENTER":
                    return Element.ALIGN_CENTER;
                default:
                    return Element.ALIGN_LEFT;
            }
        }
        private float[] getSplits(string sString, char nCH, int nLength)
        {
            if (string.IsNullOrEmpty(sString)) return null;

            int nCount = 0;
            float[] nResult = new float[nLength];

            if (nLength > 0)
            {
                if (sString.Split(nCH).Length == nLength)
                {
                    foreach (string oitem in sString.Split(nCH))
                    {
                        nResult[nCount++] = (float)Convert.ToDouble(oitem);
                    }
                }
                else throw new Exception("Sorry, String is Not In Correct Format!!");
            }
            return nResult;
        }
        private string[] getSplits(string sString, char nCH)
        {
            int nCount = 0;
            string[] nResult = new string[sString.Split(nCH).Length];

            foreach (string oitem in sString.Split(nCH))
            {
                nResult[nCount++] = oitem;
            }
            return nResult;
        }
        #endregion
    }
    
}
