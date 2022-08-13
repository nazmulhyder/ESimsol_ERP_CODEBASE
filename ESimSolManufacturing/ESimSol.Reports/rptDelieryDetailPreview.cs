using System;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Reports;
using ESimSol;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;



namespace ESimSol.Reports
{
    public class rptDelieryDetailPreview
    {
        #region Declaration
        int _nTotalColumn = 7;
        float _nAlreadyUsagesHeight = 30f;
        float _nFullPageHeight = 545f;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(7);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<DUDeliveryChallanRegister> _DUDeliveryChallanRegisters = new List<DUDeliveryChallanRegister>();
        List<DUReturnChallanDetail> _DUReturnChallanDetails = new List<DUReturnChallanDetail>();
        List<DUReturnChallan> _DUReturnChallans = new List<DUReturnChallan>();
        List<DULotDistribution> _oDULotDistributions = new List<DULotDistribution>();
        List<DULotDistribution> _oDULotDistributionsDes = new List<DULotDistribution>();
        DUOrderTracker _oDUOrderTracker = new DUOrderTracker();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        string _sMessage = "";
        int nCount = 0;
        double TotalQty = 0;
        #endregion
        public byte[] PrepareReport(DUOrderTracker DUOrderTracker, List<DUDeliveryChallanRegister> DUDeliveryChallanRegisters, List<DUReturnChallanDetail> DUReturnChallanDetails, List<DUReturnChallan> DUReturnChallans, List<DULotDistribution> oDULotDistributions, BusinessUnit oBusinessUnit, Company oCompany, string sMessage, List<DULotDistribution> oDULotDistributionsDes)
        {
            _oBusinessUnit = oBusinessUnit;
            _oDUOrderTracker = DUOrderTracker;
            _DUDeliveryChallanRegisters = DUDeliveryChallanRegisters;
            _DUReturnChallanDetails = DUReturnChallanDetails;
            _DUReturnChallans = DUReturnChallans;
            _oDULotDistributions = oDULotDistributions;
            _oDULotDistributionsDes = oDULotDistributionsDes;
            _oCompany = oCompany;
            _sMessage = sMessage;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(25f, 25f, 50f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.nFontSize = 7;
            PdfWriter.PageEvent = PageEventHandler;
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                100f,                                                
                                                100f, 
                                                150f,  
                                                150f,  
                                                150f,  
                                                150f, 
                                                100f                                             
                                                });
            #endregion
            this.PrintHeader();
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

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Colspan = 7; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 7;
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f;_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion
            #endregion

     
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            //  1st row
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.BOLD); 
            _oPdfPCell = new PdfPCell(new Phrase("Order No:", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" " + _oDUOrderTracker.OrderNo, _oFontStyle));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell); 

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.BOLD); 
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell); 


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.BOLD); 
            _oPdfPCell = new PdfPCell(new Phrase("Order Date: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; 
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell); 

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL); 
            _oPdfPCell = new PdfPCell(new Phrase(" " + _oDUOrderTracker.OrderDateSt, _oFontStyle));
            _oPdfPCell.Colspan = 2; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            //2nd row
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.BOLD); 
            _oPdfPCell = new PdfPCell(new Phrase("Customer:", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; 
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL); 
            _oPdfPCell = new PdfPCell(new Phrase(" " + _oDUOrderTracker.ContractorName, _oFontStyle));
            _oPdfPCell.Colspan = 2; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell); 

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell); 


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.BOLD); 
            _oPdfPCell = new PdfPCell(new Phrase("Buyer: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell); 

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" " + _oDUOrderTracker.BuyerName, _oFontStyle));
            _oPdfPCell.Colspan = 2; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //3nd row
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.BOLD); 
            _oPdfPCell = new PdfPCell(new Phrase("Yarn Name:", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; 
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL); 
            _oPdfPCell = new PdfPCell(new Phrase(" " + _oDUOrderTracker.ProductName, _oFontStyle));
            _oPdfPCell.Colspan = 2; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.BOLD); 
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; 
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.BOLD); 
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; 
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL); 
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 2; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
            _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL); 
            _oPdfPCell = new PdfPCell(new Phrase(" " + _oDUOrderTracker.YarnCount, _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            if (_DUDeliveryChallanRegisters.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Details Of Delivery Challan are as below:", _oFontStyle)); _oPdfPCell.Colspan = 7;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #region 2nd table
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Challan Date", _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Lot/Batch No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                nCount = 0;
                TotalQty = 0;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                foreach (DUDeliveryChallanRegister oItem in _DUDeliveryChallanRegisters)
                {
                    nCount++;
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ChallanDateSt, _oFontStyle)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ChallanNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString(), _oFontStyle)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    TotalQty += oItem.Qty;
                    _oPdfPTable.CompleteRow();


                }
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyle)); _oPdfPCell.Colspan = 5;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(TotalQty.ToString(), _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #region Space
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
                _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" " + _oDUOrderTracker.YarnCount, _oFontStyle));
                _oPdfPCell.Colspan = 7; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
                #endregion
            }



            if (_DUReturnChallanDetails.Count > 0)
            {
                // third table
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Details Of Return Challan are as below:", _oFontStyle)); _oPdfPCell.Colspan = 7;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #region 3nd table
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Challan Date", _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Lot/Batch No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                nCount = 0;
                TotalQty = 0;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                foreach (DUReturnChallanDetail oItem in _DUReturnChallanDetails)
                {
                    nCount++;
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    foreach (DUReturnChallan _oItem in _DUReturnChallans)
                    {
                        if (_oItem.DUReturnChallanID == oItem.DUReturnChallanID)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(_oItem.ReturnDateSt, _oFontStyle)); _oPdfPCell.Colspan = 2;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(_oItem.DUReturnChallanNo, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        }

                    }
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString(), _oFontStyle)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    TotalQty += oItem.Qty;
                    _oPdfPTable.CompleteRow();


                }
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyle)); _oPdfPCell.Colspan = 5;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(TotalQty.ToString(), _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #region Space
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f);
                _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" " + _oDUOrderTracker.YarnCount, _oFontStyle));
                _oPdfPCell.Colspan = 7; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
                #endregion

            }



            if (_oDULotDistributions.Count > 0)
            {
                // 4th table
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Details Of Lot Distribution are as below:", _oFontStyle)); _oPdfPCell.Colspan = 7;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #region 4nd table
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Date Time", _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Type", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                nCount = 0;
                TotalQty = 0;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                foreach (DULotDistribution oItem in _oDULotDistributions)
                {
                    nCount++;
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.DBServerDateTimeST, _oFontStyle)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Receive", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    if (oItem.Qty < 0) { oItem.Qty = 0; }
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString(), _oFontStyle)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    TotalQty += oItem.Qty;
                    _oPdfPTable.CompleteRow();


                }
                foreach (DULotDistribution oItem in _oDULotDistributionsDes)
                {
                    nCount++;
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.DBServerDateTimeST, _oFontStyle)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Issue", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    if (oItem.Qty < 0) { oItem.Qty = 0; }

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString(), _oFontStyle)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    TotalQty += oItem.Qty;
                    _oPdfPTable.CompleteRow();


                }
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyle)); _oPdfPCell.Colspan = 5;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(TotalQty.ToString(), _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
           
        }
        #endregion
    }
}
