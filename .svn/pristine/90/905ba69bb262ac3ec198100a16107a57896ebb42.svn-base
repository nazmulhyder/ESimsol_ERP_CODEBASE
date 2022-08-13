using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ESimSol.Reports
{
    public class rptProductionRFTSummary
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);//number of columns
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        double _TotalBatchRFT = 0; double _TotalBatchWidthAddition = 0; double _TotalBatch = 0; double _TotalRFTPercentageOnBatch = 0;
        double _TotalKgWithRFT = 0; double _TotalkgWithAddition = 0; double _TotalKG = 0; double _TotalRFTPercentageVolume = 0;
        RptProductionCostAnalysis _oRptProductionCostAnalysis = new RptProductionCostAnalysis();
        List<RptProductionCostAnalysis> _oRptProductionCostAnalysiss = new List<RptProductionCostAnalysis>();

        BusinessUnit _oBusinessUnit = new BusinessUnit();
        string _dateRange = "";
        string sMonth = "";
        #endregion
        public byte[] PrepareReport(List<RptProductionCostAnalysis> oRptProductionCostAnalysiss,string dateRange, Company oCompany,BusinessUnit oBusinessUnit, string sMessage)
        {
            _oBusinessUnit = oBusinessUnit;
            _oRptProductionCostAnalysiss = oRptProductionCostAnalysiss;
            _oCompany = oCompany;
            _dateRange = dateRange;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 20f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion
            this.PrintHeader();
            //this.PrintBody();
            this.GetDateRangeTable();
            //_oDocument.Add(_oPdfPTable);
            //_oDocument.NewPage();
            //_oPdfPTable.DeleteBodyRows();

            this.GetsInHouseOutSide(oRptProductionCostAnalysiss,"");
            this.GetsAllOver(oRptProductionCostAnalysiss,"");

            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
            this.PrintHeader();
            this.GetDateRangeTable();
            _oRptProductionCostAnalysiss = oRptProductionCostAnalysiss.Where(x => x.IsReDyeing == false && x.IsInHouse==true).ToList();
            this.GetsBulk(_oRptProductionCostAnalysiss, "In House");
            _oRptProductionCostAnalysiss = oRptProductionCostAnalysiss.Where(x => x.IsReDyeing == false && x.IsInHouse == false).ToList();
            this.GetsBulk(_oRptProductionCostAnalysiss, "Out Party");

            _oRptProductionCostAnalysiss = oRptProductionCostAnalysiss.Where(x => x.IsReDyeing == false && (x.OrderType == (int)EnumOrderType.BulkOrder || x.OrderType == (int)EnumOrderType.SaleOrder || x.OrderType == (int)EnumOrderType.SaleOrder_Two)).ToList();
            this.GetsBulk(_oRptProductionCostAnalysiss, "BULK");
            _oRptProductionCostAnalysiss = oRptProductionCostAnalysiss.Where(x => x.IsReDyeing == false && (x.OrderType == (int)EnumOrderType.SampleOrder || x.OrderType == (int)EnumOrderType.SampleOrder_Two)).ToList();
            this.GetsBulk(_oRptProductionCostAnalysiss, "Sample");
            //_oPdfPTable.HeaderRows = 4;
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
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        #endregion
        #region Report Body
        private void PrintBody()
        {
             GetDateRangeTable();
             GetTopTable2();
             GetTopTable();
          
        }
        #endregion

        #region Date Range TABLE
        private void GetDateRangeTable()
        {
            PdfPTable oTopTable = new PdfPTable(2);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                                             
                                                150f,//Shade Name                                                 
                                                100f, //batch with rft                                                                                    
                                            });


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Date Range: " + _dateRange, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Reporting Time: " + Convert.ToDateTime(DateTime.Now), _oFontStyle)); _oPdfPCell.Border = 0; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #endregion

            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }


        #region TOP TABLE
        private void GetTopTable()
        {
            PdfPTable oTopTable = new PdfPTable(9);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                                             
                                                150f,//Shade Name                                                 
                                                100f, //batch with rft
                                                100f, //batch with addition                                                 
                                                100f, // total batch
                                                100f, //rft batch with percentage                                                 
                                                100f,//ky with rft 
                                                100f, //kg with addition
                                                100f, //totalKG                                                 
                                                100f // RFT PERCENTAGE WITH VOLUME                                          
                                            });


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Shade Name", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Overall Bulk & Sample" + "\n(Based on Batches - Number)", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Overall Bulk & Sample " + "\n(Based on Volume -kg)", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
           

            #region 2nd head
            _oPdfPCell = new PdfPCell(new Phrase("Batch with RFT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Batch with Addition", _oFontStyle)); 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Batch", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("RFT percentage on Batch", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Kg With RFT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Kg with Addition", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Kg", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("RFT percentage on volume", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #endregion
            
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);


            var data = _oRptProductionCostAnalysiss.GroupBy(x => new { x.ShadeName }, (key, grp) => new
            {
                ShadeName = key.ShadeName,
                TotalBatchWithRft = grp.ToList().Where(x=>x.IsReDyeing==false).Count(),
                TotalBatchWithAddition = grp.ToList().Where(x => x.AddCount>0).Count(),
                TotalKgWithRft = grp.ToList().Where(x=>x.IsReDyeing == false).Sum(x => x.Qty),
                TotalKgWithAddition = grp.ToList().Where(x => x.AddCount>1).Sum(x => x.Qty),
                Results = grp.ToList()
   
            });
            double rftPercentageOnBatch = 0, rftPercentageOnVolume = 0;
            _TotalBatchRFT = 0; _TotalBatchWidthAddition = 0; _TotalBatch = 0; _TotalRFTPercentageOnBatch = 0;
            _TotalKgWithRFT = 0; _TotalkgWithAddition = 0; _TotalKG = 0; _TotalRFTPercentageVolume = 0;
            foreach (var oData in data)
            {
                double totalBatch = oData.TotalBatchWithRft + oData.TotalBatchWithAddition;
                double totalKG = oData.TotalKgWithRft + oData.TotalKgWithAddition;
                _oPdfPCell = new PdfPCell(new Phrase(oData.ShadeName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oData.TotalBatchWithRft.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oData.TotalBatchWithAddition.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(totalBatch.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                rftPercentageOnBatch =(oData.TotalBatchWithRft/totalBatch)*100; 
                _oPdfPCell = new PdfPCell(new Phrase( rftPercentageOnBatch.ToString("#,##0.00;(#,##0.00)")+"%", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oData.TotalKgWithRft.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oData.TotalKgWithAddition.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(totalKG.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                rftPercentageOnVolume =((oData.TotalKgWithRft / totalKG) * 100);
                _oPdfPCell = new PdfPCell(new Phrase(rftPercentageOnVolume.ToString("#,##0.00;(#,##0.00)")+"%", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
             
                _TotalBatchRFT += oData.TotalBatchWithRft;
                _TotalBatchWidthAddition += oData.TotalBatchWithAddition;
                _TotalBatch += totalBatch;
                _TotalRFTPercentageOnBatch+=rftPercentageOnBatch;
                _TotalKgWithRFT +=oData.TotalKgWithRft;
                _TotalkgWithAddition += oData.TotalBatchWithAddition;
                _TotalKG +=totalKG;
                _TotalRFTPercentageVolume +=rftPercentageOnVolume;

            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalBatchRFT.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

              _oPdfPCell = new PdfPCell(new Phrase( _TotalBatchWidthAddition.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
              _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

              _oPdfPCell = new PdfPCell(new Phrase(_TotalBatch.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
              _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

              _oPdfPCell = new PdfPCell(new Phrase(_TotalRFTPercentageOnBatch.ToString("#,##0.00;(#,##0.00)")+"%", _oFontStyle));
              _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

              _oPdfPCell = new PdfPCell(new Phrase(_TotalKgWithRFT.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
              _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

              _oPdfPCell = new PdfPCell(new Phrase(_TotalkgWithAddition.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
              _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

              _oPdfPCell = new PdfPCell(new Phrase(_TotalKG.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
              _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

              _oPdfPCell = new PdfPCell(new Phrase(_TotalRFTPercentageVolume.ToString("#,##0.00;(#,##0.00)")+"%", _oFontStyle));
              _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);          
               oTopTable.CompleteRow();

               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 9; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                oTopTable.CompleteRow();

            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        #endregion

        #region TOP 2
        private void GetTopTable2()
        {
            PdfPTable oTopTable = new PdfPTable(9);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                                             
                                                150f,//Shade Name                                                 
                                                100f, //batch with rft
                                                100f, //overall rft                                                
                                                100f, // bulk1
                                                100f, //sample1                                                  
                                                100f,//bulk2 
                                                100f, //sample2
                                                100f, //bulk3                                                 
                                                100f //sample3                                          
                                            });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Shade Name", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Overall RFT ", _oFontStyle)); _oPdfPCell.Rowspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Overall RFT Ref - 01 ", _oFontStyle)); _oPdfPCell.Rowspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);


            #region 2nd head
            if(_oRptProductionCostAnalysiss.Count>0)
            {
                sMonth = _oRptProductionCostAnalysiss[0].EndTime.ToString("MMMM yy");
            }

            _oPdfPCell = new PdfPCell(new Phrase(sMonth+" Percentage In Details", _oFontStyle)); _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Overall", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("In-house", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Out-house", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Bulk", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Sample", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Bulk", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Sample", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Bulk", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Sample", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);          
            oTopTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);


            var data = _oRptProductionCostAnalysiss.GroupBy(x => new { x.ShadeName }, (key, grp) => new
            {
                ShadeName = key.ShadeName,
                TotalInHouseBulk = grp.ToList().Where(x => x.IsInHouse == true && x.OrderType == (int)EnumOrderType.SaleOrder_Two).Count(),
                TotalInHouseSample = grp.ToList().Where(x => x.IsInHouse == true && (x.OrderType == (int)EnumOrderType.SampleOrder_Two || x.OrderType == (int)EnumOrderType.ClaimOrder)).Count(),

                TotalOutHouseBulk = grp.ToList().Where(x => x.IsInHouse == false && x.OrderType == (int)EnumOrderType.SaleOrder).Count(),
                TotalOutHouseSample = grp.ToList().Where(x => x.IsInHouse == false && x.OrderType == (int)EnumOrderType.SampleOrder || x.OrderType == (int)EnumOrderType.ClaimOrder).Count(),

                OverallBulk = grp.ToList().Where(x => x.OrderType == (int)EnumOrderType.SaleOrder || x.OrderType == (int)EnumOrderType.SaleOrder_Two).Count(),
                OverallSample = grp.ToList().Where(x => x.OrderType == (int)EnumOrderType.SampleOrder || x.OrderType == (int)EnumOrderType.SampleOrder_Two).Count(),

                TotalBatchWithRft = grp.ToList().Where(x => x.IsReDyeing == false).Count(),
                TotalBatchWithAddition = grp.ToList().Where(x => x.AddCount> 0).Count(),
                TotalKgWithRft = grp.ToList().Where(x => x.IsReDyeing == false).Sum(x => x.Qty),
                TotalKgWithAddition = grp.ToList().Where(x => x.AddCount > 1).Sum(x => x.Qty),
                Results = grp.ToList().Count(),

            });
            double rftPercentageOnBatch = 0;
            _TotalBatchRFT = 0; _TotalBatchWidthAddition = 0; _TotalBatch = 0; _TotalRFTPercentageOnBatch = 0;
            _TotalKgWithRFT = 0; _TotalkgWithAddition = 0; _TotalKG = 0; _TotalRFTPercentageVolume = 0;
           
            foreach (var oData in data)
            {
                double totalData = oData.Results;
                double totalBatch = oData.TotalBatchWithRft + oData.TotalBatchWithAddition;
                double totalKG = oData.TotalKgWithRft + oData.TotalKgWithAddition;
                _oPdfPCell = new PdfPCell(new Phrase(oData.ShadeName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                rftPercentageOnBatch = (oData.TotalBatchWithRft/totalBatch) * 100;
                _oPdfPCell = new PdfPCell(new Phrase(rftPercentageOnBatch.ToString("#,##0.00;(#,##0.00)") + "%", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);


                double OverallBulk = (oData.OverallBulk / totalData) * 100;
                _oPdfPCell = new PdfPCell(new Phrase(OverallBulk.ToString("#,##0.00;(#,##0.00)") + "%", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(((oData.OverallSample / totalData) * 100).ToString("#,##0.00;(#,##0.00)") + "%", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(((oData.TotalInHouseBulk / totalData) * 100).ToString("#,##0.00;(#,##0.00)") + "%", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(((oData.TotalInHouseSample / totalData) * 100).ToString("#,##0.00;(#,##0.00)") + "%", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(((oData.TotalOutHouseBulk / totalData) * 100).ToString("#,##0.00;(#,##0.00)") + "%", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(((oData.TotalOutHouseSample / totalData) * 100).ToString("#,##0.00;(#,##0.00)") + "%", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                           
                //_TotalBatchRFT += oData.TotalBatchWithRft;
                //_TotalBatchWidthAddition += oData.TotalBatchWithAddition;
                //_TotalBatch += totalBatch;
                //_TotalRFTPercentageOnBatch += rftPercentageOnBatch;
                //_TotalKgWithRFT += oData.TotalKgWithRft;
                //_TotalkgWithAddition += oData.TotalBatchWithAddition;
                //_TotalKG += totalKG;
                //_TotalRFTPercentageVolume += rftPercentageOnVolume;

            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Including Average(White): ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalBatchRFT.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalBatchWidthAddition.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalBatch.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalRFTPercentageOnBatch.ToString("#,##0.00;(#,##0.00)") + "%", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalKgWithRFT.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalkgWithAddition.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalKG.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalRFTPercentageVolume.ToString("#,##0.00;(#,##0.00)") + "%", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);          
            _oPdfPCell = new PdfPCell(new Phrase("Excluding Average(White): ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalBatchRFT.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalBatchWidthAddition.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalBatch.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalRFTPercentageOnBatch.ToString("#,##0.00;(#,##0.00)") + "%", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalKgWithRFT.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalkgWithAddition.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalKG.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TotalRFTPercentageVolume.ToString("#,##0.00;(#,##0.00)") + "%", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 9; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 9; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();

            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        #endregion

        #region TOP TABLE
        private void GetsInHouseOutSide(List<RptProductionCostAnalysis> oRptProductionCostAnalysis, string sHeader)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);


            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] {  150f,//Shade Name                                                 
                                                100f, //batch with rft
                                                100f, //batch with addition                                                 
                                                100f, // total batch
                                                100f, //rft batch with percentage                                                 
                                                100f,//ky with rft 
                                                100f, //kg with addition
                                                100f, //totalKG                                                 
                                                100f // RFT PERCENTAGE WITH VOLUME  
            });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 9, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD));
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sHeader, 0, 9, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD));
            oPdfPTable.CompleteRow();


            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shade Name", 3, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Overall RFT", 3, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Overall RFT", 3, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            if (_oRptProductionCostAnalysiss.Count > 0)
            {
                sMonth = _oRptProductionCostAnalysiss[0].EndTime.ToString("MMMM yy");
            }
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sMonth + "Percentage In Details", 0, 6, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Overall", 0, 2, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "In-house", 0, 2, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Out-house", 0, 2, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
           
            oPdfPTable.CompleteRow();

            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, sMonth + "Percentage In Details", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Bulk", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sample", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Bulk", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sample", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Bulk", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sample", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            var data = oRptProductionCostAnalysis.GroupBy(x => new { x.ShadeName }, (key, grp) => new
            {
                ShadeName = key.ShadeName,
                NoRftBulkInHouse = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0 && x.IsInHouse == true && (x.OrderType == (int)EnumOrderType.BulkOrder || x.OrderType == (int)EnumOrderType.SaleOrder || x.OrderType == (int)EnumOrderType.SaleOrder_Two)).Sum(x => x.Qty),
                TotalBulkInHouse = grp.ToList().Where(x => x.IsReDyeing == false && x.IsInHouse == true && (x.OrderType == (int)EnumOrderType.BulkOrder || x.OrderType == (int)EnumOrderType.SaleOrder || x.OrderType == (int)EnumOrderType.SaleOrder_Two)).Sum(x => x.Qty),
                NoRftSampleInHouse = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0 && x.IsInHouse == true && ( x.OrderType == (int)EnumOrderType.SampleOrder || x.OrderType == (int)EnumOrderType.SampleOrder_Two)).Sum(x => x.Qty),
                TotalSampleInHouse = grp.ToList().Where(x => x.IsReDyeing == false && x.IsInHouse == true && (x.OrderType == (int)EnumOrderType.SampleOrder || x.OrderType == (int)EnumOrderType.SampleOrder_Two)).Sum(x => x.Qty),

                NoRftBulkOutSide = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0 && x.IsInHouse == false && x.OrderType == (int)EnumOrderType.BulkOrder || x.OrderType == (int)EnumOrderType.SaleOrder || x.OrderType == (int)EnumOrderType.SaleOrder_Two).Sum(x => x.Qty),
                TotalBulkOutSide = grp.ToList().Where(x => x.IsReDyeing == false && x.IsInHouse == false && (x.OrderType == (int)EnumOrderType.BulkOrder || x.OrderType == (int)EnumOrderType.SaleOrder || x.OrderType == (int)EnumOrderType.SaleOrder_Two)).Sum(x => x.Qty),
                NoRftSampleOutSide = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0 && x.IsInHouse == false && (x.OrderType == (int)EnumOrderType.SampleOrder || x.OrderType == (int)EnumOrderType.SampleOrder_Two)).Sum(x => x.Qty),
                TotalSampleOutSide = grp.ToList().Where(x => x.IsReDyeing == false && x.IsInHouse == false &&( x.OrderType == (int)EnumOrderType.SampleOrder || x.OrderType == (int)EnumOrderType.SampleOrder_Two)).Sum(x => x.Qty),

            });

            data = data.OrderByDescending(x => x.ShadeName).ToList();

            //double nPercentage = 0;
            foreach (var oData in data)
            {
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] {  150f,//Shade Name                                                 
                                                100f, //batch with rft
                                                100f, //batch with addition                                                 
                                                100f, // total batch
                                                100f, //rft batch with percentage                                                 
                                                100f,//ky with rft 
                                                100f, //kg with addition
                                                100f, //totalKG                                                 
                                                100f // RFT PERCENTAGE WITH VOLUME  
                });

                //nSL++;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.ShadeName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.NoRftBulkInHouse + oData.NoRftBulkOutSide) == 0) ? "" : (((oData.NoRftBulkInHouse + oData.NoRftBulkOutSide) * 100) / (oData.TotalBulkInHouse + oData.TotalBulkOutSide)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.NoRftSampleInHouse + oData.NoRftSampleOutSide) == 0) ? "" : (((oData.NoRftSampleInHouse + oData.NoRftSampleOutSide) * 100) / (oData.TotalSampleInHouse + oData.TotalSampleOutSide)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftBulkInHouse == 0) ? "" : (((oData.NoRftBulkInHouse) * 100) / (oData.TotalBulkInHouse)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                //nPercentage = (oData.WithRftCount * 100.0 / (oData.NoRftCount + oData.WithRftCount));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftSampleInHouse == 0) ? "" : (((oData.NoRftSampleInHouse) * 100) / (oData.TotalSampleInHouse)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftBulkOutSide == 0) ? "" : (((oData.NoRftBulkOutSide) * 100) / (oData.TotalBulkOutSide)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftSampleOutSide==0)?"":(((oData.NoRftSampleOutSide) * 100) / (oData.TotalSampleOutSide)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


            }

            var dataTotal = oRptProductionCostAnalysis.GroupBy(x => new { }, (key, grp) => new
            {
                NoRftBulkInHouse = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0 && x.IsInHouse == true && (x.OrderType == (int)EnumOrderType.BulkOrder || x.OrderType == (int)EnumOrderType.SaleOrder || x.OrderType == (int)EnumOrderType.SaleOrder_Two)).Sum(x => x.Qty),
                TotalBulkInHouse = grp.ToList().Where(x => x.IsReDyeing == false && x.IsInHouse == true && (x.OrderType == (int)EnumOrderType.BulkOrder || x.OrderType == (int)EnumOrderType.SaleOrder || x.OrderType == (int)EnumOrderType.SaleOrder_Two)).Sum(x => x.Qty),
                NoRftSampleInHouse = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0 && x.IsInHouse == true && (x.OrderType == (int)EnumOrderType.SampleOrder || x.OrderType == (int)EnumOrderType.SampleOrder_Two)).Sum(x => x.Qty),
                TotalSampleInHouse = grp.ToList().Where(x => x.IsReDyeing == false && x.IsInHouse == true && (x.OrderType == (int)EnumOrderType.SampleOrder || x.OrderType == (int)EnumOrderType.SampleOrder_Two)).Sum(x => x.Qty),

                NoRftBulkOutSide = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0 && x.IsInHouse == false && x.OrderType == (int)EnumOrderType.BulkOrder || x.OrderType == (int)EnumOrderType.SaleOrder || x.OrderType == (int)EnumOrderType.SaleOrder_Two).Sum(x => x.Qty),
                TotalBulkOutSide = grp.ToList().Where(x => x.IsReDyeing == false && x.IsInHouse == false && (x.OrderType == (int)EnumOrderType.BulkOrder || x.OrderType == (int)EnumOrderType.SaleOrder || x.OrderType == (int)EnumOrderType.SaleOrder_Two)).Sum(x => x.Qty),
                NoRftSampleOutSide = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0 && x.IsInHouse == false && (x.OrderType == (int)EnumOrderType.SampleOrder || x.OrderType == (int)EnumOrderType.SampleOrder_Two)).Sum(x => x.Qty),
                TotalSampleOutSide = grp.ToList().Where(x => x.IsReDyeing == false && x.IsInHouse == false && (x.OrderType == (int)EnumOrderType.SampleOrder || x.OrderType == (int)EnumOrderType.SampleOrder_Two)).Sum(x => x.Qty),


            });

            foreach (var oData in dataTotal)
            {
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] {  150f,//Shade Name                                                 
                                                100f, //batch with rft
                                                100f, //batch with addition                                                 
                                                100f, // total batch
                                                100f, //rft batch with percentage                                                 
                                                100f,//ky with rft 
                                                100f, //kg with addition
                                                100f, //totalKG                                                 
                                                100f // RFT PERCENTAGE WITH VOLUME  
                });

                //nSL++;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.NoRftBulkInHouse + oData.NoRftBulkOutSide) == 0) ? "" : (((oData.NoRftBulkInHouse + oData.NoRftBulkOutSide) * 100) / (oData.TotalBulkInHouse + oData.TotalBulkOutSide)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.NoRftSampleInHouse + oData.NoRftSampleOutSide) == 0) ? "" : (((oData.NoRftSampleInHouse + oData.NoRftSampleOutSide) * 100) / (oData.TotalSampleInHouse + oData.TotalSampleOutSide)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftBulkInHouse == 0) ? "" : (((oData.NoRftBulkInHouse) * 100) / (oData.TotalBulkInHouse)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                //nPercentage = (oData.WithRftCount * 100.0 / (oData.NoRftCount + oData.WithRftCount));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftSampleInHouse == 0) ? "" : (((oData.NoRftSampleInHouse) * 100) / (oData.TotalSampleInHouse)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftBulkOutSide == 0) ? "" : (((oData.NoRftBulkOutSide) * 100) / (oData.TotalBulkOutSide)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftSampleOutSide == 0) ? "" : (((oData.NoRftSampleOutSide) * 100) / (oData.TotalSampleOutSide)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            }

            var data_ExcludingWhite = oRptProductionCostAnalysis.GroupBy(x => new { }, (key, grp) => new
            {
             

                NoRftBulkInHouse = grp.ToList().Where(x => x.IsReDyeing == false && x.ShadeName != "White" && x.AddCount > 0 && x.IsInHouse == true && (x.OrderType == (int)EnumOrderType.BulkOrder || x.OrderType == (int)EnumOrderType.SaleOrder || x.OrderType == (int)EnumOrderType.SaleOrder_Two)).Sum(x => x.Qty),
                TotalBulkInHouse = grp.ToList().Where(x => x.IsReDyeing == false && x.ShadeName != "White" && x.IsInHouse == true && (x.OrderType == (int)EnumOrderType.BulkOrder || x.OrderType == (int)EnumOrderType.SaleOrder || x.OrderType == (int)EnumOrderType.SaleOrder_Two)).Sum(x => x.Qty),
                NoRftSampleInHouse = grp.ToList().Where(x => x.IsReDyeing == false && x.ShadeName != "White" && x.AddCount > 0 && x.IsInHouse == true && (x.OrderType == (int)EnumOrderType.SampleOrder || x.OrderType == (int)EnumOrderType.SampleOrder_Two)).Sum(x => x.Qty),
                TotalSampleInHouse = grp.ToList().Where(x => x.IsReDyeing == false && x.ShadeName != "White" && x.IsInHouse == true && (x.OrderType == (int)EnumOrderType.SampleOrder || x.OrderType == (int)EnumOrderType.SampleOrder_Two)).Sum(x => x.Qty),

                NoRftBulkOutSide = grp.ToList().Where(x => x.IsReDyeing == false && x.ShadeName != "White" && x.AddCount > 0 && x.IsInHouse == false && x.OrderType == (int)EnumOrderType.BulkOrder || x.OrderType == (int)EnumOrderType.SaleOrder || x.OrderType == (int)EnumOrderType.SaleOrder_Two).Sum(x => x.Qty),
                TotalBulkOutSide = grp.ToList().Where(x => x.IsReDyeing == false && x.ShadeName != "White" && x.IsInHouse == false && (x.OrderType == (int)EnumOrderType.BulkOrder || x.OrderType == (int)EnumOrderType.SaleOrder || x.OrderType == (int)EnumOrderType.SaleOrder_Two)).Sum(x => x.Qty),
                NoRftSampleOutSide = grp.ToList().Where(x => x.IsReDyeing == false && x.ShadeName != "White" && x.AddCount > 0 && x.IsInHouse == false && (x.OrderType == (int)EnumOrderType.SampleOrder || x.OrderType == (int)EnumOrderType.SampleOrder_Two)).Sum(x => x.Qty),
                TotalSampleOutSide = grp.ToList().Where(x => x.IsReDyeing == false && x.ShadeName != "White" && x.IsInHouse == false && (x.OrderType == (int)EnumOrderType.SampleOrder || x.OrderType == (int)EnumOrderType.SampleOrder_Two)).Sum(x => x.Qty),



            });

            foreach (var oData in data_ExcludingWhite)
            {
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] {  150f,//Shade Name                                                 
                                                100f, //batch with rft
                                                100f, //batch with addition                                                 
                                                100f, // total batch
                                                100f, //rft batch with percentage                                                 
                                                100f,//ky with rft 
                                                100f, //kg with addition
                                                100f, //totalKG                                                 
                                                100f // RFT PERCENTAGE WITH VOLUME  
                });

                //nSL++;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Excluding White", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.NoRftBulkInHouse + oData.NoRftBulkOutSide) == 0) ? "" : (((oData.NoRftBulkInHouse + oData.NoRftBulkOutSide) * 100) / (oData.TotalBulkInHouse + oData.TotalBulkOutSide)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.NoRftSampleInHouse + oData.NoRftSampleOutSide) == 0) ? "" : (((oData.NoRftSampleInHouse + oData.NoRftSampleOutSide) * 100) / (oData.TotalSampleInHouse + oData.TotalSampleOutSide)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftBulkInHouse == 0) ? "" : (((oData.NoRftBulkInHouse) * 100) / (oData.TotalBulkInHouse)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                //nPercentage = (oData.WithRftCount * 100.0 / (oData.NoRftCount + oData.WithRftCount));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftSampleInHouse == 0) ? "" : (((oData.NoRftSampleInHouse) * 100) / (oData.TotalSampleInHouse)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftBulkOutSide == 0) ? "" : (((oData.NoRftBulkOutSide) * 100) / (oData.TotalBulkOutSide)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftSampleOutSide == 0) ? "" : (((oData.NoRftSampleOutSide) * 100) / (oData.TotalSampleOutSide)).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            }
        }
        private void GetsAllOver(List<RptProductionCostAnalysis> oRptProductionCostAnalysis, string sHeader)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);


            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] {  150f,//Shade Name                                                 
                                                100f, //batch with rft
                                                100f, //batch with addition                                                 
                                                100f, // total batch
                                                100f, //rft batch with percentage                                                 
                                                100f,//ky with rft 
                                                100f, //kg with addition
                                                100f, //totalKG                                                 
                                                100f // RFT PERCENTAGE WITH VOLUME  
            });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 9, Element.ALIGN_CENTER, BaseColor.WHITE, false, 20, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD));
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sHeader, 0, 9, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD));
            oPdfPTable.CompleteRow();


            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shade Name", 2, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Overall Bulk & Sample\n(Based on Batches - Number)", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Overall Bulk & Sample\n(Based on Volume -kg)", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            //if (_oRptProductionCostAnalysiss.Count > 0)
            //{
            //    sMonth = _oRptProductionCostAnalysiss[0].EndTime.ToString("MMMM yy");
            //}
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, sMonth + "Percentage In Details", 0, 6, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            //oPdfPTable.CompleteRow();

            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Overall", 0, 2, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "In-house", 0, 2, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Out-house", 0, 2, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);

            //oPdfPTable.CompleteRow();

            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, sMonth + "Percentage In Details", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch with RFT", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch with Addition", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Batch", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "RFT percentage on Batch", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Kg With RFT", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Kg with Addition", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Kg Total Kg", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "RFT percentage on volume", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            

            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            var data = oRptProductionCostAnalysis.GroupBy(x => new { x.ShadeName }, (key, grp) => new
            {
                ShadeName = key.ShadeName,
                NoRftCount = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0).Count(),
                WithRftCount = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount == 0).Count(),
                NoRftQty = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0).Sum(x => x.Qty),
                WithRftQty = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount == 0).Sum(x => x.Qty),

            });

            data = data.OrderByDescending(x => x.ShadeName).ToList();

            double nPercentage = 0;
            foreach (var oData in data)
            {
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] {  150f,//Shade Name                                                 
                                                100f, //batch with rft
                                                100f, //batch with addition                                                 
                                                100f, // total batch
                                                100f, //rft batch with percentage                                                 
                                                100f,//ky with rft 
                                                100f, //kg with addition
                                                100f, //totalKG                                                 
                                                100f // RFT PERCENTAGE WITH VOLUME  
                });

                //nSL++;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.ShadeName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
              
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.WithRftCount) == 0) ? "" : (oData.WithRftCount).ToString("#,##0.00;(#,##0.00)") , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.NoRftCount) == 0) ? "" : (oData.NoRftCount).ToString("#,##0.00;(#,##0.00)") , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.WithRftCount+oData.NoRftCount) == 0) ? "" : (oData.WithRftCount+oData.NoRftCount).ToString("#,##0.00;(#,##0.00)") , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                 nPercentage = (oData.WithRftCount*100.0 / (oData.NoRftCount + oData.WithRftCount)) ;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nPercentage == 0) ? "" : (nPercentage).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftQty == 0) ? "" : ((oData.NoRftQty).ToString("#,##0.00;(#,##0.00)")) , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.WithRftQty == 0) ? "" : ((oData.WithRftQty).ToString("#,##0.00;(#,##0.00)")) , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftQty + oData.WithRftQty == 0) ? "" : ((oData.NoRftQty + oData.WithRftQty).ToString("#,##0.00;(#,##0.00)")), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                 nPercentage = ((oData.WithRftQty*100.00 / (oData.NoRftQty + oData.WithRftQty)));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nPercentage == 0) ? "" : (nPercentage).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);


                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


            }

            var dataTotal = oRptProductionCostAnalysis.GroupBy(x => new { }, (key, grp) => new
            {
                 NoRftCount = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0).Count(),
                WithRftCount = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount == 0).Count(),
                NoRftQty = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0).Sum(x => x.Qty),
                WithRftQty = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount == 0).Sum(x => x.Qty),

            });

            foreach (var oData in dataTotal)
            {
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] {  150f,//Shade Name                                                 
                                                100f, //batch with rft
                                                100f, //batch with addition                                                 
                                                100f, // total batch
                                                100f, //rft batch with percentage                                                 
                                                100f,//ky with rft 
                                                100f, //kg with addition
                                                100f, //totalKG                                                 
                                                100f // RFT PERCENTAGE WITH VOLUME  
                });

                //nSL++;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.WithRftCount) == 0) ? "" : ((oData.WithRftCount).ToString("#,##0.00;(#,##0.00)")) , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.NoRftCount) == 0) ? "" : (oData.NoRftCount).ToString("#,##0.00;(#,##0.00)") , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.WithRftCount+oData.NoRftCount) == 0) ? "" : (oData.WithRftCount+oData.NoRftCount).ToString("#,##0.00;(#,##0.00)") , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                 nPercentage = (oData.WithRftCount*100.0 / (oData.NoRftCount + oData.WithRftCount)) ;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nPercentage == 0) ? "" : (nPercentage).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftQty == 0) ? "" : ((oData.NoRftQty).ToString("#,##0.00;(#,##0.00)")) , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.WithRftQty == 0) ? "" : ((oData.WithRftQty).ToString("#,##0.00;(#,##0.00)")) , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.NoRftQty + oData.WithRftQty) == 0) ? "" : ((oData.NoRftQty + oData.WithRftQty).ToString("#,##0.00;(#,##0.00)")), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                 nPercentage = ((oData.WithRftQty*100.00 / (oData.NoRftQty + oData.WithRftQty)));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nPercentage == 0) ? "" : (nPercentage).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);


                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            }

            var data_ExcludingWhite = oRptProductionCostAnalysis.GroupBy(x => new { }, (key, grp) => new
            {
                  NoRftCount = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0 && x.ShadeName!="White").Count(),
                WithRftCount = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount == 0 && x.ShadeName != "White").Count(),
                NoRftQty = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0 && x.ShadeName != "White").Sum(x => x.Qty),
                WithRftQty = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount == 0 && x.ShadeName != "White").Sum(x => x.Qty),


            });

            foreach (var oData in data_ExcludingWhite)
            {
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] {  150f,//Shade Name                                                 
                                                100f, //batch with rft
                                                100f, //batch with addition                                                 
                                                100f, // total batch
                                                100f, //rft batch with percentage                                                 
                                                100f,//ky with rft 
                                                100f, //kg with addition
                                                100f, //totalKG                                                 
                                                100f // RFT PERCENTAGE WITH VOLUME  
                });

                //nSL++;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Excluding White", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
              
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.WithRftCount) == 0) ? "" : (oData.WithRftCount).ToString("#,##0.00;(#,##0.00)") , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.NoRftCount) == 0) ? "" : (oData.NoRftCount).ToString("#,##0.00;(#,##0.00)") , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.WithRftCount+oData.NoRftCount) == 0) ? "" : (oData.WithRftCount+oData.NoRftCount).ToString("#,##0.00;(#,##0.00)") , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                 nPercentage = (oData.WithRftCount*100.0 / (oData.NoRftCount + oData.WithRftCount)) ;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nPercentage == 0) ? "" : (nPercentage).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftQty == 0) ? "" : ((oData.NoRftQty).ToString("#,##0.00;(#,##0.00)")) , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.WithRftQty == 0) ? "" : ((oData.WithRftQty).ToString("#,##0.00;(#,##0.00)")) , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oData.NoRftQty + oData.WithRftQty) == 0) ? "" : ((oData.NoRftQty + oData.WithRftQty).ToString("#,##0.00;(#,##0.00)")), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                 nPercentage = ((oData.WithRftQty*100.00 / (oData.NoRftQty + oData.WithRftQty)));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nPercentage == 0) ? "" : (nPercentage).ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            }
        }
        #endregion

        #region TOP TABLE
        private void GetsBulk(List<RptProductionCostAnalysis> oRptProductionCostAnalysis , string sHeader)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);


          PdfPTable  oPdfPTable = new PdfPTable(9);
           oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] {  150f,//Shade Name                                                 
                                                100f, //batch with rft
                                                100f, //batch with addition                                                 
                                                100f, // total batch
                                                100f, //rft batch with percentage                                                 
                                                100f,//ky with rft 
                                                100f, //kg with addition
                                                100f, //totalKG                                                 
                                                100f // RFT PERCENTAGE WITH VOLUME  
            });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 9, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD));
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sHeader, 0, 9, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD));
            oPdfPTable.CompleteRow();
           

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shade Name", 2, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Overall " + sHeader + "\n(Based on Batches - Number)", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Overall " + sHeader + "\n(Based on Volume -kg)", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "No", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yes", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Percentage", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "No", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yes", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Percentage", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            var data = oRptProductionCostAnalysis.GroupBy(x => new { x.ShadeName }, (key, grp) => new
            {
                ShadeName = key.ShadeName,
                NoRftCount = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount>0).Count(),
                WithRftCount = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount == 0).Count(),
                NoRftQty = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount>0).Sum(x => x.Qty),
                WithRftQty = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount == 0).Sum(x => x.Qty),
                Results = grp.ToList()

            });

            data=data.OrderByDescending(x=>x.ShadeName).ToList();

            double nPercentage = 0;
            foreach (var oData in data)
            {
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] {  150f,//Shade Name                                                 
                                                100f, //batch with rft
                                                100f, //batch with addition                                                 
                                                100f, // total batch
                                                100f, //rft batch with percentage                                                 
                                                100f,//ky with rft 
                                                100f, //kg with addition
                                                100f, //totalKG                                                 
                                                100f // RFT PERCENTAGE WITH VOLUME  
                });

                //nSL++;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.ShadeName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.NoRftCount.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.WithRftCount.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftCount + oData.WithRftCount).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                nPercentage = (oData.WithRftCount*100.0 / (oData.NoRftCount + oData.WithRftCount)) ;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, nPercentage.ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.NoRftQty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.WithRftQty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftQty + oData.WithRftQty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                nPercentage = ((oData.WithRftQty*100.00 / (oData.NoRftQty + oData.WithRftQty)));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, nPercentage.ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
              

            }

            var dataTotal = oRptProductionCostAnalysis.GroupBy(x => new { }, (key, grp) => new
            {
             
                NoRftCount = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount>0).Count(),
                WithRftCount = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount == 0).Count(),
                NoRftQty = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0).Sum(x => x.Qty),
                WithRftQty = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount == 0).Sum(x => x.Qty),
                Results = grp.ToList()

            });
          
            foreach (var oData in dataTotal)
            {
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] {  150f,//Shade Name                                                 
                                                100f, //batch with rft
                                                100f, //batch with addition                                                 
                                                100f, // total batch
                                                100f, //rft batch with percentage                                                 
                                                100f,//ky with rft 
                                                100f, //kg with addition
                                                100f, //totalKG                                                 
                                                100f // RFT PERCENTAGE WITH VOLUME  
                });

                //nSL++;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.NoRftCount.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.WithRftCount.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftCount + oData.WithRftCount).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                nPercentage = (oData.WithRftCount*100.00 / (oData.NoRftCount + oData.WithRftCount)) ;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, nPercentage.ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.NoRftQty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.WithRftQty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftQty + oData.WithRftQty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                nPercentage = ((oData.WithRftQty*100.00 / (oData.NoRftQty + oData.WithRftQty)));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, nPercentage.ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
          
            }

            var data_ExcludingWhite = oRptProductionCostAnalysis.GroupBy(x => new { }, (key, grp) => new
            {

                NoRftCount = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0 && x.ShadeName!="White").Count(),
                WithRftCount = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount == 0 && x.ShadeName != "White").Count(),
                NoRftQty = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount > 0 && x.ShadeName != "White").Sum(x => x.Qty),
                WithRftQty = grp.ToList().Where(x => x.IsReDyeing == false && x.AddCount == 0 && x.ShadeName != "White").Sum(x => x.Qty),
                Results = grp.ToList()

            });

            foreach (var oData in data_ExcludingWhite)
            {
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] {  150f,//Shade Name                                                 
                                                100f, //batch with rft
                                                100f, //batch with addition                                                 
                                                100f, // total batch
                                                100f, //rft batch with percentage                                                 
                                                100f,//ky with rft 
                                                100f, //kg with addition
                                                100f, //totalKG                                                 
                                                100f // RFT PERCENTAGE WITH VOLUME  
                });

                //nSL++;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Excluding White", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.NoRftCount.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.WithRftCount.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftCount + oData.WithRftCount).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                nPercentage = (oData.WithRftCount * 100.00 / (oData.NoRftCount + oData.WithRftCount));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, nPercentage.ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.NoRftQty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.WithRftQty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oData.NoRftQty + oData.WithRftQty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                nPercentage = ((oData.WithRftQty * 100.00 / (oData.NoRftQty + oData.WithRftQty)));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, nPercentage.ToString("#,##0.00;(#,##0.00)") + "%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            }
        }
        #endregion

    }
}
