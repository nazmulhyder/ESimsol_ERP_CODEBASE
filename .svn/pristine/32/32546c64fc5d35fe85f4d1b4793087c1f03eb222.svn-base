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
 
 

namespace ESimSol.Reports
{
    public class rptGUProductionSummery
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Image _oImag;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(3);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<GUProductionOrder> _oGUProductionOrders = new List<GUProductionOrder>();
        Company _oCompany = new Company();
        OrderRecap _oOrderRecap = new OrderRecap();
        float _nPageWidth = 0;
        #endregion

        #region Constructor
        public rptGUProductionSummery() { }
        #endregion

        public byte[] PrepareReport(List<GUProductionOrder> oGUProductionOrders, Company oCompany)
        {
            _oGUProductionOrders = oGUProductionOrders;
            _oCompany = oCompany;
            
            #region Page Setup
            _nPageWidth = (this.GetMaxStep(oGUProductionOrders) * 40 * 3) + 215;
            _oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(new iTextSharp.text.Rectangle(_nPageWidth, 595f));//842*595
            _oDocument.SetMargins(30f, 30f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            float[] TempColumnSize = new float[3];
            TempColumnSize[0] = _nPageWidth/5;
            TempColumnSize[1] = _nPageWidth / 5;
            TempColumnSize[2] = _nPageWidth / 5;
         
            _oPdfPTable.SetWidths(TempColumnSize);
            #endregion
            
            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(160f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);              
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                _oPdfPTable.AddCell(_oPdfPCell);               
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));              
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);                
            }            
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Production Summery", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;           
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;            
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion


            _oPdfPTable.CompleteRow();
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            if (_oGUProductionOrders.Count > 0)
            {
                int nBuyerID = 0;
                foreach (GUProductionOrder oGUProductionOrder in _oGUProductionOrders)
                {
                    if (oGUProductionOrder.GUProductionTracingUnits.Count > 0)
                    {
                        if (nBuyerID != oGUProductionOrder.BuyerID)
                        {
                            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                            _oPdfPCell = new PdfPCell(new Phrase(" Buyer Name: "+ oGUProductionOrder.BuyerName, _oFontStyle));
                            _oPdfPCell.FixedHeight = 37; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();
                            nBuyerID = oGUProductionOrder.BuyerID;
                        }

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                        _oPdfPCell = new PdfPCell(new Phrase("Order No :" + oGUProductionOrder.GUProductionOrderNo + " || Order Qty :" + Global.MillionFormat(oGUProductionOrder.TotalQty) + " || Style No :" + oGUProductionOrder.StyleNo + " || Recap Qty:" + oGUProductionOrder.RecapQty + " || Production Factory :" + oGUProductionOrder.ProductionFactoryName + " || Winding Status :" + oGUProductionOrder.WindingStatus.ToString(), _oFontStyle));
                        _oPdfPCell.FixedHeight = 18; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(GetGUProductionOrderWisePdfTable(oGUProductionOrder));
                        _oPdfPCell.Colspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                    }
                    
                }
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 20f, iTextSharp.text.Font.BOLD, BaseColor.RED);
                _oPdfPCell = new PdfPCell(new Phrase("Your Selected Order Recap Yet to Send Porduction Factory!", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 100; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }
        
        private PdfPTable GetGUProductionOrderWisePdfTable(GUProductionOrder oGUProductionOrder)
        {
            #region Declare PdfTable
            int nDynamicColumn = oGUProductionOrder.GUProductionProcedures.Count;           
            int nFiexdColumn = (nDynamicColumn * 3);
            nFiexdColumn = nFiexdColumn + 4;
            
            PdfPTable oPdfPTable = new PdfPTable(nFiexdColumn);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            float[] TempColumnSize = new float[nFiexdColumn];
            TempColumnSize[0] = 25f;
            TempColumnSize[1] = 70f;
            TempColumnSize[2] = 30f;
            TempColumnSize[3] = 30f;            
            int count = 3;
            for (int i = 0; i < (nDynamicColumn*3); i++)
            {
                count++;
                TempColumnSize[count] = ((_nPageWidth - 155) / (nDynamicColumn * 3));
            }
            oPdfPTable.SetWidths(TempColumnSize);
            #endregion

            #region Hading
            #region 1st Portion
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color Name", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            foreach (GUProductionProcedure oItem in oGUProductionOrder.GUProductionProcedures)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.StepName, _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);            
            }
            oPdfPTable.CompleteRow();
            #endregion

            #region 2nd Portion
            foreach (GUProductionProcedure oItem in oGUProductionOrder.GUProductionProcedures)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Start Date", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Execute Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Balance Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            #endregion
            #endregion

            #region Color Wise Details
            int nCount = 0;
            foreach (GUProductionTracingUnit oGUProductionTracingUnit in oGUProductionOrder.GUProductionTracingUnits)
            {
                nCount++;
                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oGUProductionTracingUnit.ColorName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oGUProductionTracingUnit.MeasurementUnitName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oGUProductionTracingUnit.OrderQty.ToString(), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                GUProductionTracingUnitDetail oGUProductionTracingUnitDetail = new GUProductionTracingUnitDetail();
                foreach (GUProductionProcedure oItem in oGUProductionOrder.GUProductionProcedures)
                {

                    oGUProductionTracingUnitDetail = this.GetPTUDetail(oGUProductionTracingUnit.GUProductionTracingUnitDetails, oItem.ProductionStepID);
                    _oPdfPCell = new PdfPCell(new Phrase(oGUProductionTracingUnitDetail.ExecutionStartDate.ToString("dd MMM yy"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oGUProductionTracingUnitDetail.ExecutionQty,0), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oGUProductionTracingUnitDetail.YetToExecutionQty,0), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                oPdfPTable.CompleteRow();
            }
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Total:", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            foreach (GUProductionProcedure oItem in oGUProductionOrder.GUProductionProcedures)
            {

                
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(GetTotalQty(oGUProductionOrder.GUProductionTracingUnits,oItem.ProductionStepID,true), 0), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(GetTotalQty(oGUProductionOrder.GUProductionTracingUnits, oItem.ProductionStepID, false), 0), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();


            return oPdfPTable;
        }

        private double GetTotalQty(List<GUProductionTracingUnit> oGUProductionTracingUnits, int nProductionStepID, bool IsExecutionQty )
        {
            double nTotalQty = 0;

            foreach (GUProductionTracingUnit oItem in oGUProductionTracingUnits)
            {
                nTotalQty = nTotalQty + GetDetailWiseTotal(oItem.GUProductionTracingUnitDetails, nProductionStepID, IsExecutionQty);  
            }

            return nTotalQty;
        }

        private double GetDetailWiseTotal(List<GUProductionTracingUnitDetail> oGUProductionTracingUnitDetails, int nProductionStepID, bool IsExecutionQty)
        {
            double nDetailTotal = 0;
            foreach (GUProductionTracingUnitDetail oItem in oGUProductionTracingUnitDetails)
            {
                if (oItem.ProductionStepID == nProductionStepID)
                {
                    if (IsExecutionQty == true)
                    {
                        nDetailTotal += oItem.ExecutionQty;
                    }
                    else
                    {
                        nDetailTotal += oItem.YetToExecutionQty;
                    }
                    
                }
            }
            return nDetailTotal;
        }

        private GUProductionTracingUnitDetail GetPTUDetail(List<GUProductionTracingUnitDetail> oGUProductionTracingUnitDetails, int nProductionStepID)
        {
            GUProductionTracingUnitDetail oPTUDetail = new GUProductionTracingUnitDetail();
            foreach (GUProductionTracingUnitDetail oPTUD in oGUProductionTracingUnitDetails)
            {
                if (oPTUD.ProductionStepID == nProductionStepID)
                {
                    return oPTUD;
                }
            }
            return oPTUDetail;
        }

        private int GetMaxStep(List<GUProductionOrder> oGUProductionOrders)
        {
            int nMaxStep = 0;
            foreach (GUProductionOrder oItem in oGUProductionOrders)
            {
                if (oItem.GUProductionProcedures.Count > nMaxStep)
                {
                    nMaxStep = oItem.GUProductionProcedures.Count;
                }
            }
            return nMaxStep;
        }
        #endregion
    }
}
