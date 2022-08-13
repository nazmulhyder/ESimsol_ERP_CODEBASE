using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;

namespace ESimSol.Reports
{
    public class rptBDYEACERTIFICATE
    {
        #region Declaration
        Document _oDocument;
        BaseFont _oFontStyle;
        MemoryStream _oMemoryStream = new MemoryStream();
        PdfWriter _oPdfWriter = null;
        PdfContentByte _cb = null;
        BDYEAC _oBDYEAC = new BDYEAC();
        BDYEACDetail _oBDYEACDetail = new BDYEACDetail();
        List<BDYEACDetail> _oBDYEACDetails = new List<BDYEACDetail>();
        #endregion


        public byte[] PrepareReport(BDYEAC oBDYEAC)
        {
            _oBDYEAC = oBDYEAC;
            _oBDYEACDetails = oBDYEAC.BDYEACDetails;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(591.130137f, 807.3972603f), 0f, 0f, 0f, 0f);//842*595
            _oPdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            #endregion

            _oDocument.Open();
            this.PrintBody();
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintText(string sText, float nX, float nY, float nL, float nFS, bool bIsBoold)
        {
            _oFontStyle = FontFactory.GetFont(FontFactory.HELVETICA, 10f).BaseFont;
            if (bIsBoold)
            {
                _oFontStyle = FontFactory.GetFont(FontFactory.HELVETICA, 10f, iTextSharp.text.Font.BOLD).BaseFont;
            }

            _cb.BeginText();
            _cb.SetFontAndSize(_oFontStyle, nFS);
            _cb.EndText();

            string sTemp = "", sPrint = "";
            bool bNextline = false;
            float y = (float)((807.3972603f - nY)) - nFS;
            string[] aText = sText.Split(' ');
            foreach (string sWord in aText)
            {
                sTemp += sWord + " ";
                if (nL > _cb.GetEffectiveStringWidth(sTemp, false))
                {
                    sPrint += sWord + " ";
                }
                else
                {
                    y = bNextline ? y - nFS : y;

                    _cb.BeginText();
                    _cb.SetTextMatrix((float)(nX), y);
                    _cb.ShowText(sPrint);
                    _cb.EndText();

                    sPrint = sWord + " ";
                    sTemp = sWord + " ";
                    bNextline = true;
                }
            }
            y = bNextline ? y - nFS : y;
            _cb.BeginText();
            _cb.SetTextMatrix((float)(nX), y);
            _cb.ShowText(sPrint);
            _cb.EndText();
        }

        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont(FontFactory.HELVETICA, 10f, iTextSharp.text.Font.BOLD).BaseFont;
            _cb = _oPdfWriter.DirectContent;

            _cb.BeginText();
            _cb.SetFontAndSize(_oFontStyle, 9.5f);
            _cb.EndText();

            #region Master LC/NO
            this.PrintText(_oBDYEAC.MasterLCNos, this.GetPixel(1.9f), this.GetPixel(2.4f), this.GetPixel(3.5f), 9.0f, false);
            #endregion

            #region Master LC Date
            this.PrintText(_oBDYEAC.MasterLCDates, this.GetPixel(6.1f), this.GetPixel(2.4f), this.GetPixel(1.7f), 9.0f, false);
            #endregion

            #region Garments Qty
            this.PrintText(_oBDYEAC.GarmentsQty, this.GetPixel(3.5f), this.GetPixel(2.8f), this.GetPixel(3.9f), 9.0f, false);
            #endregion

            #region Export LC/NO
            this.PrintText(_oBDYEAC.ExportLCNo, this.GetPixel(2.3f), this.GetPixel(3.25f), this.GetPixel(3.2f), 9.0f, false);
            #endregion

            #region Export LC Date
            this.PrintText(_oBDYEAC.LCOpeningDate.ToString("dd.MM.yyyy"), this.GetPixel(6.1f), this.GetPixel(3.25f), this.GetPixel(1.6f), 9.0f, false);
            #endregion

            #region Bank Name
            this.PrintText(_oBDYEAC.BankName, this.GetPixel(2.8f), this.GetPixel(3.60f), this.GetPixel(4f), 9.0f, false);
            #endregion

            #region Item Details
            float nY = this.GetPixel(4.9f); int nCount = 1; float BillNoY = 5f;
            double nTotalQtyInLbs = 0, nTotalQtyInKg = 0;
            foreach (BDYEACDetail oItem in _oBDYEACDetails)
            {
                this.PrintText(oItem.ProductName, this.GetPixel(0.45f), nY, this.GetPixel(2.3f), 7.5f, false);
                this.PrintText(Global.MillionFormat_Round(oItem.Qty) + " Lbs", this.GetPixel(2.85f), nY, this.GetPixel(0.8f), 7.5f, false);
                this.PrintText(Global.MillionFormat(oItem.QtyInKg) + " Kgs", this.GetPixel(3.75f), nY, this.GetPixel(0.8f), 7.5f, false);
                if (nCount == _oBDYEACDetails.Count)
                {
                    BillNoY = nY;
                }
                nY = nY + 8f;
                nTotalQtyInLbs += oItem.Qty;
                nTotalQtyInKg += oItem.QtyInKg;
                nCount++;
            }

            #region Line
            float nLineY = 807.3972603f - nY - 2.5f;
            _cb.SetColorStroke(BaseColor.BLACK);
            _cb.MoveTo(this.GetPixel(0.45f), nLineY);
            _cb.LineTo(this.GetPixel(7.65f), nLineY);
            _cb.ClosePathStroke();
            #endregion

            #region Grand Total
            nY = nY + 2.41780822f;
            this.PrintText("Total:", this.GetPixel(1.15f), nY, this.GetPixel(1.6f), 7.5f, true);

            this.PrintText(Global.MillionFormat_Round(nTotalQtyInLbs) + " Lbs", this.GetPixel(2.85f), nY, this.GetPixel(0.8f), 7.5f, true);

            this.PrintText(Global.MillionFormat(nTotalQtyInKg) + " Kgs", this.GetPixel(3.75f), nY, this.GetPixel(0.8f), 7.5f, true);
            #endregion

            #region KG IN Word
            nY = nY + 10f;
            this.PrintText("(" + this.InWord(Convert.ToDecimal(nTotalQtyInKg)) + " Kgs)", this.GetPixel(2.85f), nY, this.GetPixel(4.9f), 7.5f, true);
            #endregion

            #endregion

            #region Delivery Date
            this.PrintText(_oBDYEAC.DeliveryDate.ToString("dd.MM.yyyy"), this.GetPixel(4.65f), BillNoY, this.GetPixel(0.9f), 8f, false);
            #endregion

            #region Invoice No
            this.PrintText(_oBDYEAC.BUShortName + "-" + _oBDYEAC.ExportBillNo, this.GetPixel(5.85f), BillNoY, this.GetPixel(1.1f), 8f, false);
            #endregion

            #region Invoice Date
            this.PrintText(_oBDYEAC.InvoiceDate.ToString("dd.MM.yyyy"), this.GetPixel(7.05f), BillNoY, this.GetPixel(1.2f), 8f, false);
            #endregion

            #region Supplier Name
            this.PrintText(_oBDYEAC.SupplierName, this.GetPixel(1.5f), this.GetPixel(6.32f), this.GetPixel(6f), 9f, false);
            #endregion

            #region ImportLC No
            this.PrintText(_oBDYEAC.ImportLCNo, this.GetPixel(3.4f), this.GetPixel(6.73f), this.GetPixel(2.31f), 9f, false);
            #endregion

            #region ImportLC Date
            this.PrintText(_oBDYEAC.ImportLCDate.ToString("dd.MM.yyyy"), this.GetPixel(6.3f), this.GetPixel(6.73f), this.GetPixel(1.32f), 9f, false);
            #endregion

            #region Beneficiary Name Address
            this.PrintText(_oBDYEAC.BUName + ", " + _oBDYEAC.BUAddress, this.GetPixel(3.7f), this.GetPixel(7.1f), this.GetPixel(3.7f), 9f, false);
            #endregion

            #region Party Name Address
            this.PrintText(_oBDYEAC.PartyName + ", " + _oBDYEAC.PartyAddress, this.GetPixel(3.7f), this.GetPixel(7.80f), this.GetPixel(3.7f), 9f, false);
            #endregion
        }

        private float GetPixel(float nInch)
        {
            float nSizeInPixel = 0f;
            nSizeInPixel = (72.0890411f * nInch);
            return nSizeInPixel;
        }

        public string InWord(decimal number)
        {
            string[] digit = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            string[] baseten = { "", "", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            string[] expo = { "", "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion" };

            if (number == Decimal.Zero)
                return "Zero";
                        
            decimal n = Decimal.Truncate(number);
            decimal cents = Convert.ToDecimal(Global.MillionFormat(number).Split('.')[1]); //Decimal.Truncate((number - n) * 100);

            StringBuilder sb = new StringBuilder();
            int thousands = 0;
            decimal power = 1;

            if (n < 0)
            {
                sb.Append("Minus ");
                n = -n;
            }

            for (decimal i = n; i >= 1000; i /= 1000)
            {
                power *= 1000;
                thousands++;
            }

            bool sep = false;
            for (decimal i = n; thousands >= 0; i %= power, thousands--, power /= 1000)
            {
                int j = (int)(i / power);
                int k = j % 100;
                int hundreds = j / 100;
                int tens = j % 100 / 10;
                int ones = j % 10;

                if (j == 0)
                    continue;

                if (hundreds > 0)
                {
                    if (sep)
                        sb.Append(" "); //sb.Append(", ");

                    sb.Append(digit[hundreds]);
                    sb.Append(" Hundred");
                    sep = true;
                }

                if (k != 0)
                {
                    if (sep)
                    {
                        sb.Append(" "); //sb.Append(" And ");
                        sep = false;
                    }

                    if (k < 20)
                        sb.Append(digit[k]);
                    else
                    {
                        sb.Append(baseten[tens]);
                        if (ones > 0)
                        {
                            sb.Append(" "); //sb.Append("-");
                            sb.Append(digit[ones]);
                        }
                    }
                }

                if (thousands > 0)
                {
                    sb.Append(" ");
                    sb.Append(expo[thousands]);
                    sep = true;
                }
            }

            sb.Append(" "); //sb.Append(" And ");
            if (cents > 0)
            {
                sb.Append(" Point ");
                char[] aCents = Convert.ToString(cents).ToCharArray();
                foreach (char c in aCents)
                {

                    int nDigit = Convert.ToInt32(c.ToString());
                    if (nDigit == 0)
                    {
                        sb.Append("Zero");
                    }
                    else
                    {
                        sb.Append(digit[nDigit]);
                    }
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}
