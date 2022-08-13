using System;
using System.Data;
using ESimSol.BusinessObjects;
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
    public class rptProductionSummary
    {
        #region Declaration
        Document _oDocument;
        int _nCloumns = 13;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(13);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<ProductionScheduleDetail> _oPSDs = new List<ProductionScheduleDetail>();
        Company _oCompany = new Company();
        string _sMessage = "";
        string _sValue = "";
        #endregion


        #region Constructor
        public rptProductionSummary() { }
        #endregion

        public byte[] PrepareReport(List<ProductionScheduleDetail> oPSDs, Company oCompany, string sValue)
        {

            _oPSDs = oPSDs;
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
            _oPdfPTable.SetWidths(new float[] { 30f, 50f, 55f, 45f, 40f, 40f, 40f, 40f, 45f, 40f, 40f, 40f, 60f }); //height:842   width:595
           // _oPdfPTable.SetWidths(new float[] { 30f, 75f, 65f, 60f, 55f, 60f, 65f }); //height:842   width:595
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
            _oPdfPCell = new PdfPCell(new Phrase("Daily Yarn Dyeing Production Report With Cost Analysis.", _oFontStyle));
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
            _oPdfPCell.Colspan = 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            DateTime dt = DateTime.Now;
            string sDate = dt.ToString("dd-MMM-yyyy HH:mm");
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
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Machine Performance", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Production Analysis", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Cost Analysis", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Avg. Duration", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Avg. Batch/MC", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Prod./Day", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Batch Load(%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Net RFT(%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Gross RFT(%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("DC Cost", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Cost/Kg", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shade(%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Addition(%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();


            int nCount = 0;
            double nTotalWeight = 0;
            int nTotalDay = 0;
            int nTotalHour = 0;
            int nTotalMin = 0;
            int nTotalElement=_oPSDs.Count();
            double nTotalAvgDCCost = 0;
            double nTotalAvgCostPerKg = 0;
            double nTotalAvgBatchLoading = 0;
            double nTotalAvgBatchPerMachine = 0;
            double nTotalAvgNetRFT = 0;
            double nTotalAvgGrossRFT = 0;
            double nTotalAvgShadePercentage = 0;
            double nTotalAvgAddition = 0;

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            if (nTotalElement > 0)
            {
               
                _oPSDs = _oPSDs.OrderBy(x => x.DyeUnLoadTime).ToList();
                nTotalWeight = _oPSDs.Sum(x => x.ProductionQty);
                nTotalAvgDCCost = _oPSDs.Sum(x => x.DyeChemicalCost);
                nTotalAvgCostPerKg = _oPSDs.Sum(x => x.DyeChemicalCost) / _oPSDs.Sum(x => x.ProductionQty);
                nTotalAvgBatchLoading=Convert.ToDouble(_oPSDs.Sum(x => x.Loading)) / Convert.ToDouble(_oPSDs.Count());
                nTotalAvgBatchPerMachine = Convert.ToDouble(_oPSDs.Count()) / Convert.ToDouble(_oPSDs.Select(x => x.MachineName).Distinct().Count());

                nTotalAvgShadePercentage = _oPSDs.Sum(x => x.ShadePercentage) / Convert.ToDouble(_oPSDs.Count(x => x.ShadePercentage > 0));
                if (_oPSDs.Count(x => x.PRRemarks != "Ok") > 0) { nTotalAvgAddition = _oPSDs.Where(x => x.PRRemarks != "Ok").Sum(x => x.DyePerCentage) / Convert.ToDouble(_oPSDs.Count(x => x.PRRemarks != "Ok")); }
                nTotalAvgNetRFT = _oPSDs.Count(x => x.PRRemarks == "Ok") > 0 ? (_oPSDs.Count(x => x.PRRemarks == "Ok") * 100) / Convert.ToDouble(_oPSDs.Count()) : 0;
                int nAddOneCount = _oPSDs.Count(x => x.PRRemarks == "Ok, ADD-01"); int nAddTwoCount = _oPSDs.Count(x => x.PRRemarks == "Ok, ADD-02"); int nAddThreeCount = _oPSDs.Count(x => x.PRRemarks == "Ok, ADD-03");
                if ((_oPSDs.Count(x => x.PRRemarks == "Ok") + nAddOneCount + nAddTwoCount + nAddThreeCount) > 0)
                {
                    nTotalAvgGrossRFT = nTotalAvgNetRFT + ((nAddOneCount * 65) + (nAddTwoCount * 35) + (nAddThreeCount * 0)) / Convert.ToDouble(_oPSDs.Count());
                }
                List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
                while (_oPSDs.Count() > 0)
                {

                    oPSDs = new List<ProductionScheduleDetail>();
                    DateTime StartTime = _oPSDs[0].DyeUnLoadTime;
                    StartTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, 9, 0, 0);
                    DateTime EndTime = StartTime.AddDays(1);
                    //oPSDs = _oPSDs.Where(x => x.DyeUnLoadTime.ToString("dd MMM yyyy") == _oPSDs[0].DyeUnLoadTime.ToString("dd MMM yyyy")).ToList();
                    oPSDs = _oPSDs.Where(x => x.DyeUnLoadTime >= StartTime).Where(x => x.DyeUnLoadTime < EndTime).ToList();



                    foreach (ProductionScheduleDetail oItem in oPSDs)
                    {
                        _oPSDs.RemoveAll(x => x.ProductionScheduleDetailID == oItem.ProductionScheduleDetailID);
                    }

                    nCount++;

                    int nDay = 0;
                    int nHour = 0;
                    int nMin = 0;
                    nDay = oPSDs.Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                    nHour = oPSDs.Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                    nMin = oPSDs.Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));

                    nTotalDay = nTotalDay + nDay; nTotalHour = nTotalHour + nHour; nTotalMin = nTotalMin + nMin;

                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                    if (oPSDs[0].DyeUnLoadTime.Hour < 9) { oPSDs[0].DyeUnLoadTime = oPSDs[0].DyeUnLoadTime.AddDays(-1); }
                    _oPdfPCell = new PdfPCell(new Phrase(oPSDs[0].DyeUnLoadTime.ToString("dd MMM yyyy"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                    /*--------------------  Start Machine Performance --------------------------------*/

                    _oPdfPCell = new PdfPCell(new Phrase(TimeStampCoversion(nDay,nHour, nMin,oPSDs.Count()), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    int nMachine = oPSDs.Select(x => x.MachineName).Distinct().Count();
                    double nResult = Convert.ToDouble(oPSDs.Count()) / Convert.ToDouble(nMachine);
                    _oPdfPCell = new PdfPCell(new Phrase(nResult.ToString("0.00"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    /*--------------------  End Machine Performance --------------------------------*/


                    /*--------------------  Start Production Analysis --------------------------------*/

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oPSDs.Sum(x => x.ProductionQty)), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Convert.ToDouble(oPSDs.Sum(x => x.Loading) / Convert.ToDouble(oPSDs.Count())).ToString("0.00"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    double nNetRFT = 0;
                    nNetRFT = oPSDs.Count(x => x.PRRemarks == "Ok") > 0 ? (oPSDs.Count(x => x.PRRemarks == "Ok") * 100) / Convert.ToDouble(oPSDs.Count()) : 0;
                    _oPdfPCell = new PdfPCell(new Phrase( Math.Round(nNetRFT,2).ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                    int nAddOneOkCount = oPSDs.Count(x => x.PRRemarks == "Ok, ADD-01"); int nAddTwoOkCount = oPSDs.Count(x => x.PRRemarks == "Ok, ADD-02"); int nAddThreeOkCount = oPSDs.Count(x => x.PRRemarks == "Ok, ADD-03");
                    double nGrossRFT = 0;
                    if ((oPSDs.Count(x => x.PRRemarks == "Ok") + nAddOneOkCount + nAddTwoOkCount + nAddThreeOkCount) > 0)
                    {
                        nGrossRFT = nNetRFT + ((nAddOneOkCount * 65) + (nAddTwoOkCount * 35) + (nAddThreeOkCount * 0)) / Convert.ToDouble(oPSDs.Count());
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(Math.Round(nGrossRFT, 2).ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    /*-------------------- End  Production Analysis --------------------------------*/



                    /*-------------------- Start Cost Analysis --------------------------------*/
                    double nAvgAddition=0;
                    int nAddition=oPSDs.Count(x=>x.PRRemarks != "Ok");
                    if (nAddition > 0) { nAvgAddition = oPSDs.Where(x => x.PRRemarks != "Ok").Sum(x => x.DyePerCentage) / Convert.ToDouble(nAddition); }

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oPSDs.Sum(x => x.DyeChemicalCost)), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oPSDs.Sum(x => x.DyeChemicalCost) / oPSDs.Sum(x => x.ProductionQty)), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Math.Round(Convert.ToDouble((oPSDs.Count(x => x.ShadePercentage > 0) > 0) ? oPSDs.Sum(x => x.ShadePercentage) / oPSDs.Count(x => x.ShadePercentage > 0) : 0), 2).ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(nAvgAddition.ToString("0.00"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    /*-------------------- End Cost Analysis --------------------------------*/

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPTable.CompleteRow();
                }
            }


            //_oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            //_oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.BorderColorTop = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalWeight), _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.BorderColorTop = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.BorderColorTop = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPTable.CompleteRow();

                

            #region Signature Part

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
            _oPdfPCell = new PdfPCell(new Phrase("Avg Duration: " + TimeStampCoversion(nTotalDay, nTotalHour, nTotalMin, nTotalElement), _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Production(kg) : " + Global.MillionFormat(nTotalWeight), _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total DC Cost(BDT): " + Global.MillionFormat(nTotalAvgDCCost), _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            /*...............................................................................*/

            _oPdfPCell = new PdfPCell(new Phrase("Avg Batch/M: " + Math.Round(nTotalAvgBatchPerMachine,2), _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Avg. Batch Loading(%): " + Math.Round(nTotalAvgBatchLoading, 2) + "%", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Cost Per kg(BDT): " + Math.Round(nTotalAvgCostPerKg, 2), _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            /*...............................................................................*/
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Net RFT : " + Math.Round(nTotalAvgNetRFT, 2) + "%", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Avg. Shade(%): " + Math.Round(nTotalAvgShadePercentage, 2) + "%", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            /*...............................................................................*/
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Gross RFT: " + Math.Round(nTotalAvgGrossRFT, 2) + "%", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Avg. Addition Shade(%): " + Math.Round(nTotalAvgAddition,2) + "%", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();



            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nCloumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("______________________", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("______________________", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            //Executive/ Sr.Executive
            _oPdfPCell = new PdfPCell(new Phrase("Manager(Yarn Dyeing)", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Executive Director", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
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

                string sAvgDay = "", sAvgHour = "", sAvgMin = "";
                if (nAvgDay < 10) { sAvgDay = "0" + nAvgDay.ToString(); } else { sAvgDay = nAvgDay.ToString(); }
                if (nAvgHour < 10) { sAvgHour = "0" + nAvgHour.ToString(); } else { sAvgHour = nAvgHour.ToString(); }
                if (nAvgMin < 10)
                {
                    nAvgMin = Math.Round(nAvgMin, 2);
                    if (nAvgMin.ToString().Contains('.'))
                    {
                        sAvgMin = "0" + nAvgMin.ToString().Split('.')[0] + "." + nAvgMin.ToString().Split('.')[1];
                    }
                    else
                    {
                        sAvgMin = "0" + nAvgMin.ToString();
                    }
                }
                else
                {
                    sAvgMin = nAvgMin.ToString();
                }
                if (Convert.ToInt32(sAvgDay) <= 0) { sAvgTime = sAvgHour + "h " + sAvgMin + "m"; }
                else { sAvgTime = sAvgDay + "d " + sAvgHour + "h " + sAvgMin + "m"; }

            }
            return sAvgTime;
        }

        #endregion
    }
}
