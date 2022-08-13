using System;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.Reports
{
    public class rptDUPscheduleReport_RS
    {
        #region Declaration
        
        int _nColumns = 11;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        public iTextSharp.text.Image _oImag { get; set; }
        PdfPTable _oPdfPTable = new PdfPTable(11);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<DUPScheduleDetail> _oDUPSD = new List<DUPScheduleDetail>();
        Company _oCompany = new Company();
        string _sDateRange = "";

        #endregion

        public byte[] PrepareReport(List<DUPScheduleDetail> oDUPSD, Company oCompany, string sDateRange, string sRSStates, BusinessUnit oBusinessUnit)
        {
            _oDUPSD = oDUPSD;
            _oCompany = oCompany;
            _sDateRange = sDateRange;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(PageSize.A4.Rotate());
            //_oDocument.SetMargins(0f, 0f, 10f, 10f);
            _oDocument.SetMargins(0f, 0f, 6f, 6f);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 95;

            //SL M/C No.	Buyer	 Order No PS Batch	Batch No	Yarn Type	Color	Batch Qty(KG)	Batch Qty(KG) Remarks
            _oPdfPTable.SetWidths(new float[] { 23f,65f, 105f, 62f, 44f, 46f, 140f, 72f,40f, 40f, 60f}); //height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            #endregion

            this.PrintHeader(oBusinessUnit, sRSStates);
            this.PrintBody();

            _oPdfPTable.HeaderRows = 6;
            if (string.IsNullOrEmpty(sRSStates))
                _oPdfPTable.HeaderRows = 5;
            
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(BusinessUnit oBusinessUnit, string sRSStates)
        {
            #region CompanyHeader

            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase("(Yarn Dyeing Unit)", _oFontStyle));
            //_oPdfPCell.Colspan = _nColumns;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.ExtraParagraphSpace = 8f;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            //_oPdfPCell.Colspan = _nColumns;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Daily Yarn Dyeing Production Report", _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
           // _oPdfPCell.ExtraParagraphSpace = 8f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            if (!string.IsNullOrEmpty(sRSStates)) 
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("State: "+sRSStates, _oFontStyle));
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.ExtraParagraphSpace = 8f;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
           
            #endregion

            #region DateRange
            DateTime dt = DateTime.Now;
            string sDate = dt.ToString("dd-MMM-yyyy HH:mm");
            GetTopTitleTable(sDate);
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            //_oPdfPCell = new PdfPCell(new Phrase(_sDateRange, _oFontStyle));
            //_oPdfPCell.Colspan = 7;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(sDate, _oFontStyle));
            //_oPdfPCell.Colspan = 4;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        private void GetTopTitleTable(string sDate)
        {
            PdfPTable oStickerTable = new PdfPTable(3);
            oStickerTable.WidthPercentage = 100;
            oStickerTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oStickerTable.SetWidths(new float[] {               25f,                                                  
                                                                50f,
                                                                25f
                                                          });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStickerTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_sDateRange, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStickerTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sDate, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oStickerTable.AddCell(_oPdfPCell);

            oStickerTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oStickerTable);
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        #region Report Body
        private void PrintBody()
        {
            int nCount = 0, nRSID=0;
            double nQty=0;
           
            #region Default View
            //M/C No.	Buyer	 Order No	Batch No	Yarn Type	Color	Batch Qty(KG)	Batch Qty(KG)	From Stock	Remarks
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Machine No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("YP", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Batch No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Batch Qty(KG)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase("Batch Qty(KG)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            string sEndDateSt = "";
            if (_oDUPSD.Count() > 0)
            {
                _oDUPSD.ForEach(o => o.EndTime =Convert.ToDateTime(o.EndTime.ToString("dd MMM yy")));
                var data = _oDUPSD.GroupBy(x => new {x.EndDateSt,x.EndTime, x.MachineID }, (key, grp) => new
                {
                    EndDateSt = grp.Select(x => x.EndDateSt).First(),
                    EndDate = grp.Select(x => x.EndTime).First(),
                    HeaderName = grp.Select(x => x.MachineName).First(),
                    TotalPassQty = grp.Sum(x => x.Qty),
                    Results = grp.OrderBy(x=>x.RouteSheetID).ToList()
                });

                data = data.OrderBy(x => x.EndDate).ThenBy(x => x.HeaderName).ToList();

                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                foreach (var oItem in data)
                {

                    if (sEndDateSt != oItem.EndDateSt)
                    {
                        if (nQty > 0)
                        {
                            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Total", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 8, 0);
                            ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 0, 0);
                            ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 0, 0);
                            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 0, 0);
                            _oPdfPTable.CompleteRow();
                        }
                        nQty = 0;
                        ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.EndDateSt, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 11, 0);
                        _oPdfPTable.CompleteRow();
                    }

                    nCount++;
                    int nSpan= oItem.Results.Count();
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, (nCount).ToString(), Element.ALIGN_RIGHT, Element.ALIGN_TOP, this.GetColor(nCount), 15,nSpan,0,0);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.HeaderName, Element.ALIGN_LEFT, Element.ALIGN_TOP, this.GetColor(nCount), 15,nSpan,0,0);

                  

                    nRSID = -99;
                    //M/C No.	Buyer	 Order No	Batch No	Yarn Type	Color	Batch Qty(KG)	Batch Qty(KG)	From Stock	Remarks
                    foreach(var obj in oItem.Results)
                    {
                        ESimSolPdfHelper.AddCell(ref _oPdfPTable, obj.BuyerName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref _oPdfPTable, obj.OrderNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref _oPdfPTable, obj.PSBatchNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                        int nSpan_Row = oItem.Results.Where(x => x.RouteSheetID == obj.RouteSheetID).Count();

                        if (nRSID != obj.RouteSheetID)
                            ESimSolPdfHelper.AddCell(ref _oPdfPTable, obj.RouteSheetNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nSpan_Row,0,0);
                        
                        ESimSolPdfHelper.AddCell(ref _oPdfPTable, obj.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref _oPdfPTable, obj.ColorName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(obj.Qty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        nQty = nQty + obj.Qty;
                        if (nRSID != obj.RouteSheetID)
                            ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.RouteSheetID == obj.RouteSheetID).Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nSpan_Row, 0, 0);
                        
                        string sRemarks = obj.Remarks;

                        if (!string.IsNullOrEmpty(obj.DyeLoadNote))
                            sRemarks = obj.DyeLoadNote;
                        if (!string.IsNullOrEmpty(obj.DyeUnloadNote))
                            sRemarks += (!string.IsNullOrEmpty(sRemarks) ? ", " : "") + obj.DyeUnloadNote;

                        ESimSolPdfHelper.AddCell(ref _oPdfPTable, sRemarks, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        nRSID = obj.RouteSheetID;
                    }
                    _oPdfPTable.CompleteRow();
                    sEndDateSt = oItem.EndDateSt;
                }
                #endregion
                
                #region Total
                if (nQty > 0)
                {
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Total", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 8, 0);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 0, 0);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 0, 0);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 0, 0);
                    _oPdfPTable.CompleteRow();
                }

                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Grand Total", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 8, 0);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oDUPSD.Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 0, 0);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oDUPSD.Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 0, 0);
                #endregion

                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 0, 0);
                _oPdfPTable.CompleteRow();

                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, 0, 11, 10);
                _oPdfPTable.CompleteRow();

                #region Summary
                double total_batch_Qty = _oDUPSD.Sum(x => x.Qty), percentage = 0;

                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "InHouse Production", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 6, 0);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oDUPSD.Where(x => x.IsInHouse).Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 2, 0);
                if (total_batch_Qty > 0) percentage = (_oDUPSD.Where(x => x.IsInHouse).Sum(x => x.Qty) * 100 / total_batch_Qty); else percentage = 0;
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(percentage), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 3, 0);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, 0, 0, 0);
                _oPdfPTable.CompleteRow();

                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Out-Side Production", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 6, 0);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oDUPSD.Where(x => !x.IsInHouse).Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 2, 0);

                if (total_batch_Qty > 0) percentage = (_oDUPSD.Where(x => !x.IsInHouse).Sum(x => x.Qty) * 100 / total_batch_Qty); else percentage = 0;
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(percentage), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 3, 0);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, 0, 0, 0); 
                _oPdfPTable.CompleteRow();

                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Total Production", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 6, 0);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oDUPSD.Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 2, 0);
                if (total_batch_Qty > 0) percentage = (_oDUPSD.Sum(x => x.Qty) * 100 / total_batch_Qty); else percentage = 0;
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(percentage), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 3, 0);
                #endregion

                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, 0, 0, 0); 
                _oPdfPTable.CompleteRow();
            }
        }
        #endregion

        public BaseColor GetColor(int nCount)
        {
            //int mode= nCount % 4;
            //BaseColor result=ESimSolPdfHelper.Custom_BaseColor(new int[] { 17, 127, 117 });
            //switch(mode)
            //{
            //    case 0: result= ESimSolPdfHelper.Custom_BaseColor(new int[] { 67, 127, 117 }); break;
            //    case 1: result = ESimSolPdfHelper.Custom_BaseColor(new int[] { 47, 117, 127 }); break;
            //    case 2: result= ESimSolPdfHelper.Custom_BaseColor(new int[] { 67, 100, 14 }); break;
            //    case 3: result = ESimSolPdfHelper.Custom_BaseColor(new int[] { 50, 127, 142 }); break;
            //    case 4: result = ESimSolPdfHelper.Custom_BaseColor(new int[] { 40, 107, 102 }); break;
            //}
            return BaseColor.WHITE;
        }
    }
}
