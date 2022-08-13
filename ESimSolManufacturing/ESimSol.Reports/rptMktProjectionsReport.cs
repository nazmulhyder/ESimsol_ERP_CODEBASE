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
    public class rptMktProjectionsReport
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle, _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);//number of columns
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        MktSaleTarget _oMktSaleTarget = new MktSaleTarget();
        List<MktSaleTarget> _oMktSaleTargets = new List<MktSaleTarget>();
        List<MarketingAccount> _oMarketingAccounts = new List<MarketingAccount>();
        Company _oCompany = new Company();
        string _sMessage = "";
        string MktingHead = "";
        int _nYear = 0;
        int _ViewType = 0;
        List<ExportPI> _oExportPIs = new List<ExportPI>();
        #endregion
        public byte[] PrepareReport(List<MarketingAccount> oMarketingAccounts, List<MktSaleTarget> oMktSaleTargets, string MktAccountHead, Company oCompany, int ViewType, int sYear, string sMSG, List<ExportPI> oExportPIs)
        {
            _oMktSaleTargets = oMktSaleTargets;
            _oMarketingAccounts = oMarketingAccounts;
            MktingHead = MktAccountHead;
            _oCompany = oCompany;
            _sMessage = sMSG;
            _nYear = sYear;
            _ViewType = ViewType;
            _oExportPIs = oExportPIs;

            #region Page Setup
            _oDocument = new Document(PageSize.LEGAL, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.LEGAL.Rotate());
            _oDocument.SetMargins(5f, 5f, 5f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
             //PdfWriter.GetInstance(_oDocument, _oMemoryStream);
             PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
             PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
             ESimSolFooter PageEventHandler = new ESimSolFooter();
             PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 802f });
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            if (_sMessage != "")
            {
                _oPdfPCell = new PdfPCell(new Phrase("Marketing Projection Report-" + _nYear + " (" + _sMessage + ")", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Marketing Projection Report-" +_nYear, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
           
            #endregion

        }

        #region Report Body
        private void PrintBody()
        {
            GetTeamTable();
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            GetMarketingProjectionTable();
            GetSummery();
        }
        #endregion

        private void GetTeamTable()
        {
            PdfPTable oTopTable = new PdfPTable(3);
            oTopTable.WidthPercentage = 100;
            _oDocument.SetMargins(20f, 20f, 20f, 40f);
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[]{      
                                                  2f, //sl                     
                                                 48f,//mkt head
                                                 48f, //team members                                                                                                                                                                                   
                                            });

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Team Leader", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Team Member", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
             oTopTable.CompleteRow();
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region initialize
            oTopTable = new PdfPTable(3);
            oTopTable.SetWidths(new float[]{       
                                                 2f, //sl                     
                                                 48f,//mkt head
                                                 48f, //team members                        
                                                                                                                                                                                                                                                            
                                        });
            #endregion
            _oMarketingAccounts = _oMarketingAccounts.ToList();
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Group Name: " + MktingHead,_oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion
            int nCount = 1;
            #region initialize
            oTopTable = new PdfPTable(3);
            oTopTable.SetWidths(new float[]{    
                                                 2f, //sl                     
                                                 48f,//mkt head
                                                 48f, //team members                        

                                        });
            #endregion
            foreach (MarketingAccount oItem in _oMarketingAccounts)
            {
                             
                if (oItem.IsGroupHead == true)
                {
                    int rowSpan = _oMarketingAccounts.Count;

                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.Rowspan = rowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyle)); _oPdfPCell.Rowspan = rowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                    nCount++;
                }  


                _oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                //oTopTable.CompleteRow();
               
              
            }
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion
        }
        private void GetMarketingProjectionTable()
        {
            PdfPTable oTopTable = new PdfPTable(40);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                                             
                                                 8f,//1  
                                                20f,//2  
                                                20f,//3  
                                                20f,//4  
                                                10f,//5  
                                                10f,//6  
                                                10f,//7  
                                                10f,//8  
                                                10f,//9  
                                                10f,//10  
                                                10f,//11  
                                                10f,//12  
                                                10f,//13                                            
                                                10f,//14
                                                10f,//15                                             
                                                10f,//16
                                                10f,//17                                                  
                                                10f,//18
                                                10f,//19                                                
                                                10f,//20                                                 
                                                10f,//21 
                                                10f,//22 
                                                10f,//23 
                                                10f,//24 
                                                10f,//25  
                                                10f,//26 
                                                10f,//27  
                                                10f,//28
                                                10f,//29  
                                                10f,//30                                                
                                                10f,//31                                                 
                                                10f,//32 
                                                10f,//33 
                                                10f,//34 
                                                10f,//35 
                                                10f,//36  
                                                10f,//37 
                                                10f,//38  
                                                10f,//39
                                                10f,//40  
                                            });

            //Header...
            #region Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Buyer Position", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Est. Order Qty", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            string[] MonthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            foreach (string month in MonthNames)
            {
                if (!string.IsNullOrEmpty(month))
                {
                    string Month = month.Substring(0, 3).ToUpper();
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase(Month + " " + _nYear.ToString().Substring(2, 2), _oFontStyle)); _oPdfPCell.Colspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);
                }
            }
            
            oTopTable.CompleteRow();

            for (int i = 0; i <12; i++)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase("Projection Qty", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase("Achive Qty", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

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

            List<MktSaleTarget> _distMktSaleTarget = new List<MktSaleTarget>();
            _distMktSaleTarget = _oMktSaleTargets.GroupBy(x => x.ContractorID).Select(g => g.First()).ToList();
            _distMktSaleTarget = _distMktSaleTarget.OrderBy(x => x.BuyerName).ToList();
            //_distReceivingSalesReport = _ReceivingSalesReports.GroupBy(x => x.RefID).Select(g => g.First()).ToList();
            //_distMktSaleTarget = _DispoMktSaleTargets.GroupBy(x => x.MarketingAccountID).Select(g => g.First()).ToList();

            #region DATA
            string tempGroupName = ""; int nCount = 1;
            int allocationValue = 0;
            foreach (MktSaleTarget oItem in _distMktSaleTarget)
            {
                #region Table Initialize
                oTopTable = new PdfPTable(40);
                oTopTable.SetWidths(new float[] {                                             
                                                 8f,//1  
                                                20f,//2  
                                                20f,//3  
                                                20f,//4  
                                                10f,//5  
                                                10f,//6  
                                                10f,//7  
                                                10f,//8  
                                                10f,//9  
                                                10f,//10  
                                                10f,//11  
                                                10f,//12  
                                                10f,//13                                            
                                                10f,//14
                                                10f,//15                                             
                                                10f,//16
                                                10f,//17                                                  
                                                10f,//18
                                                10f,//19                                                
                                                10f,//20                                                 
                                                10f,//21 
                                                10f,//22 
                                                10f,//23 
                                                10f,//24 
                                                10f,//25  
                                                10f,//26 
                                                10f,//27  
                                                10f,//28
                                                10f,//29  
                                                10f,//30                                                
                                                10f,//31                                                 
                                                10f,//32 
                                                10f,//33 
                                                10f,//34 
                                                10f,//35 
                                                10f,//36  
                                                10f,//37 
                                                10f,//38  
                                                10f,//39
                                                10f,//40  
                                            });
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(Convert.ToInt32(nCount).ToString(), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerPosition, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase((oItem.OrderQty>0)?oItem.OrderQty.ToString("#,##0;(#,##0)"):"-", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);


                for (int i = 1; i <= 12; i++)
                {
                    double nValue = 0;
                    double nReceiveQty = 0;
                    string BPercent = "";
                    double nAmount = 0;
                    nValue = _oMktSaleTargets.Where(x => x.Month == i && x.ContractorID == oItem.ContractorID).Sum(x => x.Value);
                    nReceiveQty = _oMktSaleTargets.Where(x => x.Month == i && x.ContractorID == oItem.ContractorID).Sum(x => x.ReceiveQty);
                    BPercent = Global.MillionFormat_Round(_oMktSaleTargets.Where(x => x.Month == i && x.ContractorID == oItem.ContractorID).Sum(x => x.BPercent));
                    nAmount = _oMktSaleTargets.Where(x => x.Month == i && x.ContractorID == oItem.ContractorID).Sum(x => x.Amount);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase((nValue>0)?nValue.ToString("#,##0;(#,##0)"):"-", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase((nReceiveQty > 0) ? nReceiveQty.ToString("#,##0;(#,##0)") : "-", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL); _oPdfPCell = new PdfPCell(new Phrase((nAmount > 0) ? nAmount.ToString("#,##0;(#,##0)") : "-", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                }

                string nTotalQty = "";
      
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

            #region Total
            #region Table Initialize
            oTopTable = new PdfPTable(40);
            oTopTable.SetWidths(new float[] {                                             
                                                 8f,//1  
                                                20f,//2  
                                                20f,//3  
                                                20f,//4  
                                                10f,//5  
                                                10f,//6  
                                                10f,//7  
                                                10f,//8  
                                                10f,//9  
                                                10f,//10  
                                                10f,//11  
                                                10f,//12  
                                                10f,//13                                            
                                                10f,//14
                                                10f,//15                                             
                                                10f,//16
                                                10f,//17                                                  
                                                10f,//18
                                                10f,//19                                                
                                                10f,//20                                                 
                                                10f,//21 
                                                10f,//22 
                                                10f,//23 
                                                10f,//24 
                                                10f,//25  
                                                10f,//26 
                                                10f,//27  
                                                10f,//28
                                                10f,//29  
                                                10f,//30                                                
                                                10f,//31                                                 
                                                10f,//32 
                                                10f,//33 
                                                10f,//34 
                                                10f,//35 
                                                10f,//36  
                                                10f,//37 
                                                10f,//38  
                                                10f,//39
                                                10f,//40  
                                            });
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan =3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase(_distMktSaleTarget.Sum(x => x.OrderQty).ToString("#,##0;(#,##0)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                 
                for (int i = 1; i <= 12; i++)
                {
                    double nValue = _oMktSaleTargets.Where(x => x.Month == i).Sum(x => x.Value);
                    double nReceiveQty = _oMktSaleTargets.Where(x => x.Month == i).Sum(x => x.ReceiveQty);
                    double nAmount = _oMktSaleTargets.Where(x => x.Month == i).Sum(x => x.Amount);

                    if (nValue > 0){
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase(nValue.ToString("#,##0;(#,##0)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    }
                     
                    else
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    }

                    if (nReceiveQty > 0)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase(nReceiveQty.ToString("#,##0;(#,##0)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    }

                    else
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    }

                    if (nAmount > 0)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase(nAmount.ToString("#,##0;(#,##0)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    }

                    else
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    }
                                     
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

        private void GetSummery()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 22f, 5f, 5f, 36f, 5f, 5f, 22f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 25, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 22f, 5f, 5f, 36f, 5f, 5f, 22f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Summery", 0, 5, Element.ALIGN_CENTER, BaseColor.WHITE, true, 10, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            #region Heder Info
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 22f, 5f, 5f, 36f, 5f, 5f, 22f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Month", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "No Of PI", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "PI No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Amount", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            #region data
            var data = _oExportPIs.GroupBy(x => new { x.OpeningDate.Month, x.OpeningDate.Year }, (key, grp) => new
            {
                Month = key.Month,
                Year = key.Year,
                NoOfPI = grp.ToList().Count(),
                PIsNo = string.Join(", ", grp.ToList().Select(y => y.PINo)),
                Qty = grp.ToList().Sum(y => y.Qty),
                Amount = grp.ToList().Sum(y => y.Amount)
            }).OrderBy(c => c.Year).ThenBy(c => c.Month);

            foreach (var oItem in data)
            {
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] { 22f, 5f, 5f, 36f, 5f, 5f, 22f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                DateTime dDate = new DateTime(oItem.Year, oItem.Month, 1);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, dDate.ToString("MMM yyyy"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.NoOfPI.ToString("#,##0;(#,##0)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.PIsNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Qty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Amount.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
            #endregion

            #region Total Info
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 22f, 5f, 5f, 36f, 5f, 5f, 22f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, data.Sum(x => x.NoOfPI).ToString("#,##0;(#,##0)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, data.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, data.Sum(x => x.Amount).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }
                                                                                                                                                                                                                                                 
    }

}
