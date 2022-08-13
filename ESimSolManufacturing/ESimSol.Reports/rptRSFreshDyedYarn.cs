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
using ICS.Core.Framework;
using System.Linq;
using System.Web;
namespace ESimSol.Reports
{
    public class rptRSFreshDyedYarn
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        public iTextSharp.text.Image _oImag { get; set; }

        PdfPTable _oPdfPTable = new PdfPTable(18);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        RSFreshDyedYarn _oRSFreshDyedYarn = new RSFreshDyedYarn();
        List<RSFreshDyedYarn> _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
        #endregion

        public byte[] PrepareReport(List<RSFreshDyedYarn> oRSFreshDyedYarns, Company oCompany, BusinessUnit oBusinessUnit, string sDateRange, string sMunit)
        {
            _oRSFreshDyedYarns = oRSFreshDyedYarns;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            #region Page Setup

            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());//842*595
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;

            //_oDocument = new Document(new iTextSharp.text.Rectangle(_npageHeight, _nPageWidth), 0f, 0f, 0f, 0f);
            //_oDocument.SetMargins(10, 10, 10, 10);
            _oPdfPTable.SetWidths(new float[] { 
                20f, //SL NO
                95f, //Buyer
                95f, //Unit
                60f, //Color
                55f, //Order No
                40, // Dye Lot No
                40, //Dyeing Qty(LBS)
                40, //Dyeing Qty(LBS)
               
                30, //Packing Qty(LBS)
                30, //Recycle
                30, //Wastage
                30, //Short Qty
                20, //Wastage
                20, //Short Qty
                20, //Adding how Many time
                30, //Re-Dyeing
                36, //D.Type
                50, //QC By Name
            });

            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            #endregion


            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, _oBusinessUnit, _oCompany, "Packing Book", 18);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, sDateRange, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 18, 10);
                
            this.PrintBody(sDateRange,sMunit);
            this.SetSummary_Order();
            this.SetSummary_WorkingUnit();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Bill
        private void PrintBody(string sDateRange, string sMunit)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma",7f, iTextSharp.text.Font.BOLD);
            
            //string Header = "Tips Bills for Bank Acceptances (Maturity Letters) collection :";
            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
            string[] Headers = new string[] {"SL. No.", 
                                             "Buyer Name", 
                                             "Yarn", 
                                             "Color", 
                                             "Order No", 
                                             "Dye Lot No", 
                                             "Dyeing Qty", 
                                             "Packing Qty", 
                                             "WIP Qty", 
                                             "Recycle Qty", 
                                             "Wastage Qty", 
                                             "Short Qty",
                                             "Gain %",
                                             "Loss %",
                                              "Add D/C ", 
                                             "Re-Dyeing", 
                                             "D.Type", 
                                             "QC By Name", 
                                                    };

            ESimSolPdfHelper.PrintHeaders(ref _oPdfPTable, Headers);
            int nCount = 1;

            double nQty = 0;
            double nQtyGain = 0;
            double nQtyLoss = 0;

            var oRSShits = _oRSFreshDyedYarns.Select(x => x.RSShiftID).Distinct();

            foreach (var oRSShit in oRSShits)
            {
                ESimSolPdfHelper.FontStyle = _oFontStyle;
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, _oRSFreshDyedYarns.Where(x=>x.RSShiftID==oRSShit).Select(x=>x.RSShiftName).FirstOrDefault(), Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 18, 8);
                
                #region Data
                ESimSolPdfHelper.FontStyle = _oFontStyle;
                foreach (var oItem in _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit))
                {
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, (nCount++).ToString(), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.ContractorName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.ColorName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.OrderNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.RouteSheetNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);

                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(oItem.Qty_RS), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(oItem.FreshDyedYarnQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(oItem.WIPQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                  
                    
                    //ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.BagCount +"", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);

                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(oItem.RecycleQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(oItem.WastageQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(oItem.Loss), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(oItem.GainPer), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(oItem.LossPer), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, (oItem.DCAddCount > 0) ? oItem.DCAddCount.ToString() : "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, ((oItem.IsReDyeing) ? "Re-Dye" : ""), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, (string.IsNullOrEmpty(oItem.DyeingType))?"": oItem.DyeingType, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.RequestByName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                }
                #endregion

                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                #region Sub Total
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Sub Total", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 6, 0);
                nQty = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.Qty_RS);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.FreshDyedYarnQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.WIPQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
             
                

                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.RecycleQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.WastageQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit && x.Loss > 0).Sum(x => (x.Loss))), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);

               
                nQtyGain = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.Gain);
                nQtyGain = nQtyGain * 100 / nQty;
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQtyGain), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                nQtyLoss = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.Loss);
                nQtyLoss = nQtyLoss * 100 / nQty;
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQtyLoss), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                #endregion
            }
            #region Total
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Total", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15,0,6,0);
            nQty = _oRSFreshDyedYarns.Sum(x => x.Qty_RS);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Sum(x => x.Qty_RS)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Sum(x => x.FreshDyedYarnQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Sum(x => x.WIPQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable,_oRSFreshDyedYarns.Sum(x => x.BagCount).ToString(), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Sum(x => x.RecycleQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Sum(x => x.WastageQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Where(x => x.Loss > 0).Sum(x => (x.Loss))), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            nQtyGain = _oRSFreshDyedYarns.Sum(x => x.Gain);
            nQtyGain = nQtyGain * 100 / nQty;
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQtyGain), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            nQtyLoss = _oRSFreshDyedYarns.Sum(x => x.Loss);
            nQtyLoss = nQtyLoss * 100 / nQty;
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQtyLoss), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            #endregion
        }

        private void SetSummary_Order()
        {
          


            PdfPTable oPdfPTable = new PdfPTable(14);
            oPdfPTable.SetWidths(new float[] { 25f, 50f, 42f, 42f, 42f, 40f, 42f,30f, 42f, 42f, 42f, 42f, 42f, 25f });

            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Summary(Order Type)", 0, 12, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL));
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion
            //foreach (var oItem in oDUOrderType)
            //{
                //List<RSFreshDyedYarn> oRSFreshDyedYarns =new List<RSFreshDyedYarn>();// _oRSFreshDyedYarns.Where(x => x.OrderType == oItem.OrderType).ToList();
                #region Fabric Info
                oPdfPTable = new PdfPTable(14);
                oPdfPTable.SetWidths(new float[] { 25f, 50f, 42f, 42f, 42f, 40f, 42f, 30f, 42f, 42f, 42f, 42f, 42f, 25f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order Type", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dyeing Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Packing Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "WIP Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Adding D/C Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Adding D/C %", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Recycle Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Wastage Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Short Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Gain", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Loss%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                //   NoOfBag = grp.Sum(p => p.NoOfBag),
                ////                            WtPerBag = grp.Sum(p => p.WtPerBag),
               var oRSFreshDyedYarns = _oRSFreshDyedYarns.GroupBy(x => new { x.OrderType, x.OrderTypeSt, x.IsReDyeing }, (key, grp) =>
                                        new 
                                        {
                                            OrderType = key.OrderType,
                                            OrderTypeSt = key.OrderTypeSt,
                                            IsReDyeing = key.IsReDyeing,
                                            Qty_RS = grp.Sum(p => p.Qty_RS),
                                            QtyDCAdd = grp.Sum(p => p.QtyDCAdd),
                                            FreshDyedYarnQty = grp.Sum(p => p.FreshDyedYarnQty),
                                            WIPQty = grp.Sum(p => p.WIPQty),
                                            RecycleQty = grp.Sum(p => p.RecycleQty),
                                            WastageQty = grp.Sum(p => p.WastageQty),
                                            Loss = grp.Sum(p => p.Loss),
                                            Gain = grp.Sum(p => p.Gain),
                                        
                                        }).ToList();
                oRSFreshDyedYarns = oRSFreshDyedYarns.OrderBy(x => x.OrderType).ToList();
                foreach (var oItem1 in oRSFreshDyedYarns)
                {
                    oPdfPTable = new PdfPTable(14);
                    oPdfPTable.SetWidths(new float[] { 25f, 50f, 42f, 42f, 42f, 40f, 42f, 30f, 42f, 42f, 42f, 42f, 42f, 25f });
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.OrderTypeSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem1.IsReDyeing) ? "Re-Dye" : ""), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Qty_RS), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.FreshDyedYarnQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.WIPQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.QtyDCAdd), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat((oItem1.QtyDCAdd / oItem1.Qty_RS)*100), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.RecycleQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.WastageQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Loss ), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Gain), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Loss * 100 / oItem1.Qty_RS), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }
              var  oRSFreshDyedYarnsTotal = _oRSFreshDyedYarns.GroupBy(x => new {  }, (key, grp) =>
                                         new 
                                         {
                                             Qty_RS = grp.Sum(p => p.Qty_RS),
                                             FreshDyedYarnQty = grp.Sum(p => p.FreshDyedYarnQty),
                                             WIPQty = grp.Sum(p => p.WIPQty),
                                             QtyDCAdd = grp.Sum(p => p.QtyDCAdd),
                                             RecycleQty = grp.Sum(p => p.RecycleQty),
                                             WastageQty = grp.Sum(p => p.WastageQty),
                                             Loss = grp.Sum(p => p.Loss),
                                             Gain = grp.Sum(p => p.Gain),

                                         }).ToList();
              foreach (var oItem1 in oRSFreshDyedYarnsTotal)
                {
                    oPdfPTable = new PdfPTable(14);
                    oPdfPTable.SetWidths(new float[] { 25f, 50f, 42f, 42f, 42f, 40f, 42f, 30f, 42f, 42f, 42f, 42f, 42f, 25f });
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Qty_RS), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                  
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.FreshDyedYarnQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.WIPQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.QtyDCAdd), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat((oItem1.QtyDCAdd / oItem1.Qty_RS) * 100), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.RecycleQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.WastageQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Loss ), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Gain), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Loss * 100 / oItem1.Qty_RS), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }
          


        }
        private void SetSummary_WorkingUnit()
        {



            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.SetWidths(new float[] { 40f, 50f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f,42, 40f });

            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Summary(Store wise)", 0, 10, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL));
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion
            //foreach (var oItem in oDUOrderType)
            //{
           // List<RSFreshDyedYarn> oRSFreshDyedYarns = new List<RSFreshDyedYarn>();// _oRSFreshDyedYarns.Where(x => x.OrderType == oItem.OrderType).ToList();
            #region Fabric Info
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.SetWidths(new float[] { 50f, 50f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42, 50f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Store Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dyeing Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Packing Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "WIP Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Recycle Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Wastage Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Short Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Gain Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Loss %", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            //   NoOfBag = grp.Sum(p => p.NoOfBag),
            ////                            WtPerBag = grp.Sum(p => p.WtPerBag),
          var  oRSFreshDyedYarnsWU = _oRSFreshDyedYarns.GroupBy(x => new { x.WorkingUnitID, x.WUName, x.IsReDyeing }, (key, grp) =>
                                    new 
                                    {
                                        WorkingUnitID = key.WorkingUnitID,
                                        WUName = key.WUName,
                                        IsReDyeing = key.IsReDyeing,
                                        Qty_RS = grp.Sum(p => p.Qty_RS),
                                        WIPQty = grp.Sum(p => p.WIPQty),
                                        FreshDyedYarnQty = grp.Sum(p => p.FreshDyedYarnQty),
                                        RecycleQty = grp.Sum(p => p.RecycleQty),
                                        WastageQty = grp.Sum(p => p.WastageQty),
                                        Gain = grp.Sum(p => p.Gain),
                                        Loss = grp.Sum(p => p.Loss),
                                    }).ToList();
            //oRSFreshDyedYarns = oRSFreshDyedYarns.OrderBy(x => x.OrderType).ToList();
          foreach (var oItem1 in oRSFreshDyedYarnsWU)
            {
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.SetWidths(new float[] { 50f, 50f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42, 50f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.WUName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem1.IsReDyeing) ? "Re-Dye" : ""), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Qty_RS), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.FreshDyedYarnQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.WIPQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.RecycleQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.WastageQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Loss), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Gain), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Loss * 100 / oItem1.Qty_RS), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
         var oRSFreshDyedYarnsWUTwo = _oRSFreshDyedYarns.GroupBy(x => new {}, (key, grp) =>
                                    new
                                    {
                                         Qty_RS = grp.Sum(p => p.Qty_RS),
                                         WIPQty = grp.Sum(p => p.WIPQty),
                                         FreshDyedYarnQty = grp.Sum(p => p.FreshDyedYarnQty),
                                         RecycleQty = grp.Sum(p => p.RecycleQty),
                                         WastageQty = grp.Sum(p => p.WastageQty),
                                         Gain = grp.Sum(p => p.Gain),
                                         Loss = grp.Sum(p => p.Loss),

                                     }).ToList();

         foreach (var oItem1 in oRSFreshDyedYarnsWUTwo)
            {
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.SetWidths(new float[] { 50f, 50f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42, 50f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Qty_RS), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.FreshDyedYarnQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.WIPQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.RecycleQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.WastageQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Loss), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Gain), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Loss * 100 / oItem1.Qty_RS), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }



        }
        #endregion
    }
}
