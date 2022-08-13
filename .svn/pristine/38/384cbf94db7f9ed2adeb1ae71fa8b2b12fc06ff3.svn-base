using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
 
 
using System.Data;
using System.IO;
using System.Drawing;
using System;
namespace ESimSol.Reports
{



    public class rptProdcutionProgressReport
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        GUProductionOrder _oGUProductionOrder = new GUProductionOrder();
        DataTable _oColumnTable = new DataTable();
        DataTable _oTableValue = new DataTable();

        Company _oCompany = new Company();
        int _nColumn = 14;
        int _Count = 0;
        float[] _aColumnSize;



        #endregion

        public byte[] PrepareReport(GUProductionOrder oGUProductionOrder)
        {
            _oGUProductionOrder = oGUProductionOrder;
            _oColumnTable = oGUProductionOrder.ProductionSummeryColumnTable;
            _oTableValue = oGUProductionOrder.ProductionSummeryValueTable;
            _oCompany = oGUProductionOrder.Company;

            float nPageWidth = 790f;
            foreach (DataRow oColumn in _oColumnTable.Rows)
            {
                _Count++;
            }
            nPageWidth = nPageWidth + (_Count * 50*3);
            _nColumn = _nColumn + (_Count * 3);
            _aColumnSize = new float[_nColumn];
            _aColumnSize[0] = 25; //SL
            _aColumnSize[1] = 70; //Buyer
            _aColumnSize[2] = 70; //Factory
            _aColumnSize[3] = 60; // Order no
            _aColumnSize[4] = 80; //Style no
            _aColumnSize[5] = 30;// gg
            _aColumnSize[6] = 40; // aPProval weight
            _aColumnSize[7] = 40; // Qty 
            _aColumnSize[8] = 90; // Yarn Composition
            _aColumnSize[9] = 50;// DO Date
            _aColumnSize[10] = 50; // Yarn Receive
            _aColumnSize[11] = 50; // yarn balance
            _aColumnSize[12] = 50; // Shipment date
            _aColumnSize[13] = 30; // see /air
            
            for (int i = 14; i < _nColumn; i++)
            {
                _aColumnSize[i] = 50;
            }

            #region Page Setup

            _oDocument = new Document(new iTextSharp.text.Rectangle(nPageWidth, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(new iTextSharp.text.Rectangle(nPageWidth, 595f));
            _oDocument.SetMargins(20f, 20f, 10f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float [] { nPageWidth});
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 5;
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
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 7f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("WORK IN PROGRESS REPORT", _oFontStyle));

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

            PdfPTable oPdfPTable = new PdfPTable(_nColumn);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            oPdfPTable.SetWidths(_aColumnSize);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);

            #region Column Header

            #region 1st Row
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;  _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Factory", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("GG", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Approve Weight", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Recap Qty", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Yarn composition", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("D.O Date", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Yarn Receive", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Yarn Balance", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Shipment Date", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Air/Sea", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            foreach (DataRow oColumn in _oColumnTable.Rows)
            {
                _oPdfPCell = new PdfPCell(new Phrase((oColumn["StepName"]).ToString(), _oFontStyle));
                _oPdfPCell.Colspan = 3;   _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }

            oPdfPTable.CompleteRow();
            #endregion

        #region 2nd Row
            foreach (DataRow oColumn in _oColumnTable.Rows)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Start Date", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Execution Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Balance Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
        #endregion
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 1; _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();




            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            int nCount = 0;

            foreach (DataRow oRow in _oTableValue.Rows)
            {
                nCount++;

                oPdfPTable = new PdfPTable(_nColumn);
                oPdfPTable.SetWidths(_aColumnSize);
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((oRow["BuyerName"]).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((oRow["FactoryName"]).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((oRow["SaleOrderNo"]).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((oRow["StyleNo"]).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((oRow["GG"]).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Convert.ToString(oRow["ApproveWeight"]), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Convert.ToDouble(oRow["OrderQty"])), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((oRow["YarnComposition"]).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((Convert.ToDateTime(oRow["DyeingOrderIssueDate"]) != Convert.ToDateTime("01 Jan 1900")) ? (Convert.ToDateTime(oRow["DyeingOrderIssueDate"])).ToString("dd MMM yyyy") : " ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Convert.ToDouble(oRow["YarnReceiveQty"])), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Convert.ToDouble(oRow["YetToYarnReceiveQty"])), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((Convert.ToDateTime(oRow["ShipmentDate"]) != Convert.ToDateTime("01 Jan 1900")) ? (Convert.ToDateTime(oRow["ShipmentDate"])).ToString("dd MMM yyyy") : "", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(((EnumTransportType)Convert.ToInt32(oRow["TransPortType"])).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                foreach (DataRow oColumn in _oColumnTable.Rows)
                {


                    _oPdfPCell = new PdfPCell(new Phrase((Convert.ToDateTime(oRow["Raw" + Convert.ToString(oColumn["ColumnId"]) + "StartDate"]) != Convert.ToDateTime("01 Jan 1900")) ? Convert.ToDateTime(oRow["Raw" + Convert.ToString(oColumn["ColumnId"]) + "StartDate"]).ToString("dd MMM yyyy") : "", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Convert.ToDouble(oRow["Raw" + Convert.ToString(oColumn["ColumnId"]) + "ExecutionQty"])), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Convert.ToDouble(oRow["Raw" + Convert.ToString(oColumn["ColumnId"]) + "BalanceQty"])), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }


        }

        #endregion
    }
    
    
}
