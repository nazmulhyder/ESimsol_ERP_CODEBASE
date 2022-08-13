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
    public class rptSparePartsRequest
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_Bold;
        iTextSharp.text.Font _oFontStyle_UnLine;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        Company _oCompany = new Company();
        PurchaseRequisition _oPurchaseRequisition = new PurchaseRequisition();
        List<PurchaseRequisitionDetail> _oPurchaseRequisitionDetails = new List<PurchaseRequisitionDetail>();
        Contractor _oContractor = new Contractor();
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
        List<ApprovalHead> _oApprovalHeads = new List<ApprovalHead>();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        #endregion

        #region PurchaseRequisition LC
        public byte[] PrepareReport(PurchaseRequisition oPurchaseRequisition, BusinessUnit oBusinessUnit, Company oCompany, ClientOperationSetting oClientOperationSetting, List<SignatureSetup> oSignatureSetups, List<ApprovalHead> oApprovalHead)
        {
            _oPurchaseRequisition = oPurchaseRequisition;
            _oPurchaseRequisitionDetails = oPurchaseRequisition.PurchaseRequisitionDetails;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oClientOperationSetting = oClientOperationSetting;
            _oSignatureSetups = oSignatureSetups;
            _oApprovalHeads = oApprovalHead;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//595*842
            _oDocument.SetMargins(30f, 30f, 30f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            //PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //ESimSolFooter PageEventHandler = new ESimSolFooter();

            //List<string> signatureList = new List<string>();
            //signatureList.Add("Prepared By");
            //for (int i = 0; i < _oApprovalHeads.Count; i++)
            //{
            //    signatureList.Add(_oApprovalHeads[i].Name);
            //}

            //PageEventHandler.signatures = signatureList;
            //PageEventHandler.nFontSize = 9;
            //PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    595 //Articale
                                              });
            #endregion
         
            this.PrintHeader();
            this.PrintBody();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
     
        #region Report Header
        public void PrintHeader()
        {
            ESimSolPdfHelper.PrintHeader_Audi(ref _oPdfPTable, _oBusinessUnit,_oCompany, new string[] {"Spare Parts Request Form", "Audi Service Dhaka", "Audi", "429/432, Tejgaon I/A, Dhaka-1208", "Service"},0 );

            //PdfPTable oPdfPTableComDetail = new PdfPTable(2);
            //oPdfPTableComDetail.WidthPercentage = 100;
            //oPdfPTableComDetail.SetWidths(new float[] { 525f, 70f });
            //oPdfPTableComDetail.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTableComDetail.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            //#region Header Into Main Table
            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("Spare Parts Request Form", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            //_oPdfPTable.CompleteRow();
            //oPdfPTableComDetail.AddCell(_oPdfPCell);
            //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            //#endregion
            //#region CompanyLogo
            //_oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
            //_oImag.ScaleAbsolute(40f, 25f);
            //_oPdfPCell = new PdfPCell(_oImag);
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.FixedHeight = 35f;
            //oPdfPTableComDetail.AddCell(_oPdfPCell);
            //oPdfPTableComDetail.CompleteRow();
            //#endregion
            //#region CompanyDetails
            //_oPdfPCell = new PdfPCell(new Phrase("Audi Service Dhaka", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPTableComDetail.AddCell(_oPdfPCell);

            // _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED);
            //_oPdfPCell = new PdfPCell(new Phrase("Audi", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            //oPdfPTableComDetail.AddCell(_oPdfPCell);
            //oPdfPTableComDetail.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            //_oPdfPCell = new PdfPCell(new Phrase("429/432, Tejgaon I/A, Dhaka-1208", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPTableComDetail.AddCell(_oPdfPCell);

            //  _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED);
            //_oPdfPCell = new PdfPCell(new Phrase("Service", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            //oPdfPTableComDetail.AddCell(_oPdfPCell);
            //oPdfPTableComDetail.CompleteRow();
            //#endregion

            //#region Insert Into Main Table
            //_oPdfPCell = new PdfPCell(oPdfPTableComDetail);
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.Rowspan = 30;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //#endregion
        }
        #endregion

       
        private void PrintBody()
        {
            #region Report Details

            ESimSolPdfHelper.PrintHeaderInfo(ref _oPdfPTable, "", "Spare Parts Request Form", new string[][] {
                                    new string[] {"SPR-NO","Request Date","Request By"},
                                    new string[] {_oPurchaseRequisition.PRNo,_oPurchaseRequisition.PRDateST,_oPurchaseRequisition.RequisitionByName},
                                    new string[] {"Department","Require Date","Remarks"},
                                    new string[] {_oPurchaseRequisition.DepartmentName,_oPurchaseRequisition.RequirementDateSt,_oPurchaseRequisition.Note}
                                });

            #endregion

            #region Detail Column Header
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.SetWidths(new float[] { 15f,45f, 130f,30f, 45f,45f, 45f, 45f, 30f, 60f, });

            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("No", _oFontStyle_Bold)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Part No", _oFontStyle_Bold)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Parts description", _oFontStyle_Bold)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Model", _oFontStyle_Bold)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Last Supply", _oFontStyle_Bold)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Present Stock", _oFontStyle_Bold)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Require", _oFontStyle_Bold)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle_Bold)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Quantity", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Quantity", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Details Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
            int nCount = 0; double nTotalQty = 0;
            foreach (PurchaseRequisitionDetail oItem in _oPurchaseRequisitionDetails)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductCode, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                
                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ModelShortName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LastSupplyDateSt, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LastSupplyQtySt + " " + oItem.LastSupplyUnitName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PresentStock + " " + oItem.PresentStockUnitName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString(), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitSymbol, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
            }

            while (nCount<20)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Authorized Signature
            PdfPTable oPDFSignature = ESimSolSignature.GetSignature(535f, _oPurchaseRequisition, _oSignatureSetups, 30f); oPDFSignature.DefaultCell.Border = 0;
            _oPdfPCell = new PdfPCell(oPDFSignature);
            _oPdfPTable.AddCell(_oPdfPCell); _oPdfPCell.Border = 0;
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region HELPER
        public void AddCell(ref PdfPTable oPdfPTable, string sHeader, string sAlignment, bool isGray)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default

            if (sAlignment == "LEFT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "RIGHT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment == "CENTER")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            if (isGray)
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;

            oPdfPTable.AddCell(_oPdfPCell);
        }
        #endregion
    }
}
