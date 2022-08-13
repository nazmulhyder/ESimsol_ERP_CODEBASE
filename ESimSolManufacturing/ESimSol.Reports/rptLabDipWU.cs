using System;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace ESimSol.Reports
{
    public class rptLabDipWU
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        int _nColumn = 8;
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        FabricSalesContractDetail _oFabricSalesContractDetail = new FabricSalesContractDetail();
        LabDip _oLabDip = new LabDip();

        int _nView = 1;
        float _nHeight = 200f; 
        #endregion
        public byte[] PrepareReport(List<LabDipDetail> oLabDipDetails, int nView, Company oCompany, BusinessUnit oBusinessUnit, LabDip oLabDip, FabricSalesContractDetail oFabricSalesContractDetail)
        {
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);

            if(nView==1)
            {
                 _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                 _nHeight = 122;
            }
            else
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);

            _nView = nView;
            _oDocument.SetMargins(20f, 20f, 10f, 40f);
            _oPdfPTable.WidthPercentage = 90;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oFabricSalesContractDetail = oFabricSalesContractDetail;
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595 });
            #endregion

            this.PrintHeader_Baly(ref _oPdfPTable, new BusinessUnit() { Name = oBusinessUnit.Name }, oCompany, "COLOR ANALYSIS", 1);

            _oLabDip = oLabDip;
            this.OrderInfo(oLabDip);
            this.ReportInfo(oLabDip);
            oLabDipDetails = oLabDipDetails.OrderBy(x=>x.WarpWeftTypeInt).ThenBy(x=>x.TwistedGroup).ToList();

            if (oLabDipDetails.Select(x => x.WarpWeftTypeInt == (int)EnumWarpWeft.None).Count() > 0)
            {
                this.MiddlePart(oLabDipDetails.Where(x => x.WarpWeftType == EnumWarpWeft.None).ToList(), "");
            }
            if (oLabDipDetails.Select(x => x.WarpWeftTypeInt == (int)EnumWarpWeft.Warp || x.WarpWeftTypeInt == (int)EnumWarpWeft.WarpnWeft).Count() > 0)
            {
                this.MiddlePart(oLabDipDetails.Where(x => x.WarpWeftType == EnumWarpWeft.Warp || x.WarpWeftTypeInt == (int)EnumWarpWeft.WarpnWeft).ToList(), "Warp");
            }
            if (oLabDipDetails.Select(x => x.WarpWeftTypeInt == (int)EnumWarpWeft.Weft || x.WarpWeftTypeInt == (int)EnumWarpWeft.WarpnWeft).Count() > 0)
            {
                this.MiddlePart(oLabDipDetails.Where(x => x.WarpWeftType == EnumWarpWeft.Weft || x.WarpWeftTypeInt == (int)EnumWarpWeft.WarpnWeft).ToList(), "Weft");
            }

            _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, 0, 10f));
            _oPdfPTable.AddCell(this.SetCellValue("SPECIAL NOTE: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, 10f));
            _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, 10f));

            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void PrintHeader_Baly(ref PdfPTable oPdfPTable_Main, BusinessUnit oBusinessUnit, Company oCompany, string sReportHeader, int nColspan)
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.SetWidths(new float[] { 80f, 340f, 40f });
            PdfPCell _oPdfPCell= new PdfPCell();
            iTextSharp.text.Image _oImag;

            #region LOGO
            if (oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(35f, 20f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, oBusinessUnit.Name, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            oPdfPTable.CompleteRow();

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);            
            ESimSolPdfHelper.AddCell(ref oPdfPTable, sReportHeader, Element.ALIGN_CENTER, Element.ALIGN_CENTER, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
          
            #region Insert Into Main Table
            ESimSolPdfHelper.AddTable(ref oPdfPTable_Main, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, nColspan);
            #endregion
        }

        private void OrderInfo(LabDip oLabDip)
        {
            float cellHight = 0f;
         
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            string[] sTopLeftRight = new string[] { "TOP", "LEFT" };
            string[] sTopRight = new string[] { "TOP", "RIGHT" };

            PdfPTable oPdfPTable = new PdfPTable(5);
            if (_nView==1)
                 oPdfPTable.SetWidths(new float[] { 33f, 81f, 47f, 360f, 64f });
            else oPdfPTable.SetWidths(new float[] { 48f, 81f, 62f, 300f, 94f });

            oPdfPTable.AddCell(this.SetCellValue("Order No:" , 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopLeftRight));
            oPdfPTable.AddCell(this.SetCellValue(" " + oLabDip.LabdipNo, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, new string[] { "TOP" }));
            oPdfPTable.AddCell(this.SetCellValue(" Buyer Name:", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, new string[] { "TOP" }));
            oPdfPTable.AddCell(this.SetCellValue(" " + oLabDip.ContractorName, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, new string[] { "TOP" }));
            oPdfPTable.AddCell(this.SetCellValue(" Date: " + oLabDip.OrderDateStr, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopRight));

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 0);
        }
        private void ReportInfo(LabDip oLabDip)
        {
            float cellHight = 0f;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 100f, 100f, 95, 100, 100, 105});

            oPdfPTable.AddCell(this.SetCellValue("P NUMBER", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHight));
            oPdfPTable.AddCell(this.SetCellValue("H NUMBER", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHight));
            oPdfPTable.AddCell(this.SetCellValue("DISPO NUMBER", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHight));
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            oPdfPTable.AddCell(this.SetCellValue("ACTUAL CONSTRUCTION", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHight));
            oPdfPTable.AddCell(this.SetCellValue("BUYER CONSTRUCTION", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHight));
            oPdfPTable.AddCell(this.SetCellValue("COMPOSITION", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHight));
            oPdfPTable.CompleteRow();

            var data = oLabDip.LabDipDetailFabrics.GroupBy(x => new { x.FSCDetailID }, (key, grp) => new
            {
                Result = grp.ToList() //All data
            });

            foreach (var oResults in data) 
            {
                var oItem = oResults.Result.FirstOrDefault();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                oPdfPTable.AddCell(this.SetCellValue(oItem.FabricNo, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHight));
                oPdfPTable.AddCell(this.SetCellValue(oItem.SCNoFull, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHight));
                oPdfPTable.AddCell(this.SetCellValue(oItem.ExeNo, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHight));
                oPdfPTable.AddCell(this.SetCellValue(oItem.ActualConstruction, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHight));
                oPdfPTable.AddCell(this.SetCellValue(oItem.Construction, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHight));
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                oPdfPTable.AddCell(this.SetCellValue(oItem.ProductName, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHight));  
            }
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 1, 0, 0);
        }
        private void MiddlePart(List<LabDipDetail> oLabDipDetails,string sWarpWeftTypeSt)
        {
            bool bHaveChild = false;
            int MaxColumn = oLabDipDetails.Count();
            if (MaxColumn <= 0) return;
            oLabDipDetails = oLabDipDetails.OrderBy(x => x.WarpWeftTypeInt).ToList();

            PdfPTable oPdfPTable = new PdfPTable(MaxColumn);
            float[] oCoumnWidths = new float[MaxColumn];
            for (int i = 0; i < MaxColumn; i++)
                oCoumnWidths[i] = 595 / MaxColumn;

            oPdfPTable.SetWidths(oCoumnWidths);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0 | iTextSharp.text.Font.BOLDITALIC);
            oPdfPTable.AddCell(this.SetCellValue("", 0, MaxColumn, Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, 0, 10f));
            oPdfPTable.AddCell(this.SetCellValue(sWarpWeftTypeSt.ToUpper() + " COLOR ", 0, MaxColumn, Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, 1, 10f));

            AddDetail(ref oPdfPTable, oLabDipDetails, ref bHaveChild);
            AddDetailFooter(ref oPdfPTable, oLabDipDetails);

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 1, 0, 0);
        }
        private void AddDetail(ref PdfPTable oPdfPTable, List<LabDipDetail> oLabDipDetails, ref bool bHaveChild) 
        {
            int prv_tGroup = -99;
            foreach (var oItem in oLabDipDetails)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma",8f, 0);
                ESimSolPdfHelper.FontStyle = _oFontStyle;
                if (oItem.TwistedGroup > 0) 
                {
                    if (prv_tGroup != oItem.TwistedGroup)
                    {
                        string val = (_oLabDip.LDTwistType == EnumLDTwistType.Injecting ? "INJECT SLUB" : "GRINDLE COUNT");
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, val, Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, (oLabDipDetails.Count(x => x.TwistedGroup == oItem.TwistedGroup)), 0);
                        prv_tGroup = oItem.TwistedGroup;
                    }
                }
                else
                    oPdfPTable.AddCell(this.SetCellValue("COUNT", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 1, 10f));
            }

            foreach (var oItem in oLabDipDetails)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                oPdfPTable.AddCell(this.SetCellValue(oItem.ProductName, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, 1, 10f));
            }
            foreach (var oItem in oLabDipDetails)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                
                oPdfPTable.AddCell(this.SetCellValue("" + oItem.PantonNo + (!string.IsNullOrEmpty(oItem.RefNo) ? "\n["+oItem.RefNo+"]" : ""), 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, 1, _nHeight, 90));
            }
        }
        private void AddChild(ref PdfPTable oPdfPTable, List<LabDipDetail> oLabDipDetails)
        {
            bool dTemp=false;
            AddDetail(ref oPdfPTable, oLabDipDetails, ref dTemp);
            AddDetailFooter(ref oPdfPTable, oLabDipDetails);
        }
        private void AddDetailFooter(ref PdfPTable oPdfPTable, List<LabDipDetail> oLabDipDetails) 
        {
            foreach (var oItem in oLabDipDetails)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                oPdfPTable.AddCell(this.SetCellValue("COLOR NAME", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, 1, 10));
            }
            foreach (var oItem in oLabDipDetails)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                oPdfPTable.AddCell(this.SetCellValue(oItem.ColorName, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, 1, 10));
            }

            foreach (var oItem in oLabDipDetails)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma",9f, 0);
                oPdfPTable.AddCell(this.SetCellValue("LD NUMBER", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, 1, 10));
            }
            foreach (var oItem in oLabDipDetails)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                oPdfPTable.AddCell(this.SetCellValue(oItem.ColorNo, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, 1, 10));
            }
        }
        private PdfPCell SetCellValue(string sName, int nRowSpan, int nColumnSpan, int halign, int valign, BaseColor color, int border, float height)
        {
            PdfPCell oPdfPCell = new PdfPCell();
            oPdfPCell = SetCellValue(sName, nRowSpan, nColumnSpan, halign, valign, color, border, height, 0);
            return oPdfPCell;
        }
        private PdfPCell SetCellValue(string sName, int nRowSpan, int nColumnSpan, int halign, int valign, BaseColor color, int border, float height, int rotattion)
        {
            PdfPCell oPdfPCell = new PdfPCell(new Phrase(sName, _oFontStyle));
            oPdfPCell.Rowspan = (nRowSpan > 0) ? nRowSpan : 1;
            oPdfPCell.Colspan = (nColumnSpan > 0) ? nColumnSpan : 1;
            oPdfPCell.Rotation = (rotattion > 0) ? rotattion : 0;
            oPdfPCell.HorizontalAlignment = halign;
            oPdfPCell.VerticalAlignment = valign;
            oPdfPCell.BackgroundColor = color;
            if (border == 0)
                oPdfPCell.Border = 0;
            if (height > 0)
                oPdfPCell.MinimumHeight = height;
            //oPdfPCell.BorderWidth = iTextSharp.text.;
            return oPdfPCell;
        }
        private PdfPCell SetCellValue(string sName, int nRowSpan, int nColumnSpan, int halign, int valign, BaseColor color, int border, float height, string[] BorderWidth)
        {
            PdfPCell oPdfPCell = new PdfPCell(new Phrase(sName, _oFontStyle));
            oPdfPCell.Rowspan = (nRowSpan > 0) ? nRowSpan : 1;
            oPdfPCell.Colspan = (nColumnSpan > 0) ? nColumnSpan : 1;
            oPdfPCell.HorizontalAlignment = halign;
            oPdfPCell.VerticalAlignment = valign;
            oPdfPCell.BackgroundColor = color;
            if (border == 0)
                oPdfPCell.Border = 0;
            if (height > 0)
                oPdfPCell.MinimumHeight = height;

            foreach (string oItem in BorderWidth)
            {
                switch (oItem.ToUpper())
                {
                    case "TOP": oPdfPCell.BorderWidthTop = 1; break;
                    case "BOTTOM": oPdfPCell.BorderWidthBottom = 1; break;
                    case "LEFT": oPdfPCell.BorderWidthLeft = 1; break;
                    case "RIGHT": oPdfPCell.BorderWidthRight = 1; break;
                }
            }

            return oPdfPCell;
        }
    }



}
