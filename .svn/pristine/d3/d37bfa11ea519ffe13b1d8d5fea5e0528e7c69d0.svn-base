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
    public class rptPIWiseDispoStatement
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
        
        List<FNRequisition> _oFNRequisitions = new List<FNRequisition>();
        List<FNRequisitionDetail> _oFNRequisitionDetails = new List<FNRequisitionDetail>();
        List<ExportPI> _oExportPIs = new List<ExportPI>();
        List<FabricSalesContractDetail> _oFSCDs = new List<FabricSalesContractDetail>();
        List<FNRecipe> _oFNRecipes = new List<FNRecipe>();
        FabricSalesContract _oFSC = new FabricSalesContract();
        Company _oCompany = new Company();

        #endregion

        public byte[] PrepareReport(List<ExportPI> oExportPIs, FabricSalesContract oFSC, List<FabricSalesContractDetail> oFSCDs, List<FNRequisition> oFNRequisitions, List<FNRequisitionDetail> oFNRequisitionDetails,List<FNRecipe> oFNRecipes, Company oCompany)
        {
            _oExportPIs = oExportPIs;
            _oFSC = oFSC;
            _oFSCDs = oFSCDs;
            _oFNRequisitions = oFNRequisitions;
            _oFNRequisitionDetails = oFNRequisitionDetails;
            _oFNRecipes = oFNRecipes;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
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
            this.PrintHeaderDetail("PI Wise Statement", " ", " ");
            this.BasicInfo();
            this.PIWiseData();
        }
        #endregion

        private void BasicInfo()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 15f, 10f, 30f, 10f, 30f, 5f });

            #region Basic Info

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "PI No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + string.Join(", ", _oExportPIs.Select(x=>x.PINo)), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFSC.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle); //string.Join(", ", _oExportPIs.Select(x => x.BuyerName))
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "PI Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + string.Join(", ", _oExportPIs.Select(x => x.IssueDateInString)), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construction", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + string.Join(", ", _oFSCDs.GroupBy(x => x.Construction).Select(x => x.FirstOrDefault().Construction)), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "PO No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFSC.SCNoFull, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "LC No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + string.Join(", ", _oExportPIs.Select(x => x.ExportLCNo)), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }

        private void PIWiseData()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(11);
            oPdfPTable.SetWidths(new float[] { 60f, 60f, 60f, 110f, 50f, 60f, 60f, 60f, 60f, 70f, 70f });

            double nGrandReqQty = 0, nGrandIssuQty = 0, nGrandTotalAmount = 0;
            foreach (FabricSalesContractDetail oItem in _oFSCDs)
            {
                int nFSCDID = -9999;
                double nSubReqQty = 0, nSubIssuQty = 0, nSubTotalAmount = 0;
                List<FNRequisition> oTempFNRequisitions = new List<FNRequisition>();
                oTempFNRequisitions = _oFNRequisitions.Where(x => x.FNExODetailID == oItem.FabricSalesContractDetailID).ToList();
                foreach (FNRequisition Obj in oTempFNRequisitions)
                {
                    nSubTotalAmount += _oFNRequisitionDetails.Where(x => x.FNRID == Obj.FNRID).ToList().Sum(x => x.Amount);
                }

                List<FNRecipe> oTempFNRecipes = new List<FNRecipe>();
                oTempFNRecipes = _oFNRecipes.Where(x => x.FSCDID == oItem.FabricSalesContractDetailID).ToList();
                
                oPdfPTable = new PdfPTable(10);
                oPdfPTable.SetWidths(new float[] { 60f, 60f, 60f, 110f, 50f, 60f, 60f, 60f, 60f, 70f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
                oPdfPTable = new PdfPTable(10);
                oPdfPTable.SetWidths(new float[] { 60f, 60f, 60f, 110f, 50f, 60f, 60f, 60f, 60f, 70f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No: " + oItem.ExeNo, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color: " + oItem.ColorInfo, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty(Y): " + Global.MillionFormat(oItem.Qty), 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Cost/Yds: " + Global.MillionFormat(nSubTotalAmount / oItem.Qty), 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                #region Header
                oPdfPTable = new PdfPTable(10);
                oPdfPTable.SetWidths(new float[] { 60f, 60f, 60f, 110f, 50f, 60f, 60f, 60f, 60f, 70f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req.No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Item Code", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Name Of Dyes & Chemicals", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Unit", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "G:P:L", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Issued Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Rate(BDT)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Amount", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);
                #endregion

                

                #region Data
                //oPdfPTable = new PdfPTable(10);
                //oPdfPTable.SetWidths(new float[] { 60f, 60f, 60f, 110f, 50f, 60f, 60f, 60f, 60f, 70f });
                foreach (FNRequisition oObj in oTempFNRequisitions)
                {
                    oPdfPTable = new PdfPTable(10);
                    oPdfPTable.SetWidths(new float[] { 60f, 60f, 60f, 110f, 50f, 60f, 60f, 60f, 60f, 70f });

                    List<FNRequisitionDetail> oTempFNRequisitionDetails = new List<FNRequisitionDetail>();
                    oTempFNRequisitionDetails = _oFNRequisitionDetails.Where(x => x.FNRID == oObj.FNRID).ToList();

                    foreach (FNRequisitionDetail oDetail in oTempFNRequisitionDetails)
                    {
                        oDetail.RequiredQty = 0;
                        oDetail.LotBalance = 0;
                    }

                    foreach (FNRecipe oFNR in oTempFNRecipes)
                    {
                        bool isExist = false;
                        foreach (FNRequisitionDetail oDetail in oTempFNRequisitionDetails)
                        {
                            if (oDetail.ProductID == oFNR.ProductID)
                            {
                                oDetail.RequiredQty = oFNR.Qty;
                                oDetail.LotBalance = oFNR.GL;
                                isExist = true;
                                break;
                            }
                        }
                        if (!isExist)
                        {
                            FNRequisitionDetail oFNRDetail = new FNRequisitionDetail();
                            oFNRDetail.ProductCode = oFNR.ProductCode;
                            oFNRDetail.ProductName = oFNR.ProductName;
                            oFNRDetail.RequiredQty = oFNR.Qty;
                            oFNRDetail.LotBalance = oFNR.GL;
                            oTempFNRequisitionDetails.Add(oFNRDetail);
                        }
                    }

                    int nFNRID = -999;
                    foreach (FNRequisitionDetail oDetail in oTempFNRequisitionDetails)
                    {
                        if (oObj.FNRID != nFNRID)
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oObj.RequestDateInString, oTempFNRequisitionDetails.Count(), 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oObj.FNRNo, oTempFNRequisitionDetails.Count(), 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        }
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oDetail.ProductCode, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oDetail.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oDetail.MUName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oDetail.LotBalance), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oDetail.RequiredQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        nSubReqQty += oDetail.RequiredQty;
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oDetail.DisburseQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        nSubIssuQty += oDetail.DisburseQty;
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oDetail.Rate), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oDetail.Amount), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        
                        nFNRID = oObj.FNRID;

                    }
                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);
                }

                //oPdfPTable.CompleteRow();
                //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);
                #endregion

                #region Sub Total Info
                oPdfPTable = new PdfPTable(10);
                oPdfPTable.SetWidths(new float[] { 60f, 60f, 60f, 110f, 50f, 60f, 60f, 60f, 60f, 70f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sub Total", 0, 6, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubReqQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubIssuQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubTotalAmount), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                nGrandTotalAmount += nSubTotalAmount;
                nGrandIssuQty += nSubIssuQty;
                nGrandReqQty += nSubReqQty;

            }

            #region Grand Total Info
            oPdfPTable = new PdfPTable(10);
            oPdfPTable.SetWidths(new float[] { 60f, 60f, 60f, 110f, 50f, 60f, 60f, 60f, 60f, 70f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Grand Total", 0, 6, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nGrandReqQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nGrandIssuQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nGrandTotalAmount), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

        }

    }
}
