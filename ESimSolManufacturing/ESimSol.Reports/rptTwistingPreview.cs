using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ESimSol.Reports
{
    public class rptTwistingPreview
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);//number of columns
        PdfPCell _oPdfPCell;
        iTextSharp.text.Font _oFontStyleBold;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        DyeingOrder _oDyeingOrder = new DyeingOrder();
        List<TwistingDetail> _TwistingDetails_Product = new List<TwistingDetail>();
        List<TwistingDetail> _TwistingDetails_Twisting = new List<TwistingDetail>();
        Company _oCompany = new Company();
        string _sMessage = "";
        #endregion
        public byte[] PrepareReport(DyeingOrder oDyeingOrder, List<TwistingDetail> TwistingDetails_Product, List<TwistingDetail> TwistingDetails_Twisting,Company oCompany,string sMsg)
        {
            _oDyeingOrder = oDyeingOrder;
            _TwistingDetails_Product = TwistingDetails_Product;
            _TwistingDetails_Twisting = TwistingDetails_Twisting;
            _oCompany = oCompany;
            #region Page Setup
            _oDocument = new Document(PageSize.LEGAL, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 20f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion
            this.PrintHeader();
            this.PrintBody();
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
            oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });

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
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #region Report Body
        private void PrintBody()
        {
            PrintHead_B();
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            GetTopTable2();
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            GetTopTable3();
        }
        #endregion

        private void PrintHead_B()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 65f, 178f, 62f, 135f });
            //// 1st Row
            if (_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.LoanOrder)
            {
                oPdfPCell = new PdfPCell(new Phrase("SUPPLIER NAME", _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("BUYER NAME", _oFontStyle));
            }

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.ContractorName, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("ORDER NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrder.OrderNoFull + " ", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            /// 2nd Row
            oPdfPCell = new PdfPCell(new Phrase("CONCERN PERSON", _oFontStyle));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.ContactPersonnelName, _oFontStyle));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("ISSUE DATE", _oFontStyle));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            if (_oDyeingOrder.ReviseNo > 0)
            {
                oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrder.OrderDateSt + "  REVISE DATE: " + _oDyeingOrder.ReviseDateSt, _oFontStyleBold));
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrder.OrderDateSt, _oFontStyle));
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            }
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //// 3nd Row 

            oPdfPCell = new PdfPCell(new Phrase("DELIVERY NOTE", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Rowspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.DeliveryNote, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Rowspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("PAYMENT TERM", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            string sTemp = "";
            if (_oDyeingOrder.PaymentType == (int)EnumOrderPaymentType.CashOrCheque)
            {
                sTemp = "Cheque or Cash";
            }
            if (_oDyeingOrder.PaymentType == (int)EnumOrderPaymentType.AdjWithNextLC)
            {
                sTemp = "L/C ";
            }
            if (_oDyeingOrder.PaymentType == (int)EnumOrderPaymentType.AdjWithPI)
            {
                sTemp = "Adj With PI ";
            }
            if (_oDyeingOrder.PaymentType == (int)EnumOrderPaymentType.FoC)
            {
                sTemp = "Free Of Cost";
            }
            oPdfPCell = new PdfPCell(new Phrase(" " + sTemp, _oFontStyle));

            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //// 4th Row
            //// Use Rowspan
            if (!String.IsNullOrEmpty(_oDyeingOrder.StyleNo))
            {
                oPdfPCell = new PdfPCell(new Phrase("STYLE NO", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrder.StyleNo, _oFontStyle));
                //oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 2;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            if (!String.IsNullOrEmpty(_oDyeingOrder.RefNo))
            {
                oPdfPCell = new PdfPCell(new Phrase("BUYER REF", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrder.RefNo, _oFontStyle));
                //oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 2;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        //2ND TABLE
        private void GetTopTable2()
        {
            PdfPTable oTopTable = new PdfPTable(9);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                   5f,   //1                                               
                                                                10f,   //2
                                                                10f,    //3
                                                                10f,//4
                                                                10f,   //5                                               
                                                                10f,    //6
                                                                10f,     //7
                                                                10f,//8
                                                                10f//9
                                                          });
            #region HEADER
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Details", _oFontStyle)); _oPdfPCell.Colspan = 9; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Table Initialize
            oTopTable = new PdfPTable(9);
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                5f,//1                                               
                                                                10f, //2
                                                                10f,  //3
                                                                10f,//4
                                                                10f, //5                                               
                                                                10f, //6
                                                                10f,  //7
                                                                10f,//8
                                                                10f//9
                                                          });
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Commodity", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Unit Price", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            int nSL = 1;
            foreach (TwistingDetail oItem in _TwistingDetails_Twisting)
            {
                #region Table Initialize
                oTopTable = new PdfPTable(9);
                oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oTopTable.SetWidths(new float[] {                5f,//1                                               
                                                                10f, //2
                                                                10f,  //3
                                                                10f,//4
                                                                10f, //5                                               
                                                                10f, //6
                                                                10f,  //7
                                                                10f,//8
                                                                10f//9
                                                          });
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductCode, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUnit, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty_Order.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitPrice.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                nSL++;
                oTopTable.CompleteRow();
                #region push into main table
                _oPdfPCell = new PdfPCell(oTopTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            #region TOTAL
            #region Table Initialize
            oTopTable = new PdfPTable(9);
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                  5f,//1                                               
                                                                10f, //2
                                                                10f,  //3
                                                                10f,//4
                                                                10f, //5                                               
                                                                10f, //6
                                                                10f,  //7
                                                                10f,//8
                                                                10f//9
                                                          });
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TwistingDetails_Twisting.Sum(x => x.Qty_Order).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TwistingDetails_Twisting.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
        }

        //3rd table
        private void GetTopTable3()
        {
            PdfPTable oTopTable = new PdfPTable(7);
            oTopTable.WidthPercentage = 100;
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                   5f,   //1                                               
                                                                10f,   //2
                                                                10f,    //3
                                                                10f,//4
                                                                10f,   //5                                               
                                                                10f,    //6
                                                                10f    //7                                                              
                                                          });
            #region HEADER
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Products", _oFontStyle)); _oPdfPCell.Colspan = 7; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Table Initialize
            oTopTable = new PdfPTable(7);
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                5f,//1                                               
                                                                10f, //2
                                                                10f,  //3
                                                                10f,//4
                                                                10f, //5                                               
                                                                10f, //6
                                                                10f,  //7
                                                          });
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Commodity", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD); _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oTopTable.AddCell(_oPdfPCell);

            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            int nSL = 1;
            foreach (TwistingDetail oItem in _TwistingDetails_Product)
            {
                #region Table Initialize
                oTopTable = new PdfPTable(7);
                oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oTopTable.SetWidths(new float[] {                5f,//1                                               
                                                                10f, //2
                                                                10f,  //3
                                                                10f,//4
                                                                10f, //5                                               
                                                                10f, //6
                                                                10f,  //7
                                                          });
                #endregion
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductCode, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUnit, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
                nSL++;
                oTopTable.CompleteRow();
                #region push into main table
                _oPdfPCell = new PdfPCell(oTopTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            #region TOTAL
            #region Table Initialize
            oTopTable = new PdfPTable(7);
            oTopTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopTable.SetWidths(new float[] {                  5f,//1                                               
                                                                10f, //2
                                                                10f,  //3
                                                                10f,//4
                                                                10f, //5                                               
                                                                10f, //6
                                                                10f,  //7
                                                              
                                                         });
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle)); _oPdfPCell.Colspan = 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_TwistingDetails_Product.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            oTopTable.CompleteRow();
            #region push into main table
            _oPdfPCell = new PdfPCell(oTopTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
        }

    }
}
        