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
    public class rptFabricBatchQCReport
    {
        #region Declaration
        int _nTotalColumn = 9;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        string sHeaderString = "";
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<FabricBatchQCDetail> _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
        Company _oCompany = new Company();
        string _sMessage = "";
        #endregion
        public byte[] PrepareReport(List<FabricBatchQCDetail> oFabricBatchQCDetails, Company oCompany, string sMessage)
        {
            _oFabricBatchQCDetails = oFabricBatchQCDetails;
            _oCompany = oCompany;
            _sMessage = sMessage;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(25f, 25f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oPdfPTable.SetWidths(new float[] { 595f });
            _oDocument.Open();
            #endregion
            this.PrintHeader();
            this.PrintBody();
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
            var FabricBatchQCDetails = _oFabricBatchQCDetails.GroupBy(x => new { x.QCGrade })
                                             .OrderBy(x => x.Key.QCGrade)
                                             .Select(x => new
                                             {
                                                 _QCGrade = x.Key.QCGrade,

                                             });

            foreach (var oData in FabricBatchQCDetails)
            {
                sHeaderString += oData._QCGrade + " , ";

            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_sMessage, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(sHeaderString, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

           
            #endregion

        }
        #endregion
        #region Report Body
        private void PrintBody()
        {
            PrintMainBody();
        }
        #endregion

        private void PrintMainBody()
        {

            PdfPTable oTopTable = new PdfPTable(10);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                                             
                                             8f,//1  
                                            12f,//2  
                                            15f,//3  
                                            15f,//4  
                                            25f,//5  
                                            25f,//6  
                                            15f,//7  
                                            15f,//8  
                                            15f,//9  
                                            15f,//10                                           
                                            });

            #region DATA
            int nCount = 1;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, 0);
            var FabricBatchQCDetails = _oFabricBatchQCDetails.GroupBy(x => new { x.QCGrade })
                                             .OrderBy(x => x.Key.QCGrade)
                                             .Select(x => new
                                             {
                                                 _QCGrade = x.Key.QCGrade,
                                                 _FabricBatchQCDetailList = x.OrderBy(c => c.FBQCDetailID),
                                                 SubTotalQty = x.Sum(y => y.Qty),

                                             });

            foreach (var oData in FabricBatchQCDetails)
            {

                #region initialize
                oTopTable = new PdfPTable(10);
                oTopTable.SetWidths(new float[] {                                             
                                            8f,//1  
                                            12f,//2  
                                            15f,//3  
                                            15f,//4  
                                            25f,//5  
                                            25f,//6  
                                            15f,//7  
                                            15f,//8  
                                            15f,//9  
                                            15f,//10                                           
                                            });

                #endregion
                nCount = 1;
                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("QCGrade:" + oData._QCGrade, _oFontStyle)); _oPdfPCell.Colspan = 11;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                oTopTable.CompleteRow();
                #region push into main table
                _oPdfPCell = new PdfPCell(oTopTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region initialize
                oTopTable = new PdfPTable(10);
                oTopTable.SetWidths(new float[] {                                             
                                             8f,//1  
                                            12f,//2  
                                            15f,//3  
                                            15f,//4  
                                            25f,//5  
                                            25f,//6  
                                            15f,//7  
                                            15f,//8  
                                            15f,//9  
                                            15f,//10                                           
                                            });
 
                                       
                #endregion

                #region Header
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Loom No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Batch No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Customer", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Shift", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Quantity", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
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
                foreach (var oItem in oData._FabricBatchQCDetailList)
                {

                    #region initialize
                    oTopTable = new PdfPTable(10);
                    oTopTable.SetWidths(new float[] {                                             
                                              8f,//1  
                                            12f,//2  
                                            15f,//3  
                                            15f,//4  
                                            25f,//5  
                                            25f,//6  
                                            15f,//7  
                                            15f,//8  
                                            15f,//9  
                                            15f,//10                                           
                                            });

                                      
                    #endregion
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LoomNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BatchNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.DispoNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Construction, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ShiftName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryDateSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Remark, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    oTopTable.CompleteRow();
                    nCount++;
                    #region push into main table
                    _oPdfPCell = new PdfPCell(oTopTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                }
                #region SUB TOTAL
               #region initialize
                    oTopTable = new PdfPTable(10);
                    oTopTable.SetWidths(new float[] {                                             
                                              8f,//1  
                                            12f,//2  
                                            15f,//3  
                                            15f,//4  
                                            25f,//5  
                                            25f,//6  
                                            15f,//7  
                                            15f,//8  
                                            15f,//9  
                                            15f,//10                                           
                                            });

                                      
                    #endregion
                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Sub Total ", _oFontStyle)); _oPdfPCell.Colspan = 8;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oData.SubTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

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
            #region GRAND TOTAL
            #region initialize
                    oTopTable = new PdfPTable(10);
                    oTopTable.SetWidths(new float[] {                                             
                                              8f,//1  
                                            12f,//2  
                                            15f,//3  
                                            15f,//4  
                                            25f,//5  
                                            25f,//6  
                                            15f,//7  
                                            15f,//8  
                                            15f,//9  
                                            15f,//10                                           
                                            });

                                      
                    #endregion
            _oPdfPCell = new PdfPCell(new Phrase("GRAND Total ", _oFontStyle)); _oPdfPCell.Colspan = 8;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFabricBatchQCDetails.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

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
