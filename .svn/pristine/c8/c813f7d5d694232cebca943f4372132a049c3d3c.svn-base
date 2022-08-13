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
    public class rptFabricFSStatusPreview
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);//number of columns
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        FabricFSStatus _oFabricFSStatus = new FabricFSStatus();
        List<FabricFSStatus> _oFabricFSStatuss = new List<FabricFSStatus>();
        Company _oCompany = new Company();
        string _sMessage = "";
        double nNoOfDispo = 0;
        double nTotalOrderQty = 0;
        double nTotalQty = 0;         
        #endregion
        public byte[] PrepareReport(List<FabricFSStatus> FabricFSStatuss, Company oCompany, int ReportType, string sMSG)
        {
            _oFabricFSStatuss = FabricFSStatuss;
            _oCompany = oCompany;
            _sMessage = sMSG;
            #region Page Setup
            _oDocument = new Document(PageSize.LEGAL, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(40f, 40f, 20f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion
            this.PrintHeader();
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_sMessage, _oFontStyle)); _oPdfPCell.Colspan = 8;
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Reporting Time: "+DateTime.Now.ToString("dd MMM yyyy hh:mm"), _oFontStyle)); _oPdfPCell.Colspan = 8;
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
         
        }

        #region Report Body
        private void PrintBody()
        {
            GetTopTable();
        }
        #endregion
        private void GetTopTable()
        {
            PdfPTable oTopTable = new PdfPTable(9);
             oTopTable.WidthPercentage = 100;
                oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oTopTable.SetWidths(new float[] {                                             
                                                 2f,  //sl                        
                                                8f,  //po
                                                12f,  //buyer                                                
                                                 6f, //no of dispo
                                                8f,  //order QTY   
                                                8f,  //DELIVERY QTY        
                                                7f, //lead time
                                                8f,  //delivery date                                                
                                                10f,  //remarks                                                                                                                                                                                                
                                            });
            var nCount = 1;
          
            double Qty = 0; double DCQty = 0;

            var data = _oFabricFSStatuss.GroupBy(x => new { x.OrderType, x.OrderName }, (key, grp) => new
            {
                HeaderName = key.OrderName, //unique dt
                FreshResults = grp.Where(x=>x.ReviseNo==0).ToList(), //IsFresh
                ReviseResults = grp.Where(x => x.ReviseNo == 1).ToList(), //IsRevise
                StatusCancelResults = grp.Where(x => x.CurrentStatus == 5).ToList()

            });
            foreach (var oData in data)
            {
                #region initialize
                oTopTable = new PdfPTable(9);
                oTopTable.SetWidths(new float[] {                                             
                                                 2f,  //sl                        
                                                8f,  //po
                                                12f,  //buyer                                                
                                                 6f, //no of dispo
                                                8f,  //order QTY   
                                                8f,  //DELIVERY QTY        
                                                7f, //lead time
                                                8f,  //delivery date                                                
                                                10f,  //remarks          
                                            });
                #endregion
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(oData.HeaderName, _oFontStyle)); _oPdfPCell.Colspan = 9;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #region HEADER
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("PO NO", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("No Of Dispo", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Delivery Qty", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Lead Time", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Delivery Date", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
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

                #region FRESH Table
                if (oData.FreshResults.Count > 0)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    #region initialize
                    oTopTable = new PdfPTable(9);
                    oTopTable.SetWidths(new float[] {                                             
                                                 2f,  //sl                        
                                                8f,  //po
                                                12f,  //buyer                                                
                                                 6f, //no of dispo
                                                8f,  //order QTY   
                                                8f,  //DELIVERY QTY        
                                                7f, //lead time
                                                8f,  //delivery date                                                
                                                10f,  //remarks          
                                            });
                    #endregion
                    _oPdfPCell = new PdfPCell(new Phrase("Fresh", _oFontStyle)); _oPdfPCell.Colspan = 9;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #region push into main table
                    _oPdfPCell = new PdfPCell(oTopTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                }

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                nCount = 1;
                foreach (var oItem in oData.FreshResults)
                {
                    #region initialize
                    oTopTable = new PdfPTable(9);
                    oTopTable.SetWidths(new float[] {                                             
                                                 2f,  //sl                        
                                                8f,  //po
                                                12f,  //buyer                                                
                                                 6f, //no of dispo
                                                8f,  //order QTY   
                                                8f,  //DELIVERY QTY        
                                                7f, //lead time
                                                8f,  //delivery date                                                
                                                10f,  //remarks          
                                            });
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.SCNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.NoofDispo.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.QtyDC.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.TimeSpan, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AppDCDateInString, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);               

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.RemarksST, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    nCount++;
 
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
                #endregion
                if (oData.FreshResults.Count > 0)
                {
                    #region SUB TOTAL
                    #region initialize
                    oTopTable = new PdfPTable(9);
                    oTopTable.SetWidths(new float[] {                                             
                                                 2f,  //sl                        
                                                8f,  //po
                                                12f,  //buyer                                                
                                                 6f, //no of dispo
                                                8f,  //order QTY   
                                                8f,  //DELIVERY QTY        
                                                7f, //lead time
                                                8f,  //delivery date                                                
                                                10f,  //remarks          
                                            });
                    #endregion
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Sub Total", _oFontStyle)); _oPdfPCell.Colspan = 3;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.FreshResults.Sum(x => x.NoofDispo).ToString("#,##0;(#,##0)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.FreshResults.Sum(x => x.Qty).ToString("#,##0;(#,##0)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.FreshResults.Sum(x => x.QtyDC).ToString("#,##0;(#,##0)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

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
              
                #region REVISE TABLE
                if (oData.ReviseResults.Count > 0)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                     #region initialize
                    oTopTable = new PdfPTable(9);
                    oTopTable.SetWidths(new float[] {                                             
                                                 2f,  //sl                        
                                                8f,  //po
                                                12f,  //buyer                                                
                                                 6f, //no of dispo
                                                8f,  //order QTY   
                                                8f,  //DELIVERY QTY        
                                                7f, //lead time
                                                8f,  //delivery date                                                
                                                10f,  //remarks          
                                            });
                    #endregion
                    _oPdfPCell = new PdfPCell(new Phrase("Revise", _oFontStyle)); _oPdfPCell.Colspan = 9;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #region push into main table
                    _oPdfPCell = new PdfPCell(oTopTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                }

                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                nCount = 1;
                foreach (var oItem in oData.ReviseResults)
                {
                    #region initialize
                    oTopTable = new PdfPTable(9);
                    oTopTable.SetWidths(new float[] {                                             
                                                 2f,  //sl                        
                                                8f,  //po
                                                12f,  //buyer                                                
                                                 6f, //no of dispo
                                                8f,  //order QTY   
                                                8f,  //DELIVERY QTY        
                                                7f, //lead time
                                                8f,  //delivery date                                                
                                                10f,  //remarks          
                                            });
                    #endregion

                   _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.SCNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.NoofDispo.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.QtyDC.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.TimeSpan, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AppDCDateInString, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);                 

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.RemarksST, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    nCount++;

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
                #endregion

                if (oData.ReviseResults.Count > 0)
                {
                    #region SUB TOTAL
                    #region initialize
                    oTopTable = new PdfPTable(9);
                    oTopTable.SetWidths(new float[] {                                             
                                                 2f,  //sl                        
                                                8f,  //po
                                                12f,  //buyer                                                
                                                 6f, //no of dispo
                                                8f,  //order QTY   
                                                8f,  //DELIVERY QTY        
                                                7f, //lead time
                                                8f,  //delivery date                                                
                                                10f,  //remarks          
                                            });
                    #endregion
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Sub Total", _oFontStyle)); _oPdfPCell.Colspan = 3;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.ReviseResults.Sum(x => x.NoofDispo).ToString("#,##0;(#,##0)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.ReviseResults.Sum(x => x.Qty).ToString("#,##0;(#,##0)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.ReviseResults.Sum(x => x.QtyDC).ToString("#,##0;(#,##0)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

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

                #region STATUS/CANCEL
                if (oData.StatusCancelResults.Count > 0)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    #region initialize
                    oTopTable = new PdfPTable(9);
                    oTopTable.SetWidths(new float[] {                                             
                                                 2f,  //sl                        
                                                8f,  //po
                                                12f,  //buyer                                                
                                                 6f, //no of dispo
                                                8f,  //order QTY   
                                                8f,  //DELIVERY QTY        
                                                7f, //lead time
                                                8f,  //delivery date                                                
                                                10f,  //remarks          
                                            });
                    #endregion
                    _oPdfPCell = new PdfPCell(new Phrase("Cancel", _oFontStyle)); _oPdfPCell.Colspan = 9;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #region push into main table
                    _oPdfPCell = new PdfPCell(oTopTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                }           
                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                nCount = 1;
                foreach (var oItem in oData.StatusCancelResults)
                {
                    #region initialize
                    oTopTable = new PdfPTable(9);
                    oTopTable.SetWidths(new float[] {                                             
                                                 2f,  //sl                        
                                                8f,  //po
                                                12f,  //buyer                                                
                                                 6f, //no of dispo
                                                8f,  //order QTY   
                                                8f,  //DELIVERY QTY        
                                                7f, //lead time
                                                8f,  //delivery date                                                
                                                10f,  //remarks          
                                            });
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.SCNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.NoofDispo.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.QtyDC.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.TimeSpan, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AppDCDateInString, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.RemarksST, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    nCount++;

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
                #endregion

                if (oData.StatusCancelResults.Count > 0)
                {
                    #region SUB TOTAL
                    #region initialize
                    oTopTable = new PdfPTable(9);
                    oTopTable.SetWidths(new float[] {                                             
                                                 2f,  //sl                        
                                                8f,  //po
                                                12f,  //buyer                                                
                                                 6f, //no of dispo
                                                8f,  //order QTY   
                                                8f,  //DELIVERY QTY        
                                                7f, //lead time
                                                8f,  //delivery date                                                
                                                10f,  //remarks          
                                            });
                    #endregion
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Sub Total", _oFontStyle)); _oPdfPCell.Colspan = 3;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.StatusCancelResults.Sum(x => x.NoofDispo).ToString("#,##0;(#,##0)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.StatusCancelResults.Sum(x => x.Qty).ToString("#,##0;(#,##0)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oData.StatusCancelResults.Sum(x => x.QtyDC).ToString("#,##0;(#,##0)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

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
                nNoOfDispo = oData.FreshResults.Sum(x => x.NoofDispo)+oData.ReviseResults.Sum(x => x.NoofDispo)+oData.StatusCancelResults.Sum(x => x.NoofDispo);
                nTotalQty = oData.FreshResults.Sum(x => x.QtyDC)+oData.ReviseResults.Sum(x => x.QtyDC)+oData.StatusCancelResults.Sum(x => x.QtyDC);
                nTotalOrderQty = oData.FreshResults.Sum(x => x.Qty) + oData.ReviseResults.Sum(x => x.Qty) + oData.StatusCancelResults.Sum(x => x.Qty);

                #region TOTAL
                #region initialize
                oTopTable = new PdfPTable(9);
                oTopTable.SetWidths(new float[] {                                             
                                                 2f,  //sl                        
                                                8f,  //po
                                                12f,  //buyer                                                
                                                 6f, //no of dispo
                                                8f,  //order QTY   
                                                8f,  //DELIVERY QTY        
                                                7f, //lead time
                                                8f,  //delivery date                                                
                                                10f,  //remarks          
                                            });
                #endregion
                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total", _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nNoOfDispo.ToString("#,##0;(#,##0)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTotalOrderQty.ToString("#,##0;(#,##0)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTotalQty.ToString("#,##0;(#,##0)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

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
        }
    }
}
