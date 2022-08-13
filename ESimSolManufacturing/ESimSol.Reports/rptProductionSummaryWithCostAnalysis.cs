using System;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Linq;

namespace ESimSol.Reports
{
    public class rptProductionSummaryWithCostAnalysis
    {
        #region Declaration
        Document _oDocument;
        int _nCloumns = 13;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(13);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<RptProductionCostAnalysis> _oRPCAs = new List<RptProductionCostAnalysis>();
        Company _oCompany = new Company();
        string _sMessage = "";
        string _sValue = "";
        #endregion


        #region Constructor
        public rptProductionSummaryWithCostAnalysis() { }
        #endregion

        public byte[] PrepareReport(List<RptProductionCostAnalysis> oRPCAs, Company oCompany, string sValue)
        {

            _oRPCAs = oRPCAs;
            _oCompany = oCompany;
            _sValue = sValue;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(PageSize.A4);
            _oDocument.SetMargins(10f, 10f, 20f, 20f);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.SetWidths(new float[] { 30f, 55f, 35f, 47f, 50f, 45f, 63f, 33f, 47f, 40f, 30f, 48f, 73f }); //height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 6;
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
            _oPdfPCell.Colspan = _nCloumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("(Yarn Dyeing Unit)", _oFontStyle));
            _oPdfPCell.Colspan = _nCloumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.FactoryAddress, _oFontStyle));
            _oPdfPCell.Colspan = _nCloumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Production Summary With Cost Analysis", _oFontStyle));
            _oPdfPCell.Colspan = _nCloumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region DateRange

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_sValue, _oFontStyle));
            _oPdfPCell.Colspan = 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            DateTime dt = DateTime.Now;
            string sDate = dt.ToString("dd.MM.yy HH:mm");
            _oPdfPCell = new PdfPCell(new Phrase(sDate, _oFontStyle));
            _oPdfPCell.Colspan = _nCloumns - 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion

        #region Report Body
        private void PrintBody()
        {

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Machine Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Cap.(kg)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Work Order", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shade", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Yarn Count", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Batch", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Weight/ Load(%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Duration", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shd(%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("DC Cost(BDT)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();


            int nCount = 0;
            double nTotalWeight = 0;
            int nOkCount = 0;
            int nAddOneOkCount = 0;
            int nAddTwoOkCount = 0;
            int nAddThreeOkCount = 0;
            double nTotalBatchLoading = 0;
            double nDyeChemicalCost = 0;
            double nAdditionalShadePercentage = 0;

            double nTotalUsesWeight = 0;
            double nTotalDyesQty = 0;

            int nDay = 0;
            int nHour = 0;
            int nMin = 0;

            int nTotalDay = 0;
            int nTotalHour = 0;
            int nTotalMin = 0;


            if (_oRPCAs.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
                List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
                while (_oRPCAs.Count() > 0)
                {
                    oRPCAs = new List<RptProductionCostAnalysis>();

                    oRPCAs = (_oRPCAs[0].CombineRSNo != "") ? _oRPCAs.Where(x => x.CombineRSNo == _oRPCAs[0].CombineRSNo).ToList() : _oRPCAs.Where(x => x.RouteSheetID == _oRPCAs[0].RouteSheetID).ToList();

                    _oRPCAs.RemoveAll(x => oRPCAs.Select(o => o.RouteSheetID).Contains(x.RouteSheetID));

                    if (oRPCAs.Count() > 0)
                    {

                        nCount++;
                        nTotalWeight = nTotalWeight + oRPCAs.Sum(x => x.ProductionQty);
                        nDyeChemicalCost += oRPCAs.Sum(x => Math.Round(x.TotalCost));
                        nAdditionalShadePercentage = (oRPCAs.Count(x => x.Remark != "Ok") > 0) ? oRPCAs.Where(x => x.Remark != "Ok").Sum(x => x.AdditionalPercentage) / (oRPCAs.Count(x => x.DyesCost > 0) >0 ? Convert.ToDouble(oRPCAs.Count(x => x.DyesCost > 0) ) : 1) : 0 ;
                        nTotalUsesWeight = nTotalUsesWeight + oRPCAs[0].UsesWeight;

                        nDay = 0; nHour = 0; nMin = 0;

                        nDay =  oRPCAs.Select(x=> Convert.ToInt32(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' }))).Sum();
                        nHour = oRPCAs.Select(x=> Convert.ToInt32(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' }))).Sum();
                        nMin = oRPCAs.Select(x => Convert.ToInt32(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' }))).Sum();

                        nTotalDay = nTotalDay + nDay;
                        nTotalHour = nTotalHour + nHour;
                        nTotalMin = nTotalMin + nMin;

                        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oRPCAs[0].MachineName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oRPCAs[0].UsesWeight, 2).ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(string.Join(", ", oRPCAs.Select(x => (x.OrderNo.Contains("]")) ? x.OrderNo : x.OrderNo).Distinct().ToList()), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(string.Join(", ", oRPCAs.Select(x => x.BuyerName.Trim()).Distinct()), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(string.Join(", ", oRPCAs.Select(x => x.Shade.Trim()).Distinct()), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase(oItem.Count, _oFontStyle));
                        _oPdfPCell = new PdfPCell(new Phrase(string.Join(", ", oRPCAs.Select(x => x.ProductName.Trim()).Distinct()), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(string.Join(", ", oRPCAs.Select(x =>x.BatchNo.Trim())), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oRPCAs.Sum(x => x.ProductionQty).ToString() + "(" + Convert.ToDouble(oRPCAs.Sum(x => x.ProductionQty) * 100 / (oRPCAs[0].UsesWeight>0 ? oRPCAs[0].UsesWeight : 1) ).ToString("0.00") + ")", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(TimeStampCoversion(nDay, nHour, nMin, 1), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        nTotalDyesQty += (oRPCAs.Sum(x => x.DyesQty) > 0) ? oRPCAs.Sum(x => x.TotalDyesQty) : 0;
                        _oPdfPCell = new PdfPCell(new Phrase(Math.Round(Convert.ToDouble((oRPCAs.Sum(x => x.DyesQty) > 0) ? oRPCAs.Sum(x => x.TotalDyesQty) * 100 / (oRPCAs.Sum(x => x.ProductionQty)>0 ? oRPCAs.Sum(x => x.ProductionQty) : 0) : 0), 2).ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Math.Round(oRPCAs.Sum(x => x.TotalCost))).Split('.')[0], _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase((oRPCAs[0].Remark != "Ok" ? oRPCAs[0].PRRemarks + "(" + ((oRPCAs[0].Remark != "Ok" && oRPCAs[0].DyesCost > 0) ? (oRPCAs.Sum(x => x.AdditionalPercentage) / (oRPCAs.Sum(x => x.DyesCost)>0 ?  oRPCAs.Sum(x => x.DyesCost): 1)) : 0).ToString("0.00") + ")" : oRPCAs[0].PRRemarks), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        if (oRPCAs[0].Remark == "Ok")
                        {
                            ++nOkCount;
                        }
                        else if (oRPCAs[0].Remark == "Ok, ADD-01")
                        {
                            ++nAddOneOkCount;
                        }
                        else if (oRPCAs[0].Remark == "Ok, ADD-02")
                        {
                            ++nAddTwoOkCount;
                        }
                        else if (oRPCAs[0].Remark == "Ok, ADD-03")
                        {
                            ++nAddThreeOkCount;
                        }
                        _oPdfPTable.CompleteRow();
                    }


                }


            #region Conclusion Part

                double nNetRFT = 0;

                if (nOkCount > 0)
                {
                    nNetRFT = (nOkCount * 100) / (nCount>0?Convert.ToDouble(nCount):1);
                }
                double nGrossRFT = 0;
                if ((nOkCount + nAddOneOkCount + nAddTwoOkCount + nAddThreeOkCount) > 0)
                {
                    nGrossRFT = (((nOkCount * 100) + (nAddOneOkCount * 65) + (nAddTwoOkCount * 35) + (nAddThreeOkCount * 0))) / (nCount > 0 ? Convert.ToDouble(nCount) : 1);
                }

                nTotalBatchLoading = nTotalWeight * 100 / (nCount > 0 ? Convert.ToDouble(nTotalUsesWeight) : 1);

                double nCostPerKg = 0;
                if (nDyeChemicalCost > 0 && nTotalWeight > 0)
                {
                    nCostPerKg = Math.Round(nDyeChemicalCost / (nCount > 0 ? Convert.ToDouble(nTotalWeight) : 1), 2);
                }

                double nAvgShadePercentage = Math.Round( nTotalDyesQty * 100 / (nCount > 0 ? Convert.ToDouble(nTotalWeight) : 1) , 2);
                

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = _nCloumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Machine Performance", _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("Production Analysis", _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("Cost Analysis", _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();


                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

                /*...............................................................................*/
                _oPdfPCell = new PdfPCell(new Phrase("Avg Duration: " + TimeStampCoversion(nTotalDay, nTotalHour, nTotalMin, nCount), _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Total Production(kg) : " + Global.MillionFormat(nTotalWeight), _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Total DC Cost(BDT): " + Global.MillionFormat(nDyeChemicalCost), _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                /*...............................................................................*/
                int nMachine = _oRPCAs.Select(x => x.MachineName).Distinct().Count();
                double nResult = Convert.ToDouble(nCount) / Convert.ToDouble(nMachine);

                _oPdfPCell = new PdfPCell(new Phrase("Avg Batch/M: " + nResult.ToString("0.00"), _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Avg. Batch Loading(%): " + Math.Round(nTotalBatchLoading, 2) + "%", _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Cost Per kg(BDT): " +Global.MillionFormat(Math.Round(nCostPerKg)), _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();


                /*...............................................................................*/
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Net RFT : " + Math.Round(nNetRFT, 2) + "%", _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Avg. Shade(%): " + Math.Round(nAvgShadePercentage, 2) + "%", _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();


                /*...............................................................................*/
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Gross RFT: " + Math.Round(nGrossRFT, 2) + "%", _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Avg. Addition Shade(%): " + Math.Round(nAdditionalShadePercentage, 2)  + "%", _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();



            }

            else
            {

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            #endregion

            #region Signature Part

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nCloumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("______________________", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("______________________", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            //Executive/ Sr.Executive 
            _oPdfPCell = new PdfPCell(new Phrase("Manager(Yarn Dyeing)", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Executive Director", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Day Hour Min Conversion

        public String TimeStampCoversion(int nDay, int nHour, int nMin, int nCount)
        {
            string sAvgTime = "";
            if (nMin > 0)
            {
                nHour = nHour + (nMin / 60);
                nMin = nMin % 60;
            }
            if (nHour >= 24)
            {
                nDay = nDay + (nHour / 24);
                nHour = nHour % 24;
            }

            if (nMin > 0 || nHour > 0 || nDay > 0)
            {
                int nRemainDay = nDay % nCount;
                if (nRemainDay != 0) { nHour += nRemainDay * 24; }
                int nReminHour = nHour % nCount;
                if (nReminHour != 0) { nMin += nReminHour * 60; }

                int nAvgDay = nDay / nCount;
                int nAvgHour = nHour / nCount;
                double nAvgMin = nMin / nCount;

                if (nAvgDay > 0)
                {
                    nAvgHour = nAvgHour + (nAvgDay * 24);
                }

                sAvgTime = ((nAvgHour > 9) ? nAvgHour.ToString() : "0" + nAvgHour.ToString()) + ":" + ((nAvgMin > 9) ? nAvgMin.ToString() : "0" + nAvgMin.ToString());

                //string sAvgDay = "", sAvgHour = "", sAvgMin = "";
                //if (nAvgDay < 10) { sAvgDay = "0" + nAvgDay.ToString(); } else { sAvgDay = nAvgDay.ToString(); }
                //if (nAvgHour < 10) { sAvgHour = "0" + nAvgHour.ToString(); } else { sAvgHour = nAvgHour.ToString(); }
                //if (nAvgMin < 10)
                //{
                //    nAvgMin = Math.Round(nAvgMin, 2);
                //    if (nAvgMin.ToString().Contains('.'))
                //    {
                //        sAvgMin = "0" + nAvgMin.ToString().Split('.')[0] + "." + nAvgMin.ToString().Split('.')[1];
                //    }
                //    else
                //    {
                //        sAvgMin = "0" + nAvgMin.ToString();
                //    }
                //}
                //else
                //{
                //    sAvgMin = nAvgMin.ToString();
                //}
                //if (Convert.ToInt32(sAvgDay) <= 0) { sAvgTime = sAvgHour + "h " + sAvgMin + "m"; }
                //else { sAvgTime = sAvgDay + "d " + sAvgHour + "h " + sAvgMin + "m"; }

            }
            return sAvgTime;
        }

        #endregion
    }
}

