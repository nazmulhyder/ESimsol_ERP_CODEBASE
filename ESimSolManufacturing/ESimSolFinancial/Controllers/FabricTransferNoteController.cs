using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;
using System.Xml.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;

namespace ESimSolFinancial.Controllers
{
    public class FabricTransferNoteController : Controller
    {
        #region Declaration
        FabricTransferNote _oFTN = new FabricTransferNote();
        List<FabricTransferNote> _oFTNs = new List<FabricTransferNote>();
        #endregion

        public ActionResult View_FabricTransferNotes(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFTNs = new List<FabricTransferNote>();
            _oFTNs = FabricTransferNote.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFTNs);
        }
        public ActionResult View_FabricTransferNote(string sFTNID, string sBtnID, string sMsg)
        {
            List<FabricTransferPackingList> oFTPLs = new List<FabricTransferPackingList>();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            List<WorkingUnit> oWorkingUnitsIssue = new List<WorkingUnit>();
            string sSQL = "";
            int nFTNID = Convert.ToInt32(sFTNID);
            if (nFTNID > 0)
            {
                _oFTN = _oFTN.Get(nFTNID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = "SELECT * FROM View_FabricTransferPackingList WHERE FTNID = " + nFTNID;
                _oFTN.FTPLs = FabricTransferPackingList.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                sSQL = "SELECT * FROM View_FabricTransferPackingList WHERE FTNID = 0";
                oFTPLs = FabricTransferPackingList.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BtnIDHtml = sBtnID;
            ViewBag.FTPLs = oFTPLs;
            
            oWorkingUnits = WorkingUnit.GetsPermittedStore(0, EnumModuleName.FabricTransferNote, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            oWorkingUnitsIssue = WorkingUnit.GetsPermittedStore(0, EnumModuleName.FabricTransferNote, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            oWorkingUnitsIssue.ForEach(x => oWorkingUnits.Add(x));
         
            ViewBag.Stores = oWorkingUnits;

            return View(_oFTN);
        }
        [HttpPost]
        public JsonResult SaveFTN(FabricTransferNote oFTN)
        {
            try
            {
                oFTN = oFTN.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFTN = new FabricTransferNote();
                oFTN.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFTN);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ReceiveFTN(FabricTransferNote oFTN)
        {
            try
            {
                oFTN = oFTN.Receive(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (string.IsNullOrEmpty(oFTN.ErrorMessage) && oFTN.IsFTPLD)
                {
                    FabricTransferPackingListDetail oFTPLD=new FabricTransferPackingListDetail();
                    if (oFTN.FTPLD.FTPLDetailID > 0)
                    {
                        oFTN.FTPLD = oFTPLD.Get(oFTN.FTPLD.FTPLDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                oFTN = new FabricTransferNote();
                oFTN.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFTN);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DisburseFTN(FabricTransferNote oFTN)
        {
            try
            {
                oFTN = oFTN.Disburse(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFTN = new FabricTransferNote();
                oFTN.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFTN);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteFTN(FabricTransferNote oFTN)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFTN.Delete(oFTN.FTNID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Print(int nFTNID, bool bIsInYard, double nts)
        {
            _oFTN = new FabricTransferNote();
            if (nFTNID > 0)
            {
                _oFTN = _oFTN.Get(nFTNID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSQL = "SELECT * FROM View_FabricTransferPackingList WHERE FTNID=" + _oFTN.FTNID;
                _oFTN.FTPLs = FabricTransferPackingList.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            string sMessage = "Transfer Challan";
            rptFabricTransferNote oReport = new rptFabricTransferNote();
            byte[] abytes = oReport.PrepareReport(_oFTN, oCompany, sMessage, bIsInYard);
            return File(abytes, "application/pdf");
        }

        [HttpPost]
        public JsonResult GetsByFEONo(FabricTransferNote oFTN)
        {
            try
            {
                _oFTNs = new List<FabricTransferNote>();
                if (!string.IsNullOrEmpty(oFTN.FEONo))
                {
                    string sSQL = "SELECT * FROM View_FabricTransferNote FTN WHERE FTN.FTNID IN (SELECT FTPL.FTNID FROM View_FabricTransferPackingList FTPL WHERE FTPL.FEONo LIKE '%" + oFTN.FEONo + "%')";
                    _oFTNs = FabricTransferNote.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFTN = new FabricTransferNote();
                oFTN.ErrorMessage = ex.Message;
                _oFTNs.Add(oFTN);
            }
            var jsonResult = Json(_oFTNs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #region Searching
        [HttpPost]
        public JsonResult GetsAdvSearch(FabricTransferNote oFTN)
        {
            List<FabricTransferNote> oFTNs = new List<FabricTransferNote>();
            try
            {
                string sSQL = this.MakeSQLAdvSearch(oFTN);
                if (!string.IsNullOrEmpty(sSQL))
                {
                    oFTNs = FabricTransferNote.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFTN = new FabricTransferNote();
                    oFTN.ErrorMessage = "Give a Searching Criteria.";
                    oFTNs.Add(oFTN);
                }
            }
            catch (Exception ex)
            {
                oFTN = new FabricTransferNote();
                oFTN.ErrorMessage = ex.Message;
                oFTNs.Add(oFTN);
            }
            var jsonResult = Json(oFTNs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string MakeSQLAdvSearch(FabricTransferNote oFTN)
        {
            string sSQL = "";

            if (!string.IsNullOrEmpty(oFTN.Params))
            {
                string sReturn1 = "SELECT * FROM View_FabricTransferNote ";
                string sReturn = "";

                string sFTNNo = Convert.ToString(oFTN.Params.Split('~')[0]);

                bool bIsNoteDate = Convert.ToBoolean(oFTN.Params.Split('~')[1]);
                DateTime dFromNoteDate = Convert.ToDateTime(oFTN.Params.Split('~')[2]);
                DateTime dToNoteDate = Convert.ToDateTime(oFTN.Params.Split('~')[3]);
                string sFEONo = Convert.ToString(oFTN.Params.Split('~')[4]);

                #region FTN No
                if (!string.IsNullOrEmpty(sFTNNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FTNNo LIKE '%" + sFTNNo + "%' ";
                }
                #endregion

                #region Packing List Date
                if (bIsNoteDate)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),NoteDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromNoteDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToNoteDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                #endregion

                #region FEO No 
                if (!string.IsNullOrEmpty(sFEONo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FTNID IN (SELECT FTNID FROM View_FabricTransferPackingList Where FEONo Like '%" + sFEONo.Trim() + "%')";
                }
                
                #endregion

                sSQL = sReturn1 + " " + sReturn + " ORDER BY FTNNo";
            }
            return sSQL;
        }
        #endregion

        public Image GetCompanyLogo(Company oCompany)
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
        #region Fabric Transfer Weavcing TO Finishing
        public ActionResult ViewFabricTransferReport(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<rptFabricTransferWeavingToFinishing> _oFTRs = new List<rptFabricTransferWeavingToFinishing>();
            return View(_oFTRs);
        }
        public void ExcelFabricTransfer(string sParam, double nts)
        {
            if(string.IsNullOrEmpty(sParam))
                throw new Exception("Search parameter required");
             DateTime dtFrom=Convert.ToDateTime(sParam.Split('~')[1]);
             DateTime dtTo=Convert.ToDateTime(sParam.Split('~')[2]);
             string sFEOIDs=sParam.Split('~')[3];
             string sBuyerIDs = sParam.Split('~')[4];
            List<rptFabricTransferWeavingToFinishing> _oFTRs = new List<rptFabricTransferWeavingToFinishing>();
            _oFTRs = rptFabricTransferWeavingToFinishing.Gets(dtFrom, dtTo, sFEOIDs, sBuyerIDs,((User)Session[SessionInfo.CurrentUser]).UserID);
            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Fabric Transfer Report");
                sheet.Name = "Daily Beam Stock Report";

                int nColumnWidth = 18;
                sheet.Column(2).Width = nColumnWidth; //Date  
                sheet.Column(3).Width = nColumnWidth; //Party Name
                sheet.Column(4).Width = nColumnWidth; //Order No.
                sheet.Column(5).Width = nColumnWidth; //Construction
                sheet.Column(6).Width = nColumnWidth; //Warp Lot
                sheet.Column(7).Width = nColumnWidth; //Weft Lot
                sheet.Column(8).Width = nColumnWidth; //Grey
              
                sheet.Column(9).Width = nColumnWidth; //Deli. Ch / GP No.
                sheet.Column(10).Width = nColumnWidth; //Deli. mtr Qty
                sheet.Column(11).Width = nColumnWidth; //Deli. Yds Qty
                sheet.Column(12).Width = nColumnWidth; //Party challan
                sheet.Column(13).Width = nColumnWidth; //Recv. Qty Mtr
                sheet.Column(14).Width = nColumnWidth; //Recv. Qty Yards
                sheet.Column(15).Width = nColumnWidth; //pross loss/gain
                sheet.Column(16).Width = nColumnWidth;//% Loss(Yds)
                sheet.Column(17).Width = nColumnWidth;//Remarks
                nMaxColumn = 17;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "GOLORA,MANIKGONJ"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Daily Fabrics Transfer Weaving to Finishing unit"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value ="Reporting Date : " +DateTime.Now.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;


                #endregion

                #region Table Header
                colIndex = 2;
                string sVal = "";
                for (int i = 1; i < nMaxColumn; i++)
                {
                    switch (i)
                    {
                        case 1: sVal = "Date"; break;
                        case 2: sVal = "Party Name"; break;
                        case 3: sVal = "Order No"; break;
                        case 4: sVal = "Construction"; break;
                 
                        case 5: sVal = "Warp Lot"; break;
                        case 6: sVal = "Weft Lot"; break;
                      
                        case 7: sVal = "Grey "; break;
                        case 8: sVal = "Deli.Ch/Gp No."; break;
                        case 9: sVal = "Deli. Mtr Qty"; break;
                        case 10: sVal = "Deli. Yds Qty"; break;
                        case 11: sVal = " Party challan"; break;
                        case 12: sVal = "Rcvd Qty(Yard)"; break;
                        case 13: sVal = "Rcvd Qty(mtr) "; break;
                        case 14: sVal = "Process loss/gain"; break;
                        case 15: sVal = "% Loss(Yds)"; break;
                        case 16: sVal = "Remarks"; break;
                        default: sVal = ""; break;
                    }

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = sVal;
                    cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                rowIndex++;
                #endregion

                #region Table body

                if (_oFTRs.Any())
                {
                    foreach(rptFabricTransferWeavingToFinishing oitem in _oFTRs)
                    {
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =oitem.FTNDateStr; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        
                         cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =oitem.BuyerName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                         cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =oitem.FEONo; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                         cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =oitem.Construction; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =oitem.WarpLot; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =oitem.WeftLot; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =oitem.ProcessName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                         cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =oitem.FTNNo; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                         cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =oitem.DeliveryQtyMeter; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                         cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =oitem.DeliveryQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =oitem.PartyChallan; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                          cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =oitem.ProcessLossGain; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                          cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value =oitem.Remarks; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        rowIndex++;
                    }
                    #region Total
                    rowIndex++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Merge = true; cell.Value = "Summary Total"; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = _oFTRs.Sum(x => x.DeliveryQtyMeter); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; 

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = _oFTRs.Sum(x => x.DeliveryQty); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; 

                    cell = sheet.Cells[rowIndex, 12, rowIndex, 17]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    rowIndex++;
                    #endregion
                        
                }

               #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=FabricTransfer.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();

             }
             
              
        }

        #endregion

    }
}