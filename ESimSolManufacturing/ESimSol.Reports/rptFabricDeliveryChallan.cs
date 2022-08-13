
using System;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Reports;
using ESimSol;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.Reports
{
    public class rptFabricDeliveryChallan
    {
        #region Declaration
        int _nColumns = 9;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        public iTextSharp.text.Image _oImag { get; set; }
        PdfPTable _oPdfPTable = new PdfPTable(9);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        FabricDeliveryChallan _oFDC = new FabricDeliveryChallan();
        FabricDeliveryChallanDetail _oFDCDetail = new FabricDeliveryChallanDetail();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<ApprovalHead> _oApprovalHeads = new List<ApprovalHead>();
        List<ApprovalHistory> _oApprovalHistorys = new List<ApprovalHistory>();
        List<FabricDeliveryChallanDetail> _oFabricDeliveryChallanDetailsAdj = new List<FabricDeliveryChallanDetail>();
        int nRowHeight = 14;
        bool _bIsGatePass = false;
        float nUsagesHeight = 0;
        string _sMUnit = "";
        double _nQty = 0;
        int _nCount = 0;
        string str = "";
        int[] Color_RGB = new int[3] { 1, 85, 72 }; //(5, 138, 146)
        BaseColor oCustom_Color = ESimSolPdfHelper.Custom_BaseColor(new int[] { 67,127, 117 });
        #endregion

        #region Constructor

        public rptFabricDeliveryChallan() { }

        #endregion
        public byte[] PrepareReportGatePass(FabricDeliveryChallan oFDC, Company oCompany, bool bIsGatePass, bool isPadFormat, BusinessUnit oBusinessUnit, List<ApprovalHead> oApprovalHeads, List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetailsAdj)
        {

            _oFDC = oFDC;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _bIsGatePass = bIsGatePass;
            _oApprovalHeads = oApprovalHeads;
            _oFabricDeliveryChallanDetailsAdj = oFabricDeliveryChallanDetailsAdj;
            if (oFDC.FDCDetails.Count > 0)
            {
                _sMUnit = oFDC.FDCDetails[0].MUName;
            }
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(PageSize.A4);
            _oDocument.SetMargins(20f, 20f, 10f, 30f);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //ESimSolFooter PageEventHandler = new ESimSolFooter();
            //PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            //PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //PageEventHandler.signatures=new List<string>(new string[]{"Received By","Checked By", "Prepared By","Store in-Charge","Authorised By."});
            //PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler      --last hide
            if (_oFDC.DisburseBy == 0)
            {
                str = "Yet Not Disburse";
            }
            if (_oFDC.ApproveBy == 0)
            {
                str = "UNAUTHORIZED";
            }


            #endregion
            if (oFDC.IsSample)
            {

                if (isPadFormat)
                {
                    _oDocument.SetMargins(20f, 20f, 80f, 20f);
                    _oCompany = new Company(); _oBusinessUnit = new BusinessUnit();
                    this.PrintHeader(((_bIsGatePass) ? "Gate Pass" : "Sample Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : ""), false);
                }
                else
                {
                    this.PrintHeader(((_bIsGatePass) ? "Gate Pass" : "Sample Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : ""), false);
                } 

               // this.PrintHeaderNormaL(((_bIsGatePass) ? "Gate Pass" : "Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : " "), false);
                this.PrintHead_Sample();
                ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                ESimSolWM_Footer.WMFontSize = 75;
                ESimSolWM_Footer.WMRotation = 45;
                ESimSolWM_Footer PageEventHandlerWM = new ESimSolWM_Footer();
                PageEventHandlerWM.WaterMark = str; //Footer print with page event handler
                PDFWriter.PageEvent = PageEventHandlerWM; //Footer print with page event handler
            }
            else
            {
                if (isPadFormat)
                {
                    _oDocument.SetMargins(20f, 20f, 80f, 20f);
                    _oCompany = new Company(); _oBusinessUnit = new BusinessUnit();
                    this.PrintHeader(((_bIsGatePass) ? "Gate Pass" : "Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : " "), oFDC.FDOTypeSt, false);
                }
                else
                {
                    this.PrintHeader(((_bIsGatePass) ? "Gate Pass" : "Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : " "), oFDC.FDOTypeSt, false);
                } 

               // this.PrintHeaderNormaL(((_bIsGatePass) ? "Gate Pass" : "Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : " "), false);
                this.PrintHead_Bulk();
                ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                ESimSolWM_Footer.WMFontSize = 75;
                ESimSolWM_Footer.WMRotation = 45;
                ESimSolWM_Footer PageEventHandlerWM = new ESimSolWM_Footer();
                PageEventHandlerWM.WaterMark = str; //Footer print with page event handler
                PDFWriter.PageEvent = PageEventHandlerWM; //Footer print with page event handler

            }
            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.SetWidths(new float[] { 20f, 60f, 70f, 40f, 80f, 70f, 45f, 45f, 60f }); //height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            this.PrintBody();
            _oPdfPTable.HeaderRows = 0;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        public byte[] PrepareReport(FabricDeliveryChallan oFDC, Company oCompany, bool bIsGatePass, bool isPadFormat, BusinessUnit oBusinessUnit, List<ApprovalHead> oApprovalHeads, List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetailsAdj)
        {

            _oFDC = oFDC;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _bIsGatePass = bIsGatePass;
            _oApprovalHeads = oApprovalHeads;
            _oFabricDeliveryChallanDetailsAdj = oFabricDeliveryChallanDetailsAdj;
            if (oFDC.FDCDetails.Count > 0)
            {
                _sMUnit = oFDC.FDCDetails[0].MUName;
            }
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(PageSize.A4);
            _oDocument.SetMargins(20f, 20f, 10f, 30f);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //ESimSolFooter PageEventHandler = new ESimSolFooter();
            //PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            //PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //PageEventHandler.signatures=new List<string>(new string[]{"Received By","Checked By", "Prepared By","Store in-Charge","Authorised By."});
            //PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler      --last hide
            if (_oFDC.DisburseBy == 0)
            {
                str = "Not Disbursed";
            }
            if (_oFDC.ApproveBy == 0)
            {
                str = "UNAUTHORIZED";
            }
            
          
            #endregion
            if (_oFDC.IsBill == false)
            {
                if (oFDC.IsSample)
                {
                    // this.PrintHeader(((_bIsGatePass) ? "Gate Pass" : "Sample Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : ""), false);

                    if (isPadFormat)
                    {
                        _oDocument.SetMargins(20f, 20f, 80f, 20f);
                        _oCompany = new Company(); _oBusinessUnit = new BusinessUnit();
                        this.PrintHeaderNormaL(((_bIsGatePass) ? "Gate Pass" : "Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : " "), false);
                    }
                    else
                    {
                        this.PrintHeaderNormaL(((_bIsGatePass) ? "Gate Pass" : "Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : " "), false);
                    }


                    this.PrintHead_Sample();
                    ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    ESimSolWM_Footer.WMFontSize = 75;
                    ESimSolWM_Footer.WMRotation = 45;
                    ESimSolWM_Footer PageEventHandlerWM = new ESimSolWM_Footer();
                    PageEventHandlerWM.WaterMark = str; //Footer print with page event handler
                    PDFWriter.PageEvent = PageEventHandlerWM; //Footer print with page event handler
                }
                else
                {
                    //this.PrintHeader(((_bIsGatePass) ? "Gate Pass" : "Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : " "), oFDC.FDOTypeSt, false);
                    //this.PrintHeaderNormaL(((_bIsGatePass) ? "Gate Pass" : "Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : " "),  false);

                    if (isPadFormat)
                    {
                        _oDocument.SetMargins(20f, 20f, 80f, 20f);
                        _oCompany = new Company(); _oBusinessUnit = new BusinessUnit();
                        this.PrintHeaderNormaL(((_bIsGatePass) ? "Gate Pass" : "Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : " "), false);
                    }
                    else
                    {
                        this.PrintHeaderNormaL(((_bIsGatePass) ? "Gate Pass" : "Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : " "), false);
                    }

                    this.PrintHead_Bulk();
                    ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    ESimSolWM_Footer.WMFontSize = 75;
                    ESimSolWM_Footer.WMRotation = 45;
                    ESimSolWM_Footer PageEventHandlerWM = new ESimSolWM_Footer();
                    PageEventHandlerWM.WaterMark = str; //Footer print with page event handler
                    PDFWriter.PageEvent = PageEventHandlerWM; //Footer print with page event handler
                }
            }
            else
            {
                if (isPadFormat)
                {
                    _oDocument.SetMargins(20f, 20f, 80f, 20f);
                    _oCompany = new Company(); _oBusinessUnit = new BusinessUnit();
                    this.PrintHeaderNormaL("Bill", false);
                }
                else
                {
                    this.PrintHeaderNormaL("Bill", false);
                }


                this.PrintHead_Sample();
                ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                ESimSolWM_Footer.WMFontSize = 75;
                ESimSolWM_Footer.WMRotation = 45;
                ESimSolWM_Footer PageEventHandlerWM = new ESimSolWM_Footer();
                PageEventHandlerWM.WaterMark = str; //Footer print with page event handler
                PDFWriter.PageEvent = PageEventHandlerWM; //Footer print with page event handler
            }
            
            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.SetWidths(new float[] { 20f, 60f,70f,  40f, 80f, 70f, 45f, 45f, 60f }); //height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            if (_oFDC.IsBill == false) this.PrintBody();
            else this.PrintBodyForBill();
            _oPdfPTable.HeaderRows = 0;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        public byte[] PrepareReport_B(FabricDeliveryChallan oFDC, Company oCompany, bool bIsGatePass, bool isPadFromat, BusinessUnit oBusinessUnit, List<ApprovalHead> oApprovalHeads)
        {

            _oFDC = oFDC;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _bIsGatePass = bIsGatePass;
            _oApprovalHeads = oApprovalHeads;
            if (oFDC.FDCDetails.Count > 0)
            {
                _sMUnit = oFDC.FDCDetails[0].MUName;
            }
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(PageSize.A4);
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
           
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.SetWidths(new float[] { 20f, 70f,40f, 50f, 40f, 80f, 70f, 32f, 45f }); //height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            #endregion

            if (isPadFromat)
            {
                _oDocument.SetMargins(20f, 20f, 80f, 20f); _oDocument.Open();
                _oCompany = new Company();
                _oBusinessUnit = new BusinessUnit();
                this.PrintHeader(((_bIsGatePass) ? "Gate Pass" : "Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : " (Unapproved)"), oFDC.FDOTypeSt, true);
            }
            else
            {
                _oDocument.Open();
                this.PrintHeader(((_bIsGatePass) ? "Gate Pass" : "Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : " (Unapproved)"), oFDC.FDOTypeSt, true);
            } 

           
            this.PrintHead_B_Bulk();
            this.PrintBody_B();

            _oPdfPTable.HeaderRows = 0;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(string sTitle, bool isB)
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
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
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(sTitle, FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE, (isB? oCustom_Color : BaseColor.BLACK)  )));
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            //#region DateTime
            //_oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase(DateTime.Now.ToString("dd MMM yyy HH:mm"), _oFontStyle));
            //_oPdfPCell.Colspan = _nColumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion

        }
        private void PrintHeader(string sTitle, string DOType, bool isB)
        {
            #region CompanyHeader

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 260.5f, 100f });

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
            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
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
            #endregion

            #region ReportHeader

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0); _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
       
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(sTitle, FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE, (isB ? oCustom_Color : BaseColor.BLACK))));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 0; oPdfPTable.AddCell(_oPdfPCell);
            
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(DOType, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 5f; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        private void PrintHeader()
        {
            #region CompanyHeader

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 70f, 320.5f,70f });

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
            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
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
            _oPdfPCell.Border = 0;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            #endregion

           

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        private void PrintHeaderNormaL(string sTitle,  bool isB)
        {
            #region CompanyHeader

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 260.5f, 100f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            //if (_oCompany.CompanyLogo != null)
            //{
            //    _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    _oImag.ScaleAbsolute(70f, 40f);
            //    _oPdfPCell = new PdfPCell(_oImag);
            //}
            //else
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //}
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //_oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();
            //#endregion

           

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0); _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 60f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase((_oFDC.DisburseBy != 0 ? "" : "Not Disbursed"), FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD, (isB ? oCustom_Color : BaseColor.BLACK))));
            _oPdfPCell.FixedHeight =60f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0;  oPdfPTable.AddCell(_oPdfPCell);
             _oPdfPCell.FixedHeight = 60f;
            oPdfPTable.CompleteRow();
            #endregion


            #region ReportHeader

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0); _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(sTitle, FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD , (isB ? oCustom_Color : BaseColor.BLACK))));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 5f; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 5f; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        private void PrintHeaderTwo(string sTitle, bool isB)
        {
            #region CompanyHeader

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 260.5f, 100f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);



            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0); _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD , (isB ? oCustom_Color : BaseColor.BLACK))));
            _oPdfPCell.FixedHeight = 10f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell.FixedHeight = 10f;
            oPdfPTable.CompleteRow();
            #endregion


            #region ReportHeader

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0); _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(sTitle, FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD , (isB ? oCustom_Color : BaseColor.BLACK))));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 5f; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 5f; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        #endregion

        #region Report Body
        private void TableHeader()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);

            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Dispo No ", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style/Buyer Ref", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Roll/ Than", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty(" + _sMUnit + ")", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }

        private void TableHeaderForBill()
        {
            PdfPTable oPdfTable = new PdfPTable(10);
            oPdfTable.WidthPercentage = 100;
            oPdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfTable.SetWidths(new float[] { 20f, 50f, 70f, 40f, 70f, 45f, 40f, 40f, 50f, 50f }); //height:842   width:595

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);

            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Dispo No ", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style/Buyer Ref", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Roll/ Than", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty(" + _sMUnit + ")", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("U. Price", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTable.AddCell(_oPdfPCell);

            oPdfTable.CompleteRow();

            #region push into main table
            _oPdfPCell = new PdfPCell(oPdfTable); _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintHead_Bulk()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            

            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 84f,  255f, 70f,  165f });

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 10, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, 0, BaseColor.WHITE, 0, 0, 6, 10);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Applicant Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DeliveryToName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_bIsGatePass) ? "Gate Pass No" : "Challan No"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + ((_bIsGatePass) ? _oFDC.GatePassNo : _oFDC.ChallanNo), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Address", 2, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DeliveryToAddress, 2, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL));
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_bIsGatePass) ? "Date" : "Challan Date"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.IssueDate.ToString("dd MMM yyyy HH:mm"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery Point", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DeliveryToAddress, 2, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_bIsGatePass) ? "Challan No" : "Gate Pass No"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + ((_bIsGatePass) ? _oFDC.ChallanNo : _oFDC.GatePassNo) + " DT: " + _oFDC.IssueDateSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Mkt Concern", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.MKTPerson, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery Point", 2, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DeliveryPoint, 2, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery Man", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DeliveryMan, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery Point", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DeliveryPoint, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Vehicle No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.VehicleNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Concern Person", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.BuyerCPName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Driver Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DriverName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "DO No & Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.FabricDONo + " DT:" + _oFDC.FDODateSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Contract No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DriverMobile, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            string sVal = string.Join(", ", _oFDC.FDCDetails.Where(x => !string.IsNullOrEmpty(x.LCNo)).Select(x => x.LCNo + " DT:" + x.LCDateSt).Distinct());
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "LC No & Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + sVal, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            

            //this.AddCell_WithColon(ref oPdfPTable, new string[] { "Applicant Name", _oFDC.DeliveryToName,       "Challan No", _oFDC.ChallanNo });
            //this.AddCell_WithColon(ref oPdfPTable, new string[] { "Address",        _oFDC.DeliveryToAddress,    "    Date", _oFDC.IssueDateSt });
            //this.AddCell_WithColon(ref oPdfPTable, new string[] { "Delivery Point", _oFDC.DeliveryPoint,        "DO No", _oFDC.FabricDONo });
            //this.AddCell_WithColon(ref oPdfPTable, new string[] { "Delivery Man", _oFDC.DisburseByName,         "Gate Pass No", _oFDC.ChallanNo });
            //this.AddCell_WithColon(ref oPdfPTable, new string[] { "Concern Person", _oFDC.BuyerCPName,          "MKT Person", _oFDC.MKTPerson });

            //string sVal = string.Join(", ", _oFDC.FDCDetails.Where(x => !string.IsNullOrEmpty(x.LCNo)).Select(x => x.LCNo + " DT:" + x.LCDateSt).Distinct());
            //this.AddCell_WithColon(ref oPdfPTable, new string[] { "LC No & Date", sVal, "Vehicle No", _oFDC.VehicleNo });
            //this.AddCell_WithColon(ref oPdfPTable, new string[] { "Driver Name", _oFDC.DriverName, "Driver Phone", _oFDC.DriverMobile });
            
            //ESimSolPdfHelper.AddCell(ref oPdfPTable,"", Element.ALIGN_LEFT, 0, BaseColor.WHITE, 0,0,6,10);

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan= _nColumns;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintHead_Sample()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 84f, 270f, 72f, 150f });

            if (string.IsNullOrEmpty(_oFDC.GatePassNo)) { _oFDC.GatePassNo = _oFDC.ChallanNo; }

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 10, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, 0, BaseColor.WHITE, 0, 0, 6, 10);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Applicant Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DeliveryToName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_bIsGatePass) ? "Gate Pass No" : "Challan No"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + ((_bIsGatePass) ? _oFDC.GatePassNo : _oFDC.ChallanNo), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Address", 2, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DeliveryToAddress, 2, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL));
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Challan Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.IssueDateSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery Point", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DeliveryToAddress, 2, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_bIsGatePass) ? "Challan No" : "Gate Pass No"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + ((_bIsGatePass) ? _oFDC.ChallanNo : _oFDC.GatePassNo) + " DT: " + _oFDC.IssueDateSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Mkt Concern", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.MKTPerson, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery Point", 2, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DeliveryPoint, 2, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery Man", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DeliveryMan, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery Point", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DeliveryPoint, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Vehicle No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.VehicleNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Concern Person", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.BuyerCPName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Driver Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DriverName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            //oPdfPTable.CompleteRow();
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "DO No & Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.FabricDONo + " " + _oFDC.FDODateSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Contract No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + _oFDC.DriverMobile, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            //oPdfPTable.CompleteRow();
            //string sVal = string.Join(", ", _oFDC.FDCDetails.Where(x => !string.IsNullOrEmpty(x.LCNo)).Select(x => x.LCNo + " DT:" + x.LCDateSt).Distinct());
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "LC No & Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ": " + sVal, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            //oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintBody()
        {
            #region Fabric Delivery Challan Detail

            this.TableHeader();
            iTextSharp.text.Font oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            string sTemp = "";
            double nQty = 0;
            #region Table Content
            if (_oFDC.FDCDetails.Any())
            {
                int nCount = 0;
                var oresults = _oFDC.FDCDetails.GroupBy(x => x.ExeNo).Select(g => new
                {
                    ExeNo = g.Key,
                    Construction = g.FirstOrDefault().Construction,
                    FinishTypeName = g.FirstOrDefault().FinishTypeName,
                    ShadeStr = g.FirstOrDefault().ShadeStr,
                    FinishWidth = g.FirstOrDefault().FinishWidth,
                    StyleNo = g.FirstOrDefault().StyleNo,
                    ColorInfo = g.FirstOrDefault().ColorInfo,
                    GoodsDesp = GetGoodsDescription(g.FirstOrDefault()),
                    TotalRoll = g.Count(),
                    Qty = g.Sum(p => p.Qty)
                });

                var oFDCDetails = _oFDC.FDCDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.StyleNo, x.BuyerRef, x.FabricWeave, x.Construction, x.FinishWidth, x.MUName, x.ProcessTypeName, x.FinishTypeName }, (key, grp) =>
                                   new
                                   {
                                       ProductID = key.ProductID,
                                       ProductName = key.ProductName,
                                       GoodsDesp = GetGoodsDescription(grp.FirstOrDefault()),
                                       Color_Grp = grp.GroupBy(p => new { p.ExeNo, p.StyleNo,p.BuyerRef, p.ColorInfo }, (color_key, color_grp) => new
                                       {
                                           StyleNo = color_key.StyleNo,
                                           BuyerRef = color_key.BuyerRef,
                                           ExeNo = color_key.ExeNo,
                                           ColorInfo = color_key.ColorInfo,
                                           Qty = color_grp.Sum(p => p.Qty),
                                           NoRoll = color_grp.Count(),
                                       }).ToList(),
                                       Construction = key.Construction,
                                     
                                       FinishWidth = key.FinishWidth,
                                       FabricWeave = key.FabricWeave,
                                       FinishTypeName = key.FinishTypeName, //grp.Select(x => x.FinishType).FirstOrDefault(),
                                       MUName = key.MUName,
                                       ProcessTypeName = key.ProcessTypeName,
                                   }).ToList();



                foreach (var oItem in oFDCDetails)
                {
                    int nRowSpan = oItem.Color_Grp.Count();

                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, (++nCount).ToString(), nRowSpan, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.GoodsDesp, nRowSpan, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, oFontStyle);
                    foreach (var oColor in oItem.Color_Grp)
                    {
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oColor.ExeNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oColor.StyleNo + "," + oColor.BuyerRef, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oColor.ColorInfo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oColor.Color_List.FirstOrDefault(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oColor.NoRoll.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(oColor.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);                    
                    }
                    _oPdfPTable.CompleteRow();
                }

                nQty = _oFDC.FDCDetails.Sum(x => x.Qty);
             
                if (_oFabricDeliveryChallanDetailsAdj.Count>0)
                {
                  
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Total ", 0, _nColumns - 2, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Adjustment Detail", 0, _nColumns, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                    oFDCDetails = _oFabricDeliveryChallanDetailsAdj.GroupBy(x => new {  x.ProductID, x.ProductName, x.StyleNo, x.BuyerRef, x.FabricWeave, x.Construction, x.FinishWidth, x.MUName, x.ProcessTypeName, x.FinishTypeName }, (key, grp) =>
                                new
                                {
                                    ProductID = key.ProductID,
                                    ProductName = key.ProductName,
                                    GoodsDesp = GetGoodsDescription(grp.FirstOrDefault()),
                                    Color_Grp = grp.GroupBy(p => new { p.ExeNo, p.StyleNo,p.BuyerRef, p.ColorInfo }, (color_key, color_grp) => new
                                    {
                                        StyleNo = color_key.StyleNo,
                                        BuyerRef = color_key.BuyerRef,
                                        ExeNo = color_key.ExeNo,
                                        ColorInfo = color_key.ColorInfo,
                                        Qty = color_grp.Sum(p => p.Qty),
                                        NoRoll = color_grp.Count(),
                                    }).ToList(),
                                    Construction = key.Construction,
                                  
                                    FinishWidth = key.FinishWidth,
                                    FabricWeave = key.FabricWeave,
                                    FinishTypeName = key.FinishTypeName, //grp.Select(x => x.FinishType).FirstOrDefault(),
                                    MUName = key.MUName,
                                    ProcessTypeName = key.ProcessTypeName,
                                }).ToList();

                    nQty=nQty+_oFabricDeliveryChallanDetailsAdj.Sum(x => x.Qty);
                    nCount = 0;
                    foreach (var oItem in oFDCDetails)
                    {
                        int nRowSpan = oItem.Color_Grp.Count();

                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, (++nCount).ToString(), nRowSpan, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.GoodsDesp, nRowSpan, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, oFontStyle);
                        foreach (var oColor in oItem.Color_Grp)
                        {
                            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oColor.ExeNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oColor.StyleNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oColor.ColorInfo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                            //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oColor.Color_List.FirstOrDefault(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oColor.NoRoll.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(oColor.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        }
                        _oPdfPTable.CompleteRow();
                    }
                 
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Adjusted(Sample) ", 0, _nColumns - 2, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(_oFabricDeliveryChallanDetailsAdj.Sum(x => x.Qty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                    _oPdfPTable.CompleteRow();

                }
                float nHeight = 15f;
                for (int i = oFDCDetails.Count + 1; i <= (8 - oFDCDetails.Count); i++)
                {
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, nHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nHeight, oFontStyle);
                    _oPdfPTable.CompleteRow();
                }

                if (_oFabricDeliveryChallanDetailsAdj.Count > 0)
                {
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Grand Total", 0, _nColumns - 2, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                }
                else
                {
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Total", 0, _nColumns - 2, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                }
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                _oPdfPTable.CompleteRow();

                oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
             
                if (!string.IsNullOrEmpty(_oFDC.Note))
                {
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Remarks : " + _oFDC.Note, 0, _nColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle, true);
                }

                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Received the fabrics in good condition", 0, _nColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle, true);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _nColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle, true);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, " NB: If there is any discrepancy between this delivery challan and actual receiving, immediately inform the undersigned in written. ", 0, _nColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle, true);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Drivers Name: " + _oFDC.DriverName, 0, _nColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle, true);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Vehicle No: " + _oFDC.VehicleNo, 0, _nColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle, true);

                if (_oApprovalHeads != null && _oApprovalHeads.Any())
                {
                    nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                    this.PrintFooter(700 - nUsagesHeight);
                }

                else
                {
                    this.PrintFooter_Default();
                }

                //_oDocument.Add(_oPdfPTable);
                //_oPdfPTable.DeleteBodyRows();
                //_oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                //foreach (var oItem in oresults)
                //{
                //    PdfPTable oPdfPTable = Table_Packing();
                //    var oFDCDs = _oFDC.FDCDetails.Where(x => x.ExeNo == oItem.ExeNo).ToList();
                //    int nData = oFDCDs.Count();
                //    int nRows = (nData / 3) + ((nData % 3 == 0) ? 0 : 3-(nData % 3));
                //    nCount = 0;
                //    for (int i = 0; i < nRows; i += 40)
                //    {
                //        _oDocument.NewPage();

                //        this.PrintHeader("Packing List", false);
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Customer: " + _oFDC.DeliveryToName, 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Construction: ", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.Construction, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Date: ", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, _oFDC.IssueDateSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                //        _oPdfPTable.CompleteRow();

                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Dispo No: "+ oItem.ExeNo, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Finish Type: " + oItem.FinishTypeName + " Color: " + oItem.ColorInfo + " Shade: " + oItem.ShadeStr, 0, 5, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
                //        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Shade:", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, oFontStyle);
                //        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.ShadeStr, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Finish Width:", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.FinishWidth, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                //        _oPdfPTable.CompleteRow();


                //        SetValueToParentTable(ref nCount, 40, nData, oFDCDs);

                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Total: ", 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(oFDCDs.Sum(x => x.Qty)) + "  " + _sMUnit + ", Total Roll:" + oFDCDs.Count(), 0, 5, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //        _oPdfPTable.CompleteRow();
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "  ", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, false, 40, oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 40, oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 40, oFontStyle);
                //        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(oFDCDs.Sum(x => x.Qty)) + "         Yard, Total Roll:" + oFDCDs.Count(), 0, 5, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //        _oPdfPTable.CompleteRow();
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "_____________  ", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, false, 15, oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "_____________  ", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "_____________  ", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 15, oFontStyle);
                //        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(oFDCDs.Sum(x => x.Qty)) + "         Yard, Total Roll:" + oFDCDs.Count(), 0, 5, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //        _oPdfPTable.CompleteRow();
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Received By ", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, false, 10, oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Prepared By", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 10, oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Approved By ", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 10, oFontStyle);
                //        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(oFDCDs.Sum(x => x.Qty)) + "  Yard, Total Roll:" + oFDCDs.Count(), 0, 5, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //        _oPdfPTable.CompleteRow();
                //        _oDocument.Add(_oPdfPTable);
                //        _oPdfPTable.DeleteBodyRows();
                //    }
                //}
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "G.Total:" + Global.MillionFormat(_oFDC.FDCDetails.Sum(x => x.Qty)) + "  Yard, Total Roll:" + _oFDC.FDCDetails.Count(), 0, 9, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //_oPdfPTable.CompleteRow();
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, , 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold, true);
                //PrintFooter_Two();

                //nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
               
                //if (nUsagesHeight <0)
                //{
                //    nUsagesHeight = 15;
                //}
                //if (nUsagesHeight >0 && nUsagesHeight< 50)
                //{
                //    nUsagesHeight = 20;
                //}
                //if (nUsagesHeight >=50 && nUsagesHeight >100)
                //{
                //    nUsagesHeight = 30;
                //}
                //if (nUsagesHeight >= 100 && nUsagesHeight > 300)
                //{
                //    nUsagesHeight = 40;
                //}

                //_oPdfPTable.CompleteRow();
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "__________", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, nUsagesHeight, oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "___________", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, nUsagesHeight, oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "__________", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, nUsagesHeight, oFontStyle);
                //_oPdfPTable.CompleteRow();
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Received By", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Prepared By", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Approved By", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
                //_oPdfPTable.CompleteRow();
                
            }
            else
            {
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                _oPdfPTable.CompleteRow();
            }

          
            #endregion
            #endregion
        }
        private void PrintBody_PackingList()
        {
            #region Fabric Delivery Challan Detail

           // this.TableHeader();
            iTextSharp.text.Font oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            string sTemp = "";
            double nQty = 0;
            #region Table Content
            if (_oFDC.FDCDetails.Any())
            {
                int nCount = 0;
                var oresults = _oFDC.FDCDetails.GroupBy(x => x.ExeNo).Select(g => new
                {
                    ExeNo = g.Key,
                    Construction = g.FirstOrDefault().Construction,
                    FinishTypeName = g.FirstOrDefault().FinishTypeName,
                    ShadeStr = g.FirstOrDefault().ShadeStr,
                    FinishWidth = g.FirstOrDefault().FinishWidth,
                    StyleNo = g.FirstOrDefault().StyleNo,
                    ColorInfo = g.FirstOrDefault().ColorInfo,
                    GoodsDesp = GetGoodsDescription(g.FirstOrDefault()),
                    TotalRoll = g.Count(),
                    Qty = g.Sum(p => p.Qty)
                });

                _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                foreach (var oItem in oresults)
                {
                    PdfPTable oPdfPTable = Table_Packing();
                    var oFDCDs = _oFDC.FDCDetails.Where(x => x.ExeNo == oItem.ExeNo).ToList();
                    int nData = oFDCDs.Count();
                    int nRows = (nData / 3) + ((nData % 3 == 0) ? 0 : 3 - (nData % 3));
                    nCount = 0;
                    for (int i = 0; i < nRows; i += 40)
                    {
                        _oDocument.NewPage();

                        //this.PrintHeader("Packing List", false);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Customer: " + _oFDC.DeliveryToName, 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Construction: ", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.Construction, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Date: ", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, _oFDC.IssueDateSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                        _oPdfPTable.CompleteRow();

                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Dispo No: " + oItem.ExeNo, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Finish Type: " + oItem.FinishTypeName + " Color: " + oItem.ColorInfo + " Shade: " + oItem.ShadeStr, 0, 5, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
                        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Shade:", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, oFontStyle);
                        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.ShadeStr, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Finish Width:", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.FinishWidth, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                        _oPdfPTable.CompleteRow();


                        SetValueToParentTable(ref nCount, 40, nData, oFDCDs);

                        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Total: ", 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(oFDCDs.Sum(x => x.Qty)) + "  " + _sMUnit + ", Total Roll:" + oFDCDs.Count(), 0, 5, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                        //_oPdfPTable.CompleteRow();
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "  ", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, false, 40, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 40, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 40, oFontStyle);
                        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(oFDCDs.Sum(x => x.Qty)) + "         Yard, Total Roll:" + oFDCDs.Count(), 0, 5, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                        _oPdfPTable.CompleteRow();
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "_____________  ", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, false, 15, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "_____________  ", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "_____________  ", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 15, oFontStyle);
                        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(oFDCDs.Sum(x => x.Qty)) + "         Yard, Total Roll:" + oFDCDs.Count(), 0, 5, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                        _oPdfPTable.CompleteRow();
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Received By ", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, false, 10, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Prepared By", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 10, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Approved By ", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 10, oFontStyle);
                        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(oFDCDs.Sum(x => x.Qty)) + "  Yard, Total Roll:" + oFDCDs.Count(), 0, 5, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                        _oPdfPTable.CompleteRow();
                        _oDocument.Add(_oPdfPTable);
                        _oPdfPTable.DeleteBodyRows();
                    }
               
                }
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "G.Total:" + Global.MillionFormat(_oFDC.FDCDetails.Sum(x => x.Qty)) + "  Yard, Total Roll:" + _oFDC.FDCDetails.Count(), 0, 9, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //_oPdfPTable.CompleteRow();
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, , 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold, true);
                //PrintFooter_Two();

                //nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

                //if (nUsagesHeight <0)
                //{
                //    nUsagesHeight = 15;
                //}
                //if (nUsagesHeight >0 && nUsagesHeight< 50)
                //{
                //    nUsagesHeight = 20;
                //}
                //if (nUsagesHeight >=50 && nUsagesHeight >100)
                //{
                //    nUsagesHeight = 30;
                //}
                //if (nUsagesHeight >= 100 && nUsagesHeight > 300)
                //{
                //    nUsagesHeight = 40;
                //}

                //_oPdfPTable.CompleteRow();
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "__________", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, nUsagesHeight, oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "___________", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, nUsagesHeight, oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "__________", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, nUsagesHeight, oFontStyle);
                //_oPdfPTable.CompleteRow();
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Received By", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Prepared By", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Approved By", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
                //_oPdfPTable.CompleteRow();

            }
            else
            {
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                _oPdfPTable.CompleteRow();
            }


            #endregion
            #endregion
        }

        private void PrintBodyForBill()
        {
            #region Fabric Delivery Challan Detail

            this.TableHeaderForBill();
            iTextSharp.text.Font oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            string sTemp = "";
            double nQty = 0, nAmount = 0;
            #region Table Content

            #region Initialize table
            PdfPTable oPdfTable = new PdfPTable(10);
            oPdfTable.WidthPercentage = 100;
            oPdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfTable.SetWidths(new float[] { 20f, 50f, 70f, 40f, 70f, 45f, 40f, 40f, 50f, 50f }); //height:842   width:595
            #endregion

            if (_oFDC.FDCDetails.Any())
            {
                int nCount = 0;
                var oresults = _oFDC.FDCDetails.GroupBy(x => x.ExeNo).Select(g => new
                {
                    ExeNo = g.Key,
                    Construction = g.FirstOrDefault().Construction,
                    FinishTypeName = g.FirstOrDefault().FinishTypeName,
                    ShadeStr = g.FirstOrDefault().ShadeStr,
                    FinishWidth = g.FirstOrDefault().FinishWidth,
                    StyleNo = g.FirstOrDefault().StyleNo,
                    ColorInfo = g.FirstOrDefault().ColorInfo,
                    GoodsDesp = GetGoodsDescription(g.FirstOrDefault()),
                    TotalRoll = g.Count(),
                    Qty = g.Sum(p => p.Qty)
                });

                var oFDCDetails = _oFDC.FDCDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.StyleNo, x.BuyerRef, x.FabricWeave, x.Construction, x.FinishWidth, x.MUName, x.ProcessTypeName, x.FinishTypeName }, (key, grp) =>
                                   new
                                   {
                                       ProductID = key.ProductID,
                                       ProductName = key.ProductName,
                                       GoodsDesp = GetGoodsDescription(grp.FirstOrDefault()),
                                       Color_Grp = grp.GroupBy(p => new { p.ExeNo, p.StyleNo, p.BuyerRef, p.ColorInfo, p.UnitPrice, p.DOPriceType ,p.SCDetailType}, (color_key, color_grp) => new
                                       {
                                           StyleNo = color_key.StyleNo,
                                           BuyerRef = color_key.BuyerRef,
                                           ExeNo = color_key.ExeNo,
                                           ColorInfo = color_key.ColorInfo,
                                           Qty = color_grp.Sum(p => p.Qty),
                                           UnitPrice = color_key.UnitPrice,
                                           DOPriceType = color_key.DOPriceType,
                                           SCDetailType = color_key.SCDetailType,
                                           Amount = (color_grp.Sum(p => p.Qty) * color_key.UnitPrice),
                                           NoRoll = color_grp.Count(),
                                       }).ToList(),
                                       Construction = key.Construction,

                                       FinishWidth = key.FinishWidth,
                                       FabricWeave = key.FabricWeave,
                                       FinishTypeName = key.FinishTypeName, //grp.Select(x => x.FinishType).FirstOrDefault(),
                                       MUName = key.MUName,
                                       ProcessTypeName = key.ProcessTypeName,
                                   }).ToList();



                foreach (var oItem in oFDCDetails)
                {
                    int nRowSpan = oItem.Color_Grp.Count();

                    ESimSolItexSharp.SetCellValue(ref oPdfTable, (++nCount).ToString(), nRowSpan, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, oItem.GoodsDesp, nRowSpan, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, oFontStyle);
                    foreach (var oColor in oItem.Color_Grp)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfTable, oColor.ExeNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfTable, oColor.StyleNo + "," + oColor.BuyerRef, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        //ESimSolItexSharp.SetCellValue(ref oPdfTable, oColor.ColorInfo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        //ESimSolItexSharp.SetCellValue(ref oPdfTable, oColor.Color_List.FirstOrDefault(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfTable, oColor.NoRoll.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfTable, Global.MillionFormat(oColor.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfTable, Global.MillionFormat(oColor.UnitPrice), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfTable, Global.MillionFormat(oColor.Amount), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, oFontStyle);
                        if (oColor.SCDetailType == EnumSCDetailType.DeductCharge)
                        {
                            nAmount =nAmount- oColor.Amount;
                        }
                        else
                        {
                            nAmount += oColor.Amount;
                        }
                       
                        if (oColor.DOPriceType == EnumDOPriceType.CutPiece)
                        {
                            sTemp = oColor.DOPriceType.ToString();
                        }
                        else { sTemp = ""; }
                        ESimSolItexSharp.SetCellValue(ref oPdfTable, sTemp, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                    }
                    oPdfTable.CompleteRow();

                    #region push into main table
                    _oPdfPCell = new PdfPCell(oPdfTable); _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Initialize table
                    oPdfTable = new PdfPTable(10);
                    oPdfTable.WidthPercentage = 100;
                    oPdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfTable.SetWidths(new float[] { 20f, 50f, 70f, 40f, 70f, 45f, 40f, 40f, 50f, 50f }); //height:842   width:595
                    #endregion
                }

                nQty = _oFDC.FDCDetails.Where(f => f.SCDetailType == EnumSCDetailType.None || f.SCDetailType == EnumSCDetailType.ExtraOrder).Sum(x => x.Qty);
                //nAmount = _oFDC.FDCDetails.Sum(x => x.Amount);

                #region _oFabricDeliveryChallanDetailsAdj
                if (_oFabricDeliveryChallanDetailsAdj.Count > 0)
                {

                    ESimSolItexSharp.SetCellValue(ref oPdfTable, "Total ", 0, 10 - 4, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, Global.MillionFormat(nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, " ", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, Global.MillionFormat(nAmount), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref oPdfTable, "Adjustment Detail", 0, 10, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                    oFDCDetails = _oFabricDeliveryChallanDetailsAdj.GroupBy(x => new { x.ProductID, x.ProductName, x.StyleNo, x.BuyerRef, x.FabricWeave, x.Construction, x.FinishWidth, x.MUName, x.ProcessTypeName, x.FinishTypeName }, (key, grp) =>
                                new
                                {
                                    ProductID = key.ProductID,
                                    ProductName = key.ProductName,
                                    GoodsDesp = GetGoodsDescription(grp.FirstOrDefault()),
                                    Color_Grp = grp.GroupBy(p => new { p.ExeNo, p.StyleNo, p.BuyerRef, p.ColorInfo, p.UnitPrice, p.DOPriceType, p.SCDetailType }, (color_key, color_grp) => new
                                    {
                                        StyleNo = color_key.StyleNo,
                                        BuyerRef = color_key.BuyerRef,
                                        ExeNo = color_key.ExeNo,
                                        ColorInfo = color_key.ColorInfo,
                                        Qty = color_grp.Sum(p => p.Qty),
                                        UnitPrice = color_key.UnitPrice,
                                        DOPriceType = color_key.DOPriceType,
                                        SCDetailType= color_key.SCDetailType,
                                        Amount = (color_grp.Sum(p => p.Qty) * color_key.UnitPrice),
                                        NoRoll = color_grp.Count(),
                                    }).ToList(),
                                    Construction = key.Construction,

                                    FinishWidth = key.FinishWidth,
                                    FabricWeave = key.FabricWeave,
                                    FinishTypeName = key.FinishTypeName, //grp.Select(x => x.FinishType).FirstOrDefault(),
                                    MUName = key.MUName,
                                    ProcessTypeName = key.ProcessTypeName,
                                }).ToList();

                    nQty = nQty + _oFabricDeliveryChallanDetailsAdj.Sum(x => x.Qty);
                    nCount = 0;
                    //nAmount += _oFabricDeliveryChallanDetailsAdj.Sum(x => x.Amount);
                    foreach (var oItem in oFDCDetails)
                    {
                        int nRowSpan = oItem.Color_Grp.Count();

                        ESimSolItexSharp.SetCellValue(ref oPdfTable, (++nCount).ToString(), nRowSpan, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfTable, oItem.GoodsDesp, nRowSpan, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, oFontStyle);
                        foreach (var oColor in oItem.Color_Grp)
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfTable, oColor.ExeNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref oPdfTable, oColor.StyleNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                            ////ESimSolItexSharp.SetCellValue(ref oPdfTable, oColor.ColorInfo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                            //ESimSolItexSharp.SetCellValue(ref oPdfTable, oColor.Color_List.FirstOrDefault(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref oPdfTable, oColor.NoRoll.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref oPdfTable, Global.MillionFormat(oColor.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref oPdfTable, Global.MillionFormat(oColor.UnitPrice), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref oPdfTable, Global.MillionFormat(oColor.Amount), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, oFontStyle);
                            nAmount += oColor.Amount;
                            ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        }
                        oPdfTable.CompleteRow();

                        #region push into main table
                        _oPdfPCell = new PdfPCell(oPdfTable); _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        #region Initialize table
                        oPdfTable = new PdfPTable(10);
                        oPdfTable.WidthPercentage = 100;
                        oPdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfTable.SetWidths(new float[] { 20f, 50f, 70f, 40f, 70f, 45f, 40f, 40f, 50f, 50f }); //height:842   width:595
                        #endregion
                    }

                    ESimSolItexSharp.SetCellValue(ref oPdfTable, "Adjusted(Sample) ", 0, 10 - 4, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, Global.MillionFormat(_oFabricDeliveryChallanDetailsAdj.Sum(x => x.Qty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, Global.MillionFormat(nAmount), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                    oPdfTable.CompleteRow();

                    #region push into main table
                    _oPdfPCell = new PdfPCell(oPdfTable); _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Initialize table
                    oPdfTable = new PdfPTable(10);
                    oPdfTable.WidthPercentage = 100;
                    oPdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfTable.SetWidths(new float[] { 20f, 50f, 70f, 40f, 70f, 45f, 40f, 40f, 50f, 50f }); //height:842   width:595
                    #endregion

                }
                #endregion

                float nHeight = 15f;
                for (int i = oFDCDetails.Count + 1; i <= (8 - oFDCDetails.Count); i++)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, nHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nHeight, oFontStyle);
                    oPdfTable.CompleteRow();

                    #region push into main table
                    _oPdfPCell = new PdfPCell(oPdfTable); _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Initialize table
                    oPdfTable = new PdfPTable(10);
                    oPdfTable.WidthPercentage = 100;
                    oPdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfTable.SetWidths(new float[] { 20f, 50f, 70f, 40f, 70f, 45f, 40f, 40f, 50f, 50f }); //height:842   width:595
                    #endregion
                }

                if (_oFabricDeliveryChallanDetailsAdj.Count > 0)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, "Grand Total", 0, 10 - 4, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                }
                else
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfTable, "Total", 0, 10 - 4, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                }
                ESimSolItexSharp.SetCellValue(ref oPdfTable, Global.MillionFormat(nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfTable, Global.MillionFormat(nAmount), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                oPdfTable.CompleteRow();

                #region push into main table
                _oPdfPCell = new PdfPCell(oPdfTable); _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Initialize table
                oPdfTable = new PdfPTable(10);
                oPdfTable.WidthPercentage = 100;
                oPdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfTable.SetWidths(new float[] { 20f, 50f, 70f, 40f, 70f, 45f, 40f, 40f, 50f, 50f }); //height:842   width:595
                #endregion

                oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

                if (!string.IsNullOrEmpty(_oFDC.Note))
                {
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Remarks : " + _oFDC.Note, 0, _nColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle, true);
                }

                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Received the fabrics in good condition", 0, _nColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle, true);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _nColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle, true);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, " NB: If there is any discrepancy between this delivery challan and actual receiving, immediately inform the undersigned in written. ", 0, _nColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle, true);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Drivers Name: " + _oFDC.DriverName, 0, _nColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle, true);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Vehicle No: " + _oFDC.VehicleNo, 0, _nColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle, true);

                if (_oApprovalHeads != null && _oApprovalHeads.Any())
                {
                    nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                    this.PrintFooter(700 - nUsagesHeight);
                }

                else
                {
                    this.PrintFooter_Default();
                }

            }
            else
            {
                ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                oPdfTable.CompleteRow();

                #region push into main table
                _oPdfPCell = new PdfPCell(oPdfTable); _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Initialize table
                oPdfTable = new PdfPTable(10);
                oPdfTable.WidthPercentage = 100;
                oPdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfTable.SetWidths(new float[] { 20f, 50f, 70f, 40f, 70f, 45f, 40f, 40f, 50f, 50f }); //height:842   width:595
                #endregion
            }


            #endregion
            #endregion
        }

        private void AddCell_WithColon(ref PdfPTable oPdfPTable, string[] sData) 
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9, iTextSharp.text.Font.NORMAL);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, sData[0], Element.ALIGN_RIGHT, 0, BaseColor.WHITE, 0); ESimSolPdfHelper.AddCell(ref oPdfPTable, " :", 0, 0, BaseColor.WHITE, 0);
            //ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9, iTextSharp.text.Font.NORMAL);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, sData[1], Element.ALIGN_LEFT, 0, BaseColor.WHITE, 0);

            //ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, sData[2], Element.ALIGN_RIGHT, 0, BaseColor.WHITE, 0); ESimSolPdfHelper.AddCell(ref oPdfPTable, " : ", 0, 0, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, sData[3], Element.ALIGN_LEFT, 0, BaseColor.WHITE, 0);
        }
        #endregion

        #region Report Body _B
        private void TableHeader_B()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            _oFontStyle.Color = BaseColor.WHITE;

            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));  
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = oCustom_Color; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = oCustom_Color; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Dispo No ", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = oCustom_Color; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style/Buyer Ref", _oFontStyle)); _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = oCustom_Color; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle)); 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = oCustom_Color; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Roll/ Than", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = oCustom_Color; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty("+_sMUnit+")", _oFontStyle)); 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = oCustom_Color; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        private void PrintHead_B_Bulk()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPCell oPdfPCell;
            PdfPTable oPdfPTable_Parent = new PdfPTable(3);
            oPdfPTable_Parent.SetWidths(new float[] { 300f, 36f, 260f });

            #region 1st Table LEFT
            PdfPTable oPdfPTable_LEFT = new PdfPTable(2);
            oPdfPTable_LEFT.SetWidths(new float[] { 80, 220 });

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            //oPdfPCell = new PdfPCell(new Phrase("Challan Information :", _oFontStyle)); oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = oCustom_Color; oPdfPCell.Colspan = 4;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPTable_Right.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            ESimSolPdfHelper.FontStyle = _oFontStyle;
            ESimSolPdfHelper.AddCell(ref oPdfPTable_LEFT, "Buyer Name:", Element.ALIGN_RIGHT, 0, BaseColor.WHITE, iTextSharp.text.Rectangle.RIGHT_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_LEFT, _oFDC.BuyerName, 0, 0, BaseColor.WHITE, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable_LEFT, "Garments Name:", Element.ALIGN_RIGHT, 0, BaseColor.WHITE, iTextSharp.text.Rectangle.RIGHT_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_LEFT, _oFDC.DeliveryToName, 0, 0, BaseColor.WHITE, 15);

            string sVal = string.Join(", ", _oFDC.FDCDetails.Where(x => !string.IsNullOrEmpty(x.PINo)).Select(x => x.PINo + " DT:" + x.PIDateSt).Distinct());
            ESimSolPdfHelper.AddCell(ref oPdfPTable_LEFT, "PI No & Date:", Element.ALIGN_RIGHT, 0, BaseColor.WHITE, iTextSharp.text.Rectangle.RIGHT_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_LEFT, sVal, 0, 0, BaseColor.WHITE, 15);

            sVal = string.Join(", ", _oFDC.FDCDetails.Where(x => !string.IsNullOrEmpty(x.LCNo)).Select(x => x.LCNo + " DT:" + x.LCDateSt).Distinct());
            ESimSolPdfHelper.AddCell(ref oPdfPTable_LEFT, "LC No & Date:", Element.ALIGN_RIGHT, 0, BaseColor.WHITE, iTextSharp.text.Rectangle.RIGHT_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_LEFT, sVal, 0, 0, BaseColor.WHITE, 15);
            
            ESimSolPdfHelper.AddTable(ref oPdfPTable_Parent, oPdfPTable_LEFT, 0,0,0,0,false);
            #endregion

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.MinimumHeight = 15f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable_Parent.AddCell(oPdfPCell);

            #region 1st Table RIGHT
            PdfPTable oPdfPTable_Right = new PdfPTable(2);
            oPdfPTable_Right.SetWidths(new float[] { 80,180 });

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            //oPdfPCell = new PdfPCell(new Phrase("Delivery Order Ref :", _oFontStyle)); oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = oCustom_Color; oPdfPCell.Colspan = 4;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPTable_Right.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            ESimSolPdfHelper.FontStyle = _oFontStyle;
           
            ESimSolPdfHelper.AddCell(ref oPdfPTable_Right, "Challan No:", Element.ALIGN_RIGHT, 0, BaseColor.WHITE, iTextSharp.text.Rectangle.RIGHT_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_Right, _oFDC.ChallanNo, 0, 0, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_Right, "Challan Date:", Element.ALIGN_RIGHT, 0, BaseColor.WHITE, iTextSharp.text.Rectangle.RIGHT_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_Right, _oFDC.IssueDateSt, 0, 0, BaseColor.WHITE, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable_Right, "DO No:", Element.ALIGN_RIGHT, 0, BaseColor.WHITE, iTextSharp.text.Rectangle.RIGHT_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_Right, _oFDC.FabricDONo, 0, 0, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_Right, "DO Date:", Element.ALIGN_RIGHT, 0, BaseColor.WHITE, iTextSharp.text.Rectangle.RIGHT_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_Right, _oFDC.FDODateSt, 0, 0, BaseColor.WHITE, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable_Right, "Gate Pass No:", Element.ALIGN_RIGHT, 0, BaseColor.WHITE, iTextSharp.text.Rectangle.RIGHT_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_Right, _oFDC.ChallanNo, 0, 0, BaseColor.WHITE, 15);

          
            #endregion
            ESimSolPdfHelper.AddTable(ref oPdfPTable_Parent, oPdfPTable_Right, 0, 0, 0, 0, false);


            ESimSolPdfHelper.AddCell(ref oPdfPTable_Parent, "", 0, 0, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_Parent, "", 0, 0, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_Parent, "", 0, 0, BaseColor.WHITE, 0);

            #region 2nd Table LEFT
            oPdfPTable_LEFT = new PdfPTable(1);
            oPdfPTable_LEFT.SetWidths(new float[] { 200f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            oPdfPCell = new PdfPCell(new Phrase("Shipping Address:", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = oCustom_Color;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            oPdfPTable_LEFT.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            oPdfPCell = new PdfPCell(new Phrase(_oFDC.DeliveryPoint, _oFontStyle));
            oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            oPdfPTable_LEFT.AddCell(oPdfPCell);
            #endregion
            ESimSolPdfHelper.AddTable(ref oPdfPTable_Parent, oPdfPTable_LEFT, 0, 0, 0, 0, false);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable_Parent.AddCell(oPdfPCell);

            #region 2nd Table RIGHT
            oPdfPTable_LEFT = new PdfPTable(1);
            oPdfPTable_LEFT.SetWidths(new float[] { 300f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            oPdfPCell = new PdfPCell(new Phrase("Vehicle Information :", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = oCustom_Color;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable_LEFT.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_LEFT, "Vehicle Type: "+_oFDC.VehicleTypeName, 0,0,BaseColor.WHITE,0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_LEFT, "Vehicle No: " + _oFDC.VehicleNo, 0, 0, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_LEFT, "Driver Name: " + _oFDC.DriverName + " Mobile: " + _oFDC.DriverMobile, 0, 0, BaseColor.WHITE, 0);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable_LEFT, "Mobile: " + _oFDC.DriverMobile, 0, 0, BaseColor.WHITE, 0);
            #endregion
            ESimSolPdfHelper.AddTable(ref oPdfPTable_Parent, oPdfPTable_LEFT, 0, 0, 0, 0, false);
            
             _oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable_Parent, 0,0,9);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", 0, 0, BaseColor.WHITE, 0, 0, 9, 0);
           
        }
        private void PrintBody_B()
        {
            #region Fabric Delivery Challan Detail
            double nNoRoll = 0;

            this.TableHeader_B();
            iTextSharp.text.Font oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            string sTemp = "";
            #region Table Content
            if (_oFDC.FDCDetails.Any())
            {
                int nCount = 0;
                var oresults = _oFDC.FDCDetails.GroupBy(x => x.ExeNo).Select(g => new
                {
                    ExeNo = g.Key,
                    Construction = g.FirstOrDefault().Construction,
                    ProductName = g.FirstOrDefault().ProductName,
                    FinishTypeName = g.FirstOrDefault().FinishTypeName,
                    ShadeStr = g.FirstOrDefault().ShadeStr,
                    FinishWidth = g.FirstOrDefault().FinishWidth,
                    StyleNo = g.FirstOrDefault().StyleNo,
                    ColorInfo = g.FirstOrDefault().ColorInfo,
                    GoodsDesp = GetGoodsDescription(g.FirstOrDefault()),
                    TotalRoll = g.Count(),
                    Qty = g.Sum(p => p.Qty)
                });

                var oFDCDetails = _oFDC.FDCDetails.GroupBy(x => new { x.FabricNo, x.ProductID, x.ProductName, x.StyleNo, x.FabricWeave, x.Construction, x.FinishWidth, x.MUName, x.ProcessTypeName, x.FinishTypeName }, (key, grp) =>
                                         new
                                         {
                                             ProductID = key.ProductID,
                                             ProductName = key.ProductName,
                                             StyleNo = key.StyleNo,
                                             Color_Grp = grp.GroupBy(p => new { p.ExeNo, p.StyleNo, p.BuyerRef, p.ColorInfo }, (color_key, color_grp) => new
                                             {
                                                                                StyleNo = color_key.StyleNo,
                                                                                BuyerRef = color_key.BuyerRef,
                                                                                ExeNo = color_key.ExeNo,
                                                                                ColorInfo = color_key.ColorInfo,
                                                                                Qty = color_grp.Sum(p => p.Qty),
                                                                                NoRoll = color_grp.Count(),
                                                                            }).ToList(),
                                             Construction = key.Construction,
                                             FabricNo = key.FabricNo,
                                             FinishWidth = key.FinishWidth,
                                             FabricWeave = key.FabricWeave,
                                             FinishTypeName = key.FinishTypeName,// grp.Select(x => x.FinishType).FirstOrDefault(),
                                             //Qty = grp.Sum(p => p.Qty),
                                             //NoRoll = grp.Count(),
                                             MUName = key.MUName,
                                             ProcessTypeName = key.ProcessTypeName,
                                             //Shrinkage = key.Shrinkage,
                                             //Weight = key.Weight
                                         }).ToList();


                foreach (var oItem in oFDCDetails)
                {
                    sTemp = "";
                    if (!String.IsNullOrEmpty(oItem.FabricNo))
                    {
                        sTemp = sTemp + "Article: " + oItem.FabricNo; //+"\n" + oItem.ProductName;
                    }
                    if (!String.IsNullOrEmpty(oItem.FabricWeave))
                    {
                        sTemp = sTemp + "Comp: " + oItem.ProductName + ", Weave: " + oItem.FabricWeave;
                    }
                    else
                    { sTemp = sTemp + "Comp:" + oItem.ProductName; }
                  
                    if (!string.IsNullOrEmpty(oItem.Construction))
                    {
                        sTemp += " \nConst: " + oItem.Construction;
                    }
                    if (!string.IsNullOrEmpty(oItem.FinishWidth))
                    {
                        sTemp += " \nWidth : " + oItem.FinishWidth;
                    }
                    //if (!string.IsNullOrEmpty(oItem.Weight))
                    //{
                    //    sTemp += " Weight : " + oItem.Weight;
                    //}
                    if (!string.IsNullOrEmpty(oItem.ProcessTypeName))
                    {
                        sTemp += "\nProcess: " + oItem.ProcessTypeName;
                    }
                    if (!string.IsNullOrEmpty(oItem.FinishTypeName))
                    {
                        sTemp += ", Finish: " + oItem.FinishTypeName;
                    }
                    //if (!string.IsNullOrEmpty(oItem.Shrinkage))
                    //{
                    //    sTemp += "\nShrinkage: " + oItem.Shrinkage;
                    //}

                    int nRowSpan = oItem.Color_Grp.Count();

                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, (++nCount).ToString(), nRowSpan, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, sTemp, nRowSpan, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.Color_List.FirstOrDefault(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.NoRoll.ToString(), nRowSpan, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(oItem.Qty), nRowSpan, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, oFontStyle);

                    //oItem.Color_Grp.RemoveAt(0);
                    foreach (var oColor in oItem.Color_Grp)
                    {
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oColor.ExeNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oColor.StyleNo + " , " + oColor.BuyerRef, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oColor.ColorInfo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oColor.Color_List.FirstOrDefault(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oColor.NoRoll.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(oColor.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, oFontStyle);
                        nNoRoll = nNoRoll + oColor.NoRoll;
                    }
                    
                    _oPdfPTable.CompleteRow();
                }

                //for (int i = oFDCDetails.Count + 1; i <= (14 - oFDCDetails.Count); i++)
                //{
                //    int fHeight = 14;
                //    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, fHeight, oFontStyle);
                //    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, fHeight, oFontStyle);
                //    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, fHeight, oFontStyle);
                //    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, fHeight, oFontStyle);
                //    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, fHeight, oFontStyle);
                //    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, fHeight, oFontStyle);
                //    _oPdfPTable.CompleteRow();
                //}

                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Total ", 0, _nColumns - 2, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(nNoRoll), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(_oFDC.FDCDetails.Sum(x => x.Qty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
         
                oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                if(!string.IsNullOrEmpty(_oFDC.Note))
                {
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable,"Remarks : "+ _oFDC.Note, 0, _nColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 25, oFontStyle, true);
                }

                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Received the fabrics in good condition", 0, _nColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 10, oFontStyle, true);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _nColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle, true);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Drivers Name: " + _oFDC.DriverName, 0, _nColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle, true);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Vehicle No: " + _oFDC.VehicleNo, 0, _nColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle, true);

                if (_oApprovalHeads != null && _oApprovalHeads.Any())
                {
                    nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                    this.PrintFooter(700 - nUsagesHeight);
                }
                else
                    this.PrintFooter_Default();

                _oDocument.Add(_oPdfPTable);
                _oPdfPTable.DeleteBodyRows();
                _oDocument.NewPage();
                foreach (var oItem in oresults)
                {
                    PdfPTable oPdfPTable = Table_Packing();
                    var oFDCDs = _oFDC.FDCDetails.Where(x => x.ExeNo == oItem.ExeNo).ToList();
                    int nData = oFDCDs.Count();
                    int nRows = (nData / 3) + ((nData % 3 == 0) ? 0 : 3 - (nData % 3));
                    nCount = 0;
                    for (int i = 0; i < nRows; i += 40)
                    {
                        _oDocument.NewPage();

                        this.PrintHeader("Packing List",false);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Customer: " + _oFDC.DeliveryToName, 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Construction: ", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.Construction, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Date: ", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, _oFDC.IssueDateSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                        _oPdfPTable.CompleteRow();

                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Dispo No: " + oItem.ExeNo+"  Finish Type: " + oItem.FinishTypeName, 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Color: " + oItem.ColorInfo, 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Color: " + oItem.ColorInfo + " Shade:" + oItem.ShadeStr, 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                        //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.ShadeStr, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "F. Width:", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.FinishWidth, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, oFontStyle);
                        _oPdfPTable.CompleteRow();

                        SetValueToParentTable(ref nCount, 40, nData, oFDCDs);



                        _oDocument.Add(_oPdfPTable);
                        _oPdfPTable.DeleteBodyRows();
                    }
                }
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "G.Total:" + Global.MillionFormat(_oFDC.FDCDetails.Sum(x => x.Qty)) + " " +_sMUnit + ", Total Roll:" + _oFDC.FDCDetails.Count(), 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle, true);
                //PrintFooter_Two();

                nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

                if (nUsagesHeight < 0)
                {
                    nUsagesHeight = 15;
                }
                if (nUsagesHeight > 0 && nUsagesHeight < 50)
                {
                    nUsagesHeight = 20;
                }
                if (nUsagesHeight >= 50 && nUsagesHeight > 100)
                {
                    nUsagesHeight = 30;
                }
                if (nUsagesHeight >= 100 && nUsagesHeight > 300)
                {
                    nUsagesHeight = 40;
                }

                _oPdfPTable.CompleteRow();
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "__________", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, nUsagesHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "___________", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, nUsagesHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "__________", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, nUsagesHeight, oFontStyle);
                _oPdfPTable.CompleteRow();
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Received By", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Prepared By", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Approved By", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
                _oPdfPTable.CompleteRow();

            }
            else
            {
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                _oPdfPTable.CompleteRow();
            }


            #endregion
            #endregion
        }
        #endregion

        #region FOOTER
        private void PrintFooter(float nHight) 
        {
            if (nHight <= 0) { nHight = 15; }

            string[] signatureList = new string[_oApprovalHeads.Count];
            string[] dataList = new string[_oApprovalHeads.Count];

            for (int i = 1; i <= _oApprovalHeads.Count; i++)
            {
                signatureList[i-1] = (_oApprovalHeads[i - 1].Name);
                dataList[i-1] = (_oApprovalHistorys.Where(x => x.ApprovalHeadID == _oApprovalHeads[i - 1].ApprovalHeadID).Select(x => x.SendToPersonName).FirstOrDefault());
            }

            #region Authorized Signature
            _oPdfPCell = new PdfPCell(this.GetSignature(595f, dataList, signatureList, 10f)); _oPdfPCell.Border = 0;
            _oPdfPCell.MinimumHeight = nHight; _oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        //private void PrintFooterTwo()
        //{
        //    string[] signatureList = new string[_oApprovalHeads.Count];
        //    string[] dataList = new string[_oApprovalHeads.Count];

        //    //signatureList[0] = "Prepared By";
        //    //dataList[0] = "";

        //    for (int i = 1; i <= _oApprovalHeads.Count; i++)
        //    {
        //        signatureList[i - 1] = (_oApprovalHeads[i - 1].Name);
        //        dataList[i - 1] = (_oApprovalHistorys.Where(x => x.ApprovalHeadID == _oApprovalHeads[i - 1].ApprovalHeadID).Select(x => x.SendToPersonName).FirstOrDefault());
        //    }

        //    #region Authorized Signature
        //    _oPdfPCell = new PdfPCell(this.GetSignature(535f, dataList, signatureList, 30f)); _oPdfPCell.Border = 0;
        //    _oPdfPCell.FixedHeight = 20; _oPdfPCell.Colspan = 9;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();
        //    #endregion
        //}
        private PdfPTable GetSignature(float nTableWidth, string[] oData, string[] oSignatureSetups, float nBlankRowHeight)
        {
            iTextSharp.text.Font _oFontStyle;
            PdfPCell _oPdfPCell;
            int nSignatureCount = oSignatureSetups.Length;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            if (nSignatureCount <= 0)
            {
                #region Blank Table
                PdfPTable oPdfPTable = new PdfPTable(1);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { nTableWidth });

                if (nBlankRowHeight <= 0)
                {
                    nBlankRowHeight = 10f;
                }
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankRowHeight; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                return oPdfPTable;
                #endregion
            }
            else
            {

                PdfPTable oPdfPTable = new PdfPTable(nSignatureCount);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                #region
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", 0, 0, BaseColor.WHITE, 0, 0, 9, 0);

                //PdfPTable oPdfPTable_Remarks = new PdfPTable(2);
                //PdfPCell oPdfPCell = new PdfPCell();
                //oPdfPTable_Remarks.SetWidths(new float[] { 298, 298 });

                //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD, BaseColor.BLACK); oPdfPCell.MinimumHeight = 40f;
                //oPdfPCell = new PdfPCell(new Phrase("Goods delivered as per terms & condition of PI & LC :", _oFontStyle)); oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                //oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPTable_Remarks.AddCell(oPdfPCell);

                //oPdfPCell = new PdfPCell(new Phrase("Goods received in good condition", _oFontStyle)); oPdfPCell.MinimumHeight = 40f; oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                //oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPTable_Remarks.AddCell(oPdfPCell);

                //ESimSolPdfHelper.AddTable(ref oPdfPTable, oPdfPTable_Remarks, 0, 0, 9);
                #endregion

                int nColumnCount = -1;
                float[] columnArray = new float[nSignatureCount];
                foreach (string oItem in oSignatureSetups)
                {
                    nColumnCount++;
                    columnArray[nColumnCount] = nTableWidth / nSignatureCount;
                }
                oPdfPTable.SetWidths(columnArray);

                #region Blank Row
                if (nBlankRowHeight > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Colspan = nSignatureCount; _oPdfPCell.FixedHeight = nBlankRowHeight; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                #endregion

                nColumnCount = 0;
                for (int i = 0; i < oSignatureSetups.Length; i++)
                {
                    string sHeadName = "";
                    foreach (ApprovalHead oItem in _oApprovalHeads)
                    {
                        if (oSignatureSetups[i] == oItem.Name)
                        {
                            sHeadName = GetDBInfo(_oFDC, oItem.RefColName);
                        }
                    }
                    if (nSignatureCount == 1)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(sHeadName + "\n_________________\n" + oSignatureSetups[i], _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else if (nSignatureCount == 2)
                    {
                        if (nColumnCount == 0)
                        {

                            _oPdfPCell = new PdfPCell(new Phrase(sHeadName + "\n_________________\n" + oSignatureSetups[i], _oFontStyle));
                            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(sHeadName + "\n_________________\n" + oSignatureSetups[i] + " ", _oFontStyle));
                            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(sHeadName + "\n_________________\n" + oSignatureSetups[i], _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    nColumnCount++;
                }
                return oPdfPTable;
            }
        }
        public static string GetDBInfo(object obj, string propName)
        {
            try
            {
                return Convert.ToString(obj.GetType().GetProperty(propName).GetValue(obj, null));
            }
            catch
            {
                return "";
            }
        }
        private void PrintFooter_Default()
        {

            nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            if (nUsagesHeight < 740)
            {
                nUsagesHeight = 740 - nUsagesHeight;
            }

          
            if (nUsagesHeight > 2)
            {
                #region Blank Row
                iTextSharp.text.Font oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                while (nUsagesHeight < 740)
                {
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 9, Element.ALIGN_LEFT, BaseColor.WHITE, false, 20, oFontStyle);
                    nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                }
                #endregion
            }
            _oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "__________", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "___________", 0, 1, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "__________", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "__________", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "___________", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, _oFontStyle);
            _oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Prepared By", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Received By", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Checked By", 0, 1, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Store in-Charge", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Authorised By", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            _oPdfPTable.CompleteRow();
            
        }
        private void PrintFooter_Two()
        {

            nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            if (nUsagesHeight < 540)
            {
                nUsagesHeight =540 - nUsagesHeight;
            }
            iTextSharp.text.Font oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            if (nUsagesHeight > 20)
            {
                #region Blank Row
              
                while (nUsagesHeight < 540)
                {
                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 10, Element.ALIGN_LEFT, BaseColor.WHITE, false, 20, oFontStyle);
                    nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                }
                #endregion
            }
            _oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, _oFDC.PreparedByName, 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, _oFDC.ApproveByName, 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, oFontStyle);
            _oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "__________", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "___________", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "__________", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 15, oFontStyle);
            _oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Received By", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Prepared By", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Approved By", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
            _oPdfPTable.CompleteRow();

        }

        #endregion
        #region
        public byte[] PrepareReport_PackingList(FabricDeliveryChallan oFDC, Company oCompany, bool bIsGatePass, BusinessUnit oBusinessUnit, List<ApprovalHead> oApprovalHeads, List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetailsAdj)
        {

            _oFDC = oFDC;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _bIsGatePass = bIsGatePass;
            _oApprovalHeads = oApprovalHeads;
            _oFabricDeliveryChallanDetailsAdj = oFabricDeliveryChallanDetailsAdj;
            if (oFDC.FDCDetails.Count > 0)
            {
                _sMUnit = oFDC.FDCDetails[0].MUName;
            }
          
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(PageSize.A4);
            _oDocument.SetMargins(20f, 20f, 10f, 30f);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //ESimSolFooter PageEventHandler = new ESimSolFooter();
            //PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            //PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //PageEventHandler.signatures=new List<string>(new string[]{"Received By","Checked By", "Prepared By","Store in-Charge","Authorised By."});
            //PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler      --last hide
            if (_oFDC.DisburseBy == 0)
            {
                str = "Yet Not Disburse";
            }
            if (_oFDC.ApproveBy == 0)
            {
                str = "UNAUTHORIZED";
            }


            #endregion
            if (oFDC.IsSample)
            {
                this.PrintHeader();
                // this.PrintHeader(((_bIsGatePass) ? "Gate Pass" : "Sample Delivery Challan") + (_oFDC.ApproveBy != 0 ? "" : ""), false);
                this.PrintHeaderTwo(("Packing List") + (_oFDC.ApproveBy != 0 ? "" : " "), false);
              //  this.PrintHead_Sample();
                ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                ESimSolWM_Footer.WMFontSize = 75;
                ESimSolWM_Footer.WMRotation = 45;
                ESimSolWM_Footer PageEventHandlerWM = new ESimSolWM_Footer();
                PageEventHandlerWM.WaterMark = str; //Footer print with page event handler
                PDFWriter.PageEvent = PageEventHandlerWM; //Footer print with page event handler
            }
            else
            {
                this.PrintHeader();
               // this.PrintHeaderNormaL("Packing List", false);
                this.PrintHeaderTwo(("Packing List") + (_oFDC.ApproveBy != 0 ? "" : " "), false);
              //  this.PrintHead_Bulk();
                ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                ESimSolWM_Footer.WMFontSize = 75;
                ESimSolWM_Footer.WMRotation = 45;
                ESimSolWM_Footer PageEventHandlerWM = new ESimSolWM_Footer();
                PageEventHandlerWM.WaterMark = str; //Footer print with page event handler
                PDFWriter.PageEvent = PageEventHandlerWM; //Footer print with page event handler

            }
            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.SetWidths(new float[] { 20f, 60f, 70f, 40f, 80f, 70f, 45f, 45f, 60f }); //height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            this.PrintBody_PackingList();
            _oPdfPTable.HeaderRows = 0;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #endregion

        #region PACKING LIST
        private static PdfPTable Table_Packing()
        {
            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                   25f, //"SL", 
                                                   50f, //"Roll No"
                                                   65f, //"Fab. Qty"
                                                   30f, //"Roll No"
                                             });
            return oPdfPTable;

        }
        private string GetGoodsDescription(FabricDeliveryChallanDetail oItem)
        {
            string sTemp = "";
            //#region Process Type Name
            sTemp = "";
          
            //if (!String.IsNullOrEmpty(oItem.FabricNo))
            //{
            //    sTemp = sTemp +  oItem.FabricNo; //+"\n" + oItem.ProductName;
            //}
            if (!String.IsNullOrEmpty(oItem.FabricWeave))
            {
                sTemp = sTemp +   oItem.ProductName + ", Weave: " + oItem.FabricWeave;
            }
            else
            { sTemp = sTemp +  oItem.ProductName; }

            if (!string.IsNullOrEmpty(oItem.Construction))
            {
                sTemp += " \nConst: " + oItem.Construction;
            }
            if (!string.IsNullOrEmpty(oItem.FinishWidth))
            {
                sTemp += " \nWidth : " + oItem.FinishWidth;
            }
            //if (!string.IsNullOrEmpty(oItem.Weight))
            //{
            //    sTemp += " Weight : " + oItem.Weight;
            //}
            if (!string.IsNullOrEmpty(oItem.ProcessTypeName))
            {
                sTemp += "\nProcess: " + oItem.ProcessTypeName;
            }
            if (!string.IsNullOrEmpty(oItem.FinishTypeName))
            {
                sTemp += ", Finish: " + oItem.FinishTypeName;
            }
            return sTemp;
        }
        private void MakeTableHeader(ref PdfPTable oPdfPTable)
        {
            Font oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

            string[] tableHeader = new string[] { "SL", "Roll No", "Fab. Qty", "Shade" };
            foreach (string head in tableHeader)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, head, 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, oFontStyleBold);
            }
            oPdfPTable.CompleteRow();
        }
        private void SetValueToParentTable(ref int nStartIndex, int nIteration, int nData, List<FabricDeliveryChallanDetail> oFDCDs)
        {
            PdfPTable oPdfPTableTemp = new PdfPTable(3);
            oPdfPTableTemp.SetWidths(new float[] { 195f, 195f, 195f });
            Font oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            PdfPTable oPdfPTable = Table_Packing();
            int i = 0;

            _nQty = 0;
            _nCount = 0;

            #region Left
            MakeTableHeader(ref oPdfPTable);
            for (i = nStartIndex; i < nIteration + nStartIndex; i++)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i + 1).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFDCDs[i].RollNo : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFDCDs[i].StyleNo : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? Global.MillionFormat(oFDCDs[i].Qty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFDCDs[i].ShadeStr : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                if ((i < nData))
                {
                    _nQty = _nQty + oFDCDs[i].Qty;
                    _nCount = _nCount + 1;
                }
            }
            ESimSolItexSharp.PushTableInCell(ref oPdfPTableTemp, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, false);
            #endregion

            #region Middle
            nStartIndex = i;
            oPdfPTable = Table_Packing();
            MakeTableHeader(ref oPdfPTable);
            for (i = nStartIndex; i < nIteration + nStartIndex; i++)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i + 1).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFDCDs[i].RollNo : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFDCDs[i].StyleNo : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? Global.MillionFormat(oFDCDs[i].Qty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFDCDs[i].ShadeStr : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                if ((i < nData))
                {
                    _nQty = _nQty + oFDCDs[i].Qty;
                    _nCount = _nCount + 1;
                }
            }
            ESimSolItexSharp.PushTableInCell(ref oPdfPTableTemp, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, false);
            #endregion

            #region Right
            nStartIndex = i;
            oPdfPTable = Table_Packing();
            MakeTableHeader(ref oPdfPTable);
            for (i = nStartIndex; i < nIteration + nStartIndex; i++)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i + 1).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFDCDs[i].RollNo : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFDCDs[i].StyleNo : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? Global.MillionFormat(oFDCDs[i].Qty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFDCDs[i].ShadeStr : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                if ((i < nData))
                {
                    _nQty = _nQty + oFDCDs[i].Qty;
                    _nCount = _nCount + 1;
                }
            }
            ESimSolItexSharp.PushTableInCell(ref oPdfPTableTemp, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, false);
            nStartIndex = i;
            #endregion
            oPdfPTableTemp.CompleteRow();

            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "N.B: All rolls are inspected in four point system", 0, 7, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total: " + Global.MillionFormat(_nQty) + " Y   ||   Rolls: " + _nCount.ToString(), 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, _oFontStyleBold);
            //oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTableTemp, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, false);

            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Total: ", 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(_nQty) + "  " + _sMUnit + ", Total Roll:" + _nCount.ToString(), 0, 5, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            _oPdfPTable.CompleteRow();

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
