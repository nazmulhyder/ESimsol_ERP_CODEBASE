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

    public class rptHandoverCheckList
    {
        #region Declaration
        int _nTotalColumn = 1;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleNormal;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        PdfPCell _oPdfPCellDetail;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        PreInvoice _oPreInvoice = new PreInvoice();
        List<PreInvoice> _oPreInvoices = new List<PreInvoice>();
        List<ModelFeature> _oModelFeatures = new List<ModelFeature>();
        Company _oCompany = new Company();
        int n = 0;
        //System.Drawing.Image _img1;
        //System.Drawing.Image _img2;
        //System.Drawing.Image _img3;
        //System.Drawing.Image _img4;

        #endregion

        public byte[] PrepareReport(PreInvoice oPreInvoice, Company oCompany, List<ModelFeature> oModelFeatures)
        {
            _oPreInvoice = oPreInvoice;
            _oCompany = oCompany;
            _oModelFeatures = oModelFeatures;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(50f, 20f, 50f, 50f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;


            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            ESimSolFooter PageEventHandler = new ESimSolFooter("By signing this form the opwner agrees to vehicle detyails and condition as described above. Also, you should initial the attached sheets.");

            //PageEventHandler.signatures = signatureList;
            PageEventHandler.nFontSize = 9;
            PdfWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            //this.PrintHeader();
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, 1);
            _oFontStyleNormal = FontFactory.GetFont("Tahoma", 10f, 0);

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 25f,430f,130f
                                                 });

            #region CompanyLogo
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //TitleColumn
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            oPdfPTable.AddCell(_oPdfPCell);

            _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
            _oImag.ScaleAbsolute(73f, 25f);
            _oPdfPCell = new PdfPCell(_oImag);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0; 
            _oPdfPCell.FixedHeight = 15f;
            oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            oPdfPTable.CompleteRow();

            #region Audi
            _oPdfPCell = new PdfPCell(); //TitleColumn
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED);
            _oPdfPCell = new PdfPCell(new Phrase("Audi", _oFontStyle)); //TitleColumn
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, 1);

            #region blank
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //TitleColumn
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.AddElement(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oFontStyleNormal = FontFactory.GetFont("Tahoma", 10f, 0);

            this.PrintHeader();
            this.GetFirstTable();
            this.NewPageDeclaration();
            _oPdfPTable.HeaderRows = 1;

            this.PrintHeader();
            this.GetImageTable();
            this.NewPageDeclaration();
            _oPdfPTable.HeaderRows = 1;

            //this.PrintHeader();
            this.GetFullFeatureTable();
            //this.NewPageDeclaration();
            //_oPdfPTable.HeaderRows = 1;

            this.GetLastTable();
            this.NewPageDeclaration();
            _oPdfPTable.HeaderRows = 1;
        }
        #endregion

        #region function

        public void NewPageDeclaration()
        {
            #region New Page Declare
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
            #endregion
        }
        private void GetFirstTable()
        {
            PdfPTable oFirstTable = new PdfPTable(2);
            oFirstTable.WidthPercentage = 100;
            oFirstTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oFirstTable.SetWidths(new float[] {    
                                                    40f,
                                                    60f
                                                          });
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, 1);
            _oFontStyleNormal = FontFactory.GetFont("Tahoma", 10f, 0);

            #region head description
            _oPdfPCell = new PdfPCell(new Phrase("Vehicle Handover Checklist", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;  oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 10; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Please check the details lited on the form against the vehicle you are handing over.", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleNormal)); _oPdfPCell.FixedHeight = 10; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Your signature confirms these details are correct, and are the conditions of acceptence of the vehicle.", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleNormal)); _oPdfPCell.FixedHeight = 20; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();
            #endregion

            #region data
            _oPdfPCell = new PdfPCell(new Phrase("Name ", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oPreInvoice.CustomerName, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Company ", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " , _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Owner Contact Details", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oPreInvoice.CustomerCellPhone, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Vehicle make / model", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oPreInvoice.ModelNo, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Chassis No", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oPreInvoice.ChassisNo, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Engine Number", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oPreInvoice.EngineNo, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Dealer", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oCompany.Name, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Delivery Location", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": Audi Dhaka Showroom" , _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Contact Detail", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oCompany.Phone, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Date / Time vehicle handover", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oPreInvoice.HandoverDateST, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Vehicle mileage", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oPreInvoice.VehicleMileage + " KM", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Wheel Condition", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oPreInvoice.WheelCondition, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Bodywork Condition", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oPreInvoice.BodyWorkCondition, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Interior Condition", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oPreInvoice.InteriorCondition, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            #endregion

            #region dealer owner signature
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 40; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Dealer Signature", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.HandoverDateST, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.DealerPerson, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 30; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Owner Signature", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.HandoverDateST, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.Owner, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            if (!string.IsNullOrEmpty(_oPreInvoice.OwnerNID))
            {
                _oPdfPCell = new PdfPCell(new Phrase("National ID: " + _oPreInvoice.OwnerNID, _oFontStyle)); _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);
            }
            oFirstTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 50;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();
            #endregion

            //_oFontStyleNormal = FontFactory.GetFont("Tahoma", 7f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase("By signing this form the opwner agrees to vehicle detyails and condition as described above. Also, you should initial the attached sheets.", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oFirstTable.AddCell(_oPdfPCell);

            oFirstTable.CompleteRow();

            //return oFirstTable;
            #region push into main table
            _oPdfPCell = new PdfPCell(oFirstTable);
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void GetFullFeatureTable()
        {
            //foreach (ModelFeature oModelFeature in _oModelFeatures)
            //{
            //    _oPdfPCell = new PdfPCell(GetFeatureTable(oModelFeature)); _oPdfPCell.Border = 0;
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; //_oPdfPCell.FixedHeight = 150;
            //    _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //}

            foreach (ModelFeature oModelFeature in _oModelFeatures)
            {
                if (oModelFeature.FeatureType != EnumFeatureType.OptionalFeature)
                {
                    _oPdfPCell = new PdfPCell(GetFeatureTable(oModelFeature)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; //_oPdfPCell.FixedHeight = 150;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
            }

            foreach (SalesQuotationDetail oSalesQuotationDetail in _oPreInvoice.SalesQuotation.SalesQuotationDetails)
            {
                _oPdfPCell = new PdfPCell(GetFeatureTable(new ModelFeature()
                {
                    FeatureCode = oSalesQuotationDetail.FeatureCode,
                    Price = oSalesQuotationDetail.Price,
                    FeatureName = oSalesQuotationDetail.FeatureName,
                    FeatureType = EnumFeatureType.OptionalFeature
                })); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; //_oPdfPCell.FixedHeight = 150;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            
        }
        private PdfPTable GetFeatureTable(ModelFeature oModelFeature)
        {
            PdfPTable oFeatureTable = new PdfPTable(3);
            oFeatureTable.WidthPercentage = 100;
            oFeatureTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oFeatureTable.SetWidths(new float[] {    
                                                    15f,
                                                    75f,
                                                    10f
                                                          });
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oFontStyleNormal = FontFactory.GetFont("Tahoma", 10f, 0);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFeatureTable.AddCell(_oPdfPCell);

            //oFeatureTable.CompleteRow();
            n++;
            if (n == 1)
            {
                #region data
                _oFontStyleNormal = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE);

                _oPdfPCell = new PdfPCell(new Phrase("Vehicle Equipment Checklist: ", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 30;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                oFeatureTable.CompleteRow();

                _oFontStyleNormal = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

                _oPdfPCell = new PdfPCell(new Phrase("Model ", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.ModelNo, _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                oFeatureTable.CompleteRow();

                //_oPdfPCell = new PdfPCell(new Phrase("Model ", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.ModelNo, _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                //oFeatureTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Code ", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.SalesQuotation.ModelCode, _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                oFeatureTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Chassis No ", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.ChassisNo, _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                oFeatureTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Engine No ", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.EngineNo, _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                oFeatureTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Exterior Color ", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.ExteriorColorName, _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                oFeatureTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Interior Color ", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.InteriorColorName, _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                oFeatureTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                oFeatureTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("(" + _oPreInvoice.ModelShortName + ")", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.ModelNo, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                oFeatureTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

                oFeatureTable.CompleteRow();

                #endregion
            }
            

            #region feature
            _oPdfPCell = new PdfPCell(new Phrase(oModelFeature.FeatureCode, _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oModelFeature.FeatureName, _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFeatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("OK", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFeatureTable.AddCell(_oPdfPCell);

            oFeatureTable.CompleteRow();
            #endregion

            return oFeatureTable;
        }

        private void GetLastTable()
        {
            PdfPTable oLastTable = new PdfPTable(3);
            oLastTable.WidthPercentage = 100;
            oLastTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oLastTable.SetWidths(new float[] {    
                                                    45f,
                                                    10f,
                                                    45f
                                                          });
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oFontStyleNormal = FontFactory.GetFont("Tahoma", 10f, 0);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 35; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oLastTable.AddCell(_oPdfPCell);

            oLastTable.CompleteRow();

            #region signature
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthBottom = 1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oLastTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oLastTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthBottom = 1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oLastTable.AddCell(_oPdfPCell);

            oLastTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.DealerPerson, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oLastTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oLastTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.Owner, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oLastTable.AddCell(_oPdfPCell);

            oLastTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 15; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oLastTable.AddCell(_oPdfPCell);

            oLastTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Note:    " + _oPreInvoice.Note, _oFontStyleNormal)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oLastTable.AddCell(_oPdfPCell);

            oLastTable.CompleteRow();

            //return oLastTable;
            #region push into main table
            _oPdfPCell = new PdfPCell(oLastTable);
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void GetImageTable()
        {
            PdfPTable oImageTable = new PdfPTable(5);
            oImageTable.WidthPercentage = 100;
            oImageTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oImageTable.SetWidths(new float[] {    
                                                    20f,
                                                    20f,
                                                    20f,
                                                    20f,
                                                    20f
                                                          });
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, 1);
            _oFontStyleNormal = FontFactory.GetFont("Tahoma", 10f, 0);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 15; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            //oImageTable.CompleteRow();

            _oFontStyleNormal = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);    //| iTextSharp.text.Font.UNDERLINE
            _oPdfPCell = new PdfPCell(new Phrase("Exterior Vehicle Condition: ", _oFontStyleNormal)); _oPdfPCell.FixedHeight = 30; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleNormal)); _oPdfPCell.FixedHeight = 10; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();

            _oFontStyleNormal = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            #region immg
            //string fileDirectory = "~/Content/Images/BGTheme.jpg";
            //System.IO.File.Delete(fileDirectory);

            #region first img
            //string ImgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Images/audi1.jpg");
            //_oImag = iTextSharp.text.Image.GetInstance(ImgPath);
            if (_oPreInvoice.SideImage != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oPreInvoice.SideImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(280f, 120f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.Rowspan = 4;
                _oPdfPCell.FixedHeight = 120f;
                oImageTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 3; _oPdfPCell.Rowspan = 4;
                _oPdfPCell.FixedHeight = 120f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("Pass", _oFontStyle)); _oPdfPCell.Border = 15; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Fail", _oFontStyle)); _oPdfPCell.Border = 15; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();
            #endregion


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 15; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();


            #region 2nd img
            //ImgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Images/audi2.jpg");
            //_oImag = iTextSharp.text.Image.GetInstance(ImgPath);
            //_oImag = iTextSharp.text.Image.GetInstance(_img2, System.Drawing.Imaging.ImageFormat.Jpeg);
            if (_oPreInvoice.TopImage != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oPreInvoice.TopImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(280f, 120f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.Rowspan = 4;
                _oPdfPCell.FixedHeight = 120f;
                oImageTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 3; _oPdfPCell.Rowspan = 4;
                _oPdfPCell.FixedHeight = 120f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("Pass", _oFontStyle)); _oPdfPCell.Border = 15; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Fail", _oFontStyle)); _oPdfPCell.Border = 15; _oPdfPCell.BackgroundColor = BaseColor.WHITE; 
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 15; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();

            #region 3rd img
            //ImgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Images/audi3.jpg");
            //_oImag = iTextSharp.text.Image.GetInstance(ImgPath);
            //_oImag = iTextSharp.text.Image.GetInstance(_img3, System.Drawing.Imaging.ImageFormat.Jpeg);
            if (_oPreInvoice.FrontImage != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oPreInvoice.FrontImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(210f, 120f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 6; _oPdfPCell.Colspan = 2;
                _oPdfPCell.FixedHeight = 120f;
                oImageTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2; _oPdfPCell.Rowspan = 6;
                _oPdfPCell.FixedHeight = 120f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Pass", _oFontStyle)); _oPdfPCell.Border = 15; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; 
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Fail", _oFontStyle)); _oPdfPCell.Border = 15; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; //_oPdfPCell.Colspan = 2;// _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 15; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();

            #region 4th img
            //ImgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Images/audi4.jpg");
            //_oImag = iTextSharp.text.Image.GetInstance(ImgPath);
            //_oImag = iTextSharp.text.Image.GetInstance(_img4, System.Drawing.Imaging.ImageFormat.Jpeg);
            if (_oPreInvoice.BackImage != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oPreInvoice.BackImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(210f, 120f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.Rowspan = 6;
                _oPdfPCell.FixedHeight = 120f;
                oImageTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
                _oPdfPCell.FixedHeight = 120f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Pass", _oFontStyle)); _oPdfPCell.Border = 15; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Fail", _oFontStyle)); _oPdfPCell.Border = 15; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; //_oPdfPCell.Colspan = 2;// _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();

            #endregion

            ////_oFontStyleNormal = FontFactory.GetFont("Tahoma", 10f, 0);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 50; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            //oImageTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 60; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 60; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 60; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.DealerPerson, _oFontStyle)); _oPdfPCell.FixedHeight = 15; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 15; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.Owner, _oFontStyle)); _oPdfPCell.FixedHeight = 15; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oImageTable.AddCell(_oPdfPCell);

            oImageTable.CompleteRow();

            #endregion


            //return oImageTable;
            #region push into main table
            _oPdfPCell = new PdfPCell(oImageTable);
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        #endregion

    }
}
