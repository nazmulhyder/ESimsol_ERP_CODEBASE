using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Utility;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace ESimSol.Reports
{
    public class rptSalesOrderMultipleType
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);//number of columns
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        public List<SalesReport> _SendingSalesReports = new List<SalesReport>();
        public  List<SalesReport> _ReceivingSalesReports = new List<SalesReport>();
        public List<MktSaleTarget> _DispoMktSaleTargets = new List<MktSaleTarget>();
        public SalesReport _SalesReport = new SalesReport();
        Company _oCompany = new Company();
        string _sMessage = "";
        double nTotalAllocation = 0;
        string AllocationHeader1 = "", AllocationHeader2, AllocationHeader3 = "";
        BusinessUnit _oBusinessUnit;
        #endregion
        public byte[] PrepareReport(List<SalesReport> SendingSalesReports, List<SalesReport> ReceivingSalesReports,List<MktSaleTarget> DispoMktSaleTargets,SalesReport oSalesReport, Company oCompany,BusinessUnit oBusinessUnit,string sMSG)
        {
            _SendingSalesReports = SendingSalesReports;
            _ReceivingSalesReports = ReceivingSalesReports;
            _DispoMktSaleTargets = DispoMktSaleTargets;
            _SalesReport = oSalesReport;
            _oBusinessUnit = oBusinessUnit;
            #region Dispo wise allocation header
            AllocationHeader1 = Convert.ToString(_SalesReport.AllocationHeader.Split('~')[0]);
            AllocationHeader2 = Convert.ToString(_SalesReport.AllocationHeader.Split('~')[1]);
            AllocationHeader3 = Convert.ToString(_SalesReport.AllocationHeader.Split('~')[2]);
            #endregion
            _oCompany = oCompany;
            #region Page Setup
            _oDocument = new Document(PageSize.LEGAL, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.LEGAL.Rotate());
            _oDocument.SetMargins(20f, 20f, 20f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            //PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion
            this.PrintHeader(sMSG);
            this.PrintBody();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

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

            if (_SalesReport.BUID > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.BusinessUnitType.ToString(), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                 oPdfPTable.CompleteRow();
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            if (_SalesReport.ViewType == 2)
                sHeaderName += " " + _SalesReport.Year;
            else
                sHeaderName += " " + (_SalesReport.Year - 11) + " To " + _SalesReport.Year;

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.Colspan = 29; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Reporting Time: " + Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy hh:mm tt"), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Colspan = 29; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
            #region Blank Space
            //_oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
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
            PdfPTable oTopTable = new PdfPTable(29);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                                             
                                                5f,//1  
                                                25f,//2  
                                                15f,//3  
                                                10f,//4  
                                                12f,//5  
                                                10f,//6  
                                                12f,//7  
                                                10f,//8  
                                                12f,//9  
                                                10f,//10  
                                                12f,//11  
                                                10f,//12  
                                                12f,//13                                            
                                                10f,//14
                                                12f,//15                                             
                                                10f,//16
                                                12f,//17                                                  
                                                10f,//18
                                                12f,//19                                                
                                                10f,//20                                                 
                                                12f,//21 
                                                10f,//22 
                                                12f,//23 
                                                10f,//24 
                                                12f,//25  
                                                10f,//26 
                                                12f,//27  
                                                10f,//28
                                                12f,//29  
                                            });

            //Header...
            #region Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Mkt Person", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oTopTable.AddCell(_oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase(this.AllocationHeader1, _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oTopTable.AddCell(_oPdfPCell);


            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Dispo", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oTopTable.AddCell(_oPdfPCell);

            if (_SalesReport.ViewType == 2) //MonthWise
            {
                string[] MonthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                foreach (string month in MonthNames)
                {
                    if (!string.IsNullOrEmpty(month))
                    {
                        string Month = month.Substring(0, 3).ToUpper();
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase(Month + " " + _SalesReport.Year.ToString().Substring(2, 2), _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oTopTable.AddCell(_oPdfPCell);
                    }
                }
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();

            for (int i = 0; i <= 12; i++)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(this.AllocationHeader2, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(this.AllocationHeader3, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oTopTable.AddCell(_oPdfPCell);

                //_oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase("Send", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oTopTable.AddCell(_oPdfPCell);

                //_oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase("Receive", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oTopTable.AddCell(_oPdfPCell);

            }
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

            List<SalesReport> _distSendingSalesReport = new List<SalesReport>();
            List<SalesReport> _distReceivingSalesReport = new List<SalesReport>();
            List<MktSaleTarget> _distMktSaleTarget = new List<MktSaleTarget>();
            _distSendingSalesReport = _SendingSalesReports.GroupBy(x => x.RefID).Select(g => g.First()).ToList();
            _distReceivingSalesReport = _ReceivingSalesReports.GroupBy(x => x.RefID).Select(g => g.First()).ToList();
            _distMktSaleTarget = _DispoMktSaleTargets.GroupBy(x => x.MarketingAccountID).Select(g => g.First()).ToList();

            #region DATA
            string tempGroupName = ""; int nCount = 1;
            int allocationValue = 0;
            foreach (SalesReport oItem in _distSendingSalesReport.OrderBy(x=>x.GroupName))
            {
                #region Table Initialize
                oTopTable = new PdfPTable(29);
                oTopTable.SetWidths(new float[] {                                             
                                                5f,//1  
                                                25f,//2  
                                                10f,//3  
                                                10f,//4  
                                                15f,//5  
                                                10f,//6  
                                                15f,//7  
                                                10f,//8  
                                                15f,//9  
                                                10f,//10  
                                                15f,//11  
                                                10f,//12  
                                                15f,//13                                            
                                                10f,//14
                                                15f,//15                                             
                                                10f,//16
                                                15f,//17                                                  
                                                10f,//18
                                                15f,//19                                                
                                                10f,//20                                                 
                                                15f,//21 
                                                10f,//22 
                                                15f,//23 
                                                10f,//24 
                                                15f,//25  
                                                10f,//26 
                                                15f,//27  
                                                10f,//28
                                                15f,//29  
                                            });
                #endregion
                    if (oItem.GroupName != "" && tempGroupName != oItem.GroupName)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase(oItem.GroupName, _oFontStyle)); _oPdfPCell.Colspan = 29; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                        oTopTable.CompleteRow();                      
                    }
       
                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(Convert.ToInt32(nCount).ToString(), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(oItem.RefName, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    bool isMatch = false; allocationValue = 0;
                    foreach (MktSaleTarget oItem1 in _distMktSaleTarget)
                    {
                        if (oItem1.MarketingAccountID == oItem.RefID)
                        {
                            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(Convert.ToInt32(oItem1.Value).ToString(), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                            isMatch = true;
                            if(oItem1.Value !=null)
                            {
                                 allocationValue = Convert.ToInt32(oItem1.Value);
                            }
                            else
                            {
                                allocationValue = 0;
                            }
                           
                            break;
                        }
                    }
                    if (!isMatch)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    }

                    string oGrpName = oItem.GroupName;
                    for (int i = 1; i <= 12; i++)
                    {
                        string nQty = "";
                        nQty = Global.MillionFormat_Round(_SendingSalesReports.Where(x => x.Month == i && x.RefID == oItem.RefID).Select(x => x.Value).FirstOrDefault());
                        var sendValue = nQty.Contains("-") ? "-" : nQty;
                        sendValue = (sendValue.Contains(".00")) ? sendValue.Replace(".00", "") : sendValue;
                       // allocation wise color gray                       
                        if (sendValue != "-" &&  allocationValue > 0 && Convert.ToInt32(sendValue) > allocationValue && oGrpName == oItem.GroupName)
                        {
                            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(sendValue.ToString(), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
                            
                        }
                        else
                        {
                            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(sendValue, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                        }

                        oGrpName = oItem.GroupName;
                        string nQty2 = "";
                        nQty2 = Global.MillionFormat_Round(_ReceivingSalesReports.Where(x => x.Month == i && x.RefID == oItem.RefID).Select(x => x.Value).FirstOrDefault());
                        var rcvValue = nQty2.Contains("-") ? "-" : nQty2;
                        rcvValue = (rcvValue.Contains(".00")) ? rcvValue.Replace(".00", "") : rcvValue;
                        _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(rcvValue, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    }

                //Total , Sending - receiving
                    string nTotalQty = "";
                    nTotalQty = Global.MillionFormat_Round(_SendingSalesReports.Where(x => x.RefID == oItem.RefID).Sum(x => x.Value));
                    var totalSendQty = nTotalQty.Contains("-") ? "-" : nTotalQty;
                    totalSendQty = (totalSendQty.Contains(".00")) ? totalSendQty.Replace(".00", "") : totalSendQty;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(totalSendQty, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    string nTotalRcvQty = "";
                    nTotalRcvQty = Global.MillionFormat_Round(_ReceivingSalesReports.Where(x => x.RefID == oItem.RefID).Sum(x => x.Value));
                    var totalRcvQty = nTotalRcvQty.Contains("-") ? "-" : nTotalRcvQty;
                    totalRcvQty = (totalRcvQty.Contains(".00")) ? totalRcvQty.Replace(".00", "") : totalRcvQty;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(totalRcvQty, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    tempGroupName = oItem.GroupName;
                    nCount++;
                    nTotalAllocation += allocationValue;
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

            #region Total
            #region Table Initialize
            oTopTable = new PdfPTable(29);
            oTopTable.SetWidths(new float[] {                                             
                                                5f,//1  
                                                25f,//2  
                                                10f,//3  
                                                10f,//4  
                                                15f,//5  
                                                10f,//6  
                                                15f,//7  
                                                10f,//8  
                                                15f,//9  
                                                10f,//10  
                                                15f,//11  
                                                10f,//12  
                                                15f,//13                                            
                                                10f,//14
                                                15f,//15                                             
                                                10f,//16
                                                15f,//17                                                  
                                                10f,//18
                                                15f,//19                                                
                                                10f,//20                                                 
                                                15f,//21 
                                                10f,//22 
                                                15f,//23 
                                                10f,//24 
                                                15f,//25  
                                                10f,//26 
                                                15f,//27  
                                                10f,//28
                                                15f,//29  
                                            });
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase(nTotalAllocation.ToString("#,##0;(#,##0)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);  

                for (int i = 1; i <= 12; i++)
                {
                    //sending
                    double nValue = _SendingSalesReports.Where(x => x.Month == i).Sum(x => x.Value);
                    if (nValue > 0){
                        var totalSendingSalesReport = Global.MillionFormat_Round(nValue); //--MonthWiseTotal
                        totalSendingSalesReport = (totalSendingSalesReport.Contains(".00")) ? totalSendingSalesReport.Replace(".00", "") : totalSendingSalesReport;
                       _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase(totalSendingSalesReport.ToString(), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                       _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    }
                    else{
                       _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                       _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    }

                    //receiving
                    double nValue2 = _ReceivingSalesReports.Where(x => x.Month == i).Sum(x => x.Value);
                    if (nValue2 > 0){
                       var totalRcvingSalesReport = Global.MillionFormat_Round(nValue2); //--MonthWiseTotal
                       totalRcvingSalesReport = (totalRcvingSalesReport.Contains(".00")) ? totalRcvingSalesReport.Replace(".00", "") : totalRcvingSalesReport;
                       _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase(totalRcvingSalesReport.ToString(), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                       _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    }
                    else{
                       _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                       _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    }
                }
            
    
               //Sending
                double nLCValue = _SendingSalesReports.Sum(x => x.Value);
                if (nLCValue > 0){
                    var SendigGTotal = Global.MillionFormat_Round(nLCValue); //--GrandTotal 
                    SendigGTotal = (SendigGTotal.Contains(".00")) ? SendigGTotal.Replace(".00", "") : SendigGTotal;
                     _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase(SendigGTotal.ToString(), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                }
                else{
                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                      _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                }
                //rcving
                double nLCValue2 = _ReceivingSalesReports.Sum(x => x.Value);
                if (nLCValue2 > 0)
                {
                    var rcvingGTotal = Global.MillionFormat_Round(nLCValue2); //--GrandTotal 
                    rcvingGTotal = (rcvingGTotal.Contains(".00")) ? rcvingGTotal.Replace(".00", "") : rcvingGTotal;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase(rcvingGTotal.ToString(), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                }
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
