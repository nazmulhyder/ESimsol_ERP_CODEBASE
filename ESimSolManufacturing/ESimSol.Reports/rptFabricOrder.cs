using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
 
 


namespace ESimSol.Reports
{


    public class rptFabricOrder
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(9);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Fabric _oFabric = new Fabric();
        List<Fabric> _oFabrics = new List<Fabric>();
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
        Company _oCompany = new Company();
        List<FNLabDipDetail> _oFNLabDipDetails = new List<FNLabDipDetail>();
        #endregion

        public byte[] PrepareReport(Fabric oFabric, FabricOrderSetup oFabricOrderSetup,List<FNLabDipDetail> oFNLabDipDetails, Company oCompany, BusinessUnit oBusinessUnit)
        {
            _oFabrics = oFabric.Fabrics;
            _oCompany = oCompany;
            _oFNLabDipDetails = oFNLabDipDetails;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
           
            _oPdfPTable.SetWidths(new float[] {
                                                15,95,50,
                                                50,90,90,45,
                                                60,100
                                                    });
            #endregion

            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oBusinessUnit, oCompany,  oFabricOrderSetup.POPrintName, 9);

            this.PrintBody();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody()
        {
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 9, 0, 15);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Buyer : " + string.Join(", ", _oFabrics.Select(x => x.BuyerName).Distinct()), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 4, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 4, 0);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Date: " + string.Join(", ", _oFabrics.Select(x => x.IssueDateInString).Distinct()), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 9, 0, 15);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "SL", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Description Of Fabrics", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Mkt. Ref.", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Previous Ref.", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Customer Reference", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15, 0, 2, 0);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Style", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Color", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Weave Type", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "FinishType", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Remarks", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            
            int nCount = 0;
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            foreach (Fabric oItem in _oFabrics)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));

                ESimSolPdfHelper.AddCell(ref _oPdfPTable, nCount.ToString(), Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.ProductName +" "+ oItem.ProcessTypeName + " \nConstruction- " + oItem.Construction, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.FabricNo, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.BuyerReference, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.StyleNo, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);


                if (_oFNLabDipDetails.Where(x => x.FabricID == oItem.FabricID).Any()) 
                {
                    PdfPTable oPdfPTable = new PdfPTable(1);
                    foreach (var oColor in _oFNLabDipDetails.Where(x => x.FabricID == oItem.FabricID))
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, oColor.ColorName, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                    }
                    ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0);
                }
                else ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.FabricWeaveName, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.FinishTypeName, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.Remarks, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                _oPdfPTable.CompleteRow();
            }

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 9, 0);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 9, 0);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma",6f, 1);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Delivery Date    : " + string.Join(", ", _oFabrics.Select(x => x.SeekingSubmissionDateInString).Distinct()), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 9, 0);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 9, 0, 15);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Special Note -", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 9, 0);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            
            string sSecondaryLightSource= string.Join(", ", _oFabrics.Where(x => !String.IsNullOrEmpty(x.SecondaryLightSource)).Select(x => x.SecondaryLightSource).Distinct());

            if(String.IsNullOrEmpty(sSecondaryLightSource))
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Light Source         : " + string.Join(", ", _oFabrics.Where(x => !String.IsNullOrEmpty(x.PrimaryLightSource)).Select(x => x.PrimaryLightSource).Distinct()), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 9, 0);
            else
            {
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Light Source (P)   : " + string.Join(", ", _oFabrics.Where(x => !String.IsNullOrEmpty(x.PrimaryLightSource)).Select(x => x.PrimaryLightSource).Distinct()), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 9, 0);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Light Source (S)   : " + sSecondaryLightSource, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 9, 0);
            }
            ESimSolPdfHelper.AddCell(ref _oPdfPTable,     "End Use               : " + string.Join(", ", _oFabrics.Where(x => !String.IsNullOrEmpty(x.EndUse)).Select(x => x.EndUse.ToUpper()).Distinct()), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 9, 0);


            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = _oFabrics.Select(x => x.PrepareByName).FirstOrDefault() + "\nPrepared & Check By" });
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption =_oFabrics.Select(x => x.MKTPersonName).FirstOrDefault() + " \nTeam Leader \n " + _oCompany.Name });
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption =_oFabrics.Select(x => x.ApprovedByNameSt).FirstOrDefault() + "\nApproved By\n " + _oCompany.Name });

            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 600-ESimSolPdfHelper.CalculatePdfPTableHeight(_oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Authorized Signature
            _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(140f, _oFabric, _oSignatureSetups, 35.0f));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.FixedHeight = 140f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
    }
}
