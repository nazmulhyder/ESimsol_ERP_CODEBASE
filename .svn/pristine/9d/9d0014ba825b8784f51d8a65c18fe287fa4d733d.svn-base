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
    public class rptPrintTwistings
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);//number of columns
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Twisting _oTwisting = new Twisting();
        List<Twisting> _oTwistings = new List<Twisting>();

        List<TwistingDetail> TwistingDetails = new List<TwistingDetail>();
        List<TwistingDetail> TwistingDetails_Product = new List<TwistingDetail>();
        List<TwistingDetail> TwistingDetails_Twisting = new List<TwistingDetail>();
        Company _oCompany = new Company();
        string _sMessage = "";
        string oPrevCounts = "";
        string oPresentCount = "";
        string sPresentLots = "";
        int nUserID = 0;
        #endregion
        public byte[] PrepareReport(List<Twisting> oTwistings,  List<TwistingDetail> oTwistingDetails , Company oCompany, string sHeaderName, int UserID)
        {
            _oTwistings = oTwistings;
            TwistingDetails = oTwistingDetails;
            _oCompany = oCompany;
            nUserID = UserID;
            #region Page Setup
            _oDocument = new Document(PageSize.LEGAL, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(20f, 20f, 20f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 800f });
            #endregion
            this.PrintHeader();
            this.PrintBody();
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Doubling & Twisting Production Report", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
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
             PdfPTable oTopTable = new PdfPTable(11);
             oTopTable.WidthPercentage = 100;
             oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
             oTopTable.SetWidths(new float[] {                                             
                                                 5f,//1  
                                                15f,//2  
                                                18f,//3  
                                                20f,//4  
                                                40f,//5
                                                40f,//6   
                                                22f,//7    
                                                15f,//8   
                                                15f,//9   
                                                15f,//10  
                                                15f,//11   
                                            });

             #region Header
             _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

             _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

             _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

             _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Customer", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

             _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Previous Count", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

             _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Present Count", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

             _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Shade", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

             _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Req. No", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

             _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Batch/Lot No", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

             _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

             _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
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

             #region Data
             int nCount = 1;
             string sColorName = "";
             foreach (Twisting oItem in _oTwistings)
             {
                 #region initialize
                 oTopTable = new PdfPTable(11);
                 oTopTable.SetWidths(new float[] {                                             
                                                 5f,//1  
                                                15f,//2  
                                                18f,//3  
                                                20f,//4  
                                                40f,//5
                                                40f,//6   
                                                22f,//7    
                                                15f,//8   
                                                15f,//9   
                                                15f,//10  
                                                15f,//11   
                                            });
                 #endregion
                 _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                 _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                 _oPdfPCell = new PdfPCell(new Phrase(oItem.CompletedDateSt, _oFontStyle));
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                 _oPdfPCell = new PdfPCell(new Phrase(oItem.DyeingOrderNo, _oFontStyle));
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                 _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);


                    //TwistingDetails = TwistingDetail.Gets(oItem.TwistingID, nUserID);
                 TwistingDetails_Product = TwistingDetails.Where(x => x.InOutTypeInt == (int)EnumInOutType.Receive && x.TwistingID == oItem.TwistingID).ToList();
                 TwistingDetails_Twisting = TwistingDetails.Where(x => x.InOutTypeInt == (int)EnumInOutType.Disburse && x.TwistingID == oItem.TwistingID).ToList();


                    oPrevCounts = "";
                    foreach (TwistingDetail _oPrevCount in TwistingDetails_Twisting)
                    {
                        oPrevCounts += _oPrevCount.ProductName + ", ";
                        
                    }

                    oPresentCount = ""; sPresentLots = "";
                    foreach (TwistingDetail _oPresentCount in TwistingDetails_Product)
                    {

                        oPresentCount += _oPresentCount.ProductName + ", ";
                        sPresentLots += _oPresentCount.LotNo + ", ";
                    }

                   

                    _oPdfPCell = new PdfPCell(new Phrase(oPrevCounts != "" ? oPrevCounts : "", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oPresentCount != "" ? oPresentCount : "", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    sColorName = string.Join("+", TwistingDetails_Twisting.Select(x => x.ColorName).ToList());

                    _oPdfPCell = new PdfPCell(new Phrase((string.IsNullOrEmpty(sColorName) ? "Grey" : sColorName), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.TWNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(sPresentLots != "" ? sPresentLots : "", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.CompletedByName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
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
             #region initialize
             oTopTable = new PdfPTable(11);
             _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
             oTopTable.SetWidths(new float[] {                                             
                                                 5f,//1  
                                                15f,//2  
                                                18f,//3  
                                                20f,//4  
                                                40f,//5
                                                40f,//6   
                                                22f,//7    
                                                15f,//8   
                                                15f,//9   
                                                15f,//10  
                                                15f,//11   
                                            });

             _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan = 8;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase(_oTwistings.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); 
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
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
             #endregion

         }

    }
}