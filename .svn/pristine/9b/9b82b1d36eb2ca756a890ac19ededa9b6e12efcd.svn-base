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
    public class rptServiceInvoice
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(5);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        ServiceInvoice _oServiceInvoice = new ServiceInvoice();
        Company _oCompany = new Company();
        int _nColspan = 5;
        bool _bIsCustomerCopy;
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
        #endregion
        public byte[] PrepareReport(ServiceInvoice oServiceInvoice, Company oCompany, List<SignatureSetup> oSignatureSetups)
        {
            _oServiceInvoice = oServiceInvoice;
            _oCompany = oCompany;
            _oSignatureSetups = oSignatureSetups;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4); //595*842
            _oDocument.SetMargins(40f, 20f, 45f, 30f);
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
                                                    130f,  //1
                                                    110f, //2
                                                    75f,  //3
                                                    90f,  //4      
                                                    90f,  //5      
                                             });
            #endregion

            _bIsCustomerCopy = false;
            this.PrintHeader( "Service Invoice Form");
            this.PrintBody();

            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();

            _bIsCustomerCopy = true;
            this.PrintHeader("Service Invoice Form");
            this.PrintBody();

            _oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        public byte[] PrepareReportChallan(ServiceInvoice oServiceInvoice, Company oCompany)
        {
            _oServiceInvoice = oServiceInvoice;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4); //595*842
            _oDocument.SetMargins(40f, 20f, 45f, 30f);
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
                                                    130f,  //1
                                                    110f, //2
                                                    75f,  //3
                                                    90f,  //4      
                                                    90f,  //5      
                                             });
            #endregion

            this.PrintHeader("Challan Form");
            this.PrintBody_Challan();

            ESimSolPdfHelper.NewPageDeclaration(_oDocument,_oPdfPTable);
            this.PrintHeader("Gate Pass Form");
            this.PrintBody_Challan();

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(string sHeader)
        {
            #region 1st
            _oFontStyle = this.GetCustomFont("AudiType-ExtendedBold", 10);//FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);
            this.AddCell(ref _oPdfPTable,sHeader, "LEFT", 0,0,0);

            _oFontStyle = this.GetCustomFont("AudiType-ExtendedBold", 8);
            this.AddCell(ref _oPdfPTable, " ", "RIGHT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont_UNDERLINE("AudiType-ExtendedBold", 8);
            this.AddCell(ref _oPdfPTable, "", "LEFT", 2, 0, 0);

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
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region 2nd
            _oFontStyle = this.GetCustomFont("AudiType-ExtendedBold", 7);
            this.AddCell(ref _oPdfPTable, "Progress Motors Imports Limited", "LEFT", 2, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-ExtendedBold", 8);
            this.AddCell(ref _oPdfPTable,"", "RIGHT", 2, 0, 0);

            _oPdfPTable.CompleteRow();
            #endregion

            #region 3rd
            string sValue = "429/432, Tejgaon I/A \n" +
                            "Dhaka-1208 \n" +
                            "Bangladesh \n" +
                            "Phone    +880 2 8891243 \n" +
                            "Hotline  +880 19 CALL AUDI \n" +
                            "www.audi.com.bd ";
            _oFontStyle = _oFontStyle = this.GetCustomFont("AudiType-Normal", 6);
            this.AddCell(ref _oPdfPTable, sValue, "LEFT", 2, 2, 0);

            _oFontStyle = this.GetCustomFont("AudiType-ExtendedBold", 8);
            this.AddCell(ref _oPdfPTable, "", "RIGHT", 2, 0, 0);
           
            _oPdfPTable.CompleteRow();

            _oFontStyle = this.GetCustomFont("AudiType-ExtendedBold", 8);
            this.AddCell(ref _oPdfPTable, "", "RIGHT", 2, 0, 0);
            //this.AddCell(ref _oPdfPTable, "Audi Service", "RIGHT", 0, 0, 0);

            Phrase oPhrase = new Phrase();
            oPhrase.Add(new Chunk("Audi", ESimSolPdfHelper.GetCustomFont("AudiType-ExtendedBold", 8, BaseColor.BLACK)));
            oPhrase.Add(new Chunk("\n Service", ESimSolPdfHelper.GetCustomFont("AudiType-ExtendedBold", 8, BaseColor.RED)));
            _oPdfPCell = new PdfPCell(oPhrase); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        private void PrintBody()
        {
            this.PrintRow("");
            _oFontStyle = this.GetCustomFont("AudiType-Bold", 9);
            //this.PrintRow("Invoice No: "+_oServiceInvoice.ServiceInvoiceNo);
            if (_bIsCustomerCopy == true)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Invoice No: " + _oServiceInvoice.ServiceInvoiceNo, _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Invoice No: " + _oServiceInvoice.ServiceInvoiceNo + " ,  Requisition No : " + _oServiceInvoice.RequisitionNo, _oFontStyle));
            }
            
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 15f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 3;
            _oPdfPTable.AddCell(_oPdfPCell);

            if (_bIsCustomerCopy == true)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Customer Copy", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15f;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 2;
                _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Office Copy", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15f;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 2;
                _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPTable.CompleteRow();


            _oFontStyle = this.GetCustomFont("AudiType-Bold", 7);

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 182f, 200f, 195f});

            #region 1st

            this.AddCell(ref oPdfPTable, "Customer Name", "CENTER", true);
            this.AddCell(ref oPdfPTable, "Vehicle Identification Number", "CENTER", true);
            this.AddCell(ref oPdfPTable, "Vechile Model", "CENTER", true);
            oPdfPTable.CompleteRow();

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 7);
            this.AddCell(ref oPdfPTable, _oServiceInvoice.CustomerName, "CENTER", false);
            this.AddCell(ref oPdfPTable, _oServiceInvoice.ChassisNo, "CENTER", false);
            this.AddCell(ref oPdfPTable, _oServiceInvoice.VehicleModelNo, "CENTER", false);
            oPdfPTable.CompleteRow();
            #endregion

            #region 2nd

            _oFontStyle = this.GetCustomFont("AudiType-Bold", 7);
            this.AddCell(ref oPdfPTable,"Kilometer Reading", "CENTER", true);
            this.AddCell(ref oPdfPTable, "Vehicle Registration No", "CENTER", true);
            this.AddCell(ref oPdfPTable, "Service Order No", "CENTER", true);
            oPdfPTable.CompleteRow();

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 7);
            this.AddCell(ref oPdfPTable, _oServiceInvoice.KilometerReading, "CENTER", false);
            this.AddCell(ref oPdfPTable, _oServiceInvoice.VehicleRegNo, "CENTER", false);
            this.AddCell(ref oPdfPTable, _oServiceInvoice.ServiceOrderNo, "CENTER", false);
            oPdfPTable.CompleteRow();

            if (_bIsCustomerCopy)
            {
                _oFontStyle = this.GetCustomFont("AudiType-Bold", 7);
                _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                _oFontStyle = this.GetCustomFont("AudiType-Normal", 7);
                _oPdfPCell = new PdfPCell(new Phrase(_oServiceInvoice.CustomerRemarks, _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            else
            {
                _oFontStyle = this.GetCustomFont("AudiType-Bold", 7);
                _oPdfPCell = new PdfPCell(new Phrase("Customer Remarks", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                _oFontStyle = this.GetCustomFont("AudiType-Normal", 7);
                _oPdfPCell = new PdfPCell(new Phrase(_oServiceInvoice.CustomerRemarks, _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oFontStyle = this.GetCustomFont("AudiType-Bold", 7);
                _oPdfPCell = new PdfPCell(new Phrase("Office Remarks", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                _oFontStyle = this.GetCustomFont("AudiType-Normal", 7);
                _oPdfPCell = new PdfPCell(new Phrase(_oServiceInvoice.OfficeRemarks, _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion

            //Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Colspan = _nColspan;
            _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            this.PrintRow("");
           
            #region Details
            PdfPTable oSIDPdfPTable = new PdfPTable(9);
            oSIDPdfPTable.WidthPercentage = 100;
            oSIDPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oSIDPdfPTable.SetWidths(new float[] { 15f, 40f, 120f, 100f, 70f, 70f, 80f, 97f, 15f });

            #region Header
            _oFontStyle = this.GetCustomFont("AudiType-Bold", 7);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            this.AddCell(ref oSIDPdfPTable, "SL No", "CENTER", 0, 0, 1);
            if (_bIsCustomerCopy == false)
            {
                this.AddCell(ref oSIDPdfPTable, "Part Name", "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, "Part No", "CENTER", 0, 0, 1);
            }
            else
            {
                this.AddCell(ref oSIDPdfPTable, "Part Name", "CENTER", 2, 0, 1);
            }
            this.AddCell(ref oSIDPdfPTable, "Qty", "CENTER", 0, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "Charge Type", "CENTER", 0, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "Unit Price", "CENTER", 0, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "Amount", "CENTER", 0, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);

            oSIDPdfPTable.CompleteRow();
            #endregion

            #region Invoice Details
            _oFontStyle = this.GetCustomFont("AudiType-Normal", 7);
            string sChargeType = "";
            int nCount = 0;
            foreach (ServiceInvoiceDetail oItem in _oServiceInvoice.ServiceInvoiceDetails) 
            {
                nCount++;
                sChargeType = "";
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
                this.AddCell(ref oSIDPdfPTable, nCount.ToString(), "CENTER", 0, 0, 1);
                if (_bIsCustomerCopy == false)
                {
                    sChargeType = oItem.WorkChargeTypeSt;
                    this.AddCell(ref oSIDPdfPTable, oItem.PartsName, "CENTER", 0, 0, 1);
                    this.AddCell(ref oSIDPdfPTable, oItem.PartsNo, "CENTER", 0, 0, 1);
                }
                else
                {
                    if (oItem.WorkChargeType == EnumServiceILaborChargeType.Complementary || oItem.WorkChargeType == EnumServiceILaborChargeType.Warranty)
                    {
                        sChargeType = oItem.WorkChargeTypeSt;
                    }
                    this.AddCell(ref oSIDPdfPTable, oItem.PartsName, "CENTER", 2, 0, 1);
                }                
                this.AddCell(ref oSIDPdfPTable, oItem.Qty+oItem.MUName, "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, sChargeType, "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, Global.MillionFormat(oItem.UnitPrice), "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, Global.MillionFormat(oItem.Amount), "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);

                oSIDPdfPTable.CompleteRow();
            }
            #endregion

            _oFontStyle = this.GetCustomFont("AudiType-Bold", 7);

            #region Total
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 6, 0, 1);
            this.AddCell(ref oSIDPdfPTable, Global.MillionFormat(_oServiceInvoice.ServiceInvoiceDetails.Sum(x=>x.Amount)), "CENTER", 0, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            #endregion

            #region Labor Charge Details
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            this.AddCell(ref oSIDPdfPTable, "Labor Charge:", "LEFT", 7, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            oSIDPdfPTable.CompleteRow();

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 7);
            
            foreach (ServiceILaborChargeDetail oItem in _oServiceInvoice.ServiceILaborChargeDetails)
            {
                nCount++;
                sChargeType = "";
                if (_bIsCustomerCopy == false)
                {
                    sChargeType = oItem.LaborChargeTypeSt;                    
                }
                else
                {
                    if (oItem.LaborChargeType == EnumServiceILaborChargeType.Complementary || oItem.LaborChargeType == EnumServiceILaborChargeType.Warranty)
                    {
                        sChargeType = oItem.LaborChargeTypeSt;
                    }
                }
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
                this.AddCell(ref oSIDPdfPTable, nCount.ToString(), "CENTER", 0, 0, 1);                
                this.AddCell(ref oSIDPdfPTable, "    -" + oItem.ServiceName, "LEFT", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, "" + oItem.ChargeDescription, "LEFT", 2, 0, 1);
                this.AddCell(ref oSIDPdfPTable, sChargeType, "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, Global.MillionFormat(oItem.ChargeAmount), "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
                oSIDPdfPTable.CompleteRow();
            }
            #endregion


            _oFontStyle = this.GetCustomFont("AudiType-Bold", 7);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 0);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "Total Parts Price", "LEFT", 5, 0, 1);
            this.AddCell(ref oSIDPdfPTable, Global.MillionFormat(_oServiceInvoice.NetAmount_Parts), "CENTER", 0, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            oSIDPdfPTable.CompleteRow();

            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 0);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "Total Labor Charge", "LEFT", 5, 0, 1);
            this.AddCell(ref oSIDPdfPTable, Global.MillionFormat(_oServiceInvoice.LaborCharge_Total), "CENTER", 0, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);

            if (_oServiceInvoice.DiscountAmount_Parts > 0)
            {
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 0);
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 1);

                this.AddCell(ref oSIDPdfPTable, "Discount On Spare Parts (" + Global.MillionFormat(_oServiceInvoice.DiscountAmount_Parts * 100 / _oServiceInvoice.NetAmount_Parts) + "%)", "LEFT", 5, 0, 1);
       
                this.AddCell(ref oSIDPdfPTable, Global.MillionFormat(_oServiceInvoice.DiscountAmount_Parts), "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
                oSIDPdfPTable.CompleteRow();
            }

            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 0);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "VAT(" + _oServiceInvoice.PartsCharge_Vat + "%) On Parts Charge", "LEFT", 5, 0, 1);
            this.AddCell(ref oSIDPdfPTable, Global.MillionFormat((_oServiceInvoice.NetAmount_Parts - _oServiceInvoice.DiscountAmount_Parts) * (_oServiceInvoice.PartsCharge_Vat / 100)), "CENTER", 0, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            oSIDPdfPTable.CompleteRow();



            if (_oServiceInvoice.LaborCharge_Discount > 0)
            {
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 0);
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 1);
                if (_oServiceInvoice.LaborCharge_Total != 0)
                    this.AddCell(ref oSIDPdfPTable, "Special Discount On Labor Charge (" + Global.MillionFormat(_oServiceInvoice.LaborCharge_Discount * 100 / _oServiceInvoice.LaborCharge_Total) + "%)", "LEFT", 5, 0, 1);
                else
                    this.AddCell(ref oSIDPdfPTable, "Special Discount On Labor Charge ", "LEFT", 5, 0, 1);
                this.AddCell(ref oSIDPdfPTable, Global.MillionFormat(_oServiceInvoice.LaborCharge_Discount), "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
                oSIDPdfPTable.CompleteRow();
            }

            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 0);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "VAT(" + _oServiceInvoice.LaborCharge_Vat + "%) On Labor Charge", "LEFT", 5, 0, 1);
            this.AddCell(ref oSIDPdfPTable, Global.MillionFormat((_oServiceInvoice.LaborCharge_Total-_oServiceInvoice.LaborCharge_Discount)*(_oServiceInvoice.LaborCharge_Vat/100)), "CENTER", 0, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            oSIDPdfPTable.CompleteRow();

            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 0);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "Total Amount To Be Paid", "LEFT", 5, 0, 1);
            this.AddCell(ref oSIDPdfPTable, Global.MillionFormat(_oServiceInvoice.NetAmount_Payble), "CENTER", 0, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);

            oSIDPdfPTable.CompleteRow();
            //Insert Into Main Table
            _oPdfPCell = new PdfPCell(oSIDPdfPTable); _oPdfPCell.Colspan = 5;
            _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            #endregion

            this.PrintRow("");
            this.PrintRow("");

            while (this.CalculatePdfPTableHeight(_oPdfPTable) < 580f)
            {
                this.PrintRow("");
            }


            PrintRow("");
            _oFontStyle = this.GetCustomFont("AudiType-Bold", 7); 
            if(_oServiceInvoice.CurrencyID==1)
                PrintRow(" Amount in Words: "+Global.TakaWords(_oServiceInvoice.NetAmount_Payble));
            else
                PrintRow(" Amount in Words: "+Global.DollarWords(_oServiceInvoice.NetAmount_Payble));

            PrintRow(" Work Order By: "+_oServiceInvoice.WorkOrderByName);


            _oPdfPCell = new PdfPCell(new Phrase(" Signature: _____________________________________________________________________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 15f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColspan;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

              _oFontStyle = this.GetCustomFont("AudiType-Normal", 5);
               _oPdfPCell = new PdfPCell(new Phrase("                               I hereby acknowledge the satisfactory completion of the above described work", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 15f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColspan;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = this.GetCustomFont("AudiType-Bold", 7); 
             PrintRow(" Date: "+_oServiceInvoice.ServiceInvoiceDateSt);

             #region print Signature Captions
             if (_bIsCustomerCopy == false)
             {
                 _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = _nColspan;
                 _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                 _oPdfPTable.CompleteRow();

                 _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(525f, (object)_oServiceInvoice, _oSignatureSetups, 0.0f)); _oPdfPCell.Colspan = _nColspan;
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                 _oPdfPTable.CompleteRow();
             }
             
             #endregion

           }

        private void PrintBody_Challan()
        {
            this.PrintRow("");
            _oFontStyle = this.GetCustomFont("AudiType-Bold", 9);
            this.PrintRow("Challan No: " + _oServiceInvoice.ServiceInvoiceNo);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", 7);

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 182f, 200f, 195f });

            #region 1st

            this.AddCell(ref oPdfPTable, "Customer Name", "CENTER", true);
            this.AddCell(ref oPdfPTable, "Vehicle Identification Number", "CENTER", true);
            this.AddCell(ref oPdfPTable, "Vechile Model", "CENTER", true);
            oPdfPTable.CompleteRow();

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 7);
            this.AddCell(ref oPdfPTable, _oServiceInvoice.CustomerName, "CENTER", false);
            this.AddCell(ref oPdfPTable, _oServiceInvoice.ChassisNo, "CENTER", false);
            this.AddCell(ref oPdfPTable, _oServiceInvoice.VehicleModelNo, "CENTER", false);
            oPdfPTable.CompleteRow();
            #endregion

            #region 2nd

            _oFontStyle = this.GetCustomFont("AudiType-Bold", 7);
            this.AddCell(ref oPdfPTable, "Kilometer Reading", "CENTER", true);
            this.AddCell(ref oPdfPTable, "Vehicle Registration No", "CENTER", true);
            this.AddCell(ref oPdfPTable, "Repair Order No", "CENTER", true);
            oPdfPTable.CompleteRow();

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 7);
            this.AddCell(ref oPdfPTable, _oServiceInvoice.KilometerReading, "CENTER", false);
            this.AddCell(ref oPdfPTable, _oServiceInvoice.VehicleRegNo, "CENTER", false);
            this.AddCell(ref oPdfPTable, _oServiceInvoice.ServiceInvoiceNo, "CENTER", false);
            oPdfPTable.CompleteRow();
            #endregion

            //Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Colspan = _nColspan;
            _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            this.PrintRow("");

            #region Details
            PdfPTable oSIDPdfPTable = new PdfPTable(6);
            oSIDPdfPTable.WidthPercentage = 100;
            oSIDPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oSIDPdfPTable.SetWidths(new float[] { 15f, 40f, 120f, 100f, 70f,  15f });

            #region Header
            _oFontStyle = this.GetCustomFont("AudiType-Bold", 7);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            this.AddCell(ref oSIDPdfPTable, "SL No", "CENTER", 0, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "Part Name", "CENTER", 0, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "Part No", "CENTER", 0, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "Qty", "CENTER", 0, 0, 1);
 
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);

            oSIDPdfPTable.CompleteRow();
            #endregion

            #region Invoice Details
            _oFontStyle = this.GetCustomFont("AudiType-Normal", 7);
            int nCount = 0;
            foreach (ServiceInvoiceDetail oItem in _oServiceInvoice.ServiceInvoiceDetails)
            {
                nCount++;
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);

                this.AddCell(ref oSIDPdfPTable, nCount.ToString(), "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, oItem.PartsName, "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, oItem.PartsNo, "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, oItem.Qty + oItem.MUName, "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);

                oSIDPdfPTable.CompleteRow();
            }
            #endregion

            _oFontStyle = this.GetCustomFont("AudiType-Bold", 7);

            //#region Total
            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 5, 0, 1);
            //this.AddCell(ref oSIDPdfPTable, Global.MillionFormat(_oServiceInvoice.ServiceInvoiceDetails.Sum(x => x.Amount)), "CENTER", 0, 0, 1);
            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            //#endregion

            #region Labor Charge Details
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            this.AddCell(ref oSIDPdfPTable, "Labor Charge:", "LEFT", 4, 0, 1);
            this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            oSIDPdfPTable.CompleteRow();

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 7);
            foreach (ServiceILaborChargeDetail oItem in _oServiceInvoice.ServiceILaborChargeDetails)
            {
                nCount++;
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);

                this.AddCell(ref oSIDPdfPTable, nCount.ToString(), "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, "    -" + oItem.ServiceName, "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 1);
                this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);

                oSIDPdfPTable.CompleteRow();
            }
            #endregion


            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 0);
            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 6, 0, 1);
            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 0);
            //oSIDPdfPTable.CompleteRow();

            //_oFontStyle = this.GetCustomFont("AudiType-Bold", 7);
            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 0);
            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 1);
            //this.AddCell(ref oSIDPdfPTable, "Total Parts Price", "LEFT", 4, 0, 1);
            //this.AddCell(ref oSIDPdfPTable, Global.MillionFormat(_oServiceInvoice.NetAmount_Parts), "CENTER", 0, 0, 1);
            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            //oSIDPdfPTable.CompleteRow();

            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 0);
            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 1);
            //this.AddCell(ref oSIDPdfPTable, "Total Labor Charge", "LEFT", 4, 0, 1);
            //this.AddCell(ref oSIDPdfPTable, Global.MillionFormat(_oServiceInvoice.LaborCharge_Total), "CENTER", 0, 0, 1);
            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);

            //if (_oServiceInvoice.LaborCharge_Discount > 0)
            //{
            //    this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 0);
            //    this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 1);
            //    if (_oServiceInvoice.LaborCharge_Total != 0)
            //        this.AddCell(ref oSIDPdfPTable, "Special Discount On Labor Charge (" + Global.MillionFormat(_oServiceInvoice.LaborCharge_Discount * 100 / _oServiceInvoice.LaborCharge_Total) + "%)", "LEFT", 4, 0, 1);
            //    else
            //        this.AddCell(ref oSIDPdfPTable, "Special Discount On Labor Charge ", "LEFT", 4, 0, 1);
            //    this.AddCell(ref oSIDPdfPTable, Global.MillionFormat(_oServiceInvoice.LaborCharge_Discount), "CENTER", 0, 0, 1);
            //    this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            //    oSIDPdfPTable.CompleteRow();
            //}

            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 0);
            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 1);
            //this.AddCell(ref oSIDPdfPTable, "VAT(" + _oServiceInvoice.LaborCharge_Vat + "%)", "LEFT", 4, 0, 1);
            //this.AddCell(ref oSIDPdfPTable, Global.MillionFormat((_oServiceInvoice.LaborCharge_Total - _oServiceInvoice.LaborCharge_Discount) * (_oServiceInvoice.LaborCharge_Vat / 100)), "CENTER", 0, 0, 1);
            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);
            //oSIDPdfPTable.CompleteRow();

            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 0);
            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 1, 0, 1);
            //this.AddCell(ref oSIDPdfPTable, "Total Amount To Be Paid", "LEFT", 4, 0, 1);
            //this.AddCell(ref oSIDPdfPTable, Global.MillionFormat(_oServiceInvoice.NetAmount_Payble), "CENTER", 0, 0, 1);
            //this.AddCell(ref oSIDPdfPTable, "", "CENTER", 0, 0, 0);

            oSIDPdfPTable.CompleteRow();
            //Insert Into Main Table
            _oPdfPCell = new PdfPCell(oSIDPdfPTable); _oPdfPCell.Colspan = 5;
            _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            #endregion

            this.PrintRow("");
            this.PrintRow("");

            while (this.CalculatePdfPTableHeight(_oPdfPTable) < 580f)
            {
                this.PrintRow("");
            }


            //PrintRow("");
            //_oFontStyle = this.GetCustomFont("AudiType-Bold", 7);
            //if (_oServiceInvoice.CurrencyID == 1)
            //    PrintRow(" Amount in Words: " + Global.TakaWords(_oServiceInvoice.NetAmount_Payble));
            //else
            //    PrintRow(" Amount in Words: " + Global.DollarWords(_oServiceInvoice.NetAmount_Payble));

            PrintRow(" Work Order By: " + _oServiceInvoice.WorkOrderByName);


            _oPdfPCell = new PdfPCell(new Phrase(" Signature: _____________________________________________________________________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 15f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColspan;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 5);
            _oPdfPCell = new PdfPCell(new Phrase("                               I hereby acknowledge the satisfactory completion of the above described work", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 15f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColspan;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = this.GetCustomFont("AudiType-Bold", 7);
            PrintRow(" Date: " + _oServiceInvoice.ServiceInvoiceDateSt);
        }
   
        #region PDF HELPER
        public void AddCell(ref PdfPTable oPdfPTable, string sHeader, string sAlignment, int nColSpan, int nRowSpan, int nBORDER)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default
            if (sAlignment == "LEFT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "RIGHT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment == "CENTER")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            if (nColSpan > 0)
                _oPdfPCell.Colspan = nColSpan;
            if (nRowSpan > 0)
                _oPdfPCell.Rowspan = nRowSpan;
            if (nBORDER == 0)
                _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddCell(ref PdfPTable oPdfPTable, string sHeader, string sAlignment, bool isGray)
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

            oPdfPTable.AddCell(_oPdfPCell);
        }
        public void PrintRow(string sHeader)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 15f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColspan;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        #endregion

        public iTextSharp.text.Font GetCustomFont(string fontName, float fFontSize)
        {
            string fontpath = System.Web.Hosting.HostingEnvironment.MapPath("~/fonts/");
            BaseFont customfont = BaseFont.CreateFont(fontpath + fontName + ".ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font oFont = new iTextSharp.text.Font(customfont, fFontSize);
            return oFont;
        }
        public iTextSharp.text.Font GetCustomFont_UNDERLINE(string fontName, float fFontSize)
        {
            string fontpath = System.Web.Hosting.HostingEnvironment.MapPath("~/fonts/");
            BaseFont customfont = BaseFont.CreateFont(fontpath + fontName + ".ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font oFont = new iTextSharp.text.Font(customfont, fFontSize, iTextSharp.text.Font.UNDERLINE);
            return oFont;
        }

        public float CalculatePdfPTableHeight(PdfPTable table)
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