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
    public class rptDyeingOrder
    {
        #region Declaration
        int _nTotalColumn = 1;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        DyeingOrder _oDyeingOrder = new DyeingOrder();
        DUOrderSetup _oDUOrderSetup = new DUOrderSetup();
        DUDyeingStep _oDUDyeingStep = new DUDyeingStep();
        List<DyeingOrderDetail> _oDyeingOrderDetails = new List<DyeingOrderDetail>();
        List<DyeingOrderDetail> _oDyeingOrderDetails_Previous = new List<DyeingOrderDetail>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        ExportSCDO _oExportSCDO = new ExportSCDO();
        List<DyeingOrderNote> _oDyeingOrderNotes = new List<DyeingOrderNote>();
        List<DUColorCombo> _oDUColorCombos_Group = new List<DUColorCombo>();
        Company _oCompany = new Company();
        LightSource _oLightSource = new LightSource();
        Phrase _oPhrase = new Phrase();
        string _sMUnit = "";
        string _sWaterMark = "";
        int _nCount = 0;
        int _nCount_P = 0;
        double _nTotalAmount = 0;
        double _nTotalQty = 0;
        float nUsagesHeight = 0;
        double _nGrandTotalAmount = 0;
        double _nGrandTotalQty = 0;
        double _nTotalCurrenyPOQty = 0;
        #endregion

        public byte[] PrepareReport(DyeingOrder oDyeingOrder, Company oCompany, BusinessUnit oBusinessUnit, List<DyeingOrderDetail> oDyeingOrderDetails_Previous, ExportSCDO oExportSCDO, DUOrderSetup oDUOrderSetup, List<DyeingOrderNote> oDyeingOrderNotes)
        {
            _oDUOrderSetup = oDUOrderSetup;
            _oDyeingOrder = oDyeingOrder;
            _oBusinessUnit = oBusinessUnit;
            _oDyeingOrderNotes = oDyeingOrderNotes;
            _oDyeingOrderDetails = oDyeingOrder.DyeingOrderDetails;
            _oDyeingOrderDetails_Previous = oDyeingOrderDetails_Previous;
            _oCompany = oCompany;
            _oExportSCDO = oExportSCDO;
            if (_oDyeingOrderDetails.Count>0)
            {
                _sMUnit = _oDyeingOrderDetails[0].MUnit;
            }
           
            #region Page Setup

            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(25f, 25f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            this.ReporttHeader();
            this.PrintWaterMark(25f, 25f, 30f, 30f);



            if (_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.DyeingOnly)
            {
                this.PrintBody_DyeingOnly();
            }
            else if (_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.LoanOrder)
            {
                this.PrintBody_LoanOrder();
            }
            else
            {
                this.PrintBody();
            }
            _oPdfPTable.HeaderRows = 4;

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
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, iTextSharp.text.Font.BOLD);
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
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        #endregion

        #region Report Header
        private void ReporttHeader()
        {
            string sHeaderName = "";
            #region Proforma Invoice Heading Print
            if (_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.BulkOrder)
            {
                sHeaderName = "PRODUCTION ORDER";
            }
            else if (_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.SampleOrder)
            {
                sHeaderName = "SAMPLE ORDER";
            }
            else
            {
                sHeaderName = _oDUOrderSetup.PrintName;
            }

            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            //if (_oDyeingOrder.ApproveBy == 0)
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("Unauthorised Order", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //    _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    _oPdfPCell.Colspan = _nTotalColumn;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //}
            //else if (_oDyeingOrder.Status == (int)EnumDyeingOrderState.Cancelled)
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("Cancelled Order", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //    _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    _oPdfPCell.Colspan = _nTotalColumn;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //}
            #endregion
        }
        #endregion


        #region Report Body
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, 4);


            #region Short Info
            if (_oDyeingOrder.DyeingOrderType == (int)(EnumOrderType.BulkOrder))
            {
                _oPdfPCell = new PdfPCell(this.PrintHead_PI());
            }
            else
            {
                _oPdfPCell = new PdfPCell(this.PrintHead_Sample());
            }
            
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Short Info
            //_oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail());
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();


            _nTotalAmount = 0;
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 175f, 85f, 110f, 72f, 45f, 62f, 108f });
            int nProductID = 0;
            _nTotalQty = 0;
            _nTotalAmount = 0;
            _nCount = 0;
            _oDyeingOrderDetails = _oDyeingOrderDetails.OrderBy(o => o.ProductID).ToList();

            oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Lab-Dip No", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("COLOR NO", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("SHADE", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _sMUnit + ")", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            foreach (DyeingOrderDetail oDyeingOrderDetail in _oDyeingOrderDetails)
            {
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] { 175f, 85f, 110f, 72f, 45f, 62f, 108f });

                if (nProductID != oDyeingOrderDetail.ProductID)
                {
                    #region Header
                    if (nProductID > 0 && _nCount_P > 1)
                    {
                        #region Total
                        //oPdfPTable = new PdfPTable(7);
                        //oPdfPTable.SetWidths(new float[] { 175f, 85f, 82f, 72f, 55f, 68f, 110f });
                        _oPdfPCell = new PdfPCell(new Phrase("Yarn Total: ", _oFontStyleBold));
                        _oPdfPCell.Colspan = 5;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();

                        //_oPdfPCell = new PdfPCell(oPdfPTable);
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        //_oPdfPTable.CompleteRow();
                        #endregion
                      
                    }
                    _nTotalQty = 0;
                    _nTotalAmount = 0;
                    _nCount = 0;
                    _nCount_P = 0;
                    #endregion
                }
                #region PrintDetail
                _nCount++;
                if (nProductID != oDyeingOrderDetail.ProductID )
                {
                    _nCount_P = _oDyeingOrderDetails.Where(x => x.ProductID == oDyeingOrderDetail.ProductID).Count();
                    _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ProductName, FontFactory.GetFont("Tahoma", 11f, 1)));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.MinimumHeight = 40f;
                    //_oPdfPCell.Rowspan = 5;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable.AddCell(_oPdfPCell);
                  
                }
                //else  if (_nCount == 9)
                //{
                //    _nCount_P = _oDyeingOrderDetails.Where(x => x.ProductID == oDyeingOrderDetail.ProductID).Count();
                //    _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ProductName, FontFactory.GetFont("Tahoma", 11f, 1)));
                //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //    _oPdfPCell.FixedHeight = 70f;
                //    //_oPdfPCell.Rowspan = 5;
                //    _oPdfPCell.Border = 0;
                //    _oPdfPCell.BorderWidthLeft = 0.5f;
                //    _oPdfPCell.BorderWidthTop = 0;
                //    _oPdfPCell.BorderWidthBottom = 0;
                //    oPdfPTable.AddCell(_oPdfPCell);
                //    _nCount_P = 0;
                //}
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0;
                    _oPdfPCell.BorderWidthBottom = 0;
                    //_oPdfPCell.Rowspan = 17;
                    oPdfPTable.AddCell(_oPdfPCell);
                }

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                if (oDyeingOrderDetail.LabDipType == (int)EnumLabDipType.AVL)
                {
                    oDyeingOrderDetail.LabdipNo = oDyeingOrderDetail.LabDipTypeSt;
                }
                if (oDyeingOrderDetail.LabDipType == (int)EnumLabDipType.DTM)
                {
                    if (!String.IsNullOrEmpty(oDyeingOrderDetail.PantonNo))
                    {
                        oDyeingOrderDetail.LabdipNo = oDyeingOrderDetail.PantonNo;
                    }
                 
                }
                if (!String.IsNullOrEmpty(oDyeingOrderDetail.ApproveLotNo))
                {
                    oDyeingOrderDetail.LabdipNo = oDyeingOrderDetail.LabdipNo + " " + oDyeingOrderDetail.ApproveLotNo;
                }
                if (oDyeingOrderDetail.LabDipType == (int)EnumLabDipType.TBA)
                {
                    if (String.IsNullOrEmpty(oDyeingOrderDetail.ColorNo))
                    {
                        oDyeingOrderDetail.ColorNo = oDyeingOrderDetail.LabDipTypeSt;
                    }
                }

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.LabdipNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ShadeSt, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);
                
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);
                
                Paragraph oPdfParagraph;
                if (oDyeingOrderDetail.Note == null) { oDyeingOrderDetail.Note = ""; }
                if (oDyeingOrderDetail.Note.Length > 50)
                {
                    oPdfParagraph = new Paragraph(new Phrase(oDyeingOrderDetail.Note, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                }
                else
                {
                    oPdfParagraph = new Paragraph(new Phrase(oDyeingOrderDetail.Note, _oFontStyle));
                }
                oPdfParagraph.SetLeading(0f, 1f);
                oPdfParagraph.Alignment = Element.ALIGN_LEFT;
                _oPdfPCell = new PdfPCell();
                _oPdfPCell.AddElement(oPdfParagraph);
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                oPdfPTable.AddCell(_oPdfPCell);


                _nTotalAmount = _nTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                _nTotalQty = _nTotalQty + oDyeingOrderDetail.Qty;
                _nGrandTotalAmount = _nGrandTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                _nGrandTotalQty = _nGrandTotalQty + oDyeingOrderDetail.Qty;
                oPdfPTable.CompleteRow();
                #endregion

                nProductID = oDyeingOrderDetail.ProductID;

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #region Total
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 175f, 85f, 110f, 72f, 45f, 62f, 108f });
            if (_nCount_P > 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Yarn Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 5;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Grand Total
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 175f, 85f, 110f, 72f, 45f, 62f, 108f });
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
            _oPdfPCell.Colspan = 5;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            _nTotalCurrenyPOQty = _nGrandTotalQty;
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region In Word
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 175f, 85f, 110f, 72f, 45f, 62f, 108f });
            string sTemp = "";
            sTemp = Global.DollarWords(_nGrandTotalQty);
            if (!String.IsNullOrEmpty(sTemp))
            {
                sTemp = sTemp.Replace("Dollar", "");
                sTemp = sTemp.Replace("Only", "");
                sTemp = sTemp.ToUpper();
            }
            _oPdfPCell = new PdfPCell(new Phrase("IN WORDS: " + sTemp + " " + _sMUnit + " ONLY", _oFontStyleBold));
            _oPdfPCell.Colspan = 7;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
           
            #endregion
            #region Note
            if (!string.IsNullOrEmpty(_oDyeingOrder.Note))
            {
                _oPdfPCell = new PdfPCell(new Phrase("Note: " + _oDyeingOrder.Note, _oFontStyleBold));
                _oPdfPCell.Colspan = 7;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
            }

            string sTempT = "";
            if (!string.IsNullOrEmpty(_oDyeingOrder.StripeOrder))
            {
                sTempT = "StripeOrder: " + _oDyeingOrder.StripeOrder;
            }
            if (!string.IsNullOrEmpty(_oDyeingOrder.StripeOrder))
            {
                sTempT = sTempT + " KnittingStyle: " + _oDyeingOrder.KnittingStyle;
            }
            if (!string.IsNullOrEmpty(_oDyeingOrder.Gauge))
            {
                sTempT = sTempT + " Gauge: " + _oDyeingOrder.Gauge;
            }
            if (!string.IsNullOrEmpty(sTempT))
            {
                _oPdfPCell = new PdfPCell(new Phrase(sTempT, _oFontStyle));
                _oPdfPCell.Colspan = 7;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            #region Delivery Date
            if (_oDyeingOrder.DyeingOrderType == (int)(EnumOrderType.BulkOrder))
            {
                _oPdfPCell = new PdfPCell(this.Set_DeliveryDate());
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region
            if (_oDyeingOrderNotes.Count > 0)
            {
                string sNotes = "";
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.MinimumHeight = 5f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Special instructions:", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthRight = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _nCount = 0;
                foreach (DyeingOrderNote oItem in _oDyeingOrderNotes)
                {
                    _nCount++;
                    sNotes = sNotes + _nCount.ToString() + ". " + oItem.OrderNote + "\n";
                }


                _oPdfPCell = new PdfPCell(new Phrase(sNotes, _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion          

            #region Balank Space
            _oPdfPCell = new PdfPCell(this.PrintFooter());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Print Previous DO for a particular PI
            if (_oDyeingOrder.DyeingOrderType == (int)(EnumOrderType.BulkOrder))
            {
                _oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail_Previous());
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
        }
        #endregion
        #region Report Body Redyeing
        private void PrintBody_DyeingOnly()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);


            #region Short Info
            if (_oDyeingOrder.DyeingOrderType == (int)(EnumOrderType.BulkOrder) || _oDyeingOrder.DyeingOrderType == (int)(EnumOrderType.DyeingOnly))
            {
                _oPdfPCell = new PdfPCell(this.PrintHead_PI());
            }
            else
            {
                _oPdfPCell = new PdfPCell(this.PrintHead_Sample());
            }

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Short Info
            //_oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail());
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();


            _nTotalAmount = 0;
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(9);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 140f, 90f, 80f, 75f, 40, 63f, 70f, 70f, 40f});
            int nProductID = 0;
            _nTotalQty = 0;
            _nTotalAmount = 0;
            _nCount = 0;
            _oDyeingOrderDetails = _oDyeingOrderDetails.OrderBy(o => o.ProductID).ToList();

            oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("COLOR NAME", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

           


            oPdfPCell = new PdfPCell(new Phrase("Lab-Dip No", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("COLOR NO", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("SHADE", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("QTY(" + _sMUnit + ")", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPCell = new PdfPCell(new Phrase("STYLE NO", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("KNITTING STYLE", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("SIZE", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            foreach (DyeingOrderDetail oDyeingOrderDetail in _oDyeingOrderDetails)
            {
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] { 140f, 90f, 80f, 75f, 40, 63f, 70f, 70f, 40f });

                if (nProductID != oDyeingOrderDetail.ProductID)
                {
                    #region Header
                    if (nProductID > 0 && _nCount_P > 1)
                    {
                        #region Total
                        //oPdfPTable = new PdfPTable(7);
                        //oPdfPTable.SetWidths(new float[] { 175f, 85f, 82f, 72f, 55f, 68f, 110f });
                        _oPdfPCell = new PdfPCell(new Phrase("Yarn Total: ", _oFontStyleBold));
                        _oPdfPCell.Colspan = 5;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.Colspan = 3;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        //_oPdfPCell = new PdfPCell(oPdfPTable);
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        //_oPdfPTable.CompleteRow();
                        #endregion

                    }
                    _nTotalQty = 0;
                    _nTotalAmount = 0;
                    _nCount = 0;
                    _nCount_P = 0;
                    #endregion
                }
                #region PrintDetail
                _nCount++;
                if (nProductID != oDyeingOrderDetail.ProductID)
                {
                    _nCount_P = _oDyeingOrderDetails.Where(x => x.ProductID == oDyeingOrderDetail.ProductID).Count();
                    _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ProductName, FontFactory.GetFont("Tahoma", 11f, 1)));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.MinimumHeight = 40f;
                    //_oPdfPCell.Rowspan = 5;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable.AddCell(_oPdfPCell);

                }
                //else  if (_nCount == 9)
                //{
                //    _nCount_P = _oDyeingOrderDetails.Where(x => x.ProductID == oDyeingOrderDetail.ProductID).Count();
                //    _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ProductName, FontFactory.GetFont("Tahoma", 11f, 1)));
                //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //    _oPdfPCell.FixedHeight = 70f;
                //    //_oPdfPCell.Rowspan = 5;
                //    _oPdfPCell.Border = 0;
                //    _oPdfPCell.BorderWidthLeft = 0.5f;
                //    _oPdfPCell.BorderWidthTop = 0;
                //    _oPdfPCell.BorderWidthBottom = 0;
                //    oPdfPTable.AddCell(_oPdfPCell);
                //    _nCount_P = 0;
                //}
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0;
                    _oPdfPCell.BorderWidthBottom = 0;
                    //_oPdfPCell.Rowspan = 17;
                    oPdfPTable.AddCell(_oPdfPCell);
                }
                /// 2
                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                if (oDyeingOrderDetail.LabDipType == (int)EnumLabDipType.AVL)
                {
                    oDyeingOrderDetail.LabdipNo = oDyeingOrderDetail.LabDipTypeSt;
                }
                if (oDyeingOrderDetail.LabDipType == (int)EnumLabDipType.DTM)
                {
                    if (!String.IsNullOrEmpty(oDyeingOrderDetail.PantonNo))
                    {
                        oDyeingOrderDetail.LabdipNo = oDyeingOrderDetail.PantonNo;
                    }
                 
                }
                if (!String.IsNullOrEmpty(oDyeingOrderDetail.ApproveLotNo))
                {
                    oDyeingOrderDetail.LabdipNo = oDyeingOrderDetail.LabdipNo + " " + oDyeingOrderDetail.ApproveLotNo;
                }
                if (oDyeingOrderDetail.LabDipType == (int)EnumLabDipType.TBA)
                {
                    if (String.IsNullOrEmpty(oDyeingOrderDetail.ColorNo))
                    {
                        oDyeingOrderDetail.ColorNo = oDyeingOrderDetail.LabDipTypeSt;
                    }
                }

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.LabdipNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ShadeSt, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                //Qty 6
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);
                //--7
               
                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.BuyerRef, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);
                //Knatting Style-4
                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.LengthOfCone, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);
                //---Size 5
                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.NoOfCone_Weft, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                ///--8
                ///


                //Paragraph oPdfParagraph;
                //if (oDyeingOrderDetail.Note == null) { oDyeingOrderDetail.Note = ""; }
                //if (oDyeingOrderDetail.Note.Length > 50)
                //{
                //    oPdfParagraph = new Paragraph(new Phrase(oDyeingOrderDetail.Note, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                //}
                //else
                //{
                //    oPdfParagraph = new Paragraph(new Phrase(oDyeingOrderDetail.Note, _oFontStyle));
                //}
                //oPdfParagraph.SetLeading(0f, 1f);
                //oPdfParagraph.Alignment = Element.ALIGN_LEFT;
                //_oPdfPCell = new PdfPCell();
                //_oPdfPCell.AddElement(oPdfParagraph);
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.MinimumHeight = 40f;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                //oPdfPTable.AddCell(_oPdfPCell);


                _nTotalAmount = _nTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                _nTotalQty = _nTotalQty + oDyeingOrderDetail.Qty;
                _nGrandTotalAmount = _nGrandTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                _nGrandTotalQty = _nGrandTotalQty + oDyeingOrderDetail.Qty;
                oPdfPTable.CompleteRow();
                #endregion

                nProductID = oDyeingOrderDetail.ProductID;

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #region Total
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 140f, 90f, 80f, 75f, 40, 63f, 70f, 70f, 40f });
            if (_nCount_P > 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Yarn Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 5;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Grand Total
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 140f, 90f, 80f, 75f, 40, 63f, 70f, 70f, 40f });
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
            _oPdfPCell.Colspan = 5;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            _nTotalCurrenyPOQty = _nGrandTotalQty;
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region In Word
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 175f, 85f, 110f, 70f, 63f, 62f, 90f });
            string sTemp = "";
            sTemp = Global.DollarWords(_nGrandTotalQty);
            if (!String.IsNullOrEmpty(sTemp))
            {
                sTemp = sTemp.Replace("Dollar", "");
                sTemp = sTemp.Replace("Only", "");
                sTemp = sTemp.ToUpper();
            }
            _oPdfPCell = new PdfPCell(new Phrase("IN WORDS: " + sTemp + " " + _sMUnit + " ONLY", _oFontStyleBold));
            _oPdfPCell.Colspan = 7;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion
            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Note: " + _oDyeingOrder.Note, _oFontStyleBold));
            _oPdfPCell.Colspan = 7;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);

            string sTempT = "";
            if (!string.IsNullOrEmpty(_oDyeingOrder.StripeOrder))
            {
                sTempT = "StripeOrder: " + _oDyeingOrder.StripeOrder;
            }
            if (!string.IsNullOrEmpty(_oDyeingOrder.StripeOrder))
            {
                sTempT = sTempT + " KnittingStyle: " + _oDyeingOrder.KnittingStyle;
            }
            if (!string.IsNullOrEmpty(_oDyeingOrder.Gauge))
            {
                sTempT = sTempT + " Gauge: " + _oDyeingOrder.Gauge;
            }
            _oPdfPCell = new PdfPCell(new Phrase(sTempT, _oFontStyle));
            _oPdfPCell.Colspan = 7;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            #region Delivery Date
            if (_oDyeingOrder.DyeingOrderType == (int)(EnumOrderType.BulkOrder))
            {
                _oPdfPCell = new PdfPCell(this.Set_DeliveryDate());
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Balank Space
            _oPdfPCell = new PdfPCell(this.PrintFooter());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Print Previous DO for a particular PI
            if (_oDyeingOrder.DyeingOrderType == (int)(EnumOrderType.BulkOrder))
            {
                _oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail_Previous());
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
        }
        #endregion
        private PdfPTable PrintHead_Sample()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(5);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 110, 80f, 100f,80f,150f });

            oPdfPCell = new PdfPCell(new Phrase("DATE", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.OrderDateSt +( _oDyeingOrder.ReviseNo>0? " Revise:" + _oDyeingOrder.ReviseDateSt:""), _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("STYLE NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.StyleNo, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("ORDER NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.OrderNoFull, FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD)));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("BUYER REF", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.RefNo, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("BUYER NAME", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.ContractorName, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("MASTER BUYER", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.MBuyer, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 4;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            //oPdfPCell = new PdfPCell(new Phrase("MASTER BUYER", _oFontStyle));
            ////oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrder.MBuyer, _oFontStyle));
          
            ////oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

           // oPdfPCell = new PdfPCell(new Phrase("REQ. DT.", _oFontStyle));
           // //oPdfPCell.Border = 0;
           // oPdfPCell.BackgroundColor = BaseColor.WHITE;
           // oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
           // oPdfPTable.AddCell(oPdfPCell);
           // if (_oDyeingOrderDetails.Count>0)
           // {
           //     oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrderDetails[0].DeliveryDateSt, _oFontStyle));
           // }
           // else {
           //     oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           // }
            
           // //oPdfPCell.Border = 0;
           //// oPdfPCell.Colspan = 4;
           // oPdfPCell.BackgroundColor = BaseColor.WHITE;
           // oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
           // oPdfPTable.AddCell(oPdfPCell);
           // oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.ShortName+" CONCERN PERSON", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.MKTPName, _oFontStyle));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

             oPdfPCell = new PdfPCell(new Phrase("REQ. DT.", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            if (_oDyeingOrderDetails.Count>0)
            {
                oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrderDetails[0].DeliveryDateSt, _oFontStyle));
            }
            else {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            
            //oPdfPCell.Border = 0;
           // oPdfPCell.Colspan = 4;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("BUYER CONCERN PERSON", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.ContactPersonnelName, _oFontStyle));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Contract No ", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.DeliveryNote, FontFactory.GetFont("Tahoma", 10f, 0)));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("MODE OF PAYMENT", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            string sTemp = "";
            string sTempTwo = "";
            if (_oDyeingOrder.PaymentType==(int)EnumOrderPaymentType.CashOrCheque)
            {
                 sTemp = "Cheque or Cash";
                if(!string.IsNullOrEmpty(_oDyeingOrder.ExportPINo))
                {
                    sTempTwo = "MR No:" + _oDyeingOrder.ExportPINo;
                }
            }
            if (_oDyeingOrder.PaymentType == (int)EnumOrderPaymentType.AdjWithNextLC)
            {
                sTemp = "NEXT L/C ADJUST ";
                
                if (!string.IsNullOrEmpty(_oDyeingOrder.ExportPINo))
                {
                    sTempTwo = "P/I No:" + _oDyeingOrder.ExportPINo;
                }
            }
            if (_oDyeingOrder.PaymentType == (int)EnumOrderPaymentType.AdjWithPI)
            {
                sTemp = "Adj With PI ";
                if (!string.IsNullOrEmpty(_oDyeingOrder.ExportPINo))
                {
                    sTempTwo = "P/I No:" + _oDyeingOrder.ExportPINo;
                }
            }
            if (_oDyeingOrder.PaymentType == (int)EnumOrderPaymentType.FoC)
            {
                    sTemp = "Free Of Cost";
                    if (!string.IsNullOrEmpty(_oDyeingOrder.ExportPINo))
                    {
                        sTempTwo = "MR No:" + _oDyeingOrder.ExportPINo;
                    }
            }

            oPdfPCell = new PdfPCell(new Phrase(" " + sTemp + " " + sTempTwo, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 4;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            


            return oPdfPTable;
        }

        private void PrintWaterMarkFor_B(float marginLeft, float marginRight, float marginTop, float marginBottom)
        {
            if (_oDyeingOrder.ApproveBy == 0)
            {
                _sWaterMark = "Unauthorised";
            }
            if (_oDyeingOrder.Status == (int)EnumDyeingOrderState.Cancelled)
            {
                _sWaterMark = "Cancelled";
            }
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(marginLeft, marginRight, marginTop, marginBottom);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            ESimSolFooter PageEventHandler = new ESimSolFooter(_oBusinessUnit.ShortName + " Concern Person: " + _oDyeingOrder.MKTPName, "Left");
            PageEventHandler.nFontSize = 8;
            PdfWriter.PageEvent = PageEventHandler;

            _oDocument.Open();
            PdfContentByte cb = PdfWriter.DirectContent;
            _oDocument.NewPage();
        }
        private void PrintWaterMark(float marginLeft, float marginRight, float marginTop, float marginBottom)
        {
            if (_oDyeingOrder.ApproveBy == 0)
            {
                _sWaterMark = "Unauthorised";
            }
            if (_oDyeingOrder.Status == (int)EnumDyeingOrderState.Cancelled)
            {
                _sWaterMark = "Cancelled";
            }
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(marginLeft, marginRight, marginTop, marginBottom);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;

            ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            ESimSolWM_Footer.WMFontSize = 80;
            ESimSolWM_Footer.WMRotation = 70;
            ESimSolWM_Footer PageEventHandler = new ESimSolWM_Footer();
            PageEventHandler.WaterMark = _sWaterMark; //Footer print with page event handler
            PageEventHandler.FooterNote = _oBusinessUnit.ShortName + " Concern Person: " + _oDyeingOrder.MKTPName;
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
            PdfContentByte cb = PDFWriter.DirectContent;
            _oDocument.NewPage();
        }
        private PdfPTable PrintHead_PI()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(5);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100, 80f, 100f, 80f, 160f });

            oPdfPCell = new PdfPCell(new Phrase("DATE", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.OrderDateSt + (_oDyeingOrder.ReviseNo > 0 ? " Revise:" + _oDyeingOrder.ReviseDateSt : ""), _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("STYLE NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.StyleNo, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

                                

            oPdfPCell = new PdfPCell(new Phrase("ORDER NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.OrderNoFull, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("BUYER REF", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.RefNo, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("BUYER NAME", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.ContractorName, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("MASTER BUYER", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.MBuyer, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 4;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            //oPdfPCell = new PdfPCell(new Phrase("Master Buyer", _oFontStyle));
            ////oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.MBuyer, _oFontStyleBold));
            ////oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("P/I NO.:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.ExportPINo, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 4;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("L/C NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.ExportLCNo, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 4;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();



            oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.ShortName+" CONCERN PERSON", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.MKTPName, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("BUYER CONCERN PERSON", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.ContactPersonnelName, _oFontStyle));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Contract No", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.DeliveryNote, _oFontStyle));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 4;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            //if (!string.IsNullOrEmpty(_oDyeingOrder.DeliveryNote))
            //{
            //    oPdfPCell = new PdfPCell(new Phrase("Delivery Note", _oFontStyle));
            //    //oPdfPCell.Border = 0;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.AddCell(oPdfPCell);

            //    oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.DeliveryNote, FontFactory.GetFont("Tahoma", 10f, 0)));
            //    //oPdfPCell.Border = 0;
            //    oPdfPCell.Colspan = 4;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.AddCell(oPdfPCell);

            //    oPdfPTable.CompleteRow();
            //}

            return oPdfPTable;
        }
        private PdfPTable Set_DeliveryDate()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(2);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 140, 200f });

            oPdfPCell = new PdfPCell(new Phrase("EXPECTED DELIVERY DATE:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            if (_oDyeingOrderDetails.Count > 0)
            {
                oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrderDetails[0].DeliveryDateSt, _oFontStyle));
            }
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
         
            return oPdfPTable;
        }
   
        private PdfPTable SetDyeingOrderDetail()
        {
            _nTotalAmount = 0;
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 175f, 85f, 82f,72f ,55f, 68f, 110f });
            int nProductID = 0;
            _nTotalQty = 0;
            _nTotalAmount = 0;
            _nCount = 0;

         
            if (_oDyeingOrderDetails.Count > 0)
            {

              

                //oPdfPCell = new PdfPCell(new Phrase("Product Information", FontFactory.GetFont("Tahoma", 10f, 3)));
                //oPdfPCell.Colspan = 6;

                //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(oPdfPCell);
                //oPdfPTable.CompleteRow();
                _oDyeingOrderDetails = _oDyeingOrderDetails.OrderBy(o => o.ProductID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
             

                oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Lab-Dip No", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("COLOR NO", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("SHADE", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _sMUnit + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                foreach (DyeingOrderDetail oDyeingOrderDetail in _oDyeingOrderDetails)
                {


                    if (nProductID != oDyeingOrderDetail.ProductID)
                    {
                        #region Header

                        if (nProductID > 0 && _nCount_P>1)
                        {
                            #region Total
                            _oPdfPCell = new PdfPCell(new Phrase("Yarn Total: ", _oFontStyleBold));
                            _oPdfPCell.Colspan = 5;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            oPdfPTable.CompleteRow();
                            #endregion
                            _nTotalQty = 0;
                            _nTotalAmount = 0;
                            _nCount = 0;
                            _nCount_P = 0;
                        }

                      
                        #endregion
                     
                    }

                    #region PrintDetail
                  
                        _nCount++;
                        if (nProductID != oDyeingOrderDetail.ProductID)
                        {
                            _nCount_P = _oDyeingOrderDetails.Where(x => x.ProductID == oDyeingOrderDetail.ProductID).Count();
                            oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ProductName, FontFactory.GetFont("Tahoma", 11f, 1)));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            oPdfPCell.FixedHeight = 40f;
                            oPdfPCell.Rowspan = _nCount_P;
                            oPdfPTable.AddCell(oPdfPCell);
                        }
                        
                        //else
                        //{
                        //    oPdfPCell = new PdfPCell(new Phrase("" ,_oFontStyle));
                        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //    oPdfPCell.FixedHeight = 30f;
                            
                        //    oPdfPTable.AddCell(oPdfPCell);
                        //}

                        oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorName, _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.FixedHeight = 40f;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(oPdfPCell);

                        if (oDyeingOrderDetail.LabDipType == (int)EnumLabDipType.AVL)
                        {
                            oDyeingOrderDetail.LabdipNo = oDyeingOrderDetail.LabDipTypeSt;
                        }
                        if (oDyeingOrderDetail.LabDipType == (int)EnumLabDipType.DTM)
                        {
                            if (!String.IsNullOrEmpty(oDyeingOrderDetail.PantonNo))
                            {
                                oDyeingOrderDetail.LabdipNo = oDyeingOrderDetail.PantonNo;
                            }
                        }
                        if (!String.IsNullOrEmpty(oDyeingOrderDetail.ApproveLotNo))
                        {
                            oDyeingOrderDetail.LabdipNo = oDyeingOrderDetail.LabdipNo + " " + oDyeingOrderDetail.ApproveLotNo;
                        }
                        oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.LabdipNo, _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.FixedHeight = 40f;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorNo, _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.FixedHeight = 40f;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ShadeSt, _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.FixedHeight = 40f;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(oPdfPCell);

                      

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.FixedHeight = 40f;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(oPdfPCell);


                     
                        oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.Note, _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.FixedHeight = 40f;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(oPdfPCell);



                        _nTotalAmount = _nTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                        _nTotalQty = _nTotalQty + oDyeingOrderDetail.Qty;
                        _nGrandTotalAmount = _nGrandTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                        _nGrandTotalQty = _nGrandTotalQty + oDyeingOrderDetail.Qty;
                        oPdfPTable.CompleteRow();
                    #endregion
                      
                    nProductID = oDyeingOrderDetail.ProductID;

                }

                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Yarn Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 5;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 5;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                _nTotalCurrenyPOQty = _nGrandTotalQty;
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

             


                oPdfPTable.CompleteRow();
                #endregion
                #region In Word
                string sTemp = "";
                sTemp = Global.DollarWords(_nGrandTotalQty);
                if (!String.IsNullOrEmpty(sTemp))
                {
                    sTemp = sTemp.Replace("Dollar", "");
                    sTemp = sTemp.Replace("Only", "");
                    sTemp = sTemp.ToUpper();
                }
                _oPdfPCell = new PdfPCell(new Phrase("IN WORDS: " + sTemp + " " + _sMUnit + " ONLY", _oFontStyleBold));
                _oPdfPCell.Colspan = 7;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

               


                oPdfPTable.CompleteRow();
                #endregion
                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("Note: " + _oDyeingOrder.Note, _oFontStyleBold));
                _oPdfPCell.Colspan = 7;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                string sTempT = "";
                if (!string.IsNullOrEmpty(_oDyeingOrder.StripeOrder))
                {
                    sTempT = "StripeOrder: " + _oDyeingOrder.StripeOrder;
                }
                if (!string.IsNullOrEmpty(_oDyeingOrder.StripeOrder))
                {
                    sTempT = sTempT + " KnittingStyle: " + _oDyeingOrder.KnittingStyle;
                }
                if (!string.IsNullOrEmpty(_oDyeingOrder.Gauge))
                {
                    sTempT = sTempT + " Gauge: " + _oDyeingOrder.Gauge;
                }
                _oPdfPCell = new PdfPCell(new Phrase(sTempT, _oFontStyle));
                _oPdfPCell.Colspan = 7;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

            }
            return oPdfPTable;
        }

        private PdfPTable PrintFooter()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 148f, 148f, 148f, 148f });




            oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
           
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPCell = new PdfPCell(new Phrase("Sr. Executive", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
           
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Marketing Manager", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            //oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Managing Director", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
           
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrder.PreaperByName, FontFactory.GetFont("Tahoma", 9f, 0)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            return oPdfPTable;
        }

        private PdfPTable SetDyeingOrderDetail_Previous()
        {
            _nTotalAmount = 0;
            _nTotalQty = 0;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfPTable oPdfPTable = new PdfPTable(5);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 120f, 150f, 80f, 80f, 120f });
            int nProductID = 0;
            _nTotalQty = 0;
            _nTotalAmount = 0;
            _nCount = 0;
            _nCount_P = 0;
            _nGrandTotalQty = 0;

            if (_oDyeingOrderDetails_Previous.Count > 0)
            {

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 5;
                oPdfPCell.FixedHeight = 10f;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                oPdfPCell = new PdfPCell(new Phrase("Order History", _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
                oPdfPCell.Colspan = 4;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                oPdfPCell.Border = 0;
                oPdfPCell.FixedHeight = 10f;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

            


                _oDyeingOrderDetails = _oDyeingOrderDetails.OrderBy(o => o.ProductID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("Yarn", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _sMUnit + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                foreach (DyeingOrderDetail oDyeingOrderDetail in _oDyeingOrderDetails_Previous)
                {


                 

                    #region PrintDetail

                    _nCount++;

                    oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.OrderNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    //if (nProductID != oDyeingOrderDetail.ProductID)
                    //{
                    _nCount_P = _oDyeingOrderDetails.Where(x => x.ProductID == oDyeingOrderDetail.ProductID).Count();
                    oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ProductName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //oPdfPCell.Rowspan = _nCount_P;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //oPdfPCell.Border = 0;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //oPdfPCell.Border = 0;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    _nTotalAmount = _nTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                    _nTotalQty = _nTotalQty + oDyeingOrderDetail.Qty;
                    _nGrandTotalAmount = _nGrandTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                    _nGrandTotalQty = _nGrandTotalQty + oDyeingOrderDetail.Qty;
                    oPdfPTable.CompleteRow();
                    #endregion

                    //nProductID = oDyeingOrderDetail.ProductID;

                }

                #region Total
                if (_nCount_P > 1)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Yarn Total: ", _oFontStyleBold));
                    _oPdfPCell.Colspan = 3;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                }
                #endregion

                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 3;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                
                #endregion
                
               
            }
            

            #region Balance Total Total
            _oPdfPCell = new PdfPCell(new Phrase("P/I Qty(" + _sMUnit + "): " + Global.MillionFormat(_oExportSCDO.Qty_PI) + "  Adj Qty(" + _sMUnit + "): " + Global.MillionFormat(_oExportSCDO.AdjQty + _oExportSCDO.POQty) + " Total BPO Qty(" + _sMUnit + "): " + Global.MillionFormat(_nGrandTotalQty + _nTotalCurrenyPOQty) + " Balance(" + _sMUnit + "): " + Global.MillionFormat(_oExportSCDO.Qty_PI - _oExportSCDO.AdjQty - (_nGrandTotalQty + _nTotalCurrenyPOQty)), _oFontStyleBold));
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 5;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            #endregion
            return oPdfPTable;
        }
        private void Blank(int nFixedHeight)
        {

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 1; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nFixedHeight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }


        #region Print Type B
        public byte[] PrepareReport_B(DyeingOrder oDyeingOrder, Company oCompany, BusinessUnit oBusinessUnit, DUOrderSetup oDUOrderSetup, DUDyeingStep oDUDyeingStep, List<DyeingOrderNote> oDyeingOrderNotes, List<DUColorCombo> oDUColorCombos_Group, LightSource oLightSource)
        {
            _oDUOrderSetup = oDUOrderSetup;
            _oDyeingOrder = oDyeingOrder;
            _oDUDyeingStep = oDUDyeingStep;
            _oDUColorCombos_Group = oDUColorCombos_Group;
            _oBusinessUnit = oBusinessUnit;
            _oDyeingOrderNotes = new List<DyeingOrderNote>();
            _oDyeingOrderDetails = oDyeingOrder.DyeingOrderDetails;
            _oCompany = oCompany;
            _oDyeingOrderNotes = oDyeingOrderNotes;
            _oLightSource = oLightSource;
            if (_oDyeingOrderDetails.Count > 0)
            {
                _sMUnit = _oDyeingOrderDetails[0].MUnit;
            }

            #region Page Setup

            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(PageSize.A4);
            //_oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(33f, 15f, 15f, 55f);
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            this.ReporttHeaderTwo(_oDUOrderSetup);
            //this.PrintWaterMark(33f, 15f, 0f, 60f);
            

            if (_oDyeingOrder.ApproveBy == 0)
            {
                this.PrintWaterMark(33f, 15f, 0f, 60f);
            }
            else if (_oDyeingOrder.Status == (int)EnumDyeingOrderState.Cancelled)
            {
                this.PrintWaterMark(33f, 15f, 0f, 60f);
            }
            else
            {
                this.PrintWaterMarkFor_B(33f, 33f, 10f, 25f);
            }

            this.PrintHead_B();
            if ( _oDyeingOrder.DyeingOrderType == (int)EnumOrderType.ReConing)
            {
                this.PrintBody_B_NonDyeing();
            }
            else if (_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.LoanOrder)
            {
                this.PrintBody_LoanOrder();
            }
            else if (_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.TwistOrder)
            {
                this.PrintBody_B_Twist();
            }
            else
            {
                this.PrintBody_B();
            }
            
            _oPdfPTable.HeaderRows = 2;

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Report Header
        private void ReporttHeaderTwo(DUOrderSetup oDUOrderSetup)
        {
            //string sHeaderName = "";
            #region Proforma Invoice Heading Print
            //sHeaderName ="YARN DYEING "+ oDUOrderSetup.PrintName;

            if (string.IsNullOrEmpty(_oDUDyeingStep.Name)) { _oDUDyeingStep.Name = ""; }
            if (string.IsNullOrEmpty(oDUOrderSetup.PrintName)) { oDUOrderSetup.PrintName = ""; }

            _oPdfPCell = new PdfPCell(new Phrase(_oDUDyeingStep.Name.ToUpper() + " " + oDUOrderSetup.PrintName.ToUpper(), FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            if (_oDyeingOrder.ApproveBy == 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Unauthorised Order", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else if (_oDyeingOrder.Status == (int)EnumDyeingOrderState.Cancelled)
            {

                _oPdfPCell = new PdfPCell(new Phrase("Cancelled Order", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            #endregion
        }
        #endregion
        #region
        private void PrintHead_B()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 65f, 178f, 62f, 135f });
            //// 1st Row
            if (_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.LoanOrder)
            {
                oPdfPCell = new PdfPCell(new Phrase("SUPPLIER NAME", _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("BUYER NAME", _oFontStyle));
            }
           
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.ContractorName, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            

            oPdfPCell = new PdfPCell(new Phrase("ORDER NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

           //for ea _oDUDyeingOrderSteps

            oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrder.OrderNoFull + " " + _oDUOrderSetup.ShortName, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            /// 2nd Row
            oPdfPCell = new PdfPCell(new Phrase("CONCERN PERSON", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.ContactPersonnelName, _oFontStyle));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("ISSUE DATE", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            if (_oDyeingOrder.ReviseNo > 0)
            {
                oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrder.OrderDateSt + "  REVISE DATE: " + _oDyeingOrder.ReviseDateSt, _oFontStyleBold));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrder.OrderDateSt, _oFontStyleBold));
            }
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //// 3nd Row 

            oPdfPCell = new PdfPCell(new Phrase("DELIVERY NOTE", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Rowspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.DeliveryNote, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Rowspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            //if (_oDyeingOrder.ReviseNo>0)
            //{
            //    oPdfPCell = new PdfPCell(new Phrase("REVISE DATE", _oFontStyle));
            //    //oPdfPCell.Border = 0;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.AddCell(oPdfPCell);
            //    oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrder.ReviseDateSt, _oFontStyle));
            //    //oPdfPCell.Border = 0;
            //    //oPdfPCell.Colspan = 2;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.AddCell(oPdfPCell);
            //    oPdfPTable.CompleteRow();
            //}


            oPdfPCell = new PdfPCell(new Phrase("PAYMENT TERM",  _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            string sTemp = "";
            if (_oDyeingOrder.PaymentType == (int)EnumOrderPaymentType.CashOrCheque)
            {
                sTemp = "Cheque or Cash";
            }
            if (_oDyeingOrder.PaymentType == (int)EnumOrderPaymentType.AdjWithNextLC)
            {
                sTemp = "L/C ";
            }
            if (_oDyeingOrder.PaymentType == (int)EnumOrderPaymentType.AdjWithPI)
            {
                sTemp = "Adj With PI ";
            }
            if (_oDyeingOrder.PaymentType == (int)EnumOrderPaymentType.FoC)
            {
                sTemp = "Free Of Cost";
            }
            oPdfPCell = new PdfPCell(new Phrase(" " + sTemp, _oFontStyle));
           
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //// 4th Row
            //// Use Rowspan
            if (!String.IsNullOrEmpty(_oDyeingOrder.StyleNo))
            {
                oPdfPCell = new PdfPCell(new Phrase("STYLE NO", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrder.StyleNo, _oFontStyle));
                //oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 2;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
         
            if (!String.IsNullOrEmpty(_oDyeingOrder.RefNo))
            {
                oPdfPCell = new PdfPCell(new Phrase("BUYER REF", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrder.RefNo, _oFontStyle));
                //oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 2;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
          

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion

        #region Report Body
        private void PrintBody_B()
        {

            float  nMinimumHeight = 5;

            
            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Short Info
            //_oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail());
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();


            _nTotalAmount = 0;
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(9);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] {
                 113f,//Yarn
                80f,//COLOR
                42f,//Color No
                50f,//LD No
                55f,//Yatn Lot
                50f,//Match To
                37f, //Qty
                32f,/// No of Cond
                //50f,///Edl Date
                75f// Remarks
            });
            int nProductID = 0;
            _nTotalQty = 0;
            _nTotalAmount = 0;
            _nCount = 0;
            _oDyeingOrderDetails = _oDyeingOrderDetails.OrderBy(o => o.DyeingOrderDetailID).ToList();
            _oDyeingOrderDetails = _oDyeingOrderDetails.OrderBy(o => o.SL).ToList();

            //oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("YARN TYPE", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("COLOR No(Mkt)", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("LAB-DIP", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("YARN LOT", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("MATCH NO", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("SHADE", _oFontStyleBold));
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("QTY \n(" + _sMUnit + ")", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("No of\nCone", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("DEL. DATE", _oFontStyleBold));
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            foreach (DyeingOrderDetail oDyeingOrderDetail in _oDyeingOrderDetails)
            {
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] {
               113f,//Yarn
                80f,//COLOR
                42f,//Color No
                50f,//LD No
                55f,//Yatn Lot
                50f,//Match To
                37f, //Qty
                32f,/// No of Cond
                //50f,///Edl Date
                75f// Remarks
            });

               
                #region PrintDetail
                _nCount++;
              
                    _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ProductName, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.MinimumHeight = nMinimumHeight;
                    //_oPdfPCell.Rowspan = 5;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable.AddCell(_oPdfPCell);

              
                    if (!string.IsNullOrEmpty(oDyeingOrderDetail.PantonNo))
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorName + "\n(" + oDyeingOrderDetail.PantonNo + ")", _oFontStyle));
                    }
                    else
                    {
                        if (oDyeingOrderDetail.ColorName.Contains("("))
                        {
                            oDyeingOrderDetail.ColorName = oDyeingOrderDetail.ColorName.Replace("(", "\n(");
                        }
                        _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorName, _oFontStyle));
                    }
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                if (oDyeingOrderDetail.LabDipType == (int)EnumLabDipType.AVL)
                {
                    oDyeingOrderDetail.LabdipNo = oDyeingOrderDetail.LabDipTypeSt;
                }
                //if (oDyeingOrderDetail.LabDipType == (int)EnumLabDipType.DTM)
                //{
                //    if (!String.IsNullOrEmpty(oDyeingOrderDetail.PantonNo))
                //    {
                //        oDyeingOrderDetail.LabdipNo = oDyeingOrderDetail.PantonNo;
                //    }
                //}
                if (oDyeingOrderDetail.LabDipType == (int)EnumLabDipType.TBA)
                {
                    if (String.IsNullOrEmpty(oDyeingOrderDetail.ColorNo))
                    {
                        oDyeingOrderDetail.ColorNo = oDyeingOrderDetail.LabDipTypeSt;
                    }
                }

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.LDNo, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorNo, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.BuyerRef, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                if (String.IsNullOrEmpty(oDyeingOrderDetail.ApproveLotNo))
                {
                    oDyeingOrderDetail.ApproveLotNo = oDyeingOrderDetail.ShadeSt;
                }

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ApproveLotNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.NoOfCone + (String.IsNullOrEmpty(oDyeingOrderDetail.NoOfCone_Weft) ? "" : "+" + oDyeingOrderDetail.NoOfCone_Weft), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                if(  oDyeingOrderDetail.Status==(int)EnumDOState.Hold_Production )
                {
                    oDyeingOrderDetail.Note = "HOLD";
                }
                if (oDyeingOrderDetail.Status == (int)EnumDOState.Cancelled)
                {
                    oDyeingOrderDetail.Note = "Cancelled";
                }

                Paragraph oPdfParagraph;
                if (oDyeingOrderDetail.Note == null) { oDyeingOrderDetail.Note = ""; }
                if (oDyeingOrderDetail.Note.Length > 50)
                {
                    oPdfParagraph = new Paragraph(new Phrase(oDyeingOrderDetail.Note, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                }
                else
                {
                    oPdfParagraph = new Paragraph(new Phrase(oDyeingOrderDetail.Note, _oFontStyle));
                }
                oPdfParagraph.SetLeading(0f, 1f);
                oPdfParagraph.Alignment = Element.ALIGN_LEFT;
                _oPdfPCell = new PdfPCell();
                _oPdfPCell.AddElement(oPdfParagraph);
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                oPdfPTable.AddCell(_oPdfPCell);


                _nTotalAmount = _nTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                _nTotalQty = _nTotalQty + oDyeingOrderDetail.Qty;
                _nGrandTotalAmount = _nGrandTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                _nGrandTotalQty = _nGrandTotalQty + oDyeingOrderDetail.Qty;
                oPdfPTable.CompleteRow();
                #endregion

                nProductID = oDyeingOrderDetail.ProductID;

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

                if (nUsagesHeight > 760)
                {
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                    this.PrintHeader();
                    this.ReporttHeaderTwo(_oDUOrderSetup);
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);
                }
            }
            #region Total
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] {
                  115f,//Yarn
                80f,//COLOR
                42f,//Color No
                50f,//LD No
                55f,//Yatn Lot
                50f,//Match To
                35f, //Qty
                32f,/// No of Cond
                //50f,///Edl Date
                75f// Remarks
            });
            if (_nCount_P > 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Yarn Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 6;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                //_oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Grand Total
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] {
                   115f,//Yarn
                80f,//COLOR
                42f,//Color No
                50f,//LD No
                55f,//Yatn Lot
                50f,//Match To
                35f, //Qty
                32f,/// No of Cond
                //50f,///Edl Date
                75f// Remarks
            });
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
            _oPdfPCell.Colspan = 5;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            _nTotalCurrenyPOQty = _nGrandTotalQty;
           
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Colspan = 2;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region In Word
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 
                120f,//Yarn
                65f,//COLOR
                50f,//Color No
                50f,//LD No
                55f,//Yatn Lot
                50f,//Match To
                42f, //Qty
                27f,/// No of Cond
                //50f,///Edl Date
                80f// Remarks
            });
            string sTemp = "";
            sTemp = Global.DollarWords(_nGrandTotalQty);
            if (!String.IsNullOrEmpty(sTemp))
            {
                sTemp = sTemp.Replace("Dollar", "");
                sTemp = sTemp.Replace("Only", "");
                sTemp = sTemp.ToUpper();
            }
            _oPdfPCell = new PdfPCell(new Phrase("IN WORDS: " + sTemp + " " + _sMUnit + " ONLY", _oFontStyleBold));
            _oPdfPCell.Colspan = 9;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion
            #region Light
           
            if (_oDyeingOrder.LightSourchID>0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Light Source:", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPhrase = new Phrase();
                if (!String.IsNullOrEmpty(_oLightSource.Descriptions) && !String.IsNullOrEmpty(_oLightSource.NameTwo))
                {
                    _oPhrase.Add(new Chunk("Primary:", _oFontStyle));
                }
                _oPhrase.Add(new Chunk(_oLightSource.Descriptions, _oFontStyleBold));
                if (!String.IsNullOrEmpty(_oLightSource.NameTwo))
                {
                    _oPhrase.Add(new Chunk(" Secondary:", _oFontStyle));
                    _oPhrase.Add(new Chunk(_oLightSource.NameTwo, _oFontStyleBold));
                }
                _oPdfPCell = new PdfPCell(_oPhrase);
                //_oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 9;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

            }

            string sTempT = "";
            if (!string.IsNullOrEmpty(_oDyeingOrder.StripeOrder))
            {
                sTempT = "StripeOrder: " + _oDyeingOrder.StripeOrder;
            }
            if (!string.IsNullOrEmpty(_oDyeingOrder.StripeOrder))
            {
                sTempT = sTempT + " KnittingStyle: " + _oDyeingOrder.KnittingStyle;
            }
            if (!string.IsNullOrEmpty(_oDyeingOrder.Gauge))
            {
                sTempT = sTempT + " Gauge: " + _oDyeingOrder.Gauge;
            }
            if (!string.IsNullOrEmpty(sTempT))
            {
                _oPdfPCell = new PdfPCell(new Phrase(sTempT, _oFontStyle));
                _oPdfPCell.Colspan = 9;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
            #region Note/Remarks
            if (!string.IsNullOrEmpty(_oDyeingOrder.Note))
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 5f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Remarks: " + _oDyeingOrder.Note,  FontFactory.GetFont("Tahoma", 10f, 1)));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
              
            }
            #endregion
            #region Note/Remarks
            if (!string.IsNullOrEmpty(_oDyeingOrder.ReviseNote))
            {
                _oPdfPCell = new PdfPCell(new Phrase("Revise Remarks: " + _oDyeingOrder.ReviseNote, FontFactory.GetFont("Tahoma", 10f, 1)));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
            #endregion
            #region ColorCombo
            if (_oDUColorCombos_Group.Count > 0)
            {
                string sNotes = "";
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 5f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Color Combo:", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthRight = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _nCount = 0;
                foreach (DUColorCombo oItem in _oDUColorCombos_Group)
                {
                    sNotes = sNotes + " " + (int)oItem.ComboID + " " + oItem.ColorName + "\n";
                }

                _oPdfPCell = new PdfPCell(new Phrase(sNotes, _oFontStyleBold));
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthRight = 0.5f;
                _oPdfPCell.BorderWidthTop = 0;
                _oPdfPCell.BorderWidthBottom = 0.5f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
            #region
            if (_oDyeingOrderNotes.Count > 0)
            {
                string sNotes = "";
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 5f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Special instructions:", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthRight = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _nCount = 0;
                foreach (DyeingOrderNote oItem in _oDyeingOrderNotes)
                {
                    _nCount++;
                    sNotes = sNotes + _nCount.ToString() + ". " + oItem.OrderNote + "\n";
                }


                _oPdfPCell = new PdfPCell(new Phrase(sNotes, _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            ///
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                350,//Yarn
                20f,
                225// Color
            });
            _oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail_Summary_Two());
            _oPdfPCell.Border = 0;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail_Color());
            _oPdfPCell.Border = 0;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
           
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

       
            //_oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail_Summary());
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion

            //#region Balank Space
            //_oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail_Color());
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion

            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Footer
            _oPdfPCell = new PdfPCell(this.PrintFooter_B());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


    
        }
        private void PrintBody_B_NonDyeing()
        {

            float nMinimumHeight = 5;


            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Short Info
            //_oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail());
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();


            _nTotalAmount = 0;
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(9);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] {
                19f, //SL
                95f,//Yarn
                95f,//COLOR
                60f,//Mat/Process
                45f, //Qty
                45f,/// UP
                30f,///No Of Cone
                58f,///Delivery Date
                80f// Remarks
            });
            int nProductID = 0;
            _nTotalQty = 0;
            _nTotalAmount = 0;
            _nCount = 0;
            _oDyeingOrderDetails = _oDyeingOrderDetails.OrderBy(o => o.DyeingOrderDetailID).ToList();

            oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("YARN TYPE", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("PROCESS", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("QUANTITY \n(" + _sMUnit + ")", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("U. PRICE \n(" + _sMUnit + "/" + _oDUOrderSetup.CurrencySY + ")", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("No of\nCone", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("DEL. DATE", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            foreach (DyeingOrderDetail oDyeingOrderDetail in _oDyeingOrderDetails)
            {
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] {
                    19f, //SL
                95f,//Yarn
                95f,//COLOR
                60f,//Mat/Process
                45f, //Qty
                45f,/// UP
                30f,///No Of Cone
                58f,///Delivery Date
                80f// Remarks
                //19f, //SL
                //100f,//Yarn
                //100f,//COLOR
                //70f,//Mat/Process
                //50f, //Qty
                //50f,/// UP
                //58f,///Delivery Date
                //80f// Remarks
            });


                #region PrintDetail
                _nCount++;

                _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                //_oPdfPCell.Rowspan = 5;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ProductName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                //_oPdfPCell.Rowspan = 5;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.PantonNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

             
                //_oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ApproveLotNo, _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.MinimumHeight = nMinimumHeight;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.UnitPrice), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.NoOfCone, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.DeliveryDateSt, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                if (oDyeingOrderDetail.Status == (int)EnumDOState.Hold_Production)
                {
                    oDyeingOrderDetail.Note = "HOLD";
                }
                if (oDyeingOrderDetail.Status == (int)EnumDOState.Cancelled)
                {
                    oDyeingOrderDetail.Note = "Cancelled";
                }

                Paragraph oPdfParagraph;
                if (oDyeingOrderDetail.Note == null) { oDyeingOrderDetail.Note = ""; }
                if (oDyeingOrderDetail.Note.Length > 50)
                {
                    oPdfParagraph = new Paragraph(new Phrase(oDyeingOrderDetail.Note, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                }
                else
                {
                    oPdfParagraph = new Paragraph(new Phrase(oDyeingOrderDetail.Note, _oFontStyle));
                }
                oPdfParagraph.SetLeading(0f, 1f);
                oPdfParagraph.Alignment = Element.ALIGN_LEFT;
                _oPdfPCell = new PdfPCell();
                _oPdfPCell.AddElement(oPdfParagraph);
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                oPdfPTable.AddCell(_oPdfPCell);


                _nTotalAmount = _nTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                _nTotalQty = _nTotalQty + oDyeingOrderDetail.Qty;
                _nGrandTotalAmount = _nGrandTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                _nGrandTotalQty = _nGrandTotalQty + oDyeingOrderDetail.Qty;
                oPdfPTable.CompleteRow();
                #endregion

                nProductID = oDyeingOrderDetail.ProductID;

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

                if (nUsagesHeight > 950)
                {
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                    this.PrintHeader();
                    this.ReporttHeaderTwo(_oDUOrderSetup);
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);
                }
            }
            #region Total
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] {
              19f, //SL
                95f,//Yarn
                95f,//COLOR
                60f,//Mat/Process
                45f, //Qty
                45f,/// UP
                30f,///No Of Cone
                58f,///Delivery Date
                80f// Remarks
            });
            if (_nCount_P > 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Yarn Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 4;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Grand Total
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] {
                  19f, //SL
                95f,//Yarn
                95f,//COLOR
                60f,//Mat/Process
                45f, //Qty
                45f,/// UP
                30f,///No Of Cone
                58f,///Delivery Date
                80f// Remarks
            });
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
            _oPdfPCell.Colspan = 4;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            _nTotalCurrenyPOQty = _nGrandTotalQty;
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 3;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region In Word
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 
                19f, //SL
                95f,//Yarn
                95f,//COLOR
                60f,//Mat/Process
                45f, //Qty
                45f,/// UP
                30f,///No Of Cone
                58f,///Delivery Date
                80f// Remarks
            });
            string sTemp = "";
            sTemp = Global.DollarWords(_nGrandTotalQty);
            if (!String.IsNullOrEmpty(sTemp))
            {
                sTemp = sTemp.Replace("Dollar", "");
                sTemp = sTemp.Replace("Only", "");
                sTemp = sTemp.ToUpper();
            }
            _oPdfPCell = new PdfPCell(new Phrase("IN WORDS: " + sTemp + " " + _sMUnit + " ONLY", _oFontStyleBold));
            _oPdfPCell.Colspan = 9;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion
            #region Light

            //if (_oDyeingOrder.LightSourchID > 0)
            //{

            //    _oPhrase = new Phrase();
            //    if (!String.IsNullOrEmpty(_oLightSource.Descriptions))
            //    {
            //        _oPhrase.Add(new Chunk("Light Source:", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            //    }
            //    if (!String.IsNullOrEmpty(_oLightSource.Descriptions) && !String.IsNullOrEmpty(_oLightSource.NameTwo))
            //    {
            //        _oPhrase.Add(new Chunk("Primary:", _oFontStyle));
            //    }
            //    _oPhrase.Add(new Chunk(_oLightSource.Descriptions, _oFontStyleBold));
            //    if (!String.IsNullOrEmpty(_oLightSource.NameTwo))
            //    {
            //        _oPhrase.Add(new Chunk(" Secondary:", _oFontStyle));
            //        _oPhrase.Add(new Chunk(_oLightSource.NameTwo, _oFontStyleBold));
            //    }
                
            //    _oPdfPCell = new PdfPCell(_oPhrase);
            //    //_oPdfPCell.Border = 0;
            //    _oPdfPCell.MinimumHeight = 10f;
            //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //}

            string sTempT = "";
            if (!string.IsNullOrEmpty(_oDyeingOrder.StripeOrder))
            {
                sTempT = "StripeOrder: " + _oDyeingOrder.StripeOrder;
            }
            if (!string.IsNullOrEmpty(_oDyeingOrder.StripeOrder))
            {
                sTempT = sTempT + " KnittingStyle: " + _oDyeingOrder.KnittingStyle;
            }
            if (!string.IsNullOrEmpty(_oDyeingOrder.Gauge))
            {
                sTempT = sTempT + " Gauge: " + _oDyeingOrder.Gauge;
            }
            if (!string.IsNullOrEmpty(sTempT))
            {
                _oPdfPCell = new PdfPCell(new Phrase(sTempT, _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.MinimumHeight = 10f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                
                //_oPdfPCell = new PdfPCell(new Phrase(sTempT, _oFontStyle));
                //_oPdfPCell.Colspan = 8;
                ////_oPdfPCell.Border = 0;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //oPdfPTable.AddCell(_oPdfPCell);
            }
            //oPdfPTable.CompleteRow();
            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion
            #endregion
            #region Note/Remarks
            if (!string.IsNullOrEmpty(_oDyeingOrder.Note))
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 8f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Remarks: " + _oDyeingOrder.Note, FontFactory.GetFont("Tahoma", 10f, 1)));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 8f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
            #region ColorCombo
            if (_oDUColorCombos_Group.Count > 0)
            {
                string sNotes = "";

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 8f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Buyer Combo:", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthRight = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _nCount = 0;
                foreach (DUColorCombo oItem in _oDUColorCombos_Group)
                {

                    sNotes = sNotes + " " + (int)oItem.ComboID + " " + oItem.ColorName + "\n";
                }


                _oPdfPCell = new PdfPCell(new Phrase(sNotes, _oFontStyleBold));
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthRight = 0.5f;
                _oPdfPCell.BorderWidthTop = 0;
                _oPdfPCell.BorderWidthBottom = 0.5f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
            #region
            if (_oDyeingOrderNotes.Count > 0)
            {
                string sNotes = "";
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 8f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Special instructions:", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthRight = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _nCount = 0;
                foreach (DyeingOrderNote oItem in _oDyeingOrderNotes)
                {
                    _nCount++;
                    sNotes = sNotes + _nCount.ToString() + ". " + oItem.OrderNote + "\n";
                }


                _oPdfPCell = new PdfPCell(new Phrase(sNotes, _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Balank Space
            _oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail_Summary());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Footer
            _oPdfPCell = new PdfPCell(this.PrintFooter_B());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


        }
        private void PrintBody_B_Twist()
        {

            float nMinimumHeight = 5;


            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Short Info
            //_oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail());
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();


            _nTotalAmount = 0;
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(10);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] {
                20f, //SL
                110f,//Yarn
                90f,//COLOR
                40f,//TPI
                70f,//Twist With
                50f, //Qty
                45f,/// UP
                45f,/// No Of Cone
                58f,///Delivery Date
                55f// Remarks
            });
            int nProductID = 0;
            int nBuyerCombo = 0;
            _nTotalQty = 0;
            _nTotalAmount = 0;
            _nCount = 0;
            int _nCount_Raw = 0;
            string sTemp = "";
            _oDyeingOrderDetails = _oDyeingOrderDetails.OrderBy(o => o.BuyerCombo).ToList();

            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();

            oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
            //oPdfPCell.Border = 0; 
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("YARN TYPE", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("TPI", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Twist With", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("QUANTITY \n(" + _sMUnit + ")", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("UP \n(" + _sMUnit + "/"+_oDUOrderSetup.CurrencySY+")", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("No Of Cone", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("DEL. DATE", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);
          

            oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            foreach (DyeingOrderDetail oDyeingOrderDetail in _oDyeingOrderDetails)
            {
            //    oPdfPTable = new PdfPTable(9);
            //    oPdfPTable.SetWidths(new float[] {
            //    20f, //SL
            //    100f,//Yarn
            //    90f,//COLOR
            //    45f,//TPI
            //    70f,//Twist With
            //    50f, //Qty
            //    50f,/// UP
            //    58f,///Delivery Date
            //    80f// Remarks
            //});


                #region PrintDetail
                _nCount++;

                _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                //_oPdfPCell.Rowspan = 2;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                oPdfPTable.AddCell(_oPdfPCell);

                if (nBuyerCombo != oDyeingOrderDetail.BuyerCombo && oDyeingOrderDetail.BuyerCombo>0)
                {
                    sTemp = "";

                    _nCount_Raw = _oDyeingOrderDetails.Where(x => x.BuyerCombo == oDyeingOrderDetail.BuyerCombo).Count();
                    oDyeingOrderDetails = _oDyeingOrderDetails.Where(x => x.BuyerCombo == oDyeingOrderDetail.BuyerCombo).ToList();
                    sTemp = string.Join(" + ", oDyeingOrderDetails.Select(x => x.ProductName + " " + x.PantonNo).ToList());

                    _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.MinimumHeight = nMinimumHeight;
                    _oPdfPCell.Rowspan = (_nCount_Raw > 0) ? _nCount_Raw : 1; 
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable.AddCell(_oPdfPCell);
                }
                else if (oDyeingOrderDetail.BuyerCombo ==0)
                {

                    _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ProductName, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.MinimumHeight = nMinimumHeight;
                    _oPdfPCell.Rowspan = (_nCount_Raw > 0) ? _nCount_Raw : 1;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable.AddCell(_oPdfPCell);
                }


               _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.LengthOfCone, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                if (nBuyerCombo != oDyeingOrderDetail.BuyerCombo && oDyeingOrderDetail.BuyerCombo>0)
                {
                    sTemp = "";
                    
                    _nCount_Raw = _oDyeingOrderDetails.Where(x => x.BuyerCombo == oDyeingOrderDetail.BuyerCombo).Count();
                    oDyeingOrderDetails = _oDyeingOrderDetails.Where(x => x.BuyerCombo == oDyeingOrderDetail.BuyerCombo).ToList();
                    sTemp = string.Join("+", oDyeingOrderDetails.Select(x => x.ColorName).ToList());

                    _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));/// Twist With
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Rowspan = (_nCount_Raw > 0) ? _nCount_Raw : 1; 
                    _oPdfPCell.MinimumHeight = nMinimumHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);
                }
                else if (oDyeingOrderDetail.BuyerCombo == 0)
                {

                    _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.PantonNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.MinimumHeight = nMinimumHeight;
                    _oPdfPCell.Rowspan = (_nCount_Raw > 0) ? _nCount_Raw : 1;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable.AddCell(_oPdfPCell);
                }

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.UnitPrice), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.NoOfCone, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.DeliveryDateSt, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                if (oDyeingOrderDetail.Status == (int)EnumDOState.Hold_Production)
                {
                    oDyeingOrderDetail.Note = "HOLD";
                }
                if (oDyeingOrderDetail.Status == (int)EnumDOState.Cancelled)
                {
                    oDyeingOrderDetail.Note = "Cancelled";
                }

                Paragraph oPdfParagraph;
                if (oDyeingOrderDetail.Note == null) { oDyeingOrderDetail.Note = ""; }
                if (oDyeingOrderDetail.Note.Length > 50)
                {
                    oPdfParagraph = new Paragraph(new Phrase(oDyeingOrderDetail.Note, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                }
                else
                {
                    oPdfParagraph = new Paragraph(new Phrase(oDyeingOrderDetail.Note, _oFontStyle));
                }
                oPdfParagraph.SetLeading(0f, 1f);
                oPdfParagraph.Alignment = Element.ALIGN_LEFT;
                _oPdfPCell = new PdfPCell();
                _oPdfPCell.AddElement(oPdfParagraph);
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                oPdfPTable.AddCell(_oPdfPCell);


                _nTotalAmount = _nTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                _nTotalQty = _nTotalQty + oDyeingOrderDetail.Qty;
                _nGrandTotalAmount = _nGrandTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                _nGrandTotalQty = _nGrandTotalQty + oDyeingOrderDetail.Qty;
                oPdfPTable.CompleteRow();
                #endregion

                nProductID = oDyeingOrderDetail.ProductID;
                nBuyerCombo = oDyeingOrderDetail.BuyerCombo;

                nUsagesHeight = CalculatePdfPTableHeight(oPdfPTable);

                if (nUsagesHeight > 950)
                {
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                    this.PrintHeader();
                    this.ReporttHeaderTwo(_oDUOrderSetup);
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);
                }
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region Total
            oPdfPTable = new PdfPTable(10);
            oPdfPTable.SetWidths(new float[] {
                  20f, //SL
                110f,//Yarn
                90f,//COLOR
                40f,//TPI
                70f,//Twist With
                50f, //Qty
                45f,/// UP
                45f,/// No Of Cone
                58f,///Delivery Date
                55f// Remarks
            });
            if (_nCount_P > 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Yarn Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 5;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Grand Total
            oPdfPTable = new PdfPTable(10);
            oPdfPTable.SetWidths(new float[] {
                  20f, //SL
                110f,//Yarn
                90f,//COLOR
                40f,//TPI
                70f,//Twist With
                50f, //Qty
                45f,/// UP
                45f,/// No Of Cone
                58f,///Delivery Date
                55f// Remarks
            });
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
            _oPdfPCell.Colspan = 5;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            _nTotalCurrenyPOQty = _nGrandTotalQty;
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 4;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region In Word
            oPdfPTable = new PdfPTable(10);
            oPdfPTable.SetWidths(new float[] { 
                 20f, //SL
                110f,//Yarn
                90f,//COLOR
                40f,//TPI
                70f,//Twist With
                50f, //Qty
                45f,/// UP
                45f,/// No Of Cone
                58f,///Delivery Date
                55f// Remarks
            });
             sTemp = "";
            sTemp = Global.DollarWords(_nGrandTotalQty);
            if (!String.IsNullOrEmpty(sTemp))
            {
                sTemp = sTemp.Replace("Dollar", "");
                sTemp = sTemp.Replace("Only", "");
                sTemp = sTemp.ToUpper();
            }
            _oPdfPCell = new PdfPCell(new Phrase("IN WORDS: " + sTemp + " " + _sMUnit + " ONLY", _oFontStyleBold));
            _oPdfPCell.Colspan = 10;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion
            #region Light

            //if (_oDyeingOrder.LightSourchID > 0)
            //{

            //    _oPhrase = new Phrase();
            //    if (!String.IsNullOrEmpty(_oLightSource.Descriptions))
            //    {
            //        _oPhrase.Add(new Chunk("Light Source:", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            //    }
            //    if (!String.IsNullOrEmpty(_oLightSource.Descriptions) && !String.IsNullOrEmpty(_oLightSource.NameTwo))
            //    {
            //        _oPhrase.Add(new Chunk("Primary:", _oFontStyle));
            //    }
            //    _oPhrase.Add(new Chunk(_oLightSource.Descriptions, _oFontStyleBold));
            //    if (!String.IsNullOrEmpty(_oLightSource.NameTwo))
            //    {
            //        _oPhrase.Add(new Chunk(" Secondary:", _oFontStyle));
            //        _oPhrase.Add(new Chunk(_oLightSource.NameTwo, _oFontStyleBold));
            //    }

            //    _oPdfPCell = new PdfPCell(_oPhrase);
            //    //_oPdfPCell.Border = 0;
            //    _oPdfPCell.MinimumHeight = 10f;
            //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //}

            string sTempT = "";
            if (!string.IsNullOrEmpty(_oDyeingOrder.StripeOrder))
            {
                sTempT = "StripeOrder: " + _oDyeingOrder.StripeOrder;
            }
            if (!string.IsNullOrEmpty(_oDyeingOrder.StripeOrder))
            {
                sTempT = sTempT + " KnittingStyle: " + _oDyeingOrder.KnittingStyle;
            }
            if (!string.IsNullOrEmpty(_oDyeingOrder.Gauge))
            {
                sTempT = sTempT + " Gauge: " + _oDyeingOrder.Gauge;
            }
            if (!string.IsNullOrEmpty(sTempT))
            {
                _oPdfPCell = new PdfPCell(new Phrase(sTempT, _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.MinimumHeight = 10f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                //_oPdfPCell = new PdfPCell(new Phrase(sTempT, _oFontStyle));
                //_oPdfPCell.Colspan = 8;
                ////_oPdfPCell.Border = 0;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //oPdfPTable.AddCell(_oPdfPCell);
            }
            //oPdfPTable.CompleteRow();
            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion
            #endregion
            #region Note/Remarks
            if (!string.IsNullOrEmpty(_oDyeingOrder.Note))
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 8f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Remarks: " + _oDyeingOrder.Note, FontFactory.GetFont("Tahoma", 10f, 1)));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 8f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
            #region Note/Remarks
            if (!string.IsNullOrEmpty(_oDyeingOrder.ReviseNote))
            {
                _oPdfPCell = new PdfPCell(new Phrase("Revise Remarks: " + _oDyeingOrder.ReviseNote, FontFactory.GetFont("Tahoma", 10f, 1)));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
            #region ColorCombo
            if (_oDUColorCombos_Group.Count > 0)
            {
                string sNotes = "";

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 8f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Buyer Combo:", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthRight = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _nCount = 0;
                foreach (DUColorCombo oItem in _oDUColorCombos_Group)
                {

                    sNotes = sNotes + " " + (int)oItem.ComboID + " " + oItem.ColorName + "\n";
                }


                _oPdfPCell = new PdfPCell(new Phrase(sNotes, _oFontStyleBold));
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthRight = 0.5f;
                _oPdfPCell.BorderWidthTop = 0;
                _oPdfPCell.BorderWidthBottom = 0.5f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
            #region
            if (_oDyeingOrderNotes.Count > 0)
            {
                string sNotes = "";
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 8f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Special instructions:", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthRight = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _nCount = 0;
                foreach (DyeingOrderNote oItem in _oDyeingOrderNotes)
                {
                    _nCount++;
                    sNotes = sNotes + _nCount.ToString() + ". " + oItem.OrderNote + "\n";
                }


                _oPdfPCell = new PdfPCell(new Phrase(sNotes, _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Footer
            _oPdfPCell = new PdfPCell(this.PrintFooter_B());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private PdfPTable SetDyeingOrderDetail_Summary()
        {
            _nTotalAmount = 0;
            _nTotalQty = 0;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 50f, 150f,80f,200f});
            _nTotalQty = 0;
            _nTotalAmount = 0;
            _nCount = 0;
            _nCount_P = 0;
            _nGrandTotalQty = 0;

            if (_oDyeingOrderDetails.Count > 0)
            {

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 4;
                oPdfPCell.FixedHeight = 5f;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                oPdfPCell = new PdfPCell(new Phrase("Yarn Summary", _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                oPdfPCell.Border = 0; 
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
            
                oPdfPTable.CompleteRow();


                _oDyeingOrderDetails = _oDyeingOrderDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.ProductNameCode}, (key, grp) =>
                                                     new DyeingOrderDetail
                                                     {
                                                         ProductID = key.ProductID,
                                                         ProductName = key.ProductName,
                                                         ProductNameCode = key.ProductNameCode,
                                                         Qty = grp.Sum(p => p.Qty)
                                                         
                                                     }).ToList();


                oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Yarn", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("Order Qty (" + _sMUnit + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                foreach (DyeingOrderDetail oDyeingOrderDetail in _oDyeingOrderDetails)
                {
                    #region PrintDetail
                    _nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ProductName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //oPdfPCell.Border = 0;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                  
                    _nTotalQty = _nTotalQty + oDyeingOrderDetail.Qty;
                  
                    oPdfPTable.CompleteRow();
                    #endregion

                }

                #region Total
                if (_nCount > 1)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                    _oPdfPCell.Colspan = 2;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                }
                #endregion
            }

            return oPdfPTable;
        }
        private PdfPTable SetDyeingOrderDetail_Summary_Two()
        {
            _nTotalAmount = 0;
            _nTotalQty = 0;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable2 = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable2.SetWidths(new float[] { 35f, 170f, 120f });
            _nTotalQty = 0;
            _nTotalAmount = 0;
            _nCount = 0;
            _nCount_P = 0;
            _nGrandTotalQty = 0;

            if (_oDyeingOrderDetails.Count > 0)
            {


                oPdfPCell = new PdfPCell(new Phrase("Yarn Summary", _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable2.AddCell(oPdfPCell);
                oPdfPTable2.CompleteRow();

                _oDyeingOrderDetails_Previous = _oDyeingOrderDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.ProductNameCode }, (key, grp) =>
                                                     new DyeingOrderDetail
                                                     {
                                                         ProductID = key.ProductID,
                                                         ProductName = key.ProductName,
                                                         ProductNameCode = key.ProductNameCode,
                                                         Qty = grp.Sum(p => p.Qty)

                                                     }).ToList();


                oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable2.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Yarn", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable2.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("Order Qty (" + _sMUnit + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable2.AddCell(oPdfPCell);



                oPdfPTable2.CompleteRow();

                foreach (DyeingOrderDetail oDyeingOrderDetail in _oDyeingOrderDetails_Previous)
                {
                    #region PrintDetail
                    _nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable2.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ProductName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable2.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //oPdfPCell.Border = 0;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable2.AddCell(oPdfPCell);

                    _nTotalQty = _nTotalQty + oDyeingOrderDetail.Qty;

                    oPdfPTable2.CompleteRow();
                    #endregion

                }

                #region Total
                if (_nCount > 1)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                    _oPdfPCell.Colspan = 2;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable2.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable2.AddCell(_oPdfPCell);

                    oPdfPTable2.CompleteRow();
                }
                #endregion
            }

            return oPdfPTable2;
        }
       
        private PdfPTable SetDyeingOrderDetail_Color()
        {
            _nCount = 0;
            _nTotalQty = 0;
            PdfPTable oPdfPTable2 = new PdfPTable(3);
            oPdfPTable2.SetWidths(new float[] { 30f, 115f, 80f });
            #region 
            if (_oDyeingOrderDetails.Count > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Color Summary", _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable2.AddCell(_oPdfPCell);

                oPdfPTable2.CompleteRow();

                _oDyeingOrderDetails_Previous = _oDyeingOrderDetails;
                foreach (DyeingOrderDetail oitem in _oDyeingOrderDetails_Previous)
                {
                    if (!string.IsNullOrEmpty(oitem.ColorNo))
                    {
                        oitem.ColorNo = new String(oitem.ColorNo.Where(c => c != '/' && c != '%' && c != '-' && (c < '0' || c > '9')).ToArray());
                        if (oitem.ColorNo == "WT" || oitem.ColorNo == "OF" || oitem.ColorNo == "GM")
                        {
                            oitem.ColorNo = "White";
                        }
                        else
                        {
                            oitem.ColorNo = "color";
                        }
                    }
                    else
                    {
                        oitem.ColorNo = "color";
                    }
                }

                _oDyeingOrderDetails_Previous = _oDyeingOrderDetails_Previous.GroupBy(x => new { x.ColorNo }, (key, grp) =>
                                                     new DyeingOrderDetail
                                                     {
                                                         ColorNo = key.ColorNo,
                                                         Qty = grp.Sum(p => p.Qty)

                                                     }).ToList();


                _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable2.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("color", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable2.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Order Qty (" + _sMUnit + ")", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable2.AddCell(_oPdfPCell);

                oPdfPTable2.CompleteRow();
                foreach (DyeingOrderDetail oDyeingOrderDetail in _oDyeingOrderDetails_Previous)
                {
                    #region PrintDetail
                    _nCount++;
                    _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable2.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable2.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //oPdfPCell.Border = 0;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable2.AddCell(_oPdfPCell);


                    _nTotalQty = _nTotalQty + oDyeingOrderDetail.Qty;

                    oPdfPTable2.CompleteRow();
                    #endregion
                }
                #region Total
                if (_nCount > 1)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                    _oPdfPCell.Colspan = 2;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable2.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }
                #endregion

            }
            #endregion
           
            return oPdfPTable2;
        }
        private PdfPTable PrintFooter_B()
        {
            //#region
            //nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            //if (nUsagesHeight < 670)
            //{
            //    nUsagesHeight = 650 - nUsagesHeight;
            //}
            //if (nUsagesHeight > 20)
            //{
            //    #region Blank Row


            //    while (nUsagesHeight < 700)
            //    {
            //        #region Table Initiate
            //        PdfPTable oPdfPTableTemp = new PdfPTable(4);
            //        oPdfPTableTemp.SetWidths(new float[] { 148f, 148f, 148f, 148f });

            //        #endregion

            //        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            //        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);


            //        oPdfPTableTemp.CompleteRow();

            //        _oPdfPCell = new PdfPCell(oPdfPTableTemp);
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //        _oPdfPTable.CompleteRow();

            //        nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            //    }

            //    #endregion
            //}
            //#endregion

            float nTableHeight = CalculatePdfPTableHeight(_oPdfPTable);
            float _nfixedHight = 740 - (float)nTableHeight;
            if (_nfixedHight > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = _nfixedHight;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }



            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 197f, 197f, 197f });


            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 10;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(oPdfPCell);


            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 10;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 10;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrder.PreaperByName, _oFontStyle));
            oPdfPCell.Border = 0; 
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("__________________", _oFontStyle));
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("_______________", _oFontStyle));
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("_________________", _oFontStyle));
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyleBold));
            oPdfPCell.Border = 0; 
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Checked By", _oFontStyleBold));
            oPdfPCell.Border = 0;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Approved By", _oFontStyleBold));
            oPdfPCell.Border = 0; 
            //oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

           

            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        #endregion
        #endregion

        #region Print Loan Order
        public byte[] PrepareReport_LoanOrder(DyeingOrder oDyeingOrder, Company oCompany, BusinessUnit oBusinessUnit, DUOrderSetup oDUOrderSetup, DUDyeingStep oDUDyeingStep, List<DyeingOrderNote> oDyeingOrderNotes)
        {
            _oDUOrderSetup = oDUOrderSetup;
            _oDyeingOrder = oDyeingOrder;
            _oDUDyeingStep = oDUDyeingStep;
            _oBusinessUnit = oBusinessUnit;
            _oDyeingOrderNotes = new List<DyeingOrderNote>();
            _oDyeingOrderDetails = oDyeingOrder.DyeingOrderDetails;
            _oCompany = oCompany;
            _oDyeingOrderNotes = oDyeingOrderNotes;
            if (_oDyeingOrderDetails.Count > 0)
            {
                _sMUnit = _oDyeingOrderDetails[0].MUnit;
            }

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(35f, 10f, 10f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();

            //  PageEventHandler.signatures = new List<string>(new string[] {_oBusinessUnit.ShortName + " Concern Person:" + _oDyeingOrder.MKTPName});
            PageEventHandler.FooterNote = _oBusinessUnit.ShortName + " Concern Person: " + _oDyeingOrder.MKTPName;

            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            this.ReporttHeaderTwo(_oDUOrderSetup);
            this.PrintWaterMark(35f, 10f, 10f, 30f);
            this.PrintHead_B();
            if ( _oDyeingOrder.DyeingOrderType == (int)EnumOrderType.ReConing)
            {
                this.PrintBody_B_NonDyeing();
            }
            if (_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.LoanOrder )
            {
                this.PrintBody_LoanOrder();
            }
            else if (_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.TwistOrder)
            {
                this.PrintBody_B_Twist();
            }
            else
            {
                this.PrintBody_B();
            }

            _oPdfPTable.HeaderRows = 4;

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void PrintBody_LoanOrder()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);
            
            float nMinimumHeight = 5;


            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Short Info
            //_oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail());
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();


            _nTotalAmount = 0;
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] {
                19f, //SL
                120f,//Yarn
                95f,//COLOR
                60f,//Mat/Process
                45f, //Qty
                45f,/// UP
                80f// Remarks
            });
            int nProductID = 0;
            _nTotalQty = 0;
            _nTotalAmount = 0;
            _nCount = 0;
            _oDyeingOrderDetails = _oDyeingOrderDetails.OrderBy(o => o.DyeingOrderDetailID).ToList();

            oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("DESCRIPTION OF GOODS", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("BRAND", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("QUANTITY \n(" + _sMUnit + ")", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("U. PRICE \n(" + _sMUnit + "/" + _oDUOrderSetup.CurrencySY + ")", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

          

            oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            foreach (DyeingOrderDetail oDyeingOrderDetail in _oDyeingOrderDetails)
            {
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] {
                  19f, //SL
                120f,//Yarn
                95f,//COLOR
                60f,//Mat/Process
                45f, //Qty
                45f,/// UP
                80f// Remarks
            });


                #region PrintDetail
                _nCount++;

                _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                //_oPdfPCell.Rowspan = 5;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ProductName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                //_oPdfPCell.Rowspan = 5;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.PantonNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                //_oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ApproveLotNo, _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.MinimumHeight = nMinimumHeight;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.UnitPrice), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.NoOfCone, _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.MinimumHeight = nMinimumHeight;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.DeliveryDateSt, _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.MinimumHeight = nMinimumHeight;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);

                if (oDyeingOrderDetail.Status == (int)EnumDOState.Hold_Production)
                {
                    oDyeingOrderDetail.Note = "HOLD";
                }
                if (oDyeingOrderDetail.Status == (int)EnumDOState.Cancelled)
                {
                    oDyeingOrderDetail.Note = "Cancelled";
                }

                Paragraph oPdfParagraph;
                if (oDyeingOrderDetail.Note == null) { oDyeingOrderDetail.Note = ""; }
                if (oDyeingOrderDetail.Note.Length > 50)
                {
                    oPdfParagraph = new Paragraph(new Phrase(oDyeingOrderDetail.Note, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                }
                else
                {
                    oPdfParagraph = new Paragraph(new Phrase(oDyeingOrderDetail.Note, _oFontStyle));
                }
                oPdfParagraph.SetLeading(0f, 1f);
                oPdfParagraph.Alignment = Element.ALIGN_LEFT;
                _oPdfPCell = new PdfPCell();
                _oPdfPCell.AddElement(oPdfParagraph);
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = nMinimumHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                oPdfPTable.AddCell(_oPdfPCell);


                _nTotalAmount = _nTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                _nTotalQty = _nTotalQty + oDyeingOrderDetail.Qty;
                _nGrandTotalAmount = _nGrandTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                _nGrandTotalQty = _nGrandTotalQty + oDyeingOrderDetail.Qty;
                oPdfPTable.CompleteRow();
                #endregion

                nProductID = oDyeingOrderDetail.ProductID;

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

                if (nUsagesHeight > 950)
                {
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                    this.PrintHeader();
                    this.ReporttHeaderTwo(_oDUOrderSetup);
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);
                }
            }
            #region Total
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] {
                 19f, //SL
                120f,//Yarn
                95f,//COLOR
                60f,//Mat/Process
                45f, //Qty
                45f,/// UP
                80f// Remarks
            });
            
            #endregion

            #region Grand Total
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] {
                   19f, //SL
                120f,//Yarn
                95f,//COLOR
                60f,//Mat/Process
                45f, //Qty
                45f,/// UP
                80f// Remarks
            });
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
            _oPdfPCell.Colspan = 4;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            _nTotalCurrenyPOQty = _nGrandTotalQty;
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 2;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region In Word
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 
                 19f, //SL
                120f,//Yarn
                95f,//COLOR
                60f,//Mat/Process
                45f, //Qty
                45f,/// UP
                80f// Remarks
            });
            string sTemp = "";
            sTemp = Global.DollarWords(_nGrandTotalQty);
            if (!String.IsNullOrEmpty(sTemp))
            {
                sTemp = sTemp.Replace("Dollar", "");
                sTemp = sTemp.Replace("Only", "");
                sTemp = sTemp.ToUpper();
            }
            _oPdfPCell = new PdfPCell(new Phrase("IN WORDS: " + sTemp + " " + _sMUnit + " ONLY", _oFontStyleBold));
            _oPdfPCell.Colspan = 7;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion
            #region Light


            //oPdfPTable.CompleteRow();
            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion
            #endregion
            #region Note/Remarks
            if (!string.IsNullOrEmpty(_oDyeingOrder.Note))
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 8f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Remarks: " + _oDyeingOrder.Note, FontFactory.GetFont("Tahoma", 10f, 1)));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 8f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region
            if (_oDyeingOrderNotes.Count > 0)
            {
                string sNotes = "";
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 8f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Special instructions:", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthRight = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _nCount = 0;
                foreach (DyeingOrderNote oItem in _oDyeingOrderNotes)
                {
                    _nCount++;
                    sNotes = sNotes + _nCount.ToString() + ". " + oItem.OrderNote + "\n";
                }


                _oPdfPCell = new PdfPCell(new Phrase(sNotes, _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

        


            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Footer
            _oPdfPCell = new PdfPCell(this.PrintFooter_B());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


        }
        #endregion
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
