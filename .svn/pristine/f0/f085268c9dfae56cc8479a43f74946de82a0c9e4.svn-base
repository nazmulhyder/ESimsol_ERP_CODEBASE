using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using iTextSharp.text.pdf;
using System.Net.Mail;
using System.Net.Mime;
using System.Dynamic;
using OfficeOpenXml.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class KnitDyeingProgramController : Controller
    {
        #region Declaration

        KnitDyeingProgram _oKnitDyeingProgram = new KnitDyeingProgram();
        List<KnitDyeingProgram> _oKnitDyeingPrograms = new List<KnitDyeingProgram>();
        #endregion

        #region Functions

        #endregion

        #region Actions
        public ActionResult ViewKnitDyeingPrograms(int ProgramType, int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.KnitDyeingProgram).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            string sSQL = "SELECT * FROM View_KnitDyeingProgram  AS HH WHERE ISNULL(HH.ApprovedBy,0)=0 AND HH.ProgramType = " + ProgramType + "  ORDER BY KnitDyeingProgramID ASC";
            _oKnitDyeingPrograms = new List<KnitDyeingProgram>();
            _oKnitDyeingPrograms = KnitDyeingProgram.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            ViewBag.ProgramType = ProgramType;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.DyedTypes = EnumObject.jGets(typeof(EnumDyedType));
            ViewBag.KnitDyeingProgramStatus = EnumObject.jGets(typeof(EnumKnitDyeingProgramStatus));
            return View(_oKnitDyeingPrograms);
        }
        public ActionResult ViewKnitDyeingProgram(int id, int buid, bool bIsCopy)
        {
            _oKnitDyeingProgram = new KnitDyeingProgram();
            if (id > 0)
            {
                _oKnitDyeingProgram = _oKnitDyeingProgram.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oKnitDyeingProgram.KnitDyeingYarnRequisitions = KnitDyeingYarnRequisition.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oKnitDyeingProgram.KnitDyeingProgramDetails = KnitDyeingProgramDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);                
                foreach (KnitDyeingProgramDetail oItem in _oKnitDyeingProgram.KnitDyeingProgramDetails)
                {
                    oItem.KnitDyeingYarnConsumptions = KnitDyeingYarnConsumption.Gets(oItem.KnitDyeingProgramDetailID, (int)Session[SessionInfo.currentUserID]);
                }

                if (bIsCopy)
                {
                    _oKnitDyeingProgram.KnitDyeingProgramID = 0;
                    _oKnitDyeingProgram.RefNo = "";
                    _oKnitDyeingProgram.KnitDyeingProgramStatus = EnumKnitDyeingProgramStatus.Initilize;
                    _oKnitDyeingProgram.KnitDyeingProgramStatusInt = (int)EnumKnitDyeingProgramStatus.Initilize;
                    _oKnitDyeingProgram.ApprovedBy = 0;
                    _oKnitDyeingProgram.ApprovedByName = "";

                    foreach (KnitDyeingProgramDetail oItem in _oKnitDyeingProgram.KnitDyeingProgramDetails)
                    {
                        oItem.KnitDyeingProgramDetailID = 0;
                        oItem.KnitDyeingProgramID = 0;
                        oItem.GracePercent = 0;
                        oItem.ReqFabricQty = oItem.ReqFinishFabricQty;

                        foreach (KnitDyeingYarnConsumption oKnitDyeingYarnConsumption in oItem.KnitDyeingYarnConsumptions)
                        {
                            oKnitDyeingYarnConsumption.KnitDyeingYarnConsumptionID = 0;
                            oKnitDyeingYarnConsumption.KnitDyeingProgramDetailID = 0;
                            oKnitDyeingYarnConsumption.GracePercent = 0;
                            oKnitDyeingYarnConsumption.ReqQty = oKnitDyeingYarnConsumption.FinishReqQty;
                        }
                    }

                    foreach (KnitDyeingYarnRequisition oItem in _oKnitDyeingProgram.KnitDyeingYarnRequisitions)
                    {
                        oItem.KnitDyeingYarnRequisitionID = 0;
                        oItem.KnitDyeingProgramID = 0;
                        oItem.GracePercent = 0;
                        oItem.RequiredQty = oItem.FinishRequiredQty;
                    }
                }
            }
            ViewBag.MUnits = MeasurementUnit.Gets(EnumUniteType.Count, (int)Session[SessionInfo.currentUserID]);
            ViewBag.MWeightUnits = MeasurementUnit.Gets(EnumUniteType.Weight, (int)Session[SessionInfo.currentUserID]);
            ViewBag.DyedTypes = EnumObject.jGets(typeof(EnumDyedType));
            ViewBag.RefTypes = EnumObject.jGets(typeof(EnumKnitDyeingProgramRefType));
            return View(_oKnitDyeingProgram);
        }
        public ActionResult PrintPreview(int id, bool bIsWithGrace)
        {
            KnitDyeingProgram _oKnitDyeingProgram = new KnitDyeingProgram();
            List<KnitDyeingProgramAttachment>oKnitDyeingProgramAttachments=new List<KnitDyeingProgramAttachment>();
            string sSQL="SELECT TOP 1 * FROM KnitDyeingProgramAttachment WHERE KnitDyeingProgramID="+id;
            oKnitDyeingProgramAttachments=KnitDyeingProgramAttachment.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            _oKnitDyeingProgram = _oKnitDyeingProgram.Get(id, (int)Session[SessionInfo.currentUserID]);
            List<KnitDyeingYarnRequisition> oKnitDyeingYarnRequisitions = new List<KnitDyeingYarnRequisition>();
            oKnitDyeingYarnRequisitions = KnitDyeingYarnRequisition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            List<KnitDyeingProgramDetail> oKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
            oKnitDyeingProgramDetails = KnitDyeingProgramDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oKnitDyeingProgram.KnitDyeingYarnRequisitions = oKnitDyeingYarnRequisitions;
            _oKnitDyeingProgram.KnitDyeingProgramDetails = oKnitDyeingProgramDetails;
            _oKnitDyeingProgram.StyleImage = GetStyleImage(_oKnitDyeingProgram.TechnicalSheetID);
            if (oKnitDyeingProgramAttachments.Count>0)
            {
                _oKnitDyeingProgram.CareImage = GetCareImage(oKnitDyeingProgramAttachments);
            }
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            rptKnitDyeingProgram oReport = new rptKnitDyeingProgram();
            byte[] abytes = oReport.PrepareReport(_oKnitDyeingProgram, oCompany, bIsWithGrace);
            return File(abytes, "application/pdf");
        }
        public int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }
        public void XLPreview(int id)
        {
            KnitDyeingProgram _oKnitDyeingProgram = new KnitDyeingProgram();

            _oKnitDyeingProgram = _oKnitDyeingProgram.Get(id, (int)Session[SessionInfo.currentUserID]);
            List<KnitDyeingYarnRequisition> oKnitDyeingYarnRequisitions = new List<KnitDyeingYarnRequisition>();
            oKnitDyeingYarnRequisitions = KnitDyeingYarnRequisition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            List<KnitDyeingProgramDetail> oKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
            oKnitDyeingProgramDetails = KnitDyeingProgramDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oKnitDyeingProgram.KnitDyeingYarnRequisitions = oKnitDyeingYarnRequisitions;
            _oKnitDyeingProgram.KnitDyeingProgramDetails = oKnitDyeingProgramDetails;
            _oKnitDyeingProgram.StyleImage = GetStyleImage(_oKnitDyeingProgram.TechnicalSheetID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            int rowIndex = 2;
            int nMaxColumn = 0;
            int colIndex = 1;
            ExcelRange cell;
            Border border;
            ExcelFill fill;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Sales & Marketing");
                sheet.Name = "Sales & Marketing";

                #region Declare Column
                colIndex = 0;
                sheet.Column(++colIndex).Width = 8;  //Color
                sheet.Column(++colIndex).Width = 8;  //Garments Qty
                sheet.Column(++colIndex).Width = 10; //Fabric Type
                sheet.Column(++colIndex).Width = 10; //Composition
                sheet.Column(++colIndex).Width = 12; //ConsumptionPerDz
                sheet.Column(++colIndex).Width = 10; //Finish Dia
                sheet.Column(++colIndex).Width = 10; //Penton No
                sheet.Column(++colIndex).Width = 10; //Recipe
                sheet.Column(++colIndex).Width = 10; //Finish Fabric Qty
                sheet.Column(++colIndex).Width = 10; //Grace Percen
                sheet.Column(++colIndex).Width = 10; //Gray Qty(With Grace)
                sheet.Column(++colIndex).Width = 12; //Remarks
                nMaxColumn = colIndex;
                #endregion
                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex++, nMaxColumn]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Value = oCompany.Address + " "; cell.Merge = true; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 8;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                rowIndex++;
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Value = oCompany.FactoryAddress + " "; cell.Merge = true; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 8;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                rowIndex++;
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Value = oCompany.Phone + " " + oCompany.Fax + " " + oCompany.Email + " " + oCompany.WebAddress; cell.Merge = true; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 8;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                rowIndex++;
                if (_oKnitDyeingProgram.ProgramType == EnumProgramType.Sample)
                {
                    cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = _oKnitDyeingProgram.StyleNo + " - Season " + _oKnitDyeingProgram.SessionName + " - Sample fabric Program"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;

                }
                else
                {
                    cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = _oKnitDyeingProgram.StyleNo + " - Season " + _oKnitDyeingProgram.SessionName + " - Bulk fabric Program"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;
                }
                rowIndex += 2;

                cell = sheet.Cells[rowIndex, 1, rowIndex, 2]; cell.Merge = true; cell.Value = "Date:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[rowIndex, 3, rowIndex, 7]; cell.Merge = true; cell.Value = _oKnitDyeingProgram.IssueDateInString; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                if (_oKnitDyeingProgram.StyleImage != null)
                {
                    cell = sheet.Cells[rowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    sheet.Row(rowIndex).Height = 40;

                    ExcelPicture excelImage = null;
                    excelImage = sheet.Drawings.AddPicture("abc", _oKnitDyeingProgram.StyleImage);
                    excelImage.From.Column = 9;
                    excelImage.From.Row = rowIndex - 1;
                    excelImage.SetSize(100, 90);
                    excelImage.From.ColumnOff = this.Pixel2MTU(2);
                    excelImage.From.RowOff = this.Pixel2MTU(2);
                }
                rowIndex++;

                cell = sheet.Cells[rowIndex, 1, rowIndex, 2]; cell.Merge = true; cell.Value = "Buyer:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[rowIndex, 3, rowIndex, 7]; cell.Merge = true; cell.Value = _oKnitDyeingProgram.BuyerName; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex++;


                cell = sheet.Cells[rowIndex, 1, rowIndex, 2]; cell.Merge = true; cell.Value = "Style:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[rowIndex, 3, rowIndex, 7]; cell.Merge = true; cell.Value = _oKnitDyeingProgram.StyleNo; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex++;


                cell = sheet.Cells[rowIndex, 1, rowIndex, 2]; cell.Merge = true; cell.Value = "Fabrication:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[rowIndex, 3, rowIndex, 8]; cell.Merge = true; cell.Value = _oKnitDyeingProgram.FabricName; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[rowIndex, 9, rowIndex, 11]; cell.Merge = true; cell.Value = "GSM & Shrinkage Tolerance:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[rowIndex, 12]; cell.Value = _oKnitDyeingProgram.SrinkageTollarance; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                rowIndex++;
                cell = sheet.Cells[rowIndex, 1, rowIndex, 2]; cell.Merge = true; cell.Value = "TTL OrderQty:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[rowIndex, 3, rowIndex, 5]; cell.Merge = true; cell.Value = _oKnitDyeingProgram.OrderQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0;(#,##0)";

                cell = sheet.Cells[rowIndex, 6, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Fabric should be pass all kind of test parameter"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex++;
                var data = _oKnitDyeingProgram.KnitDyeingProgramDetails.GroupBy(x => new { OrderRecapID = x.RefObjectID, OrderRecapNo = x.JOBPAMPONo }, (key, grp) => new
                {
                    OrderRecapID = key.OrderRecapID,
                    OrderRecapNo = key.OrderRecapNo,
                    Results = grp.OrderBy(x => x.ColorID).ToList(),
                });
                string OrderRecapNos = string.Join(",", data.Select(x => x.OrderRecapNo));
                cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Order No: " + OrderRecapNos; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                #endregion
                rowIndex++;
                #region Requisition Header
                cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Yarn Requisition"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                cell = sheet.Cells[rowIndex, 1, rowIndex, 3]; cell.Merge = true; cell.Value = "Yarn Comp."; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4, rowIndex, 5]; cell.Merge = true; cell.Value = "Ratio"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 6, rowIndex, 7]; cell.Merge = true; cell.Value = "Req. Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 8]; cell.Value = "Grace(%)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 9]; cell.Value = "Req. Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 10, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Fabric Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion
                #region Requisition Body
                int nCountYarn = 0;
                double nTotalReqQty = 0; double nTotalFinishQty = 0;
                foreach (KnitDyeingYarnRequisition oItem in oKnitDyeingYarnRequisitions)
                {
                    nCountYarn++;
                    cell = sheet.Cells[rowIndex, 1, rowIndex, 3]; cell.Merge = true; cell.Value = oItem.YarnName;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4, rowIndex, 5]; cell.Merge = true; cell.Value = Global.MillionFormat(oItem.UsagesParcent) + " % ";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6, rowIndex, 7]; cell.Merge = true; cell.Value = Global.MillionFormat(oItem.FinishRequiredQty, 0) + " Kgs ";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = Global.MillionFormat(oItem.GracePercent, 0);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = Global.MillionFormat(oItem.RequiredQty, 0) + " Kgs ";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = oItem.FabricTypeName;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nTotalFinishQty += oItem.FinishRequiredQty;
                    nTotalReqQty += oItem.RequiredQty;
                    rowIndex++;
                }
                #endregion
                #region Total Yarn Requisition
                cell = sheet.Cells[rowIndex, 1, rowIndex, 3]; cell.Merge = true; cell.Value = "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4, rowIndex, 5]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 6, rowIndex, 7]; cell.Merge = true; cell.Value = nTotalFinishQty + " Kgs "; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 8]; cell.Value = nTotalReqQty + " Kgs "; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 9]; cell.Value = nTotalReqQty + " Kgs "; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 10, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                #endregion
                rowIndex += 2;
                #region Dyeing Program Header
                cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Fabrics Knitting & Dyeing Program"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Gmt. Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "F.Type & GSM"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Composition"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Fab. Consumption"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Finish Dia"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Pantone No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Shade Receipe"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Req. fab Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Grace(%)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Req. fab Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion
                #region Dyeing Program Data

                double nTotalGmtQty = 0, nTotalRqFbQty = 0, nTotalFinishFbQty = 0;
                foreach (var oData in data)
                {
                    int n = 0;
                    int nColorID = 0;
                    int rowSpan = 0;
                    foreach (var oItem in oData.Results)
                    {
                        n++;
                        if (oItem.ColorID != nColorID)
                        {
                            rowSpan = NumOfRowSpan(oData.Results, oItem.ColorID);
                            rowSpan = (rowSpan > 0 ? rowSpan - 1 : 0);
                            cell = sheet.Cells[rowIndex, 1, rowIndex + rowSpan, 1]; cell.Merge = true; cell.Value = oItem.ColorName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 2, rowIndex + rowSpan, 2]; cell.Merge = true; cell.Value = oItem.GarmentsQty; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0;(#,##0)"; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nTotalGmtQty += oItem.GarmentsQty;
                            n = 1;
                            nColorID = oItem.ColorID;

                        }
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.FabricTypeName + " " + oItem.GSMName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.CompositionName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.MillionFormat(oItem.ConsumptionPerDzn) + " kgs/dzn ";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.FinishDiaName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.PantoneNo;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.ShadeRecipe;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.MillionFormat(oItem.ReqFinishFabricQty, 0) + " Kgs";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.MillionFormat(oItem.GracePercent, 0);
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.MillionFormat(oItem.ReqFabricQty, 0) + " Kgs";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        nTotalRqFbQty += oItem.ReqFabricQty;
                        nTotalFinishFbQty = nTotalFinishFbQty + oItem.ReqFinishFabricQty;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.Remarks;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }
                }
                #endregion
                #region Total Fabrics & Dyeing Program
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nTotalGmtQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nTotalFinishFbQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nTotalRqFbQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                #endregion
                cell = sheet.Cells[1, 1, rowIndex, nMaxColumn + 3];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=BuyerAndMonthWise.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();

            }

        }

        #region CountNumberOfRowMerged
        public int NumOfRowSpan(List<KnitDyeingProgramDetail> _oKnitDyeingProgramDetail, int nColorID)
        {
            int Count = 0;
            foreach (KnitDyeingProgramDetail oItem in _oKnitDyeingProgramDetail)
            {
                if (oItem.ColorID == nColorID)
                {
                    Count++;
                }
            }
            return Count;
        }
        #endregion
        public ActionResult PrintPreviewLog(int id, bool bIsWithGrace)//LogID
        {
            KnitDyeingProgram _oKnitDyeingProgram = new KnitDyeingProgram();

            _oKnitDyeingProgram = _oKnitDyeingProgram.GetLog(id, (int)Session[SessionInfo.currentUserID]);
            List<KnitDyeingYarnRequisition> oKnitDyeingYarnRequisitions = new List<KnitDyeingYarnRequisition>();
            oKnitDyeingYarnRequisitions = KnitDyeingYarnRequisition.GetsLog(id, (int)Session[SessionInfo.currentUserID]);
            List<KnitDyeingProgramDetail> oKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
            oKnitDyeingProgramDetails = KnitDyeingProgramDetail.GetsLog(id, (int)Session[SessionInfo.currentUserID]);
            _oKnitDyeingProgram.KnitDyeingYarnRequisitions = oKnitDyeingYarnRequisitions;
            _oKnitDyeingProgram.KnitDyeingProgramDetails = oKnitDyeingProgramDetails;
            _oKnitDyeingProgram.StyleImage = GetStyleImage(_oKnitDyeingProgram.TechnicalSheetID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            rptKnitDyeingProgram oReport = new rptKnitDyeingProgram();
            byte[] abytes = oReport.PrepareReport(_oKnitDyeingProgram, oCompany, bIsWithGrace);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintList(string tvs, int buid, string ids)
        {
            List<KnitDyeingProgram> oKnitDyeingPrograms = new List<KnitDyeingProgram>();
            KnitDyeingProgram _oKnitDyeingProgram = new KnitDyeingProgram();
            oKnitDyeingPrograms = KnitDyeingProgram.Gets("SELECT * FROM View_KnitDyeingProgram  AS HH WHERE HH.KnitDyeingProgramID IN (" + ids + ") ORDER BY KnitDyeingProgramID ASC", (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);

            rptKnitDyeingPrograms oReport = new rptKnitDyeingPrograms();
            byte[] abytes = oReport.PrepareReport(oKnitDyeingPrograms, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public System.Drawing.Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        public Image GetStyleImage(int id)
        {
            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            oTechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetImage.LargeImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetImage.LargeImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(Server.MapPath("~/Content/") + "styleimage.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public Image GetCareImage(List<KnitDyeingProgramAttachment>oKnitDyeingProgramAttachments)
        {
            if (oKnitDyeingProgramAttachments[0].AttachFile != null)
            {
                MemoryStream m = new MemoryStream(oKnitDyeingProgramAttachments[0].AttachFile);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(Server.MapPath("~/Content/") + "Careimage.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult Save(KnitDyeingProgram oKnitDyeingProgram)
        {
            _oKnitDyeingProgram = new KnitDyeingProgram();
            try
            {
                _oKnitDyeingProgram = oKnitDyeingProgram;
                _oKnitDyeingProgram = _oKnitDyeingProgram.Save((int)Session[SessionInfo.currentUserID]);
                _oKnitDyeingProgram.KnitDyeingProgramDetails = KnitDyeingProgramDetail.Gets(_oKnitDyeingProgram.KnitDyeingProgramID, (int)Session[SessionInfo.currentUserID]);
                foreach (KnitDyeingProgramDetail oItem in _oKnitDyeingProgram.KnitDyeingProgramDetails)
                {
                    oItem.KnitDyeingYarnConsumptions = KnitDyeingYarnConsumption.Gets(oItem.KnitDyeingProgramDetailID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oKnitDyeingProgram = new KnitDyeingProgram();
                _oKnitDyeingProgram.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingProgram);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AcceptRevise(KnitDyeingProgram oKnitDyeingProgram)
        {
            _oKnitDyeingProgram = new KnitDyeingProgram();
            try
            {
                _oKnitDyeingProgram = oKnitDyeingProgram;
                _oKnitDyeingProgram = _oKnitDyeingProgram.AcceptRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnitDyeingProgram = new KnitDyeingProgram();
                _oKnitDyeingProgram.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingProgram);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AdvanceSearch(KnitDyeingProgram oKnitDyeingProgram)
        {
            List<KnitDyeingProgram> oKnitDyeingPrograms = new List<KnitDyeingProgram>();
            string sSQL = GetSQL(oKnitDyeingProgram);
            oKnitDyeingPrograms = KnitDyeingProgram.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnitDyeingPrograms);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL(KnitDyeingProgram oKnitDyeingProgram)
        {
            int nCount = 0;
            string sOrderRecapIDs = oKnitDyeingProgram.ErrorMessage.Split('~')[nCount++];
            int nIssueDate = Convert.ToInt32(oKnitDyeingProgram.ErrorMessage.Split('~')[nCount++]);
            DateTime dIssueDateStart = Convert.ToDateTime(oKnitDyeingProgram.ErrorMessage.Split('~')[nCount++]);
            DateTime dIssueDateEnd = Convert.ToDateTime(oKnitDyeingProgram.ErrorMessage.Split('~')[nCount++]);
            int nShipmentDate = Convert.ToInt32(oKnitDyeingProgram.ErrorMessage.Split('~')[nCount++]);
            DateTime dShipmentDateStart = Convert.ToDateTime(oKnitDyeingProgram.ErrorMessage.Split('~')[nCount++]);
            DateTime dShipmentDateEnd = Convert.ToDateTime(oKnitDyeingProgram.ErrorMessage.Split('~')[nCount++]);

            string sReturn1 = "SELECT * FROM View_KnitDyeingProgram ";
            string sReturn = "";

            #region Ref No
            if (!string.IsNullOrEmpty(oKnitDyeingProgram.RefNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefNo LIKE " + "'%" + oKnitDyeingProgram.RefNo + "%'";
            }
            #endregion

            #region Style
            if (!string.IsNullOrEmpty(oKnitDyeingProgram.StyleNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " TechnicalSheetID IN (" + oKnitDyeingProgram.StyleNo + ")";
            }
            #endregion

            #region Order Recap
            if (!string.IsNullOrEmpty(sOrderRecapIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KnitDyeingProgramID IN(SELECT PP.KnitDyeingProgramID FROM KnitDyeingProgramDetail AS PP WHERE PP.OrderRecapID IN (" + sOrderRecapIDs + "))";
            }
            #endregion

            #region Dyed Type
            if (oKnitDyeingProgram.DyedType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyedType = " + (int)oKnitDyeingProgram.DyedType;
            }
            #endregion

            #region Issue Date Wise
            if (nIssueDate > 0)
            {
                if (nIssueDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate = '" + dIssueDateStart.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (IssueDate>= '" + dIssueDateStart.ToString("dd MMM yyyy") + "' AND IssueDate < '" + dIssueDateStart.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nIssueDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate > '" + dIssueDateStart.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate < '" + dIssueDateStart.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (IssueDate>= '" + dIssueDateStart.ToString("dd MMM yyyy") + "' AND IssueDate < '" + dIssueDateEnd.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nIssueDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (IssueDate< '" + dIssueDateStart.ToString("dd MMM yyyy") + "' OR IssueDate > '" + dIssueDateEnd.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

            #region Shipment Date Wise
            if (nShipmentDate > 0)
            {
                if (nShipmentDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ValidityDate>= '" + dShipmentDateStart.ToString("dd MMM yyyy") + "' AND ValidityDate < '" + dShipmentDateStart.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nShipmentDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ValidityDate != '" + dShipmentDateStart.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ValidityDate > '" + dShipmentDateStart.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ValidityDate < '" + dShipmentDateStart.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ValidityDate>= '" + dShipmentDateStart.ToString("dd MMM yyyy") + "' AND ValidityDate < '" + dShipmentDateEnd.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nShipmentDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ValidityDate< '" + dShipmentDateStart.ToString("dd MMM yyyy") + "' OR ValidityDate > '" + dShipmentDateEnd.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

            #region Status
            if (oKnitDyeingProgram.KnitDyeingProgramStatus > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KnitDyeingProgramStatus = " + (int)oKnitDyeingProgram.KnitDyeingProgramStatus;
            }
            #endregion
            sReturn = sReturn1 + sReturn;

            return sReturn;
        }

        [HttpPost]
        public JsonResult Approved(KnitDyeingProgram oKnitDyeingProgram)
        {
            _oKnitDyeingProgram = new KnitDyeingProgram();
            try
            {
                _oKnitDyeingProgram = oKnitDyeingProgram;
                _oKnitDyeingProgram = _oKnitDyeingProgram.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnitDyeingProgram = new KnitDyeingProgram();
                _oKnitDyeingProgram.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingProgram);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendToFactory(KnitDyeingProgram oKnitDyeingProgram)
        {
            _oKnitDyeingProgram = new KnitDyeingProgram();
            try
            {
                _oKnitDyeingProgram = oKnitDyeingProgram;
                _oKnitDyeingProgram = _oKnitDyeingProgram.SendToFactory((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnitDyeingProgram = new KnitDyeingProgram();
                _oKnitDyeingProgram.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingProgram);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ProductionStart(KnitDyeingProgram oKnitDyeingProgram)
        {
            _oKnitDyeingProgram = new KnitDyeingProgram();
            try
            {
                _oKnitDyeingProgram = oKnitDyeingProgram;
                _oKnitDyeingProgram = _oKnitDyeingProgram.ProductionStart((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnitDyeingProgram = new KnitDyeingProgram();
                _oKnitDyeingProgram.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingProgram);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GSMSearch(SourcingConfigHead oSourcingConfigHead)
        {
            List<SourcingConfigHead> _oSourcingConfigHeads = new List<SourcingConfigHead>();
            int Type = (int)oSourcingConfigHead.SourcingConfigHeadType;
            try
            {
                if (oSourcingConfigHead.SourcingConfigHeadName != null && oSourcingConfigHead.SourcingConfigHeadName != "")
                {
                    string sSQL = "SELECT * FROM SourcingConfigHead WHERE SourcingConfigHeadType=" + Type + " AND SourcingConfigHeadName LIKE '%" + oSourcingConfigHead.SourcingConfigHeadName + "%'";
                    _oSourcingConfigHeads = SourcingConfigHead.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                }
                else
                {
                    string sSQL = "SELECT * FROM SourcingConfigHead WHERE SourcingConfigHeadType=" + Type + "";
                    _oSourcingConfigHeads = SourcingConfigHead.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                SourcingConfigHead _oSourcingConfigHead = new SourcingConfigHead();
                _oSourcingConfigHead.ErrorMessage = ex.Message;
                _oSourcingConfigHeads.Add(_oSourcingConfigHead);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oProducts);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jSonResult = Json(_oSourcingConfigHeads, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> _oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.KnitDyeingProgram, EnumProductUsages.Regular, oProduct.ProductName, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.KnitDyeingProgram, EnumProductUsages.Regular, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                Product _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(_oProduct);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oProducts);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jSonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        public JsonResult GetRecapOrPAMs(KnitDyeingProgram oKnitDyeingProgram)
        {
            List<OrderRecap> _oOrderRecaps = new List<OrderRecap>();
            List<PAM> _oPAMs = new List<PAM>();
            StringBuilder sSQL = new StringBuilder();

            try
            {
                if (oKnitDyeingProgram.RefTypeInt == (int)EnumKnitDyeingProgramRefType.OrderRecap)
                {
                    sSQL.Append("SELECT * FROM View_OrderRecap WHERE TechnicalSheetID  = " + oKnitDyeingProgram.TechnicalSheetID + " AND ISNULL(ApproveBy,0)!=0 AND ISNULL(IsActive,0)= 1");
                    _oOrderRecaps = OrderRecap.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    sSQL.Append("SELECT * FROM View_PAM WHERE StyleID  = " + oKnitDyeingProgram.TechnicalSheetID + " AND ISNULL(ApprovedBy,0)!=0");
                    _oPAMs = PAM.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                if (oKnitDyeingProgram.RefTypeInt == (int)EnumKnitDyeingProgramRefType.OrderRecap)
                {
                    OrderRecap _oOrderRecap = new OrderRecap();
                    _oOrderRecap.ErrorMessage = ex.Message;
                    _oOrderRecaps.Add(_oOrderRecap);
                }
                else
                {
                    PAM _oPAM = new PAM();
                    _oPAM.ErrorMessage = ex.Message;
                    _oPAMs.Add(_oPAM);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = "";
            if (oKnitDyeingProgram.RefTypeInt == (int)EnumKnitDyeingProgramRefType.OrderRecap)
            {
                sjson = serializer.Serialize(_oOrderRecaps);
            }
            else
            {
                sjson = serializer.Serialize(_oPAMs);
            }
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetColors(KnitDyeingProgramDetail oKnitDyeingProgramDetail)
        {
            List<KnitDyeingProgramDetail> oColors = new List<KnitDyeingProgramDetail>();
            List<ColorCategory> oColorCategorys = new List<ColorCategory>();
            List<OrderRecapDetail> oOrderRecapDetails = new List<OrderRecapDetail>();
            List<PAMDetail> oPAMDetails = new List<PAMDetail>();
            OrderRecap _oOrderRecap = new OrderRecap();
            try
            {
                if (oKnitDyeingProgramDetail.RefTypeInt == (int)EnumKnitDyeingProgramRefType.Open)
                {
                    StringBuilder sSQL = new StringBuilder("SELECT * FROM ColorCategory");
                    if (!string.IsNullOrEmpty(oKnitDyeingProgramDetail.ColorName)) { sSQL.Append(" WHERE ColorName LIKE '%" + oKnitDyeingProgramDetail.ColorName + "%'"); }
                    sSQL.Append(" Order By ColorName");
                    oColorCategorys = ColorCategory.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);
                    foreach (ColorCategory oItem in oColorCategorys)
                    {
                        oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
                        oKnitDyeingProgramDetail.ColorID = oItem.ColorCategoryID;
                        oKnitDyeingProgramDetail.ColorName = oItem.ColorName;
                        oColors.Add(oKnitDyeingProgramDetail);
                    }
                } if (oKnitDyeingProgramDetail.RefTypeInt == (int)EnumKnitDyeingProgramRefType.OrderRecap)
                {
                    StringBuilder sSQL = new StringBuilder("SELECT ColorID, ColorName, SUM(ISNULL(Quantity,0)) as Quantity  FROM View_OrderRecapDetail WHERE OrderRecapID  =" + oKnitDyeingProgramDetail.RefObjectID);
                    if (!string.IsNullOrEmpty(oKnitDyeingProgramDetail.ColorName)) { sSQL.Append(" AND ColorName LIKE '%" + oKnitDyeingProgramDetail.ColorName + "%'"); }
                    sSQL.Append("Group by ColorID, ColorName");
                    oOrderRecapDetails = OrderRecapDetail.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);
                    foreach (OrderRecapDetail oItem in oOrderRecapDetails)
                    {
                        oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
                        oKnitDyeingProgramDetail.ColorID = oItem.ColorID;
                        oKnitDyeingProgramDetail.ColorName = oItem.ColorName;
                        oKnitDyeingProgramDetail.GarmentsQty = oItem.Quantity;
                        oColors.Add(oKnitDyeingProgramDetail);
                    }
                } if (oKnitDyeingProgramDetail.RefTypeInt == (int)EnumKnitDyeingProgramRefType.PAM)
                {
                    StringBuilder sSQL = new StringBuilder("SELECT ColorID, ColorName, SUM(ISNULL(Quantity,0)) as Quantity  FROM View_PAMDetail WHERE PAMID  =" + oKnitDyeingProgramDetail.RefObjectID);
                    if (!string.IsNullOrEmpty(oKnitDyeingProgramDetail.ColorName)) { sSQL.Append(" AND ColorName LIKE '%" + oKnitDyeingProgramDetail.ColorName + "%'"); }
                    sSQL.Append("Group by ColorID, ColorName");
                    oPAMDetails = PAMDetail.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);
                    foreach (PAMDetail oItem in oPAMDetails)
                    {
                        oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
                        oKnitDyeingProgramDetail.ColorID = oItem.ColorID;
                        oKnitDyeingProgramDetail.ColorName = oItem.ColorName;
                        oKnitDyeingProgramDetail.GarmentsQty = oItem.Quantity;
                        oColors.Add(oKnitDyeingProgramDetail);
                    }
                }

            }

            catch (Exception ex)
            {
                oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
                oKnitDyeingProgramDetail.ErrorMessage = ex.Message;
                oColors.Add(oKnitDyeingProgramDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oColors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetYarnProducts(Product oProduct)
        {
            List<Product> _oProducts = new List<Product>();
            try
            {
                string sSQL = "SELECT * FROM View_Product WHERE ProductID IN (SELECT DISTINCT YarnCountID FROM KnitDyeingYarnRequisition)";
                _oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                Product _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(_oProduct);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oProducts);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jSonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                KnitDyeingProgram oKnitDyeingProgram = new KnitDyeingProgram();
                sFeedBackMessage = oKnitDyeingProgram.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetKDPReviseHistory(KnitDyeingProgram oKnitDyeingProgram)
        {
            _oKnitDyeingPrograms = new List<KnitDyeingProgram>();
            StringBuilder sSQL = new StringBuilder();
            try
            {
                sSQL.Append("SELECT * FROM View_KnitDyeingProgramLog WHERE KnitDyeingProgramID  = " + oKnitDyeingProgram.KnitDyeingProgramID + " Order By KnitDyeingProgramLogID");
                _oKnitDyeingPrograms = KnitDyeingProgram.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {

                _oKnitDyeingProgram = new KnitDyeingProgram();
                _oKnitDyeingProgram.ErrorMessage = ex.Message;
                _oKnitDyeingPrograms.Add(_oKnitDyeingProgram);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingPrograms);

            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Search
        [HttpPost]
        public JsonResult GetsByStyleNo(KnitDyeingProgram oKnitDyeingProgram)
        {
            List<KnitDyeingProgram> oKnitDyeingPrograms = new List<KnitDyeingProgram>();
            try
            {
                string sSQL = "SELECT * FROM View_KnitDyeingProgram AS HH WHERE (ISNULL(HH.StyleNo,'')+ISNULL(HH.RefNo,'')) LIKE '%" + oKnitDyeingProgram.StyleNo + "%' ORDER BY KnitDyeingProgramID ASC";
                oKnitDyeingPrograms = KnitDyeingProgram.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oKnitDyeingPrograms = new List<KnitDyeingProgram>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnitDyeingPrograms);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsProgramDetailByStyleNo(KnitDyeingProgram oKnitDyeingProgram)
        {
            List<KnitDyeingProgramDetail> oKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
            KnitDyeingProgramDetail oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
            try
            {
                string sSQL = "SELECT   HH.KnitDyeingProgramID, HH.KDProgramNo, HH.RefObjectID, HH.RefObjectNo, HH.StyleNo, HH.BuyerName, HH.TotalGarmentsQty, HH.FabricID, HH.FabricName, HH.FinishGSMID, HH.FinishGSMName, HH.GarmentsMUnitName FROM View_KnitDyeingProgramDetail AS HH " +
                    "WHERE (HH.StyleNo + HH.RefObjectNo) LIKE '%" + oKnitDyeingProgram.StyleNo + "%' " +
                    "GROUP BY HH.KnitDyeingProgramID, HH.KDProgramNo, HH.RefObjectID, HH.RefObjectNo, HH.StyleNo, HH.BuyerName, HH.TotalGarmentsQty, HH.FabricID, HH.FabricName, HH.FinishGSMID, HH.FinishGSMName, HH.GarmentsMUnitName";
                oKnitDyeingProgramDetails = KnitDyeingProgramDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
                oKnitDyeingProgramDetail.ErrorMessage = ex.Message;
                oKnitDyeingProgramDetails.Add(oKnitDyeingProgramDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnitDyeingProgramDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsProgramDetails(KnitDyeingProgram oKnitDyeingProgram)
        {
            List<KnitDyeingProgramDetail> oKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
            KnitDyeingProgramDetail oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
            try
            {
                string sSQL = "SELECT * FROM View_KnitDyeingProgramDetail WHERE KnitDyeingProgramID IN (SELECT KnitDyeingProgramID FROM KnitDyeingProgram WHERE KnitDyeingProgramStatus = " + (int)EnumKnitDyeingProgramStatus.InProduction + " AND ProgramType = " + oKnitDyeingProgram.ProgramTypeInt + " )";
                if (!String.IsNullOrEmpty(oKnitDyeingProgram.StyleNo))
                {
                    sSQL += " AND (ISNULL(StyleNo,'') + ISNULL(RefProgramNo,'')) LIKE '%" + oKnitDyeingProgram.StyleNo + "%' ";
                }
                oKnitDyeingProgramDetails = KnitDyeingProgramDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
                oKnitDyeingProgramDetail.ErrorMessage = ex.Message;
                oKnitDyeingProgramDetails.Add(oKnitDyeingProgramDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnitDyeingProgramDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Functions
        [HttpPost]
        public JsonResult GetKnitDyeingProgramsByStyleAndNo(KnitDyeingProgram oKnitDyeingProgram)
        {
            _oKnitDyeingPrograms = new List<KnitDyeingProgram>();
            StringBuilder sSQL = new StringBuilder("SELECT HH.KnitDyeingProgramID, HH.RefNo , HH.StyleNo, HH.BuyerName, HH.RecapOrPAMNos, (SELECT SUM(MM.GarmentsQty) FROM (SELECT DISTINCT TT.RefObjectNo, TT.ColorID, TT.GarmentsQty FROM View_KnitDyeingProgramDetail AS TT WHERE TT.KnitDyeingProgramID = HH.KnitDyeingProgramID ) AS MM) AS OrderQty  FROM View_KnitDyeingProgram AS HH");
            if (!string.IsNullOrEmpty(oKnitDyeingProgram.StyleNo))
            {
                sSQL.Append(" WHERE HH.StyleNo LIKE '%" + oKnitDyeingProgram.StyleNo + "%' ");
            }
            try
            {
                _oKnitDyeingPrograms = KnitDyeingProgram.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnitDyeingProgram = new KnitDyeingProgram();
                _oKnitDyeingProgram.ErrorMessage = ex.Message;
                _oKnitDyeingPrograms.Add(_oKnitDyeingProgram);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingPrograms);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsKnitDyeingProgramDetails(KnitDyeingProgram oKnitDyeingProgram)
        {
            List<KnitDyeingProgramDetail> oKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
            try
            {
                oKnitDyeingProgramDetails = KnitDyeingProgramDetail.Gets(oKnitDyeingProgram.KnitDyeingProgramID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
                KnitDyeingProgramDetail oKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
                oKnitDyeingProgramDetail.ErrorMessage = ex.Message;
                oKnitDyeingProgramDetails.Add(oKnitDyeingProgramDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnitDyeingProgramDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CommitGrace(KnitDyeingProgram oKnitDyeingProgram)
        {
            string sFeedBackMessage = "";
            List<KnitDyeingProgramDetail> oKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
            List<KnitDyeingYarnConsumption> oKnitDyeingYarnConsumptions = new List<KnitDyeingYarnConsumption>();
            List<KnitDyeingYarnConsumption> oTempKnitDyeingYarnConsumptions = new List<KnitDyeingYarnConsumption>();
            List<KnitDyeingYarnRequisition> oKnitDyeingYarnRequisitions = new List<KnitDyeingYarnRequisition>();
            try
            {
                oKnitDyeingProgramDetails = oKnitDyeingProgram.KnitDyeingProgramDetails;
                string sSQL = "SELECT * FROM View_KnitDyeingYarnConsumption AS HH WHERE HH.KnitDyeingProgramDetailID IN (SELECT KDPD.KnitDyeingProgramDetailID FROM KnitDyeingProgramDetail AS KDPD WHERE KDPD.KnitDyeingProgramID = " + oKnitDyeingProgram.KnitDyeingProgramID.ToString() + ") ORDER BY HH.KnitDyeingProgramDetailID ASC";
                oKnitDyeingYarnConsumptions = KnitDyeingYarnConsumption.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oKnitDyeingYarnRequisitions = KnitDyeingYarnRequisition.Gets(oKnitDyeingProgram.KnitDyeingProgramID, (int)Session[SessionInfo.currentUserID]);
                foreach (KnitDyeingProgramDetail oKnitDyeingProgramDetail in oKnitDyeingProgramDetails)
                {
                    oTempKnitDyeingYarnConsumptions = new List<KnitDyeingYarnConsumption>();
                    oTempKnitDyeingYarnConsumptions = oKnitDyeingYarnConsumptions.Where(x => x.KnitDyeingProgramDetailID == oKnitDyeingProgramDetail.KnitDyeingProgramDetailID).ToList();
                    foreach (KnitDyeingYarnConsumption oKnitDyeingYarnConsumption in oTempKnitDyeingYarnConsumptions)
                    {
                        oKnitDyeingYarnConsumption.GracePercent = oKnitDyeingProgramDetail.GracePercent;
                        oKnitDyeingYarnConsumption.ReqQty = ((oKnitDyeingYarnConsumption.FinishReqQty * oKnitDyeingProgramDetail.GracePercent) / 100.00) + oKnitDyeingYarnConsumption.FinishReqQty;
                    }
                    oKnitDyeingProgramDetail.KnitDyeingYarnConsumptions = oTempKnitDyeingYarnConsumptions;
                }
                oKnitDyeingProgram = new KnitDyeingProgram();
                oKnitDyeingProgram.KnitDyeingProgramDetails = oKnitDyeingProgramDetails;
                oKnitDyeingProgram.KnitDyeingYarnRequisitions = RefreshYarnRequisition(oKnitDyeingYarnRequisitions, oKnitDyeingProgramDetails);
                sFeedBackMessage = oKnitDyeingProgram.CommitGrace((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private List<KnitDyeingYarnRequisition> RefreshYarnRequisition(List<KnitDyeingYarnRequisition> oKnitDyeingYarnRequisitions, List<KnitDyeingProgramDetail> oKnitDyeingProgramDetails)
        {
            double nTotalReqQty = 0;
            List<KnitDyeingYarnConsumption> oTempKnitDyeingYarnConsumptions = new List<KnitDyeingYarnConsumption>();
            foreach (KnitDyeingProgramDetail oItem in oKnitDyeingProgramDetails)
            {
                foreach (KnitDyeingYarnConsumption oYarnConsumption in oItem.KnitDyeingYarnConsumptions)
                {
                    nTotalReqQty = nTotalReqQty + oYarnConsumption.ReqQty;
                    oTempKnitDyeingYarnConsumptions.Add(oYarnConsumption);
                }
            }

            double nRequiredQty = 0; double nGracePercent = 0;
            foreach (KnitDyeingYarnRequisition oYarnRequisition in oKnitDyeingYarnRequisitions)
            {
                nRequiredQty = 0;
                nRequiredQty = GetYarnAndFabricTypeWiseRequiredQty(oTempKnitDyeingYarnConsumptions, oYarnRequisition.YarnCountID, oYarnRequisition.FabricTypeID);
                if (oYarnRequisition.FinishRequiredQty != 0)
                {
                    nGracePercent = ((nRequiredQty - oYarnRequisition.FinishRequiredQty) * 100) / oYarnRequisition.FinishRequiredQty;
                    if (nGracePercent < 0)
                    {
                        nGracePercent = 0;
                    }
                }
                oYarnRequisition.RequiredQty = nRequiredQty;
                oYarnRequisition.GracePercent = nGracePercent;
                oYarnRequisition.UsagesParcent = ((100 / nTotalReqQty) * nRequiredQty);
            }
            return oKnitDyeingYarnRequisitions;
        }

        private double GetYarnAndFabricTypeWiseRequiredQty(List<KnitDyeingYarnConsumption> oTempKnitDyeingYarnConsumptions, int nYarnID, int nFabricTypeID)
        {
            double nRequiredQty = 0;
            foreach (KnitDyeingYarnConsumption oItem in oTempKnitDyeingYarnConsumptions)
            {
                if (oItem.YarnID == nYarnID && oItem.FabricTypeID == nFabricTypeID)
                {
                    nRequiredQty = nRequiredQty + oItem.ReqQty;
                }
            }
            return nRequiredQty;
        }
        #endregion
        #region Yarn Requisition XL
        public void ExportToExcelYarnRequisition(string sKnitDyeingProgramIDs)
        {
            List<KnitDyeingProgram> oKnitDyeingPrograms = new List<KnitDyeingProgram>();
            List<KnitDyeingYarnRequisition> oKnitDyeingYarnRequisitions = new List<KnitDyeingYarnRequisition>();
            Company oCompany = new Company();
            try
            {
                string sSQL = "SELECT * FROM VIEW_KnitDyeingProgram WHERE KnitDyeingProgramID IN (" + sKnitDyeingProgramIDs + ")";
                oKnitDyeingPrograms = KnitDyeingProgram.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                sSQL = "SELECT * FROM VIEW_KnitDyeingYarnRequisition WHERE KnitDyeingProgramID IN (" + sKnitDyeingProgramIDs + ")";
                oKnitDyeingYarnRequisitions = KnitDyeingYarnRequisition.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                if (oKnitDyeingPrograms.Count <= 0)
                {
                    throw new Exception("Invalid Searching Criteria!");
                }
                this.PrintYarnRequisitionXL(oKnitDyeingPrograms, oKnitDyeingYarnRequisitions, oCompany);
            }
            catch (Exception ex)
            {
                #region Errormessage
                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Yarn Requisition");
                    sheet.Name = "Yarn Requisition";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = ex.Message; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Yarn_Requisition.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }

        private void PrintYarnRequisitionXL(List<KnitDyeingProgram> oKnitDyeingPrograms, List<KnitDyeingYarnRequisition> oKnitDyeingYarnRequisitions, Company oCompany)
        {
            int rowIndex = 2;
            int nMaxColumn = 0;
            int colIndex = 1;
            ExcelRange cell;
            Border border;
            ExcelFill fill;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Yarn Requisition");
                sheet.Name = "Yarn Requisition";
                sheet.View.FreezePanes(6, 1);
                #region Declare Column
                colIndex = 0;
                sheet.Column(++colIndex).Width = 10;//SL
                sheet.Column(++colIndex).Width = 50;//Yarn name
                sheet.Column(++colIndex).Width = 20;//Ratio
                sheet.Column(++colIndex).Width = 20;//Req Qty
                sheet.Column(++colIndex).Width = 35;//Fabric Type
                nMaxColumn = colIndex;
                #endregion
                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex++, nMaxColumn]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Value = oCompany.Address + " "; cell.Merge = true; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 8;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Value = ""; cell.Merge = true; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 8;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Value = "Yarn Requisition "; cell.Merge = true; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true; cell.Style.Font.Size = 12;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                rowIndex++;
                #endregion

                #region Column Header
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Yarn name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Ratio(%)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Req Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Fabric Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion

                #region Report Data
                int nCount = 0;
                int nStartRow = rowIndex;
                double SubRatio = 0, subReqQty = 0, GrandRatio = 0, GrandReqQty = 0;
                foreach (KnitDyeingProgram oItem in oKnitDyeingPrograms)
                {
                    colIndex = 0;
                    nCount = 0;
                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, 5]; cell.Merge = true; cell.Value = "Program No: " + oItem.RefNo + "; Style NO: " + oItem.StyleNo + "; Buyer Name: " + oItem.BuyerName + "; Pcs Qty: " + oItem.OrderQty + "; MarChendiser: " + oItem.MerchandiserName;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true; cell.Style.Font.Size = 10; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    var oResults = oKnitDyeingYarnRequisitions.Where(x => x.KnitDyeingProgramID == oItem.KnitDyeingProgramID).ToList();
                    rowIndex++;
                    SubRatio = 0; subReqQty = 0;

                    foreach (KnitDyeingYarnRequisition oItemRequisition in oResults)
                    {
                        nCount++;
                        colIndex = 0;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nCount.ToString();
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItemRequisition.YarnName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItemRequisition.UsagesParcent;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        SubRatio += oItemRequisition.UsagesParcent;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItemRequisition.RequiredQty;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        subReqQty += oItemRequisition.RequiredQty;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItemRequisition.FabricTypeName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }
                    #region SubTotal
                    colIndex = 0;
                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, 2]; cell.Merge = true; cell.Value = "Sub Total:";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = SubRatio; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    GrandRatio += SubRatio;
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = subReqQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    GrandReqQty += subReqQty;
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    #endregion
                    rowIndex++;
                }
                #region GrandTotal
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, 2]; cell.Merge = true; cell.Value = "Grand Total:";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                colIndex = 2;
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = GrandReqQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                #endregion
                #endregion
                cell = sheet.Cells[1, 1, rowIndex + 3, nMaxColumn + 3];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Yarn_Requisition.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion
    }
}
