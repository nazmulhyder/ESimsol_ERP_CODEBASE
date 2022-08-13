using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ESimSol.Reports
{
    public class rptGreigeInspectionReport
    {
        #region Declaration
        int _nTotalColumn = 1;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<FabricBatchQC> _oFabricBatchQCs = new List<FabricBatchQC>();
        FabricBatchQCDetail _oFabricBatchQCDetail = new FabricBatchQCDetail();
        List<FabricBatchQCDetail> _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
        List<FabricExecutionOrderSpecification> _oFEOSs = new List<FabricExecutionOrderSpecification>();
        Company _oCompany = new Company();
        string sHeaderStatus = ""; double nTotalQty = 0;
        List<FabricQCGrade> _oFabricQCGrades = new List<FabricQCGrade>();
        List<FabricBatchQCFault> _oFabricBatchQCFault = new List<FabricBatchQCFault>();
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
        #endregion
        public byte[] PrepareReport(List<FabricBatchQC> oFabricBatchQCs, List<FabricBatchQCDetail> oFabricBatchQCDetails, List<FabricExecutionOrderSpecification> oFEOSs, Company oCompany, string sMsg, List<FabricQCGrade> oFabricQCGrades, List<FabricBatchQCFault> oFabricBatchQCFault, List<SignatureSetup> oSignatureSetups)
        {
            _oFabricBatchQCs = oFabricBatchQCs;
            _oFabricBatchQCDetails = oFabricBatchQCDetails;
            _oFEOSs = oFEOSs;
            _oCompany = oCompany;
            sHeaderStatus = sMsg;
            _oFabricQCGrades = oFabricQCGrades;
            _oFabricBatchQCFault = oFabricBatchQCFault;
            _oSignatureSetups = oSignatureSetups;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
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
        

            this.PrintBody();
            _oPdfPTable.HeaderRows = 2;
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(sHeaderStatus, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Reporting Date:"+ DateTime.Now.ToString("dd MMM yyyy"), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            PrintMainBody();

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            PrintSummery();

            float nTableHeight = CalculatePdfPTableHeight(_oPdfPTable);
            float _nfixedHight = 720 - (float)nTableHeight;
            if (_nfixedHight > 0.0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = 8; _oPdfPCell.FixedHeight = _nfixedHight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
   
             #region print Signature Captions
             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
             _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
             _oPdfPTable.CompleteRow();

             _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(525f, (object)_oFabricBatchQCDetail, _oSignatureSetups, 0.0f)); _oPdfPCell.Colspan = 9;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
             _oPdfPTable.CompleteRow();
             #endregion
        }
        #endregion
         private void PrintMainBody()
        {

            PdfPTable oTopTable = new PdfPTable(9);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                                             
                                            8f,//1  
                                            12f,//2  
                                            15f,//3  
                                            25f,//4  
                                            25f,//5  
                                            15f,//6  
                                            15f,//7  
                                            15f,//8  
                                            15f,//9                                           
                                            });

                  #region Header
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Loom No", _oFontStyle));
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

                _oPdfPCell = new PdfPCell(new Phrase("Causes Of " +_oFabricBatchQCDetails[0].QCGrade, _oFontStyle));
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

            #region DATA
            int nCount = 1;
            List<FabricBatchQCDetail> oTempFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            FabricExecutionOrderSpecification _oTempFEOS = new FabricExecutionOrderSpecification();

            foreach (var oItem in _oFabricBatchQCDetails)
            {
                 #region initialize
                oTopTable = new PdfPTable(9);
                 oTopTable.SetWidths(new float[] {                                             
                                                8f,//1  
                                            12f,//2  
                                            15f,//3  
                                            25f,//4  
                                            25f,//5  
                                            15f,//6  
                                            15f,//7  
                                            15f,//8  
                                            15f,//9                                           
                                            });
                #endregion
                 oTempFabricBatchQCDetails = _oFabricBatchQCDetails.Where(x => x.FBQCID == oItem.FBQCID).OrderBy(y => y.StoreRcvDate).ToList();
                 _oTempFEOS = _oFEOSs.Where(x => x.FEOSID == oItem.FEOSID).FirstOrDefault();
                 if (_oTempFEOS == null) _oTempFEOS = new FabricExecutionOrderSpecification();
                 string sCauses = string.Join(",", _oFabricBatchQCFault.Where(x=>x.FBQCDetailID == oItem.FBQCDetailID).Select(x => x.FPFName));
               
                   _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LoomNo, _oFontStyle));
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
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                     double Qty = Global.GetMeter(oItem.Qty, 2);
                    _oPdfPCell = new PdfPCell(new Phrase(Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(sCauses, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                    oTopTable.CompleteRow();
                    nCount++; nTotalQty += Qty;
                    #region push into main table
                    _oPdfPCell = new PdfPCell(oTopTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
            }

            #region GRAND TOTAL
            #region initialize
            oTopTable = new PdfPTable(9);
            oTopTable.SetWidths(new float[] {                                             
                                             8f,//1  
                                            12f,//2  
                                            15f,//3  
                                            25f,//4  
                                            25f,//5  
                                            15f,//6  
                                            15f,//7  
                                            15f,//8  
                                            15f,//9     
                                        });
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("GRAND Total ", _oFontStyle)); _oPdfPCell.Colspan = 7;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
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
         private void PrintSummery()
         {
             PdfPTable oTopTable = new PdfPTable(4);
             oTopTable.WidthPercentage = 100;
             oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
             oTopTable.SetWidths(new float[] {                                             
                                            20f,//1  
                                            20f,//2  
                                            20f,//3
                                            40f
                                            });


             _oFontStyle = FontFactory.GetFont("Tahoma", 6f);
             #region Row 1
             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Total Sample/Production=", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Mtr", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.Border = 0;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);
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
             #region Row 2
             oTopTable = new PdfPTable(4);
             oTopTable.SetWidths(new float[] {                                             
                                            20f,//1  
                                            20f,//2  
                                            20f,//3
                                            40f
                                            });
             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Sample Avg.Rejection=", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("%", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
             _oPdfPCell.Border = 0;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);
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
             #region Row 3
             oTopTable = new PdfPTable(4);
             oTopTable.SetWidths(new float[] {                                             
                                            20f,//1  
                                            20f,//2  
                                            20f,//3
                                            40f
                                            });
             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Bulk Y/D Production good=", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);


             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Meter", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.Border = 0;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);
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
             #region Row 4
             oTopTable = new PdfPTable(4);
             oTopTable.SetWidths(new float[] {                                             
                                            20f,//1  
                                            20f,//2  
                                            20f,//3
                                            40f
                                            });
             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Bulk S/D Production Good=", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Meter", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);


             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.Border = 0;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);
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
             #region Row 5
             oTopTable = new PdfPTable(4);
             oTopTable.SetWidths(new float[] {                                             
                                            20f,//1  
                                            20f,//2  
                                            20f,//3
                                            40f
                                            });
             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Total Sample/Production=", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Meter", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.Border = 0;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);
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
             #region Row 6
             oTopTable = new PdfPTable(4);
             oTopTable.SetWidths(new float[] {                                             
                                            20f,//1  
                                            20f,//2  
                                            20f,//3
                                            40f
                                            });
             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Yarn fault Reject=", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Meter", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.Border = 0;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);
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
             #region Row 7
             oTopTable = new PdfPTable(4);
             oTopTable.SetWidths(new float[] {                                             
                                            20f,//1  
                                            20f,//2  
                                            20f,//3
                                            40f
                                            });
             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Total Rejection(red1+red2)=", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Meter", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
             _oPdfPCell.Border = 0;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);
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
             #region Row 8
             oTopTable = new PdfPTable(4);
             oTopTable.SetWidths(new float[] {                                             
                                            20f,//1  
                                            20f,//2  
                                            20f,//3
                                            40f
                                            });
             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Meter", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.Border = 0;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);
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
             #region Row 9
             oTopTable = new PdfPTable(4);
             oTopTable.SetWidths(new float[] {                                             
                                            20f,//1  
                                            20f,//2  
                                            20f,//3
                                            40f
                                            });
             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Avg. Rejection=", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Meter", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);

             _oFontStyle.SetStyle(iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.Border = 0;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oTopTable.AddCell(_oPdfPCell);
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
    }
}
