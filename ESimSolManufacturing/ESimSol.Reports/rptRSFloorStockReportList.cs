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
    public class rptRSFloorStockReportList
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);//number of columns
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        RSFreshDyedYarn _oRSFreshDyedYarn = new RSFreshDyedYarn();
        List<RSFreshDyedYarn> _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
        Company _oCompany = new Company();
      
        #endregion
        public byte[] PrepareReport(List<RSFreshDyedYarn> oRSFreshDyedYarns, Company oCompany)
        {
            _oRSFreshDyedYarns = oRSFreshDyedYarns;
            _oCompany = oCompany;
            #region Page Setup
            _oDocument = new Document(PageSize.LEGAL, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion
            //this.PrintHeader();
           
            this.ReportHeader();
         
            //_oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
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
            #endregion
            #region Blank Space
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 95f, 270.5f, 95f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
             _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Floor Stock Report", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

              _oPdfPCell = new PdfPCell(oPdfPTable);
              _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.FixedHeight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }

      

        private void HeadTable()
        {
            PdfPTable oTopTable = new PdfPTable(4);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                                             
                                                50f,                                                  
                                                50f,
                                                50f,
                                                100f
                                            });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("In House: " + _oRSFreshDyedYarns.Where(x => x.IsInHouse == true).Sum(x => x.Qty_RS).ToString("#,##0.00;(#,##0.00)")+" kg", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Commission: " + _oRSFreshDyedYarns.Where(x => x.IsInHouse == false).Sum(x => x.Qty_RS).ToString("#,##0.00;(#,##0.00)") + " kg", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total: " + _oRSFreshDyedYarns.Sum(x => x.Qty_RS).ToString("#,##0.00;(#,##0.00)") + " kg", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLDITALIC);
            _oPdfPCell = new PdfPCell(new Phrase("")); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

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


        #region Report Body
        private void ReportHeader()
        {
           
            List<RSFreshDyedYarn> oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            oRSFreshDyedYarns = _oRSFreshDyedYarns;
            oRSFreshDyedYarns = _oRSFreshDyedYarns.Where(x => x.RSState == EnumRSState.UnloadedFromDyeMachine).ToList();
            if (oRSFreshDyedYarns.Count > 0)
            {
                this.PrintHeader();
                this.HeadTable();
                this.PrintBody(oRSFreshDyedYarns, "Unloaded From DyeMachine");
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
                
            }
            oRSFreshDyedYarns = _oRSFreshDyedYarns.Where(x => x.RSState == EnumRSState.LoadedInHydro && x.RSSubStates == EnumRSSubStates.None).ToList();
            if (oRSFreshDyedYarns.Count > 0)
            {
                this.PrintHeader();
                this.HeadTable();
                this.PrintBody(oRSFreshDyedYarns, "Loaded In Hydro");
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
               
            }

            oRSFreshDyedYarns = _oRSFreshDyedYarns.Where(x => x.RSState == EnumRSState.UnloadedFromHydro && x.RSSubStates == EnumRSSubStates.None).ToList();
            if (oRSFreshDyedYarns.Count > 0)
            {
                this.PrintHeader();
                this.HeadTable();
                this.PrintBody(oRSFreshDyedYarns, "Unloaded From Hydro");
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
               
            }
            oRSFreshDyedYarns = _oRSFreshDyedYarns.Where(x => x.RSState == EnumRSState.LoadedInDrier && x.RSSubStates == EnumRSSubStates.None).ToList();
            if (oRSFreshDyedYarns.Count > 0)
            {
                this.PrintHeader();
                this.HeadTable();
                this.PrintBody(oRSFreshDyedYarns, "Loaded In Drier");
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
               
            }
            oRSFreshDyedYarns = _oRSFreshDyedYarns.Where(x => x.RSState == EnumRSState.UnLoadedFromDrier && x.RSSubStates == EnumRSSubStates.None).ToList();
            if (oRSFreshDyedYarns.Count > 0)
            {
                this.PrintHeader();
                this.HeadTable();
                this.PrintBody(oRSFreshDyedYarns, "UnLoaded In Drier");
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
                
            }

            oRSFreshDyedYarns = _oRSFreshDyedYarns.Where(x => x.RSSubStates != EnumRSSubStates.None && x.RSState!= EnumRSState.InHW_Sub_Store).ToList();
            if (oRSFreshDyedYarns.Count > 0)
            {
                this.PrintHeader();
                this.HeadTable();
                this.PrintBody(oRSFreshDyedYarns, "In Shade QC");
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
              
            }

            oRSFreshDyedYarns = _oRSFreshDyedYarns.Where(x => x.RSState == EnumRSState.InHW_Sub_Store).ToList();
            if (oRSFreshDyedYarns.Count > 0)
            {
                this.PrintHeader();
                this.HeadTable();
                this.PrintBody(oRSFreshDyedYarns, "Wating For Receive in Hard winding");
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
               
            }
         
            
        }
        #endregion
   


        private void PrintBody(List<RSFreshDyedYarn> oRSFreshDyedYarns,string sHeader)
        {
            int nSL = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC);


            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.SetWidths(new float[] { 25f, 80f, 72f, 95f, 150f, 100f, 90f, 60f, 40f, 100f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sHeader, 0, 10, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLDITALIC));
            oPdfPTable.CompleteRow();
           
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);



            var oOrderType = oRSFreshDyedYarns.GroupBy(x => new { x.IsInHouse }, (key, grp) =>
                                  new
                                  {
                                      IsInHouse = key.IsInHouse,
                                  }).ToList();


            foreach (var oItem in oOrderType)
            {
                var oRSFloorStocks = oRSFreshDyedYarns.Where(x => x.IsInHouse == oItem.IsInHouse).ToList();
                #region Fabric Info
                oPdfPTable = new PdfPTable(10);
                oPdfPTable.SetWidths(new float[] { 25f, 80f, 72f, 95f, 150f, 100f, 90f, 60f, 40f, 100f });
                if (oOrderType.Count > 0)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable,( oItem.IsInHouse)?"In House":"Out Side", 0, 10, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    oPdfPTable.CompleteRow();
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Count", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shade", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty(kg)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Pcs", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Remarks", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion
                nSL = 0;
                //oFabricSizingPlans = oFabricSizingPlans.OrderBy(o => o.Sequence).ToList();
                foreach (var oItem1 in oRSFloorStocks)
                {
                    oPdfPTable = new PdfPTable(10);
                    oPdfPTable.SetWidths(new float[] { 25f, 80f, 72f, 95f, 150f, 100f, 90f, 60f, 40f, 100f });

                    nSL++;
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSL.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.RSDateStr, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.RouteSheetNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.OrderNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ContractorName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (string.IsNullOrEmpty(oItem1.ColorName) ? "" : oItem1.ColorName), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.Qty_RS.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.BagCount.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.RSSubStatesSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }

                oPdfPTable = new PdfPTable(10);
                oPdfPTable.SetWidths(new float[] { 25f, 80f, 72f, 95f, 150f, 100f, 90f, 60f, 40f, 100f });

                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Total ", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 7, 0);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oRSFloorStocks.Sum(x => x.Qty_RS).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oRSFloorStocks.Sum(x => x.BagCount).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 0, 0);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            }

        }
    }
}

    
    
