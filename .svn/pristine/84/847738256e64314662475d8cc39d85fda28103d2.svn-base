using System;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace ESimSol.Reports
{

    public class rptRouteSheetOld
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(5);
        int _nColumn = 5;
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        RouteSheet _oRouteSheet = new RouteSheet();
        RouteSheetDetail _oRSDetail = new RouteSheetDetail();
        List<RouteSheetDetail> _oRSDetails = new List<RouteSheetDetail>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        string _sRecipeWrittenBy, _sRecipeEditedBy, _sCombineRSNo = "";
        int _nNoOfPrint = 0;
        #endregion

        public byte[] PrepareReport(RouteSheet oRouteSheet, List<RouteSheetDetail> oRSDetails, Company oCompany, BusinessUnit oBusinessUnit, string sRecipeWrittenBy, string sRecipeEditedBy, string sCombineRSNo, bool bIsCommon)
        {
            _oRouteSheet = oRouteSheet;
            _oRSDetail = oRouteSheet.RouteSheetDetail;
            _oRSDetails = oRSDetails;
            _oBusinessUnit = oBusinessUnit;

            _sRecipeWrittenBy = sRecipeWrittenBy;
            _sRecipeEditedBy = sRecipeEditedBy;
            _sCombineRSNo = sCombineRSNo;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(5f, 5f, 5f, 5f);
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 125f, 95f, 80f, 70f, 80f });
            #endregion

            this.PrintHeader();
            this.PrintBody(bIsCommon);

            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header && Footer
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
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        #endregion


        private void PrintBody(bool bIsCommon)
        {

            if (bIsCommon)
            {
                #region RouteSheet

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                if (_sCombineRSNo == "")
                {
                    if (_nNoOfPrint > 1)
                    {
                        _oPdfPTable.AddCell(this.SetCellValue("Copy - " + _nNoOfPrint, 0, 1, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 0));

                    }
                    else
                    {
                        _oPdfPTable.AddCell(this.SetCellValue("", 0, 1, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 0));
                    }
                    _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
                    _oPdfPTable.AddCell(this.SetCellValue("DYEING CARD", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                    _oPdfPTable.AddCell(this.SetCellValue("DL No:" + _oRouteSheet.RouteSheetNo, 0, 1, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 0));
                    _oPdfPTable.CompleteRow();

                }
                else
                {
                    //_oPdfPTable.AddCell(this.SetCellValue("Combine Dyeline No: " + _sCombineRSNo, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 0));
                    //_oPdfPTable.AddCell(this.SetCellValue("DL No: " + _oRouteSheet.RouteSheetNo, 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 0));

                    //_oPdfPTable.CompleteRow();
                    _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
                    _oPdfPTable.AddCell(this.SetCellValue("", 0, 1, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
                    _oPdfPTable.AddCell(this.SetCellValue("DYEING CARD", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                    _oPdfPTable.AddCell(this.SetCellValue("LD No" + _oRouteSheet.RouteSheetNo, 0, 1, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 0));
                    _oPdfPTable.CompleteRow();
                }



                //row1

                _oPdfPTable.AddCell(this.SetCellValue("Lot No", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue(_oRouteSheet.LotNo, 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("Buyer", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue(_oRouteSheet.PTU.ContractorName, 0, 2, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.CompleteRow();

                //row2

                _oPdfPTable.AddCell(this.SetCellValue("Color", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue(_oRouteSheet.PTU.ColorNameShade, 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue(" Order", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue((_sCombineRSNo.Trim() == "") ? _oRouteSheet.PTU.OrderNo : _oRouteSheet.PTU.OrderNo, 0, 2, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.CompleteRow();

                //row3

                _oPdfPTable.AddCell(this.SetCellValue("Yarn Type & Count", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue(_oRouteSheet.PTU.ProductName, 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("Buyer Ref", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.CompleteRow();

                //row4

                _oPdfPTable.AddCell(this.SetCellValue("LD No", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue(_oRouteSheet.PTU.LabdipNo, 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("Plan Date", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue(_oRouteSheet.RouteSheetDateStr, 0, 2, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.CompleteRow();

                //row5

                _oPdfPTable.AddCell(this.SetCellValue("No Of Packages", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("Total Weight(LBS)", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_oRouteSheet.Qty) + ", KG" + Global.MillionFormat(_oRouteSheet.QtyKg), 0, 2, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.CompleteRow();

                //row6

                _oPdfPTable.AddCell(this.SetCellValue("Machine No", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue(_oRouteSheet.MachineName, 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("Batch No", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.CompleteRow();

                //row7

                _oPdfPTable.AddCell(this.SetCellValue("Shade Batch No", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("Total Liquor(l)", 0, 0, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_oRouteSheet.TtlLiquire), 0, 2, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
                _oPdfPTable.CompleteRow();

                #endregion

                this.MiddlePart();

                this.PrintNote();
                this.PrintAuthorization();
                this.PrintFooter();
            }
            else
            {

                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 1, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
                _oPdfPTable.AddCell(this.SetCellValue("DYEING CARD", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                _oPdfPTable.AddCell(this.SetCellValue(_oRouteSheet.RouteSheetNo, 0, 1, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 0));
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

                //row1
                _oPdfPTable.AddCell(this.SetCellValue("R/W Lot : " + _oRouteSheet.LotNo, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("Lab Dip No : " + _oRouteSheet.PTU.LabdipNo + "  Color No:" + _oRouteSheet.PTU.ColorNo + " (" + ((EnumShade)_oRouteSheet.PTU.Shade).ToString() + ")", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 0));
                _oPdfPTable.CompleteRow();

                //row2

                _oPdfPTable.AddCell(this.SetCellValue("Date : " + _oRouteSheet.RouteSheetDateStr, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("Machine No : " + _oRouteSheet.MachineName, 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 0));
                _oPdfPTable.CompleteRow();

                //row3

                _oPdfPTable.AddCell(this.SetCellValue("Buyer : " + _oRouteSheet.PTU.ContractorName, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("DO/SO # : " + _oRouteSheet.PTU.OrderNo, 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 0));
                _oPdfPTable.CompleteRow();

                //row4

                _oPdfPTable.AddCell(this.SetCellValue("Color : " + _oRouteSheet.PTU.ColorName, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("Remarks : " + _oRouteSheet.Note, 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 0));
                _oPdfPTable.CompleteRow();

                //row5

                _oPdfPTable.AddCell(this.SetCellValue("Yarn : " + _oRouteSheet.PTU.ProductName, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("Weight : " + Global.MillionFormat(_oRouteSheet.Qty) + " LBS      KG:" + Global.MillionFormat(Global.GetKG(_oRouteSheet.Qty, 2)), 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 0));
                _oPdfPTable.CompleteRow();


                this.MiddlePart();


                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

                _oPdfPTable.AddCell(this.SetCellValue("", 0, _nColumn, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 10f));
                _oPdfPTable.CompleteRow();

                _oPdfPTable.AddCell(this.SetCellValue("Lab Sample", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("Lot Sample", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 0));
                _oPdfPTable.AddCell(this.SetCellValue("Written By", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 0));
                _oPdfPTable.CompleteRow();


                _oPdfPTable.AddCell(this.SetCellValue("", 7, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 84f));
                _oPdfPTable.AddCell(this.SetCellValue("", 7, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 84f));
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 12f));
                _oPdfPTable.CompleteRow();

                _oPdfPTable.AddCell(this.SetCellValue("Operator", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 12f));
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 12f));
                _oPdfPTable.AddCell(this.SetCellValue("Check", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 12f));
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 12f));
                _oPdfPTable.AddCell(this.SetCellValue("Dyeing Master", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 12f));
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 12f));
            }



        }

        private void MiddlePart()
        {
            #region RouteSheetDetail

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            _oPdfPTable.AddCell(this.SetCellValue("Item Name", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, 1, 14f));
            _oPdfPTable.AddCell(this.SetCellValue("Amount(g/l)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, 1, 14f));
            _oPdfPTable.AddCell(this.SetCellValue("Amount(%)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, 1, 14f));
            _oPdfPTable.AddCell(this.SetCellValue("Total Amount", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, 1, 14f));
            _oPdfPTable.AddCell(this.SetCellValue("Addition-1(gm)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, 1, 14f));
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            foreach (RouteSheetDetail oItem in _oRSDetail.children)
            {
                int nSapn = 1;
                if (oItem.children.Count() > 0)
                {
                    nSapn = this.DepthChecker(oItem.children);
                    RecursiveChecking(oItem, ref nSapn);
                }
                else
                {
                    SetInfoToGrid(oItem, ref nSapn, "");
                }
            }

            #endregion RouteSheetDetail

            #region Summary

            int[] ParentIDs = _oRSDetails.Where(x => x.ProcessName.ToUpper().Trim() == "COTTON DYEING").Select(x => x.RouteSheetDetailID).ToArray();
            _oRSDetails = _oRSDetails.Where(x => ParentIDs.Contains(x.ParentID) && x.ProductCategoryName == "Dyes").ToList();

            var nPercentage = _oRSDetails.Sum(x => (x.Percentage + ((x.Percentage * x.DeriveGL) / 100)));
            var nAdditionalPercentage = _oRSDetails.Sum(x => x.AddOnePercentage + x.AddTwoPercentage + x.AddThreePercentage);
            var nGTPercentage = nPercentage + nAdditionalPercentage;

            var nTotal = _oRSDetails.Sum(x => (x.Percentage + ((x.Percentage * x.DeriveGL) / 100) + x.AddOnePercentage + x.AddTwoPercentage + x.AddThreePercentage));


            _oPdfPTable.AddCell(this.SetCellValue("Net: " + Global.MillionFormat(nPercentage) + "%", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 0));
            _oPdfPTable.AddCell(this.SetCellValue("Additional: " + Global.MillionFormat(nAdditionalPercentage) + "%", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 0));
            _oPdfPTable.AddCell(this.SetCellValue("Total: " + Global.MillionFormat(nGTPercentage) + "%", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 0));
            _oPdfPTable.CompleteRow();
            #endregion
        }

        int DepthChecker(List<RouteSheetDetail> oRSDetails)
        {
            int nSpan = 0;

            foreach (RouteSheetDetail oItem in oRSDetails)
            {
                if (oItem.children.Count() > 0)
                    break;
                else
                    nSpan++;
            }
            return (nSpan == 0) ? 1 : nSpan;
        }

        void RecursiveChecking(RouteSheetDetail oRSDetail, ref int nSpan)
        {
            if (oRSDetail.children.Count() > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                var bIsCottonDyeing = (oRSDetail.ProcessName.Trim().ToUpper() == "COTTON DYEING") ? true : false;

                var childen = oRSDetail.children.Where(x => x.Percentage > 0).ToList();
                childen.AddRange(oRSDetail.children.Where(x => x.Percentage <= 0).ToList());
                oRSDetail.children = childen;

                _oPdfPTable.AddCell(this.SetCellValue(oRSDetail.ProcessName, 0, 2, Element.ALIGN_LEFT, BaseColor.LIGHT_GRAY, 1, 14f));

                _oPdfPTable.AddCell(this.SetCellValue(((bIsCottonDyeing) ? oRSDetail.TempTime.Trim() : oRSDetail.Note.Trim()), 0, 3, Element.ALIGN_LEFT, BaseColor.LIGHT_GRAY, 1, 14f));
                _oPdfPTable.CompleteRow();

                foreach (RouteSheetDetail oItem in oRSDetail.children)
                {
                    nSpan = (bIsCottonDyeing) ? 1 : nSpan;
                    if (oItem.children.Count() > 0)
                    {
                        nSpan = this.DepthChecker(oItem.children);
                        this.RecursiveChecking(oItem, ref nSpan);
                    }
                    else
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                        SetInfoToGrid(oItem, ref nSpan, (bIsCottonDyeing) ? "" : oRSDetail.TempTime);
                    }
                }
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                SetInfoToGrid(oRSDetail, ref nSpan, "");
            }
        }

        void SetInfoToGrid(RouteSheetDetail oRSDetail, ref int nSapn, string sTempTime)
        {
            _oPdfPTable.AddCell(this.SetCellValue(oRSDetail.ProcessName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 14f));

            _oPdfPTable.AddCell(this.SetCellValue((oRSDetail.DeriveGL == 0) ? "" : Global.MillionFormatActualDigit(oRSDetail.DeriveGL), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f));

            _oPdfPTable.AddCell(this.SetCellValue((oRSDetail.Percentage == 0) ? "" : Global.MillionFormatActualDigit(oRSDetail.Percentage), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f));

            _oPdfPTable.AddCell(this.SetCellValue(oRSDetail.TotalQtyStr, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f));

            if (nSapn > 0)
            {
                _oPdfPTable.AddCell(this.SetCellValue(sTempTime, nSapn, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f));
                nSapn = 0;
            }
            _oPdfPTable.CompleteRow();
        }

        private void PrintNote()
        {
            _oPdfPTable.AddCell(this.SetCellValue("", 0, 5, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 14f));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("Macth: " + _oRouteSheet.Note, 0, 5, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 14f));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("", 0, 5, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 14f));
            _oPdfPTable.CompleteRow();
        }

        private void PrintAuthorization()
        {
            PdfPTable oTempTable = new PdfPTable(2);
            oTempTable.SetWidths(new float[] { 60f, 65f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            oTempTable.AddCell(this.SetCellValue("Recipe written by ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 20f));
            oTempTable.AddCell(this.SetCellValue(_sRecipeWrittenBy, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 20f));
            oTempTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oTempTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 14f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.AddCell(this.SetCellValue("Dyebath Dropped By", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 14f));
            _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 14f));
            _oPdfPTable.AddCell(this.SetCellValue("Dyes Weighted By", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 14f));
            _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 14f));
            _oPdfPTable.CompleteRow();


            oTempTable = new PdfPTable(2);
            oTempTable.SetWidths(new float[] { 60f, 65f });

            oTempTable.AddCell(this.SetCellValue("Start Time", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f));
            oTempTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f));
            oTempTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oTempTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.AddCell(this.SetCellValue("End Time", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f));
            _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f));
            _oPdfPTable.AddCell(this.SetCellValue("Cost/Kg(Tk.)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f));
            _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f));
            _oPdfPTable.CompleteRow();


            var sValue = (_sRecipeWrittenBy != _sRecipeEditedBy && _sRecipeEditedBy != "") ? "Recipe edited by:   " + _sRecipeEditedBy : "";
            _oPdfPTable.AddCell(this.SetCellValue("Checked By:", 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f));
            _oPdfPTable.AddCell(this.SetCellValue(sValue, 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f));
            _oPdfPTable.CompleteRow();



        }

        private void PrintFooter()
        {

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPTable.AddCell(this.SetCellValue("", 0, 5, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 40f));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("___________________", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
            _oPdfPTable.AddCell(this.SetCellValue("___________________", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
            _oPdfPTable.AddCell(this.SetCellValue("___________________", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("Tone Check By", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
            _oPdfPTable.AddCell(this.SetCellValue("Migration Package Check By", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
            _oPdfPTable.AddCell(this.SetCellValue("Wash to Fisnishing By", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
            _oPdfPTable.CompleteRow();
        }

        private PdfPCell SetCellValue(string sName, int nRowSpan, int nColumnSpan, int align, BaseColor color, int border, float height)
        {
            PdfPCell oPdfPCell = new PdfPCell(new Phrase(sName, _oFontStyle));
            oPdfPCell.Rowspan = (nRowSpan > 0) ? nRowSpan : 1;
            oPdfPCell.Colspan = (nColumnSpan > 0) ? nColumnSpan : 1;
            oPdfPCell.HorizontalAlignment = align;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = color;
            if (border == 0)
                oPdfPCell.Border = 0;
            if (height > 0)
                oPdfPCell.FixedHeight = height;
            return oPdfPCell;
        }

    }

}
