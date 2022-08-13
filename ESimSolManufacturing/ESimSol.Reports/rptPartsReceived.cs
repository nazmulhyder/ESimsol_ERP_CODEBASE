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
    public class rptPartsReceived
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
        #endregion

        public byte[] PrepareReport(GRN oGRN,ClientOperationSetting oClientOperationSetting,  Company oCompany, List<ApprovalHead> oAH)
        {
            _oGRN = oGRN;
            _oGRNDetails = oGRN.GRNDetails;
            _oCompany = oCompany;
            _oClientOperationSetting = oClientOperationSetting;
            _oApprovalHeads = oAH;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
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
                                                100f, //Product Name
                                                100f, //Specification
                                                70f,//Style
                                                50f,//Color 
                                                40f,//size
                                                80f,//Project name
                                                70f, //Lot nO
                                                30f, //Unit
                                                50f //qty
                                            });
  
            #endregion

            ESimSolPdfHelper.PrintHeader_Audi(ref _oPdfPTable, oGRN.BusinessUnit, _oCompany, new string[] { "Parts Received Form", "Audi Service Dhaka", "Audi", "429/432, Tejgaon I/A, Dhaka-1208", "Service" },_nColumn);
        
            this.PrintBody();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody()
        {
            PdfPTable oEsimSolPdfTable = new PdfPTable(1);
            string[][] GRNDetails = null ;

            if (_oGRN.GRNType == EnumGRNType.LocalInvoice) 
               GRNDetails= new string[][] {
                                        new string[] {"PR-NO","Date","Challan No","Received Store"},
                                        new string[] {_oGRN.GRNNo,_oGRN.GRNDateSt,_oGRN.ChallanNo,_oGRN.StoreNameWithoutLocation},
                                        new string[] {"Ref Type","Local Invoice No","Local Invoice Date","Supplier"},
                                        new string[] {_oGRN.GRNTypeSt,_oGRN.RefObjectNo,_oGRN.RefObjectDateSt,_oGRN.ContractorName},
                                        new string[] {"Gate Pass No","Vehicle No","~Merge","Remarks"},
                                        new string[] {_oGRN.GatePassNo,_oGRN.VehicleNo,"~Merge",_oGRN.Remarks},
                                        };            
            else
                GRNDetails = new string[][] {
                                        new string[] {"PR-NO","Date","Challan No","Received Store"},
                                        new string[] {_oGRN.GRNNo,_oGRN.GRNDateSt,_oGRN.ChallanNo,_oGRN.StoreNameWithoutLocation},
                                        new string[] {"Ref Type","SO No","SO Date","Supplier"},
                                        new string[] {_oGRN.GRNTypeSt,_oGRN.RefObjectNo,_oGRN.RefObjectDateSt,_oGRN.ContractorName},
                                        new string[] {"Gate Pass No","Vehicle No","~Merge","Remarks"},
                                        new string[] {_oGRN.GatePassNo,_oGRN.VehicleNo,"~Merge",_oGRN.Remarks},
                                        };

            
            ESimSolPdfHelper.PrintHeaderInfo(ref oEsimSolPdfTable, "", "Parts Receive Form", GRNDetails);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oEsimSolPdfTable, iTextSharp.text.Rectangle.NO_BORDER, 0, _nColumn);


            Phrase oPhrase = new Phrase();
            PdfPTable oTempPdfPTable = new PdfPTable(4);
            oTempPdfPTable.SetWidths(new float[] { 90f, 235f, 100f, 110f });//535
            oTempPdfPTable.WidthPercentage = 100;


            #region GRNDetail
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Part No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Item", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Project", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                       
            _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Quantity", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            int nCount = 0;
            double nTotalQty = 0; string sStyleNo = null;
            foreach (GRNDetail oItem in _oGRNDetails)
            {
                nCount++;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));  
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductCode, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;  _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 4; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
             
                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProjectName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                                
                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));  
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUSymbol, _oFontStyle));  
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.ReceivedQty), _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                nTotalQty += oItem.ReceivedQty;
            }

            int n = 35;
            for (int i = nCount; i < n; i++)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));  
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
               

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 4; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
          

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));  
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));  
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }


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
            if (_oApprovalHeads.Count > 0)
            {
                int signNumber = _oApprovalHeads.Count;
                oTempPdfPTable = new PdfPTable(signNumber);

                float colWidth = 535 / signNumber;
                float[] widths = new float[signNumber];
                for (int i = 0; i < _oApprovalHeads.Count; i++)
                {
                    widths[i] = colWidth;
                }
                oTempPdfPTable.SetWidths(widths);//535

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                foreach (ApprovalHead oItem in _oApprovalHeads)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("____________________", _oFontStyle)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                }
                oTempPdfPTable.CompleteRow();

                foreach (ApprovalHead oItem in _oApprovalHeads)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyle)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                }
                oTempPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oTempPdfPTable); _oPdfPCell.Colspan = _nColumn; _oPdfPCell.FixedHeight = 100f; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }

           
            #endregion
        }
        #endregion
    }
}
