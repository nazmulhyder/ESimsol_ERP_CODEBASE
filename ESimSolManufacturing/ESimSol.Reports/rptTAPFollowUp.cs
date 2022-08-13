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
    public class rptTAPFollowUp
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Image _oImag;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.BaseColor _oBackGroundColor;
        PdfPTable _oPdfPTable = new PdfPTable(3);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<TAP> _oTAPs = new List<TAP>();
        List<TAPDetail> _oTAPDetails = new List<TAPDetail>();
        Company _oCompany = new Company();
        OrderRecap _oOrderRecap = new OrderRecap();
        float _nPageWidth = 0;
        #endregion
    
        public byte[] PrepareReport(List<TAP> oTAPs,  List<TAPDetail> oTAPDetails, Company oCompany)
        {
            _oTAPDetails = oTAPDetails;
            _oCompany = oCompany;
            _oTAPs = oTAPs;

            #region Page Setup
            _nPageWidth = (this.GetMaxStep() * 50);
            _oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(new iTextSharp.text.Rectangle(_nPageWidth, 595f));//842*595
            _oDocument.SetMargins(30f, 30f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            float[] TempColumnSize = new float[3];
            TempColumnSize[0] = _nPageWidth / 3;
            TempColumnSize[1] = _nPageWidth / 3;
            TempColumnSize[2] = _nPageWidth / 3;

            _oPdfPTable.SetWidths(TempColumnSize);
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
            #region CompanyHeader
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(160f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
            }
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("TAP Followup", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion
            _oPdfPTable.CompleteRow();
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            #region color Legend
            PdfPTable oTemPdfPTable = new PdfPTable(6);
            //oTemPdfPTable.TotalWidth = 310f;
            oTemPdfPTable.SetWidths(new float[]{
                                                100f,//Green Block
                                                20f,//On Date Caption
                                                100f,//Yellow Block
                                                20f,//Late date Captoin
                                                100f,//Red Block
                                                20f//Pending caption
                                                });
            _oPdfPCell = new PdfPCell(new Phrase("On Date Done"));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GREEN; oTemPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTemPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Late Done"));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.YELLOW; oTemPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTemPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Pending"));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.RED; oTemPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTemPdfPTable.AddCell(_oPdfPCell);
            oTemPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oTemPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            #endregion
            foreach (TAP oTAP in _oTAPs)
            {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Plan No :" + oTAP.PlanNo + " || Style No:" +oTAP.StyleNo + " || Recap No :" + oTAP.OrderRecapNo+ " || Buyer:" + oTAP.BuyerName+ " || Shipment Date :" + oTAP.ShipmentDateInString+ " || Factory : " + oTAP.ProductionFactoryName+" || Qty : "+ Global.MillionFormat(oTAP.Quantity,0) +" "+ oTAP.UnitName+" || Merchandiser :"+ oTAP.MerchandiserName, _oFontStyle));
                    _oPdfPCell.FixedHeight = 18; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(GetTAPDetailWisePdfTable(oTAP.TAPID));
                    _oPdfPCell.Colspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                  
            }  
        }

        private PdfPTable GetTAPDetailWisePdfTable(int nTAPID)
        {
            List<TAPDetail> oTAPDetails = new List<TAPDetail>();
            oTAPDetails = GetDetailList(nTAPID);

            #region Declare PdfTable
            int nDynamicColumn = GetTotalColumn(oTAPDetails);
            PdfPTable oPdfPTable = new PdfPTable(nDynamicColumn);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            float[] TempColumnSize = new float[nDynamicColumn];
            int count = 0;
            for (int i = 0; i < nDynamicColumn ; i++)
            {
                TempColumnSize[count] = (_nPageWidth / nDynamicColumn);
                count++;
            }
            oPdfPTable.SetWidths(TempColumnSize);
            #endregion

            #region Heading
            #region 1st Portion
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            foreach (TAPDetail oItem in oTAPDetails)
            {
                int nColorNumber = GetColorNumber(oItem, oItem.TAPExecution);
                _oBackGroundColor = GetColorName(nColorNumber);
                string sTempEXecutinoDateInString = "";
                if(oItem.ExecutionIsDone)
                {
                    sTempEXecutinoDateInString = "\n Done: " + oItem.ExecutionDoneDateInString;
                }
                _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderStepName + "(" + oItem.ApprovalPlanDateInString + ")" + sTempEXecutinoDateInString, _oFontStyle));
                _oPdfPCell.Colspan = oItem.ChildOrderSteps.Count; _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = _oBackGroundColor; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            #endregion

            #region Print Step Name
            foreach (TAPDetail oItem in oTAPDetails)
            {
                if(oItem.ChildOrderSteps.Count>0)
                {
                    foreach(TAPDetail oChildStepItem in oItem.ChildOrderSteps)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.BOLD);
                        int nColorNumber = GetColorNumber(oChildStepItem, oChildStepItem.TAPExecution);
                        _oBackGroundColor = GetColorName(nColorNumber);
                        _oPdfPCell = new PdfPCell(new Phrase(oChildStepItem.OrderStepName + "\n" + oChildStepItem.ApprovalPlanDateInString, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = _oBackGroundColor; oPdfPTable.AddCell(_oPdfPCell);
                    }
                }
           }
            oPdfPTable.CompleteRow();
            #endregion
            #endregion

            #region Value Print
            foreach (TAPDetail oItem in oTAPDetails)
            {
                if(oItem.ChildOrderSteps.Count>0)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                    foreach (TAPDetail oChildStepItem in oItem.ChildOrderSteps)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(GetExecutionValue(oItem.TAPExecutions, oChildStepItem.TAPDetailID), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                }
            }
            oPdfPTable.CompleteRow();
            #endregion
           
            return oPdfPTable;
        }
        private List<TAPDetail> GetDetailList (int id)
        {
            List<TAPDetail> oTempTAPDetails = new List<TAPDetail>();
            foreach(TAPDetail oItem in _oTAPDetails)
            {
                if(id == oItem.TAPID)
                {
                    oTempTAPDetails.Add(oItem);
                }
            }
            return oTempTAPDetails;
        }

        private int GetTotalColumn(List<TAPDetail> oTAPDetails)
        {
            int nTempColumn = 0;
            foreach(TAPDetail oItem in oTAPDetails)
            {
               nTempColumn+= oItem.ChildOrderSteps.Count;
            }
            return nTempColumn;
        }
        private string GetExecutionValue(List<TAPExecution> oTAPExecutions, int nTAPDetailID)
        {
            string sTempString = " ";
            foreach(TAPExecution oItem in oTAPExecutions)
            {
                if(oItem.TAPDetailID== nTAPDetailID)
                {
                    return oItem.UpdatedData;
                }
            }
            return sTempString;
        }
       
      private int GetMaxStep()
        {
            int nMaxStep= 0;
            int nTempStep = 0;
            foreach (TAP oItem in _oTAPs)
            {
                foreach(TAPDetail oDetailITem in _oTAPDetails)
                {
                    if(oItem.TAPID == oDetailITem.TAPID)
                    {
                        nTempStep+=oDetailITem.ChildOrderSteps.Count;
                    }
                }
                if (nTempStep > nMaxStep)
                {
                    nMaxStep = nTempStep;
                }
                nTempStep = 0;
            }
            return nMaxStep;
        }

      private int GetColorNumber(TAPDetail oTAPDetail, TAPExecution oTAPExecution)
      {
          if ((oTAPDetail.ApprovalPlanDate) != DateTime.MinValue)
          {

              if (oTAPExecution.IsDone == true)
              {
                  if (oTAPDetail.ApprovalPlanDate >= oTAPExecution.DoneDate)
                  {
                      return 1;//Green color ;; On Time or before
                  }
                  else
                  {
                      return 2;//Yellow Color ;; Late Done
                  }
              }
              else
              {

                  if (oTAPDetail.ApprovalPlanDate >= DateTime.Today)
                  {
                      return 0;//No color ;; not done date not expired
                  }
                  else
                  {
                      return 3;//Red Color ;; Not Done but date is Expired
                  }
              }
          }

          return 0;// No Color
      }

       private BaseColor GetColorName(int nColorNumber)
      {
          //0=Nocolor;1=Green;2=Yellow;3=Red
           if(nColorNumber==1)
           {
               return BaseColor.GREEN;
           }else if(nColorNumber==2)
           {
               return BaseColor.YELLOW;
           }else if(nColorNumber==3)
           {
               return BaseColor.RED;
           }
           return BaseColor.WHITE;
      }
        #endregion
    }  
 
}
