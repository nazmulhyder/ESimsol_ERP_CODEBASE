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

    public class rptFollowUpSheet
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<OrderRecap> _oOrderRecaps = new List<OrderRecap>();
        Company _oCompany = new Company();
        #endregion

        public byte[] PrepareReport(List<OrderRecap> oOrderRecaps, Company oCompany)
        {
            _oOrderRecaps = oOrderRecaps;
            _oCompany = oCompany;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(1288, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(new iTextSharp.text.Rectangle(1288, 595f));
            _oDocument.SetMargins(15f, 15f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 1258f });
            #endregion

            this.PrintHeader();
          //  this.PrintBody();
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
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
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
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

        
            #endregion

        }
        #endregion

        #region Report Body
        //private void PrintBody()
        //{

            

        //    #region Heading
        //    _oPdfPCell = new PdfPCell( GetHeading());
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();
       
        //    #endregion

        //    #region Details
        //    int nCount = 0;
        //    int nSLCount = 0;
        //    double nTempDoQty = 0;
        //    foreach (OrderRecap oItem in _oOrderRecaps)
        //    {
        //        PdfPTable oPdfPTable = new PdfPTable(23);
        //        oPdfPTable.SetWidths(new float[] { 
        //                                        26f, // SL No
        //                                        60f, // Recap Captions
        //                                        100f, // Recap Values
        //                                        90f, // Technical Sheet Image
        //                                        70f, // LabDip Color Name
        //                                        55f, // LabDip Supplier Name
        //                                        30f, // Approve shade
        //                                        70f, // DO Color Name
        //                                        55f, // Do Supplier Name
        //                                        40f, // Dyeing Order Qty
        //                                        40f, // In House Qty
        //                                        60f, // Smple Rqmnt Detls
        //                                        40f, // smple rqmnt Remarks
        //                                        70f, // Print & Emboidery Details
        //                                        40f, // Print & Emboidery CustInfo
        //                                        40f, // Print & Emboidery Approval
        //                                        70f, // Accesories Details 
        //                                        40f, // Accesories Cust Info 
        //                                        70f, // Accesories Buyer Approval 
        //                                        60f, // Accesories FTy Approval 
        //                                        45f, // Accesories Comments 
        //                                        40f, // PO Sheet
        //                                        40f  //  Quantity
            
            
        //                                 });
        //        int nMaxRowSize = GetMaxRowSize(oItem);
        //        nCount = 0;
        //        #region SL No Print
        //        nSLCount++;
        //        _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
        //        _oPdfPCell = new PdfPCell(new Phrase(nSLCount.ToString(), _oFontStyle));
        //        _oPdfPCell.Rowspan = nMaxRowSize; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        #endregion

        //        #region Buyer
        //        _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        if (oItem.StyleCoverImage!= null)
        //        {
        //            _oImag = iTextSharp.text.Image.GetInstance(oItem.StyleCoverImage, System.Drawing.Imaging.ImageFormat.Jpeg);
        //            //_oImag.Border = 1;
        //            _oImag.ScaleAbsolute(85f,70f);
        //            _oPdfPCell = new PdfPCell(_oImag);
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
        //            _oPdfPCell.Padding = 2f;
        //            _oPdfPCell.Rowspan = 12;
        //            oPdfPTable.AddCell(_oPdfPCell);
        //        }
        //        else
        //        {
        //            _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
        //            _oPdfPCell.Rowspan = 12; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        }

        //        // Lab Dip Details Information Start
        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount<oItem.LabDipOrderDetails.Count)?oItem.LabDipOrderDetails[nCount].ColorName:"", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //         // Lab Dip Details Information End

        //        // Dyeing Order Details Information Start

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        #region Calculate DO Total Qty
        //        //if (nCount < oItem.DyeingOrderDetails.Count)
        //        //{
        //        //  string sValue =GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails);
        //        //    if(sValue!="")
        //        //    {
        //        //        nTempDoQty += Convert.ToDouble(sValue);
        //        //    }
        //        //}
        //        #endregion

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Dyeing Order Details Information End


               

        //        //Sample Required Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        //Sample Required End

        //        // Print & Embroidery Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy>0)?"OK":"NOT OK" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Print & Embroidery End


        //        // Accessories Details Start
        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy!=0)?"OK":"N/A" : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Accessories Details End

        //        // PO Sheet  And Qty Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // PO Sheet And Qty End
        //        nCount++;
        //        #endregion

        //        #region Brand
        //        _oPdfPCell = new PdfPCell(new Phrase("Brand", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.BrandName, _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information Start
        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ColorName : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information End

        //        // Dyeing Order Details Information Start

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        #region Calculate DO Total Qty
        //        //if (nCount < oItem.DyeingOrderDetails.Count)
        //        //{
        //        //    string sValue = GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails);
        //        //    if (sValue != "")
        //        //    {
        //        //        nTempDoQty += Convert.ToDouble(sValue);
        //        //    }
        //        //}
        //        #endregion
        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Dyeing Order Details Information End



        //        //Sample Required Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        //Sample Required End

        //        // Print & Embroidery Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy > 0) ? "OK" : "NO" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Print & Embroidery End


        //        // Accessories Details Start
        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy != 0) ? "OK" : "N/A" : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Accessories Details End

        //        // PO Sheet  And Qty Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // PO Sheet And Qty End
        //        nCount++;

        //        #endregion

        //        #region Department
        //        _oPdfPCell = new PdfPCell(new Phrase("Department", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.DeptName, _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information End

        //        // Dyeing Order Details Information Start

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        #region Calculate DO Total Qty
        //        if (nCount < oItem.DyeingOrderDetails.Count)
        //        {
        //            string sValue = GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails);
        //            if (sValue != "")
        //            {
        //                nTempDoQty += Convert.ToDouble(sValue);
        //            }
        //        }
        //        #endregion
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Dyeing Order Details Information End



        //        //Sample Required Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        //Sample Required End

        //        // Print & Embroidery Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy > 0) ? "OK" : "NO" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Print & Embroidery End


        //        // Accessories Details Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy != 0) ? "OK" : "N/A" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Accessories Details End

        //        // PO Sheet  And Qty Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // PO Sheet And Qty End
        //        nCount++;

        //        #endregion

        //        #region Order Number

        //        _oPdfPCell = new PdfPCell(new Phrase("Order Number", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderRecapNo, _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


        //        // Lab Dip Details Information Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information End

        //        // Dyeing Order Details Information Start

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        #region Calculate DO Total Qty
        //        if (nCount < oItem.DyeingOrderDetails.Count)
        //        {
        //            string sValue = GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails);
        //            if (sValue != "")
        //            {
        //                nTempDoQty += Convert.ToDouble(sValue);
        //            }
        //        }
        //        #endregion
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Dyeing Order Details Information End

        //        //Sample Required Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        //Sample Required End

        //        // Print & Embroidery Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy > 0) ? "OK" : "NO" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Print & Embroidery End


        //        // Accessories Details Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy != 0) ? "OK" : "N/A" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Accessories Details End

        //        // PO Sheet  And Qty Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // PO Sheet And Qty End
        //        nCount++;
        //        #endregion

        //        #region Art/Ref Number

        //        _oPdfPCell = new PdfPCell(new Phrase("Art/Ref Number", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information End

        //        // Dyeing Order Details Information Start

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        #region Calculate DO Total Qty
        //        if (nCount < oItem.DyeingOrderDetails.Count)
        //        {
        //            string sValue = GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails);
        //            if (sValue != "")
        //            {
        //                nTempDoQty += Convert.ToDouble(sValue);
        //            }
        //        }
        //        #endregion
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Dyeing Order Details Information End


        //        //Sample Required Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        //Sample Required End

        //        // Print & Embroidery Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy > 0) ? "OK" : "NO" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Print & Embroidery End


        //        // Accessories Details Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy != 0) ? "OK" : "N/A" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Accessories Details End

        //        // PO Sheet  And Qty Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // PO Sheet And Qty End
        //        nCount++;
        //        #endregion

        //        #region Order Quantity

        //        _oPdfPCell = new PdfPCell(new Phrase("Order Quantity", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.TotalQuantity), _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

              
        //        // Lab Dip Details Information End

        //        // Dyeing Order Details Information Start

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        #region Calculate DO Total Qty
        //        if (nCount < oItem.DyeingOrderDetails.Count)
        //        {
        //            string sValue = GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails);
        //            if (sValue != "")
        //            {
        //                nTempDoQty += Convert.ToDouble(sValue);
        //            }
        //        }
        //        #endregion
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Dyeing Order Details Information End

        //        //Sample Required Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        //Sample Required End

        //        // Print & Embroidery Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy > 0) ? "OK" : "NO" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Print & Embroidery End


        //        // Accessories Details Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy != 0) ? "OK" : "N/A" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Accessories Details End

        //        // PO Sheet  And Qty Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // PO Sheet And Qty End
        //        nCount++;
        //        #endregion

        //        #region Shipment Date

        //        _oPdfPCell = new PdfPCell(new Phrase("Shipment Date", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.ShipmentDateInString, _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


        //        // Lab Dip Details Information End

        //        // Dyeing Order Details Information Start

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        #region Calculate DO Total Qty
        //        if (nCount < oItem.DyeingOrderDetails.Count)
        //        {
        //            string sValue = GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails);
        //            if (sValue != "")
        //            {
        //                nTempDoQty += Convert.ToDouble(sValue);
        //            }
        //        }
        //        #endregion
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Dyeing Order Details Information End

        //        //Sample Required Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        //Sample Required End

        //        // Print & Embroidery Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy > 0) ? "OK" : "NO" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Print & Embroidery End


        //        // Accessories Details Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy != 0) ? "OK" : "N/A" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Accessories Details End

        //        // PO Sheet  And Qty Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // PO Sheet And Qty End
        //        nCount++;
        //        #endregion

        //        #region Unit Price

        //        _oPdfPCell = new PdfPCell(new Phrase("Unit Price", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.CurrencySymbol +Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information End

        //        // Dyeing Order Details Information Start

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        #region Calculate DO Total Qty
        //        if (nCount < oItem.DyeingOrderDetails.Count)
        //        {
        //            string sValue = GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails);
        //            if (sValue != "")
        //            {
        //                nTempDoQty += Convert.ToDouble(sValue);
        //            }
        //        }
        //        #endregion
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Dyeing Order Details Information End


        //        //Sample Required Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        //Sample Required End

        //        // Print & Embroidery Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy > 0) ? "OK" : "NO" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Print & Embroidery End


        //        // Accessories Details Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy != 0) ? "OK" : "N/A" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Accessories Details End

        //        // PO Sheet  And Qty Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // PO Sheet And Qty End
        //        nCount++;
        //        #endregion

        //        #region LC Value

        //        _oPdfPCell = new PdfPCell(new Phrase("LC Value", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.LCValue) , _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


        //        // Lab Dip Details Information End

        //        // Dyeing Order Details Information Start

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        #region Calculate DO Total Qty
        //        if (nCount < oItem.DyeingOrderDetails.Count)
        //        {
        //            string sValue = GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails);
        //            if (sValue != "")
        //            {
        //                nTempDoQty += Convert.ToDouble(sValue);
        //            }
        //        }
        //        #endregion
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Dyeing Order Details Information End


        //        //Sample Required Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        //Sample Required End

        //        // Print & Embroidery Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy > 0) ? "OK" : "NO" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Print & Embroidery End


        //        // Accessories Details Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy != 0) ? "OK" : "N/A" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Accessories Details End

        //        // PO Sheet  And Qty Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // PO Sheet And Qty End
        //        nCount++;
        //        #endregion

        //        #region Comosition

        //        _oPdfPCell = new PdfPCell(new Phrase("Composition", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricName, _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


        //        // Lab Dip Details Information Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


        //        // Lab Dip Details Information End

        //        // Dyeing Order Details Information Start

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        #region Calculate DO Total Qty
        //        if (nCount < oItem.DyeingOrderDetails.Count)
        //        {
        //            string sValue = GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails);
        //            if (sValue != "")
        //            {
        //                nTempDoQty += Convert.ToDouble(sValue);
        //            }
        //        }
        //        #endregion
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Dyeing Order Details Information End

        //        //Sample Required Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        //Sample Required End

        //        // Print & Embroidery Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy > 0) ? "OK" : "NO" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Print & Embroidery End


        //        // Accessories Details Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy != 0) ? "OK" : "N/A" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Accessories Details End

        //        // PO Sheet  And Qty Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // PO Sheet And Qty End
        //        nCount++;
        //        #endregion

        //        #region Yarn Count Ply

        //        _oPdfPCell = new PdfPCell(new Phrase("Yarn Count Ply", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.Count, _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Lab Dip Details Information Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information End

        //        // Dyeing Order Details Information Start

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        #region Calculate DO Total Qty
        //        if (nCount < oItem.DyeingOrderDetails.Count)
        //        {
        //            string sValue = GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails);
        //            if (sValue != "")
        //            {
        //                nTempDoQty += Convert.ToDouble(sValue);
        //            }
        //        }
        //        #endregion
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Dyeing Order Details Information End

        //        //Sample Required Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        //Sample Required End

        //        // Print & Embroidery Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy > 0) ? "OK" : "NO" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Print & Embroidery End


        //        // Accessories Details Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy != 0) ? "OK" : "N/A" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Accessories Details End

        //        // PO Sheet  And Qty Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // PO Sheet And Qty End
        //        nCount++;
        //        #endregion

        //        #region Gauge

        //        _oPdfPCell = new PdfPCell(new Phrase("Gauge", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.GG, _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


        //        // Lab Dip Details Information Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Lab Dip Details Information End

        //        // Dyeing Order Details Information Start

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        #region Calculate DO Total Qty
        //        if (nCount < oItem.DyeingOrderDetails.Count)
        //        {
        //            string sValue = GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails);
        //            if (sValue != "")
        //            {
        //                nTempDoQty += Convert.ToDouble(sValue);
        //            }
        //        }
        //        #endregion
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Dyeing Order Details Information End


        //        //Sample Required Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        //Sample Required End

        //        // Print & Embroidery Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy > 0) ? "OK" : "NO" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Print & Embroidery End


        //        // Accessories Details Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy != 0) ? "OK" : "N/A" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Accessories Details End

        //        // PO Sheet  And Qty Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // PO Sheet And Qty End
        //        nCount++;
        //        #endregion

        //        #region weight
        //        _oPdfPCell = new PdfPCell(new Phrase("weight", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.Weight, _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricName, _oFontStyle)); // Image Print
        //        _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Lab Dip Details Information End

        //        // Dyeing Order Details Information Start

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        #region Calculate DO Total Qty
        //        if (nCount < oItem.DyeingOrderDetails.Count)
        //        {
        //            string sValue = GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails);
        //            if (sValue != "")
        //            {
        //                nTempDoQty += Convert.ToDouble(sValue);
        //            }
        //        }
        //        #endregion
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Dyeing Order Details Information End


        //        //Sample Required Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        //Sample Required End

        //        // Print & Embroidery Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy > 0) ? "OK" : "NO" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Print & Embroidery End


        //        // Accessories Details Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy != 0) ? "OK" : "N/A" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Accessories Details End

        //        // PO Sheet  And Qty Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // PO Sheet And Qty End
        //        nCount++;

        //        #endregion

        //        #region Yarn Required

        //        _oPdfPCell = new PdfPCell(new Phrase("Yarn Required", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.YarnRequired), _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Lab Dip Details Information End

        //        // Dyeing Order Details Information Start

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        #region Calculate DO Total Qty
        //        if (nCount < oItem.DyeingOrderDetails.Count)
        //        {
        //            string sValue = GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails);
        //            if (sValue != "")
        //            {
        //                nTempDoQty += Convert.ToDouble(sValue);
        //            }
        //        }
        //        #endregion
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Dyeing Order Details Information End


        //        //Sample Required Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        //Sample Required End

        //        // Print & Embroidery Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy > 0) ? "OK" : "NO" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Print & Embroidery End


        //        // Accessories Details Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy != 0) ? "OK" : "N/A" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Accessories Details End

        //        // PO Sheet  And Qty Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // PO Sheet And Qty End
        //        nCount++;
        //        #endregion

        //        #region Size Range

        //        _oPdfPCell = new PdfPCell(new Phrase("Size Range", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.SizeRange, _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.GG+","+oItem.Count, _oFontStyle)); // Image Print
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Lab Dip Details Information End

        //        // Dyeing Order Details Information Start

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //        _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
               
        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempDoQty), _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        nTempDoQty = 0;//Reset total Qty
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Dyeing Order Details Information End


        //        //Sample Required Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        //Sample Required End

        //        // Print & Embroidery Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy > 0) ? "OK" : "NO" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // Print & Embroidery End


        //        // Accessories Details Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy != 0) ? "OK" : "N/A" : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        // Accessories Details End

        //        // PO Sheet  And Qty Start
        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        // PO Sheet And Qty End
        //        nCount++;
        //        #endregion

        //        #region Extra Row
        //        while (nCount != nMaxRowSize)
        //        {
        //            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); // Image Print
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            // Lab Dip Details Information Start
        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ColorName : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.LabDipOrderDetails.Count) ? oItem.LabDipOrderDetails[nCount].ApproveShadeName : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //            // Lab Dip Details Information End

        //            // Dyeing Order Details Information Start

        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].ColorDescription : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? oItem.DyeingOrderDetails[nCount].SupplierShortName : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetDyingOrderQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.DyeingOrderDetails.Count) ? GetReceiveQty(oItem.DyeingOrderDetails[nCount].LabDipOrderDetailID, oItem.DyeingOrderDetails) : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            // Dyeing Order Details Information End

        //            //Sample Required Start
        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].SampleName : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.SampleRequirements.Count) ? oItem.SampleRequirements[nCount].Remark : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //            //Sample Required End

        //            // Print & Embroidery Start
        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? oItem.WorkOrderDetails[nCount].ProductName : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? "YES" : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.WorkOrderDetails.Count) ? (oItem.WorkOrderDetails[nCount].ApprovedBy > 0) ? "OK" : "NO" : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //            // Print & Embroidery End


        //            // Accessories Details Start
        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].ProductName : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? "YES" : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? " " : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? (oItem.ApproveBy != 0) ? "OK" : "N/A" : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.PackageBreakdowns.Count) ? oItem.PackageBreakdowns[nCount].Remak : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            // Accessories Details End

        //            // PO Sheet  And Qty Start
        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? oItem.OrderRecapDetails[nCount].ColorName : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase((nCount < oItem.OrderRecapDetails.Count) ? Global.MillionFormat(oItem.OrderRecapDetails[nCount].Quantity) : "", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //            // PO Sheet And Qty End
        //            nCount++;
        //        }
        //        #endregion 

                

        //        _oPdfPCell = new PdfPCell(oPdfPTable);
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0;_oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //        _oPdfPTable.CompleteRow();

        //        #region Extra Space
        //        if ((nSLCount % 2) != 0 && nSLCount <_oOrderRecaps.Count )
        //        {
        //            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 15; _oPdfPTable.AddCell(_oPdfPCell);
        //            _oPdfPTable.CompleteRow();

        //            _oPdfPCell = new PdfPCell(GetHeading());
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            _oPdfPTable.AddCell(_oPdfPCell);
        //            _oPdfPTable.CompleteRow();
        //        }
        //        #endregion
               

        //    }
        //    #endregion



        //}

        //private int GetMaxRowSize(OrderRecap oOrderRecap)
        //{
        //    int nMaxRowSize = 15;  // Fixed Order Recap Infomation
        //    if (oOrderRecap.OrderRecapDetails.Count>nMaxRowSize)
        //    {
        //        nMaxRowSize = oOrderRecap.OrderRecapDetails.Count;
        //    }
        //    else if (oOrderRecap.LabDipOrderDetails.Count > nMaxRowSize)
        //    {
        //        nMaxRowSize = oOrderRecap.LabDipOrderDetails.Count;
        //    }
        //    else if (oOrderRecap.DyeingOrderDetails.Count > nMaxRowSize)
        //    {
        //        nMaxRowSize = oOrderRecap.DyeingOrderDetails.Count;
        //    }
        //    else if (oOrderRecap.WorkOrderDetails.Count > nMaxRowSize)
        //    {
        //        nMaxRowSize = oOrderRecap.WorkOrderDetails.Count;
        //    }
        //    else if (oOrderRecap.PackageBreakdowns.Count > nMaxRowSize)
        //    {
        //        nMaxRowSize = oOrderRecap.PackageBreakdowns.Count;
        //    }
        //    else if (oOrderRecap.SampleRequirements.Count > nMaxRowSize)
        //    {
        //        nMaxRowSize = oOrderRecap.SampleRequirements.Count;
        //    }

        //    return nMaxRowSize;
        //}
        //private string GetDyingOrderQty(int nLabDipDetailID, List<DyeingOrderDetail> oDyeingOrderDetails)
        //{
        //    double nTotalQty = 0;
        //    foreach (DyeingOrderDetail oItem in oDyeingOrderDetails)
        //    {
        //        if (oItem.LabDipOrderDetailID == nLabDipDetailID)
        //        {
        //            nTotalQty += oItem.Qty;
        //        }

        //    }
        //    if (nTotalQty > 0)
        //    {
        //        return Global.MillionFormat(nTotalQty);
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        //private string GetReceiveQty (int nLabDipDetailID, List<DyeingOrderDetail> oDyeingOrderDetails)
        //{
        //    double nTotalQty = 0;
        //    foreach (DyeingOrderDetail oItem in oDyeingOrderDetails)
        //    {
        //        if (oItem.LabDipOrderDetailID == nLabDipDetailID)
        //        {
        //            nTotalQty += oItem.ReceiveQty;
        //        }

        //    }
        //    if (nTotalQty > 0)
        //    {
        //        return Global.MillionFormat(nTotalQty);
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
        private PdfPTable GetHeading()
        {
            PdfPTable oPdfPTable = new PdfPTable(23);
            oPdfPTable.SetWidths(new float[] { 
                                                26f, // SL No
                                                60f, // Recap Captions
                                                100f, // Recap Values
                                                90f, // Technical Sheet Image
                                                70f, // LabDip Color Name
                                                55f, // LabDip Supplier Name
                                                30f, // Approve shade
                                                70f, // DO Color Name
                                                55f, // Do Supplier Name
                                                40f, // Dyeing Order Qty
                                                40f, // In House Qty
                                                60f, // Smple Rqmnt Detls
                                                40f, // smple rqmnt Remarks
                                                70f, // Print & Emboidery Details
                                                40f, // Print & Emboidery CustInfo
                                                40f, // Print & Emboidery Approval
                                                70f, // Accesories Details 
                                                40f, // Accesories Cust Info 
                                                70f, // Accesories Buyer Approval 
                                                60f, // Accesories FTy Approval 
                                                45f, // Accesories Comments 
                                                40f, // PO Sheet
                                                40f  //  Quantity
            
                                         });

            #region Row1
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL #", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order Details", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Sketch/PO SHEET", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Lab Dip Order Details", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Dyeing Order Details", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Sample Requirement", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Print & Embroidery", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Accessories Details", _oFontStyle));
            _oPdfPCell.Colspan = 5; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PO Sheet", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Quantity", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            #endregion

            #region  Row2
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            //Lab Dip Details start
            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Supplier", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shade", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //Lab Dip Details End

            //Dyeing Order Details start
            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Supplier", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("DO Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("InHouse", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //Dyeing Order Details End

            //Sample Requirement start
            _oPdfPCell = new PdfPCell(new Phrase("Details", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //Sample Requirement End

            //Print & Emboidery start
            _oPdfPCell = new PdfPCell(new Phrase("Details", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Cust. Info", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Approval", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //Print & Emboidery End

            //Accesories Details start
            _oPdfPCell = new PdfPCell(new Phrase("Details", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Cust. Info", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer Approval", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("FTY Approval", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Comments", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //Accesories Details End

            oPdfPTable.CompleteRow();

        
            #endregion
            return oPdfPTable;
        }

        #endregion
    }
    

}
