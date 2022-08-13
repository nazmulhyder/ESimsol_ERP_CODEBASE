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
    public class rptProductionAnalysisWithShadeSummary
    {
        #region Declaration
        Document _oDocument;
        int _nCloumns = 26;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(26);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<RptProductionCostAnalysis> _oRPCAs = new List<RptProductionCostAnalysis>();
        Company _oCompany = new Company();
        string _sMessage = "";
        string _sValue = "";
        #endregion


        #region Constructor
        public rptProductionAnalysisWithShadeSummary() { }
        #endregion

        public byte[] PrepareReport(List<RptProductionCostAnalysis> oRPCAs, Company oCompany, string sValue)
        {

            _oRPCAs = oRPCAs;
            _oCompany = oCompany;
            _sValue = sValue;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(PageSize.A4.Rotate());
            _oDocument.SetMargins(5f, 5f, 20f, 20f);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.SetWidths(new float[] { 30f, 45f, 40f, 38f, 38f, 38f, 38f, 38f, 40f, 35f, 35f, 35f, 35f, 35f, 25f, 25f, 25f, 25f, 25f, 25f, 38f, 32f, 38f, 50f, 50f, 50f, }); //height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 7;
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
            _oPdfPCell = new PdfPCell(new Phrase("Production Summary Report With Shade Group.", _oFontStyle));
            _oPdfPCell.Colspan = _nCloumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region DateRange

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_sValue, _oFontStyle));
            _oPdfPCell.Colspan = 10;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            DateTime dt = DateTime.Now;
            string sDate = dt.ToString("dd.MM.yy HH:mm");
            _oPdfPCell = new PdfPCell(new Phrase(sDate, _oFontStyle));
            _oPdfPCell.Colspan = _nCloumns - 10;
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

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Production(kg)", _oFontStyle));
            _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Duration", _oFontStyle));
            _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Avg Shade (%)", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Add. Shade (%)", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Avg Batch", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Avg Batch Load (%)", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Net RFT (%)", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Gross RFT (%)", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Cost/Kg(BDT)", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Cost(BDT)", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();




            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("White (0%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Light (0.01-.5%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Medium (0.51-2%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Deep (2.01-3%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Ext. Dark (>3%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);




            _oPdfPCell = new PdfPCell(new Phrase("Average", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("White (0%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Light (0.01-.5%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Medium (0.51-2%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Deep (2.01-3%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Ext. Dark (>3%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);




            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Dyes", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Chemical", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);




            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Dyes", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Chemical", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();


            int nCount = 0;
            double nTotalWeight = 0;
            int nTotalDay = 0;
            int nTotalHour = 0;
            int nTotalMin = 0;
            int nTotalElement = _oRPCAs.Count();


            //double nTotalAvgDCCost = 0;
            //double nTotalAvgCostPerKg = 0;
            //double nTotalAvgBatchLoading = 0;
            //double nTotalAvgBatchPerMachine = 0;
            //double nTotalAvgNetRFT = 0;
            //double nTotalAvgGrossRFT = 0;
            //double nTotalAvgShadePercentage = 0;
            //double nTotalAvgAddition = 0;

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            if (nTotalElement > 0)
            {

                _oRPCAs = _oRPCAs.OrderBy(x => x.EndTime).ToList();
                //nTotalWeight = _oRPCAs.Sum(x => Math.Round(x.ProductionQty));
                //nTotalAvgDCCost = _oRPCAs.Sum(x => x.TotalCost);
                //nTotalAvgCostPerKg = _oRPCAs.Sum(x => Math.Round(x.TotalCost)) / Convert.ToDouble(_oRPCAs.Sum(x => Math.Round(x.ProductionQty)));
                //nTotalAvgBatchLoading = Convert.ToDouble(_oRPCAs.Sum(x => x.Loading)) / Convert.ToDouble(_oRPCAs.Count());
                //nTotalAvgBatchPerMachine = Convert.ToDouble(_oRPCAs.Count()) / Convert.ToDouble(_oRPCAs.Select(x => x.MachineName).Distinct().Count());
                //nTotalAvgShadePercentage = _oRPCAs.Sum(x => x.ShadePercentage) / Convert.ToDouble(_oRPCAs.Count(x => x.ShadePercentage > 0));
                //if (_oRPCAs.Count(x => x.Remark != "Ok") > 0) { nTotalAvgAddition = _oRPCAs.Where(x => x.Remark != "Ok").Sum(x => x.AdditionalPercentage) / Convert.ToDouble(_oRPCAs.Count(x => x.Remark != "Ok")); }
                //nTotalAvgNetRFT = _oRPCAs.Count(x => x.Remark == "Ok") > 0 ? (_oRPCAs.Count(x => x.Remark == "Ok") * 100) / Convert.ToDouble(_oRPCAs.Count()) : 0;
                //int nAddOneCount = _oRPCAs.Count(x => x.Remark == "Ok, ADD-01"); int nAddTwoCount = _oRPCAs.Count(x => x.Remark == "Ok, ADD-02"); int nAddThreeCount = _oRPCAs.Count(x => x.Remark == "Ok, ADD-03");
                //if ((_oRPCAs.Count(x => x.Remark == "Ok") + nAddOneCount + nAddTwoCount + nAddThreeCount) > 0)
                //{
                //    nTotalAvgGrossRFT = nTotalAvgNetRFT + ((nAddOneCount * 65) + (nAddTwoCount * 35) + (nAddThreeCount * 0)) / Convert.ToDouble(_oRPCAs.Count());
                //}
                List<RptProductionCostAnalysis> oRPCAs = new List<RptProductionCostAnalysis>();
                while (_oRPCAs.Count() > 0)
                {

                    oRPCAs = new List<RptProductionCostAnalysis>();
                    DateTime StartTime = _oRPCAs[0].EndTime;
                    if (_oRPCAs[0].EndTime.Hour < 9) { StartTime = StartTime.AddDays(-1); }
                    StartTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, 9, 0, 0);
                    DateTime EndTime = StartTime.AddDays(1);
                    oRPCAs = _oRPCAs.Where(x => x.EndTime >= StartTime).Where(x => x.EndTime < EndTime).ToList();
                    if (oRPCAs.Count()>0)
                    {
                        foreach (RptProductionCostAnalysis oItem in oRPCAs)
                        {
                            _oRPCAs.RemoveAll(x => x.RouteSheetID == oItem.RouteSheetID);
                        }

                        nCount++;

                        int nDay = 0;
                        int nHour = 0;
                        int nMin = 0;
                        nDay = oRPCAs.Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                        nHour = oRPCAs.Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                        nMin = oRPCAs.Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));

                        nTotalDay = nTotalDay + nDay; nTotalHour = nTotalHour + nHour; nTotalMin = nTotalMin + nMin;

                        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                        if (oRPCAs[0].EndTime.Hour < 9) { oRPCAs[0].EndTime = oRPCAs[0].EndTime.AddDays(-1); }
                        _oPdfPCell = new PdfPCell(new Phrase(oRPCAs[0].EndTime.ToString("dd.MM.yy"), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                        #region Production

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oRPCAs.Sum(x => Math.Round(x.ProductionQty))).Split('.')[0], _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oRPCAs.Where(x => x.TotalShadePercentage == 0).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0], _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oRPCAs.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0], _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oRPCAs.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0], _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oRPCAs.Where(x => x.TotalShadePercentage > 2 && x.TotalShadePercentage<=3).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0], _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oRPCAs.Where(x => x.TotalShadePercentage > 3).Sum(x => Math.Round(x.ProductionQty))).Split('.')[0], _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        #endregion


                        #region Duration

                        _oPdfPCell = new PdfPCell(new Phrase(TimeStampCoversion(nDay, nHour, nMin, oRPCAs.Count()), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        if (oRPCAs.Where(x => x.TotalShadePercentage == 0).Count() > 0)
                        {
                            nDay = oRPCAs.Where(x => x.TotalShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                            nHour = oRPCAs.Where(x => x.TotalShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                            nMin = oRPCAs.Where(x => x.TotalShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                        }
                        else { nDay = 0; nHour = 0; nMin = 0; }

                        _oPdfPCell = new PdfPCell(new Phrase(TimeStampCoversion(nDay, nHour, nMin, oRPCAs.Where(x => x.TotalShadePercentage == 0).Count()), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                        if (oRPCAs.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Count() > 0)
                        {
                            nDay = oRPCAs.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                            nHour = oRPCAs.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                            nMin = oRPCAs.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                        }
                        else { nDay = 0; nHour = 0; nMin = 0; }

                        _oPdfPCell = new PdfPCell(new Phrase(TimeStampCoversion(nDay, nHour, nMin, oRPCAs.Where(x => x.TotalShadePercentage > 0).Where(x => x.TotalShadePercentage <= .5).Count()), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                        if (oRPCAs.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Count() > 0)
                        {
                            nDay = oRPCAs.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                            nHour = oRPCAs.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                            nMin = oRPCAs.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                        }
                        else { nDay = 0; nHour = 0; nMin = 0; }

                        _oPdfPCell = new PdfPCell(new Phrase(TimeStampCoversion(nDay, nHour, nMin, oRPCAs.Where(x => x.TotalShadePercentage > .5).Where(x => x.TotalShadePercentage <= 2).Count()), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                        if (oRPCAs.Where(x => x.TotalShadePercentage > 2 && x.TotalShadePercentage<=3).Count() > 0)
                        {
                            nDay = oRPCAs.Where(x => x.TotalShadePercentage > 2 && x.TotalShadePercentage <= 3).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                            nHour = oRPCAs.Where(x => x.TotalShadePercentage > 2 && x.TotalShadePercentage <= 3).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                            nMin = oRPCAs.Where(x => x.TotalShadePercentage > 2 && x.TotalShadePercentage <= 3).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                        }
                        else { nDay = 0; nHour = 0; nMin = 0; }

                        _oPdfPCell = new PdfPCell(new Phrase(TimeStampCoversion(nDay, nHour, nMin, oRPCAs.Where(x => x.TotalShadePercentage > 2 && x.TotalShadePercentage <= 3).Count()), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                        if (oRPCAs.Where(x => x.TotalShadePercentage > 3).Count() > 0)
                        {
                            nDay = oRPCAs.Where(x => x.TotalShadePercentage > 3).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                            nHour = oRPCAs.Where(x => x.TotalShadePercentage > 3).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                            nMin = oRPCAs.Where(x => x.TotalShadePercentage > 3).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                        }
                        else { nDay = 0; nHour = 0; nMin = 0; }

                        _oPdfPCell = new PdfPCell(new Phrase(TimeStampCoversion(nDay, nHour, nMin, oRPCAs.Where(x => x.TotalShadePercentage > 3).Count()), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        #endregion


                        #region Shade Percentage, Additional, Batch, Loading, Net RFT, Gross RFT

                        _oPdfPCell = new PdfPCell(new Phrase(Math.Round(Convert.ToDouble((oRPCAs.Count(x => x.TotalShadePercentage > 0) > 0) ? oRPCAs.Sum(x => x.TotalShadePercentage) / oRPCAs.Count(x => x.ShadePercentage > 0) : 0), 2).ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        double nAvgAddition = 0;
                        int nAddition = oRPCAs.Count(x => x.Remark != "Ok");
                        if (nAddition > 0) { nAvgAddition = oRPCAs.Where(x => x.Remark != "Ok").Sum(x => x.AdditionalPercentage) / Convert.ToDouble(nAddition); }

                        _oPdfPCell = new PdfPCell(new Phrase(nAvgAddition.ToString("0.00"), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                        int nMachine = oRPCAs.Select(x => x.MachineName).Distinct().Count();
                        double nResult = Convert.ToDouble(oRPCAs.Count()) / Convert.ToDouble(nMachine);
                        _oPdfPCell = new PdfPCell(new Phrase(nResult.ToString("0.00"), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                        _oPdfPCell = new PdfPCell(new Phrase(Convert.ToDouble(oRPCAs.Sum(x => x.Loading) / Convert.ToDouble(oRPCAs.Count())).ToString("0.00"), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                        double nNetRFT = 0;
                        nNetRFT = oRPCAs.Count(x => x.Remark == "Ok") > 0 ? (oRPCAs.Count(x => x.Remark == "Ok") * 100) / Convert.ToDouble(oRPCAs.Count()) : 0;
                        _oPdfPCell = new PdfPCell(new Phrase(Math.Round(nNetRFT, 2).ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                        int nAddOneOkCount = oRPCAs.Count(x => x.Remark == "Ok, ADD-01"); int nAddTwoOkCount = oRPCAs.Count(x => x.Remark == "Ok, ADD-02"); int nAddThreeOkCount = oRPCAs.Count(x => x.Remark == "Ok, ADD-03");
                        double nGrossRFT = 0;
                        if ((oRPCAs.Count(x => x.Remark == "Ok") + nAddOneOkCount + nAddTwoOkCount + nAddThreeOkCount) > 0)
                        {
                            nGrossRFT = nNetRFT + ((nAddOneOkCount * 65) + (nAddTwoOkCount * 35) + (nAddThreeOkCount * 0)) / Convert.ToDouble(oRPCAs.Count());
                        }
                        _oPdfPCell = new PdfPCell(new Phrase(Math.Round(nGrossRFT, 2).ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                        #endregion


                        #region Cost Calculation Per Kg


                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Math.Round(oRPCAs.Sum(x => Math.Round(x.TotalCost)) / oRPCAs.Sum(x => Math.Round(x.ProductionQty)))).Split('.')[0], _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Math.Round(oRPCAs.Sum(x => Math.Round(x.DyesCost)) / oRPCAs.Sum(x => Math.Round(x.ProductionQty)))).Split('.')[0], _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Math.Round(oRPCAs.Sum(x => Math.Round(x.ChemicalCost)) / oRPCAs.Sum(x => Math.Round(x.ProductionQty)))).Split('.')[0], _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        #endregion


                        #region Total Cost Calculation

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oRPCAs.Sum(x => Math.Round(x.TotalCost))).Split('.')[0], _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oRPCAs.Sum(x => Math.Round(x.DyesCost))).Split('.')[0], _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oRPCAs.Sum(x => Math.Round(x.ChemicalCost))).Split('.')[0], _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        #endregion

                        _oPdfPTable.CompleteRow();
                    }

                }
            }



            #region Signature Part

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = _nCloumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("Machine Performance", _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPCell = new PdfPCell(new Phrase("Production Analysis", _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPCell = new PdfPCell(new Phrase("Cost Analysis", _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPTable.CompleteRow();


            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            ///*...............................................................................*/
            //_oPdfPCell = new PdfPCell(new Phrase("Avg Duration: " + TimeStampCoversion(nTotalDay, nTotalHour, nTotalMin, nTotalElement), _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Total Production(kg) : " + Global.MillionFormat(nTotalWeight), _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Total DC Cost(BDT): " + Global.MillionFormat(nTotalAvgDCCost), _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPTable.CompleteRow();

            ///*...............................................................................*/

            //_oPdfPCell = new PdfPCell(new Phrase("Avg Batch/M: " + Math.Round(nTotalAvgBatchPerMachine, 2), _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Avg. Batch Loading(%): " + Math.Round(nTotalAvgBatchLoading, 2) + "%", _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Cost Per kg(BDT): " + Math.Round(nTotalAvgCostPerKg, 2), _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPTable.CompleteRow();


            ///*...............................................................................*/
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Net RFT : " + Math.Round(nTotalAvgNetRFT, 2) + "%", _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Avg. Shade(%): " + Math.Round(nTotalAvgShadePercentage, 2) + "%", _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPTable.CompleteRow();


            ///*...............................................................................*/
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Gross RFT: " + Math.Round(nTotalAvgGrossRFT, 2) + "%", _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Avg. Addition Shade(%): " + Math.Round(nTotalAvgAddition, 2) + "%", _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPTable.CompleteRow();



            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nCloumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("______________________", _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 11; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("______________________", _oFontStyle));
            _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            //Executive/ Sr.Executive
            _oPdfPCell = new PdfPCell(new Phrase("Manager(Yarn Dyeing)", _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 11; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Executive Director", _oFontStyle));
            _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
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

