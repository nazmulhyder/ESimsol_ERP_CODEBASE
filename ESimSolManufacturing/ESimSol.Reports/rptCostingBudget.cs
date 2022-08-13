using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using ICS.Core.Framework;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Linq;


namespace ESimSol.Reports
{

    public class rptCostingBudget
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle6Normal;
        iTextSharp.text.Font _oFontStyle6Bold;


        iTextSharp.text.Font _oFontStyle5Bold;
        iTextSharp.text.Font _oFontStyle5Normal;

        iTextSharp.text.Font _oFontStyle4Bold;
        iTextSharp.text.Font _oFontStyle4Normal;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(3);
        int nGlobalCol = 3;
        float nRegularFontSize = 8f;
        float nDynamicFontSize = 4.5f;
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        StyleBudget _oStyleBudget = new StyleBudget();
        StyleBudget _oStylePostBudget = new StyleBudget();
        List<StyleBudgetDetail> _oStyleBudgetYarnDetails = new List<StyleBudgetDetail>();
        List<StyleBudgetDetail> _oStyleBudgetAccessoriesDetails = new List<StyleBudgetDetail>();
        List<OrderRecapDetail> _oOrderRecapDetails = new List<OrderRecapDetail>();
        List<OrderRecapDetail> _oOrderRecapDetailsPost = new List<OrderRecapDetail>();
        TechnicalSheetImage _oTechnicalSheetImage = new TechnicalSheetImage();
        
        List<BodyMeasure> _oBodyMeasures = new List<BodyMeasure>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
        int nTempCount = 5; bool bHasLycraCost = false, bHasAOPCost = false, bHasWashCost = false, bHasYarnDyeingCost = false, bHasSuedeCost = false, bHasFinishingCost = false, bHasBrushingCost = false;
        double _nTotalValue = 0;
        double _nTotalValueWithOutBankCost = 0; 
        Company _oCompany = new Company();

        #endregion

        #region Cost Sheet
        public byte[] PrepareReport(StyleBudget oStyleBudget, StyleBudget oStylePostBudget, Company oCompany, BusinessUnit oBusinessUnit, List<BodyMeasure> oBodyMeasures, List<SignatureSetup> oSignatureSetups, List<OrderRecapDetail> oOrderRecapDetails, List<OrderRecapDetail> oORDlsForPostBudget)
        {
            _oStyleBudget = oStyleBudget;
            _oStylePostBudget = oStylePostBudget;
            _oTechnicalSheetImage = oStyleBudget.TechnicalSheetImage;
            _oStyleBudgetYarnDetails = oStyleBudget.StyleBudgetYarnDetails;
            _oStyleBudgetAccessoriesDetails = oStyleBudget.StyleBudgetAccessoriesDetails;
            _oOrderRecapDetails = oOrderRecapDetails;
            _oOrderRecapDetailsPost = oORDlsForPostBudget;//For Posting

            _oBodyMeasures = oBodyMeasures;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oSignatureSetups = oSignatureSetups;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());//842*595
            _oDocument.SetMargins(10f, 10f, 10f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 408f,6f,408f });//822
            //_oPdfPTable.SetWidths(new float[] { 40f,5f,110f, 70f, 60f, 5f, 95f,60f, 30f,30f,26f,34f });//595
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        //nothing
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
                _oImag.ScaleAbsolute(62f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.Colspan = nGlobalCol; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Colspan = nGlobalCol; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

          
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            
            #region Budget
            _oPdfPCell = new PdfPCell(PrintCosting(_oStyleBudget,true));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            #endregion
            #region Blank space
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;  _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;_oPdfPTable.AddCell(_oPdfPCell);
            
            #endregion
            #region Post Cost Sheet
            if (_oStyleBudget.PostStyleBudgetID > 0)
            {
                _oStyleBudgetYarnDetails = _oStylePostBudget.StyleBudgetYarnDetails;
                _oStyleBudgetAccessoriesDetails = _oStylePostBudget.StyleBudgetAccessoriesDetails;
            }
            else
            {
                //if not entry Post cost for formatting set default data with 0 value
                _oStylePostBudget = _oStyleBudget;//for Temporary
                bHasLycraCost = false; bHasAOPCost = false; bHasWashCost = false; bHasYarnDyeingCost = false; bHasSuedeCost = false; bHasFinishingCost = false; bHasBrushingCost = false;
                _oStyleBudget.TestPricePerDozen = 0; _oStyleBudget.PrintPricePerDozen = 0; _oStyleBudget.EmbrodaryPricePerDozen = 0; _oStyleBudget.CourierPricePerDozen = 0;
                _oStyleBudget.OthersPricePerDozen = 0; _oStyleBudget.BuyingCommission = 0; _oStyleBudget.BankingCost = 0; _oStyleBudget.CMCost = 0; _oStyleBudget.CMCostPerPcs = 0;
                _oStyleBudget.PreparedByName = ""; _oStyleBudget.ApprovedByName = ""; _oStyleBudget.FOBPricePerPcs = 0; _oStyleBudget.FOBPricePerDozen = 0;
                foreach (StyleBudgetDetail oItem in _oStyleBudgetYarnDetails)
                {
                    oItem.EstimatedCost = 0; oItem.MaterialMarketPrice = 0; oItem.KnittingCost = 0; oItem.DyeingCost = 0; oItem.LycraCost = 0; oItem.AOPCost = 0; oItem.WashCost = 0; oItem.SuedeCost = 0; oItem.BrushingCost = 0;
                }
                foreach (StyleBudgetDetail oItem in _oStyleBudgetAccessoriesDetails)
                {
                    oItem.MaterialMarketPrice = 0; oItem.EstimatedCost = 0;
                }
                
            }
            _oPdfPCell = new PdfPCell(PrintCosting(_oStylePostBudget, false));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            #endregion
            _oPdfPTable.CompleteRow();

        }
        #endregion

        public PdfPTable PrintCosting(StyleBudget oNewStyleBudget, bool bIsBudget)
        {

            _oFontStyle4Bold = FontFactory.GetFont("Tahoma", 4.5f, iTextSharp.text.Font.BOLD);
            _oFontStyle4Normal = FontFactory.GetFont("Tahoma", 4.5f, iTextSharp.text.Font.NORMAL);

            _oFontStyle5Bold = FontFactory.GetFont("Tahoma", 5f, iTextSharp.text.Font.BOLD);
            _oFontStyle5Normal = FontFactory.GetFont("Tahoma", 5f, iTextSharp.text.Font.NORMAL);

            _oFontStyle6Bold = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD);
            _oFontStyle6Normal = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);

            double _nTotalPercent = 0;
            PdfPTable oPdfPReturnTable = new PdfPTable(1);
            oPdfPReturnTable.SetWidths(new float[] { 408f });//408
            oPdfPReturnTable.WidthPercentage = 100;


            PdfPTable oPdfPStyleTable = new PdfPTable(8);
            oPdfPStyleTable.SetWidths(new float[] { 45f, 5f, 55f, 5f, 228f, 5f, 60f, 5f });//408
            oPdfPStyleTable.WidthPercentage = 100;

            _oFontStyle6Normal = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            #region Date

            _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.CostingDate.ToString("dd MMM yyyy"), _oFontStyle5Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(GRecapDetails(bIsBudget));
            _oPdfPCell.Rowspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(bIsBudget ? "Budget" : "Post Costing", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);
            oPdfPStyleTable.CompleteRow();
            #endregion

            #region Buyer

            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.BuyerName, _oFontStyle5Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            oPdfPStyleTable.CompleteRow();
            #endregion

            #region Item

            _oPdfPCell = new PdfPCell(new Phrase("Item", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.GarmentsName, _oFontStyle5Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            oPdfPStyleTable.CompleteRow();
            #endregion

            #region Style

            _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.StyleNo, _oFontStyle5Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);


            #region Image

            if (oNewStyleBudget.TechnicalSheetImage.LargeImage != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(oNewStyleBudget.TechnicalSheetImage.LargeImage);
                _oImag.Border = 1; _oImag.ScaleAbsolute(55f, 50f);
                _oPdfPCell = new PdfPCell(_oImag); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.Padding = 1f; _oPdfPCell.FixedHeight = 52f;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = 3; oPdfPStyleTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("No Image ", _oFontStyle5Bold));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);
            }

            #endregion
            oPdfPStyleTable.CompleteRow();
            #endregion

            #region SeasonName

            _oPdfPCell = new PdfPCell(new Phrase("Season", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.StyleBudgetRecaps.Count > 0 ? oNewStyleBudget.StyleBudgetRecaps[0].SessionName : "", _oFontStyle5Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);
            oPdfPStyleTable.CompleteRow();
            #endregion

            #region Confirmation

            _oPdfPCell = new PdfPCell(new Phrase("O.Confirmation", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle6Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.StyleBudgetRecaps.Count > 0 ? oNewStyleBudget.StyleBudgetRecaps[0].OrderConfimationDateSt : "", _oFontStyle5Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPStyleTable.AddCell(_oPdfPCell);
            oPdfPStyleTable.CompleteRow();
            #endregion
            #region Insert in Return table
            _oPdfPCell = new PdfPCell(oPdfPStyleTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPReturnTable.AddCell(_oPdfPCell);
            oPdfPReturnTable.CompleteRow();
            #endregion

            #region Blank Space

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle6Normal)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPReturnTable.AddCell(_oPdfPCell);
            oPdfPReturnTable.CompleteRow();
            #endregion


            PdfPTable oPdfPYarnOrAccTable = new PdfPTable(11);
            oPdfPYarnOrAccTable.SetWidths(new float[] { 30f,//caption
                                                        12f, //SL
                                                        50f, //Description
                                                        23f, //Unit
                                                        23f, //Gar.qt
                                                        20f, //Cons/Dz
                                                        25f, //total Req Qty
                                                        150f ,//Dynamic part
                                                        25f,//Unit price
                                                        28f,//total Budget
                                                        22f //Supplier Name
                                                    });//408
            oPdfPYarnOrAccTable.WidthPercentage = 100;
            int nTempCol = 11; double nFixedpercent = 6;//may be changed latter
            double nTotalYarnCost = 0;
            #region Yarn Details
            #region Initial Cost BreakDown
            _oFontStyle6Bold = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Initial Cost BreakDown:", _oFontStyle6Bold));
            _oPdfPCell.Colspan = nTempCol; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
            oPdfPYarnOrAccTable.CompleteRow();
            #endregion

            #region Yarn  Heading

            _oFontStyle = FontFactory.GetFont("Tahoma", nDynamicFontSize, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Description Of Items", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Gmt. Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Cons/Dzn", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total req Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            #region Dynamic Table Configure
            PdfPTable oTempPdfPTable = YarnInfoTable();


            _oPdfPCell = new PdfPCell(new Phrase("Yarn\n(" + oNewStyleBudget.CurrencySymbol + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Knitting\n(" + oNewStyleBudget.CurrencySymbol + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Dye\n(" + oNewStyleBudget.CurrencySymbol + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempPdfPTable.AddCell(_oPdfPCell);

            if (bHasLycraCost == true)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Lycra\n(" + oNewStyleBudget.CurrencySymbol + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempPdfPTable.AddCell(_oPdfPCell);
            }

            if (bHasAOPCost == true)
            {
                _oPdfPCell = new PdfPCell(new Phrase("AOP\n(" + oNewStyleBudget.CurrencySymbol + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempPdfPTable.AddCell(_oPdfPCell);
            }
            if (bHasWashCost == true)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Wash\n(" + oNewStyleBudget.CurrencySymbol + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempPdfPTable.AddCell(_oPdfPCell);
            }
            if (bHasYarnDyeingCost == true)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Y/D\n(" + oNewStyleBudget.CurrencySymbol + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempPdfPTable.AddCell(_oPdfPCell);
            }
            if (bHasSuedeCost == true)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Sued\n(" + oNewStyleBudget.CurrencySymbol + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempPdfPTable.AddCell(_oPdfPCell);
            }
            if (bHasFinishingCost == true)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Finish\n(" + oNewStyleBudget.CurrencySymbol + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempPdfPTable.AddCell(_oPdfPCell);
            }
            if (bHasBrushingCost == true)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Brush\n(" + oNewStyleBudget.CurrencySymbol + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTempPdfPTable.AddCell(_oPdfPCell);
            }
            oTempPdfPTable.CompleteRow();
            //insert into parent table
            _oPdfPCell = new PdfPCell(oTempPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            #endregion
            _oPdfPCell = new PdfPCell(new Phrase("Unit Price\n(" + oNewStyleBudget.CurrencySymbol + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Budget\n(" + oNewStyleBudget.CurrencySymbol + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Supplier", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            oPdfPYarnOrAccTable.CompleteRow();
            #endregion

            #region Yarn value Print
            int nCount; double nTotalFabricCostPerPc = 0, nFabricConsumptionPerDz = 0, nFabricConsumptionPerPc = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", nDynamicFontSize, iTextSharp.text.Font.NORMAL);

            nCount = 0;
            _oPdfPCell = new PdfPCell(new Phrase("Fabrics", _oFontStyle5Bold));
            _oPdfPCell.Rowspan = _oStyleBudgetYarnDetails.Count; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            foreach (StyleBudgetDetail oItem in _oStyleBudgetYarnDetails)
            {

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle4Normal));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle4Normal));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(_oStyleBudget.ApproxQty.ToString("#,##0"), _oFontStyle4Normal));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Consumption.ToString("#,##0.##"), _oFontStyle4Normal));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((_oStyleBudget.ApproxQty * oItem.ConsumptionPerPc).ToString("#,##0.##"), _oFontStyle4Normal));//requar qty
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

                #region Dynamic Table Configure
                oTempPdfPTable = YarnInfoTable();//table Initialize

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MaterialMarketPrice.ToString("#,##0.000"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.KnittingCost.ToString("#,##0.000"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DyeingCost.ToString("#,##0.000"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                if (bHasLycraCost == true)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LycraCost.ToString("#,##0.000"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                }
                if (bHasAOPCost == true)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AOPCost.ToString("#,##0.000"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                }
                if (bHasWashCost == true)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.WashCost.ToString("#,##0.000"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                }
                if (bHasYarnDyeingCost == true)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.YarnDyeingCost.ToString("#,##0.000"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                }
                if (bHasSuedeCost == true)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.SuedeCost.ToString("#,##0.000"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                }
                if (bHasFinishingCost == true)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.FinishingCost.ToString("#,##0.000"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                }
                if (bHasBrushingCost == true)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BrushingCost.ToString("#,##0.000"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                }
                oTempPdfPTable.CompleteRow();

                //insert into parent table
                _oPdfPCell = new PdfPCell(oTempPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
                #endregion



                _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricCost.ToString("#,##0.00000"), _oFontStyle4Normal));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((_oStyleBudget.ApproxQty * oItem.ConsumptionPerPc * oItem.FabricCost).ToString("#,##0.##"), _oFontStyle4Normal));//requar qty
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.Description, _oFontStyle4Normal));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

                nTotalFabricCostPerPc = nTotalFabricCostPerPc + oItem.FabricCost;
                nFabricConsumptionPerDz = nFabricConsumptionPerDz + oItem.Consumption;
                nFabricConsumptionPerPc = nFabricConsumptionPerPc + oItem.ConsumptionPerPc;
                oPdfPYarnOrAccTable.CompleteRow();
            }
            #endregion
            #region Total Print

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("(a).Sub-total Fabric Cost", _oFontStyle6Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            nTotalYarnCost  = _oStyleBudgetYarnDetails.Sum(x => x.ConsumptionPerPc * x.FabricCost * _oStyleBudget.ApproxQty);
            _oPdfPCell = new PdfPCell(new Phrase(nTotalYarnCost.ToString("#,##0.##"), _oFontStyle4Bold));//requar qty
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
            oPdfPYarnOrAccTable.CompleteRow();

            #endregion
            #region Total % FOB
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Inc", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nFixedpercent.ToString("#,##0.###") + "%", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("% of FOB", _oFontStyle4Normal));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            if (nTotalYarnCost <= 0)
            {
                nTotalYarnCost = 1.00;
            }
            double nTempFobPercent = (nTotalYarnCost / _nTotalValueWithOutBankCost) * 100;
            _nTotalPercent += nTempFobPercent;
            _oPdfPCell = new PdfPCell(new Phrase(nTempFobPercent.ToString("#,##0.##") + "%", _oFontStyle));//requar qty
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
            oPdfPYarnOrAccTable.CompleteRow();

            #endregion
            #endregion

           #region Accesoreis
           #region Accessores Value

            double nTotalAccessoriesCost = 0;
            double nRequredQty = oNewStyleBudget.ApproxQty + (oNewStyleBudget.ApproxQty * nFixedpercent / 100);
           if (_oStyleBudgetAccessoriesDetails.Count > 0)
           {
               nCount = 0;
               _oPdfPCell = new PdfPCell(new Phrase("Accesories & Trim", _oFontStyle5Bold));
               _oPdfPCell.Rowspan = _oStyleBudgetAccessoriesDetails.Count; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
               
               foreach (StyleBudgetDetail oItem in _oStyleBudgetAccessoriesDetails)
               {
                   nCount++;
                   _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle4Normal));
                   _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

                   //oAccessoriesPTable = AccessoriesInfoTable();//table Initializing
                   _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle4Normal));
                   _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);


                   _oPdfPCell = new PdfPCell(new Phrase(oItem.RateUnit.ToString() + " " + oItem.UnitSymbol, _oFontStyle4Normal));
                   _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

                   
                   
                   _oPdfPCell = new PdfPCell(new Phrase(nRequredQty.ToString("#,##0"), _oFontStyle4Normal));
                   _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

                   _oPdfPCell = new PdfPCell(new Phrase(oItem.Consumption.ToString("#,##0.00"), _oFontStyle4Normal));
                   _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

                   if (oItem.Consumption <= 0)
                   {
                       oItem.Consumption = 1;
                   }
                   double nTotalRequredQty = (nRequredQty / 12) * oItem.Consumption;
                   _oPdfPCell = new PdfPCell(new Phrase((nTotalRequredQty).ToString("#,##0.00"), _oFontStyle4Normal));
                   _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);



                   #region Dynamic Table Configure
                   oTempPdfPTable = YarnInfoTable();//table Initialize

                   _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                   _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                   _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                   _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                   _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                   _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                   if (bHasLycraCost == true)
                   {
                       _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                       _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                   }
                   if (bHasAOPCost == true)
                   {
                       _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                       _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                   }
                   if (bHasWashCost == true)
                   {
                       _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                       _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                   }
                   if (bHasYarnDyeingCost == true)
                   {
                       _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                       _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                   }
                   if (bHasSuedeCost == true)
                   {
                       _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                       _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                   }
                   if (bHasFinishingCost == true)
                   {
                       _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                       _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                   }
                   if (bHasBrushingCost == true)
                   {
                       _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                       _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                   }
                   oTempPdfPTable.CompleteRow();

                   //insert into parent table
                   _oPdfPCell = new PdfPCell(oTempPdfPTable);
                   _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
                   #endregion


                   _oPdfPCell = new PdfPCell(new Phrase(oItem.MaterialMarketPrice.ToString("#,##0.00000"), _oFontStyle4Normal));
                   _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

                   nTotalAccessoriesCost = nTotalAccessoriesCost + (nTotalRequredQty * (oItem.MaterialMarketPrice / oItem.RateUnit));
                   _oPdfPCell = new PdfPCell(new Phrase((nTotalRequredQty * (oItem.MaterialMarketPrice / oItem.RateUnit)).ToString("#,##0.00"), _oFontStyle4Normal));
                   _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

                   _oPdfPCell = new PdfPCell(new Phrase(oItem.Description, _oFontStyle4Normal));
                   _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

                   oPdfPReturnTable.CompleteRow();

               }
           }
           #endregion

           #region Total Print

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("(b).Sub-total Accesories & Trims Expense", _oFontStyle6Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           //nTotalAccessoriesCost = _oStyleBudgetAccessoriesDetails.Sum(x => (x.MaterialMarketPrice/x.RateUnit) *nRequredQty);
           _oPdfPCell = new PdfPCell(new Phrase(nTotalAccessoriesCost.ToString("#,##0.##"), _oFontStyle4Bold));//requar qty
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
           oPdfPYarnOrAccTable.CompleteRow();

           #endregion
           #region Total % FOB

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("% of FOB", _oFontStyle4Normal));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           if (nTotalAccessoriesCost <= 0)
           {
               nTotalAccessoriesCost = 1.00;
           }
           nTempFobPercent = (nTotalAccessoriesCost / _nTotalValueWithOutBankCost) * 100;
           _nTotalPercent += nTempFobPercent;
           _oPdfPCell = new PdfPCell(new Phrase(nTempFobPercent.ToString("#,##0.##") + "%", _oFontStyle));//requar qty
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
           oPdfPYarnOrAccTable.CompleteRow();

           #endregion
           #endregion

           #region Print/Emb/Test Cost
           double nTotalOthesCost = 0;
           nCount = 0;
           _oPdfPCell = new PdfPCell(new Phrase("Test, Inspection, Embellishment & Freight", _oFontStyle5Bold));
           _oPdfPCell.Rowspan = 5; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           #region Test Charge Heading

           nCount++; _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("Testing Charge", _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("DZ", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase((oNewStyleBudget.ApproxQty / 12).ToString("#,##0") + " dz", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           #region Dynamic Table Configure
           oTempPdfPTable = YarnInfoTable();//table Initialize

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           if (bHasLycraCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasAOPCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasWashCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

           }
           if (bHasYarnDyeingCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasSuedeCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasFinishingCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasBrushingCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           oTempPdfPTable.CompleteRow();

           //insert into parent table
           _oPdfPCell = new PdfPCell(oTempPdfPTable);
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
           #endregion

           _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.TestPricePerDozen.ToString("#,##0.00000"), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           nTotalOthesCost += (oNewStyleBudget.TestPricePerDozen * (oNewStyleBudget.ApproxQty / 12));
           _oPdfPCell = new PdfPCell(new Phrase((oNewStyleBudget.TestPricePerDozen * (oNewStyleBudget.ApproxQty / 12)).ToString("#,##0.00"), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           oPdfPReturnTable.CompleteRow();

           #endregion

           #region Print Charge Heading

           nCount++; _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("Printing Charge", _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("DZ", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase((oNewStyleBudget.ApproxQty / 12).ToString("#,##0") + " dz", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           #region Dynamic Table Configure
           oTempPdfPTable = YarnInfoTable();//table Initialize

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           if (bHasLycraCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasAOPCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasWashCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

           }
           if (bHasYarnDyeingCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasSuedeCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasFinishingCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasBrushingCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           oTempPdfPTable.CompleteRow();

           //insert into parent table
           _oPdfPCell = new PdfPCell(oTempPdfPTable);
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
           #endregion

           _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.PrintPricePerDozen.ToString("#,##0.00000"), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           nTotalOthesCost += (oNewStyleBudget.PrintPricePerDozen * (oNewStyleBudget.ApproxQty / 12));
           _oPdfPCell = new PdfPCell(new Phrase((oNewStyleBudget.PrintPricePerDozen * (oNewStyleBudget.ApproxQty / 12)).ToString("#,##0.00"), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           oPdfPReturnTable.CompleteRow();

           #endregion

           #region Embroidery Charge Heading

           nCount++; _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("Embroidery Charge", _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("DZ", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase((oNewStyleBudget.ApproxQty / 12).ToString("#,##0") + " dz", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           #region Dynamic Table Configure
           oTempPdfPTable = YarnInfoTable();//table Initialize

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           if (bHasLycraCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasAOPCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasWashCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

           }
           if (bHasYarnDyeingCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasSuedeCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasFinishingCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasBrushingCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           oTempPdfPTable.CompleteRow();

           //insert into parent table
           _oPdfPCell = new PdfPCell(oTempPdfPTable);
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
           #endregion

           _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.EmbrodaryPricePerDozen.ToString("#,##0.00000"), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           nTotalOthesCost += (oNewStyleBudget.EmbrodaryPricePerDozen * (oNewStyleBudget.ApproxQty / 12));
           _oPdfPCell = new PdfPCell(new Phrase((oNewStyleBudget.PrintPricePerDozen * (oNewStyleBudget.ApproxQty / 12)).ToString("#,##0.00"), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           oPdfPReturnTable.CompleteRow();

           #endregion

           #region Courier Charge Heading

           nCount++; _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.CourierCaption == "" ? "DHL Charge" : oNewStyleBudget.CourierCaption + " Charge", _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("DZ", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase((oNewStyleBudget.ApproxQty / 12).ToString("#,##0") + " dz", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           #region Dynamic Table Configure
           oTempPdfPTable = YarnInfoTable();//table Initialize

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           if (bHasLycraCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasAOPCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasWashCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

           }
           if (bHasYarnDyeingCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasSuedeCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasFinishingCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasBrushingCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           oTempPdfPTable.CompleteRow();

           //insert into parent table
           _oPdfPCell = new PdfPCell(oTempPdfPTable);
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
           #endregion

           _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.CourierPricePerDozen.ToString("#,##0.00000"), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           nTotalOthesCost += (oNewStyleBudget.CourierPricePerDozen * (oNewStyleBudget.ApproxQty / 12));
           _oPdfPCell = new PdfPCell(new Phrase((oNewStyleBudget.PrintPricePerDozen * (oNewStyleBudget.ApproxQty / 12)).ToString("#,##0.00"), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           oPdfPReturnTable.CompleteRow();

           #endregion

           #region Others Charge Heading

           nCount++; _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.OthersCaption == "" ? "Others Charge" : oNewStyleBudget.OthersCaption + " Charge", _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("DZ", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase((oNewStyleBudget.ApproxQty / 12).ToString("#,##0") + " dz", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           #region Dynamic Table Configure
           oTempPdfPTable = YarnInfoTable();//table Initialize

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           if (bHasLycraCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasAOPCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasWashCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

           }
           if (bHasYarnDyeingCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasSuedeCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasFinishingCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           if (bHasBrushingCost == true)
           {
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
           }
           oTempPdfPTable.CompleteRow();

           //insert into parent table
           _oPdfPCell = new PdfPCell(oTempPdfPTable);
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
           #endregion

           _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.CourierPricePerDozen.ToString("#,##0.00000"), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           nTotalOthesCost += (oNewStyleBudget.OthersPricePerDozen * (oNewStyleBudget.ApproxQty / 12));
           _oPdfPCell = new PdfPCell(new Phrase((oNewStyleBudget.OthersPricePerDozen * (oNewStyleBudget.ApproxQty / 12)).ToString("#,##0.00"), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           oPdfPReturnTable.CompleteRow();

           #endregion


           #region Total Print

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("(c).Sub-total Testing & Inspection & Emblishment Expense", _oFontStyle6Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           
           _oPdfPCell = new PdfPCell(new Phrase(nTotalOthesCost.ToString("#,##0.##"), _oFontStyle4Bold));//requar qty
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
           oPdfPYarnOrAccTable.CompleteRow();

           #endregion
           #region Total % FOB
             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("% of FOB", _oFontStyle4Normal));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           if (nTotalOthesCost <= 0)
           {
               nTotalOthesCost = 1.00;
           }
           nTempFobPercent = (nTotalOthesCost / _nTotalValueWithOutBankCost) * 100;
           _nTotalPercent += nTempFobPercent;
           _oPdfPCell = new PdfPCell(new Phrase(nTempFobPercent.ToString("#,##0.##") + "%", _oFontStyle));//requar qty
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
           oPdfPYarnOrAccTable.CompleteRow();

           #endregion
            #endregion

           #region Grand Total

           #region Total Print

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
          _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("Total Expenses(a+b+c)", _oFontStyle6Bold));
           _oPdfPCell.BorderWidthBottom = 0;  _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase((nTotalYarnCost+nTotalAccessoriesCost+nTotalOthesCost).ToString("#,##0.##"), _oFontStyle4Bold));//requar qty
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
           oPdfPYarnOrAccTable.CompleteRow();

           #endregion
           #region Total % FOB
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("% of FOB", _oFontStyle4Normal));
           _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase(_nTotalPercent.ToString("#,##0.##") + "%", _oFontStyle));//requar qty
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPYarnOrAccTable.AddCell(_oPdfPCell);
           oPdfPYarnOrAccTable.CompleteRow();

           #endregion
           #endregion

           #region Insert in Return table
           _oPdfPCell = new PdfPCell(oPdfPYarnOrAccTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
           _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPReturnTable.AddCell(_oPdfPCell);
           oPdfPReturnTable.CompleteRow();
           #endregion
           #region Blank Space
           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle6Normal)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
           _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPReturnTable.AddCell(_oPdfPCell);
           oPdfPReturnTable.CompleteRow();
           #endregion


           #region Designation & CM Val calculation
           PdfPTable PdfPDesignationCMTable = new PdfPTable(6);
           PdfPDesignationCMTable.SetWidths(new float[] { 
                                                        30f,//Blank
                                                        110f, //signation
                                                        50f, //Blank
                                                        140f, //CM Header
                                                        56f, //Vlaue
                                                        22f //Blank
                                                    });//408
           PdfPDesignationCMTable.WidthPercentage = 100;
           #region Projected cm
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase("Projected C.M. Sign By Md. dated On.........", _oFontStyle5Bold));
           _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           PdfPDesignationCMTable.CompleteRow();
           #endregion

           #region Prepared By
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("Prepared By: " + oNewStyleBudget.PreparedByName, _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);
           PdfPDesignationCMTable.CompleteRow();
           #endregion

           #region Master L/C Value

           List<OrderRecapDetail> oDistinctRecapDetails = new List<OrderRecapDetail>();
           oDistinctRecapDetails = _oOrderRecapDetails.GroupBy(x => x.ColorID).Select(group => new OrderRecapDetail
           {
               ColorID = group.First().ColorID,
               ColorName = group.First().ColorName,
               Quantity = group.Sum(x => x.Quantity),
               UnitPrice = group.Average(x => x.UnitPrice)
               //RecapNos = _oStyleBudget.StyleBudgetRecaps.Where(x=>x.OrderRecapID.ToString().Contains(group.))

           }).ToList();
           double ntotalVlue = oDistinctRecapDetails.Sum(x => x.Quantity * x.UnitPrice) - ((oDistinctRecapDetails.Sum(x => x.Quantity * x.UnitPrice)) * oNewStyleBudget.BankingCost / 100);
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("Master L/C Value", _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.CurrencySymbol + " " + ntotalVlue.ToString("#,##0.00"), _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);
           PdfPDesignationCMTable.CompleteRow();
           #endregion

           #region Position &  Rawmateril L/C Value
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("Position : ", _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("Total Raw Material Value", _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.CurrencySymbol + " " + (nTotalYarnCost + nTotalAccessoriesCost + nTotalOthesCost).ToString("#,##0.00"), _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);
           PdfPDesignationCMTable.CompleteRow();
           #endregion

           #region CM Value
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("CM Value", _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.CurrencySymbol + " " + (oNewStyleBudget.CMCostPerPcs * oNewStyleBudget.ApproxQty).ToString("#,##0.00"), _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);
           PdfPDesignationCMTable.CompleteRow();
           #endregion

           #region Checked &  C.M. Per Doz
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("Checked By : " + oNewStyleBudget.ApprovedByName, _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("C.M. Per Doz", _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.CurrencySymbol + " " + oNewStyleBudget.CMCost.ToString("#,##0.00"), _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);
           PdfPDesignationCMTable.CompleteRow();
           #endregion

           #region CM Per Unit
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("CM Per Unit", _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(oNewStyleBudget.CurrencySymbol + " " + oNewStyleBudget.CMCostPerPcs.ToString("#,##0.00"), _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);
           PdfPDesignationCMTable.CompleteRow();
           #endregion

           #region Position &  C.M Ratio
           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("Position : ", _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("C.M. Ratio(%)", _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           nTempFobPercent = oNewStyleBudget.FOBPricePerPcs > 0 ? (100 / (oNewStyleBudget.ApproxQty * oNewStyleBudget.FOBPricePerPcs)) * (oNewStyleBudget.CMCostPerPcs * oNewStyleBudget.ApproxQty) : 0;
           _oPdfPCell = new PdfPCell(new Phrase(nTempFobPercent.ToString("#,##0.00")+"%", _oFontStyle6Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; PdfPDesignationCMTable.AddCell(_oPdfPCell);
           PdfPDesignationCMTable.CompleteRow();
           #endregion
           #endregion

           #region Insert in Return table
           _oPdfPCell = new PdfPCell(PdfPDesignationCMTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPReturnTable.AddCell(_oPdfPCell);
                oPdfPReturnTable.CompleteRow();
                #endregion

                return oPdfPReturnTable;
            }
        public PdfPTable GRecapDetails(bool bIsBudget)
        {
            PdfPTable oRecapDetails = new PdfPTable(7);
            oRecapDetails.SetWidths(new float[] { 55f, 30f, 35f, 40f, 40f, 65f,34f });//595
            oRecapDetails.WidthPercentage = 100;
            List<OrderRecapDetail> oDistinctRecapDetails = new List<OrderRecapDetail>();
            if (bIsBudget)
            {
                oDistinctRecapDetails = _oOrderRecapDetails.GroupBy(x => x.ColorID).Select(group => new OrderRecapDetail
                {
                    ColorID = group.First().ColorID,
                    ColorName = group.First().ColorName,
                    Quantity = group.Sum(x => x.Quantity),
                    UnitPrice = group.Average(x => x.UnitPrice)

                }).ToList();
            }
            else
            {
                oDistinctRecapDetails = _oOrderRecapDetailsPost.GroupBy(x => x.ColorID).Select(group => new OrderRecapDetail
                {
                    ColorID = group.First().ColorID,
                    ColorName = group.First().ColorName,
                    Quantity = group.Sum(x => x.Quantity),
                    UnitPrice = group.Average(x => x.UnitPrice)

                }).ToList();
            }

            #region Header
            
            _oPdfPCell = new PdfPCell(new Phrase("Body Color", _oFontStyle5Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oRecapDetails.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order Qty Pcs", _oFontStyle5Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oRecapDetails.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit Price", _oFontStyle5Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oRecapDetails.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Value($)", _oFontStyle5Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oRecapDetails.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Ship Date", _oFontStyle5Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oRecapDetails.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PO#", _oFontStyle5Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oRecapDetails.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order Qty.", _oFontStyle5Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oRecapDetails.AddCell(_oPdfPCell);

            oRecapDetails.CompleteRow();

            #endregion
            #region Value print
            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, iTextSharp.text.Font.NORMAL);
           foreach(OrderRecapDetail oItem in oDistinctRecapDetails)
           {
               nCount++;
               _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle4Normal));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


               _oPdfPCell = new PdfPCell(new Phrase(oItem.Quantity.ToString("#,##0"), _oFontStyle4Normal));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


               _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitPriceInString, _oFontStyle4Normal));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


               _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice * oItem.Quantity), _oFontStyle4Normal));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);

               //_oPdfPCell = new PdfPCell(new Phrase(string.Join(",", _oOrderRecapDetails.Where(x => x.ColorID == oItem.ColorID).ToList().Select(x => x.ShipmentDateInString)), _oFontStyle));
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Normal));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


               _oPdfPCell = new PdfPCell(new Phrase(bIsBudget ? string.Join(",", _oOrderRecapDetails.Where(x => x.ColorID == oItem.ColorID).ToList().Select(x => x.OrderRecapNo).Distinct()) : string.Join(",", _oOrderRecapDetailsPost.Where(x => x.ColorID == oItem.ColorID).ToList().Select(x => x.OrderRecapNo).Distinct()), _oFontStyle4Normal));               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);

               _oPdfPCell = new PdfPCell(new Phrase("0", _oFontStyle4Normal));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);
               oRecapDetails.CompleteRow();
           }

           for (int n = nCount; n < 7;n++ )
           {
               nCount++;
               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Normal));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Normal));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Normal));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Normal));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);

               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Normal));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Normal));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);

               _oPdfPCell = new PdfPCell(new Phrase("0", _oFontStyle4Normal));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);
               oRecapDetails.CompleteRow();
           }
            #endregion
           #region Total print
           _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD);
           _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase((oDistinctRecapDetails.Sum(x => x.Quantity)).ToString("#,##0"), _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDistinctRecapDetails.Sum(x => x.Quantity * x.UnitPrice)), _oFontStyle5Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("0", _oFontStyle5Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);
           oRecapDetails.CompleteRow();
           #endregion

           #region Commercial Cost
           
           _oPdfPCell = new PdfPCell(new Phrase("Commercial Cost", _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase(_oStyleBudget.BankingCost.ToString() + " %", _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat((oDistinctRecapDetails.Sum(x => x.Quantity * x.UnitPrice)) * _oStyleBudget.BankingCost / 100), _oFontStyle5Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle5Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);
           oRecapDetails.CompleteRow();
           #endregion

           #region Actual Total FOB

           _oPdfPCell = new PdfPCell(new Phrase("Actual Total FOB", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;  _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle5Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;  _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase(_oStyleBudget.CurrencySymbol + " " + _oStyleBudget.FOBPricePerPcs.ToString(), _oFontStyle5Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;  _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);

           double ntotalVlue = oDistinctRecapDetails.Sum(x => x.Quantity * x.UnitPrice) - ((oDistinctRecapDetails.Sum(x => x.Quantity * x.UnitPrice)) * _oStyleBudget.BankingCost / 100);

           _nTotalValueWithOutBankCost = ntotalVlue;
           _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(ntotalVlue), _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;  _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle4Normal));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;  _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);


           _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;  _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);

           _oPdfPCell = new PdfPCell(new Phrase("0", _oFontStyle4Bold));
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;  _oPdfPCell.BackgroundColor = BaseColor.WHITE; oRecapDetails.AddCell(_oPdfPCell);
           oRecapDetails.CompleteRow();
           #endregion
           return oRecapDetails;
        }

        #region Style Info Table Initialize
        public PdfPTable YarnInfoTable()
        {
            #region Make Dynamic Table
            int nColumnCount = 3; 
            if (_oStyleBudgetYarnDetails.Sum(x => x.LycraCost) > 0) { nColumnCount++; bHasLycraCost = true; }
            if (_oStyleBudgetYarnDetails.Sum(x => x.AOPCost) > 0) { nColumnCount++; bHasAOPCost = true; }
            if (_oStyleBudgetYarnDetails.Sum(x => x.WashCost) > 0) { nColumnCount++; bHasWashCost = true; }
            if (_oStyleBudgetYarnDetails.Sum(x => x.YarnDyeingCost) > 0) { nColumnCount++; bHasYarnDyeingCost = true; }
            if (_oStyleBudgetYarnDetails.Sum(x => x.SuedeCost) > 0) { nColumnCount++; bHasSuedeCost = true; }
            if (_oStyleBudgetYarnDetails.Sum(x => x.FinishingCost) > 0) { nColumnCount++; bHasFinishingCost = true; }
            if (_oStyleBudgetYarnDetails.Sum(x => x.BrushingCost) > 0) { nColumnCount++; bHasBrushingCost = true; }
            PdfPTable oTempPdfPTable = new PdfPTable(nColumnCount);
            oTempPdfPTable.WidthPercentage = 100;
            oTempPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTempPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            float[] nTableCoulumnWidth = new float[nColumnCount];

            nTableCoulumnWidth[0] = 15f;//yarn price
            nTableCoulumnWidth[1] = 15f;//kniting
            nTableCoulumnWidth[2] = 15f;//dyeing
            nTempCount = 2;
            for (int i = 0; i < nColumnCount - 3; i++)
            {
                nTempCount++;
                nTableCoulumnWidth[nTempCount] = 15f;
            }
            int nTempWidth = 150, nExtraWidth = 0, nColumnDif = nColumnCount - 3;//fixed 3 columns
            if (nColumnDif < 6)//priv 7
            {
                nExtraWidth = ((nTempWidth - (nColumnCount * 15)) / (3 + nColumnDif));
                for (int i = 0; i <=nTempCount; i++)
                {
                    nTableCoulumnWidth[i] = nTableCoulumnWidth[i] + nExtraWidth;
                }
                switch (nColumnDif)
                {

                    case 0: nDynamicFontSize = 5.5f; break;
                    case 1:
                    case 2: 
                    case 3:
                    case 4: 
                    case 5:
                    case 6: nDynamicFontSize = 5f; break;
                }
            }
            oTempPdfPTable.SetWidths(nTableCoulumnWidth);
            #endregion
            return oTempPdfPTable;
        }
        #endregion
        #endregion

    }
    
    
    

}
