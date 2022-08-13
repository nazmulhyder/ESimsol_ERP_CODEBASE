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
    public class rptServiceOrder
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(6);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        ServiceOrder _oServiceOrder = new ServiceOrder();
        Company _oCompany = new Company();
        List _checkmark = new ZapfDingbatsList(52);
        int _nColspan = 6;
        #endregion
        public byte[] PrepareReport(ServiceOrder oServiceOrder, Company oCompany)
        {
            _oServiceOrder = oServiceOrder;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4); //595*842
            _oDocument.SetMargins(20f, 20f, 25f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    20f,  //1 
                                                    110f,  //2
                                                    110f, //3
                                                    75f,  //4
                                                    90f,  //5      
                                                    90f,  //6      
                                             });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 595 / 3, 595 / 3, 595 / 3});

            #region 1st
            _oFontStyle = this.GetCustomFont("AudiType-ExtendedBold", 8);
            this.AddCell(ref oPdfPTable, "", "LEFT", 0,0,0);

            _oFontStyle = this.GetCustomFont("AudiType-ExtendedBold", 8);
            this.AddCell(ref oPdfPTable, "", "CENTER", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-ExtendedBold", 6);
            this.AddCell(ref oPdfPTable, "Printed On: "+DateTime.Now.ToString("dd MMM yyyy"), "CENTER", 0,0,0);
            #endregion

            #region 2nd
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; 
            _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //string sValue = "429/432, Tejgaon I/A \n" +
            //                "Dhaka-1208 Bangladesh ";
            _oFontStyle = _oFontStyle = this.GetCustomFont("AudiType-ExtendedBold", 9);
            this.AddCell(ref oPdfPTable, _oCompany.Name, "CENTER", 0, 0, 0);

            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(70f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 35f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion

            #region 3rd

            _oFontStyle = _oFontStyle = this.GetCustomFont("AudiType-Bold", 8);
            this.AddCell(ref oPdfPTable, _oCompany.Address, "CENTER", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 8);
            this.AddCell(ref oPdfPTable, "", "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 8);
            this.AddCell(ref oPdfPTable, "      TEL :     " + _oCompany.Phone, "LEFT", 0, 0, 0);

            Phrase oPhrase = new Phrase();
            oPhrase.Add(new Chunk("Audi", ESimSolPdfHelper.GetCustomFont("AudiType-ExtendedBold", 8, BaseColor.BLACK)));
            oPhrase.Add(new Chunk(" Service", ESimSolPdfHelper.GetCustomFont("AudiType-ExtendedBold", 8, BaseColor.RED)));
            _oPdfPCell = new PdfPCell(oPhrase); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            
            #endregion

            #region 4th
            _oFontStyle = this.GetCustomFont("AudiType-Normal", 8);
            this.AddCell(ref oPdfPTable, "  T.I.N No. " + _oCompany.VatRegNo, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 8);
            this.AddCell(ref oPdfPTable, "      FAX :       " + _oCompany.Fax, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 8);
            this.AddCell(ref oPdfPTable, "CIN No. " + _oCompany.CompanyRegNo, "LEFT", 0, 0, 0);
            #endregion

            #region 5th
            _oFontStyle = this.GetCustomFont("AudiType-Normal", 8);
            this.AddCell(ref oPdfPTable, "", "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 8);
            this.AddCell(ref oPdfPTable, "      EMAIL :    " + _oCompany.Email, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 8);
            this.AddCell(ref oPdfPTable, "", "LEFT", 0, 0, 0);
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 6; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion

        private void PrintBody()
        {

            #region Body Header
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;
            float nWidth = 595 / 6;
            oPdfPTable.SetWidths(new float[] { nWidth - 5f, 5f, nWidth, nWidth - 5f, 5f, nWidth+5f, nWidth - 10f, 5f, nWidth });

            this.AddCell(ref oPdfPTable, "", "LEFT", 9, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", 8);
            this.AddCell(ref oPdfPTable, "Repair Order", "CENTER", 9, 0, 0);

            int nFontSize = 8;

            #region 1st
            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Repair Order No", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0);
            _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.ServiceOrderNoFull, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Registration No", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0);
            _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.VehicleRegNo, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Date In", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0);
            _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.RcvDateTime.ToString("dd MMM yyyy"), "LEFT", 0, 0, 0);
            oPdfPTable.CompleteRow();
            #endregion
            #region 2nd
            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Customer Name", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.CustomerName, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "VIN", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.ChassisNo, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Time In", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.RcvDateTime.ToString("h:mm tt"), "LEFT", 0, 0, 0);
            oPdfPTable.CompleteRow();
            #endregion
            #region 3rd
            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Address", "LEFT", 0, 0, 0); 
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0);_oFontStyle = this.GetCustomFont("AudiType-Normal", 6);
            this.AddCell(ref oPdfPTable, _oServiceOrder.CustomerAddress, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Date Of Delivery", "LEFT", 0, 0, 0); 
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0);_oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.DelDateTime.ToString("dd MMM yyyy"), "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Service Advisor", "LEFT", 0, 0, 0); 
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0);_oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.AdvisorName, "LEFT", 0, 0, 0);
            oPdfPTable.CompleteRow();
            #endregion
            #region 4th
            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Mobile No", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.CustomerPhone, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Model", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.VehicleModelNo, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "No Show Status", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.NoShowStatus, "LEFT", 0, 0, 0);
            oPdfPTable.CompleteRow();
            #endregion
            #region 5th
           
            #endregion

            #region 6th
            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Email Address", "LEFT", 0, 0, 0); 
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0);_oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.CustomerEmail, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Engine No. ", "LEFT", 0, 0, 0); 
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0);_oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.EngineNo, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Reason of Visit", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.ReasonOfVisit, "LEFT", 0, 0, 0);
            oPdfPTable.CompleteRow();
            #endregion
            #region 7th
            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Contact Person", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.ContactPerson, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Insurance Policy No. ", "LEFT", 0, 0, 0); 
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0);_oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.IPNo, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Extended Warranty", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.ExtendedWarranty, "LEFT", 0, 0, 0);
            oPdfPTable.CompleteRow();
            #endregion
            #region 7th
            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Contact P. Phone", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.ContactPersonPhone, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "I P Expiry Date. ", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.IPExpDate, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Service Plan", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.ServicePlan, "LEFT", 0, 0, 0);
            oPdfPTable.CompleteRow();
            #endregion
            #region 8th
            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Mobility Service", "LEFT", 0, 0, 0); 
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0);_oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.MobilityService, "LEFT", 0, 0, 0);
           
            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "kilometer", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.KilometerReading, "LEFT", 0, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "RSA Policy No", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.RSAPolicyNo, "LEFT", 0, 0, 0);
            oPdfPTable.CompleteRow();
            #endregion


            #region 9th
            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Remarks", "LEFT", 0, 0, 0);
            this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            this.AddCell(ref oPdfPTable, _oServiceOrder.Remarks, "LEFT", 7, 0, 0);

            //_oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            //this.AddCell(ref oPdfPTable, "kilometer", "LEFT", 0, 0, 0);
            //this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            //this.AddCell(ref oPdfPTable, _oServiceOrder.KilometerReading, "LEFT", 0, 0, 0);

            //_oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            //this.AddCell(ref oPdfPTable, "RSA Policy No", "LEFT", 0, 0, 0);
            //this.AddCell(ref oPdfPTable, ": ", "LEFT", 0, 0, 0); _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
            //this.AddCell(ref oPdfPTable, _oServiceOrder.RSAPolicyNo, "LEFT", 0, 0, 0);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 6; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Customer Order Description
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;
            nWidth = 595 / 2;
            oPdfPTable.SetWidths(new float[] { 25, nWidth-25, nWidth});

            this.AddCell(ref oPdfPTable, "", "LEFT", 6, 0, 0);

            _oFontStyle = this.GetCustomFont("AudiType-Bold", 8);
            this.AddCell(ref oPdfPTable, "Customer Order Description", "CENTER",3,0,0);

            #region Description
            _oFontStyle = this.GetCustomFont("AudiType-Bold", nFontSize);
            this.AddCell(ref oPdfPTable, "Demand Repair (Customer Voice)", "CENTER", 2, 0, 1);
            this.AddCell(ref oPdfPTable, "Work To Be Done (Based Upon Customer Voice)", "CENTER", 0, 0, 1);
            oPdfPTable.CompleteRow();

            _oServiceOrder.RegularServiceOrderDetails=_oServiceOrder.ServiceOrderDetails.Where(x => x.ServiceWorkType == EnumServiceType.RegularService).ToList();
            _oServiceOrder.ExtraServiceOrderDetails=_oServiceOrder.ServiceOrderDetails.Where(x => x.ServiceWorkType == EnumServiceType.ExtraService).ToList();
            for (int i = 0; i<8; i++) 
            {
                _oFontStyle = this.GetCustomFont("AudiType-Normal", nFontSize);
                this.AddCell(ref oPdfPTable, (i+1).ToString(), "CENTER", 0, 0, 1);
                this.AddCell(ref oPdfPTable, (_oServiceOrder.RegularServiceOrderDetails.Count() <= i ? "" : _oServiceOrder.RegularServiceOrderDetails[i].WorkDescription), "CENTER", 0, 0, 1);
                this.AddCell(ref oPdfPTable, (_oServiceOrder.ExtraServiceOrderDetails.Count() <= i ? "" : _oServiceOrder.ExtraServiceOrderDetails[i].WorkDescription), "CENTER", 0, 0, 1);
            }

            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 6; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Work & Image
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 595 / 3, 595 / 3+30, 595 / 3-30 });

            _oFontStyle = this.GetCustomFont("AudiType-Bold", 8);
            this.AddCell(ref oPdfPTable, "Inventory", "CENTER", 3, 0, 1);

            #region Image

            string ImgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Images/Service_Order_Img.png");
            _oImag = iTextSharp.text.Image.GetInstance(ImgPath);
            if (_oImag != null)
            {
                _oImag.ScaleAbsolute(180f, 185f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
                this.AddCell(ref oPdfPTable, "Image", "CENTER", 0, 0, 0);
            #endregion

            #region Regular ServiceWork
            PdfPTable oInvPdfPTable = new PdfPTable(6);
            oInvPdfPTable.WidthPercentage = 100;
            oInvPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            float nTWidth=((595/3)+30)/2;
            oInvPdfPTable.SetWidths(new float[] { 18f, 18f, nTWidth - 36, 18f, 18f, nTWidth - 36 });//99f*2

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 8);
            this.AddCell(ref oInvPdfPTable, "X      Dent", "LEFT", 3, 0, 1);
            this.AddCell(ref oInvPdfPTable, "Tyer Condition", "LEFT", 3, 0, 1);
            this.AddCell(ref oInvPdfPTable, "~      Scratch", "LEFT", 3, 0, 1);
            this.AddCell(ref oInvPdfPTable, "Tread Depth (MM)", "LEFT", 3, 0, 1);
            this.AddCell(ref oInvPdfPTable, "##     Body Damage", "LEFT", 3, 0, 1);
            this.AddCell(ref oInvPdfPTable, "F/R", "LEFT", 3, 0, 1);

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 7);
            this.AddCell(ref oInvPdfPTable, "Modification/ Non OEM Fittings", "LEFT", 3, 0, 1);
            _oFontStyle = this.GetCustomFont("AudiType-Normal", 8);

            this.AddCell(ref oInvPdfPTable, "F/L", "LEFT", 3, 0, 1);
            this.AddCell(ref oInvPdfPTable, "", "LEFT", 3, 0, 1);
            this.AddCell(ref oInvPdfPTable, "R/R", "LEFT", 3, 0, 1);
            this.AddCell(ref oInvPdfPTable, "", "LEFT", 3, 0, 1);
            this.AddCell(ref oInvPdfPTable, "R/L", "LEFT", 3, 0, 1);
            oInvPdfPTable.CompleteRow();

            _oFontStyle = this.GetCustomFont("AudiType-Bold", 8);
            this.AddCell(ref oInvPdfPTable, "Direct Reception", "CENTER", 6, 0, 1);

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 8);
            this.AddCell(ref oInvPdfPTable, "OK", "LEFT", 0, 0, 1);
            this.AddCell(ref oInvPdfPTable, "Not OK", "LEFT", 2, 0, 1);
            this.AddCell(ref oInvPdfPTable, "OK", "LEFT", 0, 0, 1);
            this.AddCell(ref oInvPdfPTable, "Not OK", "LEFT", 2, 0, 1);
            oInvPdfPTable.CompleteRow();

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 7);
            this.AddCell(ref oInvPdfPTable, "Windows/Glazing", _oServiceOrder.IsWindows);
            this.AddCell(ref oInvPdfPTable, "Oil Level", _oServiceOrder.IsOilLevel);
            this.AddCell(ref oInvPdfPTable, "Wiper Blades", _oServiceOrder.IsWiperBlades);
            this.AddCell(ref oInvPdfPTable, "Coolant", _oServiceOrder.IsCoolant);
            this.AddCell(ref oInvPdfPTable, "Lights", _oServiceOrder.IsLIghts);
            this.AddCell(ref oInvPdfPTable, "Windshield Washer", _oServiceOrder.IsWindWasher);
            this.AddCell(ref oInvPdfPTable, "Exhaust System", _oServiceOrder.IsExhaustSys);
            this.AddCell(ref oInvPdfPTable, "Brakes", _oServiceOrder.IsBreakes);
            this.AddCell(ref oInvPdfPTable, "Underboody", _oServiceOrder.IsUnderbody);
            this.AddCell(ref oInvPdfPTable, "Axle", _oServiceOrder.IsAxle);
            this.AddCell(ref oInvPdfPTable, "Engine Compartment", _oServiceOrder.IsEngineComp);
            this.AddCell(ref oInvPdfPTable, "Monograms", _oServiceOrder.IsMonograms);
            this.AddCell(ref oInvPdfPTable, "Washing", _oServiceOrder.IsWashing);
            this.AddCell(ref oInvPdfPTable, "Polishing", _oServiceOrder.IsPolishing);

            //Insert Regular Service
            _oPdfPCell = new PdfPCell(oInvPdfPTable); _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region Reception
            PdfPTable oRcpPdfPTable = new PdfPTable(2);
            oRcpPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oRcpPdfPTable.SetWidths(new float[] { (595/3)-25, 25 });//99f*2

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 8);
            _oPdfPCell = new PdfPCell(GetFuelTbl((595 / 3))); _oPdfPCell.Colspan = 2; oRcpPdfPTable.AddCell(_oPdfPCell); oRcpPdfPTable.CompleteRow();
            this.AddCell_Mark(ref oRcpPdfPTable, "Owners Manual", _oServiceOrder.IsOwnersManual);
            this.AddCell_Mark(ref oRcpPdfPTable, "Ser. Sche/ Maintenance Manual", _oServiceOrder.IsScheManual);
            this.AddCell_Mark(ref oRcpPdfPTable, "Infotainment MMI/ Navig. Manual", _oServiceOrder.IsNavManual);
            this.AddCell_Mark(ref oRcpPdfPTable, "Ext. Warranty Book ", _oServiceOrder.IsWBook);
            this.AddCell_Mark(ref oRcpPdfPTable, "Quick Reference Guide", _oServiceOrder.IsRefGuide);
            this.AddCell_Mark(ref oRcpPdfPTable, "Spare Wheel", _oServiceOrder.IsSpareWheel);
            this.AddCell_Mark(ref oRcpPdfPTable, "Jack/ Tool Kits", _oServiceOrder.IsToolKits);
            this.AddCell_Mark(ref oRcpPdfPTable, "Floor Mats", _oServiceOrder.IsFloorMats);
            this.AddCell_Mark(ref oRcpPdfPTable, "Mud Flaps", _oServiceOrder.IsMudFlaps);
            this.AddCell_Mark(ref oRcpPdfPTable, "Warning Triangle", _oServiceOrder.IsWarningT);
            this.AddCell_Mark(ref oRcpPdfPTable, "First Aid Kit", _oServiceOrder.IsFirstAidKit);
            this.AddCell(ref oRcpPdfPTable, "CD's","LEFT", 0, 0, 1);
            this.AddCell(ref oRcpPdfPTable, _oServiceOrder.NoOfCDs.ToString(), "CENTER", 0, 0, 1);
            this.AddCell(ref oRcpPdfPTable, "No Of Keys","LEFT", 0,0,1);
            this.AddCell(ref oRcpPdfPTable, _oServiceOrder.NoOfKeys.ToString(), "CENTER", 0, 0, 1);
            this.AddCell_Mark(ref oRcpPdfPTable, "Other Loose Items", _oServiceOrder.IsOwnersManual);
            _oPdfPCell = new PdfPCell(oRcpPdfPTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            //Insert Regular Service
            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 6; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Financial Information

            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 595 / 2 +15, 595 / 2 -15});

            _oFontStyle = this.GetCustomFont("AudiType-Bold", 7);
            this.AddCell(ref oPdfPTable, "", "LEFT", 0, 0, 1);
            this.AddCell(ref oPdfPTable, "  Financial Information", "LEFT", 0, 0, 1);
            this.AddCell(ref oPdfPTable, "", "LEFT", 0, 0, 1);
            this.AddCell(ref oPdfPTable, "  Mode Of Payment- "+_oServiceOrder.ModeOfPayment, "LEFT", 0, 0, 1);

           
            this.AddCell(ref oPdfPTable, "", "LEFT", 0, 0, 1);
            AddChunk(ref oPdfPTable, "  Estimated Cost " + (string.IsNullOrEmpty(_oServiceOrder.CurrencyName) ? "" : "(" + _oServiceOrder.CurrencyName) + ")", Global.MillionFormat(_oServiceOrder.ENetAmount), _oFontStyle, _oFontStyle, _oServiceOrder.IsTaxesApplicable);  //table,header,data,headerfont,datafont
            AddChunk(ref oPdfPTable, "  Estimated Date Of Delivery (Out) ", _oServiceOrder.DelDateTime.ToString("dd MMM yyyy"), _oFontStyle, _oFontStyle, false);  //table,header,data,headerfont,datafont
            AddChunk(ref oPdfPTable, "  Labour " + (string.IsNullOrEmpty(_oServiceOrder.CurrencyName) ? "" : "(" + _oServiceOrder.CurrencyName + ")  "), Global.MillionFormat(_oServiceOrder.ELCAmount), _oFontStyle, _oFontStyle, false);  //table,header,data,headerfont,datafont
            AddChunk(ref oPdfPTable, "  Estimated Delivery Time (Out)      ", _oServiceOrder.DelDateTime.ToString("h:mm tt"), _oFontStyle, _oFontStyle, false);  //table,header,data,headerfont,datafont
            AddChunk(ref oPdfPTable, "  Parts " + (string.IsNullOrEmpty(_oServiceOrder.CurrencyName) ? "" : "(" + _oServiceOrder.CurrencyName) + ")     ", Global.MillionFormat(_oServiceOrder.EPartsAmount), _oFontStyle, _oFontStyle, false);  //table,header,data,headerfont,datafont

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 6;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            PdfPTable oFTPdfPTable = new PdfPTable(2);
            oFTPdfPTable.SetWidths(new float[] { 595 / 2, 595 / 2 });

            _oFontStyle = this.GetCustomFont_ITALIC("AudiType-Normal", 8);
            string sAuthorization = " HEREBY AUTHORIZE THE ABOVE REPAIR WORK TO BE DONE WITH THE NECESSARY MATERIAL, AND HEREBY GRANT YOU AND/OR YOUR EMPLOYEES TO OPERATE THE CAR HEREIN DESCRIBED ON STREET, HIGHWAYS OR ELSE WHERE FOR THE PURPOSE OF TESTING AND INSPECTION. AN EXPRESS MECHANIC’S LIEN IS HERE BY ACKNOWLEDGED ON ABOVE CAR TO SECURE THE AMOUNT OF THE REPAIR THERETO.";
            AddParagraphRow(ref oFTPdfPTable,"       I"+sAuthorization.ToLowerInvariant());
            //while (this.CalculatePdfPTableHeight(_oPdfPTable) < 580f)
            //{
            //    this.PrintRow("","");
            //    this.PrintRow("", "");
            //}

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 8);


            Phrase _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk("Service Advisor Signature ", this.GetCustomFont("AudiType-Bold", 8)));
            _oPhrase.Add(new Chunk("_______________________________", _oFontStyle));
            _oPhrase.Add(new Chunk("    Terms strictly cash unless otherwise arranged", this.GetCustomFont_ITALIC("AudiType-Normal", 7)));
            _oPhrase.Add(new Chunk("    Customer Signature ", this.GetCustomFont("AudiType-Bold", 8)));
            _oPhrase.Add(new Chunk("__________________________", _oFontStyle));
            _oPdfPCell = new PdfPCell(_oPhrase); _oPdfPCell.FixedHeight = 25; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            oFTPdfPTable.AddCell(_oPdfPCell);

            this.AddCell(ref oFTPdfPTable, "Remarks Area (safety related)", "LEFT", 0, 0, 1);
            this.AddCell(ref oFTPdfPTable, "    Last Visit (History)", "LEFT", 0, 0, 1);
            this.AddCell(ref oFTPdfPTable, "", "LEFT", 0, 0, 1);
            this.AddCell(ref oFTPdfPTable, "", "LEFT", 0, 0, 1);
            this.AddCell(ref oFTPdfPTable, "", "LEFT", 0, 0, 1);
            this.AddCell(ref oFTPdfPTable, "", "LEFT", 0, 0, 1);
            //_oFontStyle = this.GetCustomFont("AudiType-Bold", 6);
            //string sAuthorization = "I HEREBY AUTHORIZE THE ABOVE REPAIR WORK TO BE DONE WITH THE NECESSARY MATERIAL, AND HEREBY GRANT YOU AND/OR YOUR EMPLOYEES TO OPERATE THE CAR HEREIN DESCRIBED ON STREET, HIGHWAYS OR ELSE WHERE FOR THE PURPOSE OF TESTINGANDINSPECTION.ANEXPRESSMECHANIC’SLIENISHEREBYACKNOWLEDGED ONABOVECARTOSECURETHEAMOUNTOFTHEREPAIRTHERETO.";
            //this.AddCell(ref oFTPdfPTable, sAuthorization, "LEFT", 0, 0, 0);

            //string sResponse = "CUSTOMERS ARE REQUESTED TO REMOVE ALL PERSONAL BELONGINGS FORM THEIR VEHICLE. SINCE WE DO NOT ACCEPT ANY RESPONSIBILITY FOR LOSS OR DAMAGE TO CARSORARTICLESLEFTINCARSINCASEOFFIRE,THEFTORANYOTHERCAUSEBEYOND OURCONTROL.7DAYSAFTERJOBCOMPLETED,CARMUSTBECOLLECTEDOTHERWISETHE COMPANYWON’TBERESPONSIBLEFORANYDAMAGEORLOSSCAUSED.";
            //this.AddCell(ref oFTPdfPTable, sResponse, "LEFT", 0, 0, 0);

            //Insert Into Main Table
            _oPdfPCell = new PdfPCell(oFTPdfPTable); _oPdfPCell.Colspan = 6;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        public PdfPTable GetFuelTbl(float nCellWidth) 
        {
            var Fuels = EnumObject.jGets(typeof(EnumFuelStatus));
            Fuels = Fuels.Where(x => x.id != (int)EnumFuelStatus.None).ToList();
            //float nWidth=(nCellWidth- 25)/Fuels.Count();
            //float[] nWidths = new float[Fuels.Count()+1];

            PdfPTable oPdfPTable = new PdfPTable(Fuels.Count() + 1);
            //oPdfPTable.SetWidths(nWidths);
            _oFontStyle = this.GetCustomFont("AudiType-Normal", 7);
            _oPdfPCell = new PdfPCell(new Phrase("Fuel ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);

            foreach (var Fuel in Fuels)
            {
                _oFontStyle = this.GetCustomFont("AudiType-Normal", 5f);
                _oPdfPCell = new PdfPCell(new Phrase(Fuel.Value, _oFontStyle));
                if(Fuel.id==_oServiceOrder.FuelStatusInt)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); ;
                    _checkmark.Add(new ListItem(Fuel.Value, _oFontStyle)); _oPdfPCell.FixedHeight = 12.7f;
                    _oPdfPCell.AddElement(_checkmark);
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(_oPdfPCell);
            }
            
            return oPdfPTable;
        }

        #region PDF HELPER
        public void AddParagraphRow(ref PdfPTable oPdfPTable, string sData)
        {
            Paragraph _oPdfParagraph;
            _oPdfParagraph = new Paragraph(new Phrase(sData, _oFontStyle));
            _oPdfParagraph.SetLeading(0f, 1f);
            _oPdfParagraph.Alignment = Element.ALIGN_JUSTIFIED;
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.Border = 0;
            _oPdfPCell.AddElement(_oPdfParagraph);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddChunk(ref PdfPTable oPdfPTable, string sHeader, string sValue, iTextSharp.text.Font oFontStyleRowHeader, iTextSharp.text.Font oFontStyleData,bool nBit)  //table,header,data,headerfont,datafont
        {
            Phrase _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk(sHeader+":    ", oFontStyleRowHeader));
            _oPhrase.Add(new Chunk(sValue, oFontStyleData));
            if (nBit)
                _oPhrase.Add(new Chunk("    Taxes as applicable", oFontStyleRowHeader));

            _oPdfPCell = new PdfPCell(_oPhrase); 
            oPdfPTable.AddCell(_oPdfPCell);
        }
      
        public void AddCell(ref PdfPTable oPdfPTable, string sHeader, string sAlignment, int nColSpan, int nRowSpan, int nBorder)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default
            this.SetAlgin(ref _oPdfPCell, sAlignment);

            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.MinimumHeight = 13f;

            if (nColSpan > 0)
                _oPdfPCell.Colspan = nColSpan;
            if (nRowSpan > 0)
                _oPdfPCell.Rowspan = nRowSpan;
            if (nBorder == 0)
                _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddCell(ref PdfPTable oPdfPTable, string sHeader, string sAlignment, float nRowHeight)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default
            this.SetAlgin(ref _oPdfPCell, sAlignment);
            //_oPdfPCell.Border = 1;
            _oPdfPCell.FixedHeight = nRowHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddCell(string sHeader, string sAlignment, bool isGray)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default
            this.SetAlgin(ref _oPdfPCell, sAlignment);
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            if (isGray)
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;

            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void SetAlgin(ref PdfPCell oPdfPCell, string sAlignment) 
        {
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "LEFT")
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "RIGHT")
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment == "CENTER")
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        }
        public void PrintRow(string sHeader, string sAlign)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            this.SetAlgin(ref _oPdfPCell, sAlign);
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 15f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColspan;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void AddCell(ref PdfPTable oPdfPTable, string sHeader, bool nBit)
        {
            string OK = "", Not_OK = "";

            if (nBit)
                OK = "YES";
            else
                Not_OK = "NO";

            _oPdfPCell = new PdfPCell(new Phrase(OK, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Not_OK, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddCell_Mark(ref PdfPTable oPdfPTable, string sHeader, bool nBit)
        {
            _oFontStyle = this.GetCustomFont("AudiType-Normal", 7);
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            if (nBit)
            {
                _checkmark= new ZapfDingbatsList(52);
                _checkmark.Lowercase = true;
                _oFontStyle = this.GetCustomFont("AudiType-Normal", 6);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _checkmark.Add(new ListItem("   ", _oFontStyle)); _oPdfPCell.FixedHeight = 12.7f;
                _oPdfPCell.AddElement(_checkmark);
            }
            else 
            {
                _oFontStyle = this.GetCustomFont("AudiType-Bold", 7);
                _oPdfPCell = new PdfPCell(new Phrase("X", _oFontStyle));
                _oPdfPCell.FixedHeight = 12.7f;
            }

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(_oPdfPCell);
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
        public iTextSharp.text.Font GetCustomFont_ITALIC(string fontName, float fFontSize)
        {
            string fontpath = System.Web.Hosting.HostingEnvironment.MapPath("~/fonts/");
            BaseFont customfont = BaseFont.CreateFont(fontpath + fontName + ".ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font oFont = new iTextSharp.text.Font(customfont, fFontSize, iTextSharp.text.Font.ITALIC);
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