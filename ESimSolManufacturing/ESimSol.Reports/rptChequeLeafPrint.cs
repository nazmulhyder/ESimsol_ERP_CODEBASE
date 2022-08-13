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


namespace ESimSol.Reports
{
    public class rptChequeLeafPrint
    {
        #region Declaration
        Document _oDocument;
        BaseFont _oFontStyle;
        iTextSharp.text.Image _oImage;
        MemoryStream _oMemoryStream = new MemoryStream();
        PdfWriter _oPdfWriter = null;
        PdfContentByte _cb = null;
        Cheque _oCheque = new Cheque();
        ChequeSetup _oChequeSetup = new ChequeSetup();
        bool _bIsDatePrint = false;
        System.Drawing.Image _iACPayee = null;
        System.Drawing.Image _iEquel3 = null;
        ColumnText _ct = null;
        #endregion


        public byte[] PrepareReport(Cheque oCheque, ChequeSetup oChequeSetup, System.Drawing.Image iACPayee, System.Drawing.Image iEquel3, bool bIsDatePrint)
        {
            _bIsDatePrint = bIsDatePrint;
            _oCheque = oCheque;
            _oChequeSetup = oChequeSetup;
            _iACPayee = iACPayee;
            _iEquel3 = iEquel3;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle((float)(_oChequeSetup.Length), (float)(_oChequeSetup.Width)), 0f, 0f, 0f, 0f);
            _oPdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            #endregion

            _oDocument.Open();
            this.PrintBody();
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        
        private void PrintText(string sText, float nX, float nY, float nL, float nFS)
        {
            _cb.BeginText();
            _cb.SetFontAndSize(_oFontStyle, nFS);
            _cb.EndText();

            string sTemp = "", sPrint = "";
            bool bNextline = false;
            float y = (float)((_oChequeSetup.Width - nY))-nFS;
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
                    y = bNextline ? y - nFS-5 : y;

                    _cb.BeginText();
                    _cb.SetTextMatrix((float)(nX), y);
                    _cb.ShowText(sPrint);
                    _cb.EndText();

                    sPrint = sWord + " ";
                    sTemp = sWord + " ";
                    bNextline = true;
                }
            }
            y = bNextline ? y - nFS - 5 : y;
            _cb.BeginText();
            _cb.SetTextMatrix((float)(nX), y);
            _cb.ShowText(sPrint);
            _cb.EndText();
        }

        #region Report Body
        private void PrintBody()
        {            
            _oFontStyle = FontFactory.GetFont(FontFactory.HELVETICA, 10f, iTextSharp.text.Font.NORMAL).BaseFont;
            _cb = _oPdfWriter.DirectContent;


            //payment Type           
            if (_oCheque.PaymentType == EnumPaymentType.AccountPay)
            {
                _oImage = iTextSharp.text.Image.GetInstance(_iACPayee, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImage.ScaleAbsolute((float)(_oChequeSetup.paymentMethodL), (float)(_oChequeSetup.paymentMethodW));

                _oImage.SetAbsolutePosition((float)(_oChequeSetup.PaymentMethodX), (float)((_oChequeSetup.Width - (_oChequeSetup.paymentMethodY + _oChequeSetup.paymentMethodW))));
                _oDocument.Add(_oImage);
            }
            else
            {

                PrintText(".", (float)_oChequeSetup.TPaymentTypeX, (float)_oChequeSetup.TPaymentTypeY, (float)_oChequeSetup.TPaymentTypeL, (float)_oChequeSetup.paymentMethodFS);
            }


            if (_oChequeSetup.IsSplit == true)
            {
                //Date
                if (_bIsDatePrint)
                {
                    string sDate = _oCheque.ChequeDate.ToString(_oChequeSetup.DateFormat.Split(',')[0]);
                    double nPosition = _oChequeSetup.DateX;
                    for (int i = 0; i < sDate.Length; i++)
                    {
                        PrintText(sDate[i].ToString(), (float)nPosition, (float)_oChequeSetup.DateY, (float)(_oChequeSetup.DateL / sDate.Length), (float)_oChequeSetup.DateFS);
                        //nPosition = nPosition + (_oChequeSetup.DateL / sDate.Length);
                        nPosition = nPosition + _oChequeSetup.DateSpace;
                    }
                }
            }
            else
            {
                if (_bIsDatePrint)
                {
                    PrintText(_oCheque.ChequeDate.ToString("dd MM yyyy"), (float)_oChequeSetup.DateX, (float)_oChequeSetup.DateY, (float)_oChequeSetup.DateL, (float)_oChequeSetup.DateFS);
                }
                else
                {
                    PrintText("", (float)_oChequeSetup.DateX, (float)_oChequeSetup.DateY, (float)_oChequeSetup.DateL, (float)_oChequeSetup.DateFS);
                }
            }

            //PayTo
            _oFontStyle = FontFactory.GetFont(FontFactory.HELVETICA, 10f, iTextSharp.text.Font.BOLD).BaseFont;
            PrintText(_oCheque.ChequeIssueTo, (float)_oChequeSetup.PayToX, (float)_oChequeSetup.PayToY, (float)_oChequeSetup.PayToL, (float)_oChequeSetup.PayToFS);

            //Amount In word
            _oFontStyle = FontFactory.GetFont(FontFactory.HELVETICA, 10f, iTextSharp.text.Font.BOLD).BaseFont;
            string sTempTakaInWord = Global.TakaWords(_oCheque.Amount);
            sTempTakaInWord = sTempTakaInWord.Remove(0, 4);
            PrintText("***" + sTempTakaInWord.Trim() + "***", (float)_oChequeSetup.AmountWordX, (float)_oChequeSetup.AmountWordY, (float)_oChequeSetup.AmountWordL, (float)_oChequeSetup.AmountWordFS);

            //equel image
            _oImage = iTextSharp.text.Image.GetInstance(_iEquel3, System.Drawing.Imaging.ImageFormat.Jpeg);
            _oImage.ScaleAbsolute((float)(0.35 * 37.795275591), (float)(0.25 * 37.795275591));
            _oImage.SetAbsolutePosition((float)(_oChequeSetup.AmountX), (float)(_oChequeSetup.Width - (_oChequeSetup.AmountY + _oChequeSetup.AmountFS)));
            _oDocument.Add(_oImage);

            //Amount
            _oFontStyle = FontFactory.GetFont(FontFactory.HELVETICA, 10f, iTextSharp.text.Font.BOLD).BaseFont;
            PrintText(" " + Global.TakaFormat(_oCheque.Amount), (float)(_oChequeSetup.AmountX + (0.35 * 37.795275591)), (float)_oChequeSetup.AmountY, (float)_oChequeSetup.AmountL, (float)_oChequeSetup.AmountFS);
            //PrintText("***" + Global.TakaFormat(_oCheque.Amount) + "***", (float)(_oChequeSetup.AmountX + (0.35 * 37.795275591)), (float)_oChequeSetup.AmountY, (float)_oChequeSetup.AmountL, (float)_oChequeSetup.AmountFS);

            //tDate
            _oFontStyle = FontFactory.GetFont(FontFactory.HELVETICA, 10f, iTextSharp.text.Font.NORMAL).BaseFont;
            if (_bIsDatePrint)
            {
                PrintText(_oCheque.ChequeDate.ToString("dd MM yyyy"), (float)_oChequeSetup.TDateX, (float)_oChequeSetup.TDateY, (float)_oChequeSetup.TDateL, (float)_oChequeSetup.TDateFS);
            }
            else
            {
                PrintText("", (float)_oChequeSetup.TDateX, (float)_oChequeSetup.TDateY, (float)_oChequeSetup.TDateL, (float)_oChequeSetup.TDateFS);
            }

            //tPayTo
            _oFontStyle = FontFactory.GetFont(FontFactory.HELVETICA, 10f, iTextSharp.text.Font.NORMAL).BaseFont;
            PrintText(_oCheque.ContractorName, (float)_oChequeSetup.TPayToX, (float)_oChequeSetup.TPayToY, (float)_oChequeSetup.TPayToL, (float)_oChequeSetup.TPayToFS);

            //tAmount
            _oFontStyle = FontFactory.GetFont(FontFactory.HELVETICA, 10f, iTextSharp.text.Font.NORMAL).BaseFont;
            PrintText("=" + Global.MillionFormat(_oCheque.Amount) + " ", (float)_oChequeSetup.TAmountX, (float)_oChequeSetup.TAmountY, (float)_oChequeSetup.TAmountL, (float)_oChequeSetup.TAmountFS);

            //Barcode Number
            _oFontStyle = FontFactory.GetFont(FontFactory.HELVETICA, 10f, iTextSharp.text.Font.NORMAL).BaseFont;
            PrintText("", (float)(_oChequeSetup.TBookNoX - (1.20 * 37.795275591)), (float)_oChequeSetup.TBookNoY, (float)(3 * 37.795275591), (float)8);

            //tBookNo
            _oFontStyle = FontFactory.GetFont(FontFactory.HELVETICA, 10f, iTextSharp.text.Font.NORMAL).BaseFont;
            PrintText(_oCheque.BookCode, (float)_oChequeSetup.TBookNoX, (float)_oChequeSetup.TBookNoY, (float)_oChequeSetup.TBookNoL, (float)_oChequeSetup.TBookNoFS);
        }
        #endregion
    }
}
