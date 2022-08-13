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
    public class tempClass
    {
        public string pTerm { get; set; }
        public double dTotal { get; set; }
    }

    public class rptNOAFormat1
    {
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_Bold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        Company _oCompany = new Company();

        List<tempClass> summery = new List<tempClass>();

        NOA _oNOA = new NOA();
        List<NOADetail> _oNOADetails = new List<NOADetail>();
        Contractor _oContractor = new Contractor();
        List<SupplierRateProcess> _oSupplierRateProcess = new List<SupplierRateProcess>();
        List<PurchaseQuotationDetail> _oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
        List<NOAQuotation> _oNOAQuotations = new List<NOAQuotation>();
        List<NOAQuotation> _oNOADistinctSuppliers = new List<NOAQuotation>();
        List<NOAQuotation> oTempNOAQuotations = new List<NOAQuotation>();
        NOAQuotation _oNOAQuotation = new NOAQuotation();
        List<NOASpec> _NOASpecs = new List<NOASpec>();
        List<NOASignatory> _oNOASignatorys = new List<NOASignatory>();
        List<NOASignatoryComment> _oNOASignatoryComments = new List<NOASignatoryComment>();
        ClientOperationSetting _oClientOperationSettingNOA = new ClientOperationSetting();

        public byte[] PrepareReport(NOA oNOA, BusinessUnit oBusinessUnit, Company oCompany, ClientOperationSetting oClientOperationSetting, List<SignatureSetup> oSignatureSetups, ClientOperationSetting oClientOperationSettingNOA, bool bWithTechHead, List<NOASignatory> oNOASignatorys, bool bIsNormalFormat)
        {
            _oClientOperationSettingNOA = oClientOperationSettingNOA;

            _oNOA = oNOA;
            _oNOADetails = oNOA.NOADetailLst;
            _oNOASignatoryComments = oNOA.NOASignatoryComments;
            _oPurchaseQuotationDetails = oNOA.PurchaseQuotationDetailList;
            _oNOAQuotations = oNOA.NOAQuotationList;
            _NOASpecs = _oNOA.NOASpecs;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oNOASignatorys = oNOASignatorys;
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());//842*595
            
            _oDocument.SetMargins(30f, 30f, 20f, 70f);//footer value Large for double page problem soluation
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
         
                ESimSolFooter PageEventHandler = new ESimSolFooter();
                if (bIsNormalFormat)
                {
                  
                    PageEventHandler.signatures = new List<string>(new string[] {_oNOA.PrepareByName,       "",             "",               "",         _oNOA.ApprovedByName  });
                    PageEventHandler.SignatureName = new List<string>(new string[] { "Prepared By", "Section Incharge", "User Department", "Audit Department", "Approved By" });
                }
                else
                {

                    if (oNOASignatorys.Count > 0)
                    {

                        foreach (NOASignatory oItem in oNOASignatorys)
                        {
                            if (oItem.ApproveBy == 0) { oItem.Name_Request = " "; }
                        }

                        string sTemp = "";
                        string[] sTopLeftRight = new string[] { };
                        sTemp = string.Join("~", oNOASignatorys.Where(p => p.ApproveBy >= 0).ToList().Select(x => x.Name_Request).ToList());
                        sTopLeftRight = sTemp.Split('~');
                        PageEventHandler.signatures = new List<string>(sTopLeftRight);
                        sTemp = string.Join("~", oNOASignatorys.Where(p => p.ApproveBy >= 0).ToList().Select(x => x.HeadName).ToList());
                        sTopLeftRight = sTemp.Split('~');
                        PageEventHandler.SignatureName = new List<string>(sTopLeftRight);
                    }
                    else
                    {

                        if (bWithTechHead)
                        {
                            PageEventHandler.signatures = new List<string>(new string[] { "Prepared By", "Section Incharge", "User Department", "Audit Department", "CFO", "Technical Head", "Authorised By" });
                        }
                        else
                        {
                            PageEventHandler.signatures = new List<string>(new string[] { "Prepared By", "Section Incharge", "User Department", "Audit Department", "Authorised By" });
                        }

                    }
                }
                PageEventHandler.nFontSize = 9;
                PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
        
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    842 //Articale
                                                 
                                              });

            this.PrintHeader();
            this.PrintBody();
            SetNote();
            if (oNOASignatorys.Count > 0)
            {
                this.PrintNOASignatoryComment();
            }
            _oPdfPTable.HeaderRows = 1;
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
            oPdfPTable.SetWidths(new float[] { 140f, 250f, 95f });

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
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 19f, 1);

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
       
            #endregion

            int nDistinctSuppliers = _oNOAQuotations.GroupBy(x => x.SupplierID).Select(group => new NOAQuotation
                {
                    SupplierID = group.First().SupplierID,
                    SupplierName = group.First().SupplierName,
                    ShortName = group.First().ShortName,
                    PQID = group.First().PQID
                }).OrderBy(x => x.SupplierID).ToList().Count();

            string sCaption = nDistinctSuppliers == 1 ? "Unique Statement(US)" : "Comparative Statement(CS)";
            string sRefNoCaption = nDistinctSuppliers ==1 ? "US No :" :"CS No :" ;

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oClientOperationSettingNOA.Value == "CS" ? "                        " + sCaption : "                        Note Of Approval", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oClientOperationSettingNOA.Value == "CS" ? sRefNoCaption + _oNOA.NOANo : " NOA No : " + _oNOA.NOANo, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void SetNote()
        {
            #region PO Info
            if (!string.IsNullOrEmpty(_oNOA.Note))
            {
                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] {50f, 150f, 80f, 170f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Note", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable,_oNOA.Note, 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL));
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);
            }
            #endregion


        }
        private void PrintBody()
        {


            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);
            _oNOADistinctSuppliers = new List<NOAQuotation>();

            _oNOADistinctSuppliers = _oNOAQuotations.GroupBy(x => x.SupplierID).Select(group => new NOAQuotation
                {
                    SupplierID = group.First().SupplierID,
                    SupplierName = group.First().SupplierName,
                    ShortName = group.First().ShortName,
                    PQID = group.First().PQID,
                    PQNo = group.First().PQNo
                }).OrderBy(x => x.SupplierID).ToList();

            int nMaxCount = 0;
            if (_oNOAQuotations.Count > 0)
            {
                nMaxCount = _oNOAQuotations.GroupBy(x => x.NOADetailID).Max(grp => grp.Count()); //find Maximum Supplier with there Specification
            }
            //When same Supplier but different type of Specification exist for single product, In this case nMaxCount Value will be greater than distinct suppliers other wise 
            if(nMaxCount<_oNOADistinctSuppliers.Count())
            {
                nMaxCount = _oNOADistinctSuppliers.Count();
            }
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 324f, 518});


            int nCoulumnLength = 9, ncolumncount = 7;
            int nMaxSupplierCount = nMaxCount * 2;// RAte  & Amount so multiply with 2

            nCoulumnLength += nMaxSupplierCount;

            oPdfPTable = new PdfPTable(nCoulumnLength);
            float[] dCollumn = new float[nCoulumnLength];

            dCollumn[0] = 29;//SL No
            dCollumn[1] = 60; //MPR No
            dCollumn[2] = 120; //Item
            dCollumn[3] = 35; //QTy
            dCollumn[4] = 30; //Unit
            dCollumn[5] = 35; //Currency
            dCollumn[6] = 35; //LPP
            dCollumn[7] = 30; //Date

            if (_oNOADistinctSuppliers.Count > 0)
            {
                for (int i = 0; i < nMaxSupplierCount; i++)
                {
                    if (i % 2 == 0)
                    {
                        ncolumncount++;
                        dCollumn[ncolumncount] = 35;
                    }
                    else
                    {
                        ncolumncount++;
                        dCollumn[ncolumncount] = 45;
                    }

                }
            }
            ncolumncount++;
            dCollumn[ncolumncount] = 45;//total

            oPdfPTable.SetWidths(dCollumn);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);


            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
            _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MPR No", _oFontStyle));
            _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Item", _oFontStyle));
            _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle));
            _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Currency", _oFontStyle));
            _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("LPP", _oFontStyle));
            _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
            _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            string sPQNO="";
            if (_oNOADistinctSuppliers.Count > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Supplier", _oFontStyle));
                _oPdfPCell.Colspan = nMaxSupplierCount; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                foreach (NOAQuotation oNOAQ in _oNOADistinctSuppliers)
                {
                //    if (_oPurchaseQuotationDetails.FirstOrDefault() != null && _oPurchaseQuotationDetails.FirstOrDefault().PurchaseQuotationDetailID > 0 && _oPurchaseQuotationDetails.Where(b => (b.PurchaseQuotationDetailID ==oNOAQ.PQDetailID)).Count() > 0)
                //         {
                //             sPQNO = _oPurchaseQuotationDetails.Where(x => x.PurchaseQuotationDetailID == oNOAQ.PQDetailID && x.SupplierID == oNOAQ.SupplierID).FirstOrDefault().PurchaseQuotationNo; 
                //}
                       //    oTempNOAQuotations = _oNOAQuotations.Where(x => x.NOADetailID == _oNOADetails[i].NOADetailID).ToList(); 
                    if (!string.IsNullOrEmpty(oNOAQ.PQNo) && oNOAQ.PQNo.Length > 6)
                    {
                        sPQNO = oNOAQ.PQNo.Substring(0, oNOAQ.PQNo.Length - 5);
                        //if (sPQNO.Length > 3)
                        //{
                        //    sPQNO = oNOAQ.PQNo.Substring(sPQNO.Length, sPQNO.Length - 2);
                        //}
                    }

                    int nTempColSpan = _oNOAQuotations.Where(x => x.SupplierID == oNOAQ.SupplierID).GroupBy(x => new { x.SupplierID, x.NOADetailID }).Max(grp => grp.Count());
                    _oPdfPCell = new PdfPCell(new Phrase(oNOAQ.SupplierName + " " + sPQNO, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = nTempColSpan*2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }
                oPdfPTable.CompleteRow();

                for (int i = 0; i < nMaxCount; i++)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Rate", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }
                oPdfPTable.CompleteRow();
            }
            if (_oNOADistinctSuppliers.Count == 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();

            //Main data print
            int nSL = 0;
            double nTempApprovedAmount = 0, nTotalApprovedAmount = 0;
            for (int i = 0; i < _oNOADetails.Count; i++) 
            {
                nSL++;

                _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oNOADetails[i].MPRNO, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                string tempProductName=_oNOADetails[i].ProductName;
                var tempSpecsName =_NOASpecs.Where(x=>x.NOADetailID == _oNOADetails[i].NOADetailID).ToList();
                if(tempSpecsName.Any()){
                    tempProductName = tempProductName + Environment.NewLine + string.Join(",",tempSpecsName.Select(x=>x.SpecName +" : " +x.NOADescription));
                }

                _oPdfPCell = new PdfPCell(new Phrase(tempProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oNOADetails[i].PurchaseQty.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oNOADetails[i].UnitSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(getCurrencySymbol(_oNOADetails[i].ProductID), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oNOADetails[i].LPP.ToString("#,##0.####"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oNOA.NOADate.ToString("dd MMM yyyy"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                #region Horizontaly Product, specification with Supplier wise value Print

                oTempNOAQuotations = new List<NOAQuotation>();
                oTempNOAQuotations = _oNOAQuotations.Where(x => x.NOADetailID == _oNOADetails[i].NOADetailID).ToList(); //find full list for specific NOA Detail
                string sSupplierIDs = string.Join(",", oTempNOAQuotations.Select(x => x.SupplierID));
                oTempNOAQuotations.AddRange(_oNOADistinctSuppliers.Where(x => !sSupplierIDs.Contains(x.SupplierID.ToString())).ToList());
                oTempNOAQuotations = oTempNOAQuotations.OrderBy(x => x.SupplierID).ToList();
                while (oTempNOAQuotations.Count < nMaxCount)
                {
                    _oNOAQuotation = new NOAQuotation();
                    oTempNOAQuotations.Add(_oNOAQuotation);
                }
                for (int j = 0; j < oTempNOAQuotations.Count; j++)
                {
                    //double dRate = getUnitRate(_oNOADistinctSuppliers[j].SupplierID, _oNOADetails[i].ProductID);
                    _oPdfPCell = new PdfPCell(new Phrase((oTempNOAQuotations[j].UnitPrice).ToString("#,##0.####"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    if (_oNOA.ApproveBy != 0 ? checkApproveRate(oTempNOAQuotations[j].PQDetailID) : checkTemporaryApproveRate(oTempNOAQuotations[j].PQDetailID))
                    {
                        _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                    }
                    else
                    {
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    }
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase((oTempNOAQuotations[j].UnitPrice * _oNOADetails[i].PurchaseQty).ToString("#,##0.####"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    if (checkApproveRate(oTempNOAQuotations[j].PQDetailID))
                    {
                        _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        nTempApprovedAmount = oTempNOAQuotations[j].UnitPrice * _oNOADetails[i].PurchaseQty;
                        nTotalApprovedAmount += oTempNOAQuotations[j].UnitPrice * _oNOADetails[i].PurchaseQty;
                    }
                    else _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(_oPdfPCell);

                }
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(nTempApprovedAmount.ToString("#,##0.####"), _oFontStyle));
                _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                #region Horizontaly Specification Print for PRoduct
                for (int j = 0; j < oTempNOAQuotations.Count; j++)
                {

                    string specs = string.Join(" , ", _oNOA.PQSpecs.Where(x=>x.PQDetailID==oTempNOAQuotations[j].PQDetailID).Select(x => x.SpecName + " : " + x.PQDescription));  // getspecs(_oNOADistinctSuppliers[j].SupplierID, _oNOADetails[i].ProductID);
                    _oPdfPCell = new PdfPCell(new Phrase(specs, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Colspan = 2;
                    oPdfPTable.AddCell(_oPdfPCell);
                }
                #endregion
                oPdfPTable.CompleteRow();
            }

            #region Horizontaly sub total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total", _oFontStyle));
            _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            for (int i = 0; i < _oNOADistinctSuppliers.Count; i++)
            {
                int nTempColSpan = _oNOAQuotations.Where(x => x.SupplierID == _oNOADistinctSuppliers[i].SupplierID).GroupBy(x => new { x.SupplierID, x.NOADetailID }).Max(grp => grp.Count());
                double nSupplierWiseTotalAmount = 0;
                for (int j = 0; j < _oNOADetails.Count; j++)
                {
                    oTempNOAQuotations = _oNOAQuotations.Where(x => x.SupplierID == _oNOADistinctSuppliers[i].SupplierID & x.NOADetailID == _oNOADetails[j].NOADetailID).ToList();
                    foreach (NOAQuotation oItem in oTempNOAQuotations)
                    {
                        nSupplierWiseTotalAmount += (oItem.UnitPrice) * _oNOADetails[j].PurchaseQty;
                    }
                }

                _oPdfPCell = new PdfPCell(new Phrase(nSupplierWiseTotalAmount.ToString("#,##0.####"), _oFontStyle));
                _oPdfPCell.Colspan = nTempColSpan * 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase((nTotalApprovedAmount).ToString("#,##0.####"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            
            #region Horizontaly Vat
            _oPdfPCell = new PdfPCell(new Phrase("Vat", _oFontStyle));
            _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            for (int i = 0; i < _oNOADistinctSuppliers.Count; i++)
            {
                int nTempColSpan = _oNOAQuotations.Where(x => x.SupplierID == _oNOADistinctSuppliers[i].SupplierID).GroupBy(x => new { x.SupplierID, x.NOADetailID }).Max(grp => grp.Count());
                List<PurchaseQuotationDetail> oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
                oPurchaseQuotationDetails = _oPurchaseQuotationDetails.Where(x => x.PurchaseQuotationID == _oNOADistinctSuppliers[i].PQID).OrderBy(x => x.PurchaseQuotationDetailID).ToList();
                oTempNOAQuotations = _oNOAQuotations.Where(x => x.PQID == _oNOADistinctSuppliers[i].PQID).OrderBy(x => x.PQDetailID).ToList();

                string sPQDetailIs = string.Join(",", oPurchaseQuotationDetails.Select(x => x.PurchaseQuotationDetailID.ToString()));
                if (QuotationExists(sPQDetailIs, oTempNOAQuotations) && oTempNOAQuotations[0].VatInAmount > 0)
                {
                    //Set these value for next Calculation
                    _oNOADistinctSuppliers[i].VatInPercent = oTempNOAQuotations[0].VatInPercent;
                    _oNOADistinctSuppliers[i].VatInAmount = oTempNOAQuotations[0].VatInAmount;

                    _oPdfPCell = new PdfPCell(new Phrase(oTempNOAQuotations[0].VatInPercent.ToString("#,##,##0.00##") + " % ", _oFontStyle));
                    _oPdfPCell.Colspan = nTempColSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTempNOAQuotations[0].VatInAmount > 0 ? "+ " + oTempNOAQuotations[0].VatInAmount.ToString("#,##0.####") : "-", _oFontStyle));
                    _oPdfPCell.Colspan = nTempColSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                    _oPdfPCell.Colspan = nTempColSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                    _oPdfPCell.Colspan = nTempColSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }
            }

            _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion
            #region Horizontaly TransPort Cost
            _oPdfPCell = new PdfPCell(new Phrase("TransPort Cost", _oFontStyle));
            _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            for (int i = 0; i < _oNOADistinctSuppliers.Count; i++)
            {
                int nTempColSpan = _oNOAQuotations.Where(x => x.SupplierID == _oNOADistinctSuppliers[i].SupplierID).GroupBy(x => new { x.SupplierID, x.NOADetailID }).Max(grp => grp.Count());
                List<PurchaseQuotationDetail> oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
                oPurchaseQuotationDetails = _oPurchaseQuotationDetails.Where(x => x.PurchaseQuotationID == _oNOADistinctSuppliers[i].PQID).OrderBy(x => x.PurchaseQuotationDetailID).ToList();
                oTempNOAQuotations = _oNOAQuotations.Where(x => x.PQID == _oNOADistinctSuppliers[i].PQID).OrderBy(x => x.PQDetailID).ToList();

                string sPQDetailIs = string.Join(",", oPurchaseQuotationDetails.Select(x => x.PurchaseQuotationDetailID.ToString()));
                if (QuotationExists(sPQDetailIs, oTempNOAQuotations) && oTempNOAQuotations[0].TransportCostInAmount > 0)
                {
                    //Set these value for next Calculation
                    _oNOADistinctSuppliers[i].TransportCostInPercent = oTempNOAQuotations[0].TransportCostInPercent;
                    _oNOADistinctSuppliers[i].TransportCostInAmount = oTempNOAQuotations[0].TransportCostInAmount;

                    _oPdfPCell = new PdfPCell(new Phrase(oTempNOAQuotations[0].TransportCostInPercent.ToString("#,##,##0.00##") + " % ", _oFontStyle));
                    _oPdfPCell.Colspan = nTempColSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTempNOAQuotations[0].TransportCostInAmount > 0 ? "+ " + oTempNOAQuotations[0].TransportCostInAmount.ToString("#,##0.####") : "-", _oFontStyle));
                    _oPdfPCell.Colspan = nTempColSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                    _oPdfPCell.Colspan = nTempColSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                    _oPdfPCell.Colspan = nTempColSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }
            }

            _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Horizontaly Special Honor
            _oPdfPCell = new PdfPCell(new Phrase("Special Honor", _oFontStyle));
            _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            for (int i = 0; i < _oNOADistinctSuppliers.Count; i++)
            {
                int nTempColSpan = _oNOAQuotations.Where(x => x.SupplierID == _oNOADistinctSuppliers[i].SupplierID).GroupBy(x => new { x.SupplierID, x.NOADetailID }).Max(grp => grp.Count());
                List<PurchaseQuotationDetail> oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
                oPurchaseQuotationDetails = _oPurchaseQuotationDetails.Where(x => x.PurchaseQuotationID == _oNOADistinctSuppliers[i].PQID).OrderBy(x => x.PurchaseQuotationDetailID).ToList();
                oTempNOAQuotations = _oNOAQuotations.Where(x => x.PQID == _oNOADistinctSuppliers[i].PQID).OrderBy(x => x.PQDetailID).ToList();

                string sPQDetailIs = string.Join(",", oPurchaseQuotationDetails.Select(x => x.PurchaseQuotationDetailID.ToString()));
                if (string.Join(",", oTempNOAQuotations.Select(x => x.PQDetailID.ToString())).Contains(sPQDetailIs) && oTempNOAQuotations[0].DiscountInAmount > 0)
                {
                    //Set these value for next Calculation
                    _oNOADistinctSuppliers[i].DiscountInpercent = oTempNOAQuotations[0].DiscountInpercent;
                    _oNOADistinctSuppliers[i].DiscountInAmount = oTempNOAQuotations[0].DiscountInAmount;

                    _oPdfPCell = new PdfPCell(new Phrase(oTempNOAQuotations[0].DiscountInpercent.ToString() + " % ", _oFontStyle));
                    _oPdfPCell.Colspan = nTempColSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTempNOAQuotations[0].DiscountInAmount > 0 ? "- " + oTempNOAQuotations[0].DiscountInAmount.ToString("#,##0.####") : "-", _oFontStyle));
                    _oPdfPCell.Colspan = nTempColSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                    _oPdfPCell.Colspan = nTempColSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                    _oPdfPCell.Colspan = nTempColSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }
            }

            _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Horizontaly Sub total

            _oPdfPCell = new PdfPCell(new Phrase("Payble Amount", _oFontStyle));
            _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            for (int i = 0; i < _oNOADistinctSuppliers.Count; i++)
            {
                int nTempColSpan = _oNOAQuotations.Where(x => x.SupplierID == _oNOADistinctSuppliers[i].SupplierID).GroupBy(x => new { x.SupplierID, x.NOADetailID }).Max(grp => grp.Count());
                double nSupplierWiseTotalAmount = 0;
                for (int j = 0; j < _oNOADetails.Count; j++)
                {
                    oTempNOAQuotations = _oNOAQuotations.Where(x => x.SupplierID == _oNOADistinctSuppliers[i].SupplierID & x.NOADetailID == _oNOADetails[j].NOADetailID).ToList();
                    foreach (NOAQuotation oItem in oTempNOAQuotations)
                    {
                        nSupplierWiseTotalAmount += (oItem.UnitPrice) * _oNOADetails[j].PurchaseQty;
                    }
                }


                _oPdfPCell = new PdfPCell(new Phrase(((nSupplierWiseTotalAmount + _oNOADistinctSuppliers[i].VatInAmount + _oNOADistinctSuppliers[i].TransportCostInAmount) - _oNOADistinctSuppliers[i].DiscountInAmount).ToString("#,##0.####"), _oFontStyle));
                _oPdfPCell.Colspan = nTempColSpan * 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Horizontaly Final Selection

            _oPdfPCell = new PdfPCell(new Phrase("Final Selection", _oFontStyle));
            _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            for (int i = 0; i < _oNOADistinctSuppliers.Count; i++)
            {
                int nTempColSpan = _oNOAQuotations.Where(x => x.SupplierID == _oNOADistinctSuppliers[i].SupplierID).GroupBy(x => new { x.SupplierID, x.NOADetailID }).Max(grp => grp.Count());
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                _oPdfPCell.Colspan = nTempColSpan*2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Horizontaly PAyment Method
            _oPdfPCell = new PdfPCell(new Phrase("Payment Mode", _oFontStyle));
            _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            for (int i = 0; i < _oNOADistinctSuppliers.Count; i++)
            {
                int nTempColSpan = _oNOAQuotations.Where(x => x.SupplierID == _oNOADistinctSuppliers[i].SupplierID).GroupBy(x => new { x.SupplierID, x.NOADetailID }).Max(grp => grp.Count());
                _oPdfPCell = new PdfPCell(new Phrase(getPaymentTerm(_oNOADistinctSuppliers[i].SupplierID), _oFontStyle));
                _oPdfPCell.Colspan = nTempColSpan* 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private bool QuotationExists(string sPQDetailIs, List<NOAQuotation> oNOAQuotations)
        {
            foreach (NOAQuotation oItem in oNOAQuotations)
            {
                if (sPQDetailIs.Contains(oItem.PQDetailID.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        private void PrintNOASignatoryComment()
        {
            #region Balnk Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 4f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            #region Balnk Space
            _oPdfPCell = new PdfPCell(new Phrase("Comment(s)", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE;  _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

         

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 90f, 105f, 100f, 80f });

            if (!string.IsNullOrEmpty(_oNOA.Note))
            {
                oPdfPCell = new PdfPCell(new Phrase(_oNOA.PrepareByName, _oFontStyle));
                //oPdfPCell.Colspan = 3;
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(": " + _oNOA.Note, _oFontStyle));
                oPdfPCell.Colspan = 3;
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            foreach (NOASignatory oItem in _oNOASignatorys)
            {
                if (!string.IsNullOrEmpty(oItem.Note))
                {
                    oPdfPCell = new PdfPCell(new Phrase(oItem.Name_Request, _oFontStyle));
                    //oPdfPCell.Colspan = 3;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(": "+oItem.Note, _oFontStyle));
                    oPdfPCell.Colspan = 3;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

        }
        private bool checkApproveRate(int PQDetailID)
        {
            bool flag = false;
            if(PQDetailID==0) return false;
            for (int i = 0; i < _oNOADetails.Count; i++)
            {
                    if ((_oNOADetails[i].PQDetailID == PQDetailID))
                    {
                        flag = true;
                        break;
                    }
            }
            return flag;
        }


        private bool checkTemporaryApproveRate(int PQDetailID)
        {
            bool flag = false;
            if(PQDetailID==0) return false;
            for (int i = 0; i < _oNOASignatoryComments.Count; i++)
            {
                     if ((_oNOASignatoryComments[i].PQDetailID == PQDetailID))
                    {
                        flag = true;
                        break;
                    }
            }
            return flag;
        }
        private string getCurrencySymbol(int nProductID)
        {
            string symbol = "";
            for (int i = 0; i < _oPurchaseQuotationDetails.Count; i++)
            {
                if (_oPurchaseQuotationDetails[i].ProductID == nProductID)
                {
                    symbol = _oPurchaseQuotationDetails[i].CurrencySymbol;
                    break;
                }
            }
            return symbol;
        }
        private double getUnitRate(int nSuppID, int nProductID)
        {
            double rate = 0.0;

            for (int i = 0; i < _oPurchaseQuotationDetails.Count; i++)
            {
                if (_oPurchaseQuotationDetails[i].SupplierID == nSuppID && _oPurchaseQuotationDetails[i].ProductID == nProductID)
                {
                    rate = _oPurchaseQuotationDetails[i].UnitPrice;
                    break;
                }
            }

            return rate;
        }
        private string getPaymentTerm(int nSuppID)
        {
            string term = "";

            for (int i = 0; i < _oPurchaseQuotationDetails.Count; i++)
            {
                if (_oPurchaseQuotationDetails[i].SupplierID == nSuppID)
                {
                    term = _oPurchaseQuotationDetails[i].PaymentTerm;
                    break;
                }
            }

            return term;
        }

      
    }
}


