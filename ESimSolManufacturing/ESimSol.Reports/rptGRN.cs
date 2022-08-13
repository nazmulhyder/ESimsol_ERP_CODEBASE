using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using System.Linq;
using iTextSharp.text.pdf;
namespace ESimSol.Reports
{
    public class rptGRN
    {
        #region Declaration
        private int _nColumn = 10;
        private Document _oDocument;
        private iTextSharp.text.Font _oFontStyle;
        private PdfPTable _oPdfPTable = new PdfPTable(10);
        PdfWriter PDFWriter;
        private PdfPCell _oPdfPCell;
        private iTextSharp.text.Image _oImag;
        private MemoryStream _oMemoryStream = new MemoryStream();
        private GRN _oGRN = new GRN();
        private List<GRNDetail> _oGRNDetails = new List<GRNDetail>();
        private List<ApprovalHead> _oApprovalHeads = new List<ApprovalHead>();
        private Company _oCompany = new Company();
        Phrase _oPhrase = new Phrase();
        Chunk _oChunk = new Chunk();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
        #endregion

        public byte[] PrepareReport(GRN oGRN, ClientOperationSetting oClientOperationSetting, Company oCompany, List<ApprovalHead> oAH, bool bIsA4, List<SignatureSetup> oSignatureSetups)//
        {
            _oGRN = oGRN;
            _oGRNDetails = oGRN.GRNDetails;
            _oCompany = oCompany;
            _oClientOperationSetting = oClientOperationSetting;
            _oApprovalHeads = oAH;
            _oSignatureSetups = oSignatureSetups;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            if (bIsA4)
            {
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            }
            else
            {
                _oDocument = new Document(new iTextSharp.text.Rectangle(595f, 421f), 0f, 0f, 0f, 0f); //A4:842*595            
            } 
            _oDocument.SetMargins(15f, 15f, 20f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;


                _oPdfPTable = new PdfPTable(10);
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
                PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
                ESimSolFooter PageEventHandler = new ESimSolFooter();
                PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler

                _oDocument.Open();
                _oPdfPTable.SetWidths(new float[] { 
                                                25f, //SL
                                                70f, //Product Name
                                                65f, //Specification
                                                60f,//Style
                                                60f,//Color 
                                                30f,//size
                                                120f,//Project name
                                                //67f,//weight/cartoon
                                                //60f,//cone/cartoon
                                                65f, //Lot nO
                                                30f, //Unit
                                                70f //qty
                                            });
  
            #endregion

            this.PrintHeader();
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 4;
            _oPdfPTable.HeaderRows = 1;
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
            oPdfPTable.SetWidths(new float[] { 100f, 300f, 60f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oGRN.BusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oGRN.BusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Goods Receive Note (GRN)", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = _nColumn; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion


        #region Report Body
        private void PrintBody()
        {
            Phrase oPhrase = new Phrase();

            #region GRN
            PdfPTable oTempPdfPTable = new PdfPTable(4);
            oTempPdfPTable.SetWidths(new float[] { 90f, 235f, 100f, 110f});//535
            oTempPdfPTable.WidthPercentage = 100;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("GRN No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.GRNNo,FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            oPhrase.Add(new Chunk("                 Date :", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.GRNDateSt, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));

            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.ChallanNo, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Received Store", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.StoreNameWithoutLocation, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;           
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Ref Type", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.GRNTypeSt, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();



            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oGRN.GRNType.ToString()+" No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.RefObjectNo, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oGRN.GRNType.ToString() + " Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.RefObjectDateSt, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Supplier", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.ContractorName, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.ImportLCNo, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();




            _oPdfPCell = new PdfPCell(new Phrase("Gate In No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.GatePassNo, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            oPhrase.Add(new Chunk("                 Vehicle No :", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.VehicleNo, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));

            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.Remarks, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oTempPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = _nColumn; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            #endregion
            #region Blank Rows 
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 15f; _oPdfPCell.Colspan = _nColumn; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region GRNDetail
            _oPdfPCell = new PdfPCell(new Phrase("Item Details Description", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            

            if (_oClientOperationSetting.Value == "1")//if styleno  Apply
            {
                _oPdfPCell = new PdfPCell(new Phrase("Product", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;  _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Specification", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Style", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Size", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Product", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Specification", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("Project/Dept/Section", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Weight/Crtn", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Cone/Crtn", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                       
            _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Quantity", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            int nCount = 0;
            double nTotalQty = 0; string sStyleNo = null; string TempDescription = "";
            foreach (GRNDetail oItem in _oGRNDetails)
            {
                nCount++;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));  
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                if (_oClientOperationSetting.Value == "1")//ip style no Apply
                {

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;  _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.TechnicalSpecification, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    
                    if (oItem.StyleNo != sStyleNo)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                        _oPdfPCell.Rowspan = _oGRNDetails.Where(x => x.StyleNo == oItem.StyleNo).Count(); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        sStyleNo = oItem.StyleNo;
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.SizeName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    //for print mc dia, finish dia and gsm
                    if (!string.IsNullOrEmpty(oItem.MCDia) || !string.IsNullOrEmpty(oItem.FinishDia) || !string.IsNullOrEmpty(oItem.GSM))
                    {

                        TempDescription = oItem.ProductName + "\n";
                        TempDescription = TempDescription + "(";
                        TempDescription = oItem.MCDia != "" ? TempDescription + oItem.MCDia + "," : TempDescription;
                        TempDescription = oItem.FinishDia != "" ? TempDescription + oItem.FinishDia + "," : TempDescription;
                        TempDescription = oItem.GSM != "" ? TempDescription + oItem.GSM + "," : TempDescription;
                        TempDescription = TempDescription.Substring(0, TempDescription.Length - 1);//remove Last Comma
                        TempDescription = TempDescription + ")";
                        _oPdfPCell = new PdfPCell(new Phrase(TempDescription, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.TechnicalSpecification, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
       

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProjectName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.WeightPerCartoon.ToString("###,0.00"), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.ConePerCartoon.ToString("###,0.00"), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                                
                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));  
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUSymbol, _oFontStyle));  
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.ReceivedQty), _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                nTotalQty += oItem.ReceivedQty;
            }

            //int n = 35;
            //for (int i = nCount; i < n; i++)
            //{
            //    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            //    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));  
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            //    if (_oClientOperationSetting.Value == "1")//ip style no Apply
            //    {

            //        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;  _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            //        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    }
            //    else
            //    {

            //        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 5; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    }
              

            //    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));  
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            //    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));  
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //}


            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyle)); _oPdfPCell.Colspan = _nColumn - 1; _oPdfPCell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyle)); _oPdfPCell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            #endregion

            
            #endregion

            #region Authorization
            //if (_oApprovalHeads.Count > 0)
            //{
            //    int signNumber = _oApprovalHeads.Count;
            //    oTempPdfPTable = new PdfPTable(signNumber);

            //    float colWidth = 535 / signNumber;
            //    float[] widths = new float[signNumber];
            //    for (int i = 0; i < _oApprovalHeads.Count; i++)
            //    {
            //        widths[i] = colWidth;
            //    }
            //    oTempPdfPTable.SetWidths(widths);//535

            //    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            //    foreach (ApprovalHead oItem in _oApprovalHeads)
            //    {
            //        _oPdfPCell = new PdfPCell(new Phrase("__________________", _oFontStyle)); _oPdfPCell.Border = 0;
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

            //    }
            //    oTempPdfPTable.CompleteRow();

            //    foreach (ApprovalHead oItem in _oApprovalHeads)
            //    {
            //        _oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyle)); _oPdfPCell.Border = 0;
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

            //    }
            //    oTempPdfPTable.CompleteRow();

            //    _oPdfPCell = new PdfPCell(oTempPdfPTable); 
            //    _oPdfPCell.Colspan = _nColumn; _oPdfPCell.FixedHeight = 50f; _oPdfPCell.Border = 0;
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //}

           
            #endregion


            #region print Signature Captions
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.FixedHeight = 20f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(525f, (object)_oGRN, _oSignatureSetups, 20.0f)); _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion
    }
}
