using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
 
namespace ESimSol.Reports
{
    public class rptWYRequisitionBeam
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);

        PdfWriter _oWriter;
        MemoryStream _oMemoryStream = new MemoryStream();

        WYRequisition _oWYRequisition = new WYRequisition();
        List<FabricExecutionOrderYarnReceive> _oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
        Company _oCompany = new Company();
        #endregion

        #region Order Sheet List
        public byte[] PrepareReport(WYRequisition oWYRequisition, List<SelectedField> oSelectedFields, Company oCompany, BusinessUnit oBusinessUnit, string sDateRange)
        {
            _oWYRequisition = oWYRequisition;
            _oFabricExecutionOrderYarnReceives = oWYRequisition.FEOYSList;
       
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(30f, 30f, 10f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 842 });
            #endregion

            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oBusinessUnit, oCompany, "Beam Transfer Report", 0);

            PdfPTable oPdfPTable = new PdfPTable(3); oPdfPTable.SetWidths(new float[] { 18,70,13});

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Requisition No :  " + _oWYRequisition.RequisitionNo, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Date :    " + _oWYRequisition.IssueDateSt, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0);

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0,0,0,10);
           

            this.PrintBody(oSelectedFields);
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody(List<SelectedField> oSelectedFields)
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            int nColumns = oSelectedFields.Count;

            float[] tablecolumns = new float[nColumns];
            for (int i = 0; i < nColumns; i++)
            {
                tablecolumns[i] = Convert.ToInt32(oSelectedFields[i].Width);
            }
            
            PdfPTable oPdfPTable = new PdfPTable(nColumns);
            oPdfPTable.SetWidths(tablecolumns);

            ESimSolPdfHelper.FontStyle = _oFontStyle;
            ESimSolPdfHelper.AddCells(ref oPdfPTable, oSelectedFields.Select(x => x.Caption).ToArray(), Element.ALIGN_CENTER, 15); _oPdfPTable.CompleteRow();

            int nCount = 0, nTotalCount = 0, nFSCDID = -99;
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, 0);

            foreach (FabricExecutionOrderYarnReceive oItem in _oFabricExecutionOrderYarnReceives.OrderBy(x=>x.FSCDID))
            {
                //SelectedField oSelectedField = new SelectedField("", "#SL", 10, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("DispoNo", "Dispo No", 25, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("BuyerName", "Customer", 60, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("ProductName", "Count", 90, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);

                //oSelectedField = new SelectedField("ColorName", "Colour", 25, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("LotNo", "Batch", 30, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);

                //oSelectedField = new SelectedField("ReqCones", "Need Pcs", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("NumberOfCone", "T/F Pcs (c)", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);

                //oSelectedField = new SelectedField("ReceiveQty", "Issue Kg", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("UnitName", "Kg", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("Dia", "Dia", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("BagQty", "Bag", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
               
                //oSelectedField = new SelectedField("TFLength", "T/F Mtr", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("BeamNo", "Beam No", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("Remarks", "Remarks", 37, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);

                if (nFSCDID != oItem.FSCDID) 
                {
                    nCount++;
                    int nRowSpan = _oFabricExecutionOrderYarnReceives.Where(x => x.FSCDID == oItem.FSCDID).Count();
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, nCount.ToString(), Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.DispoNo, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.BuyerName, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                }
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.ProductName, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.ColorName, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.LotNo, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.ReqCones, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.NumberOfCone, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.ReceiveQty, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

                //ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.UnitName, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Dia, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.BagQty, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

                if (nFSCDID != oItem.FSCDID)
                {
                    int nRowSpan = _oFabricExecutionOrderYarnReceives.Where(x => x.FSCDID == oItem.FSCDID).Count();
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.TFLength, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.TFLengthLB, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.BeamNo, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                }
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Remarks, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

                nTotalCount++;
                nFSCDID = oItem.FSCDID;
            }
            oPdfPTable.CompleteRow();

            while (nTotalCount < 20) 
            {
                nCount++;
                nTotalCount++;

                ESimSolPdfHelper.AddCell(ref oPdfPTable, nCount.ToString(), Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15,0,0,20);

                oPdfPTable.CompleteRow();
            }

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

            ESimSolPdfHelper.Signature(ref _oPdfPTable, new string[] { "Prepared By",                       "Approved By",              "Issued By",             "Received By" },
                                                        new string[] { _oWYRequisition.RequisitionByName, _oWYRequisition.ApprovedByName, _oWYRequisition.ReceivedByName, "" }, 595, 20, 0);
        }
        #endregion

        #endregion 
    }

}
