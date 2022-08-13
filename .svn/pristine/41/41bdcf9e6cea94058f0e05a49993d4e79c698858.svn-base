using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
 
 

namespace ESimSol.Reports
{

    public class rptRecycleProcess
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        RecycleProcess _oRecycleProcess = new RecycleProcess();
        List<RecycleProcessDetail> _oRecycleProcessDetails = new List<RecycleProcessDetail>();
        
        Company _oCompany = new Company();

        #endregion

        public byte[] PrepareReport(RecycleProcess oRecycleProcess, Company oCompany)
        {
            _oRecycleProcess = oRecycleProcess;
            _oRecycleProcessDetails = oRecycleProcess.RecycleProcessDetails;
            _oCompany =oCompany;
            

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(30f, 30f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 535f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(160f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

        
            #endregion

            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Re-Cycle Process", _oFontStyle));

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            #region Recycle Process Information

            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 55f, 200f, 95f, 105f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC);

            #region 1st Row
            _oPdfPCell = new PdfPCell(new Phrase("Ref No :", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oRecycleProcess.RefNo, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Process Date :", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oRecycleProcess.ProcessDateInString, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

          
            oPdfPTable.CompleteRow();
            #endregion

            #region 2nd Row

            _oPdfPCell = new PdfPCell(new Phrase("Approve By:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oRecycleProcess.ApprovedByName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Note:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oRecycleProcess.Note, _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Recycle Process Detail Information

            oPdfPTable = new PdfPTable(7);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 30f, 120f, 150f, 100f,67f, 100f,100f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            #region Detail title


            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan =5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 10f;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion

            #region LC Detail Tittle
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("RawMaterial Process Detail ", _oFontStyle));
            _oPdfPCell.Colspan = 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 7f;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

         
            _oPdfPCell = new PdfPCell(new Phrase("Product Code", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


             _oPdfPCell = new PdfPCell(new Phrase("Store Name", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ProcessProduct Type", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            int nCount = 0,nCountDamage=0;
            
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            foreach (RecycleProcessDetail oItem in _oRecycleProcessDetails)
            {

                nCount++;
                if (oItem.ProcessProductType.ToString() != "RawMaterial")
                {
                    nCountDamage++;
               
                    if (nCountDamage == 1)
                    {
                        oPdfPTable.CompleteRow();
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        nCount = 1;
                        #region New Table For Damaged Type
                        oPdfPTable = new PdfPTable(7);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        oPdfPTable.SetWidths(new float[] { 30f, 120f, 150f,100f, 67f, 100f, 100f });
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                        #endregion
                        #region Blank Space
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Colspan = 5;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.ExtraParagraphSpace = 10f;
                        oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        //_oPdfPCell = new PdfPCell(oPdfPTable);
                        //_oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        //_oPdfPTable.CompleteRow();
                        #endregion
                        #region LC Detail Tittle
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                        _oPdfPCell = new PdfPCell(new Phrase("Damaged Product Detail ", _oFontStyle));
                        _oPdfPCell.Colspan = 5;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.ExtraParagraphSpace = 7f;
                        oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();


                        #endregion
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
                        _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                        _oPdfPCell = new PdfPCell(new Phrase("Product Code", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Store Name", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("ProcessProduct Type", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                    }
                }
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductCode, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.WorkingUName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProcessProductType.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
      
            }
            //_oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyle)); _oPdfPCell.Colspan = 5;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //double sSum = 0.00;
            //for(int i=0; i<_oRecycleProcessDetails.Count; i++)
            //{
            //    sSum = sSum + _oRecycleProcessDetails[i].Qty;
            //}

            //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(sSum), _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
    }
    

}
