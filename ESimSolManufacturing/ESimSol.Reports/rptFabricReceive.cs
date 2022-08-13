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
using System.Linq;

namespace ESimSol.Reports
{
    public class rptFabricReceive
    {
        #region Declaration
        Document _oDocument;
        int _nColumns = 10;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Image _oImag { get; set; }
        PdfPTable _oPdfPTable = new PdfPTable(10);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<FNOrderFabricReceive> _oFNEOFRs = new List<FNOrderFabricReceive>();
        Company _oCompany = new Company();
        string _sReportFor = "";
        #endregion


        #region Constructor
        public rptFabricReceive() { }
        #endregion

        public byte[] PrepareReport(List<FNOrderFabricReceive> oFNEOFRs, Company oCompany, string sReportFor)
        {

            _oFNEOFRs = oFNEOFRs;
            _oCompany = oCompany;
            _sReportFor = sReportFor;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(PageSize.A4);
            _oDocument.SetMargins(10f, 10f, 20f, 20f);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.SetWidths(new float[] { 30f, 65f, 60f, 30f, 50f, 60f, 55f, 50f, 50f, 50f }); //height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader
            int nflag = 0;
            if (_oCompany.CompanyLogo != null)
            {
                nflag = 1;
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(70f, 25f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.FixedHeight = 25f;
                _oPdfPCell.Border = 0;
                _oPdfPCell.PaddingRight = 0f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            if (nflag == 1)
            {
                _oPdfPCell.Colspan = _nColumns - 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            }
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader

            string sTitle = "Fabric Wise";
            if (_sReportFor == "OrderWise") sTitle = "Order Wise";

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric Receive Report: " + sTitle, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region DateRange

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            DateTime dt = DateTime.Now;
            string sDate = dt.ToString("dd-MMM-yyyy HH:mm");
            _oPdfPCell = new PdfPCell(new Phrase(sDate, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
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
            if (_sReportFor == "OrderWise")
            {
                #region Order Wise
                if (_oFNEOFRs.Count() > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Colspan = _nColumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    #region Table Header
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Customer", _oFontStyle));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Process Type", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Roll No", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Receive Date", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty(M)", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty(Y)", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Body
                    int nSL = 0;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
                    List<FNOrderFabricReceive> oTempFNEOFRs = new List<FNOrderFabricReceive>();
                    string[] sFEOIDs = string.Join(",", _oFNEOFRs.Select(o => o.FEOID).Distinct()).Split(',');
                    foreach (string sFEOID in sFEOIDs)
                    {
                        int nFEOID = Convert.ToInt32(sFEOID);
                        oTempFNEOFRs = new List<FNOrderFabricReceive>();
                        oTempFNEOFRs = _oFNEOFRs.Where(o => o.FEOID == nFEOID).ToList();
                        if (oTempFNEOFRs.Count > 0)
                        {
                            nSL++;

                            _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oTempFNEOFRs[0].OrderNo, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oTempFNEOFRs[0].BuyerName, _oFontStyle));
                            _oPdfPCell.Colspan = 2;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oTempFNEOFRs[0].IssueDateInStr, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oTempFNEOFRs[0].ProcessTypeName, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(this.GetRollsInfo(oTempFNEOFRs));
                            _oPdfPCell.Colspan = 4;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();
                        }
                    }
                    #endregion

                    #region Total

                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);

                    double nQtyInYard = _oFNEOFRs.Sum(x => x.Qty);
                    double nQtyInMeter = _oFNEOFRs.Sum(x => Global.GetMeter(x.Qty, 2));
                    int nTotalBuyer = _oFNEOFRs.Select(o => o.BuyerID).Distinct().Count();

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(nTotalBuyer.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQtyInMeter), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQtyInYard), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    #endregion

                }
                else
                {
                    this.NoListFound();
                }

                #endregion
            }
            else
            {
                #region Fabric Wise

                if (_oFNEOFRs.Count() > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Colspan = _nColumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    #region Table Header

                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Fabric", _oFontStyle));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Customer", _oFontStyle));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Roll No", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Receive Date", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty(M)", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty(Y)", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    #endregion

                    #region Table Body
                    int nSL = 0;
                        

                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
                    List<FNOrderFabricReceive> oTempFNEOFRs = new List<FNOrderFabricReceive>();
                    string[] sFabricIDs = string.Join(",", _oFNEOFRs.Select(o => o.FabricID).Distinct()).Split(',');
                    foreach (string sFabricID in sFabricIDs)
                    {
                        int nFabricID = Convert.ToInt32(sFabricID);
                        oTempFNEOFRs = new List<FNOrderFabricReceive>();
                        oTempFNEOFRs = _oFNEOFRs.Where(o => o.FabricID == nFabricID).ToList();
                        if (oTempFNEOFRs.Count > 0)
                        {
                            nSL++;

                            _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; 
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; 
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE; 
                            _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oTempFNEOFRs[0].Construction + " [" + oTempFNEOFRs[0].FabricNo + "]", _oFontStyle));
                            _oPdfPCell.Colspan = 2;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oTempFNEOFRs[0].BuyerName, _oFontStyle));
                            _oPdfPCell.Colspan = 2;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oTempFNEOFRs[0].OrderNo, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(this.GetRollsInfo(oTempFNEOFRs));
                            _oPdfPCell.Colspan = 4;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPTable.CompleteRow();
                        }
                    }
                    #endregion

                    #region Total

                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);

                    double nQtyInYard = _oFNEOFRs.Sum(x => x.Qty);
                    double nQtyInMeter = _oFNEOFRs.Sum(x => Global.GetMeter(x.Qty, 2));
                    int nTotalFabric = _oFNEOFRs.Select(o => o.FabricID).Distinct().Count();
                    int nTotalBuyer = _oFNEOFRs.Select(o => o.BuyerID).Distinct().Count();
                    int nTotalOrder = _oFNEOFRs.Select(o => o.OrderNo).Distinct().Count();

                    _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(nTotalFabric.ToString(), _oFontStyle));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(nTotalBuyer.ToString(), _oFontStyle));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(nTotalOrder.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                   

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQtyInMeter), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQtyInYard), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                }
                else
                {
                    this.NoListFound();
                }

                #endregion
            }

            #region Signature Part
            if (_oFNEOFRs.Count() > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = _nColumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25f; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);


                _oPdfPCell = new PdfPCell(new Phrase("_________________________", _oFontStyle));
                _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("_________________________", _oFontStyle));
                _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();


                _oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyle));
                _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("Approved By", _oFontStyle));
                _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
        }
        #endregion

        private PdfPTable GetRollsInfo(List<FNOrderFabricReceive> oFNEOFRs)
        {
            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 55f, 50f, 50f, 50f });
            foreach (FNOrderFabricReceive oItem in oFNEOFRs)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ReceiveDateInStr, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Global.GetMeter(oItem.Qty, 2)), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            return oPdfPTable;
        }

        private void NoListFound()
        {
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("No List Found", _oFontStyle));
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
    }
}