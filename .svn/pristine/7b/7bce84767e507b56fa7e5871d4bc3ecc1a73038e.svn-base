using System;
using System.Data;
using ESimSol.BusinessObjects;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ICS.Core;
using ICS.Core.Utility;
using System.Linq;

namespace ESimSol.Reports
{
    public class rptDUProGuideLine
    {
        public rptDUProGuideLine() 
        {
            LotParents = new List<LotParent>();
            DURequisitionDetails_SRM = new List<DURequisitionDetail>();
            DUSoftWindings = new List<DUSoftWinding>();
            DUProGuideLineDetails_Return = new List<DUProGuideLineDetail>();
        }

        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        iTextSharp.text.Font _oFontStyleTwo;
        iTextSharp.text.Font _oFontStyleForCC;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPTable _oPdfPTableTemp = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        DUProGuideLine _oDUProGuideLine = new DUProGuideLine();
        List<DUProGuideLineDetail> _oDUProGuideLineDetails = new List<DUProGuideLineDetail>();
        List<DUProGuideLineDetail> _oDUProGuideLineDetails_All = new List<DUProGuideLineDetail>();
        List<DyeingOrderDetail> _oDyeingOrderDetails = new List<DyeingOrderDetail>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        //DUProGuideLineSetup _oDUProGuideLineSetup = new DUProGuideLineSetup();

        List<LotParent> _oLotParents_In = new List<LotParent>();
        List<LotParent> _oLotParents_Out = new List<LotParent>();

        public List<LotParent> LotParents { set; get; }
        public List<DUSoftWinding> DUSoftWindings { set; get; }
        public List<DURequisitionDetail> DURequisitionDetails_SRM { set; get; }
        public List<DURequisitionDetail> DURequisitionDetails_SRS { set; get; }
        public List<DUProGuideLineDetail> DUProGuideLineDetails_Return { set; get; }

        double _nTotalQty = 0;
        string _sMUnitName = "";
        #endregion

        public byte[] PrepareReport(DUProGuideLine oDUProGuideLine, List<DUProGuideLineDetail> oDUProGuideLineDetails,List<DyeingOrderDetail> oDyeingOrderDetails, Company oCompany, BusinessUnit oBusinessUnit)
        {
            _oCompany = oCompany;
            _oDUProGuideLine = oDUProGuideLine;
            _oBusinessUnit = oBusinessUnit;
            _oDUProGuideLineDetails = oDUProGuideLine.DUProGuideLineDetails;
            _oDUProGuideLineDetails_All = oDUProGuideLineDetails;
            _oDyeingOrderDetails = oDyeingOrderDetails;

            _oLotParents_In = LotParents.Where(x=>x.DyeingOrderID == oDUProGuideLine.DyeingOrderID).ToList();
            _oLotParents_Out = LotParents.Where(x => x.DyeingOrderID_Out == oDUProGuideLine.DyeingOrderID).ToList();

            if (_oDUProGuideLineDetails.Count > 0)
            {
                _sMUnitName = _oDUProGuideLineDetails[0].MUnit;
            }

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(33f, 35f, 50f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 525f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 9;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();

            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 267.5f, 100f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Print Date: " + DateTime.Now.ToShortDateString(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Blank Space
            //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 267.5f, 100f });

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("  Goods " + (_oDUProGuideLine.InOutType == EnumInOutType.Disburse ? "Return" : "Receive") + " Notes  ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 1;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            if (_oDUProGuideLine.ApproveByID == 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.ITALIC, iTextSharp.text.BaseColor.RED);
                _oPdfPCell = new PdfPCell(new Phrase("Unauthorize Notes", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPTableTemp.AddCell(_oPdfPCell);
                _oPdfPTableTemp.CompleteRow();
            }
            #endregion
            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] {70f,140f, 70f, 90f, 80f, 90f });

            #region Report Vocher No, DUProGuideLine Date & Currency
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8.5f, 1);

            #region First
            _oPdfPCell = new PdfPCell(new Phrase("GRN No : ", _oFontStyle)); 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(_oDUProGuideLine.SLNo, _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oDUProGuideLine.InOutType == EnumInOutType.Disburse ? "Returned" : "Received") + " Date : ", _oFontStyle)); 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oDUProGuideLine.IssueDateST, _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Challan No: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oDUProGuideLine.ChallanNo, _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region second
            _oPdfPCell = new PdfPCell(new Phrase((_oDUProGuideLine.OrderType == EnumOrderType.LoanOrder ? "Supplier" : "Buyer") + " Name: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oDUProGuideLine.ContractorName, _oFontStyleBold)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Ref Type: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oDUProGuideLine.OrderTypeST, _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Third
            _oPdfPCell = new PdfPCell(new Phrase(_oDUProGuideLine.OrderTypeST + " No : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oDUProGuideLine.DyeingOrderNo, _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Vehicle No: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oDUProGuideLine.VehicleNo, _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oDUProGuideLine.OrderTypeST+" Date: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oDUProGuideLine.OrderDateSt, _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Fourth
            _oPdfPCell = new PdfPCell(new Phrase((_oDUProGuideLine.InOutType == EnumInOutType.Disburse ? "Returned" : "Received") + " Store: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oDUProGuideLine.ReceiveStore, _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Gate In No: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oDUProGuideLine.GateInNo, _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oDUProGuideLine.Note, _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region DUProGuideLine Details
            oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 455.5f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oFontStyleTwo = FontFactory.GetFont("Tahoma", 9f, 1);

            _oPdfPCell = new PdfPCell(new Phrase("Goods Details Description", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            oPdfPTable = new PdfPTable(7);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 30.5f, 75f, 245.2f, 70.5f, 60f, 37.5f, 77.5f });

            #region Header
            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Commodity", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Quantity", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();
            #endregion

            int nCount = 0;
            int nRawCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oFontStyleForCC = FontFactory.GetFont("Tahoma", 8f, 0);
            foreach (DUProGuideLineDetail oItem in _oDUProGuideLineDetails)
            {
                #region
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 30.5f, 75f, 245.2f, 70.5f, 60f, 37.5f, 77.5f });

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDUProGuideLine.DyeingOrderNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUnit, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _nTotalQty = _nTotalQty + oItem.Qty;
                #endregion

                #region For Page brack if raw count >45

                if (_oPdfPTable.Rows.Count >= 45)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(new Phrase("Continued ...", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();

                    _oPdfPCell = new PdfPCell(_oPdfPTableTemp);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();


                    nCount = 0;


                }
                #endregion Page brack
            }
            nRawCount = _oPdfPTable.Rows.Count;
            #region For Print blank raw if (30-raw count)>0
            for (int i = 0; i <= (30 - nRawCount); i++)
            {

                oPdfPTable = new PdfPTable(7);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 30.5f, 75f, 245.2f, 70.5f, 60f, 37.5f, 77.5f });

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            for (int i = 1; i <= 1; i++)
            {

                oPdfPTable = new PdfPTable(7);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 30.5f, 75f, 245.2f, 70.5f, 60f, 37.5f, 77.5f });

                nCount++;

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Total DUProGuideLine Amount
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 401.7f, 58f, 37.5f, 77.5f });
            //oPdfPTable.SetWidths(new float[] { 30.5f, 75f, 245.2f, 70.5f, 60f, 37.5f, 77.5f });


            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(" Total", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("   " + Global.MillionFormat(_nTotalQty) + " " + _sMUnitName, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            if (_oDUProGuideLine.DyeingOrderID != 0) 
            {

                #region Summary Print V1
                //oPdfPTable = new PdfPTable(8);
                //oPdfPTable.WidthPercentage = 100;
                //oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                //oPdfPTable.SetWidths(new float[] { 25.5f, 160.2f, 70.5f, 70.5f, 65f, 65f, 70f, 70.5f }); //130

                //#region Header
                //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                //_oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_oPdfPCell.BorderWidthLeft = _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("Commodity", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("GRN Qty", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("Transfer In", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("Transfer Out", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("Return Qty", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("Balance", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //#endregion

                //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                //int nProductID = nCount = 0;
                //foreach (var oItem in _oDyeingOrderDetails.OrderBy(x=>x.ProductID))
                //{
                //    if (oItem.ProductID != nProductID)
                //    {
                //        nCount++;
                //        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //        _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                //        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                //        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //        double nQty_Order = _oDyeingOrderDetails.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty);
                //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Order), _oFontStyle));
                //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                //        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //        //double nQty = _oDUProGuideLineDetails_All.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty);
                //        double nQty = _oLotParents_In.Where(x => x.ProductID == oItem.ProductID && x.DyeingOrderID_Out==0).Sum(x => x.Qty);
                //        double nQty_T_In = _oLotParents_In.Where(x => x.ProductID == oItem.ProductID && x.DyeingOrderID_Out != 0).Sum(x => x.Qty);
                //        double nQty_T_Out = _oLotParents_Out.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty);

                //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty), _oFontStyle));
                //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                //        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_In), _oFontStyle));
                //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                //        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_Out), _oFontStyle));
                //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                //        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //        double nQty_Return = _oDUProGuideLineDetails_All.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty_Return);
                //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Return), _oFontStyle));
                //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                //        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //        double nBalance = nQty_Order - nQty - nQty_T_In  + nQty_Return + nQty_T_Out;
                //        nBalance = nBalance < 0 ? 0 : nBalance;

                //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nBalance), _oFontStyle));
                //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                //        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //    }
                //    nProductID = oItem.ProductID;
                //}

                //#region
                //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //_oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //_oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //double _Order = _oDyeingOrderDetails.Sum(x => x.Qty);
                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_Order), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //_oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //double _nQty = _oDUProGuideLineDetails_All.Sum(x => x.Qty);
                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //_oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //double _nQty_Return = _oDUProGuideLineDetails_All.Sum(x => x.Qty_Return);
                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty_Return), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //_oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //double nGrand_Balance = _Order - _nQty + _nQty_Return;
                //nGrand_Balance = nGrand_Balance > 0 ? 0 : nGrand_Balance;

                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrand_Balance), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //_oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //#endregion

                //_oPdfPCell = new PdfPCell(oPdfPTable);
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();
                #endregion

                //Count, OQ, GQ, SRM-Q, T-IN,  

                #region Summary Print V2
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 25.5f, 125.5f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f }); //130

                #region Header
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

                #region 1st Row
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 4;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Received Status", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 4;
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Issued Status", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                #region 2nd Row
                _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Commodity", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Yarn Lot", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("GRN Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("SRM Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Transfer In", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //DUE, Y-Lot, I-S/W,T-OUT, RPQ, SB

                _oPdfPCell = new PdfPCell(new Phrase("Received Due", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Issued to S/W", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Transfer Out", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Return Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Balance", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #endregion


                var oProducts = LotParents.GroupBy(x => new { x.DyeingOrderID, x.ProductID, x.ProductName }, (key, grp) =>
                                 new
                                 {
                                     DyeingOrderID = key.DyeingOrderID,
                                     ProductID = key.ProductID,
                                     ProductName = key.ProductName,
                                 }).ToList();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                int nProductID = 0, nLotID = nCount = 0;
                double _nQTy_Total_DUE = 0;
                foreach (var oItem in oProducts.OrderBy(x => x.ProductID))
                {
                    if (oItem.ProductID != nProductID)
                    {
                        if (nCount > 1)
                        {
                            #region SUB TOTAL
                            _oPdfPCell = new PdfPCell(new Phrase("Sub Total", _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Colspan = 3;
                            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                            double nQty_Order = _oDyeingOrderDetails.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Order), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                            double nQty_GRN = _oDUProGuideLineDetails_All.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                            double nQty_SRM = DURequisitionDetails_SRM.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                            double nQty_T_In = _oLotParents_In.Where(x => x.ProductID == nProductID && x.DyeingOrderID_Out != 0).Sum(x => x.Qty);
                            double nQty_T_Out = _oLotParents_Out.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                            double nQty_SW = DURequisitionDetails_SRS.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                            double nQty_Return = DUProGuideLineDetails_Return.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_GRN), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SRM), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_In), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                            //DUE
                            double nBalance_DUE = nQty_Order - nQty_GRN - nQty_T_In;
                            nBalance_DUE = nBalance_DUE < 0 ? 0 : nBalance_DUE;

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nBalance_DUE), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                            //I-S/W
                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SW), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                            //T-OUT
                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_Out), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                            //RETURN
                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Return), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                            //BALANCE
                            double nBalance_Store_SubTotal = (nQty_GRN + nQty_SRM + nQty_T_In) - nQty_SW - nQty_T_Out - nQty_Return;
                            nBalance_Store_SubTotal = nBalance_Store_SubTotal < 0 ? 0 : nBalance_Store_SubTotal;

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nBalance_Store_SubTotal), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                            #endregion
                        }

                        #region Lot Wise Status
                        nCount = 0;

                        #region Group By Lot Wise
                        var dataGrpList = LotParents.Where(x => x.ProductID == oItem.ProductID).GroupBy(x => new { LotID = x.LotID }, (key, grp) => new LotParent
                        {
                            LotID = key.LotID,
                            LotNo = grp.First().LotNo,
                            ProductID = grp.First().ProductID,
                            ProductName = grp.First().ProductName,
                            DyeingOrderID = grp.First().DyeingOrderID,
                            DyeingOrderID_Out = grp.First().DyeingOrderID_Out,
                            Qty = grp.Sum(x => x.Qty)
                        });
                        #endregion

                        foreach (var oLotParent in dataGrpList.OrderBy(x => x.LotID)) 
                        {
                            if (oLotParent.LotID != nLotID)
                            {
                                nCount++;
                                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                //Y-Lot
                                _oPdfPCell = new PdfPCell(new Phrase(oLotParent.LotNo, _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                if (oItem.ProductID != nProductID)
                                {
                                    double nQty_Order = _oDyeingOrderDetails.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty);
                                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Order), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    _oPdfPCell.Rowspan = dataGrpList.Where(x => x.ProductID == oItem.ProductID).Count();
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                                }

                                double nBalance_Store = 0;
                                double nQty_GRN = _oDUProGuideLineDetails_All.Where(x => x.ProductID == oItem.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty);
                                double nQty_SRM = DURequisitionDetails_SRM.Where(x => x.ProductID == oItem.ProductID && x.DestinationLotID == oLotParent.LotID).Sum(x => x.Qty);
                                double nQty_T_In = _oLotParents_In.Where(x => x.ProductID == oItem.ProductID && x.DyeingOrderID_Out != 0 && x.LotID == oLotParent.LotID).Sum(x => x.Qty);
                                double nQty_T_Out = _oLotParents_Out.Where(x => x.ProductID == oItem.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty);
                                double nQty_SW = DURequisitionDetails_SRS.Where(x => x.ProductID == oItem.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty);
                                double nQty_Return = DUProGuideLineDetails_Return.Where(x => x.ProductID == oItem.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty);

                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_GRN), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SRM), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_In), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                #region BALANCE
                                //DUE
                                if (oItem.ProductID != nProductID)
                                {
                                    double nQty_Order_Product   = _oDyeingOrderDetails.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty);
                                    double nQty_Rcv = _oDUProGuideLineDetails_All.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty); //&& x.LotID == oLotParent.LotID
                                    double nQty_Rcv_In = _oLotParents_In.Where(x => x.ProductID == oItem.ProductID && x.DyeingOrderID_Out != 0).Sum(x => x.Qty); //&& x.LotID == oLotParent.LotID
                                    double nQty_Rcv_Out = _oLotParents_Out.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty); //&& x.LotID == oLotParent.LotID

                                    double nQty_Party_Return = DUProGuideLineDetails_Return.Where(x => x.ProductID == oItem.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty); //
                                    
                                    double nQty_RSRS            = DURequisitionDetails_SRS.Where(x => x.ProductID == oItem.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty);
                                    double nQty_RSRM            = DURequisitionDetails_SRM.Where(x => x.ProductID == oItem.ProductID && x.DestinationLotID == oLotParent.LotID).Sum(x => x.Qty);
                                  
                                    double nBalance_DUE = nQty_Order_Product - nQty_Rcv - nQty_Rcv_In;
                                    nBalance_DUE = nBalance_DUE < 0 ? 0 : nBalance_DUE;
                                    
                                    _nQTy_Total_DUE += nBalance_DUE;

                                    nBalance_Store = nQty_Rcv + nQty_RSRM + nQty_Rcv_In - nQty_RSRS - nQty_Rcv_Out - nQty_Party_Return;

                                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nBalance_DUE), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    _oPdfPCell.Rowspan = dataGrpList.Where(x => x.ProductID == oItem.ProductID).Count();
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                                }
                                #endregion

                                //I-S/W
                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SW), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                //T-OUT
                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_Out), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                //RETURN
                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Return), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                //BALANCE
                                var nLot_Balance_Store = nQty_GRN + nQty_SRM + nQty_T_In - nQty_SW - nQty_T_Out - nQty_Return;
                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nLot_Balance_Store), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                            }
                            nLotID = oLotParent.LotID;
                            nProductID = oItem.ProductID;
                        }
                        #endregion
                    }
                }

                if (nCount > 1)
                {
                    #region SUB TOTAL
                    _oPdfPCell = new PdfPCell(new Phrase("Sub Total", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Colspan = 3;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    double nQty_Order = _oDyeingOrderDetails.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Order), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    double nQty_GRN = _oDUProGuideLineDetails_All.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                    double nQty_SRM = DURequisitionDetails_SRM.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                    double nQty_T_In = _oLotParents_In.Where(x => x.ProductID == nProductID && x.DyeingOrderID_Out != 0).Sum(x => x.Qty);
                    double nQty_T_Out = _oLotParents_Out.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                    double nQty_SW = DURequisitionDetails_SRS.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                    double nQty_Return = DUProGuideLineDetails_Return.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_GRN), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SRM), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_In), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //DUE
                    double nBalance_DUE = nQty_Order - nQty_GRN - nQty_T_In;
                    nBalance_DUE = nBalance_DUE < 0 ? 0 : nBalance_DUE;

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nBalance_DUE), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //I-S/W
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SW), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //T-OUT
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_Out), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //RETURN
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Return), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //BALANCE
                    double nBalance_Store_SubTotal = (nQty_GRN + nQty_SRM + nQty_T_In) - nQty_SW - nQty_T_Out - nQty_Return;
                    nBalance_Store_SubTotal = nBalance_Store_SubTotal < 0 ? 0 : nBalance_Store_SubTotal;

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nBalance_Store_SubTotal), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    #endregion
                }

                #region Grand Total
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                
                //Count, OQ, GQ, SRM-Q, T-IN,        DUE, Y-Lot, I-S/W,T-OUT, RPQ, SB
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                double _Order = _oDyeingOrderDetails.Sum(x => x.Qty);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_Order), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                double _nQty_GRN = _oDUProGuideLineDetails_All.Sum(x => x.Qty);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty_GRN), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                double _nQty_SRM = DURequisitionDetails_SRM.Sum(x => x.Qty);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty_SRM), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                double _nQty_T_IN = _oLotParents_In.Where(x => x.DyeingOrderID_Out != 0).Sum(x => x.Qty);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty_T_IN), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQTy_Total_DUE), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                double _nQty_SRS = DURequisitionDetails_SRS.Sum(x => x.Qty);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty_SRS), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                double _nQty_T_OUT = _oLotParents_Out.Where(x => x.DyeingOrderID_Out != 0).Sum(x => x.Qty);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty_T_OUT), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                double _nQty_Return = DUProGuideLineDetails_Return.Sum(x => x.Qty);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty_Return), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                double nGrand_Balance = _nQty_GRN + _nQty_SRM + _nQty_T_IN - _nQty_T_OUT - _nQty_SRS - _nQty_Return;
                nGrand_Balance = nGrand_Balance <= 0 ? 0 : nGrand_Balance;

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrand_Balance), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                oPdfPTable.CompleteRow();
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region print Signature Captions
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 175+87.5f, 175+87.5f});


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            //_oPdfPCell = new PdfPCell(new Phrase("__________________ \n" + " Requisition By" + "\n" + _oDUProGuideLine.RequisitionByName, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("__________________  \n Approved By \n" + _oDUProGuideLine.ApprovedByName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("__________________  \n Issued By \n" + _oDUProGuideLine.IssuedByName, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("__________________ \n " + (_oDUProGuideLine.InOutType == EnumInOutType.Disburse ? "Returned" : "Received") + " By \n" + _oDUProGuideLine.ReceivedByName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();



            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //  oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            #endregion
        }
        #endregion
    }
}
