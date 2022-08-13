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

    public class rptStockLedger
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        FNBatchQC _oFNBatchQC = new FNBatchQC();
        List<FNBatchQCDetail> _oFNBatchQCDetails = new List<FNBatchQCDetail>();
        FabricSCReport _oFabricSCReport = new FabricSCReport();
        List<FDCRegister> _oFDCRegisters = new List<FDCRegister>();
        List<FabricReturnChallanDetail> _oFabricReturnChallanDetails = new List<FabricReturnChallanDetail>();
        #endregion
        public byte[] PrepareReport( List<FNBatchQCDetail> oFNBatchQCDetails, FabricSCReport oFabricSCReport, List<FDCRegister> oFDCRegister, List<FabricReturnChallanDetail> oFabricReturnChallanDetails, Company oCompany, BusinessUnit oBusinessUnit)
        {
        
            _oFNBatchQCDetails = oFNBatchQCDetails;
            _oFabricSCReport = oFabricSCReport;
            _oFDCRegisters = oFDCRegister;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oFabricReturnChallanDetails = oFabricReturnChallanDetails;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(30f, 30f, 10f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595});
            #endregion

            this.PrintEmptyRow("", 25);
            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, _oCompany, "Finished Fabric Stock Ledger", 1);
            this.PrintEmptyRow("", 20);
            this.PrintObjectTable();
            
            this.PrintEmptyRow("", 20);
            this.FinishedFabric();

            this.PrintEmptyRow("", 20);
            this.FabricDeliveryChallanDetail();


            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        public void PrintObjectTable()
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;
            oPdfPTable.SetWidths(new float[] { 125, 140, 80, 50, 100, 120 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            #region 1st Row
            _oPdfPCell = new PdfPCell(new Phrase("Garments Name", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFabricSCReport.ContractorName, _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFabricSCReport.MUName, _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region 2nd Row
            _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFabricSCReport.BuyerName, _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

           
            _oPdfPCell = new PdfPCell(new Phrase("PO No", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFabricSCReport.SCNoFull, _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region 3rd Row
            _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFabricSCReport.ExeNo, _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyleBold));
            //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oFabricSCReport.Construction, _oFontStyle));
            //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PO Qty", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Math.Round(_oFabricSCReport.Qty, 2).ToString(), _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region 3rd Row
            _oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFabricSCReport.Construction, _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion



            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
    
        public void FinishedFabric()
        {
            PdfPTable oPdfPTable = new PdfPTable(5);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 110, 80, 80, 80, 80 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

         

                _oPdfPCell = new PdfPCell(new Phrase("Finished Fabric Received", _oFontStyleBold)); _oPdfPCell.Colspan = 5;
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("Good", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Hold", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Reject", _oFontStyleBold)); 
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("C. Total", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("Inter-Transfer", _oFontStyleBold)); _oPdfPCell.Colspan = 2;
                //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("Return", _oFontStyleBold)); _oPdfPCell.Rowspan = 2;
                //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("G. Total", _oFontStyleBold)); _oPdfPCell.Rowspan = 2;
                //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                //_oPdfPCell = new PdfPCell(new Phrase("Transform", _oFontStyleBold));
                //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("Only", _oFontStyleBold));
                //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
       
            int nCount = 1;
            double dCTotal = 0;
            double dGood = 0;
            double dHold = 0;
            double dReject = 0;
            string sDate = "";
            bool isFirst = false;
            PdfPTable oCPdfPTable = new PdfPTable(10);
            _oFNBatchQCDetails.ForEach(o => o.StoreRcvDate = o.StoreRcvDate.Date);
            _oFNBatchQCDetails = _oFNBatchQCDetails.GroupBy(x => new { x.Grade, x.StoreRcvDate }, (key, grp) =>
                                            new FNBatchQCDetail
                                            {
                                                Grade = key.Grade,
                                                StoreRcvDate = key.StoreRcvDate,
                                                Qty = grp.Sum(p => p.Qty),
                                            }).ToList();
            foreach (FNBatchQCDetail oItem in _oFNBatchQCDetails)
            {
                    oCPdfPTable = new PdfPTable(5);
                    oCPdfPTable.WidthPercentage = 100;
                    oCPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oCPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    oCPdfPTable.SetWidths(new float[] {110, 80, 80, 80, 80 });

                    _oPdfPCell = new PdfPCell(new Phrase((!string.IsNullOrEmpty(oItem.StoreRcvDateStr) ? oItem.StoreRcvDateStr : "Waiting for received"), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);
                    dGood = 0; dReject = 0; dHold = 0;
                    if (!string.IsNullOrEmpty(oItem.StoreRcvDateStr))
                    {
                        if (oItem.Grade != EnumFBQCGrade.Reject) { dGood = oItem.Qty; }
                        else { dReject = oItem.Qty; }
                    }
                    else
                    {
                        dHold = oItem.Qty;
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(((dGood <= 0) ? "" : Global.MillionFormat(dGood)), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(((dHold <= 0) ? "" : Global.MillionFormat(dHold)), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(((dReject<=0)?"":Global.MillionFormat(dReject)), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    dCTotal = dGood + dReject + dHold + dCTotal;
                    _oPdfPCell = new PdfPCell(new Phrase(((dCTotal<=0)?"":Global.MillionFormat(dCTotal)), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    oCPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(oCPdfPTable);
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                }
        
        }
        public void FabricDeliveryChallanDetail()
        {
            PdfPTable oPdfPTable = new PdfPTable(5);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 100, 80, 80, 80, 80 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);


            _oPdfPCell = new PdfPCell(new Phrase("Finished Fabric Delivery", _oFontStyleBold)); _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

        
            _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        
            _oPdfPCell = new PdfPCell(new Phrase("Del. Qty", _oFontStyleBold)); 
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Return Qty", _oFontStyleBold)); 
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold)); 
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            int nCount = 1;
            double dCTotal = 0;

            // Add Return Challan with Challan List

            var oFabricReturnChallanDetails = _oFabricReturnChallanDetails.GroupBy(x => new { x.ChallanNo, x.ReturnDate }, (key, grp) =>
                                         new
                                         {
                                             ChallanNo = key.ChallanNo,
                                             ChallanDate = key.ReturnDate,
                                             IsReturn = true,
                                             Printlayout=0,
                                             Qty = grp.Sum(p => p.Qty),
                                         }).ToList();
            FDCRegister oFDCRegister = new FDCRegister();
            foreach (var oItem in oFabricReturnChallanDetails)
            {
                oFDCRegister = new FDCRegister();
                oFDCRegister.ChallanNo="RC:"+oItem.ChallanNo;
                  oFDCRegister.ChallanDate=oItem.ChallanDate;
                 oFDCRegister.Qty = oItem.Qty;
                 oFDCRegister.Printlayout = 1;
               _oFDCRegisters.Add(oFDCRegister);
            }
            /////

            _oFDCRegisters.ForEach(o => o.ChallanDate = o.ChallanDate.Date);
            _oFDCRegisters = _oFDCRegisters.GroupBy(x => new { x.ChallanNo, x.ChallanDate, x.Printlayout }, (key, grp) =>
                                            new FDCRegister()
                                            {
                                                ChallanNo = key.ChallanNo,
                                                ChallanDate = key.ChallanDate,
                                                Qty = grp.Sum(p => p.Qty),
                                                Printlayout = key.Printlayout,
                                            }).ToList();


            _oFDCRegisters.ForEach(o => o.ChallanDate = o.ChallanDate.Date);

           foreach (FDCRegister oItem in _oFDCRegisters)
            {
                PdfPTable oCPdfPTable = new PdfPTable(5);
                oCPdfPTable.WidthPercentage = 100;
                oCPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oCPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                oCPdfPTable.SetWidths(new float[] { 100, 80, 80, 80, 80 });


                _oPdfPCell = new PdfPCell(new Phrase(oItem.ChallanDateSt, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

               
                _oPdfPCell = new PdfPCell(new Phrase(oItem.ChallanNo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                if (oItem.Printlayout == 0)
                {

                    dCTotal = dCTotal + oItem.Qty;
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    dCTotal = dCTotal - oItem.Qty;

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

 
                }

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(dCTotal), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                
                _oPdfPCell = new PdfPCell(oCPdfPTable);
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();


            }
        }
        public void PrintEmptyRow(string sString, int nHeight)
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 595f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL);

            #region Row
            _oPdfPCell = new PdfPCell(new Phrase(sString, _oFontStyle)); _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
    }
}

