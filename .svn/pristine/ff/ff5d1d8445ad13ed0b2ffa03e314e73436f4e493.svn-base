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
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
	public class RouteSheetGraceController : Controller
	{
		#region Declaration
		RouteSheetGrace _oRouteSheetGrace = new RouteSheetGrace();
		List<RouteSheetGrace> _oRouteSheetGraces = new  List<RouteSheetGrace>();
		#endregion
		#region Functions

		#endregion
		#region Actions
        public ActionResult ViewRouteSheetGraces(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<RouteSheetGrace> oRouteSheetGraces = new List<RouteSheetGrace>();
            oRouteSheetGraces = RouteSheetGrace.Gets((int)Session[SessionInfo.currentUserID]);

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            return View(oRouteSheetGraces);
        }
        public ActionResult ViewRouteSheetGraceApprove(int nId)
        {
            RouteSheetGrace oRouteSheetGrace = new RouteSheetGrace();
            oRouteSheetGrace = oRouteSheetGrace.Get(nId, (int)Session[SessionInfo.currentUserID]);
            RouteSheetDO oRouteSheetDO = new RouteSheetDO();
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            if (oRouteSheetGrace.RouteSheetGraceID > 0)
            {
                oRouteSheetDOs = RouteSheetDO.Gets("SELECT * FROM View_RouteSheetDO WHERE DyeingOrderDetailID = " + oRouteSheetGrace.DyeingOrderDetailID, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.RouteSheetDOs = oRouteSheetDOs;
            return View(oRouteSheetGrace);
        }
        [HttpPost]
        public JsonResult Save(RouteSheetGrace oRouteSheetGrace)
        {
            _oRouteSheetGrace = new RouteSheetGrace();
            try
            {
                _oRouteSheetGrace = _oRouteSheetGrace.Save(oRouteSheetGrace, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRouteSheetGrace = new RouteSheetGrace();
                _oRouteSheetGrace.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheetGrace);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsRouteSheetGraceByNo(RouteSheetGrace oRouteSheetGrace)
        {
            _oRouteSheetGraces = new List<RouteSheetGrace>();
            try
            {
                string sRouteSheetNo = (string.IsNullOrEmpty(oRouteSheetGrace.RouteSheetNo)) ? "" : oRouteSheetGrace.RouteSheetNo.Trim();
                string sOrderNo = (string.IsNullOrEmpty(oRouteSheetGrace.OrderNo)) ? "" : oRouteSheetGrace.OrderNo.Trim();

                string sSQL = "Select * from View_RouteSheetGrace Where RouteSheetID<>0 ";
                if (sRouteSheetNo != "") sSQL = sSQL + " And RouteSheetNo Like '%" + sRouteSheetNo + "%'";
                if (sOrderNo != "") sSQL = sSQL + " And OrderNo Like '%" + sOrderNo + "%'";
                _oRouteSheetGraces = RouteSheetGrace.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheetGrace = new RouteSheetGrace();
                _oRouteSheetGraces = new List<RouteSheetGrace>();
                oRouteSheetGrace.ErrorMessage = ex.Message;
                _oRouteSheetGraces.Add(oRouteSheetGrace);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheetGraces);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AdvSearchRSGrace(RouteSheetGrace oRouteSheetGrace)
        {
            _oRouteSheetGraces = new List<RouteSheetGrace>();
            try
            {
                string sSQL = MakeSQL(oRouteSheetGrace);
                _oRouteSheetGraces = RouteSheetGrace.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheetGrace = new RouteSheetGrace();
                _oRouteSheetGraces = new List<RouteSheetGrace>();
                oRouteSheetGrace.ErrorMessage = ex.Message;
                _oRouteSheetGraces.Add(oRouteSheetGrace);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheetGraces);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string MakeSQL(RouteSheetGrace oRouteSheetGrace)
        {
            int nBUID = 0,
                nDateCriteria_Batch = 0,
                nDateCriteria_Req = 0,
                nDateCriteria_App = 0;

            string sOrderNo = "",
                   sRouteSheetNo = "";

            DateTime dStart_Batch = DateTime.Today,
                     dEnd_Batch = DateTime.Today;
            DateTime dStart_Req = DateTime.Today,
                     dEnd_Req = DateTime.Today;
            DateTime dStart_App = DateTime.Today,
                     dEnd_App = DateTime.Today;
            bool YetToApprove = false;


            string sParams = oRouteSheetGrace.ErrorMessage;
            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                sRouteSheetNo = sParams.Split('~')[nCount++];
                sOrderNo = sParams.Split('~')[nCount++];
              
                nDateCriteria_Batch = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_Batch = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_Batch = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nDateCriteria_Req = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_Req = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_Req = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nDateCriteria_App = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_App = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_App = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                YetToApprove = Convert.ToBoolean(sParams.Split('~')[nCount++]);
            }

            string sReturn1 = "SELECT * FROM View_RouteSheetGrace";
            string sReturn = " WHERE DyeingOrderDetailID <>0 ";

            #region DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, "DBServerDateTime", nDateCriteria_Req, dStart_Batch, dEnd_Batch);
            DateObject.CompareDateQuery(ref sReturn, "RouteSheetDate", nDateCriteria_Batch, dStart_Req, dEnd_Req);
            DateObject.CompareDateQuery(ref sReturn, "ApproveDate", nDateCriteria_App, dStart_App, dEnd_App);
            #endregion

            #region OrderNo
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderNo LIKE '%" + sOrderNo + "%'";
            }
            #endregion

            #region BatchNo
            if (!string.IsNullOrEmpty(sRouteSheetNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RouteSheetNo LIKE '%" + sRouteSheetNo + "%'";
            }
            #endregion

            #region Yet To Approve
            if (YetToApprove)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApprovedByID,0) = 0";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        [HttpPost]
        public JsonResult Approve(RouteSheetGrace oRouteSheetGrace)
        {
            _oRouteSheetGrace = new RouteSheetGrace();
            try
            {
                _oRouteSheetGrace = _oRouteSheetGrace.Approve(oRouteSheetGrace, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRouteSheetGrace = new RouteSheetGrace();
                _oRouteSheetGrace.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheetGrace);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

		#endregion

        #region PRINT
        public ActionResult Preview_RSGrace(int BUID, int id)
        {
            DyeingOrder oDyeingOrder = new DyeingOrder();
            DyeingOrderDetail oDyeingOrderDetail = new DyeingOrderDetail();
            List<DUOrderRS> oDUOrderRSs = new List<DUOrderRS>();

            List<DUReturnChallan> oDUReturnChallans = new List<DUReturnChallan>();
            List<DUDeliveryChallan> oDUDeliveryChallans = new List<DUDeliveryChallan>();

            List<DUDeliveryChallanDetail> oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
            List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();

            if (id>0)
            {
                oDyeingOrderDetail = oDyeingOrderDetail.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDyeingOrder = DyeingOrder.Get(oDyeingOrderDetail.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oDUOrderRSs = DUOrderRS.GetsQC(" WHERE DyeingOrderDetailID ="+ id, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oDUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets("SELECT * FROM View_DUDeliveryChallanDetail WHERE DyeingOrderDetailID = " + id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDUReturnChallanDetails = DUReturnChallanDetail.Gets("SELECT * FROM View_DUReturnChallanDetail WHERE DyeingOrderDetailID = " + id, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oDUDeliveryChallans = DUDeliveryChallan.Gets("SELECT * FROM View_DUDeliveryChallan WHERE DUDeliveryChallanID IN (SELECT DUDeliveryChallanID FROM DUDeliveryChallanDetail WHERE DyeingOrderDetailID = " + id + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDUReturnChallans = DUReturnChallan.Gets("SELECT * FROM View_DUReturnChallan WHERE DUReturnChallanID IN (SELECT DUReturnChallanID FROM View_DUDeliveryChallanDetail WHERE DyeingOrderDetailID = " + id + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (var oitem in oDUDeliveryChallanDetails) 
                {
                    var oChallan = oDUDeliveryChallans.Where(x=>x.DUDeliveryChallanID==oitem.DUDeliveryChallanID).FirstOrDefault();
                    oitem.ChallanDate = oChallan.ChallanDateSt;
                    oitem.ChallanNo = oChallan.ChallanNo;
                }
                foreach (var oitem in oDUReturnChallans)
                {
                    var oChallan = oDUReturnChallans.Where(x => x.DUReturnChallanID == oitem.DUReturnChallanID).FirstOrDefault();
                    oitem.ReturnDate = oChallan.ReturnDate;
                    oitem.DUReturnChallanNo = oChallan.DUReturnChallanNo;
                }
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptRouteSheetGrace oReport = new rptRouteSheetGrace();
            oReport.DUDeliveryChallanDetails = oDUDeliveryChallanDetails;
            oReport.DUReturnChallanDetails = oDUReturnChallanDetails;
            byte[] abytes = oReport.PrepareReport(oDUOrderRSs, oDyeingOrder, oDyeingOrderDetail,  oBusinessUnit, oCompany, "Dyeing Batch Report");
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintList(string sParams, int nBuid)
        {
            RouteSheetGrace oRouteSheetGrace = new RouteSheetGrace();
            List<RouteSheetGrace> oRouteSheetGraces = new List<RouteSheetGrace>();
            if (!string.IsNullOrEmpty(sParams))
            {
                oRouteSheetGrace.ErrorMessage = sParams;
                oRouteSheetGraces = RouteSheetGrace.Gets(MakeSQL(oRouteSheetGrace), ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBuid, (int)Session[SessionInfo.currentUserID]);

            rptRouteSheetGraceReport oReport = new rptRouteSheetGraceReport();
            byte[] abytes;
            abytes = oReport.PrepareReport(oRouteSheetGraces, oCompany, oBusinessUnit);
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
        #endregion

        #region Excel
        public void PrintExcel(string sParams, int nBuid)
        {
            RouteSheetGrace oRouteSheetGrace = new RouteSheetGrace();
            List<RouteSheetGrace> oRouteSheetGraces = new List<RouteSheetGrace>();
            if(!string.IsNullOrEmpty(sParams))
            {
                oRouteSheetGrace.ErrorMessage = sParams;
                oRouteSheetGraces = RouteSheetGrace.Gets(MakeSQL(oRouteSheetGrace),((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBuid, (int)Session[SessionInfo.currentUserID]);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Batch No", Width = 20f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Grace Count", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Order Qty", Width = 15f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            table_header.Add(new TableHeader { Header = "Qty Grace", Width = 15f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            table_header.Add(new TableHeader { Header = "Qty(Pro)", Width = 15f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            table_header.Add(new TableHeader { Header = "Recycle Qty", Width = 15f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            table_header.Add(new TableHeader { Header = "Wastage Qty", Width = 15f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            table_header.Add(new TableHeader { Header = "Yet To Production", Width = 18f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            table_header.Add(new TableHeader { Header = "Reason", Width = 20f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Approve Qty", Width = 20f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            table_header.Add(new TableHeader { Header = "Requested By", Width = 25f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Request Date", Width = 20f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Approved By", Width = 25f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Approve Date", Width = 20f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Approve Note", Width = 40f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Grace Report";
                var sheet = excelPackage.Workbook.Worksheets.Add("Grace Report");
                sheet.Name = "Grace Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = table_header.Count;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "GraceReport"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                #endregion

                #region Data
                nRowIndex++;
                nStartCol = 2;

                int nCount = 0; nEndCol = table_header.Count() + nStartCol;
                nEndCol = table_header.Count() + nStartCol;

                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                foreach (RouteSheetGrace obj in oRouteSheetGraces)
                {
                    nStartCol = 2;
                    ExcelTool.Formatter = "";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), true, true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.RouteSheetNo, false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.OrderNo, false, false);
                    ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.GraceCount.ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.OrderQty.ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.QtyGrace.ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty_Pro.ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.RecycleQty.ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.WastageQty.ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.YetToProduction.ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.Formatter = "";                    
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Note.ToString(), false, false);
                    ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.QtyGraceForApprove.ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.Formatter = "";                                        
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PrepareByName.ToString(), false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LastUpdateDateTimeSt.ToString(), false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ApprovedByName.ToString(), false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ApproveDateSt.ToString(), false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.NoteApp.ToString(), false, false);
                    nRowIndex++;
                }

                #region Grand Total
                nStartCol = 2;
                ExcelTool.Formatter = "";
                ExcelTool.FillCellMerge(ref sheet, "Total :", nRowIndex, nRowIndex, nStartCol, 4, true, ExcelHorizontalAlignment.Right, true);
                nStartCol = 5;
                ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oRouteSheetGraces.Sum(x => x.GraceCount).ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oRouteSheetGraces.Sum(x => x.OrderQty).ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oRouteSheetGraces.Sum(x => x.QtyGrace).ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oRouteSheetGraces.Sum(x => x.Qty_Pro).ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oRouteSheetGraces.Sum(x => x.RecycleQty).ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oRouteSheetGraces.Sum(x => x.WastageQty).ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oRouteSheetGraces.Sum(x => x.YetToProduction).ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                ExcelTool.Formatter = "";                
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false, false, ExcelHorizontalAlignment.Right, false);
                ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oRouteSheetGraces.Sum(x => x.QtyGraceForApprove).ToString(), true, false, ExcelHorizontalAlignment.Right, false);
                ExcelTool.Formatter = "";
                ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol++, 18, true, ExcelHorizontalAlignment.Right, true);
                #endregion

                #endregion
                nRowIndex++;

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=GraceReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion

        }
        #endregion

    }
}
