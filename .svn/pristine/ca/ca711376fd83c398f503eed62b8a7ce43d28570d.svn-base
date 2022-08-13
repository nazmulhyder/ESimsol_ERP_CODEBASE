using System;
using System.Data;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.BusinessObjects;
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
    public class rptServiceOrderList
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(14);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<ServiceOrder> _oServiceOrderList = new List<ServiceOrder>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit;
        int _nColspan = 14;
        #endregion

        #region Order Sheet
        public byte[] PrepareReport(List<ServiceOrder> oServiceOrderList, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName)
        {
            _oServiceOrderList = oServiceOrderList;
            _oBusinessUnit = oBusinessUnit;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //842*595
            _oDocument.SetMargins(30f, 30f, 45f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    40f,  //1 
                                                    52f,  //2
                                                    45f, //3
                                                    52f,  //4
                                                    52f,  //5                                                
                                                    52f,  //6                                               
                                                    52f,  //7
                                                    100f,  //8
                                                    72f,  //9
                                                    72f,  //10
                                                    45f,  //11
                                                    45f,  //12
                                                    45f,  //13
                                                    52f,  //14
                                             });
            #endregion

            this.PrintHeader(sHeaderName);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        public byte[] PrepareReport_Service(List<ServiceOrder> oServiceOrderList, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName)
        {
            _oServiceOrderList = oServiceOrderList;
            _oBusinessUnit = oBusinessUnit;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //842*595
            _oDocument.SetMargins(10f, 10f, 10f, 10f);

            _oPdfPTable = new PdfPTable(8);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    25f,  //1 SL 
                                                    71f,  //2 Order
                                                    100f, //3 Reg/Vin
                                                               
                                                    100f,  //4 Customer
                                                    226f,  //5 v      
                                                    226f,  //6 v

                                                    45f,  //7 d
                                                    45f,  //8 d
                                             });
            #endregion

            this.PrintHeader(sHeaderName);
            this.PrintBody_Service();
            _oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        public byte[] PrepareReport_Schedules(List<ServiceOrder> oServiceOrderList, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName)
        {
            _oServiceOrderList = oServiceOrderList;
            _oBusinessUnit = oBusinessUnit;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //842*595
            _oDocument.SetMargins(30f, 30f, 45f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //1 SLNo
                                                    44f,  //2 OrderBo
                                                    95f, //3 Customer
                                                    52f,  //4 Phone 
                                                    50f,  //5 RcvDt                                               
                                                    55f,  //6 RegNo                                         
                                                    69f,  //7 VIN
                                                    57f,  //8 EngNo
                                                    52f,  //9 VechileType
                                                    60f,  //10 Kilometer
                                                    50f,  //11 DelDt
                                                    55f,  //12 CP
                                                    52f,  //13 Advisor
                                                    45f,  //14 Type
                                             });
            #endregion

            this.PrintHeader(sHeaderName);
            this.PrintBody_Schedules();
            _oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Report Header
        private void PrintHeader(string sHeaderName)
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
                _oImag.ScaleAbsolute(60f, 25f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));//_oBusinessUnit.PringReportHead
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColspan; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.Colspan = _nColspan; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            #region HEADER
            this.AddCell("#SL", "CENTER", true);
            this.AddCell("Order No", "CENTER", true);
            this.AddCell("Order Date", "CENTER", true);
            this.AddCell("Reg. No", "CENTER", true);
            this.AddCell("Model No", "CENTER", true);
            this.AddCell("Chassis/VIN", "CENTER", true);
            this.AddCell("Engine No", "CENTER", true);
            this.AddCell("Customer Name", "CENTER", true);
            this.AddCell("Contact Person", "CENTER", true);
            this.AddCell("Advisor", "CENTER", true);
            this.AddCell("Issue Date", "CENTER", true);
            this.AddCell("Rcv Date", "CENTER", true);
            this.AddCell("Del Date", "CENTER", true);
            this.AddCell("Service Type", "CENTER", true);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Data
            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (ServiceOrder oItem in _oServiceOrderList)
            {
                this.AddCell((++nCount).ToString(), "RIGHT", false);
                this.AddCell(oItem.ServiceOrderNo, "LEFT", false);
                this.AddCell(oItem.ServiceOrderDateSt, "CENTER", false);
                this.AddCell(oItem.VehicleRegNo, "LEFT", false);
                this.AddCell(oItem.VehicleModelNo, "LEFT", false);
                this.AddCell(oItem.ChassisNo, "LEFT", false);
                this.AddCell(oItem.EngineNo, "LEFT", false);
                this.AddCell(oItem.CustomerName, "LEFT", false);
                this.AddCell(oItem.ContactPerson, "LEFT", false);
                this.AddCell(oItem.AdvisorName, "LEFT", false);
                this.AddCell(oItem.IssueDateSt, "CENTER", false);
                this.AddCell(oItem.RcvDateTimeSt, "CENTER", false);
                this.AddCell(oItem.DelDateTimeSt, "CENTER", false);
                this.AddCell(oItem.ServiceOrderTypeSt, "LEFT", false); 
                _oPdfPTable.CompleteRow();
            }
            #endregion
        }
        private void PrintBody_Service()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            #region HEADER
            this.AddCell("#SL", "CENTER", true);
            this.AddCell("Order", "CENTER", true);
            this.AddCell("Vehicle", "CENTER", true);
            this.AddCell("Customer", "CENTER", true);
            this.AddCell("Customer Voice", "CENTER", true);
            this.AddCell("Technical Voice", "CENTER", true);
            this.AddCell("Rcv Date", "CENTER", true);
            this.AddCell("Del Date", "CENTER", true);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Data
            int nCount = 0;
            int nHeight = 30;

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (ServiceOrder oItem in _oServiceOrderList)
            {
                this.AddCell((++nCount).ToString(), "RIGHT", false, nHeight);
                this.AddCell(oItem.ServiceOrderNo + "\n" + oItem.ServiceOrderDateSt + "\n" + oItem.ServiceOrderTypeSt, "LEFT", false, nHeight);

                this.AddCell("Reg: " + oItem.VehicleRegNo + "\nVIN: " + oItem.ChassisNo + "\nEN : " + oItem.EngineNo, "LEFT", false, nHeight);
                this.AddCell(oItem.CustomerName+"\nCP: "+oItem.ContactPerson, "LEFT", false, nHeight);

                //this.AddCell(oItem.CustomerVoice.Replace("~", System.Environment.NewLine).ToString(), "LEFT", false, nHeight);
                //this.AddCell(oItem.TechincalVoice.Replace("~", System.Environment.NewLine).ToString(), "LEFT", false, nHeight);

                this.AddCell(this.ConertToNewline(oItem.CustomerVoice), "LEFT", false, nHeight);
                this.AddCell(this.ConertToNewline(oItem.TechincalVoice), "LEFT", false, nHeight);
                this.AddCell(oItem.RcvDateTimeSt, "CENTER", false, nHeight);
                this.AddCell(oItem.DelDateTimeSt, "CENTER", false, nHeight);
                _oPdfPTable.CompleteRow();
            }
            #endregion
        }

        private void PrintBody_Schedules()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            #region HEADER
            this.AddCell("#SL", "CENTER", true);
            this.AddCell("Order No", "CENTER", true);
            this.AddCell("Customer Name", "CENTER", true);
            this.AddCell("Customer Phone", "CENTER", true);
            this.AddCell("Rcv Date", "CENTER", true);
            this.AddCell("Reg. No", "CENTER", true);
            this.AddCell("VIN", "CENTER", true);
            this.AddCell("Engine No", "CENTER", true);
            this.AddCell("Vechile Type", "CENTER", true);
            this.AddCell("Kilometer Reading", "CENTER", true);
            this.AddCell("Del Date", "CENTER", true);
            this.AddCell("Contact Person", "CENTER", true);
            this.AddCell("Advisor", "CENTER", true);
            this.AddCell("Service Type", "CENTER", true);
           
            _oPdfPTable.CompleteRow();
            #endregion

            #region Data
            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (ServiceOrder oItem in _oServiceOrderList)
            {
                this.AddCell((++nCount).ToString(), "RIGHT", false);
                this.AddCell(oItem.ServiceOrderNo, "LEFT", false);
                this.AddCell(oItem.CustomerName, "LEFT", false);
                this.AddCell(oItem.CustomerPhone, "LEFT", false);
                this.AddCell(oItem.ActualRcvDateTimeSt, "CENTER", false);
                this.AddCell(oItem.VehicleRegNo, "LEFT", false);
                this.AddCell(oItem.ChassisNo, "LEFT", false);
                this.AddCell(oItem.EngineNo, "LEFT", false);
                this.AddCell(oItem.VehicleTypeName, "LEFT", false);
                this.AddCell(oItem.KilometerReading, "LEFT", false);
                this.AddCell(oItem.ActualDelDateTimeSt, "CENTER", false);
                this.AddCell(oItem.ContactPerson, "LEFT", false);
                this.AddCell(oItem.AdvisorName, "LEFT", false);
                this.AddCell(oItem.ServiceOrderTypeSt, "LEFT", false);

                _oPdfPTable.CompleteRow();
            }
            #endregion
        }

        #endregion

        #region PDF HELPER
        private string ConertToNewline(string sData) 
        {
            int nCount = 0; string result = "";

            if (string.IsNullOrEmpty(sData))
                return result;

            foreach(var oItem in sData.Split('~'))
            {
                result += (++nCount) + ". " + oItem+System.Environment.NewLine;
            }
            return result;
        } 
        public void AddCell(string sHeader, string sAlignment, int nColSpan, int nRowSpan)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default
            if (sAlignment == "LEFT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "RIGHT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment == "CENTER")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            if (nColSpan > 0)
                _oPdfPCell.Colspan = nColSpan;
            if (nRowSpan > 0)
                _oPdfPCell.Rowspan = nRowSpan;

            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddCell(string sHeader, string sAlignment, bool isGray, int mHight)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default

            if (sAlignment == "LEFT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "RIGHT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment == "CENTER")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            if (isGray)
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            if (mHight>0)
                _oPdfPCell.MinimumHeight = mHight;

            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddCell(string sHeader, string sAlignment, bool isGray)
        {
            this.AddCell(sHeader, sAlignment, isGray, 0);
        }
        public void PrintRow(string sHeader)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColspan;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion

        #endregion
    }
}