using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;
using System.Security;
using OfficeOpenXml;
using OfficeOpenXml.Style;
namespace ESimSolFinancial.Controllers
{
    public class FabricYarnDeliveryOrderController : PdfViewController
    {
        #region FabricYarnDeliveryOrder


        public ActionResult ViewFabricYarnDeliveryOrders(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<FabricYarnDeliveryOrder> oFabricYarnDeliveryOrders = new List<FabricYarnDeliveryOrder>();
            string sSQL = "Select * from View_FabricYarnDeliveryOrder FYDO Where ISNULL(FYDO.ApproveBy,0)<>0 And FYDO.OrderQty > FYDO.ChallanQty ORDER BY FYDOID DESC";
            oFabricYarnDeliveryOrders = FabricYarnDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            
            List<FabricYarnDeliveryChallan> oFabricYarnDeliveryChallans = new List<FabricYarnDeliveryChallan>();
            sSQL = "Select * from View_FabricYarnDeliveryChallan Where ISNULL(DisburseBy,0)=0";
            oFabricYarnDeliveryChallans = FabricYarnDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FYDCs = oFabricYarnDeliveryChallans;

            int[] TUnits = new int[] { (int)EnumTextileUnit.Dyeing, (int)EnumTextileUnit.Weaving };
            ViewBag.TUnits = EnumObject.jGets(typeof(EnumTextileUnit)).Where(x => TUnits.Contains(x.id)).ToList();

            var oWUs = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricDeliveryOrder,EnumStoreType.IssueStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.WUs = oWUs;

            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
           
            return View(oFabricYarnDeliveryOrders);
        }

        public ActionResult ViewFabricYarnDeliveryOrder(int nId, double ts)
        {
            FabricYarnDeliveryOrder oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
            List<FabricYarnDeliveryOrderDetail> oFabricYarnDeliveryOrderDetails = new List<FabricYarnDeliveryOrderDetail>();

            string sSQL = "";
            if (nId > 0)
            {
                oFabricYarnDeliveryOrder = FabricYarnDeliveryOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = "Select * from View_FabricYarnDeliveryOrderDetail Where FYDOID=" + oFabricYarnDeliveryOrder.FYDOID + "";
                oFabricYarnDeliveryOrderDetails = FabricYarnDeliveryOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.FabricYarnDeliveryOrderDetails = oFabricYarnDeliveryOrderDetails;
            return View(oFabricYarnDeliveryOrder);
        }

        public ActionResult AdvanceSearch()
        {
            return PartialView();
        }



        [HttpPost]
        public JsonResult Save(FabricYarnDeliveryOrder oFabricYarnDeliveryOrder)
        {
            try
            {
                if (oFabricYarnDeliveryOrder.FYDOID <= 0)
                {
                    oFabricYarnDeliveryOrder = oFabricYarnDeliveryOrder.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFabricYarnDeliveryOrder = oFabricYarnDeliveryOrder.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
                oFabricYarnDeliveryOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricYarnDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FabricYarnDeliveryOrder oFabricYarnDeliveryOrder)
        {
            try
            {
                if (oFabricYarnDeliveryOrder.FYDOID <= 0) { throw new Exception("Please select an valid item."); }
                oFabricYarnDeliveryOrder = oFabricYarnDeliveryOrder.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
                oFabricYarnDeliveryOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricYarnDeliveryOrder.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveFYDO(FabricYarnDeliveryOrder oFabricYarnDeliveryOrder)
        {
            try
            {
                if (oFabricYarnDeliveryOrder.FYDOID <= 0) { throw new Exception("Please select an valid item."); }
                else if (oFabricYarnDeliveryOrder.ApproveBy != 0) { throw new Exception("Already Approved."); }

                oFabricYarnDeliveryOrder = oFabricYarnDeliveryOrder.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
                oFabricYarnDeliveryOrder.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricYarnDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ReviseFYDO(FabricYarnDeliveryOrder oFabricYarnDeliveryOrder)
        {
            try
            {

                if (oFabricYarnDeliveryOrder.FYDOID <= 0)
                    throw new Exception("Invalid fabric yarn order.");

                oFabricYarnDeliveryOrder = oFabricYarnDeliveryOrder.IUD((int)EnumDBOperation.Revise, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
                oFabricYarnDeliveryOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricYarnDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        

        [HttpPost]
        public JsonResult SaveFYDODetail(FabricYarnDeliveryOrderDetail oFabricYarnDeliveryOrderDetail)
        {
            try
            {
                if (oFabricYarnDeliveryOrderDetail.FYDODetailID <= 0)
                {
                    oFabricYarnDeliveryOrderDetail = oFabricYarnDeliveryOrderDetail.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFabricYarnDeliveryOrderDetail = oFabricYarnDeliveryOrderDetail.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFabricYarnDeliveryOrderDetail = new FabricYarnDeliveryOrderDetail();
                oFabricYarnDeliveryOrderDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricYarnDeliveryOrderDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFYDODetail(FabricYarnDeliveryOrderDetail oFabricYarnDeliveryOrderDetail)
        {
            try
            {
                if (oFabricYarnDeliveryOrderDetail.FYDODetailID <= 0) { throw new Exception("Please select an valid item."); }
                oFabricYarnDeliveryOrderDetail = oFabricYarnDeliveryOrderDetail.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricYarnDeliveryOrderDetail = new FabricYarnDeliveryOrderDetail();
                oFabricYarnDeliveryOrderDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricYarnDeliveryOrderDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Get(FabricYarnDeliveryOrder oFabricYarnDeliveryOrder)
        {
            try
            {
                if (oFabricYarnDeliveryOrder.FYDOID <= 0) { throw new Exception("Please select an valid item."); }
                oFabricYarnDeliveryOrder = FabricYarnDeliveryOrder.Get(oFabricYarnDeliveryOrder.FYDOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
                oFabricYarnDeliveryOrder.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricYarnDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AdvSearch(FabricYarnDeliveryOrder oFYDO)
        {
            List<FabricYarnDeliveryOrder> oFYDOs = new List<FabricYarnDeliveryOrder>();
            try
            {
                if (string.IsNullOrEmpty(oFYDO.Params) || oFYDO.Params.Split('~').Length < 7)
                    throw new Exception("Search parameter required.");

                string sFYDNo = string.Empty, sFEONo = string.Empty ,BuyerIDs =string.Empty;
                EnumTextileUnit enumDU = EnumTextileUnit.None;
                int status = -1; // All=>-1, NotApproved => 0, Approve => 1
                bool IsDateSearch = false;
                DateTime dtExptFrom, dtExptTo;
                bool IsIssueDateSearch = false;
                DateTime dtIssueFrom, dtIssueTo;


                sFYDNo = oFYDO.Params.Split('~')[0];
                sFEONo = oFYDO.Params.Split('~')[1];
                Enum.TryParse<EnumTextileUnit>(oFYDO.Params.Split('~')[2], out enumDU);
                bool.TryParse(oFYDO.Params.Split('~')[3], out IsDateSearch);
                DateTime.TryParse(oFYDO.Params.Split('~')[4], out dtExptFrom);
                DateTime.TryParse(oFYDO.Params.Split('~')[5], out dtExptTo);
                int.TryParse(oFYDO.Params.Split('~')[6], out status);
                bool.TryParse(oFYDO.Params.Split('~')[7], out IsIssueDateSearch);
                DateTime.TryParse(oFYDO.Params.Split('~')[8], out dtIssueFrom);
                DateTime.TryParse(oFYDO.Params.Split('~')[9], out dtIssueTo);
                BuyerIDs = oFYDO.Params.Split('~')[10];


                string sSQL = "Select * from View_FabricYarnDeliveryOrder Where FYDOID<>0 ";

                if (!string.IsNullOrEmpty(sFYDNo))
                    sSQL += " And FYDNo Like '%" + sFYDNo.Trim() + "%'";

                if (!string.IsNullOrEmpty(sFEONo))
                    sSQL += " And FEONo Like '%" + sFEONo.Trim() + "%'";
                if (!string.IsNullOrEmpty(BuyerIDs))
                    sSQL += " And BuyerID IN ( " + BuyerIDs.Trim() + ")";

                if (enumDU != EnumTextileUnit.None)
                    sSQL += " And DeliveryUnit = " + (int)enumDU + "";

                if (IsDateSearch)
                    sSQL += " And ExpectedDeliveryDate Between '" + dtExptFrom.ToString("dd MMM yyyy") + "' And '" + dtExptTo.ToString("dd MMM yyyy") + "'";
                if (IsIssueDateSearch)
                    sSQL += " And CONVERT(DATE, DBServerDateTime) Between '" + dtIssueFrom.ToString("dd MMM yyyy") + "' And '" + dtIssueTo.ToString("dd MMM yyyy") + "'";


                if (status == 0) // Not Approved
                    sSQL += " And ISNULL(ApproveBy,0)=0";
                else if (status == 1) // Approved
                    sSQL += " And ISNULL(ApproveBy,0)<>0";
                sSQL += " ORDER BY FYDOID DESC";

                oFYDOs = FabricYarnDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFYDOs = new List<FabricYarnDeliveryOrder>();
                oFYDO = new FabricYarnDeliveryOrder();
                oFYDO.ErrorMessage = ex.Message;
                oFYDOs.Add(oFYDO);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFYDOs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult GetsByFYDNo(FabricYarnDeliveryOrder oFabricYarnDeliveryOrder)
        {
            List<FabricYarnDeliveryOrder> oFYDOs = new List<FabricYarnDeliveryOrder>();
            try
            {
                if (string.IsNullOrEmpty(oFabricYarnDeliveryOrder.FYDNo))
                    throw new Exception("Search parameter required.");

                string sSQL = "Select * from View_FabricYarnDeliveryOrder Where FYDNo Like '%" + oFabricYarnDeliveryOrder.FYDNo + "%'";
                oFYDOs = FabricYarnDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFYDOs = new List<FabricYarnDeliveryOrder>();
                oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
                oFabricYarnDeliveryOrder.ErrorMessage = ex.Message;
                oFYDOs.Add(oFabricYarnDeliveryOrder);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFYDOs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByPendingDO()
        {
            List<FabricYarnDeliveryOrder> oFYDOs = new List<FabricYarnDeliveryOrder>();
            FabricYarnDeliveryOrder oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
            try
            {


                string sSQL = "Select * from View_FabricYarnDeliveryOrder FYDO Where FYDO.OrderQty <> FYDO.ChallanQty";
                oFYDOs = FabricYarnDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFYDOs = new List<FabricYarnDeliveryOrder>();
                oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
                oFabricYarnDeliveryOrder.ErrorMessage = ex.Message;
                oFYDOs.Add(oFabricYarnDeliveryOrder);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFYDOs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByFEONo(FabricYarnDeliveryOrder oFabricYarnDeliveryOrder)
        {
            List<FabricYarnDeliveryOrder> oFYDOs = new List<FabricYarnDeliveryOrder>();
            try
            {
                if (string.IsNullOrEmpty(oFabricYarnDeliveryOrder.FEONo))
                    throw new Exception("Search parameter required.");

                string sSQL = "Select * from View_FabricYarnDeliveryOrder Where FEONo Like '%" + oFabricYarnDeliveryOrder.FEONo + "%'";
                oFYDOs = FabricYarnDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFYDOs = new List<FabricYarnDeliveryOrder>();
                oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
                oFabricYarnDeliveryOrder.ErrorMessage = ex.Message;
                oFYDOs.Add(oFabricYarnDeliveryOrder);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFYDOs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByIssueDate(FabricYarnDeliveryOrder oFYDO)
        {
            List<FabricYarnDeliveryOrder> oFYDOs = new List<FabricYarnDeliveryOrder>();
            try
            {
                if (string.IsNullOrEmpty(oFYDO.Params) || oFYDO.Params.Split('~').Length <3)
                    throw new Exception("Search parameter required.");
 
                bool IsIssueDateSearch = false;
                DateTime dtIssueFrom, dtIssueTo;
                bool.TryParse(oFYDO.Params.Split('~')[0], out IsIssueDateSearch);
                DateTime.TryParse(oFYDO.Params.Split('~')[1], out dtIssueFrom);
                DateTime.TryParse(oFYDO.Params.Split('~')[2], out dtIssueTo);
                string sSQL = "Select * from View_FabricYarnDeliveryOrder Where FYDOID<>0 ";
                sSQL += " And DBServerDateTime Between '" + dtIssueFrom.ToString("dd MMM yyyy") + "' And '" + dtIssueTo.ToString("dd MMM yyyy") + "'";
                oFYDOs = FabricYarnDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFYDOs = new List<FabricYarnDeliveryOrder>();
                oFYDO = new FabricYarnDeliveryOrder();
                oFYDO.ErrorMessage = ex.Message;
                oFYDOs.Add(oFYDO);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFYDOs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintFYDODeliveryStatement(FormCollection fc)
        {
            if (string.IsNullOrEmpty(fc["FYDOIDs"]))
                throw new Exception("Invalid Search ");

            String FYDOIDs = fc["FYDOIDs"].Substring(0, fc["FYDOIDs"].Length - 1);
            string sSQL =string.Empty;

            List<FabricYarnDeliveryOrder> oFYDOs = new List<FabricYarnDeliveryOrder>();
            List<FabricYarnDeliveryChallan> oFYDCs = new List<FabricYarnDeliveryChallan>();
             List<FabricYarnDeliveryChallanDetail> oFYDCDs = new List<FabricYarnDeliveryChallanDetail>();
            sSQL="SELECT * FROM View_FabricYarnDeliveryOrder WHERE FYDOID IN ("+FYDOIDs+")";
            oFYDOs = FabricYarnDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
           
            sSQL="SELECT * FROM View_FabricYarnDeliveryChallan WHERE FYDOID IN ("+FYDOIDs+")";
            oFYDCs = FabricYarnDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oFYDCs.Any())
            {
                sSQL = "SELECT * FROM View_FabricYarnDeliveryChallanDetail WHERE FYDChallanID IN (" + string.Join(",", oFYDCs.Select(x=>x.FYDChallanID)) + ")";
                oFYDCDs = FabricYarnDeliveryChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
          
            string Messge = "Delivery Statement";
            rptFabricYarnDeliveryOrderStatement oReport = new rptFabricYarnDeliveryOrderStatement();
            byte[] abytes = oReport.PrepareReport(oFYDOs, oFYDCs, oFYDCDs, Messge, oCompany);
            return File(abytes, "application/pdf");
        }

        public void ExcelFYDODeliveryStatement(FormCollection fc)
        {
            if (string.IsNullOrEmpty(fc["FYDOIDs"]))
                throw new Exception("Invalid Search ");

            String FYDOIDs = fc["FYDOIDs"].Substring(0, fc["FYDOIDs"].Length - 1);
            string sSQL = string.Empty;

            List<FabricYarnDeliveryOrder> oFYDOs = new List<FabricYarnDeliveryOrder>();
            List<FabricYarnDeliveryChallan> oFYDCs = new List<FabricYarnDeliveryChallan>();
            List<FabricYarnDeliveryChallanDetail> oFYDCDs = new List<FabricYarnDeliveryChallanDetail>();
            sSQL = "SELECT * FROM View_FabricYarnDeliveryOrder WHERE FYDOID IN (" + FYDOIDs + ")";
            oFYDOs = FabricYarnDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = "SELECT * FROM View_FabricYarnDeliveryChallan WHERE FYDOID IN (" + FYDOIDs + ")";
            oFYDCs = FabricYarnDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oFYDCs.Any())
            {
                sSQL = "SELECT * FROM View_FabricYarnDeliveryChallanDetail WHERE FYDChallanID IN (" + string.Join(",", oFYDCs.Select(x => x.FYDChallanID)) + ")";
                oFYDCDs = FabricYarnDeliveryChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";


                var sheet = excelPackage.Workbook.Worksheets.Add("Delivery Statement");
                sheet.Name = "Delivery Statement";

                sheet.Column(2).Width = 8; //SL
                sheet.Column(3).Width = 18; //Do No.
                sheet.Column(4).Width = 15; // FEO No.

                sheet.Column(5).Width = 20; // DO Date
                sheet.Column(6).Width = 20; // DO Qty

                sheet.Column(7).Width = 20; // Party Name
                sheet.Column(8).Width = 18; // LC No. 

                sheet.Column(9).Width = 20; //Del.Point.

                sheet.Column(10).Width = 18; // Challan No.
                sheet.Column(11).Width = 16; // Challan Date

                sheet.Column(12).Width = 16; //Lot No.
                sheet.Column(13).Width = 16; //Yarn Count
                sheet.Column(14).Width = 20; // Challan Qty
                sheet.Column(15).Width = 16; // Total Challan Qty
                sheet.Column(16).Width = 20; //Yet To Challan Qty
                sheet.Column(17).Width = 20; //Yet To Challan Qty
                sheet.Column(18).Width = 20; //Yet To Challan Qty

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Delivery Statement"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Column Header
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 3]; cell.Value = "Do No."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "FEO No."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 5]; cell.Value = "DO Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 6]; cell.Value = " DO Qty"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 7]; cell.Value = "Party Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 8]; cell.Value = "LC No. "; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 9]; cell.Value = "Del.Point."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 10]; cell.Value = "Challan No."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 11]; cell.Value = "Challan Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 12]; cell.Value = "Lot No."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 13]; cell.Value = "Yarn Count"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 14]; cell.Value = "Challan Qty"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 15]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 16]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, 17]; cell.Value = "Total Challan Qty"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, 18]; cell.Value = "Yet To Challan Qty" ; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                rowIndex = rowIndex + 1;
                #endregion

                #region Report Data
                if (oFYDOs.Count > 0)
                {
                    int nSL = 0;
                    double _nTotalDOQty = 0, _nTotalChallanQty = 0, _nTotalYetQty = 0, _nChallanQty=0,_nAmount=0;
                    foreach (FabricYarnDeliveryOrder oItem in oFYDOs)
                    {
                        nSL++;

                        int mergecount = 0;
                       

                        List<FabricYarnDeliveryChallan> oTempFYDCs = new List<FabricYarnDeliveryChallan>();
                        oTempFYDCs = oFYDCs.Where(o => o.FYDOID == oItem.FYDOID).ToList();
                        if (oTempFYDCs.Any())
                        {
                            mergecount = oTempFYDCs.Count - 1;
                        }
                        int TempRowindex = rowIndex;
                        cell = sheet.Cells[rowIndex, 2, rowIndex + mergecount, 2]; cell.Value = nSL; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3, rowIndex + mergecount, 3]; cell.Value = oItem.FYDNo; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4, rowIndex + mergecount, 4]; cell.Value = oItem.FEONo; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5, rowIndex + mergecount, 5]; cell.Value = oItem.ExpectedDeliveryDateStr; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        _nTotalDOQty += oItem.OrderQty;
                        cell = sheet.Cells[rowIndex, 6, rowIndex + mergecount, 6]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 7, rowIndex + mergecount, 7]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8, rowIndex + mergecount, 8]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 9, rowIndex + mergecount, 9]; cell.Value = oItem.DeliveryUnitStr; cell.Style.Font.Bold = false;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        double nChallanUnitPrice = 0;
                        if (oTempFYDCs.Any())
                        {
                            foreach (FabricYarnDeliveryChallan oItem1 in oTempFYDCs)
                            {
                                cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem1.FYDChallanNo; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem1.DBServerDateTimeStr; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, 12]; cell.Value = string.Join(" , ", oFYDCDs.Where(x => x.FYDChallanID == oItem1.FYDChallanID).ToList().Select(x=>x.LotNo)); cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, 13]; cell.Value = string.Join(" , ", oFYDCDs.Where(x => x.FYDChallanID == oItem1.FYDChallanID).ToList().Select(x => x.ProductName)); cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                double challanQty = oFYDCDs.Where(x => x.FYDChallanID == oItem1.FYDChallanID).ToList().Sum(x => x.Qty);
                                _nChallanQty += challanQty;
                                cell = sheet.Cells[rowIndex, 14]; cell.Value =challanQty; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                double unitPrice = oFYDCDs.Where(x => x.FYDChallanID == oItem1.FYDChallanID).ToList().Sum(x => x.UnitPrice);
                            
                                cell = sheet.Cells[rowIndex, 15]; cell.Value = unitPrice; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                _nAmount += challanQty*unitPrice;
                                cell = sheet.Cells[rowIndex, 16]; cell.Value =Math.Round(challanQty * unitPrice,2); cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                rowIndex++;

                            }
                        }
                        else
                        {
                            rowIndex++;
                        }
                         _nTotalChallanQty+=GetReturnChallanQty(oItem.FYDOID , ref oFYDCs ,ref oFYDCDs);
                         cell = sheet.Cells[TempRowindex, 17, TempRowindex + mergecount, 17]; cell.Value = GetReturnChallanQty(oItem.FYDOID, ref oFYDCs, ref oFYDCDs); cell.Style.Font.Bold = false;
                         cell.Merge = true; 
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                         fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                   
                         double nYetToChallanQty = oItem.OrderQty  - GetReturnChallanQty(oItem.FYDOID , ref oFYDCs ,ref oFYDCDs);

                         _nTotalYetQty += nYetToChallanQty;

                         cell = sheet.Cells[TempRowindex, 18, TempRowindex + mergecount, 18]; cell.Value = nYetToChallanQty; cell.Style.Font.Bold = false;
                         cell.Merge = true;  
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                         fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        
                    }

                    #region Subtotal

                    cell = sheet.Cells[rowIndex, 2,rowIndex,5]; cell.Value = "Subtotal"; cell.Style.Font.Bold = true; cell.Merge = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                   
                    cell = sheet.Cells[rowIndex, 6]; cell.Value = _nTotalDOQty; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = " "; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 14]; cell.Value = _nChallanQty; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 16]; cell.Value = Math.Round(_nAmount,2); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, 17]; cell.Value = _nTotalChallanQty; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 18]; cell.Value = _nTotalYetQty; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    rowIndex = rowIndex + 1;

                    #endregion



                #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=DeliveryStatement.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
            }
        }
        private double GetReturnChallanQty(int nFYDOID, ref List<FabricYarnDeliveryChallan> oFYDCs, ref List<FabricYarnDeliveryChallanDetail> oFYDCDs)
        {
            double nResult = 0;

            List<FabricYarnDeliveryChallan> oTempFYDCs = new List<FabricYarnDeliveryChallan>();
            List<FabricYarnDeliveryChallanDetail> oTempFYDCDs = new List<FabricYarnDeliveryChallanDetail>();
            oTempFYDCs = oFYDCs.Where(o => o.FYDOID == nFYDOID).ToList();

            foreach (FabricYarnDeliveryChallan oItem in oTempFYDCs)
            {
                oTempFYDCDs = new List<FabricYarnDeliveryChallanDetail>();
                oTempFYDCDs = oFYDCDs.Where(x => x.FYDChallanID == oItem.FYDChallanID).ToList();
                if (oTempFYDCDs.Count > 0)
                {
                    nResult += oTempFYDCDs.Sum(x => x.Qty);
                }
            }
            return nResult;
        }

        [HttpPost]
        public JsonResult GetsFYDOByFEO(FabricYarnDeliveryOrder oFYDO)
        {

            List<FabricYarnDeliveryOrder> oFYDOs = new List<FabricYarnDeliveryOrder>();
            List<FabricYarnDeliveryOrderDetail> oFYDODetails = new List<FabricYarnDeliveryOrderDetail>();

            try
            {

                int nFEOID = oFYDO.FEOID;
                int nFYDOID = oFYDO.FYDOID;

                oFYDO = new FabricYarnDeliveryOrder();
                string sSQL = "Select * from View_FabricYarnDeliveryOrder Where FEOID=" + nFEOID + " Order By FYDOID DESC";
                oFYDOs = FabricYarnDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oFYDOs.Any() && oFYDOs.First().FYDOID > 0)
                {
                    oFYDO = (nFYDOID>0)? oFYDOs.Where(x=>x.FYDOID==nFYDOID).First() : oFYDOs.First();
                    sSQL = "Select * from View_FabricYarnDeliveryOrderDetail Where FYDOID = " + oFYDO.FYDOID + "";
                    oFYDO.FYDODetails = FabricYarnDeliveryOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFYDOs.RemoveAll(x => x.FYDOID == oFYDO.FYDOID);
                }


                List<FabricSalesContractDetail> oFEODetails = new List<FabricSalesContractDetail>();
                sSQL = "Select * from View_FabricSalesContractDetail Where FEOID=" + nFEOID + " And ProductID Not In (Select ProductID from SUDeliveryOrderDetail Where SUDeliveryOrderID In (Select SUDeliveryOrderID from SUDeliveryOrder Where FEOID=" + nFEOID + "))";
                sSQL += " And ProductID NOT IN (Select ProductID from FabricYarnDeliveryOrderDetail Where FYDOID In (Select FYDOID from FabricYarnDeliveryOrder Where FEOID=" + nFEOID + "))";

                oFEODetails = FabricSalesContractDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                //if (oFEODetails.Any() && oFEODetails.FirstOrDefault().FEODID > 0)
                //{
                //    oFEODetails.ForEach(x =>
                //    {
                //        oFYDODetails.Add(new FabricYarnDeliveryOrderDetail
                //        {
                //            FYDODetailID = 0,
                //            FYDOID = 0,
                //            ProductID = x.ProductID,
                //            Qty = x.Qty,
                //            ProductName = x.ProductName,
                //            ProductCode = x.ProductCode
                //        });

                //    });
                //    oFYDO.Params = string.Join(",", oFEODetails.Where(x => x.SuggestedQty > 0).Select(x => x.ProductName + " - " + x.SuggestedQty + " Kg"));
                //}
                //List<DyeingExecutionOrderDetail> oDEODs = new List<DyeingExecutionOrderDetail>();
                //sSQL = "SELECT * FROM View_DyeingExecutionOrderDetail DEOD WHERE DEOD.DEOID IN (SELECT DEO.DEOID FROM DyeingExecutionOrder DEO WHERE DEO.FEOID=" + nFEOID + ")";
                //oDEODs = DyeingExecutionOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //oFYDO.DEOQty = oDEODs.Sum(o => o.Qty);

            }
            catch (Exception ex)
            {
                oFYDO = new FabricYarnDeliveryOrder();
                oFYDO.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(new { FYDO = oFYDO, FYDOrders = oFYDOs, FYDODetails = oFYDODetails });
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFYDODetail(FabricYarnDeliveryOrderDetail oFabricYarnDeliveryOrderDetail)
        {
            try
            {
                if (oFabricYarnDeliveryOrderDetail.FYDODetailID <= 0) { throw new Exception("Please select an valid item."); }
                oFabricYarnDeliveryOrderDetail = FabricYarnDeliveryOrderDetail.Get(oFabricYarnDeliveryOrderDetail.FYDODetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricYarnDeliveryOrderDetail = new FabricYarnDeliveryOrderDetail();
                oFabricYarnDeliveryOrderDetail.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricYarnDeliveryOrderDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetChallanInfoByFYDO(FabricYarnDeliveryOrder oFabricYarnDeliveryOrder)
        {
            FabricYarnDeliveryChallan oFYDChallan = new FabricYarnDeliveryChallan();
            try
            {
                if (oFabricYarnDeliveryOrder.FYDOID <= 0) { throw new Exception("Please select an valid item."); }
                oFabricYarnDeliveryOrder = FabricYarnDeliveryOrder.Get(oFabricYarnDeliveryOrder.FYDOID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oFabricYarnDeliveryOrder.FYDOID > 0 && oFabricYarnDeliveryOrder.OrderQty > oFabricYarnDeliveryOrder.ChallanQty)
                {


                    oFYDChallan.FYDNo = oFabricYarnDeliveryOrder.FYDNo;
                    oFYDChallan.FYDOID = oFabricYarnDeliveryOrder.FYDOID;

                    List<FabricYarnDeliveryOrderDetail> oFYDODetails = new List<FabricYarnDeliveryOrderDetail>();
                    string sSQL = "Select * from View_FabricYarnDeliveryOrderDetail FYDOD Where FYDOD.FYDOID=" + oFabricYarnDeliveryOrder.FYDOID + " And FYDOD.Qty > FYDOD.ChallanQty";
                    oFYDODetails = FabricYarnDeliveryOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oFYDODetails.Any() && oFYDODetails.First().FYDODetailID > 0)
                    {
                        oFYDODetails.ForEach(x =>
                        {
                            oFYDChallan.FYDCDetails.Add(
                               new FabricYarnDeliveryChallanDetail
                               {
                                   FYDCDetailID = 0,
                                   FYDChallanID = 0,
                                   FYDODetailID = x.FYDODetailID,
                                   Qty = x.RemainingQty,
                                   LotID = 0,
                                   LotNo = string.Empty,
                                   Balance = 0,
                                   ProductName = x.ProductName,
                                   ProductCode = x.ProductCode,
                                   ProductID = x.ProductID,
                                   Remarks = string.Empty,
                                   BagQty=""
                               }
                             );
                        });
                    }
                   
                }
                else
                {
                    throw new Exception("No remaining order qty found to make challan.");
                }
            }
            catch (Exception ex)
            {
                oFYDChallan = new FabricYarnDeliveryChallan();
                oFYDChallan.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFYDChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsProduct(FabricYarnOrder oFabricYarnOrder)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                string sSQL = "Select * from View_Product Where ProductID Not In (Select ProductID from SUDeliveryOrderDetail SUD Where SUD.SUDeliveryOrderID In (Select SUDeliveryOrderID from SUDeliveryOrder Where FEOID=" + oFabricYarnOrder.FEOID + ") AND SUD.Qty>(SELECT ISNULL(SUM(Qty),0) FROM FabricSalesContractDetail WHERE FEOID =" + oFabricYarnOrder.FEOID + "AND ProductID=SUD.ProductID))";

                if (!string.IsNullOrEmpty(oFabricYarnOrder.ProductName) && oFabricYarnOrder.ProductName.Trim() != "")
                    sSQL += " And (ProductName Like '%" + oFabricYarnOrder.ProductName.Trim() + "%' OR ProductCode Like '%" + oFabricYarnOrder.ProductName.Trim() + "%')";


                string sDBObjectName = oFabricYarnOrder.Params.Split('~')[0];
                int nTriggerParentsType = Convert.ToInt32(oFabricYarnOrder.Params.Split('~')[1]);
                int nOperationalEvent = Convert.ToInt32(oFabricYarnOrder.Params.Split('~')[2]);
                int nInOutType = Convert.ToInt32(oFabricYarnOrder.Params.Split('~')[3]);
                int nDirections = Convert.ToInt32(oFabricYarnOrder.Params.Split('~')[4]);
                int nStoreID = Convert.ToInt32(oFabricYarnOrder.Params.Split('~')[5]);
                int nMapStoreID = Convert.ToInt32(oFabricYarnOrder.Params.Split('~')[6]);
                sSQL = sSQL + " and ProductCategoryID in ( select ProductCategoryID from [dbo].[fn_GetProductCategoryByMTR] ( '" + sDBObjectName + "', " + nOperationalEvent + ",  " + nTriggerParentsType + ",  " + nInOutType + ",  " + nDirections + ", " + nStoreID + ", " + nMapStoreID + ", " + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")) order by ProductCategoryID";

                oProducts = Product.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception e)
            {
                oProducts = new List<Product>();
                Product oProduct = new Product();
                oProduct.ErrorMessage = e.Message;
                oProducts.Add(oProduct);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintFabricYarnDeliveryOrder(int nFYDOID, bool bPrintFormat, bool bIsInText, double nts)
        {
            string sSQL = "";
            FabricSalesContract oFEO = new FabricSalesContract();
            FabricYarnDeliveryOrder oFYDO = new FabricYarnDeliveryOrder();
            List<FabricYarnDeliveryOrderDetail> oFYDODetails = new List<FabricYarnDeliveryOrderDetail>();
            System.Drawing.Image signature = null;
            UserImage oUserImage = new UserImage();


            oFYDO = FabricYarnDeliveryOrder.Get(nFYDOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            sSQL = "Select * FROM View_FabricYarnDeliveryOrderDetail Where FYDOID =" + oFYDO.FYDOID;
            oFYDO.FYDODetails = FabricYarnDeliveryOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //oFEO = FabricSalesContract.Get(oFYDO.FEOID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<FabricYarnDeliveryChallanDetail> oFYDChallanDetails = new List<FabricYarnDeliveryChallanDetail>();
            sSQL = "Select * from View_FabricYarnDeliveryChallanDetail Where FYDChallanID In (Select FYDChallanID from FabricYarnDeliveryChallan Where FYDOID=" + oFYDO.FYDOID + " And ISNULL(DisburseBy,0)<>0)";
            oFYDChallanDetails = FabricYarnDeliveryChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oUserImage = oUserImage.GetbyUser(oFYDO.ApproveBy, (int)EnumUserImageType.Signature, ((User)Session[SessionInfo.CurrentUser]).UserID);
            signature = GetSignature(oUserImage);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oCompany.CompanyLogo = this.GetCompanyLogo(oCompany);
            oCompany.CompanyTitle = this.GetCompanyTitle(oCompany);
            rptFabricYarnDeliveryOrder oReport = new rptFabricYarnDeliveryOrder();
            byte[] abytes = oReport.PrepareReport(oFEO, oFYDO, oFYDChallanDetails, oCompany, signature, bPrintFormat, bIsInText);
            return File(abytes, "application/pdf");

        }

        public Image GetSignature(UserImage oUserImage)
        {
            if (oUserImage.ImageFile != null)
            {
                MemoryStream m = new MemoryStream(oUserImage.ImageFile);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(this.ControllerContext.HttpContext.Server.MapPath("~/Content/") + "SignatureImage.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

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

        public Image GetCompanyTitle(Company oCompany)
        {
            if (oCompany.OrganizationTitle != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationTitle);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(this.ControllerContext.HttpContext.Server.MapPath("~/Content/") + "CompanyImageTitle.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }


        //[HttpPost]
        //public ActionResult PrintFabricYarnDeliveryComplete(FormCollection DataCollection)
        //{
        //    SUDeliveryChallan _oSUDeliveryChallan = new SUDeliveryChallan();
        //    List<SUReturnChallan> oSUReturnChallans = new List<SUReturnChallan>();
        //    List<SUDeliveryOrder> oSUDeliveryOrders = new List<SUDeliveryOrder>();
        //    _oSUDeliveryChallan.SUDeliveryChallans = new JavaScriptSerializer().Deserialize<List<SUDeliveryChallan>>(DataCollection["txtDeliveryComplete"]);

        //    string sSUDOIDs = string.Join(",", _oSUDeliveryChallan.SUDeliveryChallans.Select(o => o.SUDeliveryOrderID).Distinct());
        //    string sSQL = "";
        //    if (!string.IsNullOrEmpty(sSUDOIDs))
        //    {
        //        _oSUDeliveryChallan.SUDeliveryChallans = new List<SUDeliveryChallan>();
        //        sSQL = "SELECT * FROM View_SUDeliveryChallan WHERE SUDeliveryOrderID IN (" + sSUDOIDs + ") Order By SUDeliveryChallanID";
        //        _oSUDeliveryChallan.SUDeliveryChallans = SUDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //        sSQL = "SELECT * FROM View_SUDeliveryOrder WHERE SUDeliveryOrderID IN (" + sSUDOIDs + ")";
        //        oSUDeliveryOrders = SUDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //        if (_oSUDeliveryChallan.SUDeliveryChallans.Count > 0)
        //        {
        //            string sSUDOChallanIDs = string.Join(",", _oSUDeliveryChallan.SUDeliveryChallans.Select(x => x.SUDeliveryChallanID).Distinct());
        //            if (!string.IsNullOrEmpty(sSUDOChallanIDs))
        //            {
        //                sSQL = "SELECT * FROM View_SUReturnChallan WHERE SUDeliveryChallanID IN (" + sSUDOChallanIDs + ")";
        //                oSUReturnChallans = SUReturnChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //            }
        //        }

        //    }

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    //oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    _oSUDeliveryChallan.Company = oCompany;
        //    string Messge = "Delivery Statement";
        //    rptSUDCDeliveryComplete oReport = new rptSUDCDeliveryComplete();
        //    byte[] abytes = oReport.PrepareReport(_oSUDeliveryChallan, oSUDeliveryOrders, oSUReturnChallans, Messge);
        //    return File(abytes, "application/pdf");
        //}
       

        #endregion
    }
}