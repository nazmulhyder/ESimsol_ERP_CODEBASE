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
    public class rptFNRequisitionReport
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
        FNRequisitionReport _oFNRequisitionReport = new FNRequisitionReport();
        List<FNRequisitionReport> _oFNRequisitionReports = new List<FNRequisitionReport>();
        Company _oCompany = new Company();
        int _nReportType = 0;
        #endregion

        public byte[] PrepareReport(List<FNRequisitionReport> oFNRequisitionReports, Company oCompany, int nReportType)
        {
            _oFNRequisitionReports = oFNRequisitionReports;
            _oCompany = oCompany;
            _nReportType = nReportType;

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
            string sHeader = "";
            if (_nReportType == 1) sHeader = "Lot Wise Requisition Report";
            else if (_nReportType == 2) sHeader = "Product Wise Requisition Report";

            this.PrintHeader(sHeader);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header

        private void PrintHeader(string sHeader)
        {
            #region Company & Report Header
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 450f, 250f });

            #region Company Name & Report Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(95f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Address & Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Phone Number
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            if (_nReportType == 1) this.SetDataForLotWise();
            else if (_nReportType == 2) this.SetDataForProductWise();
        }
        #endregion

        private void SetDataForLotWise()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            #region initialize table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
            #endregion
            int nTotalCol = 12;

            #region Heder Info
            #region initialize table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
            #endregion

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer Name", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construction", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Machine", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Treatment", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. Qty (" + _oFNRequisitionReports[0].MUName + ")", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Used Qty (" + _oFNRequisitionReports[0].MUName + ")", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Balance (" + _oFNRequisitionReports[0].MUName + ")", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            #region data
            var dataGrpList = _oFNRequisitionReports.GroupBy(x => new { x.LotID, x.LotNo, x.ProductID, x.ProductName }, (key, grp) => new
            {
                HeaderName = "Lot No : " + key.LotNo + " [Dyes/Chemical: " + key.ProductName + "]", //unique dt
                Results = grp.ToList() //All data
            });
            double nSubTotal_Qty_Requisition = 0,
                           nSubTotal_Qty_Consume = 0,
                           nSubTotal_Qty_Balance = 0,
                           nTotal_Qty_Requisition = 0,
                           nTotal_Qty_Consume = 0,
                           nTotal_Qty_Balance = 0,
                           nGrndTotal_Qty_Requisition = 0,
                           nGrndTotal_Qty_Consume = 0,
                           nGrndTotal_Qty_Balance = 0;
            foreach (var oDataGrp in dataGrpList)
            {
                int nCount = 0;
                DateTime dFNR_Date = DateTime.MinValue;
                nSubTotal_Qty_Requisition = 0;
                nSubTotal_Qty_Consume = 0;

                #region initialize table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
                #endregion
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oDataGrp.HeaderName, 0, nTotalCol, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                foreach (var oItem in oDataGrp.Results.OrderBy(x => x.RequestDate))
                {
                    #region Batch Wise Total
                    if (dFNR_Date != oItem.RequestDate && nCount > 0)
                    {
                        #region initialize table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
                        #endregion
                        #region Total
                        
                        nSubTotal_Qty_Requisition += oDataGrp.Results.Where(x => x.RequestDate == dFNR_Date).Sum(x => x.Qty_Requisition);
                        nSubTotal_Qty_Consume += oDataGrp.Results.Where(x => x.RequestDate == dFNR_Date).Sum(x => x.Qty_Consume);
                        nSubTotal_Qty_Balance = nSubTotal_Qty_Requisition - nSubTotal_Qty_Consume;

                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 9, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubTotal_Qty_Requisition), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubTotal_Qty_Consume), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubTotal_Qty_Balance), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                        oPdfPTable.CompleteRow();
                        ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                        #endregion
                        //oPdfPTable.CompleteRow();
                        //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                        nTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                        nTotal_Qty_Consume += nSubTotal_Qty_Consume;
                        nTotal_Qty_Balance += nSubTotal_Qty_Balance;

                        nGrndTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                        nGrndTotal_Qty_Consume += nSubTotal_Qty_Consume;
                        nGrndTotal_Qty_Balance += nSubTotal_Qty_Balance;

                        nSubTotal_Qty_Requisition = 0; nSubTotal_Qty_Consume = 0; nSubTotal_Qty_Balance = 0;
                    }
                    dFNR_Date = oItem.RequestDate;
                    #endregion

                    #region DATA
                    #region initialize table
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
                    #endregion

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nCount).ToString("00"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.RequestDateInString, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.FNRNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.DispoNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Construction, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ColorName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.MachineName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.TreatmentSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty_Requisition), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty_Consume), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat((oItem.Qty_Requisition - oItem.Qty_Consume)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    
                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                    #endregion
                }

                #region Total
                #region initialize table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
                #endregion

                nSubTotal_Qty_Requisition += oDataGrp.Results.Where(x => x.RequestDate == dFNR_Date).Sum(x => x.Qty_Requisition);
                nSubTotal_Qty_Consume += oDataGrp.Results.Where(x => x.RequestDate == dFNR_Date).Sum(x => x.Qty_Consume);
                nSubTotal_Qty_Balance = nSubTotal_Qty_Requisition - nSubTotal_Qty_Consume;

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 9, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubTotal_Qty_Requisition), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubTotal_Qty_Consume), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubTotal_Qty_Balance), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                #endregion

                nTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                nTotal_Qty_Consume += nSubTotal_Qty_Consume;
                nTotal_Qty_Balance += nSubTotal_Qty_Balance;

                nGrndTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                nGrndTotal_Qty_Consume += nSubTotal_Qty_Consume;
                nGrndTotal_Qty_Balance += nSubTotal_Qty_Balance;

                nSubTotal_Qty_Requisition = 0; nSubTotal_Qty_Consume = 0; nSubTotal_Qty_Balance = 0;

                #region Sub Total
                #region initialize table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
                #endregion

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sub Total", 0, 9, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotal_Qty_Requisition), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotal_Qty_Consume), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotal_Qty_Balance), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                nTotal_Qty_Requisition = 0;
                nTotal_Qty_Consume = 0;
                nTotal_Qty_Balance = 0;
                #endregion

            }

            #region Grand Total
            #region initialize table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
            #endregion

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Grand Total", 0, 9, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nGrndTotal_Qty_Requisition), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nGrndTotal_Qty_Consume), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nGrndTotal_Qty_Balance), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            #endregion

            #endregion

        }

        private void SetDataForProductWise()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            #region initialize table
            PdfPTable oPdfPTable = new PdfPTable(13);
            oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                70f, //Lot no
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
            #endregion
            int nTotalCol = 13;

            #region Heder Info
            #region initialize table
            oPdfPTable = new PdfPTable(13);
            oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                70f, //Lot no
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
            #endregion

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer Name", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construction", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Machine", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Treatment", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Lot No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. Qty (" + _oFNRequisitionReports[0].MUName+")", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Used Qty (" + _oFNRequisitionReports[0].MUName + ")", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Balance (" + _oFNRequisitionReports[0].MUName + ")", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            #region data
            var dataGrpList = _oFNRequisitionReports.GroupBy(x => new { x.ProductID, x.ProductName }, (key, grp) => new
            {
                HeaderName = "Dyes/Chemical : " + key.ProductName, //unique dt
                Results = grp.ToList() //All data
            });
            double nSubTotal_Qty_Requisition = 0,
                           nSubTotal_Qty_Consume = 0,
                           nSubTotal_Qty_Balance = 0,
                           nTotal_Qty_Requisition = 0,
                           nTotal_Qty_Consume = 0,
                           nTotal_Qty_Balance = 0,
                           nGrndTotal_Qty_Requisition = 0,
                           nGrndTotal_Qty_Consume = 0,
                           nGrndTotal_Qty_Balance = 0;
            foreach (var oDataGrp in dataGrpList)
            {
                int nCount = 0;
                int nLotID = 0;
                nSubTotal_Qty_Requisition = 0;
                nSubTotal_Qty_Consume = 0;

                #region initialize table
                oPdfPTable = new PdfPTable(13);
                oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                70f, //Lot no
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
                #endregion
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oDataGrp.HeaderName, 0, nTotalCol, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                foreach (var oItem in oDataGrp.Results.OrderBy(x => x.LotID))
                {
                    #region Batch Wise Total
                    if (nLotID != oItem.LotID && nCount > 0)
                    {
                        #region initialize table
                        oPdfPTable = new PdfPTable(13);
                        oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                70f, //Lot no
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
                        #endregion
                        #region Total
                        nSubTotal_Qty_Requisition += oDataGrp.Results.Where(x => x.LotID == nLotID).Sum(x => x.Qty_Requisition);
                        nSubTotal_Qty_Consume += oDataGrp.Results.Where(x => x.LotID == nLotID).Sum(x => x.Qty_Consume);
                        nSubTotal_Qty_Balance = nSubTotal_Qty_Requisition - nSubTotal_Qty_Consume;

                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 10, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubTotal_Qty_Requisition), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubTotal_Qty_Consume), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubTotal_Qty_Balance), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                        oPdfPTable.CompleteRow();
                        ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                        #endregion
                        
                        nTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                        nTotal_Qty_Consume += nSubTotal_Qty_Consume;
                        nTotal_Qty_Balance += nSubTotal_Qty_Balance;

                        nGrndTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                        nGrndTotal_Qty_Consume += nSubTotal_Qty_Consume;
                        nGrndTotal_Qty_Balance += nSubTotal_Qty_Balance;

                        nSubTotal_Qty_Requisition = 0; nSubTotal_Qty_Consume = 0; nSubTotal_Qty_Balance = 0;
                    }
                    nLotID = oItem.LotID;
                    #endregion

                    #region DATA
                    #region initialize table
                    oPdfPTable = new PdfPTable(13);
                    oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                70f, //Lot no
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
                    #endregion

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nCount).ToString("00"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.RequestDateInString, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.FNRNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.DispoNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Construction, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ColorName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.MachineName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.TreatmentSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty_Requisition), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty_Consume), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat((oItem.Qty_Requisition - oItem.Qty_Consume)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                    #endregion
                }

                #region Total
                #region initialize table
                oPdfPTable = new PdfPTable(13);
                oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                70f, //Lot no
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
                #endregion

                nSubTotal_Qty_Requisition += oDataGrp.Results.Where(x => x.LotID == nLotID).Sum(x => x.Qty_Requisition);
                nSubTotal_Qty_Consume += oDataGrp.Results.Where(x => x.LotID == nLotID).Sum(x => x.Qty_Consume);
                nSubTotal_Qty_Balance = nSubTotal_Qty_Requisition - nSubTotal_Qty_Consume;

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 10, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubTotal_Qty_Requisition), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubTotal_Qty_Consume), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nSubTotal_Qty_Balance), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                
                #endregion

                nTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                nTotal_Qty_Consume += nSubTotal_Qty_Consume;
                nTotal_Qty_Balance += nSubTotal_Qty_Balance;

                nGrndTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                nGrndTotal_Qty_Consume += nSubTotal_Qty_Consume;
                nGrndTotal_Qty_Balance += nSubTotal_Qty_Balance;

                nSubTotal_Qty_Requisition = 0; nSubTotal_Qty_Consume = 0; nSubTotal_Qty_Balance = 0;

                #region Sub Total
                #region initialize table
                oPdfPTable = new PdfPTable(13);
                oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                70f, //Lot no
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
                #endregion

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sub Total", 0, 10, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotal_Qty_Requisition), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotal_Qty_Consume), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotal_Qty_Balance), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                nTotal_Qty_Requisition = 0;
                nTotal_Qty_Consume = 0;
                nTotal_Qty_Balance = 0;
                #endregion

            }

            #region Grand Total
            #region initialize table
            oPdfPTable = new PdfPTable(13);
            oPdfPTable.SetWidths(new float[] { 30f, 
                                                60f, //Req date 
                                                60f, //Req no
                                                70f, //order no
                                                90f, //buyer name
                                                110f, //Construction
                                                70f, //Color
                                                90f, //machine
                                                60f, //process
                                                70f, //Lot no
                                                60f, //Req Qty
                                                60f, //Used Qty
                                                60f //Balance
                                            });
            #endregion

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Grand Total", 0, 10, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nGrndTotal_Qty_Requisition), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nGrndTotal_Qty_Consume), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nGrndTotal_Qty_Balance), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            #endregion

            #endregion

        }

    }
}
