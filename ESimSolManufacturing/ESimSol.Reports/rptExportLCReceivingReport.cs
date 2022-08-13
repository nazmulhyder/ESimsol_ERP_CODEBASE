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
    public class rptExportLCReceiving
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
        ExportLC _oExportLC = new ExportLC();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<ExportPILCMapping> _oExportPILCMappings = new List<ExportPILCMapping>();
        List<ExportPIDetail> _oExportPIDetails = new List<ExportPIDetail>();
        List<ExportBill> _oExportBills = new List<ExportBill>();
        List<ExportBillDetail> _oExportBillDetails = new List<ExportBillDetail>();
        List<FDCRegister> _oFDCRegisters = new List<FDCRegister>();
        List<DyeingOrderReport> _oDyeingOrderReports = new List<DyeingOrderReport>();
        Company _oCompany = new Company();
        #endregion

        public byte[] PrepareReport(ExportLC oExportLC, List<ExportPIDetail> oExportPIDetails, Company oCompany, BusinessUnit oBusinessUnit, List<ExportBill> oExportBills, List<ExportBillDetail> oExportBillDetails, List<FDCRegister> oFDCRegisters, List<DyeingOrderReport> oDyeingOrderReports, List<ExportLC> oExportLCLogs)
        {
            _oExportLC = oExportLC;
            _oExportPILCMappings = oExportLC.ExportPILCMappings.Where(x=>x.Activity==true).OrderBy(x=>x.ExportPIID).ThenBy(x=>x.AmendmentSt).ToList();
            _oBusinessUnit = oBusinessUnit;
            _oExportPIDetails = oExportPIDetails;
            _oExportBillDetails = oExportBillDetails;
            _oExportBills = oExportBills;
            _oDyeingOrderReports = oDyeingOrderReports;
            _oFDCRegisters = oFDCRegisters;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(20f, 20f, 20f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            this.ReporttHeader();
            this.PrintLCAmemendInfo(oExportLCLogs);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;

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

            _oPdfPCell = new PdfPCell(new Phrase("Print Date: " + DateTime.Now.ToShortDateString(), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        #endregion

        #region Report Header
        private void ReporttHeader()
        {
            string sTemp = "";
            #region LC Heading Print
            _oPdfPCell = new PdfPCell(new Phrase("EXPORT L/C STATEMENT ", FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.FixedHeight = 30; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region LC Info
            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 90f, 254f, 90f, 160f });
            float _FontSize = 8.5f;
            #region 1st Row LC No
            _oFontStyle = FontFactory.GetFont("Tahoma",_FontSize, 1);
            _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase(": "+_oExportLC.ExportLCNo, _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma",_FontSize, 0);
            _oPdfPCell = new PdfPCell(new Phrase("L/C  Date", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportLC.OpeningDateST, _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion
            #region 2nd Row Factory
            _oFontStyle = FontFactory.GetFont("Tahoma",_FontSize, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Applicant Name", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": "+_oExportLC.ApplicantName, _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma",_FontSize, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Received Date", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma",_FontSize, 0);
            _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportLC.LCRecivedDateST, _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion
            #region 2nd Row Factory
            _oFontStyle = FontFactory.GetFont("Tahoma", _FontSize, 1);
            _oPdfPCell = new PdfPCell(new Phrase("End Buyer" , _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            if (_oExportPILCMappings.Count > 0)
            {
                _oExportLC.DeliveryToName = _oExportPILCMappings[0].BuyerName;
            }
            _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportLC.DeliveryToName, _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", _FontSize, 0);
            _oPdfPCell = new PdfPCell(new Phrase("MKT Concern", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", _FontSize, 0);
            if (_oExportPILCMappings.Count > 0)
            {
                sTemp = _oExportPILCMappings[0].MKTPName;
            }
            _oPdfPCell = new PdfPCell(new Phrase(": " + sTemp, _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion

          
            #region 2nd Row Factory
            _oFontStyle = FontFactory.GetFont("Tahoma", _FontSize, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Negotiate Bank", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportLC.BankName_Nego, _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            #endregion

            #region 2nd Row Factory
            _oFontStyle = FontFactory.GetFont("Tahoma", _FontSize, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Issue Bank", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportLC.BankName_Issue + "[" + _oExportLC.BBranchName_Issue+"]", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

         
            #endregion


            //#region 3rd Row Contact
            //_oFontStyle = FontFactory.GetFont("Tahoma",_FontSize, 0);
            //_oPdfPCell = new PdfPCell(new Phrase("Contact Person", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(": " + _oExportLC.ContactPersonnelName, _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
           
            //_oPdfPCell = new PdfPCell(new Phrase(" Contact Person", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(": " + _oExportLC.ContactPersonnelName, _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //#endregion
            #region 4th Row Tenor
            _oFontStyle = FontFactory.GetFont("Tahoma",_FontSize, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Tenor ", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportLC.LCTermsName +( ((int)_oExportLC.PaymentInstruction>0)? " From the Date of " + _oExportLC.PaymentInstruction.ToString():""), _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma",_FontSize, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Overdue Interest", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma",_FontSize, 0);
            _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportLC.OverDueRate.ToString("0.00"), _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion
            #region 5th Row Overdue
            _oFontStyle = FontFactory.GetFont("Tahoma",_FontSize, 0);
            _oPdfPCell = new PdfPCell(new Phrase("B Bank FDD", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((": " + (_oExportLC.BBankFDD ? "Yes" : "No")), _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shipment Date ", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportLC.ShipmentDateST, _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion
            #region 6th Row Shipment
            _oFontStyle = FontFactory.GetFont("Tahoma",_FontSize, 0);
            _oPdfPCell = new PdfPCell(new Phrase("D. Charge", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportLC.DCharge.ToString("0.00"), _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Expiry Date", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportLC.ExpiryDateST, _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion
           
            #region 8th Row LC Qty
            _oFontStyle = FontFactory.GetFont("Tahoma",_FontSize, 0);
            _oPdfPCell = new PdfPCell(new Phrase("L/C Value", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportLC.AmountSt, _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/C Qty", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            
            _oPdfPCell = new PdfPCell(new Phrase(": " + Global.MillionFormat_Round(_oExportLC.ExportPILCMappings.Sum(x => x.Qty)) + " " + _oExportLC.ExportPILCMappings.Select(x => x.MUName).FirstOrDefault(), _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
           

            PdfPTable oPdfPTable = new PdfPTable(8);
            oPdfPTable.SetWidths(new float[] { 30f, 155f, 30f, 40f, 30f, 50f, 40f, 40f });

            iTextSharp.text.Font oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC);
            PdfPCell oPdfPCell = new PdfPCell(new Phrase("P/I Information", oFontStyle));
            oPdfPCell.Colspan = 8;
            //oPdfPCell.BorderWidth = 0;
            oPdfPCell.BackgroundColor = BaseColor.GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            #region L/C Details
            int nExportPIID = -9999;
            foreach (ExportPILCMapping oItem in _oExportPILCMappings) 
            {
                if (nExportPIID != oItem.ExportPIID) 
                {
                     oPdfPTable = new PdfPTable(8);
                    oPdfPTable.SetWidths(new float[] { 30f, 155f, 30f, 40f,30f, 50f, 40f, 40f});
                    
                    
                    this.PrintHead_LC(ref oPdfPTable, _oExportPILCMappings.Where(x => x.ExportPIID == oItem.ExportPIID).FirstOrDefault());
                    this.PrintDetail(ref oPdfPTable, _oExportPILCMappings.Where(x => x.ExportPIID == oItem.ExportPIID).ToList());

                    _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
            }
            #endregion

            if (_oExportBills.Count > 0) 
            {
                #region Commercial Invoice
                PdfPTable oPdfPTable_CI = new PdfPTable(7);
                oPdfPTable_CI.SetWidths(new float[] { 70f, 60f, 50f, 50f, 70f, 60f, 60f });

                #region Header
                iTextSharp.text.Font oFontStyleTemp = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC);
                oPdfPCell = new PdfPCell(new Phrase("Commercial Invoices", oFontStyleTemp));
                oPdfPCell.Colspan = 7;
                //oPdfPCell.BorderWidth = 0;
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable_CI.AddCell(oPdfPCell);
                oPdfPTable_CI.CompleteRow();

                oPdfPCell = new PdfPCell(new Phrase("Bill No", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable_CI.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Invoice Date", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable_CI.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable_CI.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Status", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable_CI.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("LDBC No", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable_CI.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Maturity Date", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable_CI.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Payment Date", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable_CI.AddCell(oPdfPCell);
                oPdfPTable_CI.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(oPdfPTable_CI); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #region Data
                foreach (var oItem in _oExportBills)
                {
                    oPdfPTable_CI = new PdfPTable(7);
                    oPdfPTable_CI.SetWidths(new float[] { 70f, 60f, 50f, 50f, 70f, 60f, 60f });

                    #region PrintDetail
                    oPdfPCell = new PdfPCell(new Phrase(oItem.ExportBillNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable_CI.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.StartDateSt, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable_CI.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable_CI.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.StateSt, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable_CI.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable_CI.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.MaturityDateSt, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable_CI.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.RelizationDateSt , _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable_CI.AddCell(oPdfPCell);

                    oPdfPTable_CI.CompleteRow();
                    #endregion

                    _oPdfPCell = new PdfPCell(oPdfPTable_CI); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                #endregion

                oPdfPTable_CI = new PdfPTable(7);
                oPdfPTable_CI.SetWidths(new float[] { 70f, 60f, 50f, 50f, 70f, 60f, 60f });

                #region Total
                oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
                oPdfPCell.Colspan = 2; oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable_CI.AddCell(oPdfPCell);

                double val = _oExportBills.Sum(x => x.Amount);
                oPdfPCell = new PdfPCell(new Phrase(_oExportBills.Select(x=>x.Currency).FirstOrDefault() + Global.MillionFormat(val), _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable_CI.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.Colspan = 4;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable_CI.AddCell(oPdfPCell);

                oPdfPTable_CI.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(oPdfPTable_CI); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
        }
        #endregion

        private void PrintHead_LC(ref PdfPTable oPdfPTable, ExportPILCMapping oExportPILCMapping)
        {
            string sRowHeader = "";
            sRowHeader += (string.IsNullOrEmpty(oExportPILCMapping.PINo) ? "" : "PI No: " + oExportPILCMapping.PINo_Full);
            sRowHeader += (string.IsNullOrEmpty(oExportPILCMapping.IssueDateST) ? "" : "  PI Date: " + oExportPILCMapping.IssueDateST);
            if (oExportPILCMapping.VersionNo > 0)
            {
                sRowHeader += (string.IsNullOrEmpty(oExportPILCMapping.IssueDateST) ? "" : " A- " + oExportPILCMapping.VersionNo + "" + oExportPILCMapping.AmendmentSt);

            }
            sRowHeader += (string.IsNullOrEmpty(oExportPILCMapping.DateST) ? "" : "  Received Date: " + oExportPILCMapping.DateST);

            PdfPCell oPdfPCell = new PdfPCell(new Phrase(sRowHeader, _oFontStyleBold));
            oPdfPCell.MinimumHeight = 20; oPdfPCell.Colspan = 8;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthBottom = 0; 
            oPdfPCell.BorderWidth = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            sRowHeader = "";
            sRowHeader += (string.IsNullOrEmpty(oExportPILCMapping.CPerson) ? "" : "Concern Person: " + oExportPILCMapping.CPerson);
            sRowHeader += (string.IsNullOrEmpty(oExportPILCMapping.CPerson) ? "" : " Contract#: " + oExportPILCMapping.CPPhone);
            sRowHeader += (string.IsNullOrEmpty(oExportPILCMapping.MKTPName) ? "" : " "+_oBusinessUnit.ShortName + " Concern: " + oExportPILCMapping.MKTPName);

            oPdfPCell = new PdfPCell(new Phrase(sRowHeader, _oFontStyle));
            oPdfPCell.MinimumHeight = 10; oPdfPCell.Colspan = 8;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthBottom = 0; 
            oPdfPCell.BorderWidth = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
        }
        private void PrintDetail(ref PdfPTable oPdfPTable, List<ExportPILCMapping> oExportPILCMapping)
        {
            int nCount=0;
            int nCountPIDetail = 0;
            double nval = 0;
            double nTBillQty = 0;
            string sTemp = "";
            if (oExportPILCMapping.Count > 0)
            {
                #region Header
                PdfPCell oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("U/P", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Del. Qty", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Inv. Qty", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                #region Data
                foreach (ExportPIDetail oItem in _oExportPIDetails.Where(x => x.ExportPIID == oExportPILCMapping.Select(p => p.ExportPIID).FirstOrDefault()).ToList())
                {
                    #region PrintDetail
                    nCountPIDetail = _oExportPIDetails.Where(x => x.ExportPIID == oItem.ExportPIID).ToList().Count;
                    nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    if (_oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Finishing || _oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Weaving)
                    {
                        sTemp = "";
                        if (!string.IsNullOrEmpty(oItem.ProductName))
                        {
                            sTemp =  oItem.ProductName;
                        }
                        if (!string.IsNullOrEmpty(oItem.Construction))
                        {
                            sTemp =sTemp+" Const: "+ oItem.Construction;
                        }

                        if (!string.IsNullOrEmpty(oItem.StyleNo))
                        {
                            sTemp = sTemp + "\nStyle : " + oItem.StyleNo;
                            if (!string.IsNullOrEmpty(oItem.ColorInfo))
                            {
                                sTemp = sTemp + ",";
                            }
                        }
                        if (!string.IsNullOrEmpty(oItem.ColorInfo))
                        {
                            sTemp = sTemp + " Color : " + oItem.ColorInfo;
                        }
                    }

                    oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase( oItem.MUName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty) , _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oExportPILCMapping.Select(x => x.Currency).FirstOrDefault() + " " + Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oExportPILCMapping.Select(x => x.Currency).FirstOrDefault() + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                    if (nCount == 1)
                    {
                        if (_oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Finishing || _oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Weaving)
                        {
                            nval = _oFDCRegisters.Where(x => x.ExportPIID == oExportPILCMapping.Select(p => p.ExportPIID).FirstOrDefault()).Sum(x => x.Qty);
                        }
                        else if (_oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Dyeing)
                        {
                            nval = _oDyeingOrderReports.Where(x => x.ExportPIID == oExportPILCMapping.Select(p => p.ExportPIID).FirstOrDefault()).Sum(x => (x.Qty_DC-x.Qty_RC));
                        }
                        
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nval) , _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.Rowspan = nCountPIDetail;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(oPdfPCell);
                    }

                    nval = _oExportBillDetails.Where(x => x.ExportPIDetailID == oItem.ExportPIDetailID).Sum(x => x.Qty);
                    nTBillQty = nTBillQty + nval;
                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nval) , _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPTable.CompleteRow();
                    #endregion
                }
                #endregion

                #region Total
                oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
                oPdfPCell.Colspan = 3; 
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);

                double val = _oExportPIDetails.Where(x => x.ExportPIID == oExportPILCMapping.Select(p => p.ExportPIID).FirstOrDefault()).Sum(x => x.Qty);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(val), _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE; 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE; 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);

                val = _oExportPIDetails.Where(x => x.ExportPIID == oExportPILCMapping.Select(p => p.ExportPIID).FirstOrDefault()).Sum(x => x.Amount);
                oPdfPCell = new PdfPCell(new Phrase(oExportPILCMapping.Select(x => x.Currency).FirstOrDefault() + " " + Global.MillionFormat(val), _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE; 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);

                val = _oFDCRegisters.Where(x => x.ExportPIID == oExportPILCMapping.Select(p => p.ExportPIID).FirstOrDefault()).Sum(x => x.Qty);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(val), _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);

             
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTBillQty), _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion
            }
        }
        private void PrintLCAmemendInfo(List<ExportLC> oExportLCLogs)
        {
            int nCount = 0;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            
            PdfPTable oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 30f, 60f, 50f, 60f, 60f});


            //var oExportPILCMappings = _oExportPILCMappings.GroupBy(x => new { x.VersionNo }, (key, grp) =>
            //                             new ExportPILCMapping
            //                             {
            //                                 VersionNo = key.VersionNo,
            //                                 Date = grp.FirstOrDefault().Date,
            //                                 LCReceiveDate = grp.FirstOrDefault().LCReceiveDate,
            //                                 Amount = grp.Sum(p => p.Amount),
            //                             }).ToList();

            if (oExportLCLogs.Count > 0)
            {
                #region Header
                PdfPCell oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amendment No.", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amendment Date", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Received Date", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                #region Data


                oExportLCLogs = oExportLCLogs.OrderBy(x => x.VersionNo).ToList();

                foreach (var oItem in oExportLCLogs)
                {
                    #region PrintDetail
                    nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(((EnumNumericOrder)oItem.VersionNo).ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(oItem.Currency+""+ Global.MillionFormat(oItem.Amount), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.AmendmentDateSt, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.LCRecivedDateST, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                  

                    oPdfPTable.CompleteRow();
                    #endregion
                }
                #endregion

                //#region Total
                //oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
                //oPdfPCell.Colspan = 2;
                //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(oPdfPCell);

                //double val = oExportPILCMappings.Sum(x => x.Amount);
                //oPdfPCell = new PdfPCell(new Phrase(_oExportLC.Currency+""+ Global.MillionFormat(val), _oFontStyleBold));
                //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(oPdfPCell);

                //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(oPdfPCell);


                //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(oPdfPCell);

                //oPdfPTable.CompleteRow();
                //#endregion
            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        private void Blank(int nFixedHeight)
        {
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 1; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nFixedHeight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
    }
}
