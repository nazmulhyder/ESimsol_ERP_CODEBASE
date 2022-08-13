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
    public class rptFabricAnalysis
    {
        #region Declaration

        int _nColumn = 2;

        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(2);
        PdfPCell _oPdfPCell;
        Company _oCompany;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Fabric _oFabric = new Fabric();
        FabricPOSetup _oFabricPOSetup = new FabricPOSetup();
        FabricRnD _oFabricRnD = new FabricRnD();
        FabricSCReport _oFabricSCReport = new FabricSCReport();
        #endregion

        public byte[] PrepareReport(Fabric oFabric,FabricPOSetup oFabricPOSetup , Company oCompany, BusinessUnit oBU)
        {
            _oFabric = oFabric;
            _oFabricPOSetup=oFabricPOSetup;
            #region Page Setup
          
            _oDocument = new Document(new iTextSharp.text.Rectangle(595f, 842), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 265f, 330f });
            #endregion
            _oCompany = oCompany;
            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oBU, oCompany, "Fabric Analysis",2);
            FabricSCReport oFabricSCReport = new FabricSCReport();
            this.PrintBody_Old();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body Old
        private void PrintBody_Old()
        {
            float LeftColumn = 85;

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Request From Marketing:  ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, 2, 0);

            #region Date
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetTotalWidth(new float[] { 415f, 80f });
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Request From Marketing:  ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Date:  " + _oFabric.IssueDateInString, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 2);
            #endregion



            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            #region Head
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.DefaultCell.Border = 0;
            oPdfPTable.SetTotalWidth(new float[] { LeftColumn, 365 - LeftColumn, 230 });
            Print_FabricInfo(ref oPdfPTable, "BUYER: ", _oFabric.BuyerName);
            Print_FabricInfo(ref oPdfPTable, _oFabricPOSetup.FabricCode + ": ", _oFabric.FabricNo);
            Print_FabricInfo(ref oPdfPTable, "STYLE/SEASON: ", _oFabric.StyleNo);
            Print_FabricInfo(ref oPdfPTable, "Buyer Ref: ", _oFabric.BuyerReference);
            Print_FabricInfo(ref oPdfPTable, "DEPARTMENT: ", "-");
            Print_FabricInfo(ref oPdfPTable, "END USE: ", _oFabric.EndUse);
            Print_FabricInfo(ref oPdfPTable, "Light Source (P): ", _oFabric.PrimaryLightSource);
            Print_FabricInfo(ref oPdfPTable, "Light Source (S): ", _oFabric.SecondaryLightSource);
            Print_FabricInfo(ref oPdfPTable, "Seeking Date: ", _oFabric.SeekingSubmissionDateInString);
            Print_FabricInfo(ref oPdfPTable, "Remarks: ", _oFabric.Remarks);

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 2);
            #endregion


            #region Signature

            List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = _oFabric.PrepareByName + "\nPrepared" });
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = _oFabric.MKTPersonName + " \nTeam Leader \n " });
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = _oFabric.ApprovedByNameSt + "\nApproved By\n " });

            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = 0;
            //_oPdfPCell.FixedHeight = 600 - ESimSolPdfHelper.CalculatePdfPTableHeight(_oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Authorized Signature
            _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(50f, _oFabric, _oSignatureSetups, 15.0f));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.FixedHeight = 80f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion


            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetTotalWidth(new float[] { 415f, 80f });
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Fabric Analysis Report For R & D : " + _oFabric.HandLoomNo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Date:  " + _oFabric.ReceiveDateSt, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 2);

            #region Type of Fabric
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetTotalWidth(new float[] { LeftColumn, 595 - LeftColumn });

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Type of Fabric", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Solid dyed / Print ", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 2);
            #endregion
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 8);

            #region YARN DETAILS
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetTotalWidth(new float[] { LeftColumn, 45, 400 - LeftColumn, 150 });

            #region Table Header
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "YARN DETAILS", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            Print_TableHeader(new string[] { "Count", "Yarn composition", " Yarn Type (assumed)" }, ref oPdfPTable);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            Print_YarnDetail(ref oPdfPTable, 1, "Warp yarn count", true);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 7, 8);
            Print_YarnDetail(ref oPdfPTable, 1, "Weft yarn count", false);
            #endregion

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, _nColumn);
            #endregion

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 8);

            #region FABRIC & FINISHING DETAILS
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetTotalWidth(new float[] { LeftColumn, 265 - LeftColumn });

            #region FABRIC
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLDITALIC);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "FABRIC DETAILS", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 10);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

            Print_TableData(new string[] {   "EPI:",_oFabric.EPI, 
                                             "PPI:",_oFabric.PPI,
                                             "Actual weight", Global.MillionFormat(_oFabric.WeightAct), 
                                             "Calculated weight",Global.MillionFormat(_oFabric.WeightCal),
                                             "Weave Type",_oFabric.FabricWeaveName,
                                             "Fabric Width",_oFabric.FabricWidth
                                          }, ref oPdfPTable);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "Requied no of frames (assumed)", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 8);
            #endregion

            #region FINISHIG
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "FINISH DETAILS", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 10);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            Print_TableData(new string[] {   "Finish type (Actual):",_oFabric.FinishTypeName, 
                                             "Suggested finish type:",_oFabric.FinishTypeNameSugg
                                         }, ref oPdfPTable);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "possible technical limitation / comments", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 10);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 2, 8);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 2, 8);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 2, 8);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 8);
            #endregion

            #region SUGGESTED
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "SUGGESTED DETAILS", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 10);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            Print_TableData(new string[] {   "Construction:",_oFabric.ActualConstruction, 
                                             "Calculated weight",Global.MillionFormat(_oFabric.WeightCal),
                                             "Declared weight",Global.MillionFormat(_oFabric.WeightDec)
                                         }, ref oPdfPTable);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "special note for fabric feasibility (if required)", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 10);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oFabric.NoteRnD, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 2, 8);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 2, 8);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 2, 8);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 8);
            #endregion

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_BOLD, 18f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.LIGHT_GRAY);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "SWATCH", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            #endregion

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 25);

            #region Signature

            _oSignatureSetups = new List<SignatureSetup>();
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = "" + "\nPrepared By \n" }); //_oFabric.PrepareByName+ "\n
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = "" + "\nChecked By\n" }); //_oFabric.MKTPersonName + " \n
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = "" + "\nApproved By (Head of Q.A)\n" }); //_oFabric.ApprovedByNameSt + "\n

            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 600 - ESimSolPdfHelper.CalculatePdfPTableHeight(_oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Authorized Signature
            _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(60f, _oFabric, _oSignatureSetups, 25.0f));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.FixedHeight = 80f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
        }
        #endregion

        public byte[] PrepareReport(Fabric oFabric, FabricSCReport oFabricSCReport,FabricRnD oFabricRnD, FabricPOSetup oFabricPOSetup, Company oCompany, BusinessUnit oBU)
        {
            _oFabric = oFabric;
            _oFabricPOSetup = oFabricPOSetup;
            _oFabricRnD = oFabricRnD;
            _oFabricSCReport = oFabricSCReport;
            #region Page Setup

            _oDocument = new Document(new iTextSharp.text.Rectangle(595f, 842), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 265f, 330f });
            #endregion
            _oCompany = oCompany;
            oCompany.Name = oBU.Name;
            oCompany.Address = oBU.Address;
            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oBU, oCompany, "Fabric Analysis", 2);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody()
        {
            float LeftColumn = 90;

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Request From Marketing:  ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, 2, 0);

            #region Date
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetTotalWidth(new float[] { 415f, 80f });
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.TOP_BORDER, 0, 2, 0);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Request From Marketing:  ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Date:  " + _oFabricSCReport.SCDateSt, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 2);
            #endregion



            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            #region Head
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.DefaultCell.Border = 0;

            //oPdfPTable.SetTotalWidth(new float[] { LeftColumn, 365 - LeftColumn, 230 });
            oPdfPTable.SetTotalWidth(new float[] { LeftColumn, 365 - 0, 0 });
            
            Print_FabricInfo(ref oPdfPTable, "BUYER: ", _oFabricSCReport.BuyerName);
            Print_FabricInfo(ref oPdfPTable, _oFabricPOSetup.FabricCode + ": ", _oFabricSCReport.FabricNo);
            Print_FabricInfo(ref oPdfPTable, "STYLE/SEASON: ", _oFabricSCReport.StyleNo);
            Print_FabricInfo(ref oPdfPTable, "Buyer Ref: ", _oFabricSCReport.BuyerReference);
            Print_FabricInfo(ref oPdfPTable, "DEPARTMENT: ", "-");
            Print_FabricInfo(ref oPdfPTable, "END USE: ", _oFabricSCReport.EndUse);
            Print_FabricInfo(ref oPdfPTable, "Light Source (P): ", _oFabricSCReport.LightSourceName);
            Print_FabricInfo(ref oPdfPTable, "Light Source (S): ", _oFabricSCReport.LightSourceNameTwo);
            Print_FabricInfo(ref oPdfPTable, "Seeking Date: ", _oFabric.SeekingSubmissionDateInString);
            Print_FabricInfo(ref oPdfPTable, "Remarks: ", _oFabric.Remarks);

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 2);
            #endregion


            #region Signature
            List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = _oFabricSCReport.PreapeByName + "\nPrepared" });
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = _oFabricSCReport.MKTGroup + " \nTeam Leader \n " });
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = _oFabricSCReport.ApproveByName + "\nApproved By\n " });

            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = 0;
            //_oPdfPCell.FixedHeight = 600 - ESimSolPdfHelper.CalculatePdfPTableHeight(_oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Authorized Signature
            _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(50f, _oFabric, _oSignatureSetups, 15.0f));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthRight = 1;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.FixedHeight = 60f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetTotalWidth(new float[] { 215f, 280f });
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, 2,10);
            if (!string.IsNullOrEmpty(_oFabricSCReport.ExeNo)) { _oFabric.Code = _oFabricSCReport.ExeNo; }
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Fabric Analysis Report : "+_oFabric.Code, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE,0);
            
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Lab Received Date:" + _oFabricRnD.ReceiveDateSt, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 2);

            #region Type of Fabric
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetTotalWidth(new float[] { LeftColumn, 595 - LeftColumn });

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLDITALIC);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Type of Fabric", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            //if (_oFabricSCReport.ProcessType>0)
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oFabricSCReport.ProcessTypeName, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 2);
            #endregion

            #region YARN DETAILS
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetTotalWidth(new float[] { LeftColumn, 45, 350 - LeftColumn, 200 });

            #region Table Header
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLDITALIC);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "YARN DETAILS", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            Print_TableHeader(new string[] { "Count", "Yarn composition", "" }, ref oPdfPTable);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            Print_YarnDetailRnD(ref oPdfPTable, 1, "Warp yarn count", true);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 7, 8);
            Print_YarnDetailRnD(ref oPdfPTable, 1, "Weft yarn count", false);
            #endregion

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, _nColumn);
            #endregion

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 8);

            #region FABRIC & FINISHING DETAILS
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetTotalWidth(new float[] { LeftColumn, 265 - LeftColumn });

            #region FABRIC
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma",9f, iTextSharp.text.Font.BOLDITALIC);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "FABRIC DETAILS", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 10);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            Print_TableData(new string[] {   "EPI:",_oFabricRnD.EPI, 
                                             "PPI:",_oFabricRnD.PPI,
                                             "Actual weight", Global.MillionFormat(_oFabricRnD.WeightAct), 
                                             "Calculated weight",Global.MillionFormat(_oFabricRnD.WeightCal),
                                             "Weave Type",_oFabricRnD.FabricWeaveName,
                                             "Fabric Width",_oFabricRnD.FabricWidth
                                          }, ref oPdfPTable);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "Requied no of frames (assumed)", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 8);
            #endregion

            #region FINISHIG
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma",9f, iTextSharp.text.Font.BOLDITALIC);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "FINISH DETAILS", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 10);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            Print_TableData(new string[] {   "Finish type:",_oFabricRnD.FinishTypeName, 
                                             //"Suggested finish type:",_oFabricRnD.FinishTypeName
                                         }, ref oPdfPTable);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "possible technical limitation / comments", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 10);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 2, 8);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 2, 8);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 2, 8);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 8);
            #endregion

            #region SUGGESTED
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLDITALIC);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "SUGGESTED DETAILS", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 10);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            Print_TableData(new string[] {   "Construction:",_oFabricRnD.ConstructionSuggest, 
                                             //"Calculated weight",Global.MillionFormat(_oFabricRnD.WeightCal),
                                             "Declared weight",Global.MillionFormat(_oFabricRnD.WeightDec)
                                         }, ref oPdfPTable);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Special note for fabric feasibility(if required)", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 10);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oFabricRnD.Note, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 2, 8);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 2, 8);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 2, 8);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 8);
            #endregion

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_BOLD, 18f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.LIGHT_GRAY);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "SWATCH", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            #endregion
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 15);

            #region Signature

            _oSignatureSetups = new List<SignatureSetup>();
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = "" + "\nPrepared By \n" }); //_oFabric.PrepareByName+ "\n
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = "" + "\nChecked By\n" }); //_oFabric.MKTPersonName + " \n
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = "" + "\nApproved By (Head of Q.A)\n" }); //_oFabric.ApprovedByNameSt + "\n

            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = 0;
           // _oPdfPCell.FixedHeight = 20;// 600 - ESimSolPdfHelper.CalculatePdfPTableHeight(_oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Authorized Signature
            _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(60f, _oFabric, _oSignatureSetups, 25.0f));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.FixedHeight = 80f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetTotalWidth(new float[] { 215f, 280f });
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Mkt Fabric Info.: " + _oFabric.Code, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Forward from Lab Date: " + ((_oFabricRnD.ForwardDate == DateTime.MinValue) ? "" : _oFabricRnD.ForwardDate.ToString("dd MMM yy")) + " " + " Received Date:  " + ((_oFabricRnD.ReceiveMKTDate == DateTime.MinValue) ? "" : _oFabricRnD.ReceiveMKTDate.ToString("dd MMM yy")), Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 2);

            #region FABRIC & FINISHING DETAILS
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetTotalWidth(new float[] { LeftColumn, 265 - LeftColumn });

            #region FABRIC
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLDITALIC);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "FABRIC DETAILS", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 10);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            Print_TableData(new string[] {   "Construction:",_oFabric.Construction, 
                                             "Construction(PI):",_oFabric.ConstructionPI,
                                             "Weight", Global.MillionFormat(_oFabric.WeightDec), 
                                             //"Calculated weight",Global.MillionFormat(_oFabricRnD.WeightCal),
                                             "Weave Type",_oFabric.FabricWeaveName,
                                             "Fabric Width",_oFabric.FabricWidth
                                          }, ref oPdfPTable);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "Requied no of frames (assumed)", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 8);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 2);
            #endregion
            #endregion
        }
        #endregion
       
        #region Helper
        public void Print_FabricInfo(ref PdfPTable oPdfPTable, string sHeader, string sData)
        {
            if (!string.IsNullOrEmpty(sData))
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, sHeader, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, sData, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            }
        }
        public void Print_YarnDetail(ref PdfPTable oPdfPTable, int Count, string sData, bool isWarp)
        {
            int nCount = 0;
            for (int i = 0; i < Count; i++)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, sData + "(" + (++nCount) + ")", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                if (isWarp)
                    Print_TableHeader(new string[] { _oFabric.WarpCount, _oFabric.ProductName, "" }, ref oPdfPTable);
                else
                    Print_TableHeader(new string[] { _oFabric.WeftCount, _oFabric.ProductNameWeft, "" }, ref oPdfPTable);
            }
        }
        public void Print_YarnDetailRnD(ref PdfPTable oPdfPTable, int Count, string sData, bool isWarp)
        {
            int nCount = 0;
            for (int i = 0; i < Count; i++)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, sData + "(" + (++nCount) + ")", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                if (isWarp)
                    Print_TableHeader(new string[] { _oFabricRnD.WarpCount, (string.IsNullOrEmpty(_oFabricRnD.ProductNameWarp)) ? "" : _oFabricRnD.ProductNameWarp, "Yarn Ply: " + _oFabricRnD.YarnFly }, ref oPdfPTable);
                else
                    Print_TableHeader(new string[] { _oFabricRnD.WeftCount, (string.IsNullOrEmpty(_oFabricRnD.ProductNameWeft)) ? "" : _oFabricRnD.ProductNameWeft, "Yarn Quality: " + _oFabricRnD.YarnQuality }, ref oPdfPTable);
            }
        } 

        public void Print_TableHeader(string[] Headers, ref PdfPTable oPdfPTable) 
        {
            foreach(string Header in Headers)
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Header, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
        }
        public void Print_TableData(string[] sData, ref PdfPTable oPdfPTable)
        {
            foreach (string Header in sData)
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Header, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
        }
        #endregion
    }
}