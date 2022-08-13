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
    public class rptLotStockReport
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(8);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<LotStockReport> _oLotStockReportList = new List<LotStockReport>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit;
        int _nColspan = 8;
        int _nEndHeight = 0;
        #endregion

        #region Order Sheet
        public byte[] PrepareReport(List<LotStockReport> oLotStockReportList, Company oCompany, BusinessUnit oBusinessUnit,int nTab, int nReportLayout, string sHeaderName)
        {
            _oLotStockReportList = oLotStockReportList;
            _oBusinessUnit = oBusinessUnit;
            _oCompany = oCompany;

            _nEndHeight = 720;
            if (nTab == 1)
                _nEndHeight = 520;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //842*595
            _oDocument.SetMargins(30f, 30f, 45f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            if (nTab == 1)
            {
                _oPdfPTable.SetWidths(new float[] { 
                                                    40f,  //0
                                                    220f, //1 
                                                    70f,  //2
                                                    90f,  //3
                                                    70f,  //4
                                                    70f,  //5                                                
                                                    70f,  //6                                               
                                                    127+85f,  //7
                                                    //85f,  //8
                                             });
                _nColspan = 8;
            }
            else 
            {
                _oPdfPTable = new PdfPTable(5);
                _oPdfPTable.WidthPercentage = 100;
                _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4); //595*842
                _oPdfPTable.SetWidths(new float[] { 
                                                    40f,  //0
                                                    220f, //1 
                                                    100f,  //2
                                                    100f,  //3
                                                    130f,  //4
                                                    //50f,  //5                                                
                                                    //50f,  //6                                               
                                                    //85f,  //7
                                                    //85f,  //8
                                             });
                _nColspan = 5;
            }
            #endregion

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();


            this.PrintHeader(sHeaderName);

            this.PrintBody(nReportLayout, nTab); // 1: CategoryWise -- 2: CategoryWithBaseWise 

            //_oPdfPTable.HeaderRows = 5;
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
        private void PrintData(LotStockReport oItem,int nSL,int nTab) 
        {
            this.AddCell(nSL.ToString(), "RIGHT", false);
            this.AddCell(oItem.ProductName, "LEFT", false);
            if (nTab == 1)
            {
                this.AddCell(oItem.LotNo, "LEFT", false);
                this.AddCell(oItem.LCNo, "LEFT", false);
                this.AddCell(oItem.InvoiceNo, "LEFT", false);
            }
            if (oItem.Qty_Total > 0)
                this.AddCell(Global.MillionFormat(oItem.Qty_Total), "RIGHT", false);
            else
                this.AddCell("-", "RIGHT", false);

            if (oItem.Balance > 0)
                this.AddCell(Global.MillionFormat(oItem.Balance), "RIGHT", false);
            else
                this.AddCell("-", "RIGHT", false);

            if (nTab == 2)
            {
                this.AddCell(oItem.OperationUnitName, "LEFT", false);
            }
            else
                this.AddCell(oItem.ContractorName, "LEFT", false);
            _oPdfPTable.CompleteRow();
        }
        private void PrintDataPBaseWise(ref int nCount,int nTab, List<LotStockReport> oLotStockReportList)
        {
            #region Data
            int nSL = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (LotStockReport oItem in oLotStockReportList)
            {
                nSL++; nCount++;
                PrintData(oItem, nSL, nTab);
                float nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);


                if (nUsagesHeight > _nEndHeight)
                {
                    nUsagesHeight = 0;
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    this.PrintRow(oItem.ProductName_Base + " [" + oItem.CategoryName + "]", "L", 1);
                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD);
                    PrintHeaderColumn(nTab);
                }
            }
            #region Sub Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            if (nTab == 1)
                this.AddCell("Sub Total", "RIGHT", 5, 0);
            else
                this.AddCell("Sub Total", "RIGHT", 2, 0);

            double nSubValue = oLotStockReportList.Sum(x => x.Qty_Total);
            if (nSubValue > 0)
                this.AddCell(Global.MillionFormat(nSubValue), "RIGHT", false); //--GrandTotal
            else
                this.AddCell("-", "RIGHT", false);

            nSubValue = oLotStockReportList.Sum(x => x.Balance);
            if (nSubValue > 0)
                this.AddCell(Global.MillionFormat(nSubValue), "RIGHT", false); //--GrandTotal
            else
                this.AddCell("-", "RIGHT", false);

            this.AddCell("", "RIGHT", false);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
        }
        private void PrintDataPCategoryWise(ref int nCount, int nTab, List<LotStockReport> oLotStockReportList)
        {
            #region Data
            int nSL = 0;

            
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (LotStockReport oItem in oLotStockReportList)
            {
                nSL++; nCount++;
                PrintData(oItem, nSL, nTab);

                float nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                if (nUsagesHeight > _nEndHeight)
                {
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    this.PrintRow(oItem.CategoryName, "L", 1);
                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD);
                    PrintHeaderColumn(nTab);
                }
            }

            #region Sub Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            if (nTab == 1)
                this.AddCell("Sub Total", "RIGHT", 5, 0);
            else
                this.AddCell("Sub Total", "RIGHT", 2, 0);


            double nSubValue = oLotStockReportList.Sum(x => x.Qty_Total);
            if (nSubValue > 0)
                this.AddCell(Global.MillionFormat(nSubValue), "RIGHT", false); //--GrandTotal
            else
                this.AddCell("-", "RIGHT", false);

            nSubValue = oLotStockReportList.Sum(x => x.Balance);
            if (nSubValue > 0)
                this.AddCell(Global.MillionFormat(nSubValue), "RIGHT", false); //--GrandTotal
            else
                this.AddCell("-", "RIGHT", false);

            this.AddCell("", "RIGHT", false);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }
        private void PrintBody(int nReportLayout, int nTab)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            this.PrintRow("As on "+DateTime.Now.ToString("dd/MM/yyyy")+"  Reporting Date "+DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"),"",0);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            #region Data
            int nCount = 0, nProductCategoryID = 0, nProductBaseID=0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

            if (nReportLayout == 2)
            {
                #region Base & Category Wise
                foreach (LotStockReport oItem in _oLotStockReportList)
                {
                    if (oItem.ProductBaseID != nProductBaseID)
                    {
                        nCount += 3;
                        #region PrintData
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                        this.PrintRow("", "", 0);
                        this.PrintRow(oItem.ProductName_Base+" ["+oItem.CategoryName+"]", "L", 1);

                        _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD);
                        PrintHeaderColumn(nTab);

                        _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
                        this.PrintDataPBaseWise(ref nCount,nTab, _oLotStockReportList.Where(x => x.ProductBaseID == oItem.ProductBaseID).ToList());
                        #endregion
                    }
                    nProductBaseID = oItem.ProductBaseID;
                }
                #endregion
            }
            else 
            {
                #region Category Wise
                foreach (LotStockReport oItem in _oLotStockReportList)
                {
                    if (oItem.ProductCategoryID != nProductCategoryID) 
                    {
                        nCount +=3;
                        #region PrintData
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                        this.PrintRow("", "", 0);
                        this.PrintRow(oItem.CategoryName, "L", 1);

                        _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD);
                        PrintHeaderColumn(nTab);

                        _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
                        this.PrintDataPCategoryWise(ref nCount,nTab, _oLotStockReportList.Where(x => x.ProductCategoryID == oItem.ProductCategoryID).ToList());
                        #endregion
                    }
                    nProductCategoryID = oItem.ProductCategoryID;
                }
                #endregion
            }
            #endregion

            #region Total
            this.PrintRow("", "", 0);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            if(nTab==1)
                this.AddCell("Total", "RIGHT", 5,0);
            else
                this.AddCell("Total", "RIGHT", 2, 0);

            double nLCQTy = _oLotStockReportList.Sum(x => x.Qty_Total);
            if (nLCQTy > 0)
                this.AddCell(Global.MillionFormat(nLCQTy), "RIGHT", false); //--GrandTotal
            else
                this.AddCell("-", "RIGHT", false);

            nLCQTy = _oLotStockReportList.Sum(x => x.Balance);
            if (nLCQTy > 0)
                this.AddCell(Global.MillionFormat(nLCQTy), "RIGHT", false); //--GrandTotal
            else
                this.AddCell("-", "RIGHT", false);

            this.AddCell("", "RIGHT", false);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintHeaderColumn(int nTab) 
        {
            #region HEADER
            this.AddCell("SL#", "CENTER", true);
            this.AddCell("Product Name", "CENTER", true);

            if (nTab == 1) 
            {
                this.AddCell("Lot No", "CENTER", true);
                this.AddCell("L/C No", "CENTER", true);
                this.AddCell("Invoice No", "CENTER", true);
            }
            this.AddCell("Total Qty", "CENTER", true);
            this.AddCell("Balance", "CENTER", true);

            if (nTab == 1)
            {
                this.AddCell("Contractor", "CENTER", true);
            }else
                this.AddCell("Store", "CENTER", true);
            //this.AddCell("Store", "CENTER", true);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        #endregion

        #region PDF HELPER
        public static float CalculatePdfPTableHeight(PdfPTable table)
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
        public void AddCell(string sHeader, string sAlignment, bool isGray)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;//Default
            if (sAlignment.Equals("LEFT"))
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment.Equals("RIGHT"))
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment.Equals("CENTER"))
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            if(isGray)
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void PrintRow(string sHeader, string sAlign, int nBorder)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            if (sAlign.Equals("R"))
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; 
            if (sAlign.Equals("L"))
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;

            if (nBorder == 0)
                _oPdfPCell.Border = 0;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColspan;
            _oPdfPCell.FixedHeight = 10f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion

        #endregion
    }
}