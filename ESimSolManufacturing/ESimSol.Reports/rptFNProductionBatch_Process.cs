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

    public class rptFNProductionBatch_Process
    {
        #region Declaration
        int _nTotalColumn = 10;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();

        List<FNBatch> _oFNBatchs = new List<FNBatch>();
        List<FNBatchCard> _oFNBCards = new List<FNBatchCard>();
        List<FNProductionBatch> _oFNPBatchs = new List<FNProductionBatch>();
        List<FNProductionConsumption> _oFNPConsumptions = new List<FNProductionConsumption>();
        List<FabricSalesContractDetail> _oFSCDetails = new List<FabricSalesContractDetail>();

        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();

        string _sMessage = "";
        BaseColor _oBC = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EFF0EA"));
        #endregion

        public byte[] PrepareReport(List<FabricSalesContractDetail> oFSCDetails, List<FNBatchCard> oFNBCards, List<FNProductionBatch> oFNPBatchs, List<FNBatch> oFNBatchs, Company oCompany, BusinessUnit oBU)
        {
            _oFSCDetails = oFSCDetails;
            _oFNBCards = oFNBCards;
            _oFNBatchs = oFNBatchs.OrderBy(x=>x.FNExOID).ToList();
            //_oFNPConsumptions = oFNPConsumptions;
            //_oFNBCards = oFNBCards;
            _oCompany = oCompany;
            _oBusinessUnit = oBU;

            #region Page Setup
            _oDocument = new Document(PageSize.LEGAL.Rotate(), 0f, 0f, 0f, 0f);         
            //_oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f); //A4:842*595   
            _oDocument.SetMargins(10f, 10f, 5f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 1008f });
            #endregion

            #region Treatment Wise Print
            List<FNBatchCard> oFNBCards_Temp = new List<FNBatchCard>();
            oFNBCards_Temp = _oFNBCards.GroupBy(x => new { x.FNTreatment }, (key, grp) => new FNBatchCard
            {
                FNTreatment = key.FNTreatment,
                FNBatchCards = grp.ToList()
            }).ToList();

            foreach(FNBatchCard oItem in oFNBCards_Temp)
            {
                ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, _oBusinessUnit, _oCompany, " Production Report (" + oItem.FNTreatmentSt + ")", 0);
                this.PrintBody(oItem);
                ESimSolPdfHelper.NewPageDeclaration(_oDocument, _oPdfPTable);
            }

            #endregion

            _oPdfPTable.HeaderRows = 2;
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
                _oPdfPCell.Colspan = _nTotalColumn;
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
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("(Finishing Unit)", _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("Requisition for Dyes & Chemicals", _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody(FNBatchCard oFNBatchCard)
        {
            #region FN Production Batch
            // FN Traetment Process :: Distinct
            var oProcessList = oFNBatchCard.FNBatchCards.Select(x => new
            {
                FNTreatmentProcessID = x.FNTreatmentProcessID,
                FNProcess = x.FNProcess
            }).Distinct().ToList();

            _nTotalColumn = 8 + oProcessList.Count();
            PdfPTable oPdfPTable = new PdfPTable(_nTotalColumn);

            #region SET WIDHT
            int nProcessWidth = oProcessList.Count() * 35;
            int nAssigningWidth = (1008 - nProcessWidth) - 20 ;

            int nColCount = 0;
            float[] nColWidths = new float[_nTotalColumn];
            nColWidths[nColCount++] = 20f; //20f
            nColWidths[nColCount++] = (float)(nAssigningWidth * 0.10);
            nColWidths[nColCount++] = (float)(nAssigningWidth * 0.12);
            nColWidths[nColCount++] = (float)(nAssigningWidth * 0.28);
            nColWidths[nColCount++] = (float)(nAssigningWidth * 0.10);
            nColWidths[nColCount++] = (float)(nAssigningWidth * 0.10);

            if (oProcessList.Count() > 0)
                foreach (var oItem in oProcessList)
                    nColWidths[nColCount++] = 40;

            nColWidths[nColCount++] = (float)(nAssigningWidth * 0.15); ;
            nColWidths[nColCount++] = (float)(nAssigningWidth * 0.15); ;

            oPdfPTable.SetTotalWidth(nColWidths);
            #endregion

            #region Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Gray Issue Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            foreach (var oItem in oProcessList)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.FNProcess, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("R.F.D", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Delivered", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region ADD To Main Table
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
            oPdfPTable = new PdfPTable(_nTotalColumn); oPdfPTable.SetTotalWidth(nColWidths);
            #endregion

            int nCount = 0, nFNExOID=0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            foreach (var oItem_Batch in _oFNBatchs)
            {
                nCount++;
                var oFSCD = _oFSCDetails.Where(x=>x.FabricSalesContractDetailID==oItem_Batch.FNExOID).FirstOrDefault();
                if (oItem_Batch.FNExOID != nFNExOID)
                {
                    var oBatch_list = _oFNBCards.Where(x => x.FNBatchID == oItem_Batch.FNBatchID).ToList();
                    #region Data
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oFSCD.ExeNoFull, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oFSCD.ColorInfo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oFSCD.Construction, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oFSCD.Qty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oFSCD.RawFabricRcvQty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    foreach (var oItem in oProcessList)
                    {
                        double nQTY = 0;
                        oBatch_list.ForEach(p=>
                                nQTY+= _oFNPBatchs.Where(x=>x.FNBatchCardID==p.FNBatchCardID).Sum(x=>x.StartQty)
                            );
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQTY), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Add To Main Table
                    ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                    oPdfPTable = new PdfPTable(_nTotalColumn); oPdfPTable.SetTotalWidth(nColWidths);
                    #endregion
                    nFNExOID = oItem_Batch.FNExOID;
                }
            }

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 35f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }
        #endregion
    }
}
