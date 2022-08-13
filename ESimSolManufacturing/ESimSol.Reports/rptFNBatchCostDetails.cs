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

    public class rptFNBatchCostDetails
    {
        #region Declaration
        int count, num;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        FNBatchCost _oFNBatchCost = new FNBatchCost();
        List<FNBatchCost> _oFNBatchCosts = new List<FNBatchCost>();
        Company _oCompany = new Company();
        string sQCNo = "";
        string sReportHeader = "";
        int _nLayout = 0;
        string _sDateRange = "";
        #endregion

        public byte[] PrepareReport(List<FNBatchCost> oFNBatchCosts, Company oCompany, int nLayout, string sDateRange)
        {
            _oFNBatchCosts = oFNBatchCosts;
            _oCompany = oCompany;
            _nLayout = nLayout;
            _sDateRange = sDateRange;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.LEGAL.Rotate());
            _oDocument.SetMargins(25f, 25f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);

            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            ESimSolFooter PageEventHandler = new ESimSolFooter();

            //PageEventHandler.signatures = signatureList;
            PageEventHandler.nFontSize = 9;
            PdfWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 842f });
            #endregion

            if (_nLayout == 1)
                sReportHeader = "Machine Wise Costing Report";
            else if (_nLayout == 2)
                sReportHeader = "Buyer Wise Costing Report";
            else if (_nLayout == 3)
                sReportHeader = "MKT Person Wise Costing Report";
            else if (_nLayout == 4)
                sReportHeader = "Process Wise Costing Report";
            else if (_nLayout == 5)
                sReportHeader = "PI Wise Costing Report";

            this.PrintHeader(sReportHeader);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(string sReportHeader)
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
            _oPdfPCell = new PdfPCell(new Phrase(sReportHeader, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Address & Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_sDateRange, _oFontStyle));
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
            if (_nLayout == 1)
            {
                GetMachineWiseTable();
            }
            else if (_nLayout == 2)
            {
                GetBuyerWiseTable();
            }
            else if (_nLayout == 3)
            {
                GetMKTWiseTable();
            }
            else if (_nLayout == 4)
            {
                GetProcessWiseTable();
            }
            else if (_nLayout == 5)
            {
                GetPIWiseTable();
            }
        }
        #endregion

        #region function

        #region Machine wise
        private void GetMachineWiseTable()
        {
            PdfPTable oMachineWiseTable = new PdfPTable(17);
            oMachineWiseTable.WidthPercentage = 100;
            oMachineWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oMachineWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
            int nTotalCol = 17;

            var dataGrpList = _oFNBatchCosts.GroupBy(x => new { x.FNTreatment, x.MachineID, x.MachineName,x.FNCode }, (key, grp) => new
            {
                FNTreatment = key.FNTreatment, //unique dt
                HeaderName = key.MachineName, //unique dt
                FNCode = key.FNCode, //unique dt
                Results = grp.ToList() //All data
            });

            dataGrpList = dataGrpList.OrderBy(x => x.FNTreatment).ThenBy(x => x.FNCode);

            double nSubTotal_Qty_Order = 0,
                           nSubTotal_Qty_Production = 0,
                           nGrndTotal_Qty_Order = 0,
                           nGrndTotal_Qty_Production = 0;
            double nGrandTtlQty = 0, nGrandTtlAmount = 0, nGrandTtlRate = 0;

            double nGrandTtlQty_Production = 0;
            foreach (var oDataGrp in dataGrpList)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                #region header
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = nTotalCol; _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);
                oMachineWiseTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(oDataGrp.FNTreatment.ToString() + " " + oDataGrp.HeaderName, _oFontStyle)); _oPdfPCell.Colspan = nTotalCol; 
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);
                oMachineWiseTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PO No", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Dispo Qty", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Production Qty", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Process", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Raw Material", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Type", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Rate", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount Cost", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Avg Cost(Yds)", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMachineWiseTable.AddCell(_oPdfPCell);

                oMachineWiseTable.CompleteRow();
                #endregion

                #region push into main table
                _oPdfPCell = new PdfPCell(oMachineWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Initialize table
                oMachineWiseTable = new PdfPTable(17);
                oMachineWiseTable.WidthPercentage = 100;
                oMachineWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oMachineWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                #endregion

                int nCount = 0, nPreviousBatchID = 0, nPreviousFNPBatchID = 0;
                string sPreviousPINo = "~~", sPreviousSCNo = "~~~", sFNProcess = "~~~~";
                double nTtlQty = 0, nTtlAmount = 0, nTtlRate = 0;
                double nSubTtlQty = 0, nSubTtlAmount = 0, nSubTtlRate = 0;
                nSubTotal_Qty_Order = 0;
                nSubTotal_Qty_Production = 0;
                double nTtlQty_Production = 0, nSubTtlQty_Production = 0;
                foreach (var oItem in oDataGrp.Results.OrderBy(x => x.FNCode).ThenBy(y => y.FNBatchID).ThenBy(y => y.PINo).ThenBy(z => z.SCNo))
                {
                    if (nPreviousBatchID != oItem.FNBatchID && nCount > 0)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                        #region Total
                        nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                        nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                        nTtlRate = (nTtlAmount / nTtlQty);

                        _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell = new PdfPCell(new Phrase(nTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase((nTtlAmount/nTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        oMachineWiseTable.CompleteRow();
                        nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                        nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();
                        sPreviousPINo = "~~"; sPreviousSCNo = "~~~"; nTtlQty_Production = 0;
                        #endregion

                        #region push into main table
                        _oPdfPCell = new PdfPCell(oMachineWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        #region Initialize table
                        oMachineWiseTable = new PdfPTable(17);
                        oMachineWiseTable.WidthPercentage = 100;
                        oMachineWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oMachineWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                        #endregion
                    }

                    #region data
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                    
                    if (sPreviousPINo != oItem.PINo)
                    {
                        int rowCountForPI = oDataGrp.Results.Count(x => x.PINo == oItem.PINo && x.FNBatchID == oItem.FNBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase((++nCount).ToString("00"), _oFontStyle)); _oPdfPCell.Rowspan = rowCountForPI;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle)); _oPdfPCell.Rowspan = rowCountForPI;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);
                    }
                    
                    if (sPreviousSCNo != oItem.SCNo)
                    {
                        int rowCountForSCNo = oDataGrp.Results.Count(x => x.SCNo == oItem.SCNo && x.FNBatchID == oItem.FNBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.SCNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCountForSCNo;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);
                    }
                    
                    if (nPreviousBatchID != oItem.FNBatchID)
                    {
                        int rowCount = oDataGrp.Results.Count(x => x.FNBatchID == oItem.FNBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FNBatchNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Construction, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Color, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);
                        sFNProcess = "~~~~~~";
                    }

                    if (nPreviousFNPBatchID != oItem.FNPBatchID)
                    {
                        int rowCount = oDataGrp.Results.Count(x => x.FNPBatchID == oItem.FNPBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProDate.ToString("dd MMM yy"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);
                        nTtlQty_Production += oItem.Qty_Production; 
                        nSubTtlQty_Production += oItem.Qty_Production;
                        nGrandTtlQty_Production += oItem.Qty_Production;
                    }

                    if (sFNProcess != oItem.FNProcess && nPreviousBatchID != oItem.FNBatchID)
                    {
                        int rowCountForFNprocess = oDataGrp.Results.Count(x => x.FNProcess == oItem.FNProcess && x.FNBatchID == oItem.FNBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FNProcess, _oFontStyle)); _oPdfPCell.Rowspan = rowCountForFNprocess;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);                        
                    }
                    //_oPdfPCell = new PdfPCell(new Phrase(oItem.FNProcess, _oFontStyle));
                    //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.IsProductionSt, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Value.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase((oItem.Value / oItem.Qty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                    #endregion

                    oMachineWiseTable.CompleteRow();
                    nPreviousBatchID = oItem.FNBatchID;
                    sPreviousPINo = oItem.PINo;
                    sPreviousSCNo = oItem.SCNo;
                    nPreviousFNPBatchID = oItem.FNPBatchID;
                    sFNProcess = oItem.FNProcess;
                }


                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

                #region Total
                nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                nTtlRate = (nTtlAmount / nTtlQty);

                _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell = new PdfPCell(new Phrase(nTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((nTtlAmount / nTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                oMachineWiseTable.CompleteRow();
                #endregion

                nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();

                nGrndTotal_Qty_Order += nSubTotal_Qty_Order;
                nGrndTotal_Qty_Production += nSubTotal_Qty_Production;

                #region Sub Total
                nSubTtlQty = oDataGrp.Results.Sum(x => x.Qty);
                nSubTtlAmount = oDataGrp.Results.Sum(x => x.Value);
                nSubTtlRate = (nSubTtlAmount / nSubTtlQty);

                _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTotal_Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(nSubTotal_Qty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((nSubTtlAmount/nSubTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

                oMachineWiseTable.CompleteRow();
                nSubTtlQty_Production = 0;
                #endregion

                #region push into main table
                _oPdfPCell = new PdfPCell(oMachineWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Initialize table
                oMachineWiseTable = new PdfPTable(17);
                oMachineWiseTable.WidthPercentage = 100;
                oMachineWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oMachineWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                #endregion
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            #region Grand Total
            nGrandTtlQty = _oFNBatchCosts.Sum(x => x.Qty);
            nGrandTtlAmount = _oFNBatchCosts.Sum(x => x.Value);
            nGrandTtlRate = (nGrandTtlAmount / nGrandTtlQty);

            _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrndTotal_Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(nGrndTotal_Qty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((nGrandTtlAmount / nGrandTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMachineWiseTable.AddCell(_oPdfPCell);

            oMachineWiseTable.CompleteRow();
            #endregion

            #region push into main table
            _oPdfPCell = new PdfPCell(oMachineWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion

        #region Buyer wise
        private void GetBuyerWiseTable()
        {
            PdfPTable oBuyerWiseTable = new PdfPTable(17);
            oBuyerWiseTable.WidthPercentage = 100;
            oBuyerWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oBuyerWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
            int nTotalCol = 17;

            var dataGrpList = _oFNBatchCosts.GroupBy(x => new { x.BuyerID, x.BuyerName }, (key, grp) => new
            {
                HeaderName = key.BuyerName, //unique dt
                Results = grp.ToList().OrderBy(z => z.FNCode) //All data
            });

            double nSubTotal_Qty_Order = 0,
                           nSubTotal_Qty_Production = 0,
                           nGrndTotal_Qty_Order = 0,
                           nGrndTotal_Qty_Production = 0;
            double nGrandTtlQty = 0, nGrandTtlAmount = 0, nGrandTtlRate = 0;

            double nGrandTtlQty_Production = 0;
            foreach (var oDataGrp in dataGrpList)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                #region header
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = nTotalCol; _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);
                oBuyerWiseTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(oDataGrp.HeaderName, _oFontStyle)); _oPdfPCell.Colspan = nTotalCol;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);
                oBuyerWiseTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PO No", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Dispo Qty", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Machine", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Production Qty", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Process", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Raw Material", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Type", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Rate", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount Cost", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Avg Cost(Yds)", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oBuyerWiseTable.AddCell(_oPdfPCell);

                oBuyerWiseTable.CompleteRow();
                #endregion

                #region push into main table
                _oPdfPCell = new PdfPCell(oBuyerWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Initialize table
                oBuyerWiseTable = new PdfPTable(17);
                oBuyerWiseTable.WidthPercentage = 100;
                oBuyerWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oBuyerWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                #endregion

                int nCount = 0, nPreviousBatchID = 0, nPreviousFNPBatchID = 0;
                string sPreviousPINo = "~~", sPreviousSCNo = "~~~";
                double nTtlQty = 0, nTtlAmount = 0, nTtlRate = 0;
                double nSubTtlQty = 0, nSubTtlAmount = 0, nSubTtlRate = 0;
                nSubTotal_Qty_Order = 0;
                nSubTotal_Qty_Production = 0;
                double nTtlQty_Production = 0, nSubTtlQty_Production = 0;
                foreach (var oItem in oDataGrp.Results.OrderBy(x => x.FNBatchID).ThenBy(y => y.PINo).ThenBy(z => z.SCNo))
                {
                    if (nPreviousBatchID != oItem.FNBatchID && nCount > 0)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                        #region Total
                        nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                        nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                        nTtlRate = (nTtlAmount / nTtlQty);

                        _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell = new PdfPCell(new Phrase(nTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase((nTtlAmount / nTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        oBuyerWiseTable.CompleteRow();
                        nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                        nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();
                        sPreviousPINo = "~~"; sPreviousSCNo = "~~~"; nTtlQty_Production = 0;
                        #endregion

                        #region push into main table
                        _oPdfPCell = new PdfPCell(oBuyerWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        #region Initialize table
                        oBuyerWiseTable = new PdfPTable(17);
                        oBuyerWiseTable.WidthPercentage = 100;
                        oBuyerWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oBuyerWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                        #endregion
                    }

                    #region data
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

                    if (sPreviousPINo != oItem.PINo)
                    {
                        int rowCountForPI = oDataGrp.Results.Count(x => x.PINo == oItem.PINo && x.FNBatchID == oItem.FNBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase((++nCount).ToString("00"), _oFontStyle)); _oPdfPCell.Rowspan = rowCountForPI;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle)); _oPdfPCell.Rowspan = rowCountForPI;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);
                    }

                    if (sPreviousSCNo != oItem.SCNo)
                    {
                        int rowCountForSCNo = oDataGrp.Results.Count(x => x.SCNo == oItem.SCNo && x.FNBatchID == oItem.FNBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.SCNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCountForSCNo;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);
                    }

                    if (nPreviousBatchID != oItem.FNBatchID)
                    {
                        int rowCount = oDataGrp.Results.Count(x => x.FNBatchID == oItem.FNBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FNBatchNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.MachineName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Construction, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Color, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                    }

                    if (nPreviousFNPBatchID != oItem.FNPBatchID)
                    {
                        int rowCount = oDataGrp.Results.Count(x => x.FNPBatchID == oItem.FNPBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProDate.ToString("dd MMM yy"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);
                        nTtlQty_Production += oItem.Qty_Production;
                        nSubTtlQty_Production += oItem.Qty_Production;
                        nGrandTtlQty_Production += oItem.Qty_Production;
                    }

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.FNProcess, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.IsProductionSt, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Value.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase((oItem.Value / oItem.Qty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                    #endregion

                    oBuyerWiseTable.CompleteRow();
                    nPreviousBatchID = oItem.FNBatchID;
                    sPreviousPINo = oItem.PINo;
                    sPreviousSCNo = oItem.SCNo;
                    nPreviousFNPBatchID = oItem.FNPBatchID;
                }


                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

                #region Total
                nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                nTtlRate = (nTtlAmount / nTtlQty);

                _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell = new PdfPCell(new Phrase(nTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((nTtlAmount / nTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                oBuyerWiseTable.CompleteRow();
                #endregion

                nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();

                nGrndTotal_Qty_Order += nSubTotal_Qty_Order;
                nGrndTotal_Qty_Production += nSubTotal_Qty_Production;

                #region Sub Total
                nSubTtlQty = oDataGrp.Results.Sum(x => x.Qty);
                nSubTtlAmount = oDataGrp.Results.Sum(x => x.Value);
                nSubTtlRate = (nSubTtlAmount / nSubTtlQty);

                _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTotal_Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(nSubTotal_Qty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((nSubTtlAmount / nSubTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

                oBuyerWiseTable.CompleteRow();
                nSubTtlQty_Production = 0;
                #endregion

                #region push into main table
                _oPdfPCell = new PdfPCell(oBuyerWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Initialize table
                oBuyerWiseTable = new PdfPTable(17);
                oBuyerWiseTable.WidthPercentage = 100;
                oBuyerWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oBuyerWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                #endregion
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            #region Grand Total
            nGrandTtlQty = _oFNBatchCosts.Sum(x => x.Qty);
            nGrandTtlAmount = _oFNBatchCosts.Sum(x => x.Value);
            nGrandTtlRate = (nGrandTtlAmount / nGrandTtlQty);

            _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrndTotal_Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(nGrndTotal_Qty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((nGrandTtlAmount / nGrandTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oBuyerWiseTable.AddCell(_oPdfPCell);

            oBuyerWiseTable.CompleteRow();
            #endregion

            #region push into main table
            _oPdfPCell = new PdfPCell(oBuyerWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion

        #region MKT wise
        private void GetMKTWiseTable()
        {
            PdfPTable oMKTWiseTable = new PdfPTable(17);
            oMKTWiseTable.WidthPercentage = 100;
            oMKTWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oMKTWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
            int nTotalCol = 17;

            var dataGrpList = _oFNBatchCosts.GroupBy(x => new { x.MktAccountID, x.MktName }, (key, grp) => new
            {
                HeaderName = key.MktName, //unique
                Results = grp.ToList().OrderBy(z => z.FNCode) //All data
            });

            double nSubTotal_Qty_Order = 0,
                           nSubTotal_Qty_Production = 0,
                           nGrndTotal_Qty_Order = 0,
                           nGrndTotal_Qty_Production = 0;
            double nGrandTtlQty = 0, nGrandTtlAmount = 0, nGrandTtlRate = 0;

            double nGrandTtlQty_Production = 0;
            foreach (var oDataGrp in dataGrpList)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                #region header
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = nTotalCol; _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);
                oMKTWiseTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(oDataGrp.HeaderName, _oFontStyle)); _oPdfPCell.Colspan = nTotalCol;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);
                oMKTWiseTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PO No", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Dispo Qty", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Production Qty", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Process", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Raw Material", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Type", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Rate", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount Cost", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Avg Cost(Yds)", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                oMKTWiseTable.CompleteRow();
                #endregion

                #region push into main table
                _oPdfPCell = new PdfPCell(oMKTWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Initialize table
                oMKTWiseTable = new PdfPTable(17);
                oMKTWiseTable.WidthPercentage = 100;
                oMKTWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oMKTWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                #endregion

                int nCount = 0, nPreviousBatchID = 0, nPreviousFNPBatchID = 0;
                string sPreviousPINo = "~~", sPreviousSCNo = "~~~";
                double nTtlQty = 0, nTtlAmount = 0, nTtlRate = 0;
                double nSubTtlQty = 0, nSubTtlAmount = 0, nSubTtlRate = 0;
                nSubTotal_Qty_Order = 0;
                nSubTotal_Qty_Production = 0;
                double nTtlQty_Production = 0, nSubTtlQty_Production = 0;
                foreach (var oItem in oDataGrp.Results.OrderBy(x => x.FNBatchID).ThenBy(y => y.PINo).ThenBy(z => z.SCNo))
                {
                    if (nPreviousBatchID != oItem.FNBatchID && nCount > 0)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                        #region Total
                        nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                        nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                        nTtlRate = (nTtlAmount / nTtlQty);

                        _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell = new PdfPCell(new Phrase(nTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase((nTtlAmount / nTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        oMKTWiseTable.CompleteRow();
                        nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                        nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();
                        sPreviousPINo = "~~"; sPreviousSCNo = "~~~"; nTtlQty_Production = 0;
                        #endregion

                        #region push into main table
                        _oPdfPCell = new PdfPCell(oMKTWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        #region Initialize table
                        oMKTWiseTable = new PdfPTable(17);
                        oMKTWiseTable.WidthPercentage = 100;
                        oMKTWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oMKTWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                        #endregion
                    }

                    #region data
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

                    if (sPreviousPINo != oItem.PINo)
                    {
                        int rowCountForPI = oDataGrp.Results.Count(x => x.PINo == oItem.PINo && x.FNBatchID == oItem.FNBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase((++nCount).ToString("00"), _oFontStyle)); _oPdfPCell.Rowspan = rowCountForPI;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle)); _oPdfPCell.Rowspan = rowCountForPI;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);
                    }

                    if (sPreviousSCNo != oItem.SCNo)
                    {
                        int rowCountForSCNo = oDataGrp.Results.Count(x => x.SCNo == oItem.SCNo && x.FNBatchID == oItem.FNBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.SCNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCountForSCNo;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);
                    }

                    if (nPreviousBatchID != oItem.FNBatchID)
                    {
                        int rowCount = oDataGrp.Results.Count(x => x.FNBatchID == oItem.FNBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FNBatchNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Construction, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Color, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    }

                    if (nPreviousFNPBatchID != oItem.FNPBatchID)
                    {
                        int rowCount = oDataGrp.Results.Count(x => x.FNPBatchID == oItem.FNPBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProDate.ToString("dd MMM yy"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);
                        nTtlQty_Production += oItem.Qty_Production;
                        nSubTtlQty_Production += oItem.Qty_Production;
                        nGrandTtlQty_Production += oItem.Qty_Production;
                    }

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.FNProcess, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.IsProductionSt, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Value.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase((oItem.Value / oItem.Qty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    #endregion

                    oMKTWiseTable.CompleteRow();
                    nPreviousBatchID = oItem.FNBatchID;
                    sPreviousPINo = oItem.PINo;
                    sPreviousSCNo = oItem.SCNo;
                    nPreviousFNPBatchID = oItem.FNPBatchID;
                }


                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

                #region Total
                nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                nTtlRate = (nTtlAmount / nTtlQty);

                _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell = new PdfPCell(new Phrase(nTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((nTtlAmount / nTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                oMKTWiseTable.CompleteRow();
                #endregion

                nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();

                nGrndTotal_Qty_Order += nSubTotal_Qty_Order;
                nGrndTotal_Qty_Production += nSubTotal_Qty_Production;

                #region Sub Total
                nSubTtlQty = oDataGrp.Results.Sum(x => x.Qty);
                nSubTtlAmount = oDataGrp.Results.Sum(x => x.Value);
                nSubTtlRate = (nSubTtlAmount / nSubTtlQty);

                _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTotal_Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(nSubTotal_Qty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((nSubTtlAmount / nSubTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                oMKTWiseTable.CompleteRow();
                nSubTtlQty_Production = 0;
                #endregion

                #region push into main table
                _oPdfPCell = new PdfPCell(oMKTWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Initialize table
                oMKTWiseTable = new PdfPTable(17);
                oMKTWiseTable.WidthPercentage = 100;
                oMKTWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oMKTWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                #endregion
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            #region Grand Total
            nGrandTtlQty = _oFNBatchCosts.Sum(x => x.Qty);
            nGrandTtlAmount = _oFNBatchCosts.Sum(x => x.Value);
            nGrandTtlRate = (nGrandTtlAmount / nGrandTtlQty);

            _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrndTotal_Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(nGrndTotal_Qty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((nGrandTtlAmount / nGrandTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            oMKTWiseTable.CompleteRow();
            #endregion

            #region push into main table
            _oPdfPCell = new PdfPCell(oMKTWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion

        #region Process wise
        private void GetProcessWiseTable()
        {
            PdfPTable oProcessWiseTable = new PdfPTable(17);
            oProcessWiseTable.WidthPercentage = 100;
            oProcessWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oProcessWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Machine
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
            int nTotalCol = 17;

            var dataGrpList = _oFNBatchCosts.OrderBy(z=>z.FNTreatment).GroupBy(x => new { x.FNTreatment, x.FNTreatmentSt }, (key, grp) => new
            {
                HeaderName = key.FNTreatmentSt, //unique dt
                Results = grp.ToList().OrderBy(z => z.FNCode) //All data
            });

            double nSubTotal_Qty_Order = 0,
                           nSubTotal_Qty_Production = 0,
                           nGrndTotal_Qty_Order = 0,
                           nGrndTotal_Qty_Production = 0;
            double nGrandTtlQty = 0, nGrandTtlAmount = 0, nGrandTtlRate = 0;

            double nGrandTtlQty_Production = 0;
            foreach (var oDataGrp in dataGrpList)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                #region header
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = nTotalCol; _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);
                oProcessWiseTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(oDataGrp.HeaderName, _oFontStyle)); _oPdfPCell.Colspan = nTotalCol;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);
                oProcessWiseTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PO No", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Dispo Qty", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Production Qty", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Machine", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Raw Material", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Type", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Rate", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount Cost", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Avg Cost(Yds)", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oProcessWiseTable.AddCell(_oPdfPCell);

                oProcessWiseTable.CompleteRow();
                #endregion

                #region push into main table
                _oPdfPCell = new PdfPCell(oProcessWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Initialize table
                oProcessWiseTable = new PdfPTable(17);
                oProcessWiseTable.WidthPercentage = 100;
                oProcessWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oProcessWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                #endregion

                int nCount = 0, nPreviousBatchID = 0, nPreviousFNPBatchID = 0;
                string sPreviousPINo = "~~", sPreviousSCNo = "~~~";
                double nTtlQty = 0, nTtlAmount = 0, nTtlRate = 0;
                double nSubTtlQty = 0, nSubTtlAmount = 0, nSubTtlRate = 0;
                nSubTotal_Qty_Order = 0;
                nSubTotal_Qty_Production = 0;
                double nTtlQty_Production = 0, nSubTtlQty_Production = 0;
                foreach (var oItem in oDataGrp.Results.OrderBy(x => x.FNBatchID).ThenBy(y => y.PINo).ThenBy(z => z.SCNo))
                {
                    if (nPreviousBatchID != oItem.FNBatchID && nCount > 0)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                        #region Total
                        nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                        nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                        nTtlRate = (nTtlAmount / nTtlQty);

                        _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell = new PdfPCell(new Phrase(nTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase((nTtlAmount / nTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        oProcessWiseTable.CompleteRow();
                        nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                        nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();
                        sPreviousPINo = "~~"; sPreviousSCNo = "~~~"; nTtlQty_Production = 0;
                        #endregion

                        #region push into main table
                        _oPdfPCell = new PdfPCell(oProcessWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        #region Initialize table
                        oProcessWiseTable = new PdfPTable(17);
                        oProcessWiseTable.WidthPercentage = 100;
                        oProcessWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oProcessWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                        #endregion
                    }

                    #region data
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

                    if (sPreviousPINo != oItem.PINo)
                    {
                        int rowCountForPI = oDataGrp.Results.Count(x => x.PINo == oItem.PINo && x.FNBatchID == oItem.FNBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase((++nCount).ToString("00"), _oFontStyle)); _oPdfPCell.Rowspan = rowCountForPI;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle)); _oPdfPCell.Rowspan = rowCountForPI;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);
                    }

                    if (sPreviousSCNo != oItem.SCNo)
                    {
                        int rowCountForSCNo = oDataGrp.Results.Count(x => x.SCNo == oItem.SCNo && x.FNBatchID == oItem.FNBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.SCNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCountForSCNo;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);
                    }

                    if (nPreviousBatchID != oItem.FNBatchID)
                    {
                        int rowCount = oDataGrp.Results.Count(x => x.FNBatchID == oItem.FNBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FNBatchNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Construction, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Color, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                    }

                    if (nPreviousFNPBatchID != oItem.FNPBatchID)
                    {
                        int rowCount = oDataGrp.Results.Count(x => x.FNPBatchID == oItem.FNPBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProDate.ToString("dd MMM yy"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);
                        nTtlQty_Production += oItem.Qty_Production;
                        nSubTtlQty_Production += oItem.Qty_Production;
                        nGrandTtlQty_Production += oItem.Qty_Production;
                    }

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.MachineName, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.IsProductionSt, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Value.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase((oItem.Value / oItem.Qty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                    #endregion

                    oProcessWiseTable.CompleteRow();
                    nPreviousBatchID = oItem.FNBatchID;
                    sPreviousPINo = oItem.PINo;
                    sPreviousSCNo = oItem.SCNo;
                    nPreviousFNPBatchID = oItem.FNPBatchID;
                }


                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

                #region Total
                nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                nTtlRate = (nTtlAmount / nTtlQty);

                _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell = new PdfPCell(new Phrase(nTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((nTtlAmount / nTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                oProcessWiseTable.CompleteRow();
                #endregion

                nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();

                nGrndTotal_Qty_Order += nSubTotal_Qty_Order;
                nGrndTotal_Qty_Production += nSubTotal_Qty_Production;

                #region Sub Total
                nSubTtlQty = oDataGrp.Results.Sum(x => x.Qty);
                nSubTtlAmount = oDataGrp.Results.Sum(x => x.Value);
                nSubTtlRate = (nSubTtlAmount / nSubTtlQty);

                _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTotal_Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(nSubTotal_Qty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((nSubTtlAmount / nSubTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

                oProcessWiseTable.CompleteRow();
                nSubTtlQty_Production = 0;
                #endregion

                #region push into main table
                _oPdfPCell = new PdfPCell(oProcessWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Initialize table
                oProcessWiseTable = new PdfPTable(17);
                oProcessWiseTable.WidthPercentage = 100;
                oProcessWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oProcessWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                60f,    //pi no
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                #endregion
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            #region Grand Total
            nGrandTtlQty = _oFNBatchCosts.Sum(x => x.Qty);
            nGrandTtlAmount = _oFNBatchCosts.Sum(x => x.Value);
            nGrandTtlRate = (nGrandTtlAmount / nGrandTtlQty);

            _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrndTotal_Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(nGrndTotal_Qty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((nGrandTtlAmount / nGrandTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oProcessWiseTable.AddCell(_oPdfPCell);

            oProcessWiseTable.CompleteRow();
            #endregion

            #region push into main table
            _oPdfPCell = new PdfPCell(oProcessWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion

        #region PI wise
        private void GetPIWiseTable()
        {
            PdfPTable oMKTWiseTable = new PdfPTable(17);
            oMKTWiseTable.WidthPercentage = 100;
            oMKTWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oMKTWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                90f,    //MKT
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
            int nTotalCol = 17;

            var dataGrpList = _oFNBatchCosts.GroupBy(x => new { x.PIID, x.PINo }, (key, grp) => new
            {
                HeaderName = key.PINo, //unique
                Results = grp.ToList().OrderBy(z => z.FNCode), //All data
                count = grp.ToList().Count()
            });

            //int nnn = dataGrpList.Where(x=>x.Results.Count)

            double nSubTotal_Qty_Order = 0,
                           nSubTotal_Qty_Production = 0,
                           nGrndTotal_Qty_Order = 0,
                           nGrndTotal_Qty_Production = 0;
            double nGrandTtlQty = 0, nGrandTtlAmount = 0, nGrandTtlRate = 0;

            double nGrandTtlQty_Production = 0;
            #region Loop
            foreach (var oDataGrp in dataGrpList)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

                

                #region header
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = nTotalCol; _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);
                oMKTWiseTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("PI No: " + oDataGrp.HeaderName, _oFontStyle)); _oPdfPCell.Colspan = nTotalCol;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);
                oMKTWiseTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PO No", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Dispo Qty", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("MKT Person", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Production Qty", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Process", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Raw Material", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Type", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Rate", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount Cost", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Avg Cost(Yds)", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oMKTWiseTable.AddCell(_oPdfPCell);

                oMKTWiseTable.CompleteRow();
                #endregion

                #region push into main table
                _oPdfPCell = new PdfPCell(oMKTWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Initialize table
                oMKTWiseTable = new PdfPTable(17);
                oMKTWiseTable.WidthPercentage = 100;
                oMKTWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oMKTWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                90f,    //MKT
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                #endregion

                int nCount = 0, nPreviousBatchID = 0, nPreviousFNPBatchID = 0;
                string sPreviousPINo = "~~", sPreviousSCNo = "~~~";
                double nTtlQty = 0, nTtlAmount = 0, nTtlRate = 0;
                double nSubTtlQty = 0, nSubTtlAmount = 0, nSubTtlRate = 0;
                nSubTotal_Qty_Order = 0;
                nSubTotal_Qty_Production = 0;
                double nTtlQty_Production = 0, nSubTtlQty_Production = 0;
                foreach (var oItem in oDataGrp.Results.OrderBy(x => x.FNBatchID).ThenBy(y => y.PINo).ThenBy(z => z.SCNo))
                {
                    if (nPreviousBatchID != oItem.FNBatchID && nCount > 0)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

                        #region Total
                        nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                        nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                        nTtlRate = (nTtlAmount / nTtlQty);

                        _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle)); _oPdfPCell.Colspan = 3;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell = new PdfPCell(new Phrase(nTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(nTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase((nTtlAmount / nTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        oMKTWiseTable.CompleteRow();
                        nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                        nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();
                        sPreviousPINo = "~~"; sPreviousSCNo = "~~~"; nTtlQty_Production = 0;
                        #endregion

                        #region push into main table
                        _oPdfPCell = new PdfPCell(oMKTWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        #region Initialize table
                        oMKTWiseTable = new PdfPTable(17);
                        oMKTWiseTable.WidthPercentage = 100;
                        oMKTWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oMKTWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                90f,    //MKT
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                        #endregion

                    }

                    #region data
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

                    //if (sPreviousPINo != oItem.PINo)
                    //{
                    //    int rowCountForPI = oDataGrp.Results.Count(x => x.PINo == oItem.PINo && x.FNBatchID == oItem.FNBatchID);
                    //    _oPdfPCell = new PdfPCell(new Phrase((++nCount).ToString("00"), _oFontStyle)); _oPdfPCell.Rowspan = rowCountForPI;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle)); _oPdfPCell.Rowspan = rowCountForPI;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);
                    //}

                    if (sPreviousSCNo != oItem.SCNo)
                    {
                        int rowCountForSCNo = oDataGrp.Results.Count(x => x.SCNo == oItem.SCNo && x.FNBatchID == oItem.FNBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase((++nCount).ToString("00"), _oFontStyle)); _oPdfPCell.Rowspan = rowCountForSCNo;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);
                        
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.SCNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCountForSCNo;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);
                    }

                    if (nPreviousBatchID != oItem.FNBatchID)
                    {
                        int rowCount = oDataGrp.Results.Count(x => x.FNBatchID == oItem.FNBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.FNBatchNo, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.MktName, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Construction, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Color, _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    }

                    if (nPreviousFNPBatchID != oItem.FNPBatchID)
                    {
                        int rowCount = oDataGrp.Results.Count(x => x.FNPBatchID == oItem.FNPBatchID);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProDate.ToString("dd MMM yy"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.Rowspan = rowCount;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);
                        nTtlQty_Production += oItem.Qty_Production;
                        nSubTtlQty_Production += oItem.Qty_Production;
                        nGrandTtlQty_Production += oItem.Qty_Production;
                    }

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.FNProcess, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.IsProductionSt, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Value.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase((oItem.Value / oItem.Qty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                    #endregion

                    //oMKTWiseTable.CompleteRow();
                    nPreviousBatchID = oItem.FNBatchID;
                    sPreviousPINo = oItem.PINo;
                    sPreviousSCNo = oItem.SCNo;
                    nPreviousFNPBatchID = oItem.FNPBatchID;


                }
                #region push into main table
                _oPdfPCell = new PdfPCell(oMKTWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Initialize table
                oMKTWiseTable = new PdfPTable(17);
                oMKTWiseTable.WidthPercentage = 100;
                oMKTWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oMKTWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                90f,    //MKT
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

                #region Total
                nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                nTtlRate = (nTtlAmount / nTtlQty);

                _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault().ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell = new PdfPCell(new Phrase(nTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((nTtlAmount / nTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                oMKTWiseTable.CompleteRow();
                #endregion

                #region push into main table
                _oPdfPCell = new PdfPCell(oMKTWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Initialize table
                oMKTWiseTable = new PdfPTable(17);
                oMKTWiseTable.WidthPercentage = 100;
                oMKTWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oMKTWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                90f,    //MKT
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                #endregion
                nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();

                nGrndTotal_Qty_Order += nSubTotal_Qty_Order;
                nGrndTotal_Qty_Production += nSubTotal_Qty_Production;

                #region Sub Total
                nSubTtlQty = oDataGrp.Results.Sum(x => x.Qty);
                nSubTtlAmount = oDataGrp.Results.Sum(x => x.Value);
                nSubTtlRate = (nSubTtlAmount / nSubTtlQty);

                _oPdfPCell = new PdfPCell(new Phrase("Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTotal_Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(nSubTotal_Qty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSubTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((nSubTtlAmount / nSubTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

                oMKTWiseTable.CompleteRow();
                nSubTtlQty_Production = 0;
                #endregion

                #region push into main table
                _oPdfPCell = new PdfPCell(oMKTWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Initialize table
                oMKTWiseTable = new PdfPTable(17);
                oMKTWiseTable.WidthPercentage = 100;
                oMKTWiseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oMKTWiseTable.SetWidths(new float[] {           28f,                                                  
                                                                80f,     //Order No
                                                                80f,    //Dispo No
                                                                45f,    //Order Qty
                                                                90f,    //Buyer
                                                                90f,    //MKT
                                                                80f,    //Construction                                                                
                                                                60f,    //Color
                                                                45f,    //Date
                                                                50f,    //Batch qty
                                                                70f,    //Process
                                                                110f,    //Raw material
                                                                40f,    //Is Production
                                                                40f,    //Qty
                                                                40f,    //Rate 
                                                                60f,    //Amount Cost
                                                                40f    //avg Cost
                                                          });
                #endregion
            }
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            #region Grand Total
            nGrandTtlQty = _oFNBatchCosts.Sum(x => x.Qty);
            nGrandTtlAmount = _oFNBatchCosts.Sum(x => x.Value);
            nGrandTtlRate = (nGrandTtlAmount / nGrandTtlQty);

            _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrndTotal_Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(nGrndTotal_Qty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlQty_Production.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlRate.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nGrandTtlAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((nGrandTtlAmount / nGrandTtlQty_Production).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oMKTWiseTable.AddCell(_oPdfPCell);

            oMKTWiseTable.CompleteRow();
            #endregion

            #region push into main table
            _oPdfPCell = new PdfPCell(oMKTWiseTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion

        #endregion

        #region Summart Top (Buyer)
        public byte[] PrepareReportSUmmary(List<FNBatchCost> oFNBatchCosts, Company oCompany, int nLayout, string sDateRange)
        {
            _oFNBatchCosts = oFNBatchCosts;
            _oCompany = oCompany;
            _nLayout = nLayout;
            _sDateRange = sDateRange;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);

            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            ESimSolFooter PageEventHandler = new ESimSolFooter();

            //PageEventHandler.signatures = signatureList;
            PageEventHandler.nFontSize = 9;
            PdfWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 842f });
            #endregion

            #region Report Body & Header
            this.PrintHeader("Dyes Chemical Summarry");
            this.PrintBodySummary();
            _oPdfPTable.HeaderRows = 3;
            #endregion

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintBodySummary()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);

            #region Fabric Info
            PdfPTable oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 18f, 100f, 100f, 42f, 42f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL", 0, 0, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Catagory", 0, 0, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Name", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Value", 0, 0, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);


            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            //   NoOfBag = grp.Sum(p => p.NoOfBag),
            int nSLNo = 0;
            foreach (FNBatchCost oItem1 in _oFNBatchCosts)
            {
                nSLNo++;
                oPdfPTable = new PdfPTable(5);
                oPdfPTable.SetWidths(new float[] { 18f, 100f, 100f, 42f, 42f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSLNo.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ProductCategoryName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable,  Global.MillionFormat(oItem1.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.Currency + "" + Global.MillionFormat(oItem1.Value), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
            var oLCSummary = _oFNBatchCosts.GroupBy(x => new { x.Currency }, (key, grp) =>
                                       new
                                       {
                                           Qty = grp.Sum(p => p.Qty),
                                           Value = grp.Sum(p => p.Value),
                                           Currency = key.Currency

                                       }).ToList();
            foreach (var oItem1 in oLCSummary)
            {
                oPdfPTable = new PdfPTable(5);
                oPdfPTable.SetWidths(new float[] { 18f, 100f, 100f, 42f, 42f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0,3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Value), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }



        }
        #endregion
    }
}
