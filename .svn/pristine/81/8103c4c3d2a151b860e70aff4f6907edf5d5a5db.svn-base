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
    public class rptDUProGuideLineReportV2
    {
        #region Declaration
        int _nTotalColumn = 1;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        DyeingOrder _oDyeingOrder = new DyeingOrder();
        DUProGuideLine _oDUProGuideLine = new DUProGuideLine();
        List<LotParent> LotParents = new List<LotParent>();
        List<DyeingOrderDetail> _oDyeingOrderDetails = new List<DyeingOrderDetail>();
        List<DUProGuideLineDetail> _oDUProGuideLineDetails_All = new List<DUProGuideLineDetail>();
        List<DURequisitionDetail> DURequisitionDetails_SRM = new List<DURequisitionDetail>();
        List<LotParent> _oLotParents_In = new List<LotParent>();
        List<LotParent> _oLotParents_Out = new List<LotParent>();
        List<DURequisitionDetail> DURequisitionDetails_SRS = new List<DURequisitionDetail>();
        List<DUProGuideLineDetail> DUProGuideLineDetails_Return = new List<DUProGuideLineDetail>();
        Company _oCompany = new Company();

        #endregion

        public byte[] PrepareReport(DyeingOrder oDyeingOrder, DUProGuideLine oDUProGuideLine, List<DUProGuideLineDetail> oDUProGuideLineDetails, List<LotParent> oLotParents, List<DyeingOrderDetail> oDyeingOrderDetails, List<DUProGuideLineDetail> oDUProGuideLineDetails_Return, List<DURequisitionDetail> oDURequisitionDetails_SRS, List<DURequisitionDetail> oDURequisitionDetails_SRM, Company oCompany)
        {
            _oDyeingOrder = oDyeingOrder;
            _oDUProGuideLine = oDUProGuideLine;
            LotParents = oLotParents;
            _oDyeingOrderDetails = oDyeingOrderDetails;
            DUProGuideLineDetails_Return = oDUProGuideLineDetails_Return;
            DURequisitionDetails_SRS = oDURequisitionDetails_SRS;
            DURequisitionDetails_SRM = oDURequisitionDetails_SRM;
            _oDUProGuideLineDetails_All = oDUProGuideLineDetails;
            _oLotParents_In = LotParents.Where(x => x.DyeingOrderID == oDUProGuideLine.DyeingOrderID).ToList();
            _oLotParents_Out = LotParents.Where(x => x.DyeingOrderID_Out == oDUProGuideLine.DyeingOrderID).ToList();
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(25f, 25f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });
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


            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(70f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        private void PrintHeaderDetail(string sReportHead, string sOrderType, string sDate)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 170f, 200f, 170f });
            #region
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sReportHead, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sDate, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sOrderType, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLDITALIC));
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reporting Date:" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            this.PrintHeaderDetail("Order Statement", " ", " ");
            this.SetData();
        }
        #endregion

        private void SetData()
        {
            this.PrintBasicInfo();
            this.PrintSummary();
            this.TransferInDetails();
            this.TransferOutDetails();
            this.ReturnDetails();
            this.SRSDetails();
            this.SRMDetails();
        }

        private void PrintBasicInfo()
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            #region Header
            PdfPTable oPdfHeader_Table = new PdfPTable(4);
            oPdfHeader_Table.SetWidths(new float[] { 100, 150, 100, 145 });
            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, "Order No", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, _oDyeingOrder.OrderNoFull, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, "Order Date", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, _oDyeingOrder.OrderDateSt, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, (_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.LoanOrder) ? "Supplier Name" : "Buyer Name", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, _oDyeingOrder.ContractorName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 3, 10);
            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, "MKT Person", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, _oDyeingOrder.MKTPName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 3, 10);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfHeader_Table);

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, 15);
            #endregion

        }


        private void TransferInDetails()
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Transfer In Details", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, 15, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "#SL", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Lot No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Product", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Entry Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            int nSLNo = 0;
            _oLotParents_In = _oLotParents_In.Where(x => x.DyeingOrderID_Out != 0).ToList();
            foreach (var oItem in _oLotParents_In)
            {
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSLNo).ToString("00"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.DyeingOrderNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.EntryDate.ToString("dd MMM yyyy hh:mm tt"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
            if (_oLotParents_In.Count > 0)
            {
                #region Total Info
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 4, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_oLotParents_In.Sum(x => x.Qty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion
            }
            
        }

        private void TransferOutDetails()
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Transfer Out Details", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, 15, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "#SL", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Lot No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Product", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Entry Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            int nSLNo = 0;
            foreach (var oItem in _oLotParents_Out)
            {
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSLNo).ToString("00"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.DyeingOrderNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.EntryDate.ToString("dd MMM yyyy hh:mm tt"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
            if (_oLotParents_Out.Count > 0)
            {
                #region Total Info
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 4, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_oLotParents_Out.Sum(x => x.Qty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion
            }
            
        }

        private void ReturnDetails()
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Return Details", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, 15, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "#SL", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Lot No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Product", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Rcv. Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            int nSLNo = 0;
            foreach (var oItem in DUProGuideLineDetails_Return)
            {
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSLNo).ToString("00"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.SLNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ReceiveDate.ToString("dd MMM yyyy hh:mm tt"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
            if (DUProGuideLineDetails_Return.Count > 0)
            {
                #region Total Info
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 4, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(DUProGuideLineDetails_Return.Sum(x => x.Qty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion
            }
            
        }

        private void SRSDetails()
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SRS Details", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, 15, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "#SL", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Lot No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Product", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Rcv. Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            int nSLNo = 0;
            foreach (var oItem in DURequisitionDetails_SRS)
            {
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSLNo).ToString("00"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.RequisitionNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ReceiveDate.ToString("dd MMM yyyy hh:mm tt"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
            if (DURequisitionDetails_SRS.Count > 0)
            {
                #region Total Info
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 4, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(DURequisitionDetails_SRS.Sum(x => x.Qty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion
            }
            
        }

        private void SRMDetails()
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SRM Details", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, 15, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "#SL", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Lot No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Product", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Rcv. Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            int nSLNo = 0;
            foreach (var oItem in DURequisitionDetails_SRM)
            {
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSLNo).ToString("00"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.RequisitionNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ReceiveDate.ToString("dd MMM yyyy hh:mm tt"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
            if (DURequisitionDetails_SRM.Count > 0)
            {
                #region Total Info
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 10f, 20f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 4, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(DURequisitionDetails_SRM.Sum(x => x.Qty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion
            }
            
        }

        private void PrintSummary()
        {
            if (_oDUProGuideLine.DyeingOrderID != 0)
            {
                //Count, OQ, GQ, SRM-Q, T-IN,  
                _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                #region Summary Print V2
                PdfPTable oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.SetWidths(new float[] { 25.5f, 125.5f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f }); //130

                #region Header

                #region 1st Row
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.SetWidths(new float[] { 25.5f, 125.5f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Received Status", 0, 4, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Issued Status", 0, 4, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                #endregion

                #region 2nd Row
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.SetWidths(new float[] { 25.5f, 125.5f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "#SL", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Commodity", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Lot", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "GRN Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SRM Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Transfer In", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Received Due", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Issued To S/W", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Transfer Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Return Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Balance", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                #endregion

                #endregion

                LotParents = LotParents.GroupBy(x => new { x.LotID, x.ProductID }).Select(y => y.FirstOrDefault()).ToList();
                var data = LotParents.GroupBy(x => new { x.ProductID }, (key, grp) => new
                {
                    ProductID = key.ProductID,
                    //LotID = key.LotID,
                    Results = grp.ToList().OrderBy(y=>y.ProductID)
                });
                double nTotalQty_GRN = 0, nTotalQty_SRM = 0, nTotalQty_T_In = 0, nTotalQty_T_Out = 0, nTotalQty_SW = 0, nTotalQty_Return = 0, nTotalLot_Balance_Store = 0, nTotalQty_Order = 0, nTotalBalance_DUE = 0;
                foreach (var oData in data)
                {
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.SetWidths(new float[] { 25.5f, 125.5f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f }); //130

                    int nSLNo = 0; int nProductID = -999;
                    double nSubQty_GRN = 0, nSubQty_SRM = 0, nSubQty_T_In = 0, nSubQty_T_Out = 0, nSubQty_SW = 0, nSubQty_Return = 0, nSubLot_Balance_Store = 0, nSubQty_Order = 0, nSubBalance_DUE = 0;

                    foreach (var oItem in oData.Results)
                    {
                        
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSLNo).ToString("00"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        if (oData.ProductID != nProductID)
                        {
                            double nQty_Order = _oDyeingOrderDetails.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty);
                            nSubQty_Order += nQty_Order;
                            nTotalQty_Order += nQty_Order;
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQty_Order), oData.Results.Count(), 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        }
                        double nQty_GRN = _oDUProGuideLineDetails_All.Where(x => x.ProductID == oItem.ProductID && x.LotID == oItem.LotID).Sum(x => x.Qty);
                        nSubQty_GRN += nQty_GRN; nTotalQty_GRN += nQty_GRN;
                        double nQty_SRM = DURequisitionDetails_SRM.Where(x => x.ProductID == oItem.ProductID && x.DestinationLotID == oItem.LotID).Sum(x => x.Qty);
                        nSubQty_SRM += nQty_SRM; nTotalQty_SRM += nQty_SRM;
                        double nQty_T_In = _oLotParents_In.Where(x => x.ProductID == oItem.ProductID && x.DyeingOrderID_Out != 0 && x.LotID == oItem.LotID).Sum(x => x.Qty);
                        nSubQty_T_In += nQty_T_In; nTotalQty_T_In += nQty_T_In;
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQty_GRN), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQty_SRM), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQty_T_In), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        if (oData.ProductID != nProductID)
                        {
                            double nQty_Order_Product = _oDyeingOrderDetails.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty);
                            double nQty_Rcv = _oDUProGuideLineDetails_All.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty); //&& x.LotID == oLotParent.LotID
                            double nQty_Rcv_In = _oLotParents_In.Where(x => x.ProductID == oItem.ProductID && x.DyeingOrderID_Out != 0).Sum(x => x.Qty); //&& x.LotID == oLotParent.LotID
                            double nQty_Rcv_Out = _oLotParents_Out.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty); //&& x.LotID == oLotParent.LotID
                            double nQty_Party_Return = DUProGuideLineDetails_Return.Where(x => x.ProductID == oItem.ProductID && x.LotID == oItem.LotID).Sum(x => x.Qty);
                            double nQty_RSRS = DURequisitionDetails_SRS.Where(x => x.ProductID == oItem.ProductID && x.LotID == oItem.LotID).Sum(x => x.Qty);
                            double nQty_RSRM = DURequisitionDetails_SRM.Where(x => x.ProductID == oItem.ProductID && x.DestinationLotID == oItem.LotID).Sum(x => x.Qty);

                            double nBalance_DUE = nQty_Order_Product - nQty_Rcv - nQty_Rcv_In;
                            nBalance_DUE = nBalance_DUE < 0 ? 0 : nBalance_DUE;
                            nSubBalance_DUE += nBalance_DUE; nTotalBalance_DUE += nBalance_DUE;
                            //_nQTy_Total_DUE += nBalance_DUE;
                            //nBalance_Store = nQty_Rcv + nQty_RSRM + nQty_Rcv_In - nQty_RSRS - nQty_Rcv_Out - nQty_Party_Return;
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nBalance_DUE), oData.Results.Count(), 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        }
                        double nQty_T_Out = _oLotParents_Out.Where(x => x.ProductID == oItem.ProductID && x.LotID == oItem.LotID).Sum(x => x.Qty);
                        nSubQty_T_Out += nQty_T_Out; nTotalQty_T_Out += nQty_T_Out;
                        double nQty_SW = DURequisitionDetails_SRS.Where(x => x.ProductID == oItem.ProductID && x.LotID == oItem.LotID).Sum(x => x.Qty);
                        nSubQty_SW += nQty_SW; nTotalQty_SW += nQty_SW;
                        double nQty_Return = DUProGuideLineDetails_Return.Where(x => x.ProductID == oItem.ProductID && x.LotID == oItem.LotID).Sum(x => x.Qty);
                        nSubQty_Return += nQty_Return; nTotalQty_Return += nQty_Return;
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQty_SW), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQty_T_Out), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQty_Return), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        
                        var nLot_Balance_Store = nQty_GRN + nQty_SRM + nQty_T_In - nQty_SW - nQty_T_Out - nQty_Return;
                        nSubLot_Balance_Store += nLot_Balance_Store; nTotalLot_Balance_Store += nLot_Balance_Store;
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nLot_Balance_Store), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                        nProductID = oData.ProductID;
                    }
                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                    #region SubTotal
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.SetWidths(new float[] { 25.5f, 125.5f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f }); //130
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sub Total", 0, 3, Element.ALIGN_LEFT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubQty_Order), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubQty_GRN), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubQty_SRM), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubQty_T_In), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubBalance_DUE), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubQty_SW), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubQty_T_Out), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);                    
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubQty_Return), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubLot_Balance_Store), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                    #endregion

                }

                #region Grand Total
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.SetWidths(new float[] { 25.5f, 125.5f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f }); //130
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Grand Total", 0, 3, Element.ALIGN_LEFT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotalQty_Order), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotalQty_GRN), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotalQty_SRM), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotalQty_T_In), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotalBalance_DUE), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotalQty_SW), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotalQty_T_Out), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotalQty_Return), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotalLot_Balance_Store), 0, 0, Element.ALIGN_RIGHT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                
                #endregion
            }
        }
        
    }
}
