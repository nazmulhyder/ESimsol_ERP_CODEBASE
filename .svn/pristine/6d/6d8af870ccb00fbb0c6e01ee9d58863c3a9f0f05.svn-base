using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Utility;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace ESimSol.Reports
{
    public class rptFabricBatchProductionPrints
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyle_Bold_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);//number of columns
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<FabricBatchProductionDetail> _oFabricBatchProductionDetails = new List<FabricBatchProductionDetail>();
        List<FabricBatch> _oFabricBatchs = new List<FabricBatch>();
        List<FabricBatchProductionBeam> _oFabricBatchProductionBeams = new List<FabricBatchProductionBeam>();
        Company _oCompany = new Company();
        string _sDateRange = "";
        #endregion
        #region Warping
        public byte[] PrepareReport( List<FabricBatch> oFabricBatchs,List<FabricBatchProductionDetail> oFabricBatchProductionDetails,List<FabricBatchProductionBeam> oFabricBatchProductionBeams, Company oCompany, BusinessUnit oBusinessUnit, string sDateRange)
        {
            _oFabricBatchProductionDetails = oFabricBatchProductionDetails;
            _oFabricBatchs = oFabricBatchs;
            _oFabricBatchProductionBeams = oFabricBatchProductionBeams;
            _oCompany = oCompany;
            _sDateRange = sDateRange;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
           
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 800f });
            #endregion
            this.PrintHeaderTwo();
            this.PrintBody();
            Signature();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintHeaderTwo()
        {
            #region CompanyHeader


            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 130f, 200.5f, 130f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.Rowspan = 2;
                _oPdfPCell.MinimumHeight = 25;
                oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Rowspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.UNDERLINE | iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("WARPING PRODUCTION", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Production Date " + _sDateRange, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Reporting Date " + DateTime.Now.ToString("dd MMM yyyy"), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBody()
        {
          
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.UNDERLINE);

            PdfPTable oPdfPTable = new PdfPTable(12);

            //#region DATA
            int nSL = 1;
            double nQty = 0;
            double nWarpBeam = 0;
            var oMachines = _oFabricBatchProductionDetails.GroupBy(x => new { x.FMID, x.MachineName })
                                                .OrderBy(x => x.Key.FMID)
                                                .Select(x => new
                                                {
                                                    FMID = x.Key.FMID,
                                                    MachineName = x.Key.MachineName

                                                });

            var oFBs = _oFabricBatchProductionDetails.GroupBy(x => new { x.FBID, x.FMID, x.MachineName }, (key, grp) =>
                                   new
                                   {
                                       FBID = key.FBID,
                                       FMID = key.FMID,
                                       MachineName = key.MachineName,
                                   }).ToList();


         
            foreach (var oMachine in oMachines)
            {
                var oFabricBatchs = oFBs.Where(x => x.FMID == oMachine.FMID).ToList();
              
                #region Fabric Info
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.SetWidths(new float[] { 20f, 120f, 80f, 80f, 120f, 60f, 45f, 80f, 45f, 48f, 48f, 55f });
                //if (oMachines.le > 0)
                //{
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oMachine.MachineName, 0, 18, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    oPdfPTable.CompleteRow();
                //}
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Program No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construction", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Ends", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "T. Beam", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Satting Length", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warp Beam", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warp Length", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Breakage", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Remarks", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion
                nSL = 0;
                //oFabricSizingPlans = oFabricSizingPlans.OrderBy(o => o.Sequence).ToList();
                foreach (var oItem1 in oFabricBatchs)
                {
                    var oFabricBatch = _oFabricBatchs.Where(x => x.FBID == oItem1.FBID).ToList();

                    var oFabricBatchProsDetails = _oFabricBatchProductionDetails.Where(x => x.FMID == oMachine.FMID && x.FBID == oItem1.FBID).ToList();
                    nWarpBeam = 0;
                    foreach (var oIem in oFabricBatchProsDetails)
                    {
                     nWarpBeam=_oFabricBatchProductionBeams.Where(x => x.FBPDetailID == oIem.FBPDetailID && x.FBID == oItem1.FBID).Count();
                    }

                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.SetWidths(new float[] { 20f, 120f, 80f, 80f, 120f, 60f, 45f, 80f, 45f, 48f, 48f, 55f });

                    nSL++;
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSL.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFabricBatch.FirstOrDefault().BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFabricBatch.FirstOrDefault().FEONo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFabricBatch.FirstOrDefault().BatchNoMCode, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFabricBatch.FirstOrDefault().Construction, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFabricBatch.FirstOrDefault().TotalEnds.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (string.IsNullOrEmpty(oFabricBatch.FirstOrDefault().WarpBeam) ? "" : oFabricBatch.FirstOrDefault().WarpBeam), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oFabricBatch.FirstOrDefault().Qty) > 0 ? oFabricBatch.FirstOrDefault().Qty.ToString("#,##0.00;(#,##0.00)") : ""), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                 //   nQty = _oFabricBatchProductionBeams.Where(p => p.FBID == oItem1.FBID && p.FBPDetailID == oMachine.FMID).Sum(x => x.QtyM);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nWarpBeam) > 0 ? nWarpBeam.ToString() : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                    nQty = _oFabricBatchProductionDetails.Where(p => p.FBID == oItem1.FBID && p.FMID == oMachine.FMID).Sum(x => x.QtyM);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nQty) > 0 ? nQty.ToString("#,##0.00;(#,##0.00)") : ""), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    nQty = _oFabricBatchProductionDetails.Where(p => p.FBID == oItem1.FBID && p.FMID == oMachine.FMID).Sum(x => x.NoOfBreakage);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nQty) > 0 ? nQty.ToString("#,##0.00;(#,##0.00)") : ""), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }

                oPdfPTable = new PdfPTable(12);
                oPdfPTable.SetWidths(new float[] { 20f, 120f, 80f, 80f, 120f, 60f, 45f, 80f, 45f, 48f, 48f, 55f });

                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Total ", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 9, 0);
                nQty = _oFabricBatchProductionDetails.Where(p => p.FMID == oMachine.FMID).Sum(x => x.QtyM);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, nQty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 6, 0);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            }
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.SetWidths(new float[] { 20f, 120f, 80f, 80f, 120f, 60f, 45f, 80f, 45f, 48f, 48f, 55f });
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 12, 0);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Grand Total ", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 9, 0);
            nQty = _oFabricBatchProductionDetails.Sum(x => x.QtyM);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, nQty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 6, 0);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
        }
        #endregion

        private void Signature()
        {
            float nTableHeight = CalculatePdfPTableHeight(_oPdfPTable);
            float _nfixedHight = 590 - (float)nTableHeight;
            if (_nfixedHight > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = _oPdfPTable.NumberOfColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = _nfixedHight;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 10f, 20f, 10f, 20f, 10f });
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Prepared By", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 2, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Checked By", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 2, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Approved By", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 2, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 0);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
        }
        public static float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        table.TotalWidth = 535f;
                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }

        #region Sizing 
        public byte[] PrepareReportSizing(List<FabricBatch> oFabricBatchs, List<FabricBatchProductionDetail> oFabricBatchProductionDetails, List<FabricBatchProductionBeam> oFabricBatchProductionBeams, Company oCompany, BusinessUnit oBusinessUnit, string sDateRange)
        {
            _oFabricBatchProductionDetails = oFabricBatchProductionDetails;
            _oFabricBatchs = oFabricBatchs;
            _oFabricBatchProductionBeams = oFabricBatchProductionBeams;
            _oCompany = oCompany;
            _sDateRange = sDateRange;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 800f });
            #endregion
            this.PrintHeaderSizing();
            this.PrintBodySizing();
            this.Signature();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintHeaderSizing()
        {
            #region CompanyHeader


            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 130f, 200.5f, 130f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.Rowspan = 2;
                _oPdfPCell.MinimumHeight = 25;
                oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Rowspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.UNDERLINE | iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("SIZING PRODUCTION", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Production Date " + _sDateRange, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Reporting Date " + DateTime.Now.ToString("dd MMM yyyy"), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBodySizing()
        {

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.UNDERLINE);

            PdfPTable oPdfPTable = new PdfPTable(12);

            //#region DATA
            int nSL = 1;
            double nQty = 0;
            double nWarpBeam = 0;
            string sBeamNo = "", sDIA="";
            string sBeamLength = "";
            var oMachines = _oFabricBatchProductionDetails.GroupBy(x => new { x.FMID, x.MachineName })
                                                .OrderBy(x => x.Key.FMID)
                                                .Select(x => new
                                                {
                                                    FMID = x.Key.FMID,
                                                    MachineName = x.Key.MachineName

                                                });

            var oFBs = _oFabricBatchProductionDetails.GroupBy(x => new { x.FBID, x.FMID, x.MachineName }, (key, grp) =>
                                   new
                                   {
                                       FBID = key.FBID,
                                       FMID = key.FMID,
                                       MachineName = key.MachineName,
                                   }).ToList();



            foreach (var oMachine in oMachines)
            {
                var oFabricBatchs = oFBs.Where(x => x.FMID == oMachine.FMID).ToList();

                #region Fabric Info
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.SetWidths(new float[] { 20f, 110f, 60f, 60f, 120f, 50f, 45f, 65f, 75f, 48f, 85f, 55f });
                //if (oMachines.le > 0)
                //{
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oMachine.MachineName, 0, 18, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                //}
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Program No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construction", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Ends", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "T. Beam", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Beam No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Beam Length", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Length", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dia", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Remarks", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion
                nSL = 0;
                //oFabricSizingPlans = oFabricSizingPlans.OrderBy(o => o.Sequence).ToList();
                foreach (var oItem1 in oFabricBatchs)
                {
                    var oFabricBatch = _oFabricBatchs.Where(x => x.FBID == oItem1.FBID).ToList();

                    var oFabricBatchProsDetails = _oFabricBatchProductionDetails.Where(x => x.FMID == oMachine.FMID && x.FBID == oItem1.FBID).ToList();
                    nWarpBeam = 0;
                    foreach (var oIem in oFabricBatchProsDetails)
                    {
                       nWarpBeam= _oFabricBatchProductionBeams.Where(x => x.FBID == oItem1.FBID && x.FBPDetailID == oIem.FBPDetailID).Select(c => c.BeamID).Distinct().Count();
                       sBeamNo = string.Join(",", _oFabricBatchProductionBeams.Where(x => x.FBID == oItem1.FBID && x.FBPDetailID == oIem.FBPDetailID).Select(c => c.BeamNo).Distinct().ToList());
                       nQty = _oFabricBatchProductionDetails.Where(p => p.FBID == oItem1.FBID && p.FMID == oMachine.FMID).Sum(x => x.QtyM);
                       sDIA = string.Join(",", _oFabricBatchProductionBeams.Where(x => x.FBID == oItem1.FBID && x.FBPDetailID == oIem.FBPDetailID).Select(c => c.ChildMachineTypeName + "-" + c.ParentMachineTypeName).Distinct().ToList());

                       var oBeamLength = _oFabricBatchProductionBeams.Where(x => x.FBID == oItem1.FBID && x.FBPDetailID == oIem.FBPDetailID).GroupBy(x => new { x.FBID, x.QtyM })
                                                                      .Select(x => new
                                                                      {
                                                                          BeamID = x.Count(),
                                                                          QtyM = x.Key.QtyM,
                                                                      });

                       sBeamLength = string.Join("+", oBeamLength.Select(c => c.QtyM + ((c.BeamID > 1) ?"x"+c.BeamID.ToString() : "")).Distinct().ToList());

                    }

                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.SetWidths(new float[] { 20f, 110f, 60f, 60f, 120f, 50f, 45f, 65f, 75f, 48f, 85f, 55f });

                    nSL++;
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSL.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFabricBatch.FirstOrDefault().BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFabricBatch.FirstOrDefault().FEONo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFabricBatch.FirstOrDefault().BatchNoMCode, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFabricBatch.FirstOrDefault().Construction, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFabricBatch.FirstOrDefault().TotalEnds.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, (string.IsNullOrEmpty(oFabricBatch.FirstOrDefault().WarpBeam) ? "" : oFabricBatch.FirstOrDefault().WarpBeam), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nWarpBeam) > 0 ? nWarpBeam.ToString() : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (string.IsNullOrEmpty(sBeamNo)) ? "" : sBeamNo, 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (string.IsNullOrEmpty(sBeamLength)) ? "" : sBeamLength, 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    nQty = _oFabricBatchProductionDetails.Where(p => p.FBID == oItem1.FBID && p.FMID == oMachine.FMID).Sum(x => x.QtyM);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nQty) > 0 ? nQty.ToString("#,##0.00;(#,##0.00)") : ""), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, sDIA, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }

                oPdfPTable = new PdfPTable(12);
                oPdfPTable.SetWidths(new float[] { 20f, 110f, 60f, 60f, 120f, 50f, 45f, 65f, 75f, 48f, 85f, 55f });

                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Total ", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 9, 0);
                nQty = _oFabricBatchProductionDetails.Where(p => p.FMID == oMachine.FMID).Sum(x => x.QtyM);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, nQty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 6, 0);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            }

            oPdfPTable = new PdfPTable(12);
            oPdfPTable.SetWidths(new float[] { 20f, 110f, 60f, 60f, 120f, 50f, 45f, 65f, 75f, 48f, 85f, 55f });
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, oPdfPTable.NumberOfColumns, 0);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Grand Total ", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 9, 0);
            nQty = _oFabricBatchProductionDetails.Sum(x => x.QtyM);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, nQty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 6, 0);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
        }
        #endregion

    }
}
