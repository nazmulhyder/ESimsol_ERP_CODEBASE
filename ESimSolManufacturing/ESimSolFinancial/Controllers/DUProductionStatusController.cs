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
    public class DUProductionStatusController : PdfViewController
    {
        DUProductionStatus _oDUProductionStatus = new DUProductionStatus();
        List<DUProductionStatus> _oDUProductionStatusList = new List<DUProductionStatus>();
        string _sFormatter = ""; string _sDateRange = "";
        public ActionResult ViewDUProductionStatusList(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<DUProductionStatus> oDUProductionStatusList = new List<DUProductionStatus>();

            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            ViewBag.Buid = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            List<EnumObject> oObjs = EnumObject.jGets(typeof(EnumCompareOperator)).Where(x => x.id == (int)EnumCompareOperator.EqualTo || x.id == (int)EnumCompareOperator.Between).ToList();            
            EnumObject obj = new EnumObject();
            obj.id = 7;obj.Value="Month Wise";
            oObjs.Add(obj);
            obj = new EnumObject();
            obj.id = 8;obj.Value="Year Wise";
            oObjs.Add(obj);
            ViewBag.CompareOperators = oObjs;
            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);// EnumObject.jGets(typeof(EnumOrderType)).Where(x => x.id > 0);
            ViewBag.RSShifts = RSShift.GetsByModule(buid, "" + (int)EnumModuleName.RouteSheet, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUDyeingTypes = DUDyeingType.Gets("SELECT * FROM DUDyeingType", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RSStates = EnumObject.jGets(typeof(EnumRSState));
            return View(oDUProductionStatusList);
        }
        #region Report
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
        public ActionResult PrintDUProductionStatus(String sTemp)
        {
            List<DUProductionStatus> oDUProductionStatusList = new List<DUProductionStatus>();
            DUProductionStatus oDUProductionStatus = new DUProductionStatus();

            int nReportLayout = 0;
            DateTime dtYear = DateTime.MinValue;
            DateTime dtMonth = DateTime.MinValue;
            int BUID = 0;

            if (string.IsNullOrEmpty(sTemp))
            {
                throw new Exception("Nothing  to Print");
            }
            else
            {
                nReportLayout = Convert.ToInt32(sTemp.Split('~')[0]);
                dtYear = Convert.ToDateTime(sTemp.Split('~')[1]);
                dtMonth = Convert.ToDateTime(sTemp.Split('~')[2]);
                BUID = Convert.ToInt32(sTemp.Split('~')[3]);

                if (nReportLayout == 1)//Year Wise
                {
                    oDUProductionStatus.StartDate = dtYear;
                    oDUProductionStatus.EndDate = new DateTime(dtYear.Year, dtYear.Month, 1).AddYears(1).AddDays(-1);
                }else if (nReportLayout == 2)//Month Wise
                {
                    oDUProductionStatus.StartDate = dtMonth;
                    oDUProductionStatus.EndDate = new DateTime(dtMonth.Year, dtMonth.Month, 1).AddMonths(1);
                    oDUProductionStatus.EndDate = new DateTime(dtMonth.Year, dtMonth.Month, 1).AddMonths(1).AddDays(-1);
                }
                else if (nReportLayout > 2)//Product or Machine Wise
                {
                    oDUProductionStatus.StartDate = dtMonth;
                    oDUProductionStatus.EndDate = new DateTime(dtMonth.Year, dtMonth.Month, 1).AddMonths(1);
                    oDUProductionStatus.EndDate = new DateTime(dtMonth.Year, dtMonth.Month, 1).AddMonths(1).AddDays(-1);
                }
                oDUProductionStatusList = DUProductionStatus.GetsDUProductionStatus(BUID, nReportLayout, oDUProductionStatus.StartDate, oDUProductionStatus.EndDate, "", _oDUProductionStatus.RSState,((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptDUProductionStatus oReport = new rptDUProductionStatus();
            byte[] abytes = oReport.PrepareReport(oDUProductionStatusList, oCompany, oBusinessUnit, "Production Status", "", nReportLayout);
            return File(abytes, "application/pdf");
        }
        #endregion

        [HttpPost]
        public JsonResult GetsData(DUProductionStatus objDUProductionStatus)
        {
            List<DUProductionStatus> oDUProductionStatusList = new List<DUProductionStatus>();
            DUProductionStatus oDUProductionStatus = new DUProductionStatus();
            oDUProductionStatusList = new List<DUProductionStatus>();
            string sTemp = objDUProductionStatus.ErrorMessage;
            try
            {
                string sSQL = GetSQL(objDUProductionStatus);

                int nReportLayout = 0;
                DateTime dtYear = DateTime.MinValue;
                DateTime dtMonth = DateTime.MinValue;
                int BUID = 0;

                if (string.IsNullOrEmpty(sTemp))
                {
                    throw new Exception("Nothing  to Print");
                }
                else
                {
                    nReportLayout = Convert.ToInt32(sTemp.Split('~')[0]);
                    dtYear = Convert.ToDateTime(sTemp.Split('~')[1]);
                    dtMonth = Convert.ToDateTime(sTemp.Split('~')[2]);
                    BUID = Convert.ToInt32(sTemp.Split('~')[3]);

                    //if (nReportLayout == 1)//Month Wise
                    //{
                    //    oDUProductionStatus.StartDate = dtYear;
                    //    oDUProductionStatus.StartDate = new DateTime(dtYear.Year, 1, 1);
                    //    oDUProductionStatus.EndDate = new DateTime(dtYear.Year, 12, 31).AddYears(1).AddDays(-1);

                    //}
                    //else if (nReportLayout == 2)//Day Wise
                    //{
                    //    oDUProductionStatus.StartDate = dtMonth;
                    //    oDUProductionStatus.StartDate = new DateTime(dtMonth.Year, oDUProductionStatus.StartDate.Month, 1);
                    //    oDUProductionStatus.EndDate = new DateTime(dtMonth.Year, oDUProductionStatus.StartDate.Month, 1).AddMonths(1).AddDays(-1);
                    //}
                    //else if (nReportLayout == 3)//Product Wise (month)
                    //{
                    //    oDUProductionStatus.StartDate = dtMonth;
                    //    oDUProductionStatus.EndDate = new DateTime(dtMonth.Year, dtMonth.Month, 1).AddMonths(1);
                    //    oDUProductionStatus.EndDate = new DateTime(dtMonth.Year, dtMonth.Month, 1).AddMonths(1).AddDays(-1);
                    //}
                    //else if (nReportLayout == 4)//Machine Wise (month)
                    //{
                    //    oDUProductionStatus.StartDate = dtMonth;
                    //    oDUProductionStatus.EndDate = new DateTime(dtMonth.Year, dtMonth.Month, 1).AddMonths(1);
                    //    oDUProductionStatus.EndDate = new DateTime(dtMonth.Year, dtMonth.Month, 1).AddMonths(1).AddDays(-1);
                    //}                    

                    oDUProductionStatusList = DUProductionStatus.GetsDUProductionStatus(BUID, nReportLayout, _oDUProductionStatus.StartDate, _oDUProductionStatus.EndDate, sSQL, _oDUProductionStatus.RSState, ((User)Session[SessionInfo.CurrentUser]).UserID);


                    if (nReportLayout == 1)
                    {
                        foreach (DUProductionStatus oItem in oDUProductionStatusList)
                        {
                            oItem.RefName = oItem.StartDateStr_MMYY;
                        }
                    }
                    else  if (nReportLayout == 2)
                    {
                        foreach (DUProductionStatus oItem in oDUProductionStatusList)
                        {
                            oItem.RefName = oItem.StartDateStr;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oDUProductionStatusList = new List<DUProductionStatus>();
                oDUProductionStatus.ErrorMessage = ex.Message;
                oDUProductionStatusList.Add(oDUProductionStatus);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oDUProductionStatusList);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jSonResult = Json(oDUProductionStatusList, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        private string GetSQL(DUProductionStatus oDUProductionStatus)
        {
            _sDateRange = "";           
            int nOrderType = 0;
            int nRSShiftID = 0;
            int nDyeingType = 0;
            string sProductIDs = "";
            string sMachineIDs = "";
            bool bIsRedyeing = false;
            int nLayout = 0;
            //_oDUProductionStatus.RSNo = oDUProductionStatus.RSNo;
            _oDUProductionStatus.StartDate = DateTime.Now;
            _oDUProductionStatus.EndDate = DateTime.Now;
            if (!String.IsNullOrEmpty(oDUProductionStatus.Params))
            {
                int nCount = 0;
                //_oDUProductionStatus.RSNo = oDUProductionStatus.Params.Split('~')[nCount++];
                //sOrderNo = oDUProductionStatus.Params.Split('~')[nCount++];
                nOrderType = Convert.ToInt16(oDUProductionStatus.Params.Split('~')[nCount++]);
                nRSShiftID = Convert.ToInt16(oDUProductionStatus.Params.Split('~')[nCount++]);
                nDyeingType = Convert.ToInt16(oDUProductionStatus.Params.Split('~')[nCount++]);
                _oDUProductionStatus.RSState = (EnumRSState)Convert.ToInt16(oDUProductionStatus.Params.Split('~')[nCount++]);
                _oDUProductionStatus.DateType = Convert.ToInt16(oDUProductionStatus.Params.Split('~')[nCount++]);
                _oDUProductionStatus.StartDate = Convert.ToDateTime(oDUProductionStatus.Params.Split('~')[nCount++]);
                _oDUProductionStatus.EndDate = Convert.ToDateTime(oDUProductionStatus.Params.Split('~')[nCount++]);
                _oDUProductionStatus.Month = Convert.ToDateTime(oDUProductionStatus.Params.Split('~')[nCount++]);
                _oDUProductionStatus.Year = Convert.ToInt16(oDUProductionStatus.Params.Split('~')[nCount++]);
                sProductIDs = Convert.ToString(oDUProductionStatus.Params.Split('~')[nCount++]);
                sMachineIDs = Convert.ToString(oDUProductionStatus.Params.Split('~')[nCount++]);
                bIsRedyeing = Convert.ToBoolean(oDUProductionStatus.Params.Split('~')[nCount++]);
                
                if( _oDUProductionStatus.DateType==7)/// 7==Month
                {
                    _oDUProductionStatus.StartDate = new DateTime(_oDUProductionStatus.Month.Year, _oDUProductionStatus.Month.Month, 1);
                    _oDUProductionStatus.EndDate = new DateTime(_oDUProductionStatus.Month.Year, _oDUProductionStatus.Month.Month, 1).AddMonths(1).AddDays(-1);
                    _sDateRange = "Month: " + _oDUProductionStatus.StartDate.ToString("MMM yyyy");
                }
                if (_oDUProductionStatus.DateType == 8)/// 7==Year
                {
                    _oDUProductionStatus.StartDate = new DateTime(_oDUProductionStatus.Year, 1, 1);
                    _oDUProductionStatus.EndDate = new DateTime(_oDUProductionStatus.Year, 12, 31).AddYears(1).AddDays(-1);
                    _sDateRange = "Year: " + _oDUProductionStatus.StartDate.ToString("yyyy");
                }
                if (_oDUProductionStatus.DateType == (int)EnumCompareOperator.EqualTo)
                {
                    _sDateRange = "Date: " + _oDUProductionStatus.StartDate.ToString("dd MMM yyyy");
                }
                if (_oDUProductionStatus.DateType == (int)EnumCompareOperator.Between)
                {
                    _sDateRange = "Date: " + _oDUProductionStatus.StartDate.ToString("dd MMM yyyy")+"  -To-  "+_oDUProductionStatus.EndDate.ToString("dd MMM yyyy");
                }
            }

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            // oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            // AND  RouteSheet.RouteSheetID In (Select RouteSheetID from RouteSheetHistory as RSH where RSH.[CurrentStatus]=12 and RSH.EventTime>= ''10 JAN 2016'' and RSH.EventTime< ''10 JAN 2018'') '
            string sSQLQuery = "", sSQL="", sWhereCluse = "";


          
            #region nRSShiftID
            if (nRSShiftID > 0)
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " RSShiftID =" + nRSShiftID;
            }
            #endregion
            #region nDyeingType
            if (nDyeingType > 0)
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " HanksCone =" + nDyeingType;
            }
            #endregion
            #region bIsRedyeing
            if (bIsRedyeing == true)
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " IsReDyeing=1 ";
                //sWhereCluse = sWhereCluse + "(IsReDyeing=1 or RouteSheetID in (select RouteSheetID from RSRawLot where LotID in (Select lotID from lot where ParentType=106)))";
            }
            #endregion
            #region Machine IDs
            if (!string.IsNullOrEmpty(sMachineIDs))
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " MachineID In (" + sMachineIDs + ")";
            }
            #region nOrderType
            if (nOrderType > 0)
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " OrderType =" + nOrderType;
            }
            #endregion
            #endregion
            #region Product IDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductID IN  (" + sProductIDs + ")";
            }
            #endregion
            
            #region RSState & Date
            if (_oDUProductionStatus.RSState > 0 && _oDUProductionStatus.DateType > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                string sSearchPart = " WHERE RSH.[CurrentStatus]=" + (int)_oDUProductionStatus.RSState;
                //  DateObject.CompareDateQuery(ref sSearchPart, "RSH.EventTime", _oDUProductionStatus.DateType, _oDUProductionStatus.StartDate, _oDUProductionStatus.EndDate);
                if (_oDUProductionStatus.DateType == (int)EnumCompareOperator.EqualTo)
                {
                    sSearchPart = sSearchPart + "  and RSH.EventTime>='" + _oDUProductionStatus.StartDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and RSH.EventTime<'" + _oDUProductionStatus.StartDate.AddDays(1).ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "'";
                    //sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (_oDUProductionStatus.DateType == (int)EnumCompareOperator.Between)
                {
                    sSearchPart = sSearchPart + " and RSH.EventTime>='" + _oDUProductionStatus.StartDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and RSH.EventTime<'" + _oDUProductionStatus.EndDate.AddDays(1).ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "'";
                }
                else if (_oDUProductionStatus.DateType == 7) //for month
                {
                    sSearchPart = sSearchPart + " and MONTH(RSH.EventTime)=" + _oDUProductionStatus.Month.ToString("MM"); 
                }
                else if (_oDUProductionStatus.DateType == 8) //for year
                {
                    sSearchPart = sSearchPart + " and YEAR(RSH.EventTime)=" + _oDUProductionStatus.Year;
                }
                sWhereCluse = sWhereCluse + " RouteSheetID In (Select RouteSheetID from RouteSheetHistory as RSH " + sSearchPart + ")";
            }
            else if (_oDUProductionStatus.RSState > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RouteSheetID In (Select RouteSheetID from RouteSheetHistory as RSH WHERE RSH.[CurrentStatus]=" + (int)_oDUProductionStatus.RSState + ")";
            }
            #endregion

         
            sSQLQuery = sSQLQuery + sWhereCluse;
            if (!string.IsNullOrEmpty(sSQL)) { sSQLQuery = sSQLQuery + " and RouteSheetID in (Select RouteSheetID from RouteSheet " + sSQL + " )"; }
            return sSQLQuery;
        }

        #region Print
        [HttpPost]
        public ActionResult SetProductionReportListData(DUProductionStatus oDUProductionStatus)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oDUProductionStatus);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintDUProductionStatuss()
        {
            _oDUProductionStatus = new DUProductionStatus();
            int nBUID = 0; int nReportLayout = 0;
            try
            {
                _oDUProductionStatus = (DUProductionStatus)Session[SessionInfo.ParamObj];
                string sTemp = _oDUProductionStatus.ErrorMessage;
                string sSQL = GetSQL(_oDUProductionStatus);

                DateTime dtYear = DateTime.MinValue;
                DateTime dtMonth = DateTime.MinValue;                

                if (string.IsNullOrEmpty(sTemp))
                {
                    throw new Exception("Nothing  to Print");
                }
                else
                {
                    nReportLayout = Convert.ToInt32(sTemp.Split('~')[0]);
                    dtYear = Convert.ToDateTime(sTemp.Split('~')[1]);
                    dtMonth = Convert.ToDateTime(sTemp.Split('~')[2]);
                    nBUID = Convert.ToInt32(sTemp.Split('~')[3]);

                    _oDUProductionStatusList = DUProductionStatus.GetsDUProductionStatus(nBUID, nReportLayout, _oDUProductionStatus.StartDate, _oDUProductionStatus.EndDate, sSQL, _oDUProductionStatus.RSState, ((User)Session[SessionInfo.CurrentUser]).UserID);


                    if (nReportLayout == 1)
                    {
                        foreach (DUProductionStatus oItem in _oDUProductionStatusList)
                        {
                            oItem.RefName = oItem.StartDateStr_MMYY;
                        }
                    }
                    else if (nReportLayout == 2)
                    {
                        foreach (DUProductionStatus oItem in _oDUProductionStatusList)
                        {
                            oItem.RefName = oItem.StartDateStr;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _oDUProductionStatus = new DUProductionStatus();
                _oDUProductionStatusList = new List<DUProductionStatus>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (nBUID > 0)
            {
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }

            rptDUProductionStatusList oReport = new rptDUProductionStatusList();
            byte[] abytes = oReport.PrepareReport(_oDUProductionStatusList, oCompany, nReportLayout, _sDateRange);
            return File(abytes, "application/pdf");
        }

        public void ExcelProductionStatusList()
        {
            _oDUProductionStatus = new DUProductionStatus();
            int nBUID = 0; int nReportLayout = 0;
            string sErrorMsg = "";
            try
            {
                _oDUProductionStatus = (DUProductionStatus)Session[SessionInfo.ParamObj];
                string sTemp = _oDUProductionStatus.ErrorMessage;
                string sSQL = GetSQL(_oDUProductionStatus);

                DateTime dtYear = DateTime.MinValue;
                DateTime dtMonth = DateTime.MinValue;

                if (string.IsNullOrEmpty(sTemp))
                {
                    sErrorMsg = "Nothing  to Print";
                }
                else
                {
                    nReportLayout = Convert.ToInt32(sTemp.Split('~')[0]);
                    dtYear = Convert.ToDateTime(sTemp.Split('~')[1]);
                    dtMonth = Convert.ToDateTime(sTemp.Split('~')[2]);
                    nBUID = Convert.ToInt32(sTemp.Split('~')[3]);

                    _oDUProductionStatusList = DUProductionStatus.GetsDUProductionStatus(nBUID, nReportLayout, _oDUProductionStatus.StartDate, _oDUProductionStatus.EndDate, sSQL, _oDUProductionStatus.RSState, ((User)Session[SessionInfo.CurrentUser]).UserID);


                    if (nReportLayout == 1)
                    {
                        foreach (DUProductionStatus oItem in _oDUProductionStatusList)
                        {
                            oItem.RefName = oItem.StartDateStr_MMYY;
                        }
                    }
                    else if (nReportLayout == 2)
                    {
                        foreach (DUProductionStatus oItem in _oDUProductionStatusList)
                        {
                            oItem.RefName = oItem.StartDateStr;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _oDUProductionStatus = new DUProductionStatus();
                _oDUProductionStatusList = new List<DUProductionStatus>();
            }

            if (string.IsNullOrEmpty(sErrorMsg))
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (nBUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }

                string sNameTitle = "";
                if (nReportLayout == 1) sNameTitle = "Month";
                else if (nReportLayout == 2) sNameTitle = "Day";
                else if (nReportLayout == 3) sNameTitle = "Product";
                else if (nReportLayout == 4) sNameTitle = "Machine";

                #region Header
                List<TableHeader> table_header = new List<TableHeader>();

                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                if (nReportLayout == 1 || nReportLayout == 2) table_header.Add(new TableHeader { Header = sNameTitle, Width = 15f, IsRotate = false });
                else table_header.Add(new TableHeader { Header = sNameTitle, Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order Type", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dying Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Westage Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Recycle Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Packing Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dyeing Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Westage Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Recycle Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Packing Qty", Width = 20f, IsRotate = false });
                
                #endregion


                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Production Status Report");
                    sheet.Name = "Production_Status_Report";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    #endregion
                    nRowIndex++;
                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion
                    nRowIndex++;

                    string sHeaderMsg = "";
                    if (nReportLayout == 1) sHeaderMsg = "Month Wise Production Status Report";
                    else if (nReportLayout == 2) sHeaderMsg = "Day Wise Production Status Report";
                    else if (nReportLayout == 3) sHeaderMsg = "Product Wise Production Status Report";
                    else if (nReportLayout == 4) sHeaderMsg = "Machine Wise Production Status Report";

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = sHeaderMsg; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 1;

                    if (nReportLayout == 3 || nReportLayout == 4)
                    {
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                        cell.Value = _sDateRange; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex += 1;
                    }
                    nRowIndex += 1;

                    #region Top header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 4]; cell.Merge = true; cell.Value = " "; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5, nRowIndex, 8]; cell.Merge = true; cell.Value = "Fresh Dying"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, 12]; cell.Merge = true; cell.Value = "Re-Dying"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex += 1;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    #endregion

                    #region Data

                    nRowIndex++;

                    _sFormatter = " #,##0.00;(#,##0.00)";
                    int nCount = 0;
                    foreach (var oItem in _oDUProductionStatusList)
                    {
                        nStartCol = 2;

                        #region DATA
                        FillCellMerge(ref sheet, (++nCount).ToString("00"), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.RefName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.OrderTypeSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                        FillCell(sheet, nRowIndex, nStartCol++, oItem.QtyDyeing.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.QtyWestage.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.QtyRecycle.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.QtyPacking.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.QtyDyeing_ReP.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.QtyRecycle_ReP.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.QtyWestage_ReP.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.QtyPacking_ReP.ToString(), true);

                        #endregion
                        nRowIndex++;

                    }

                    #region Total
                    nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";

                    FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUProductionStatusList.Sum(x => x.QtyDyeing).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUProductionStatusList.Sum(x => x.QtyWestage).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUProductionStatusList.Sum(x => x.QtyRecycle).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUProductionStatusList.Sum(x => x.QtyPacking).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUProductionStatusList.Sum(x => x.QtyDyeing_ReP).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUProductionStatusList.Sum(x => x.QtyRecycle_ReP).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUProductionStatusList.Sum(x => x.QtyWestage_ReP).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUProductionStatusList.Sum(x => x.QtyPacking_ReP).ToString(), true, true);

                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename= Production_Status_Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }

        }

        #region Excel Support
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber)
        {
            return FillCell(sheet, nRowIndex, nStartCol, sVal, IsNumber, false);
        }
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber, bool IsBold)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[nRowIndex, nStartCol++];
            if (IsNumber && !string.IsNullOrEmpty(sVal))
                cell.Value = Convert.ToDouble(sVal);
            else
                cell.Value = sVal;
            cell.Style.Font.Bold = IsBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            if (IsNumber)
            {
                cell.Style.Numberformat.Format = _sFormatter;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            return cell;
        }

        private void FillCellMerge(ref ExcelWorksheet sheet, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, string sVal)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = false;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex)
        {
            FillCellMerge(ref sheet, sVal, startRowIndex, endRowIndex, startColIndex, endColIndex, false, ExcelHorizontalAlignment.Left);
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool isBold, ExcelHorizontalAlignment HoriAlign)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = isBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = HoriAlign;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }
        #endregion

        #endregion


    }
}