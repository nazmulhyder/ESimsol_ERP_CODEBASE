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

    public class rptAdjustmentRequisitionSlip
    {
        #region Declaration
        int _nTotalColumn = 6;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle2;
        PdfPTable _oPdfPTable = new PdfPTable(6);
        PdfPTable _oPdfPTableDetail = new PdfPTable(9);
        PdfPCell _oPdfPCell;
        PdfPCell _oPdfPCellDetail;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        AdjustmentRequisitionSlip _oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
        AdjustmentRequisitionSlipDetail _oAdjustmentRequisitionSlipDetail = new AdjustmentRequisitionSlipDetail();
        List<AdjustmentRequisitionSlip> _oAdjustmentRequisitionSlips = new List<AdjustmentRequisitionSlip>();
        List<AdjustmentRequisitionSlipDetail> _oAdjustmentRequisitionSlipDetails = new List<AdjustmentRequisitionSlipDetail>();
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
        Company _oCompany = new Company();

        #endregion

        public byte[] PrepareReport(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip, List<AdjustmentRequisitionSlipDetail> oAdjustmentRequisitionSlipDetails, Company oCompany, List<SignatureSetup> oSignatureSetups)
        {
            _oAdjustmentRequisitionSlip = oAdjustmentRequisitionSlip;
            _oAdjustmentRequisitionSlipDetails = oAdjustmentRequisitionSlipDetails;
            _oCompany = oCompany;
            _oSignatureSetups = oSignatureSetups;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(25f, 25f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oPdfPTableDetail.WidthPercentage = 100;
            _oPdfPTableDetail.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);

            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            ESimSolFooter PageEventHandler = new ESimSolFooter();

            //PageEventHandler.signatures = signatureList;
            PageEventHandler.nFontSize = 9;
            PdfWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 99f, 99f, 99f, 99f, 99f, 99f });
            _oPdfPTableDetail.SetWidths(new float[] { 25f, 80f, 50f, 100f, 60f, 50f, 50f, 50f, 100f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;
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
                _oPdfPCell.Colspan = _nTotalColumn;
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
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Adjustment Requisition Slip", _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Adjustment Requisition Slip Detail", _oFontStyle));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTableDetail.AddCell(_oPdfPCell);
            _oPdfPTableDetail.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle2 = FontFactory.GetFont("Tahoma", 8f, 0);

            #region Main obj
            _oPdfPCell = new PdfPCell(new Phrase("Request No: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oAdjustmentRequisitionSlip.ARSlipNo, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Requisition Date: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oAdjustmentRequisitionSlip.DateSt, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Adjustment Type: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oAdjustmentRequisitionSlip.AdjustmentTypeSt, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("In /Out Type: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oAdjustmentRequisitionSlip.InOutTypeSt, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Status: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oAdjustmentRequisitionSlip.Status, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Voucher Effect: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oAdjustmentRequisitionSlip.IsWillVoucherEffectSt, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("Prepare Name: ", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPCell = new PdfPCell(new Phrase(_oAdjustmentRequisitionSlip.PreaperByName, _oFontStyle2));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Aproved By: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oAdjustmentRequisitionSlip.AprovedByName, _oFontStyle2)); //_oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Note: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oAdjustmentRequisitionSlip.Note, _oFontStyle2)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("Note: ", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPCell = new PdfPCell(new Phrase(_oAdjustmentRequisitionSlip.Note, _oFontStyle2));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 6; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(); _oPdfPCell.FixedHeight = 15f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 6; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            
            #region Another table
            _oPdfPCellDetail = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Store", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Code", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Product", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Lot No", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Current Balance", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Adjustment Qty", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPTableDetail.CompleteRow();


            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            if (_oAdjustmentRequisitionSlipDetails.Count > 0)
            {
                foreach (AdjustmentRequisitionSlipDetail oItem in _oAdjustmentRequisitionSlipDetails)
                {
                    nCount++;
                    _oPdfPCellDetail = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.StoreShortName, _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.ProductCode, _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.CurrentBalance.ToString("#,##0.00"), _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00"), _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellDetail.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPTableDetail.CompleteRow();
                }
            }
            #endregion
            #region push into main table
            _oPdfPCell = new PdfPCell(_oPdfPTableDetail);
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            float nTableHeight = CalculatePdfPTableHeight(_oPdfPTable);
            float _nfixedHight = 760 - (float)nTableHeight;
            if (_nfixedHight > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = _nfixedHight;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            #region Signature
            //_oPdfPCell = new PdfPCell(GetSignatureTable()); _oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            #endregion

            #region Signature
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 6;
            //_oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(525f, (object)_oAdjustmentRequisitionSlip, _oSignatureSetups, 0.0f)); _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion

        #region function
        public static float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        table.TotalWidth = 535f;
                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }

        private PdfPTable GetSignatureTable()
        {
            PdfPTable oSignatureTable = new PdfPTable(9);
            oSignatureTable.WidthPercentage = 100;
            oSignatureTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oSignatureTable.SetWidths(new float[] {             5f,                                                  
                                                                18f,
                                                                6f,
                                                                18f,
                                                                6f,
                                                                18f,
                                                                6f,
                                                                18f,
                                                                5f
                                                          });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 1; //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 1; //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 1; //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 1; //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            oSignatureTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Service Advisor", _oFontStyle)); _oPdfPCell.Border = 0;  //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Approved By", _oFontStyle)); _oPdfPCell.Border = 0;  //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Parts Advisor", _oFontStyle)); _oPdfPCell.Border = 0;  //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Received By", _oFontStyle)); _oPdfPCell.Border = 0;  //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; //_oPdfPCell.FixedHeight = 18;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oSignatureTable.AddCell(_oPdfPCell);

            oSignatureTable.CompleteRow();


            return oSignatureTable;
        }

        #endregion
    }
}
