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

    public class rptReportRnD
    {
        #region Declaration
        int _nTotalColumn = 4;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImg;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<FabricSCReport> _oFabricSCReports = new List<FabricSCReport>();
        #endregion

        public byte[] PrepareReport(Company oCompany, BusinessUnit oBusinessUnit, List<FabricSCReport> oFabricSCReports)
        {

            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oFabricSCReports = oFabricSCReports;

            #region Page Setup
            _oDocument = new Document(PageSize.A4.Rotate(), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 842f});
            #endregion
            this.PrintHeaderWithLogo();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintHeaderWithLogo()
        {
            #region CompanyHeader
            if (_oCompany.CompanyLogo != null)
            {
                _oImg = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImg.ScaleAbsolute(160f, 35f);
                _oPdfPCell = new PdfPCell(_oImg);
                _oPdfPCell.Colspan = 5;
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
                _oPdfPCell.Colspan = 5;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Colspan = 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Bulk PO Received Formet", _oFontStyle));
            _oPdfPCell.Colspan = 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBody()
        {
            PdfPTable oPdfPTable = new PdfPTable(23);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] 
            { 
                35f,35f,35f,35f,35f,35f,35f,35f,35f,35f,35f,35f,35f,35f,35f,35f,35f,35f,35f,35f,35f,35f,35f
            });

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);


            oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("PO Issue Date", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Receive Date", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("PO No", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("MKT Team", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Garments Name", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Constraction", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Composition", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Fabric Code", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Fabric Type", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Wave Type", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Finish Type", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Lab Dip", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Color Qty", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("MKT ef", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Buyer Ref", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Width", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Production Status", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            //_oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

            int nCount = 0;
            if (_oFabricSCReports.Count() > 0)
            {
                foreach (FabricSCReport oItem in _oFabricSCReports)
                {
                    nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.SCDateSt, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.FabricReceiveDateStr, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.HLReference, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.SCNoFull, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.MKTGroup, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Construction, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Code, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ProcessTypeName, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.FabricWeaveName, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.FinishTypeName, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ColorInfo, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString(), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerReference, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.FabricWidth, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                }
                oPdfPTable.CompleteRow();


                oPdfPCell = new PdfPCell(oPdfPTable);
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(oPdfPCell);

            }
            _oPdfPTable.CompleteRow();
        }
    }
}
