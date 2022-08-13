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
    public class rptFabrics
    {
        #region Declaration
        int _nTotalColumn = 13;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(13);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Fabric _oFabric = new Fabric();
        List<Fabric> _oFabrics = new List<Fabric>();

        Company _oCompany = new Company();
        string _sMessage = "";
        int _nCount = 0;
        #endregion

        public byte[] PrepareReport(Fabric oFabric, string sMessage, Company oCompany)
        {
            _oFabrics = oFabric.Fabrics;
            _oCompany = oCompany;
            _sMessage = sMessage;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 40f, 80f, 100f, 130f, 100f, 120f, 100f, 60f, 100f, 55f, 100f, 100f, 100f });
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
            _oPdfPCell = new PdfPCell(new Phrase(_sMessage, _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumn;
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            if (_oFabrics.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("ATML", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Received from H/O", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Prog Given to Dyeing", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Rcv from Dyeing", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Delay", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Send to HO", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Days Taken", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Remarks ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("MKT Executive", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();



                List<string> sIssueDateList = _oFabrics.OrderBy(x => x.IssueDate).Select(o => o.IssueDateInString).Distinct().ToList();

                foreach (string sDate in sIssueDateList)
                {
                    List<Fabric> oTempFabrics = new List<Fabric>();
                    oTempFabrics = _oFabrics.Where(o => o.IssueDateInString == sDate).ToList();

                    for (int i = 0; i < oTempFabrics.Count; i++)
                    {
                        _oPdfPCell = new PdfPCell(SetSerialNumber());
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        #region Set Issue Date
                        int mid = ((oTempFabrics.Count % 2) == 0 ? oTempFabrics.Count / 2 : (oTempFabrics.Count - 1) / 2);

                        if (i < mid)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            if (i == 0)
                            {
                                _oPdfPCell.BorderWidthBottom = 0;
                            }
                            else {
                                _oPdfPCell.BorderWidthBottom = 0;
                                _oPdfPCell.BorderWidthTop = 0;
                            }
                            _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (i == mid)
                        {
                            _oPdfPCell = new PdfPCell(SetUniqueIssueDate(sDate));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            if (mid != 0)
                            {
                                _oPdfPCell.BorderWidthTop = 0;
                                _oPdfPCell.BorderWidthBottom = 0;
                            }
                            _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            if (i == oTempFabrics.Count - 1)
                            {
                               
                                _oPdfPCell.BorderWidthTop = 0;
                            }
                            else 
                            {
                                _oPdfPCell.BorderWidthBottom = 0;
                                _oPdfPCell.BorderWidthTop = 0;
                            }
                            _oPdfPTable.AddCell(_oPdfPCell);
                        }

                        #endregion

                        _oPdfPCell = new PdfPCell(SetFabricValues(oTempFabrics[i]));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.Colspan = _nTotalColumn - 2;
                        _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                    }
                }
            }
        }
        #endregion

        private PdfPTable SetFabricValues(Fabric oFabric)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            PdfPTable oPdfPTable = new PdfPTable(11);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100f, 130f, 100f, 120f, 100f, 60f, 100f, 55f, 100f, 100f, 100f });

            oPdfPCell = new PdfPCell(new Phrase(oFabric.FabricNo, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabric.BuyerName, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabric.ReceiveDateSt, _oFontStyle)); //RAndDRcvDate
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //ProgramGivenToDyeing
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //ReceivedFromDyeing
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //Delay
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabric.SubmissionDateSt, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);


            int nDaysTaken = Convert.ToInt32((oFabric.ReceiveDate - oFabric.SubmissionDate).TotalDays);
            if (oFabric.SubmissionDateSt == "-")
            {
                nDaysTaken = 0;
            }
            string sDaysTaken = (nDaysTaken <= 0 ? "-" : nDaysTaken.ToString());

            oPdfPCell = new PdfPCell(new Phrase(sDaysTaken, _oFontStyle)); //DaysTaken
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabric.Construction, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabric.Remarks, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oFabric.MKTPersonName, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }
        private PdfPTable SetUniqueIssueDate(string sDate)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            PdfPTable oPdfPTable = new PdfPTable(1);
            PdfPCell oPdfPCell;

            oPdfPCell = new PdfPCell(new Phrase(sDate, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.BorderWidthTop = 0;
            oPdfPCell.BorderWidthBottom = 0;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }
        private PdfPTable SetSerialNumber()
        {
            _nCount++;

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            PdfPTable oPdfPTable = new PdfPTable(1);
            PdfPCell oPdfPCell;

            oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }
    }
}
