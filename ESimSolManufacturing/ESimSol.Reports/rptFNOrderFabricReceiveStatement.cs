using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace ESimSol.Reports
{
    public class rptFNOrderFabricReceiveStatement
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        string sHeaderString = "";
        int nSL = 0;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<FNOrderFabricReceive> _oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
        List<FNOrderFabricTransfer> _oInQtys = new List<FNOrderFabricTransfer>();
        List<FNOrderFabricTransfer> _oOutQtys = new List<FNOrderFabricTransfer>();
        List<FNOrderFabricTransfer> _oReturnQtys = new List<FNOrderFabricTransfer>();
        List<FNOrderFabricTransfer> _oConQtys = new List<FNOrderFabricTransfer>();
        FNOrderFabricReceive _oFNOrderFabricReceive = new FNOrderFabricReceive();
        Company _oCompany = new Company();
        string sOrderNo = ""; string sDispoNo = ""; string sConstructionNo = ""; string sBuyerNo = ""; string sIssueDate = "";
        #endregion
        public byte[] PrepareReport(List<FNOrderFabricReceive> oFNOrderFabricReceives, List<FNOrderFabricTransfer> oInQtys, List<FNOrderFabricTransfer> oOutQtys, List<FNOrderFabricTransfer> oReturnQtys, List<FNOrderFabricTransfer> oConQtys, Company oCompany, string sMessage, FabricSCReport oFabricSCReport)
        {
            _oFNOrderFabricReceives = oFNOrderFabricReceives;
            _oInQtys = oInQtys;
            _oOutQtys = oOutQtys;
            _oReturnQtys = oReturnQtys;
            _oConQtys = oConQtys;
            _oCompany = oCompany;

            sOrderNo = oFabricSCReport.SCNoFull;
            sDispoNo = oFabricSCReport.ExeNo;
            sBuyerNo = oFabricSCReport.BuyerName;
            sConstructionNo = oFabricSCReport.Construction;
            sIssueDate = oFabricSCReport.SCDateSt;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(25f, 25f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.nFontSize = 7;
            PdfWriter.PageEvent = PageEventHandler;
            _oPdfPTable.SetWidths(new float[] { 595f });
            _oDocument.Open();
            #endregion
            this.PrintHeader();
            this.PrintBody();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header 1
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Space          
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("FN Order Fabric Receive Statement", _oFontStyle)); 
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            ReportHeader2();
            Space();
            FNOrderFabricTransferDescriptionTable();
            Space();
            if (_oInQtys.Count > 0) { FabricReceiveTable(); }          
            Space();
            if (_oOutQtys.Count > 0) { FabricOutTable(); }
            Space();
            if (_oReturnQtys.Count > 0) { FabricReturnTable(); }
            Space();
            if (_oConQtys.Count > 0) { FabricConTable(); }    
            Space();         
        }
        #endregion

        public void Space()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void ReportHeader2()
        {
            PdfPTable oTopTable = new PdfPTable(6);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                                             
                                            15f,//1  
                                            15f,//2  
                                            15f,//3  
                                            15f,//4   
                                            15f,//5  
                                            15f, //6                             
                                           });

            #region Initialize Table
            oTopTable = new PdfPTable(6);
            oTopTable.SetWidths(new float[] {                                             
                                            15f,//1  
                                            15f,//2  
                                            15f,//3  
                                            15f,//4   
                                            15f,//5  
                                            15f,//6                             
                                           });
            #endregion
            #region ROW-1
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Order No:", _oFontStyle)); //_oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sOrderNo, _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Issue Date", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sIssueDate, _oFontStyle)); //_oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyle)); //_oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sBuyerNo, _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();

            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
            #region ROW-2
            #region Initialize Table
            oTopTable = new PdfPTable(6);
            oTopTable.SetWidths(new float[] {                                             
                                            15f,//1  
                                            15f,//2  
                                            15f,//3  
                                            15f,//4   
                                            15f,//5  
                                            15f, //6                             
                                           });
            #endregion
    
            _oPdfPCell = new PdfPCell(new Phrase("Construction:", _oFontStyle)); //_oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sConstructionNo, _oFontStyle)); //_oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Dispo No:", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sDispoNo, _oFontStyle)); //_oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //_oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
             #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
        
        }
        public void FNOrderFabricTransferDescriptionTable()
        {
            PdfPTable oTopTable = new PdfPTable(10);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5  
                                            12f,//6    
                                            12f,//7  
                                            12f,//8  
                                            15f,//9 
                                            12f //10 
                                           });

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Items Description", _oFontStyle)); _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Colspan = 10; //_oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region HEADER
            #region initialize table
            oTopTable = new PdfPTable(10);
            oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5  
                                            12f,//6    
                                            12f,//7  
                                            12f,//8  
                                            15f,//9 
                                            12f //10
                                           });
            #endregion
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Roll No", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric Source", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric Rcv Qty(Y)", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric Rcv Qty(M)", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Qty Receive(Y)", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Qty Transfer(Y)", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Qty Return(Y)", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Qty Consumption(Y)", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Balance(Y)", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
            #region DATA
            int nSL = 1;
            foreach (FNOrderFabricReceive oItem in _oFNOrderFabricReceives)
            {
                #region initialize table
                oTopTable = new PdfPTable(10);
                oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5  
                                            12f,//6    
                                            12f,//7  
                                            12f,//8  
                                            15f,//9 
                                            12f //10
                                           });
                #endregion
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricSource, _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.QtyInMeter.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.QtyTrIn.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.QtyTrOut.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.QtyReturn.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.QtyCon.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.YetToTransferQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                oTopTable.CompleteRow();
                #region push into main table
                _oPdfPCell = new PdfPCell(oTopTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            #region TOTAL
            #region initialize table
            oTopTable = new PdfPTable(10);
            oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5  
                                            12f,//6    
                                            12f,//7  
                                            12f,//8  
                                            15f,//9 
                                            12f //10
                                           });
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan = 3; // _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFNOrderFabricReceives.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFNOrderFabricReceives.Sum(x => x.QtyInMeter).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFNOrderFabricReceives.Sum(x => x.QtyTrIn).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFNOrderFabricReceives.Sum(x => x.QtyTrOut).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFNOrderFabricReceives.Sum(x => x.QtyReturn).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFNOrderFabricReceives.Sum(x => x.QtyCon).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFNOrderFabricReceives.Sum(x => x.YetToTransferQty).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
            #endregion
        }
        public void FabricReceiveTable()
        {
            PdfPTable oTopTable = new PdfPTable(5);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5    
                                           });

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric Receive", _oFontStyle)); _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Colspan = 6; //_oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region HEADER
            #region initialize table
            oTopTable = new PdfPTable(5);
            oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5    
                                           });
            #endregion
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Roll No", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric Source", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric From", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Receive Qty(Yard)", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            #region DATA
          
            nSL = 1;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (FNOrderFabricTransfer oItem in _oInQtys)
            {
                #region initialize table
                oTopTable = new PdfPTable(5);
                oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5  
                                           });
                #endregion
                _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Lot_From, _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Fabric_From, _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Dispo_From, _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                nSL++;
                oTopTable.CompleteRow();
                #region push into main table
                _oPdfPCell = new PdfPCell(oTopTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }

            #region TOTAL
            #region initialize table
            oTopTable = new PdfPTable(5);
            oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5  
                                           });
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan = 4; // _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oInQtys.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
            #endregion
        }
        public void FabricOutTable()
        {
            PdfPTable oTopTable = new PdfPTable(5);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5    
                                           });

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric Send/Out", _oFontStyle)); _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Colspan = 6; //_oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region HEADER
            #region initialize table
            oTopTable = new PdfPTable(5);
            oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5    
                                           });
            #endregion
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Roll No", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric Source", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric From", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Out Qty(Yard)", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            #region DATA
            nSL = 1;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (FNOrderFabricTransfer oItem in _oOutQtys)
            {
                #region initialize table
                oTopTable = new PdfPTable(5);
                oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5  
                                           });
                #endregion
                _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Lot_To, _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Fabric_From, _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Dispo_To, _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                nSL++;
                oTopTable.CompleteRow();
                #region push into main table
                _oPdfPCell = new PdfPCell(oTopTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }

            #region TOTAL
            #region initialize table
            oTopTable = new PdfPTable(5);
            oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5  
                                           });
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan = 4; // _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oOutQtys.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
            #endregion
        }
        public void FabricReturnTable()
        {
            PdfPTable oTopTable = new PdfPTable(5);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5    
                                           });

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric Return", _oFontStyle)); _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Colspan = 6; //_oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region HEADER
            #region initialize table
            oTopTable = new PdfPTable(5);
            oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5    
                                           });
            #endregion
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Roll No", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric Source", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric From", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Return Qty(Yard)", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
            #region DATA         
            nSL = 1;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (FNOrderFabricTransfer oItem in _oReturnQtys)
            {
                #region initialize table
                oTopTable = new PdfPTable(5);
                oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5  
                                           });
                #endregion
                _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Lot_To, _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Fabric_From, _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Dispo_To, _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                nSL++;
                oTopTable.CompleteRow();
                #region push into main table
                _oPdfPCell = new PdfPCell(oTopTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }

            #region TOTAL
            #region initialize table
            oTopTable = new PdfPTable(5);
            oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5  
                                           });
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan = 4; // _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oReturnQtys.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
            #endregion
        }
        public void FabricConTable()
        {
            PdfPTable oTopTable = new PdfPTable(5);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5    
                                           });

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric Consumption", _oFontStyle)); _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Colspan = 6; //_oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region HEADER
            #region initialize table
            oTopTable = new PdfPTable(5);
            oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5    
                                           });
            #endregion
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Roll No", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric Source", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric From", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Consumption Qty(Yard)", _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            #region DATA
           
            nSL = 1;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (FNOrderFabricTransfer oItem in _oConQtys)
            {
                #region initialize table
                oTopTable = new PdfPTable(5);
                oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5  
                                           });
                #endregion
                _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Lot_From, _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Fabric_From, _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Dispo_From, _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                nSL++;
                oTopTable.CompleteRow();
                #region push into main table
                _oPdfPCell = new PdfPCell(oTopTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }

            #region TOTAL
            #region initialize table
            oTopTable = new PdfPTable(5);
            oTopTable.SetWidths(new float[] {                                             
                                            3f,//1  
                                            12f,//2  
                                            12f,//3  
                                            12f,//4   
                                            12f,//5  
                                           });
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan = 4; // _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oConQtys.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));// _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
            #endregion
        }
    }
}
