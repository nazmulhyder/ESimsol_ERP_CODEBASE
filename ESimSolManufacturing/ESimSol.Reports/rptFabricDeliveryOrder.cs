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
    public class rptFabricDeliveryOrder
    {
        #region Declaration
        int _nColumns = 1;
        float _nfixedHight = 5f;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        Phrase _oPhrase = new Phrase();
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        FabricDeliveryOrder _oFabricDeliveryOrder = new FabricDeliveryOrder();
        List<FabricDeliveryOrderDetail> _oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
        List<FabricDeliverySchedule> _oFabricDeliverySchedules = new List<FabricDeliverySchedule>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
      
        Company _oCompany = new Company();
        string _sMUnit = "";
      
     
        List<ExportPI> _oExportPIs = new List<ExportPI>();
        int _nCount = 0;
        int _nCount_P = 0;
        double _nTotalAmount = 0;
        double _nTotalQty = 0;
        double _nTotalDSQty = 0;
        bool _bIsInYard = true;
        #endregion
        float nUsagesHeight = 0;
        public byte[] PrepareReport(FabricDeliveryOrder oFabricDeliveryOrder, Company oCompany, BusinessUnit oBusinessUnit, bool bIsInYard, List<ExportPI> oExportPIs)
        {
            _oFabricDeliveryOrder = oFabricDeliveryOrder;
            _oExportPIs = oExportPIs;
            _oBusinessUnit = oBusinessUnit;
            _oFabricDeliveryOrderDetails = oFabricDeliveryOrder.FDODetails;
            _oCompany = oCompany;
            if (_oFabricDeliveryOrderDetails.Count > 0)
            {
                _sMUnit = _oFabricDeliveryOrderDetails[0].MUName;
            }
            _bIsInYard = bIsInYard;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

           this.PrintHeader();
           this.ReporttHeader();
           this.ExportDO();
            //this.PrintFooter();
            //_oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Print Header
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
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        #endregion
        private void ExportDO()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, 0);

            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 200f, 200f });

            #region 1st Row

            _oPdfPCell = new PdfPCell(this.ExportDOFirstRowLeft());
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(this.ExportDOFirstRowRight());
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Delivery Point
            if (!string.IsNullOrEmpty(_oFabricDeliveryOrder.DeliveryPoint))
            {
                oPdfPTable = new PdfPTable(2);
                oPdfPTable.SetWidths(new float[] { 200, 200f });

                //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                //_oPdfPCell = new PdfPCell(new Phrase("DELIVERY POINT : " + _oFabricDeliveryOrder.DeliveryPoint, _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //oPdfPTable.AddCell(_oPdfPCell);

                _oPhrase = new Phrase();
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPhrase.Add(new Chunk("Delivery Point : ", _oFontStyle));
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.DeliveryPoint, _oFontStyle));

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);
                

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Name of buying House
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 300f, 80f });

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Garments Contact Persone
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 150f, 110f, 130f });

            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase("Concern Person : " + _oFabricDeliveryOrder.BuyerCPName, _oFontStyle)); //prob
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.BorderWidthRight = 0;
            //_oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase("Contact No : " + _oFabricDeliveryOrder.BuyerCPPhone, _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.BorderWidthLeft = 0;
            //_oPdfPCell.BorderWidthTop = 0;
            //oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPhrase = new Phrase();
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPhrase.Add(new Chunk("Mkt. Person : ", _oFontStyle));
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.MKTPerson, _oFontStyle));

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Detail Table

            this.Blank(10);

            #region Title

            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 40f, 120f, 200f, 50f, 120f, 80f,100f, 10f, 60f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PO No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("QTY (" + _sMUnit + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Mkt Ref", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            #endregion

            #region Detail

            double nTotalQty = 0;
            _nTotalAmount = 0;
            string sTemp = "";
            if (_oFabricDeliveryOrder.FDODetails.Count > 0)
            {
                int nSL = 0;
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                foreach (FabricDeliveryOrderDetail oItem in _oFabricDeliveryOrder.FDODetails)
                {
                   
                    nSL++;
                    _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    #region sTemp1
                    string sTemp1 = "";
                    if (oItem.ProcessType != "" && oItem.ProcessType != null)
                    {
                        //sTemp1 = FabricProcessTypeObj.GetFabricProcessTypeObjs(oItem.ProcessType);
                        sTemp1 = oItem.ProcessType;
                    }
                    if (oItem.FabricWeave != "" && oItem.FabricWeave != null)
                    {
                        //sTemp1 = sTemp1 + " " + FabricWeaveObj.GetFabricWeaveObjs(oItem.FabricWeave);
                        sTemp1 = sTemp1 + " " + oItem.FabricWeave;
                    }
                    #endregion

                    #region sTemp
                    sTemp = "";
                    if (!string.IsNullOrEmpty(oItem.Construction))
                    {
                        sTemp = oItem.Construction;
                    }

                    if (!string.IsNullOrEmpty(oItem.StyleNo))
                    {
                        sTemp = sTemp + " Style : " + oItem.StyleNo;
                        if (!string.IsNullOrEmpty(oItem.ColorInfo))
                        {
                            sTemp = sTemp + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(oItem.ColorInfo))
                    {
                        sTemp = sTemp + " Color : " + oItem.ColorInfo;
                    }
                    //if (oItem.FabricWeave != EnumFabricWeave.None)
                    //{
                    //    sTemp = sTemp + ", Weave : " + oItem.FabricWeaveInString;
                    //}
                    if (oItem.FinishType != "" && oItem.FinishType != null)
                    {
                        //sTemp = sTemp + ", Finish Type : " + EnumFinishTypeObj.GetEnumFinishTypeObjs(oItem.FinishType);
                        sTemp = sTemp + ", Finish Type : " + oItem.FinishType;
                    }
                    if (!string.IsNullOrEmpty(oItem.BuyerRef))
                    {
                        sTemp = sTemp + ", Buyer Ref : " + oItem.BuyerRef;
                    }
                    #endregion

                    _oPhrase = new Phrase();
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                    _oPhrase.Add(new Chunk(sTemp1, _oFontStyle));
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    _oPhrase.Add(new Chunk("\n" + sTemp, _oFontStyle));

                    _oPdfPCell = new PdfPCell(_oPhrase);
                    _oPdfPCell.Colspan = 3;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    //string sOrderNo = oItem.OrderNo;
                    //int nnIndex = sOrderNo.IndexOf("EXE-");
                    //if (nnIndex > -1)
                    //{
                    //    sOrderNo = oItem.OrderNo.Replace("EXE-", "");
                    //}
                    //else
                    //{
                    //    nnIndex = sOrderNo.IndexOf("SWC-");
                    //    if (nnIndex > -1)
                    //    {
                    //        sOrderNo = oItem.OrderNo.Replace("SWC-", "");
                    //    }
                    //}

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.FEONo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                
                    nTotalQty += oItem.Qty;
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _nTotalAmount += (oItem.Qty);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExeNo, _oFontStyle));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                 

                }
            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Total

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 40f, 120f, 200f, 50f, 120f, 80f, 100f, 10f, 60f });

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

           // string sWord = Global.AmountInWords(_nTotalAmount, _oFabricDeliveryOrder.CurrencyName, _oFabricDeliveryOrder.CurrencyName).ToString();

            string sWord = Global.DollarWords(_nTotalAmount);
            if (!String.IsNullOrEmpty(sWord))
            {
                sWord = sWord.Replace("Dollar", "");
                sWord = sWord.Replace("Only", "");
                sWord = sWord.ToUpper();
                _sMUnit = _sMUnit.ToUpper();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("In Words: " + sWord + ""+_sMUnit, _oFontStyle));
            _oPdfPCell.Colspan = 9; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

         

            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Signature Part
            this.Blank(15);
            float nHeight = CalculatePdfPTableHeight(_oPdfPTable);
            while (nHeight< 580f)
            {
                this.Blank(15);
                nHeight = CalculatePdfPTableHeight(_oPdfPTable);
            }

            _oPdfPCell = new PdfPCell(this.SignaturePart());
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Remarks
            this.Blank(15);

            _oPdfPCell = new PdfPCell(this.Remarks());
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private PdfPTable ExportDOFirstRowLeft()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 400f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name.ToUpper() + " (WEAVING UNIT) ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 10f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();



            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase("Buying House : " + _oFabricDeliveryOrder.BuyerName, _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.Colspan = 2;
            //oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();



            _oPhrase = new Phrase();
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPhrase.Add(new Chunk("Buying House : ", _oFontStyle));
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.BuyerName, _oFontStyle));

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase(_oFabricDeliveryOrder.ContractorName, _oFontStyle)); //Confusion
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            //oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();

            if (!string.IsNullOrEmpty(_oFabricDeliveryOrder.FactoryAddress))
            {
                _oPhrase = new Phrase();
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPhrase.Add(new Chunk("Factory : ", _oFontStyle));
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.FactoryAddress, _oFontStyle));

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            if (!string.IsNullOrEmpty(_oFabricDeliveryOrder.DeliveryToName))
            {
                _oPhrase = new Phrase();
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPhrase.Add(new Chunk("Garments Name : ", _oFontStyle));
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.DeliveryToName, _oFontStyle));

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.BorderWidthBottom = 0;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPhrase = new Phrase();
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPhrase.Add(new Chunk("Address : ", _oFontStyle));
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.BuyerAddress, _oFontStyle));

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.BorderWidthTop = 0;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            if (!string.IsNullOrEmpty(_oFabricDeliveryOrder.BuyerCPName))
            {

                _oPhrase = new Phrase();
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPhrase.Add(new Chunk("Concern Person : ", _oFontStyle));
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.BuyerCPName, _oFontStyle));

                _oPdfPCell = new PdfPCell(_oPhrase); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.BorderWidthTop = 0;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPhrase = new Phrase();
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPhrase.Add(new Chunk("Contact No : ", _oFontStyle));
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.BuyerCPPhone, _oFontStyle));

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.BorderWidthTop = 0;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            return oPdfPTable;
        }
        private PdfPTable ExportDOFirstRowRight()
        {
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 200f, 60f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("DO. NO. DOF-" + _oFabricDeliveryOrder.DONo + " DATE : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricDeliveryOrder.DODateSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            int nLCID=0;
            foreach (ExportPI oItem in _oExportPIs)
            {
                if (oItem.LCID > 0 && nLCID!=oItem.LCID)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase("L/C NO : " + oItem.ExportLCNo + " DATE : ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AmendmentDate.ToString("dd MMM yyyy"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();

                    if (oItem.AmendmentNo > 0)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase("AMENDMENT NO : " + oItem.AmendmentNo + " DATE : ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.Border = 0;
                        oPdfPTable.AddCell(_oPdfPCell);

                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.AmendmentDate.ToString("dd MMM yyyy"), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.Border = 0;
                        oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                    }

                    //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                    //_oPdfPCell = new PdfPCell(new Phrase("LAST SHIPMENT DATE : ", _oFontStyle));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0;
                    //oPdfPTable.AddCell(_oPdfPCell);

                    //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                    //_oPdfPCell = new PdfPCell(new Phrase(oItem.AmendmentDate.ToString("dd MMM yyyy"), _oFontStyle));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0;
                    //oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
                nLCID = oItem.LCID;
            }

            #region Export PI List
            if (_oExportPIs.Count > 0)
            {

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("DATE", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                foreach (ExportPI oItem1 in _oExportPIs)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem1.PINo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem1.IssueDateInString, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
            }
            #endregion

            return oPdfPTable;
        }
        private PdfPTable SignaturePart()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 198f, 198f, 198f });

            #region Part

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("For " + _oCompany.Name, _oFontStyle)); _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 2; 
            _oPdfPCell.Border = 0;_oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
          
            if (_oFabricDeliveryOrder.Signature == null)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            else
            {
                iTextSharp.text.Image oImag;
                oImag = iTextSharp.text.Image.GetInstance(_oFabricDeliveryOrder.Signature, System.Drawing.Imaging.ImageFormat.Jpeg);
                oImag.ScaleAbsolute(90f, 50f);
                _oPdfPCell = new PdfPCell(oImag);
            }
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase(_oFabricDeliveryOrder.PreparedByName+" \n ________________________ \n Prepared By", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
          
            _oPdfPCell = new PdfPCell(new Phrase("" + " \n ________________________ \n Checked By", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricDeliveryOrder.ApproveByName + " \n ________________________ \n Authorized By", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase("DGM (Sales & Marketing)", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
            //oPdfPTable1.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }

        private PdfPTable Remarks()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 200f });

            _oPhrase = new Phrase();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPhrase.Add(new Chunk("REMARKS :   ", _oFontStyle));
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.Note, _oFontStyle));


            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.FixedHeight = 50f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        #region Report Header
        private void ReporttHeader()
        {
            string sHeaderName = "";
            #region Proforma Invoice Heading Print
         
            sHeaderName = _oFabricDeliveryOrder.FDOTypeInSt.ToUpper()+" DELIVERY ORDER";
      
            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            if (_oFabricDeliveryOrder.ApproveBy == 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Unauthorised Order", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            #endregion
        }
        #endregion


        #region Report Body
       
       
       
        private void PrintFooter()
        {

            nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            if (nUsagesHeight < 640)
            {
                nUsagesHeight = 640 - nUsagesHeight;
            }
            //else
            //{
            //    #region Continue
            //    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            //    _oPdfPCell = new PdfPCell(new Phrase("Continue..", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = iTextSharp.text.Rectangle.TOP_BORDER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //    #endregion

            //    //_oDocument.Add(_oPdfPTable);
            //    _oDocument.NewPage();
            //    //_oPdfPTable.DeleteBodyRows();
            //    nUsagesHeight = 0.0f;


            //}
            if (nUsagesHeight > 2)
            {
                #region Blank Row
              
              
                    while (nUsagesHeight < 640)
                    {
                        #region Table Initiate
                        PdfPTable oPdfPTableTemp = new PdfPTable(4);
                        oPdfPTableTemp.SetWidths(new float[] { 148f, 148f, 148f, 148f });

                        #endregion

                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);


                        oPdfPTableTemp.CompleteRow();

                        _oPdfPCell = new PdfPCell(oPdfPTableTemp);
                      _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();

                        nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                    }
                
                #endregion
            }

            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);
            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 148f, 148f, 148f, 148f });

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 4f;
            oPdfPCell.Colspan = 4;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


         

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Account Holder", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 4;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPTable.CompleteRow();
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 35f;
            oPdfPCell.Colspan = 4;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            //oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 35f;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(oPdfPCell);


            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            //oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 35f;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            //oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 35f;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(_oFabricDeliveryOrder.PreparedByName, _oFontStyle_UnLine));
            //oPdfPCell.FixedHeight = 30f;
            oPdfPCell.Colspan = 2;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);




            oPdfPCell = new PdfPCell(new Phrase(_oFabricDeliveryOrder.ApproveByName, _oFontStyle_UnLine));
            //oPdfPCell.FixedHeight = 30f;
            oPdfPCell.Colspan = 2;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyleBold));
            //oPdfPCell.FixedHeight = 5f;
            oPdfPCell.Colspan = 2;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


    

            oPdfPCell = new PdfPCell(new Phrase("Authorised Signature", _oFontStyleBold));
            //oPdfPCell.FixedHeight = 5f;
            oPdfPCell.Colspan = 2;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

         

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion

       


      
        private void Blank(int nFixedHeight)
        {

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 1; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nFixedHeight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        public static float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        table.TotalWidth = 500f;
                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }
    }
}
