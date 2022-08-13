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
     public class rptProductionScheduleReport
    {
        #region Declaration
        int _nColumns;
        int _nTempCol;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        iTextSharp.text.Image _oImag { get; set; }
        PdfPTable _oPdfPTable = new PdfPTable(9);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<ProductionSchedule> _oProductionSchedules = new List<ProductionSchedule>();
        ProductionSchedule _oProductionSchedule = new ProductionSchedule();
        Company _oCompany = new Company();
        double _nTotalPSQty = 0, _nGrandTotal = 0, _nTotalQtyInHouse = 0, _nTotalQtyOutside = 0, _nDoubeYarn = 0;
        string _sMessage = "";
        int _nPageWidth = 0;
        int _npageHeight = 595;


        int nSLNo = 0;
        DateTime date = DateTime.Now;

        string[] arrayDateName = new string[] { "Sun", "Mon", "Tue", "Wed", "Thr", "Fri", "Sat" };

        int nstartTimeInHour = 0;
        int nstartTimeInMinute = 0;
        int nendTimeInHour = 0;
        int nendTimeInMinute = 0;

        string sStartDate = "";
        string sEndDate = "";
        string sStartTimeInHour = "";
        string sStartTimeInMinute = "";
        string sEndTimeInHour = "";
        string sEndTimeInMinute = "";
       
        #endregion


        #region Constructor

        public rptProductionScheduleReport() { }

        #endregion


        public byte[] PrepareReport(ProductionSchedule oProductionSchedule, Company oCompany, double nTotalPSQty, double nGrandTotal, double nTotalQtyInHouse, double nTotalQtyOutside, double nDoubeYarn)
        {
            _oProductionSchedule = oProductionSchedule;
           // _oProductionSchedules = oProductionSchedule.;
            _oCompany = oCompany;
            _nTotalPSQty = nTotalPSQty;
            _nGrandTotal = nGrandTotal; 
            _nTotalQtyInHouse = nTotalQtyInHouse; 
            _nTotalQtyOutside = nTotalQtyOutside;
            _nDoubeYarn = nDoubeYarn;

            int len = oProductionSchedule.CapitalResources.Count;

            if (len > 8)
            {
                if ((len % 8) == 0)
                {
                    _nTempCol = len;
                }
                else
                {
                    _nTempCol = len + (8 - (len % 8));
                    _nTempCol = 8;
                }
                _nTempCol = _nColumns + 1;
            }
            else
            {
                _nTempCol = 9;
            }
            _nColumns = 9;
            _oPdfPTable = new PdfPTable(_nColumns);

            _nPageWidth = (100 * (_nColumns - 1)) + 40;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle( _nPageWidth,_npageHeight), 0f, 0f, 0f, 0f);
            //_oDocument.SetPageSize(PageSize.A4.Rotate());
            _oDocument.SetMargins(10f,10f, 10f, 20f);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.WidthPercentage = 100;

            float[] tablecolumns = new float[_nColumns];
            tablecolumns[0] = 20f;
            for (int i = 1; i < _nColumns; i++)
            {
                    tablecolumns[i] = 100f;
            }

            _oPdfPTable.SetWidths(tablecolumns); //height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            #endregion


            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }


        #region Report Header
        private void PrintHeader()
        {
            int nflag = 0;
            #region CompanyHeader

            if (_oCompany.CompanyLogo != null)
            {
                nflag = 1;
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(55f, 25f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.FixedHeight = 25f;
                _oPdfPCell.Border = 0;
                _oPdfPCell.PaddingRight = 20f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            if (nflag == 1)
            {
                _oPdfPCell.Colspan = 5;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            }
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f,0);
            //_oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            //_oPdfPCell.Colspan = _nColumns;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader

            //_oPdfPTable.CompleteRow();
            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.UNDERLINE);
            //if (_oProductionSchedule.sDay == "Day")
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("Daily Production Schedule", _oFontStyle));
            //}
            //else if (_oProductionSchedule.sWeek == "Week")
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("Weekly Production Schedule", _oFontStyle));
            //}
            //else if (_oProductionSchedule.sMonth == "Month")
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("Monthly Production Schedule", _oFontStyle));
            //}
           
            //_oPdfPCell.Colspan = _nColumns;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 5f;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion

            #region DateRange

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Schedule of: "+_oProductionSchedule.ProductionScheduleOf, _oFontStyle));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);


            string sQuantity = "InHouse Qty: " + Global.MillionFormat(_nTotalQtyInHouse) + "kg   Outside Qty: " + Global.MillionFormat(_nTotalQtyOutside) +
                               "kg   D.Y. Qty: " + _nDoubeYarn + "kg   Total Qty: " + Global.MillionFormat(_nTotalPSQty) +"kg   G.T. Qty: " + Global.MillionFormat(_nGrandTotal) +"kg";

            _oPdfPCell = new PdfPCell(new Phrase(sQuantity + "     " + DateTime.Now.ToString("dd MMM yyy HH:mm"), _oFontStyle));
            _oPdfPCell.Colspan = _nColumns-3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
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
            _oFontStyleBold = _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER;

            
            if (_oProductionSchedule.CapitalResources.Count<=8)
            {

                TableHeader(_oProductionSchedule.CapitalResources);
                TableContent(_oProductionSchedule.MaxValue, _oProductionSchedule.CapitalResources);
            }
            else
            {
                List<CapitalResource> oDyemachines = new List<CapitalResource>();
                List<ProductionSchedule> oPSS = new List<ProductionSchedule>();
                oPSS = _oProductionSchedule.ProductionScheduleList;
                int nCount = 0;
                int nMachineCount = 0;
                foreach (CapitalResource oDyemachine in _oProductionSchedule.CapitalResources)
                {
                    ++nCount;
                    ++nMachineCount;

                    oDyemachines.Add(oDyemachine);
                    if (oDyemachines.Count == 8)
                    {
                        TableHeader(oDyemachines);
                        TableContent(RetrunMaxValue(oDyemachines, oPSS), oDyemachines);
                        nCount = 0;
                        oDyemachines = new List<CapitalResource>();
                    }
                    else if (nMachineCount == _oProductionSchedule.CapitalResources.Count)
                    {
                        TableHeader(oDyemachines);
                        TableContent(RetrunMaxValue(oDyemachines, oPSS), oDyemachines);
                        //nCount = 0;
                        //oDyemachines = new List<CapitalResource>();
                    }

                   
                }
              
            }
            


      


        }

        private void TableHeader(List<CapitalResource> oDyemachines)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            if (oDyemachines.Count == 8)
            {
                foreach (CapitalResource oDyemachine in oDyemachines)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("["+oDyemachine.Code+"]"+oDyemachine.Name+"("+oDyemachine.MachineCapacity+")", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                _oPdfPTable.CompleteRow();
            }

            else if (oDyemachines.Count < 8)
            {
                foreach (CapitalResource oDyemachine in oDyemachines)
                {


                    _oPdfPCell = new PdfPCell(new Phrase("[" + oDyemachine.Code + "]" + oDyemachine.Name + "(" + oDyemachine.MachineCapacity + ")", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                }
                for (int i = (_oProductionSchedule.CapitalResources.Count + 1); i < 9; i++)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                _oPdfPTable.CompleteRow();
            }
        }

        private void TableContent(int nMaxValue, List<CapitalResource> oDyemachines)
        {
            int nj = 0;
            int flag = 0;
            int x = 0; ;

            if (nMaxValue == 0)
            {
        
                for (int i = 1; i <= 5; i++)
                {

                    _oPdfPCell = new PdfPCell(new Phrase((++nSLNo).ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 85f; _oPdfPTable.AddCell(_oPdfPCell);
                    for (int j = 1; j <= 8; j++)
                    {

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 85f; _oPdfPTable.AddCell(_oPdfPCell);

                    }

                    _oPdfPTable.CompleteRow();
                }

            }

            else
            {
                int len = nMaxValue;
                int nRows = len;
                // int nRows = 0;

                if (len > 5)
                {
                    if ((len % 5) == 0)
                    {
                        nRows = len;
                    }
                    else
                    {
                        nRows = len + (5 - (len % 5));
                    }

                }
                else
                {
                    nRows = 5;
                }


                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);

                if (nMaxValue > 0)
                {

                    for (x = 1; x <= nRows; x++)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase((++nSLNo).ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.MinimumHeight = 85f; _oPdfPTable.AddCell(_oPdfPCell);


                        foreach (CapitalResource oItems in oDyemachines)
                        {
                            flag = 0;

                            for (nj = 0; nj < _oProductionSchedule.ProductionScheduleList.Count; nj++)
                            {

                                if (_oProductionSchedule.ProductionScheduleList[nj].MachineID == oItems.CRID)
                                {

                                    sStartDate = _oProductionSchedule.ProductionScheduleList[nj].StartTime.ToString("dd MMM yyyy");
                                    sEndDate = _oProductionSchedule.ProductionScheduleList[nj].EndTime.ToString("dd MMM yyyy");
                                    nstartTimeInHour = _oProductionSchedule.ProductionScheduleList[nj].StartTime.Hour;
                                    nstartTimeInMinute = _oProductionSchedule.ProductionScheduleList[nj].StartTime.Minute;
                                    nendTimeInHour = _oProductionSchedule.ProductionScheduleList[nj].EndTime.Hour;
                                    nendTimeInMinute = _oProductionSchedule.ProductionScheduleList[nj].EndTime.Minute;

                                    sStartTimeInHour = nstartTimeInHour.ToString();
                                    sStartTimeInMinute = nstartTimeInMinute.ToString();
                                    sEndTimeInHour = nendTimeInHour.ToString();
                                    sEndTimeInMinute = nendTimeInMinute.ToString();

                                    if (nstartTimeInHour < 10)
                                    {
                                        sStartTimeInHour = "";
                                        sStartTimeInHour = "0" + nstartTimeInHour;
                                    }
                                    if (nstartTimeInMinute < 10)
                                    {
                                        sStartTimeInMinute = "";
                                        sStartTimeInMinute = "0" + nstartTimeInMinute;
                                    }
                                    if (nendTimeInHour < 10)
                                    {
                                        sEndTimeInHour = "";
                                        sEndTimeInHour = "0" + nendTimeInHour;
                                    }
                                    if (nendTimeInMinute < 10)
                                    {
                                        sEndTimeInMinute = "";
                                        sEndTimeInMinute = "0" + nendTimeInMinute;
                                    }

                                  

                                    PdfPTable oTempTable = new PdfPTable(1);
                                    oTempTable.SetWidths(new float[] { 98f });
                                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

                                    List<ProductionScheduleDetail> oTempPSDs = new List<ProductionScheduleDetail>();
                                    oTempPSDs = _oProductionSchedule.ProductionScheduleList[nj].ProductionScheduleDetails;

                                    string sFactory = string.Join(", ", (from p in oTempPSDs where p.FactoryName.Trim() != "" select p.FactoryName.Trim()).Distinct().ToList());
                                    string sBuyerRef = string.Join(", ", (from p in oTempPSDs where p.BuyerRef.Trim() != "" select p.BuyerRef.Trim()).Distinct().ToList());
                                    string sOrderNo = string.Join(", ", (from p in oTempPSDs where p.OrderNo.Trim() != "" select p.OrderNo.Trim()).Distinct().ToList());
                                    string sRSState = string.Join(", ", (from p in oTempPSDs where p.RSStateInString.Trim() != "" select p.RSStateInString.Trim()).Distinct().ToList());
                                    string sYarnName = string.Join(", ", (from p in oTempPSDs where p.ProductName.Trim() != "" select p.ProductName.Trim()).Distinct().ToList());
                                    string sColorName = string.Join(", ", (from p in oTempPSDs where p.ColorName.Trim() != "" select p.ColorName.Trim() + "/" + p.PSBatchNo).Distinct().ToList());
                                    string sBatchCardNo = string.Join(", ", (from p in oTempPSDs where p.BatchCardNo.Trim() != "" select p.BatchCardNo).Distinct().ToList());
                                    string sRemarks = string.Join(", ", (from p in oTempPSDs where p.Remarks.Trim() != "" select p.Remarks.Trim()).Distinct().ToList());
                                    string sTotal = Global.MillionFormat(Convert.ToDouble((from p in oTempPSDs select p.ProductionQty).Sum())) + " kg ";
                                    string sDeliveryDate = (oTempPSDs.Where(o => o.ExpDeliveryDateByFactory != DateTime.MinValue).ToList().Count() > 0) ? oTempPSDs.Where(o => o.ExpDeliveryDateByFactory != DateTime.MinValue).Min(o => o.ExpDeliveryDateByFactory).ToString("dd MMM yyyy") : "";
                                    
                                    _oPdfPCell = new PdfPCell(new Phrase(sFactory, _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                    _oPdfPCell = new PdfPCell(new Phrase("BYR- " + sBuyerRef, _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                    _oPdfPCell = new PdfPCell(new Phrase(sOrderNo, _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                    _oPdfPCell = new PdfPCell(new Phrase(sRSState, _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                    _oPdfPCell = new PdfPCell(new Phrase(sYarnName, _oFontStyleBold));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                    _oPdfPCell = new PdfPCell(new Phrase(sColorName, _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                    _oPdfPCell = new PdfPCell(new Phrase(sBatchCardNo, _oFontStyleBold));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                    _oPdfPCell = new PdfPCell(new Phrase(sTotal, _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                    _oPdfPCell = new PdfPCell(new Phrase(sRemarks, _oFontStyleBold));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                    _oPdfPCell = new PdfPCell(new Phrase(sDeliveryDate, _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);

                                    oTempTable.CompleteRow();

                                    //foreach (ProductionScheduleDetail oPSD in oTempPSDs)
                                    //{
                                        //if (oPSD.BuyerName == "" || (oPSD.BuyerName.Trim() == oPSD.FactoryName.Trim()))
                                        //{
                                        //    _oPdfPCell = new PdfPCell(new Phrase(oPSD.FactoryName.Trim(), _oFontStyle));
                                        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                        //    _oPdfPCell = new PdfPCell(new Phrase("BYR- " + oPSD.BuyerRef.Trim(), _oFontStyle));
                                        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                        //    _oPdfPCell = new PdfPCell(new Phrase(oPSD.OrderNo.Trim().Split(']')[1], _oFontStyle));
                                        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                        //    _oPdfPCell = new PdfPCell(new Phrase( oPSD.RSStateInString, _oFontStyle));
                                        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                        //    _oPdfPCell = new PdfPCell(new Phrase(oPSD.ProductName.Trim(), _oFontStyleBold));
                                        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                        //    _oPdfPCell = new PdfPCell(new Phrase(oPSD.ColorName.Trim() + "/" + oPSD.PSBatchNo, _oFontStyle));
                                        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                        //    _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oPSD.ProductionQty, 2).ToString() + " KG, " + oPSD.BatchCardNo, _oFontStyle));
                                        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                        //    _oPdfPCell = new PdfPCell(new Phrase(oPSD.Remarks.Trim(), _oFontStyleBold));
                                        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
                                        //    oTempTable.CompleteRow();
                                        //}
                                   // }
                                    

                                    _oPdfPCell = new PdfPCell(oTempTable);
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.MinimumHeight = 85f; _oPdfPTable.AddCell(_oPdfPCell);


                                    int nIndex = nj;
                                    nj = _oProductionSchedule.ProductionScheduleList.Count;
                                    _oProductionSchedule.ProductionScheduleList.RemoveAt(nIndex);
                                    flag = 1;


                                }
                                if ((nj == (_oProductionSchedule.ProductionScheduleList.Count - 1)) && flag == 0)
                                {
                                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 85f; _oPdfPTable.AddCell(_oPdfPCell);

                                }

                            }


                        }

                        //if (x > nMaxValue)
                        //{
                        //    for (int i = x; i <= nRows; i++)
                        //    {

                        //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.MinimumHeight = 85f; _oPdfPTable.AddCell(_oPdfPCell);

                        //    }
                        //}

                        _oPdfPTable.CompleteRow();
                    }
                }

            }

        }

        private int RetrunMaxValue(List<CapitalResource> oDyemachines, List<ProductionSchedule> oPSS)
        {
            int nMaxValue = 0;
            int nTempMax = 0;
            foreach (CapitalResource oDM in oDyemachines)
            {
                nTempMax = 0;
                foreach (ProductionSchedule oPS in oPSS)
                {

                    if (oDM.CRID == oPS.MachineID)
                    {
                        ++nTempMax;
                        if (nTempMax > nMaxValue)
                        {
                            nMaxValue = nTempMax;
                        }
                       // oPSS.Remove(oPS);

                    }
                }
            }
            return nMaxValue;
        }
        #endregion
    }
}
