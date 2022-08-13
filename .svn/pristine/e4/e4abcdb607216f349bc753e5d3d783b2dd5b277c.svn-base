using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using ICS.Core.Utility;
using System.Dynamic;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class ProductionTimeSetupController : Controller
    {
        #region Declaration
        ProductionTimeSetup _oProductionTimeSetup = new ProductionTimeSetup();
        List<ProductionTimeSetup> _oProductionTimeSetups = new List<ProductionTimeSetup>();
        ProductionExecutionPlanDetail _oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
        List<ProductionExecutionPlanDetail> _oProductionExecutionPlanDetails = new List<ProductionExecutionPlanDetail>();
        List<PLineConfigure> _oPLineConfigures = new List<PLineConfigure>();
        #endregion

        #region ProductionTimeSetup Actions
        public ActionResult ViewProductionTimeSetup(int menuid)
        {
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ProductionTimeSetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oProductionTimeSetups = new List<ProductionTimeSetup>();
            _oProductionTimeSetups = ProductionTimeSetup.Gets((int)Session[SessionInfo.currentUserID]);
            @ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            @ViewBag.WeekDays = EnumObject.jGets(typeof(EnumWeekDays));
            return View(_oProductionTimeSetups);
        }

        [HttpPost]
        public JsonResult Save(ProductionTimeSetup oProductionTimeSetup)
        {
            _oProductionTimeSetup = new ProductionTimeSetup();
            try
            {
                _oProductionTimeSetup = oProductionTimeSetup;
                _oProductionTimeSetup = _oProductionTimeSetup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionTimeSetup = new ProductionTimeSetup();
                _oProductionTimeSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionTimeSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(ProductionTimeSetup oProductionTimeSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oProductionTimeSetup.Delete(oProductionTimeSetup.ProductionTimeSetupID, (int)Session[SessionInfo.currentUserID]);
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

        #region Produciotn Plan
        public ActionResult ViewProductionPlan(int menuid, int OrderRecapID)
        {
            List<dynamic> oDynamics = new List<dynamic>();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oProductionTimeSetup = new ProductionTimeSetup();
            string sStartDate = DateTime.Today.ToString("dd MMM yyyy"), sEndDate = DateTime.Today.ToString("dd MMM yyyy");
            string sSQL = "SELECT * FROM View_BusinessUnit WHERE BUsinessUnitID IN (SELECT BUID FROM OrderRecap)";
            @ViewBag.BusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            @ViewBag.ProductionUnits = ProductionUnit.Gets((int)Session[SessionInfo.currentUserID]);
            if (OrderRecapID != 0)
            {
                List<ProductionExecutionPlanDetail> oProductionExecutionPlanDetails = new List<ProductionExecutionPlanDetail>();
                oProductionExecutionPlanDetails = ProductionExecutionPlanDetail.Gets("SELECT Min(StartDate) AS StartDate, Max(EndDate) AS EndDate FROM ProductionExecutionPlanDetail WHERE ProductionExecutionPlanID IN ( SELECT ProductionExecutionPlanID FROM ProductionExecutionPlan WHERE OrderRecapID = " + OrderRecapID + ")", (int)Session[SessionInfo.currentUserID]);
                sStartDate = oProductionExecutionPlanDetails[0].StartDateInString;
                sEndDate = oProductionExecutionPlanDetails[0].EndDateInString;
            }
            else
            {
                _oPLineConfigures = new List<PLineConfigure>();
                _oProductionExecutionPlanDetails = new List<ProductionExecutionPlanDetail>();
                _oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
                List<ProductionExecutionPlanDetailBreakdown> _oProductionExecutionPlanDetailBreakdowns = new List<ProductionExecutionPlanDetailBreakdown>();
                
                _oProductionExecutionPlanDetail.StartDate = DateTime.Today;
                _oProductionExecutionPlanDetail.EndDate = DateTime.Today.AddDays(60);

                sSQL = "SELECT * FROM View_PLineConfigure WHERE ProductionUnitType = " + ((int)EnumProductionUnitType.InHouseProduction).ToString() + " ORDER BY ProductionUnitID ASC";
                _oPLineConfigures = PLineConfigure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sSQL = "SELECT * FROM  View_ProductionExecutionPlanDetailBreakDown AS HH WHERE CONVERT(DATE,CONVERT(VARCHAR(12),HH.WorkingDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + _oProductionExecutionPlanDetail.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + _oProductionExecutionPlanDetail.EndDate.ToString("dd MMM yyyy") + "',106)) AND HH.PLineConfigureID IN(" + String.Join(",", _oPLineConfigures.Select(x => x.PLineConfigureID)).ToString() + ") Order BY BUID, ProductionUnitID, PLineConfigureID";
                _oProductionExecutionPlanDetailBreakdowns = ProductionExecutionPlanDetailBreakdown.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oDynamics = GetDynamicList(_oProductionExecutionPlanDetail, _oPLineConfigures, _oProductionExecutionPlanDetailBreakdowns);

                sStartDate = _oProductionExecutionPlanDetail.StartDateInString;
                sEndDate = _oProductionExecutionPlanDetail.EndDateInString;
            }
            @ViewBag.ProductionPlanData = oDynamics;
            @ViewBag.OrderRecapInfo = OrderRecapID + "~" + sStartDate + "~" + sEndDate;
            return View(_oProductionTimeSetup);
        }

        [HttpPost]
        public JsonResult GetPlans(ProductionExecutionPlanDetail oProductionExecutionPlanDetail)
        {
            List<dynamic> oDynamics = new List<dynamic>();
            _oProductionExecutionPlanDetails = new List<ProductionExecutionPlanDetail>();
            _oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
            List<ProductionExecutionPlanDetailBreakdown> _oProductionExecutionPlanDetailBreakdowns = new List<ProductionExecutionPlanDetailBreakdown>();
            try
            {
                string sSQL = "SELECT * FROM View_PLineConfigure ", sTemp = "";
                string sBuyerIDs = oProductionExecutionPlanDetail.Params.Split('~')[0]; string sOrderRecapIDs = oProductionExecutionPlanDetail.Params.Split('~')[1];
                if (oProductionExecutionPlanDetail.ProductionUnitID != 0)
                {
                    Global.TagSQL(ref sTemp);
                    sTemp += " ProductionUnitID  = " + oProductionExecutionPlanDetail.ProductionUnitID;
                }
                else if (oProductionExecutionPlanDetail.BUID != 0 || !string.IsNullOrEmpty(sBuyerIDs) || !string.IsNullOrEmpty(sOrderRecapIDs))
                {
                    sSQL = "SELECT * FROM View_PLineConfigure WHERE ProductionUnitType = " + (int)EnumProductionUnitType.InHouseProduction;
                    sTemp = " OR  PLineConfigureID  IN (SELECT HH.PLineConfigureID FROM  View_ProductionExecutionPlanDetailBreakDown AS HH WHERE  HH.WorkingDate Between '" + oProductionExecutionPlanDetail.StartDate.ToString("dd MMM yyyy") + "' AND '" + oProductionExecutionPlanDetail.EndDate.ToString("dd MMM yyyy") + "')";
                }
                else
                {
                    sSQL = "SELECT * FROM View_PLineConfigure WHERE ProductionUnitType = " + (int)EnumProductionUnitType.InHouseProduction;
                }
                sSQL += sTemp;
                _oPLineConfigures = PLineConfigure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM  View_ProductionExecutionPlanDetailBreakDown WHERE PLineConfigureID IN(" + String.Join(",", _oPLineConfigures.Select(x => x.PLineConfigureID)).ToString() + ")";
                if (!string.IsNullOrEmpty(sBuyerIDs))
                {
                    sSQL += " AND BuyerID IN (" + sBuyerIDs + ")";
                }
                if (!string.IsNullOrEmpty(sOrderRecapIDs))
                {
                    sSQL += " AND OrderRecapID IN (" + sOrderRecapIDs + ")";
                }
                if (oProductionExecutionPlanDetail.BUID != 0)
                {
                    sSQL += " AND BUID =" + oProductionExecutionPlanDetail.BUID;
                }
                sSQL += " Order BY BUID, ProductionUnitID, PLineConfigureID";
                _oProductionExecutionPlanDetailBreakdowns = ProductionExecutionPlanDetailBreakdown.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oDynamics = GetDynamicList(oProductionExecutionPlanDetail, _oPLineConfigures, _oProductionExecutionPlanDetailBreakdowns);

            }
            catch (Exception ex)
            {
                _oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
                _oProductionExecutionPlanDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDynamics);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private List<dynamic> GetDynamicList(ProductionExecutionPlanDetail oProductionExecutionPlanDetail, List<PLineConfigure> oPLineConfigures, List<ProductionExecutionPlanDetailBreakdown> oProductionExecutionPlanDetailBreakdowns)
        {
            List<dynamic> oDynamiObjects = new List<dynamic>();
            DateTime dStartDate = oProductionExecutionPlanDetail.StartDate;
            DateTime dEndDate = oProductionExecutionPlanDetail.EndDate;
            foreach (PLineConfigure oItem in oPLineConfigures)
            {
                dynamic obj = new ExpandoObject();
                var expobj = obj as IDictionary<string, object>;
                //RefShortName~PUShortName~LineNo
                //expobj.Add("LineNo", oItem.RefShortName+"~"+oItem.PUShortName+"~"+oItem.LineNo);
                string sTable = "<table border='0' style='width:100%;height:100%;' cellpadding='0' cellspacing='0'> <tr>";
                sTable += "<td style='height:80px;width:220px;'>BU Name:" + oItem.RefShortName + "</br>PU :" + oItem.PUShortName + "</br>Line No:" + oItem.LineNo + "</br>Machine Qty: " + oItem.MachineQty.ToString() + "</td>";
                sTable += "</tr></table>";
                expobj.Add("LineNo", sTable);
                int nCount = 1;
                dStartDate = oProductionExecutionPlanDetail.StartDate;
                dEndDate = oProductionExecutionPlanDetail.EndDate;
                while (dStartDate <= dEndDate)
                {
                    expobj.Add("Date" + nCount, GetOrderInfo(dStartDate, oItem.PLineConfigureID, oItem.ProductionUnitID, oItem.RefID,  oProductionExecutionPlanDetailBreakdowns));
                    nCount++;
                    dStartDate = dStartDate.AddDays(1);
                }
                oDynamiObjects.Add(expobj);
            }
            return oDynamiObjects;
        }

        private string GetOrderInfo(DateTime dTempDate, int PLineConfigureID, int ProductionUnitID, int BUID, List<ProductionExecutionPlanDetailBreakdown> oProductionExecutionPlanDetailBreakdowns)
        {
            string sReturn = "'" + dTempDate.ToString("dd MMM yyyy") + "'" + "~" + PLineConfigureID.ToString();
            ProductionExecutionPlanDetailBreakdown oPEPDB = new ProductionExecutionPlanDetailBreakdown();
            oPEPDB = oProductionExecutionPlanDetailBreakdowns.Where(x => x.PLineConfigureID == PLineConfigureID && x.BUID == BUID && x.ProductionUnitID == ProductionUnitID && x.WorkingDate == dTempDate).FirstOrDefault();
            if (oPEPDB != null)
            {
                //StyleNo~RecapNo~BuyerName~DailyProduction~OrderRecapID~PLineConfigureID~ProductionExecutionPlanID~ExecutionQty~PEPDBID
                sReturn = oPEPDB.StyleNo + "~" + oPEPDB.RecapNo + "(" + Global.MillionFormatActualDigit(oPEPDB.RecapQty) + " " + oPEPDB.UnitSymbol + ")" + "~" + oPEPDB.BuyerName + "~" + Global.MillionFormatActualDigit(oPEPDB.DailyProduction) + "~" + oPEPDB.OrderRecapID.ToString() + "~" + oPEPDB.PLineConfigureID.ToString() + "~" + oPEPDB.ProductionExecutionPlanID.ToString() + "~" + Global.MillionFormatActualDigit(oPEPDB.ExecutionQty) + "~" + oPEPDB.ProductionExecutionPlanDetailBreakdownID.ToString() + "~" + "'" + dTempDate.ToString("dd MMM yyyy") + "'";
            }
            return sReturn;
        }

        [HttpPost]
        public JsonResult PastePlans(ProductionExecutionPlanDetailBreakdown oPEPDB)
        {
            string sMessage = "";

            try
            {
                sMessage = oPEPDB.Paste((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public void ExportToExcel(DateTime StartDate, DateTime EndDate, int BUID, int ProductionUnitID, string BuyerIDs, string OrderRecapIDs, double ts)
        {
            #region Data gets

            List<ProductionExecutionPlanDetailBreakdown> _oProductionExecutionPlanDetailBreakdowns = new List<ProductionExecutionPlanDetailBreakdown>();
            string sSQL = "SELECT * FROM View_PLineConfigure ", sTemp = "";
            if (ProductionUnitID != 0)
            {
                Global.TagSQL(ref sTemp);
                sTemp += " ProductionUnitID  = " + ProductionUnitID;
            }
            else if (BUID != 0 || !string.IsNullOrEmpty(BuyerIDs) || !string.IsNullOrEmpty(OrderRecapIDs))
            {
                sSQL = "SELECT * FROM View_PLineConfigure WHERE ProductionUnitType = " + (int)EnumProductionUnitType.InHouseProduction;
                sTemp = " OR  PLineConfigureID  IN (SELECT HH.PLineConfigureID FROM  View_ProductionExecutionPlanDetailBreakDown AS HH WHERE  HH.WorkingDate Between '" + StartDate.ToString("dd MMM yyyy") + "' AND '" + EndDate.ToString("dd MMM yyyy") + "')";
            }
            else
            {
                sSQL = "SELECT * FROM View_PLineConfigure WHERE ProductionUnitType = " + (int)EnumProductionUnitType.InHouseProduction;
            }
            sSQL += sTemp;
            _oPLineConfigures = PLineConfigure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            sSQL = "SELECT * FROM  View_ProductionExecutionPlanDetailBreakDown WHERE PLineConfigureID IN(" + String.Join(",", _oPLineConfigures.Select(x => x.PLineConfigureID)).ToString() + ")";
            if (!string.IsNullOrEmpty(BuyerIDs))
            {
                sSQL += " AND BuyerID IN (" + BuyerIDs + ")";
            }
            if (!string.IsNullOrEmpty(OrderRecapIDs))
            {
                sSQL += " AND OrderRecapID IN (" + OrderRecapIDs + ")";
            }
            if (BUID != 0)
            {
                sSQL += " AND BUID =" + BUID;
            }
            _oProductionExecutionPlanDetailBreakdowns = ProductionExecutionPlanDetailBreakdown.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            #endregion
            if (_oPLineConfigures.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);


                int rowIndex = 2, nColumnCount = 0, _nOrderRecapID = 0, _nColorCount = 0, _PLineConfigureID = 0, nRowHeight = 60;
                //string[] Colors = { "Goldenrod", "MediumSpringGreen", "DarkOrchid", "DarkGray" };
                Color[] oDynamicColor = { Color.Goldenrod, Color.MediumSpringGreen, Color.DarkOrchid, Color.DarkGray };
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Production Plan");
                    sheet.Name = "Production Plan Report";
                    sheet.Column(2).Width = 20; //Line NO
                    nColumnCount = 3;
                    DateTime TempStartDate = StartDate;
                    while (TempStartDate <= EndDate)
                    {
                        sheet.Column(nColumnCount).Width = 35; nColumnCount++; //Date Column
                        TempStartDate = TempStartDate.AddDays(1);
                    }

                    #region Report Header
                    sheet.Cells[rowIndex, 2, rowIndex, nColumnCount].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, nColumnCount].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Productoin Plan"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Line No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumnCount = 3;
                    TempStartDate = StartDate;
                    while (TempStartDate <= EndDate)
                    {
                        cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = TempStartDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        TempStartDate = TempStartDate.AddDays(1);
                    }
                    rowIndex++;
                    #endregion
                    if (_nOrderRecapID == 0)//just initialize first value
                    {

                        ProductionExecutionPlanDetailBreakdown oPEPDB = new ProductionExecutionPlanDetailBreakdown();
                        oPEPDB = _oProductionExecutionPlanDetailBreakdowns.Where(x => x.OrderRecapID != 0).First();
                        _nOrderRecapID = oPEPDB.OrderRecapID;
                        _PLineConfigureID = oPEPDB.PLineConfigureID;

                    }
                    #region Value Pring
                    foreach (PLineConfigure oItem in _oPLineConfigures)
                    {
                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, 2]; nColumnCount++; cell.Value = "BU Name:" + oItem.RefShortName + "\nPU :" + oItem.PUShortName + "\nLine No :" + oItem.LineNo + "\nMachine Qty :" + oItem.MachineQty.ToString(); cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumnCount = 3;
                        TempStartDate = StartDate;
                        while (TempStartDate <= EndDate)
                        {
                            ProductionExecutionPlanDetailBreakdown oPEPDB = new ProductionExecutionPlanDetailBreakdown();
                            oPEPDB = _oProductionExecutionPlanDetailBreakdowns.Where(x => x.PLineConfigureID == oItem.PLineConfigureID && x.WorkingDate == TempStartDate).FirstOrDefault();
                            if (oPEPDB != null)
                            {
                                if (oPEPDB.OrderRecapID == _nOrderRecapID)//Order Recap ID
                                {
                                    if (oPEPDB.PLineConfigureID != _PLineConfigureID)//Set PLine Configure ID
                                    {
                                        if (_nColorCount == 3) { _nColorCount = 0; }//Again Reset Color code
                                        _nColorCount++;//Change color
                                        _PLineConfigureID = oPEPDB.PLineConfigureID;
                                    }
                                }
                                else
                                {
                                    _nOrderRecapID = oPEPDB.OrderRecapID;
                                    _PLineConfigureID = oPEPDB.PLineConfigureID;
                                    if (_nColorCount == 3) { _nColorCount = 0; }//Again Reset Color code
                                    _nColorCount++;//Change Color
                                }
                                sheet.Row(rowIndex).Height = nRowHeight;
                                cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = "Style No :" + oPEPDB.StyleNo + "\nPO NO: " + oPEPDB.RecapNo + "(" + Global.MillionFormatActualDigit(oPEPDB.RecapQty) + " " + oPEPDB.UnitSymbol + ")" + "\nBuyer : " + oPEPDB.BuyerName + "\nTarget Qty :" + Global.MillionFormatActualDigit(oPEPDB.DailyProduction) + ", Execution Qty :" + Global.MillionFormatActualDigit(oPEPDB.ExecutionQty); cell.Style.Font.Bold = true;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(oDynamicColor[_nColorCount]); cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            else
                            {
                                sheet.Row(rowIndex).Height = nRowHeight;
                                cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = " "; cell.Style.Font.Bold = true;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            TempStartDate = TempStartDate.AddDays(1);
                        }
                        rowIndex++;
                    }


                    #endregion


                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Production Plan.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }



            }

        }
        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(Server.MapPath("~/Content/") + "companyLogo.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
    }
}