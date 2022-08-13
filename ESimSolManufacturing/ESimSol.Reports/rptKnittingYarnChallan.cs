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
    public class rptKnittingYarnChallan
    {
        #region Declaration

        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleHeader;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();

        List<KnittingYarnChallanDetail> _oKnittingYarnChallanDetails = new List<KnittingYarnChallanDetail>();
        List<ApprovalHead> _oApprovalHeads = new List<ApprovalHead>();
        List<ApprovalHistory> _oApprovalHistorys = new List<ApprovalHistory>();
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
        KnittingYarnChallan _oKnittingYarnChallan = new KnittingYarnChallan();
        int nDistinctStore = 0, nDistinctStyle = 0;
        #endregion

        private void PrintHeader()
        {
            #region Company & Report Header
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 200f, 400f, 200f });

            #region Company Name & Report Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(95f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Address & Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Phone Number
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Phone + ";  " + _oBusinessUnit.Email + ";  " + _oBusinessUnit.WebAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #endregion
        }

        public byte[] PrepareReport(KnittingYarnChallan oKnittingYarnChallan, Company oCompany, BusinessUnit oBusinessUnit, List<SignatureSetup> oSignatureSetups)
        {
            _oCompany = oCompany;
            _oKnittingYarnChallan = oKnittingYarnChallan;
            _oKnittingYarnChallanDetails = oKnittingYarnChallan.KnittingYarnChallanDetails;
            _oBusinessUnit = oBusinessUnit;
            //_oApprovalHeads = oApprovalHeads;
            //_oApprovalHistorys = oApprovalHistorys;
            _oSignatureSetups = oSignatureSetups;
            if (_oKnittingYarnChallanDetails.Count > 0)
            {
                nDistinctStore = _oKnittingYarnChallanDetails.Select(x => x.OperationUnitName).Distinct().Count();
                nDistinctStyle = _oKnittingYarnChallanDetails.Select(x => new { x.KODStyleNo, x.PAM }).Distinct().Count();
            }
            
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(20f, 20f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595 });
            #endregion

            #region 1st
            Break(20);
            this.PrintHeader();
            Break(5);
            this.PrintKnittingYarnChallanHeader("Yarn Delivery Challan", "Office Copy");
            this.PrintKnitChallan();
            Break(20);
            this.PrintKnitChallanDetails();
            float nFullPageHeight = 840f;
            float nAlreadyUsagesHeight = 150f;
            nAlreadyUsagesHeight = nAlreadyUsagesHeight + CalculatePdfPTableHeight(_oPdfPTable);
            float nBlankRowHeight = nFullPageHeight - nAlreadyUsagesHeight;
            if (nBlankRowHeight > 0)
            {
                Break(nBlankRowHeight);
            }
            this.PrintSignature();
            #endregion

            NewPageDeclaration();

            #region 2nd
            Break(20);
            this.PrintHeader();
            Break(5);
            this.PrintKnittingYarnChallanHeader("Yarn Delivery Challan", "Client Copy");
            this.PrintKnitChallan();
            Break(20);
            this.PrintKnitChallanDetails();
            if (nBlankRowHeight > 0)
            {
                Break(nBlankRowHeight);
            }
            this.PrintSignature();
            #endregion

            NewPageDeclaration();

            #region 3rd
            Break(20);
            this.PrintHeader();
            Break(5);
            this.PrintKnittingYarnChallanHeader("Yarn Delivery Challan", "Gate Pass");
            this.PrintKnitChallan();
            Break(20);
            this.PrintKnitChallanDetails();
            if (nBlankRowHeight > 0)
            {
                Break(nBlankRowHeight);
            }
            this.PrintSignature();
            #endregion

            NewPageDeclaration();

            #region 4th
            Break(20);
            this.PrintHeader();
            Break(5);
            this.PrintKnittingYarnChallanHeader("Yarn Delivery Challan", "Client Gate Pass");
            this.PrintKnitChallan();
            Break(20);
            this.PrintKnitChallanDetails();
            if (nBlankRowHeight > 0)
            {
                Break(nBlankRowHeight);
            }
            this.PrintSignature();
            #endregion            

            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        public void PrintKnitChallan()
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 11, 30, 10, 20, 12, 17 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleHeader = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("Knitting Challan Info", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.NO_BORDER; _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Challan No:", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallan.ChallanNo, _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Challan Date:", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallan.ChallanDateInString, _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Knitting Order:", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallan.KnittingOrderNo, _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Factory:", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallan.FactoryName, _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase("Knitting Date:", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallan.KnittingOrderDateInString, _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Delivery Point:", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallan.DeliveryPoint, _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Truck No:", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallan.CarNumber, _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Driver:", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallan.DriverName, _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            if (nDistinctStyle == 1 && nDistinctStore == 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Style:", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallanDetails[0].KODStyleNo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Buyer:", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallanDetails[0].BuyerName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PAM:", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallanDetails[0].PAM.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Order Qty:", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallanDetails[0].KODOrderQty.ToString("###,0.00"), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Store:", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallanDetails[0].OperationUnitName, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
            }
            else if (nDistinctStyle == 1 && nDistinctStore > 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Style:", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallanDetails[0].KODStyleNo, _oFontStyle)); 
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Buyer:", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallanDetails[0].BuyerName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PAM:", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallanDetails[0].PAM.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Order Qty:", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallanDetails[0].KODOrderQty.ToString("###,0.00"), _oFontStyle)); _oPdfPCell.Colspan = 5;
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
            }

            if (nDistinctStyle > 1 && nDistinctStore == 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Store:", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallanDetails[0].OperationUnitName, _oFontStyle)); _oPdfPCell.Colspan = 5;
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
            }
            
            _oPdfPCell = new PdfPCell(new Phrase("Remarks:", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oKnittingYarnChallan.Remarks, _oFontStyle)); _oPdfPCell.Colspan = 5;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        public void PrintKnitChallanDetails()
        {
            int nCol = 0, nTotalCol = 0;
            if (nDistinctStore > 1 && nDistinctStyle > 1) nCol = 4;
            else if (nDistinctStore == 1 && nDistinctStyle > 1) nCol = 3;
            else if (nDistinctStore > 1 && nDistinctStyle == 1) nCol = 1;
            nTotalCol = 8 + nCol;
            PdfPTable oPdfPTable = new PdfPTable(nTotalCol);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            if (nCol == 0) oPdfPTable.SetWidths(new float[] { 20, 100, 50, 50, 70, 30, 35, 35 });
            else if (nCol == 4) oPdfPTable.SetWidths(new float[] { 20, 80, 80, 35, 40, 100, 50, 50, 55, 35, 35, 40 });
            else if (nCol == 3) oPdfPTable.SetWidths(new float[] { 20, 80, 30, 40, 100, 50, 60, 55, 30, 35, 40 });
            else if (nCol == 1) oPdfPTable.SetWidths(new float[] { 20, 80, 100, 50, 70, 55, 30, 35, 40 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleHeader = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("Knitting Challan Details", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.Colspan = 12; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #region header
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            if (nDistinctStore > 1 && nDistinctStyle > 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Store", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Style/Buyer/Qty", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PAM", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else if (nDistinctStore == 1 && nDistinctStyle > 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Style/Buyer/Qty", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PAM", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else if (nDistinctStore > 1 && nDistinctStyle == 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Store", _oFontStyleHeader));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("Yarn", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Brand", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M. Unit", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Bag Qty", _oFontStyleHeader));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region challan details data
            int nCount = 0;
            double totalQty = 0, totalBagQty = 0;
            double totalBlnc = 0;

            foreach (KnittingYarnChallanDetail oItem in _oKnittingYarnChallanDetails)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                if (nDistinctStore > 1 && nDistinctStyle > 1)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.OperationUnitName.ToString(), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.KODStyleNo + "\n" + oItem.BuyerName + "\n" + oItem.KODQty + " " + oItem.KODMUShortName, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PAM.ToString(), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.KODOrderQty.ToString("###,0.00"), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                else if (nDistinctStore == 1 && nDistinctStyle > 1)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.KODStyleNo + "\n" + oItem.BuyerName + "\n" + oItem.KODQty + " " + oItem.KODMUShortName, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PAM.ToString(), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.KODOrderQty.ToString("###,0.00"), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                
                }
                else if (nDistinctStore > 1 && nDistinctStyle == 1)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.OperationUnitName.ToString(), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                
                _oPdfPCell = new PdfPCell(new Phrase(oItem.YarnName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BrandShortName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUnitName.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BagQty.ToString("#,###;(#,###)"), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                totalQty = totalQty + oItem.Qty;
                totalBagQty += oItem.BagQty;
            }
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Grand Total:", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.Colspan = nTotalCol-2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(totalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(totalBagQty.ToString("#,###;(#,###)"), _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = nTotalCol; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("In Words: " + GlobalObject.QtyInWords(totalQty, _oKnittingYarnChallanDetails[0].MUnitName), _oFontStyleHeader)); _oPdfPCell.FixedHeight = 20f; _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = nTotalCol; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        public void PrintSignature()
        {
            #region print Signature Captions
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(525f, (object)_oKnittingYarnChallan, _oSignatureSetups, 15f));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region another signature
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 60f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(GetSignatureTable()); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; 
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            #endregion
        }

        private PdfPTable GetSignatureTable()
        {
            PdfPTable oSignatureTable = new PdfPTable(5);
            oSignatureTable.WidthPercentage = 100;
            oSignatureTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oSignatureTable.SetWidths(new float[] { 20f, 20f, 20f, 20f, 20f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));  _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Security", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Receiver Seal & Signature", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            oSignatureTable.CompleteRow();

            return oSignatureTable;
        }

        public void Break(float nFixedHeight)
        {

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight; _oPdfPCell.Border = 0;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

        }
        public void PrintKnittingYarnChallanHeader(string str, string copyMsg)
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 200, 440, 200 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);

            #region Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(str, FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            oPdfPTable.CompleteRow();
            #endregion

            #region Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(copyMsg, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            _oPdfPCell.ExtraParagraphSpace = 30f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        public void NewPageDeclaration()
        {
            #region New Page Declare
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
            #endregion
        }

        #region Support Functions
        public static float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        table.TotalWidth = 525f;
                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }
        #endregion

    }
}
