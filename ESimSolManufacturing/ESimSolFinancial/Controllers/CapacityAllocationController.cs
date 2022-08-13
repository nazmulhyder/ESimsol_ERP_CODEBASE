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
using System.Dynamic;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
namespace ESimSolFinancial.Controllers
{
	public class CapacityAllocationController : Controller
	{
		#region Declaration
		CapacityAllocation _oCapacityAllocation = new CapacityAllocation();
		List<CapacityAllocation> _oCapacityAllocations = new  List<CapacityAllocation>();
        FPMReportSetup _oFPMReportSetup = new FPMReportSetup();
        List<FPMReportSetup> _oFPMReportSetups = new List<FPMReportSetup>();
        ProductionExecutionPlanDetail _oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
		#endregion

        #region Capacity Allocation Actions
        public ActionResult ViewCapacityAllocations(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
            //this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser("'CapacityAllocation'", (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oCapacityAllocations = new List<CapacityAllocation>(); 
			_oCapacityAllocations = CapacityAllocation.Gets((int)Session[SessionInfo.currentUserID]);
			return View(_oCapacityAllocations);
		}
		public ActionResult ViewCapacityAllocation(int id)
		{
			_oCapacityAllocation = new CapacityAllocation();
			if (id > 0)
			{
				_oCapacityAllocation = _oCapacityAllocation.Get(id, (int)Session[SessionInfo.currentUserID]);
			}
            ViewBag.MUnits = MeasurementUnit.Gets((int)Session[SessionInfo.currentUserID]);
			return View(_oCapacityAllocation);
		}
		[HttpPost]
		public JsonResult Save(CapacityAllocation oCapacityAllocation)
		{
			_oCapacityAllocation = new CapacityAllocation();
			try
			{
				_oCapacityAllocation = oCapacityAllocation;
				_oCapacityAllocation = _oCapacityAllocation.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oCapacityAllocation = new CapacityAllocation();
				_oCapacityAllocation.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oCapacityAllocation);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		} 
		[HttpGet]
		public JsonResult Delete(int id)
		{
			string sFeedBackMessage = "";
			try
			{
				CapacityAllocation oCapacityAllocation = new CapacityAllocation();
				sFeedBackMessage = oCapacityAllocation.Delete(id, (int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				sFeedBackMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(sFeedBackMessage);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		} 
		#endregion

        #region FPMgt Report Setup Actions
        public ActionResult ViewFPMReportSetups(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFPMReportSetups = new List<FPMReportSetup>();
            _oFPMReportSetups = FPMReportSetup.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.SetUpTypes = EnumObject.jGets(typeof(EnumFPReportSetUpType));
            ViewBag.SubSetUps = EnumObject.jGets(typeof(EnumFPReportSubSetup));
            return View(_oFPMReportSetups);
        }

        [HttpPost]
        public JsonResult GetSubSetUpList(FPMReportSetup oFPMReportSetup)
        {
            EnumObject oProcess = new EnumObject();
            List<EnumObject> oProcessList = new List<EnumObject>();
            try
            {
                EnumFPReportSetUpType eOperationType = (EnumFPReportSetUpType)oFPMReportSetup.SetUpTypeInt;
                List<EnumObject> oEnumObjects = EnumObject.jGets(typeof(EnumFPReportSubSetup));
                foreach (EnumObject oItem in oEnumObjects)
                {
                    switch (eOperationType)
                    {
                        case EnumFPReportSetUpType.Short_Term_Loan:
                            if ((EnumFPReportSubSetup)oItem.id == EnumFPReportSubSetup.Packing_Credit || (EnumFPReportSubSetup)oItem.id == EnumFPReportSubSetup.Cash_Incentive || (EnumFPReportSubSetup)oItem.id == EnumFPReportSubSetup.LATR_OR_EDF|| (EnumFPReportSubSetup)oItem.id == EnumFPReportSubSetup.Cash_Credit || (EnumFPReportSubSetup)oItem.id == EnumFPReportSubSetup.FDBP_OR_LDBP )
                            {
                                oProcessList.Add(oItem);
                            }
                            break;

                        case EnumFPReportSetUpType.Long_Term_Loan:
                            if ((EnumFPReportSubSetup)oItem.id == EnumFPReportSubSetup.LTL_Local|| (EnumFPReportSubSetup)oItem.id == EnumFPReportSubSetup.LTL_OBU)
                            {
                                oProcessList.Add(oItem);
                            }
                            break;

                       
                    }
                }
            }
            catch (Exception ex)
            {
                oProcess = new EnumObject(); ;
                oProcess.Value = ex.Message;
                oProcessList.Add(oProcess);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProcessList);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 

        [HttpPost]
        public JsonResult SaveFPMReportSetup(FPMReportSetup oFPMReportSetup)
        {
            _oFPMReportSetup = new FPMReportSetup();
            try
            {
                _oFPMReportSetup = oFPMReportSetup;
                _oFPMReportSetup = _oFPMReportSetup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFPMReportSetup = new FPMReportSetup();
                _oFPMReportSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFPMReportSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFPMReportSetup(FPMReportSetup oFPMReportSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFPMReportSetup.Delete(oFPMReportSetup.FPMReportSetupID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetAccountHeads(FPMReportSetup oFPMReportSetup)
        {
            List<ChartsOfAccount> oCOAs = new List<ChartsOfAccount>();
            List<EnumObject> oProcessList = new List<EnumObject>();
            try
            {
                EnumFPReportSetUpType eOperationType = (EnumFPReportSetUpType)oFPMReportSetup.SetUpTypeInt;
                string sSQL = "SELECT * FROM View_ChartsOfAccount Where AccountType ="+(int)EnumAccountType.Ledger;
                if(!string.IsNullOrEmpty(oFPMReportSetup.AccountHeadName))
                {
                    sSQL += " AND AccountCode+AccountHeadName LIKE '%"+oFPMReportSetup.AccountHeadName+"%'";//Assets
                }
                if (eOperationType == EnumFPReportSetUpType.Account_Reciveable || eOperationType == EnumFPReportSetUpType.Bill_Reciveable || eOperationType == EnumFPReportSetUpType.FC_Margin_Account || eOperationType == EnumFPReportSetUpType.FC_ERQ_Account || eOperationType == EnumFPReportSetUpType.FDR_Account)
                {
                    sSQL += " AND ComponentID = 2";//Assets
                }
                else
                {
                    sSQL += " AND ComponentID = 3";// Liability = 3,
                }
                oCOAs = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                ChartsOfAccount oCOA = new ChartsOfAccount(); ;
                oCOA.ErrorMessage = ex.Message;
                oCOAs.Add(oCOA);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCOAs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region Print
        public ActionResult PrintFPMgtReport(string sParam)
        {
            
            DateTime PositionDate = (sParam!=null && sParam!="")? Convert.ToDateTime(sParam.Split('~')[0]):DateTime.Today;
            int CurrencyID = (sParam!=null && sParam!="")? Convert.ToInt32(sParam.Split('~')[1]):2;//USD
            bool IsApproved = (sParam!=null && sParam!="")? Convert.ToBoolean(sParam.Split('~')[2]):true;//Approved
            double nConvertionRate =(sParam!=null && sParam!="")?  Convert.ToDouble(sParam.Split('~')[3]):80;//Rate

            FPData oFPData = new FPData();
            List<FPData> oFPDatas = new List<FPData>();
            oFPDatas = FPData.Gets("SELECT TOP 1 * FROM FPDataHistory WHERe FPDate Between DATEADD(month, DATEDIFF(month, 0, '" + PositionDate.ToString("dd MMM yyyy") + "'), 0) AND DATEADD(dd, -DAY(DATEADD(mm, 1, '" + PositionDate.ToString("dd MMM yyyy") + "')), DATEADD(mm, 1, '" + PositionDate.ToString("dd MMM yyyy") + "'))", (int)Session[SessionInfo.currentUserID]);
            if (oFPDatas.Count > 0)
            {
                oFPData = oFPDatas[0];
            }
            FPMgtReport oFPMgtReport = new FPMgtReport();
            oFPMgtReport.FPMgtReports = FPMgtReport.GetsFPMgtReports(PositionDate, CurrencyID, IsApproved, (int)Session[SessionInfo.currentUserID]);

         
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptFPMgtReport oReport = new rptFPMgtReport();
            byte[] abytes = oReport.PrepareReport(oFPMgtReport, PositionDate, nConvertionRate, oFPData, oCompany, CurrencyID);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintBookingStatus(string sParam)
        {

            DateTime dStartDate = (sParam!=null && sParam!="")? Convert.ToDateTime(sParam.Split('~')[0]):DateTime.Today;
            DateTime dEndDate = (sParam != null && sParam != "") ? Convert.ToDateTime(sParam.Split('~')[1]) : DateTime.Today;
            CapacityAllocation oCapacityAllocation = new CapacityAllocation();
            oCapacityAllocation.CapacityAllocations = CapacityAllocation.GetsBookingStatus(dStartDate, dEndDate, (int)Session[SessionInfo.currentUserID]);


            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptBookingStatus oReport = new rptBookingStatus();
            byte[] abytes = oReport.PrepareReport(oCapacityAllocation, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintOrderRecapList(int nBuyerID, DateTime dShipmentDate)
        {
            OrderRecap oOrderRecap = new OrderRecap();
            Contractor oBuyer = new Contractor();
            string sSQL = "SELECT * FROM View_OrderRecap AS HH WHERE HH.BuyerID = " + nBuyerID + " AND   HH.ShipmentDate BETWEEN '" + dShipmentDate.ToString("dd MMM yyyy") + "' AND '" + new DateTime(dShipmentDate.Year, dShipmentDate.Month, 1).AddMonths(1).AddDays(-1).ToString("dd MMM yyyy") + "' Order by BUID , ShipmentDate";
            oOrderRecap.OrderRecapList = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            //oBuyer = oBuyer.Get(nBuyerID, (int)Session[SessionInfo.currentUserID]);
            //string sParam = "Buyer :" + oBuyer.Name + "; Date Range :" + dShipmentDate.ToString("dd MMM yyyy") + " To " + new DateTime(dShipmentDate.Year, dShipmentDate.Month, 1).AddMonths(1).AddDays(-1).ToString("dd MMM yyyy");

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oOrderRecap.Company = oCompany;
            rptOrderRecapList oReport = new rptOrderRecapList();
            byte[] abytes = oReport.PrepareReport(oOrderRecap);
            return File(abytes, "application/pdf");
        }
        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Print in Excel
        public void PrintReportXL(string sParam)
        {
             
            DateTime dStartDate = (sParam != null && sParam != "") ? Convert.ToDateTime(sParam.Split('~')[0]) : DateTime.Today;
            DateTime dEndDate = (sParam != null && sParam != "") ? Convert.ToDateTime(sParam.Split('~')[1]) : DateTime.Today;
            CapacityAllocation oCapacityAllocation = new CapacityAllocation();
            oCapacityAllocation.CapacityAllocations = CapacityAllocation.GetsBookingStatus(dStartDate, dEndDate, (int)Session[SessionInfo.currentUserID]);


            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            #region Export Excel
            int nRowIndex = 2,  nEndRow = 0, nStartCol = 2, nEndCol = 6;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Booking Status");
                sheet.Name = "Booking Status";
                sheet.Column(2).Width = 20;//Month
                sheet.Column(3).Width = 30;//Buyer
                sheet.Column(4).Width = 20;//Allocated Qty
                sheet.Column(5).Width = 20;//Order/Booking Qty
                sheet.Column(6).Width = 20;//Total Value
                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Header Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Booking Status"; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Header

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Month"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Buyer"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Allocated Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 5]; cell.Value =  "Order/Booking Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Total Value"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #region Data
                DateTime dShipmentDate = DateTime.MinValue, dFirstMonthDate = DateTime.MinValue; int nItemCount = 0, nMonthCount = 0;
                foreach (CapacityAllocation oItem in oCapacityAllocation.CapacityAllocations)
                {

                    if (dShipmentDate.Month != oItem.ShipmentDate.Month || dShipmentDate == DateTime.MinValue)
                    {
                        
                        nItemCount = oCapacityAllocation.CapacityAllocations.Where(x => x.ShipmentDate.Year == oItem.ShipmentDate.Year && x.ShipmentDate.Month == oItem.ShipmentDate.Month).Count();
                        cell = sheet.Cells[nRowIndex, 2, (nRowIndex + 1 + oCapacityAllocation.CapacityAllocations.Where(x => x.ShipmentDate.Year == oItem.ShipmentDate.Year && x.ShipmentDate.Month == oItem.ShipmentDate.Month).Count() - 1), 2];
                        cell.Value = oItem.ShipmentDateSt; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        dShipmentDate = oItem.ShipmentDate;
                        if(nMonthCount==0)
                        {
                            dFirstMonthDate = oItem.ShipmentDate;

                        }
                        nMonthCount++;
                    }
                    
                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "" + oItem.BuyerName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.Quantity; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.OrderValue; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nItemCount--;
                    if(nItemCount==0)
                    {
                        nRowIndex++;
                        #region Total
                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oCapacityAllocation.CapacityAllocations.Where(x => x.ShipmentDate == dShipmentDate).Sum(x=>x.Quantity); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Font.Bold = true;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oCapacityAllocation.CapacityAllocations.Where(x => x.ShipmentDate == dShipmentDate).Sum(x => x.OrderQty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Font.Bold = true;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oCapacityAllocation.CapacityAllocations.Where(x => x.ShipmentDate == dShipmentDate).Sum(x => x.OrderValue); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Font.Bold = true;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        #endregion
                    }
                    if (nMonthCount % 3 == 0 && nItemCount == 0)
                    {
                        nMonthCount = 0;//reset month
                        nRowIndex++;
                        #region  Preodic summary
                        #region 1st Row
                        nRowIndex++;
                        cell = sheet.Cells[nRowIndex, 3, nRowIndex + 2, 3]; cell.Value = "Periodic Summery (" + dFirstMonthDate.ToString("MMM yy") + "-" + dShipmentDate.ToString("MMM yy") + ")"; cell.Merge = true; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Total Capacity"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oCapacityAllocation.CapacityAllocations.Where(x=>x.ShipmentDate>=dFirstMonthDate && x.ShipmentDate<=dShipmentDate).Sum(x => x.Quantity); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        #endregion

                        #region 2nd Row
                        nRowIndex++;
                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Total Booking"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oCapacityAllocation.CapacityAllocations.Where(x => x.ShipmentDate >= dFirstMonthDate && x.ShipmentDate <= dShipmentDate).Sum(x => x.OrderQty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        #endregion

                        #region 3rd Row
                        nRowIndex++;
                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Free Capacity"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oCapacityAllocation.CapacityAllocations.Where(x => x.ShipmentDate >= dFirstMonthDate && x.ShipmentDate <= dShipmentDate).Sum(x => x.Quantity) - oCapacityAllocation.CapacityAllocations.Where(x => x.ShipmentDate >= dFirstMonthDate && x.ShipmentDate <= dShipmentDate).Sum(x => x.OrderQty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0);";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        #endregion
                        #endregion
                        nRowIndex++;
                    }
                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                if (nMonthCount != 0)
                {
                    #region  Preodic summary
                    #region 1st Row
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 3, nRowIndex + 2, 3]; cell.Value = "Periodic Summery (" + dFirstMonthDate.ToString("MMM yy") + "-" + dShipmentDate.ToString("MMM yy") + ")"; cell.Merge = true; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Total Capacity"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oCapacityAllocation.CapacityAllocations.Where(x => x.ShipmentDate >= dFirstMonthDate && x.ShipmentDate <= dShipmentDate).Sum(x => x.Quantity); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion

                    #region 2nd Row
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Total Booking"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oCapacityAllocation.CapacityAllocations.Where(x => x.ShipmentDate >= dFirstMonthDate && x.ShipmentDate <= dShipmentDate).Sum(x => x.OrderQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion

                    #region 3rd Row
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Free Capacity"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oCapacityAllocation.CapacityAllocations.Where(x => x.ShipmentDate >= dFirstMonthDate && x.ShipmentDate <= dShipmentDate).Sum(x => x.Quantity) - oCapacityAllocation.CapacityAllocations.Where(x => x.ShipmentDate >= dFirstMonthDate && x.ShipmentDate <= dShipmentDate).Sum(x => x.OrderQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0);";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    nRowIndex++;
                    #endregion
                }

                #region GRand Total
                nRowIndex++;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 4]; cell.Value = oCapacityAllocation.CapacityAllocations.Sum(x => x.Quantity); cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Font.Bold = true;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = oCapacityAllocation.CapacityAllocations.Sum(x => x.OrderQty); cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Font.Bold = true;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = oCapacityAllocation.CapacityAllocations.Sum(x => x.OrderValue); cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Font.Bold = true;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                #endregion

                #region Blank
                nRowIndex++;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion
          
                #region Buyer Wise Summary
                #region Header

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Buyer"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Avg. FOB"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "%"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion
                List<CapacityAllocation> oDistinctBuyers = new List<CapacityAllocation>();
                oDistinctBuyers = oCapacityAllocation.CapacityAllocations.GroupBy(x => x.BuyerID).Select(group => new CapacityAllocation
                {

                    BuyerName = group.First().BuyerName,
                    OrderQty = group.Sum(x => x.OrderQty),
                    OrderValue = group.Sum(x => x.OrderValue)
                }).ToList();

                foreach (CapacityAllocation oItem in oDistinctBuyers)
                {

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.BuyerName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.OrderValue; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = (oItem.OrderValue / oItem.OrderQty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = ((oItem.OrderQty / oDistinctBuyers.Sum(x => x.OrderQty)) * 100); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                }
                #endregion

                #region Total Print
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Total"; cell.Style.Font.Bold = true; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 3]; cell.Value = oDistinctBuyers.Sum(x => x.OrderQty); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = oDistinctBuyers.Sum(x => x.OrderValue); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 5, nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Merge = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=BookingStatus.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion

        }

        public void PrintOrderRecapListInExcel(int nBuyerID, DateTime dShipmentDate)
        {
            List<OrderRecap> oOrderRecaps = new List<OrderRecap>();
            Contractor oBuyer = new Contractor();
            string sSQL = "SELECT * FROM View_OrderRecap AS HH WHERE HH.BuyerID = " + nBuyerID + " AND   HH.ShipmentDate BETWEEN '" + dShipmentDate.ToString("dd MMM yyyy") + "' AND '" + new DateTime(dShipmentDate.Year, dShipmentDate.Month, 1).AddMonths(1).AddDays(-1).ToString("dd MMM yyyy") + "' Order by BUID , ShipmentDate";
            oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            int nUserID = Convert.ToInt32(Session[SessionInfo.currentUserID]);
            
            if (oOrderRecaps.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Order Recap List");
                    sheet.Name = "Order Recap List";
                    //Last changed 23 July 2018 @Request of Jahid 
                    sheet.Column(2).Width = 10; //SL
                    sheet.Column(3).Width = 15; //Buyer Name
                    sheet.Column(4).Width = 15; // Dept/ Gender 
                    sheet.Column(5).Width = 18; //Stytle No
                    sheet.Column(6).Width = 15; //Order No
                    sheet.Column(7).Width = 15; //Fabrics
                    sheet.Column(8).Width = 15; //Quantity
                    sheet.Column(9).Width = 15; //FOB
                    sheet.Column(10).Width = 15; //Amount
                    sheet.Column(11).Width = 18; //Shipment Date
                    sheet.Column(12).Width = 15; //session
                    sheet.Column(13).Width = 15; //BU Name
                    sheet.Column(14).Width = 15; //Class
                    sheet.Column(15).Width = 15; //Sub Class
                    sheet.Column(16).Width = 18; //Wash
                    sheet.Column(17).Width = 18; //Merchandiser
                    sheet.Column(18).Width = 18; //Factory


                    #region Report Header
                    sheet.Cells[rowIndex, 2, rowIndex, 18].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, 18].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Order Recap List"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "Dept/Gender"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "Fabric Code"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = "Quantity"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = "FOB"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, 11]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, 12]; cell.Value = "Session"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, 13]; cell.Value = "BU Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

          
                   cell = sheet.Cells[rowIndex, 14]; cell.Value = "Class"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 15]; cell.Value = "Sub Class"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                  
                    cell = sheet.Cells[rowIndex, 16]; cell.Value = "Wash"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 17]; cell.Value = "Merchandiser"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 18]; cell.Value = "Factory Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    rowIndex = rowIndex + 1;
                    #endregion

                    #region Report Data
                    int nSL = 0;

                    double nTotalOrderQty = 0, nTotalAmount = 0;
                    foreach (OrderRecap oItem in oOrderRecaps)
                    {

                        nSL++;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = nSL.ToString(); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.DeptName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.FabCode; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.TotalQuantity; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.UnitPrice * oItem.TotalQuantity; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex,12]; cell.Value = oItem.SessionName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 13]; cell.Value = oItem.BUShortName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 14]; cell.Value = oItem.ClassName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 15]; cell.Value = oItem.SubClassName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 16]; cell.Value = oItem.Wash; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 17]; cell.Value = oItem.MerchandiserName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 18]; cell.Value = oItem.FactoryName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                   
                        nTotalOrderQty += oItem.TotalQuantity;
                        nTotalAmount += oItem.TotalQuantity * oItem.UnitPrice;
                        rowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = nTotalOrderQty; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = nTotalAmount; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 14]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 16]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 17]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 18]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=OrderRecapList.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }



            }

        }
        #endregion

        #region DeshBoard
        public ActionResult ViewFinancialDeshBoard(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            Company oCompany = new Company();
            _oCapacityAllocation = new CapacityAllocation();
            DateTime PositionDate = DateTime.Now;
            int CurrencyID = 2;
            bool IsApproved = true;
            List<dynamic> oDynamics = new List<dynamic>();
            _oCapacityAllocations = new List<CapacityAllocation>();
            List<CapacityAllocation>  oMonthAllocations = new List<CapacityAllocation>();
            FPMgtReport oFPMgtReport = new FPMgtReport();
            List<PLineConfigure> oPLineConfigures = new List<PLineConfigure>();
            List<ProductionExecutionPlanDetailBreakdown> _oProductionExecutionPlanDetailBreakdowns = new List<ProductionExecutionPlanDetailBreakdown>();

            #region Booking Status
            DateTime firstMonth = DateTime.Today;//First/Running Month
            DateTime lastMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(3).AddDays(-1);//Next 2 Months
            CapacityAllocation oTempCapacityAllocation = new CapacityAllocation();
            oTempCapacityAllocation = Session[SessionInfo.ParamObj]!=null? (CapacityAllocation)Session[SessionInfo.ParamObj]:new CapacityAllocation();
            DateTime dStartDate = (oTempCapacityAllocation.Param != null && oTempCapacityAllocation.Param != "") ? Convert.ToDateTime(oTempCapacityAllocation.Param.Split('~')[0]) : firstMonth;
            DateTime dEndDate = (oTempCapacityAllocation.Param != null && oTempCapacityAllocation.Param != "") ? Convert.ToDateTime(oTempCapacityAllocation.Param.Split('~')[1]) : lastMonth;
            this.Session.Remove(SessionInfo.ParamObj);
            ViewBag.BookingDateRange = dStartDate.ToString("dd MMM yyyyy") + '~' + dEndDate.ToString("dd MMM yyyyy");
            _oCapacityAllocations = CapacityAllocation.GetsBookingStatus(dStartDate, dEndDate, (int)Session[SessionInfo.currentUserID]);
            oMonthAllocations = _oCapacityAllocations.GroupBy(x=>x.ShipmentDate.Month).Select(grp=> new CapacityAllocation
            {
                ShipmentDate = grp.First().ShipmentDate,
                MonthWiseCapacity = grp.Sum(y=>y.Quantity),
                MonthWiseBooking = grp.Sum(y=>y.OrderQty),
                MonthWiseFreeCapacity = grp.Sum(y => y.Quantity - y.OrderQty),
                MonthWiseValue = grp.Sum(y=>y.OrderValue)

             }).ToList();

            foreach(CapacityAllocation oItem in oMonthAllocations)
            {
                oItem.MonthWiseDetailsAllocations = _oCapacityAllocations.Where(z => z.ShipmentDate.Month == oItem.ShipmentDate.Month).ToList();
            }
            #endregion

            #region Productoin Plan
            string sSQL = "SELECT * FROM View_BusinessUnit WHERE BUsinessUnitID IN (SELECT BUID FROM OrderRecap)";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            _oProductionExecutionPlanDetail.StartDate = DateTime.Today;
            _oProductionExecutionPlanDetail.EndDate = DateTime.Today.AddMonths(2);
            sSQL = "SELECT * FROM View_PLineConfigure WHERE ProductionUnitType = " + ((int)EnumProductionUnitType.InHouseProduction).ToString() + " ORDER BY ProductionUnitID ASC";
            oPLineConfigures = PLineConfigure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.PLineConfigures = PLineConfigure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            sSQL = "SELECT * FROM  View_ProductionExecutionPlanDetailBreakDown AS HH WHERE MONTH(HH.WorkingDate) BETWEEN MONTH('" + DateTime.Today.ToString("dd MMM yyyy") + "') AND MONTH('" + DateTime.Today.AddMonths(2).ToString("dd MMM yyyy") + "') AND HH.PLineConfigureID IN(" + String.Join(",", oPLineConfigures.Select(x => x.PLineConfigureID)).ToString() + ") Order BY BUID, ProductionUnitID, PLineConfigureID";
            _oProductionExecutionPlanDetailBreakdowns = ProductionExecutionPlanDetailBreakdown.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            oDynamics = GetDynamicList(_oProductionExecutionPlanDetail, oPLineConfigures, _oProductionExecutionPlanDetailBreakdowns);
            ViewBag.PlanDateRange = _oProductionExecutionPlanDetail.StartDate.ToString("dd MMM yyyyy") + '~' + _oProductionExecutionPlanDetail.EndDate.ToString("dd MMM yyyyy");
            #endregion

            ViewBag.ProductionPlanData = oDynamics;
            ViewBag.Allocations = oMonthAllocations;
            FPData oFPData = new FPData();
            ViewBag.FPData = FPData.Gets("SELECT top 1 * FROM FPDataHistory WHERe FPDate Between DATEADD(month, DATEDIFF(month, 0, '" + PositionDate.ToString("dd MMM yyyy") + "'), 0) AND DATEADD(dd, -DAY(DATEADD(mm, 1, '" + PositionDate.ToString("dd MMM yyyy") + "')), DATEADD(mm, 1, '" + PositionDate.ToString("dd MMM yyyy") + "'))", (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
            ViewBag.FPMgtReports = FPMgtReport.GetsFPMgtReports(PositionDate, CurrencyID, IsApproved, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ProductionUnits = ProductionUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Comapny = oCompany.Get(1,(int)Session[SessionInfo.currentUserID]);
            return View(_oCapacityAllocation);
        }

        public ActionResult ViewBookingStatus(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            Company oCompany = new Company();
            _oCapacityAllocation = new CapacityAllocation();
            _oCapacityAllocations = new List<CapacityAllocation>();
            List<CapacityAllocation> oMonthAllocations = new List<CapacityAllocation>();

            #region Booking Status
            DateTime firstMonth = DateTime.Today;//First/Running Month
            DateTime lastMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(3).AddDays(-1);//Next 2 Months
            CapacityAllocation oTempCapacityAllocation = new CapacityAllocation();
            oTempCapacityAllocation = Session[SessionInfo.ParamObj] != null ? (CapacityAllocation)Session[SessionInfo.ParamObj] : new CapacityAllocation();
            DateTime dStartDate = (oTempCapacityAllocation.Param != null && oTempCapacityAllocation.Param != "") ? Convert.ToDateTime(oTempCapacityAllocation.Param.Split('~')[0]) : firstMonth;
            DateTime dEndDate = (oTempCapacityAllocation.Param != null && oTempCapacityAllocation.Param != "") ? Convert.ToDateTime(oTempCapacityAllocation.Param.Split('~')[1]) : lastMonth;
            this.Session.Remove(SessionInfo.ParamObj);
            ViewBag.BookingDateRange = dStartDate.ToString("dd MMM yyyyy") + '~' + dEndDate.ToString("dd MMM yyyyy");
            _oCapacityAllocations = CapacityAllocation.GetsBookingStatus(dStartDate, dEndDate, (int)Session[SessionInfo.currentUserID]);
            oMonthAllocations = _oCapacityAllocations.GroupBy(x => x.ShipmentDate.Month).Select(grp => new CapacityAllocation
            {
                ShipmentDate = grp.First().ShipmentDate,
                MonthWiseCapacity = grp.Sum(y => y.Quantity),
                MonthWiseBooking = grp.Sum(y => y.OrderQty),
                MonthWiseFreeCapacity = grp.Sum(y => y.Quantity - y.OrderQty),
                MonthWiseValue = grp.Sum(y => y.OrderValue)

            }).ToList();

            foreach (CapacityAllocation oItem in oMonthAllocations)
            {
                oItem.MonthWiseDetailsAllocations = _oCapacityAllocations.Where(z => z.ShipmentDate.Month == oItem.ShipmentDate.Month).ToList();
            }
            #endregion
            ViewBag.Allocations = oMonthAllocations;
            return View(_oCapacityAllocation);
        }
        [HttpPost]
        public JsonResult GetPlans(ProductionExecutionPlanDetail oProductionExecutionPlanDetail)
        {
            List<dynamic> oDynamics = new List<dynamic>();
            List<PLineConfigure> oPLineConfigures = new List<PLineConfigure>();
            _oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
            _oProductionExecutionPlanDetail = oProductionExecutionPlanDetail;
            List<ProductionExecutionPlanDetailBreakdown> _oProductionExecutionPlanDetailBreakdowns = new List<ProductionExecutionPlanDetailBreakdown>();
            try
            {
                string sSQL = "SELECT * FROM View_PLineConfigure ", sTemp = "";
                if (oProductionExecutionPlanDetail.ProductionUnitID != 0)
                {
                    Global.TagSQL(ref sTemp);
                    sTemp += " ProductionUnitID  = " + oProductionExecutionPlanDetail.ProductionUnitID;
                }
                else if (oProductionExecutionPlanDetail.BUID != 0 )
                {
                    sSQL = "SELECT * FROM View_PLineConfigure WHERE ProductionUnitType = " + (int)EnumProductionUnitType.InHouseProduction + " AND RefID ="+oProductionExecutionPlanDetail.BUID;
                    //sTemp = " OR  PLineConfigureID  IN (SELECT HH.PLineConfigureID FROM  View_ProductionExecutionPlanDetailBreakDown AS HH WHERE  HH.WorkingDate Between '" + oProductionExecutionPlanDetail.StartDate.ToString("dd MMM yyyy") + "' AND '" + oProductionExecutionPlanDetail.EndDate.ToString("dd MMM yyyy") + "')";
                }
                else
                {
                    sSQL = "SELECT * FROM View_PLineConfigure WHERE ProductionUnitType = " + (int)EnumProductionUnitType.InHouseProduction;
                }
                sSQL += sTemp + " ORDER BY PLineConfigureID ASC";
                oPLineConfigures = PLineConfigure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                _oProductionExecutionPlanDetail.PLineConfigures = oPLineConfigures;

                sSQL = "SELECT * FROM  View_ProductionExecutionPlanDetailBreakDown AS HH WHERE MONTH(HH.WorkingDate) BETWEEN MONTH('" + _oProductionExecutionPlanDetail.StartDate.ToString("dd MMM yyyy") + "') AND MONTH('" + _oProductionExecutionPlanDetail.EndDate.ToString("dd MMM yyyy") + "') AND HH.PLineConfigureID IN(" + String.Join(",", oPLineConfigures.Select(x => x.PLineConfigureID)).ToString() + ")";
                if (oProductionExecutionPlanDetail.BUID != 0)
                {
                    sSQL += " AND BUID =" + oProductionExecutionPlanDetail.BUID;
                }
                sSQL += " Order BY BUID, ProductionUnitID, PLineConfigureID";
                _oProductionExecutionPlanDetailBreakdowns = ProductionExecutionPlanDetailBreakdown.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                _oProductionExecutionPlanDetail.Dynamics = GetDynamicList(_oProductionExecutionPlanDetail, oPLineConfigures, _oProductionExecutionPlanDetailBreakdowns);

            }
            catch (Exception ex)
            {
                _oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
                _oProductionExecutionPlanDetail.ErrorMessage = ex.Message;
            }

            var jsonResult = Json(_oProductionExecutionPlanDetail, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private List<dynamic> GetDynamicList(ProductionExecutionPlanDetail oProductionExecutionPlanDetail, List<PLineConfigure> oPLineConfigures, List<ProductionExecutionPlanDetailBreakdown> oProductionExecutionPlanDetailBreakdowns)
        {
            List<dynamic> oDynamiObjects = new List<dynamic>();
            DateTime dStartDate = oProductionExecutionPlanDetail.StartDate;
            DateTime dEndDate = oProductionExecutionPlanDetail.EndDate;
            int months = ((dEndDate.Year - dStartDate.Year) * 12 + dEndDate.Month - dStartDate.Month) +1;
            DateTime dTempEndDate = new DateTime(dStartDate.Year, dStartDate.Month, 1).AddMonths(1).AddDays(-1);
            if (dTempEndDate > dEndDate) { dTempEndDate = dEndDate; }
            while (months!= 0)
            {
                dynamic obj = new ExpandoObject();
                var expobj = obj as IDictionary<string, object>;
                
                string sTable = "<table border='0' style='width:100%;height:100%;' cellpadding='0' cellspacing='0'> <tr>";
                sTable += "<td style='height:90px;width:100px;'>" + dStartDate.ToString("MMM-yy") + "</td></tr></table>";
                expobj.Add("DateField", sTable);
                foreach (PLineConfigure oItem in oPLineConfigures)
                {
                    expobj.Add("Line"+ oItem.PLineConfigureID, GetOrderInfo(dStartDate, dTempEndDate, oItem.PLineConfigureID, oItem.ProductionUnitID, oItem.RefID,  oProductionExecutionPlanDetailBreakdowns));
                }
                oDynamiObjects.Add(expobj);
                dStartDate = new DateTime(dStartDate.AddMonths(1).Year, dStartDate.AddMonths(1).Month, 1);
                dTempEndDate = dStartDate.AddMonths(1).AddDays(-1);
                if (dTempEndDate > dEndDate)
                {
                    dTempEndDate = dEndDate;
                }
                //dStartDate = dStartDate.AddMonths(1);
                months--;
            }
            return oDynamiObjects;
        }

        private string GetOrderInfo(DateTime dTempDate, DateTime dTempEndDate, int PLineConfigureID, int ProductionUnitID, int BUID, List<ProductionExecutionPlanDetailBreakdown> oProductionExecutionPlanDetailBreakdowns)
        {
            DateTime firstDayOfMonth = dTempDate;// new DateTime(dTempDate.Year, dTempDate.Month, 1);
            DateTime lastDayOfMonth = dTempEndDate;// firstDayOfMonth.AddMonths(1).AddDays(-1);
            string sDiv = "<div style='width:100%;height:90px;margin:0px;background-color:#f34430;vertical-align: top;outline:none' id='container" + PLineConfigureID + "'> ";
            int nHeightCount = 1, nHeightForBalank = 1, nTempDaysOfMonth = (int)(lastDayOfMonth - firstDayOfMonth).TotalDays + 1; /*DateTime.DaysInMonth(firstDayOfMonth.Year, firstDayOfMonth.Month);*/ string sColorName = "#f34430";//default Red (Blank), #eee63f(yellow) for Partialy , #0B3B17(Green) for full

            while(firstDayOfMonth!=lastDayOfMonth)
            {
                
                if(oProductionExecutionPlanDetailBreakdowns.Where(x=>x.PLineConfigureID==PLineConfigureID && x.ProductionUnitID==ProductionUnitID && x.BUID==BUID  && x.WorkingDate==firstDayOfMonth).Count()>0)
                {
                   if (firstDayOfMonth.AddDays(1)!= lastDayOfMonth && oProductionExecutionPlanDetailBreakdowns.Where(x => x.PLineConfigureID == PLineConfigureID && x.ProductionUnitID==ProductionUnitID && x.BUID==BUID && x.WorkingDate == firstDayOfMonth.AddDays(1)).Count() <= 0)
                   {
                       if(nHeightCount==nTempDaysOfMonth)
                       {
                           sColorName = "#0B3B17";//Green for fulll 
                       }
                       else
                       {
                           sColorName = "#eee63f";//Yellow for Partially
                       }
                       sDiv += "<div style='width:100%;height:" + ((100 * nHeightCount) / nTempDaysOfMonth) + "%;background-color:" + sColorName + "' id='tempcontainer" + PLineConfigureID + "-" + nHeightCount + "'>&nbsp;&nbsp;</div>";
                       nHeightCount = 0;
                       nHeightForBalank = 1;
                   }
                }
                else
                {
                    if (firstDayOfMonth.AddDays(1) != lastDayOfMonth && oProductionExecutionPlanDetailBreakdowns.Where(x => x.PLineConfigureID == PLineConfigureID && x.ProductionUnitID == ProductionUnitID && x.BUID == BUID && x.WorkingDate == firstDayOfMonth.AddDays(1)).Count()>0)
                    {
                        sColorName = "#f34430";// for Red 
                        sDiv += "<div style='width:100%;height:" + ((100 * nHeightForBalank) / nTempDaysOfMonth) + "%;background-color:" + sColorName + "' id='tempcontainer" + PLineConfigureID + "-" + nHeightCount + "'>&nbsp;&nbsp;</div>";
                        nHeightForBalank = 0;
                    }
                    else
                    {
                        nHeightForBalank++;
                        nHeightCount = 0;
                    }                 
                }
                nHeightCount++;
                firstDayOfMonth = firstDayOfMonth.AddDays(1);
            }
            if (nHeightForBalank > 1)
            {
                sColorName = "#f34430";//Green for Red 
                sDiv += "<div style='width:100%;height:" + ((100 * nHeightForBalank) / nTempDaysOfMonth) + "%;background-color:" + sColorName + "' id='tempcontainer" + PLineConfigureID + "-" + nHeightCount + "'>&nbsp;&nbsp;</div>";
            }
            if(nHeightCount>1)
            {
                if (nHeightCount == nTempDaysOfMonth)
                {
                    sColorName = "#0B3B17";//Green for fulll 
                }
                else
                {
                    sColorName = "#eee63f";//Yellow for Partially
                }
                sDiv += "<div style='width:100%;height:" + ((100 * nHeightCount) / nTempDaysOfMonth) + "%;background-color:" + sColorName + "' id='tempcontainer" + PLineConfigureID + "-" + nHeightCount + "'>&nbsp;&nbsp;</div>";
            }
          
            sDiv += "</div>";
          
            return sDiv;
        }


        [HttpPost]
        public ActionResult GetAccountsWithFinance(FPMgtReport oFPMgtReport)
        {

            DateTime PositionDate = Convert.ToDateTime(oFPMgtReport.Param.Split('~')[0]);
            int CurrencyID = Convert.ToInt32(oFPMgtReport.Param.Split('~')[1]);
            bool IsApproved = Convert.ToBoolean(oFPMgtReport.Param.Split('~')[2]);
            double nConvertionRate = Convert.ToDouble(oFPMgtReport.Param.Split('~')[3]);
            
            FPData oFPData = new FPData();
            oFPMgtReport.FPData = FPData.Gets("SELECT top 1 * FROM FPDataHistory WHERe FPDate Between DATEADD(month, DATEDIFF(month, 0, '" + PositionDate.ToString("dd MMM yyyy") + "'), 0) AND DATEADD(dd, -DAY(DATEADD(mm, 1, '" + PositionDate.ToString("dd MMM yyyy") + "')), DATEADD(mm, 1, '" + PositionDate.ToString("dd MMM yyyy") + "'))", (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
            List<FPMgtReport>  oFPMgtReports = new List<FPMgtReport>();
            oFPMgtReport.FPMgtReports = FPMgtReport.GetsFPMgtReports(PositionDate, CurrencyID, IsApproved, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFPMgtReport);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetSessionData(CapacityAllocation oCapacityAllocation)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oCapacityAllocation);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
