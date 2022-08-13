using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using ESimSol.BusinessObjects;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Reflection;



namespace ESimSolFinancial.Controllers
{
    public class ProductionScheduleController : PdfViewController
    {
        #region Declartion

        ProductionSchedule _oProductionSchedule = new ProductionSchedule();
        List<ProductionSchedule> _oProductionScheduleList = new List<ProductionSchedule>();
        DataTable _oDataTable = new DataTable();
        DataSet _oDataSet = new DataSet();
        List<RSDTree> _oRSDTs = new List<RSDTree>();
        RSDTree _oRSDT = new RSDTree();
        string _sQuery = "SELECT * FROM Location where LocationID in(SELECT Distinct LocationID FROM VIEW_CapitalResource where IsActive=1)";

        #endregion

        public ActionResult ViewProductionSchedules(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oProductionScheduleList = new List<ProductionSchedule>();
            _oProductionScheduleList = ProductionSchedule.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oProductionScheduleList);
        }

        public ActionResult ViewProductionSchedule(int nPSID, int nMID, int nBUID)
        {
            _oProductionSchedule = new ProductionSchedule();
            TempData["StartTimeInString"] = null;
            if (nPSID > 0)
            {
                _oProductionSchedule = _oProductionSchedule.Get(nPSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.Gets(nPSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (nMID > 0)
            {
                ProductionSchedule oPS = new ProductionSchedule();
                List<ProductionSchedule> oPSs = new List<ProductionSchedule>();
                string sSQL = "Select * from View_ProductionSchedule Where EndTime = (Select MAX(EndTime) from ProductionSchedule Where MachineID=" + nMID + ")";
                oPSs = ProductionSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPSs.Count > 0)
                {
                    TempData["StartTimeInString"] = oPSs[0].EndTime.AddMinutes(1).ToString("MM'/'dd'/'yyyy HH:mm");
                }
            }
            _sQuery = "SELECT * FROM Location where LocationID in(SELECT Distinct LocationID FROM VIEW_CapitalResource where ISNULL(IsActive,1)=1 AND BUID = " + nBUID + " AND ResourcesType = " + (int)EnumResourcesType.Machine + " )";
            _oProductionSchedule.LocationList = Location.Gets(_sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.CapitalResources = CapitalResource.GetsByBUandResourceTypeWithName(nBUID, (int)EnumResourcesType.Machine, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.OrderTypes = Enum.GetValues(typeof(EnumOrderType)).Cast<EnumOrderType>().Select(x => new SelectListItem { Text = Global.EnumerationFormatter(x.ToString()), Value = ((int)x).ToString() }).ToList();
            return View(_oProductionSchedule);
        }

        public ActionResult ViewEditProductionSchedule(int id, int nBUID)
        {
            if (id > 0)
            {
                _oProductionSchedule = _oProductionSchedule.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _sQuery = "SELECT * FROM Location where LocationID in(SELECT Distinct LocationID FROM VIEW_CapitalResource where ISNULL(IsActive,1)=1 AND BUID = " + nBUID + " AND ResourcesType = " + (int)EnumResourcesType.Machine + " )";
            _oProductionSchedule.LocationList = Location.Gets(_sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.CapitalResources = CapitalResource.GetsByBUandResourceTypeWithName(nBUID, (int)EnumResourcesType.Machine, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.OrderTypes = Enum.GetValues(typeof(EnumOrderType)).Cast<EnumOrderType>().Select(x => new SelectListItem { Text = Global.EnumerationFormatter(x.ToString()), Value = ((int)x).ToString() }).ToList();
            return View(_oProductionSchedule);
        }

        public ActionResult ViewScheduleInformation(int id)
        {
            _oProductionSchedule = new ProductionSchedule();
            if (id > 0)
            {
                _oProductionSchedule = _oProductionSchedule.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(_oProductionSchedule);
        }

        public ActionResult ViewProductionSchedule_Machine(int menuid, int buid)
        {

            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            string sLocationIDs = "";
            string sPSIDs = "";

            _oProductionSchedule.LocationList = Location.Gets(_sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.CapitalResources = CapitalResource.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sLocationIDs = Location.IDInString(_oProductionSchedule.LocationList);
            _oProductionSchedule.ProductionScheduleList = new List<ProductionSchedule>();

            DateTime today = DateTime.Today;
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

            string sql = "Select * from View_ProductionSchedule where LocationID=" + sLocationIDs + " and ProductionScheduleID in ( Select distinct(ProductionScheduleID) from View_ProductionScheduleDetail Where ((StartTime>='" + startOfMonth + "' and StartTime<='" + endOfMonth + "') or (EndTime>='" + startOfMonth + "' and EndTime<='" + endOfMonth + "'))  and RSState<7 ) and Activity='1' order by StartTime ";
            _oProductionSchedule.ProductionScheduleList = ProductionSchedule.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            // _oProductionSchedule.ProductionScheduleList = ProductionSchedule.Gets(startOfMonth, endOfMonth, sLocationIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oProductionSchedule.ProductionScheduleList != null)
            {
                sPSIDs = ProductionSchedule.IDInString(_oProductionSchedule.ProductionScheduleList);
            }
            if (sPSIDs.Length > 0)
            {

                // _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.Gets(sPSIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sql = "SELECT * FROM View_ProductionScheduleDetail where ProductionScheduleID in (" + sPSIDs + ") and RSState<7";
                _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ProductionSchedule oItem in _oProductionSchedule.ProductionScheduleList)
                {
                    this.AddOrderDetail(oItem, "View");
                }
            }


            string sSql = "select isnull(Max(schedule),0) from(select count(MachineID)as schedule from ProductionSchedule group by MachineID)as maxSchedule";

            _oProductionSchedule.MaxValue = ProductionSchedule.GetsMax(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);


            return View(_oProductionSchedule);
        }

        public ActionResult ViewOrderChange()
        {
            _oProductionSchedule.ProductionScheduleList = ProductionSchedule.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(_oProductionSchedule);
        }



        #region Machine Picker

        public ActionResult ViewMachine(int buid)
        {
            _oProductionSchedule.LocationList = Location.Gets(_sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.CapitalResources = CapitalResource.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(_oProductionSchedule);
        }

        public ActionResult ViewMachineByName(string sStr, int buid, double nts)
        {
            List<CapitalResource> oCapitalResources = new List<CapitalResource>();
            CapitalResource oCapitalResource = new CapitalResource();
            try
            {
                // Supplier=1; Buyer=2, Factory=3, Company=5,
                DateTime startTime = DateTime.Now; DateTime endTime = DateTime.Now; int nDaysInMonth = 0;
                string sSQL = "SELECT * FROM VIEW_CapitalResource WHERE Name Like '%" + sStr + "%' AND BUID = " + buid;
                oCapitalResources = CapitalResource.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oCapitalResources.Count <= 0)
                {
                    throw new Exception("Nothing Found.");
                }
                else
                {
                    List<ProductionSchedule> oProductionSchedules = new List<ProductionSchedule>();
                    nDaysInMonth = DateTime.DaysInMonth(startTime.Year, startTime.Month);
                    endTime = Convert.ToDateTime(endTime.AddDays(nDaysInMonth - endTime.Day).ToString("dd MMM yyyy"));
                    startTime = Convert.ToDateTime(startTime.AddDays(-startTime.Day + 1).ToString("dd MMM yyyy"));
                    sSQL = "Select * from View_ProductionSchedule where ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "')) and MachineID IN (SELECT MachineID FROM VIEW_CapitalResource WHERE MachineName Like '%" + sStr + "%' ) and Activity='1'  order by MachineID,StartTime ";
                    oProductionSchedules = ProductionSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oProductionSchedules.Count() > 0)
                    {
                        var oTPS = (from oPS in oProductionSchedules
                                    group oPS by oPS.MachineID into oTemp
                                    select new { MachineID = oTemp.Key, TotalSchedule = oTemp.Count() });
                        foreach (CapitalResource oCR in oCapitalResources)
                        {
                            var oCount = from x in oTPS where x.MachineID == oCR.CRID select x.TotalSchedule;
                            //if (oCount.Count()>0) { oCR.MonthlySchedule = Convert.ToInt32(oCount.First()); }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                oCapitalResources = new List<CapitalResource>();
                oCapitalResource.ErrorMessage = ex.Message;
                oCapitalResources.Add(oCapitalResource);
            }
            return PartialView(oCapitalResources);
        }

        #endregion

        #region Machine Change

        public ActionResult ViewMachineChange(int buid)
        {
            _oProductionSchedule.LocationList = Location.Gets(_sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.CapitalResources = CapitalResource.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(_oProductionSchedule);
        }

        #endregion

        #region Get Waiting Production Quantity

        [HttpPost]
        public JsonResult GetWaitingProductionQuantity(ProductionScheduleDetail oProductionScheduleDetail)
        {

            try
            {

                ProductionSchedule oProductionSchedule = new ProductionSchedule();
                string sSql = " Select Isnull(sum(ProductionQty),0) from ProductionScheduleDetail where ProductionTracingUnitID='" + oProductionScheduleDetail.ProductionTracingUnitID + "' ";

                oProductionScheduleDetail.WaitingForProductionQty = oProductionSchedule.GetWaitingProductionQuantity(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oProductionScheduleDetail = new ProductionScheduleDetail();
                oProductionScheduleDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionScheduleDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region HTTP Save & Edit

        #region Save Batch wise

        [HttpPost]
        public JsonResult Save_Batch(ProductionSchedule oProductionSchedule)
        {



            DateTime startDate = DateTime.Now, endDate = DateTime.Now;

            try
            {

                Boolean bflag = true;
                int nBatch = Convert.ToInt32(oProductionSchedule.BatchNo);
                double ntotalQuantity = oProductionSchedule.ProductionQty * nBatch;
                double nYetToSchedule = oProductionSchedule.ProductionScheduleDetails[0].YetToProductionQty - oProductionSchedule.ProductionScheduleDetails[0].WaitingForProductionQty;
                double ntempBatchNo = nBatch - 1;
                double ntempTotalQuantity = ntempBatchNo * oProductionSchedule.ProductionQty;
                string sErrorMessage = "";
                string sRemarks = oProductionSchedule.ProductionScheduleDetails[0].Remarks;
                double nQuantity = 0;



                int nStartMinute = oProductionSchedule.StartTime.Minute;
                int nEndMinute = oProductionSchedule.EndTime.Minute;

                int nMinuteRemainder = nStartMinute % 15;
                if (nMinuteRemainder > 0)
                {
                    startDate = oProductionSchedule.StartTime.AddMinutes(-nMinuteRemainder);
                    startDate = startDate.AddMinutes(15); //--------------
                }
                else
                {
                    startDate = oProductionSchedule.StartTime;
                }


                nMinuteRemainder = nEndMinute % 15;
                if (nMinuteRemainder > 0)
                {

                    endDate = oProductionSchedule.EndTime.AddMinutes(-(nMinuteRemainder + 1));
                    endDate = endDate.AddMinutes(15); //----------------

                }
                else
                {
                    if (nMinuteRemainder == 0)
                        endDate = oProductionSchedule.EndTime.AddMinutes(-1);
                }

                TimeSpan diffTime = endDate - startDate;

                if (nBatch > 1)
                {
                    for (int i = 1; i < nBatch; i++)
                    {
                        endDate = endDate.AddMinutes(1);
                        endDate = endDate.Add(diffTime);
                    }

                }

                string sSql = "select COUNT(*) from ProductionSchedule where EndTime >= '" + startDate + "' and StartTime <='" + endDate + "' and LocationID=" + oProductionSchedule.LocationID + " and MachineID=" + oProductionSchedule.MachineID + "";
                int nCount = ProductionSchedule.GetsMax(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (nCount > 0 && oProductionSchedule.bIncreaseTime == false)
                {
                    sErrorMessage = "Already a schedule is created in this time period. \nPlease choose with another time period.";
                }


                //if (ntotalQuantity > nYetToSchedule)
                //{

                //    double ntemp = Math.Ceiling(oProductionSchedule.ProductionScheduleDetails[0].YetToProductionQty * 0.08) + oProductionSchedule.ProductionScheduleDetails[0].YetToProductionQty;

                //    if (ntotalQuantity > ntempTotalQuantity && ntempTotalQuantity < nYetToSchedule)
                //    {



                //        if (ntemp >= (oProductionSchedule.ProductionScheduleDetails[0].WaitingForProductionQty + oProductionSchedule.ProductionScheduleDetails[0].ProductionQty))
                //        {
                //            nQuantity = ntotalQuantity;
                //            bflag = false;
                //        }
                //        else if (ntemp < (oProductionSchedule.ProductionScheduleDetails[0].WaitingForProductionQty + oProductionSchedule.ProductionScheduleDetails[0].ProductionQty))
                //        {
                //            sErrorMessage = "You can assign maximum " + (ntemp - oProductionSchedule.ProductionScheduleDetails[0].WaitingForProductionQty) + " quantity per batch.";
                //        }
                //        else
                //        {
                //            nQuantity = nYetToSchedule - ntempTotalQuantity;
                //        }
                //    }

                //    else
                //    {
                //        var nMaxQuantityPerBatch = Math.Floor(nYetToSchedule / nBatch);
                //        if (oProductionSchedule.ProductionScheduleDetails[0].YetToProductionQty <= oProductionSchedule.ProductionScheduleDetails[0].WaitingForProductionQty)
                //        {

                //            if (oProductionSchedule.ProductionScheduleDetails[0].WaitingForProductionQty >= ntemp)
                //            {
                //                // ntemp = ntemp - oProductionSchedule.ProductionScheduleDetails[0].WaitingForProductionQty;
                //                sErrorMessage = "Already scheduled 8% more.";
                //            }



                //            bflag = false;

                //        }
                //        else
                //        {
                //            sErrorMessage = "You can assign maximum " + nMaxQuantityPerBatch + " quantity per batch.";
                //        }
                //    }
                //}

                //Modification part For without validation of Quantity
                nQuantity = oProductionSchedule.ProductionQty;


                if (sErrorMessage == "")
                {

                    for (int i = 1; i <= nBatch; i++)
                    {
                        //nStartMinuteRemainder

                        nStartMinute = oProductionSchedule.StartTime.Minute;
                        nEndMinute = oProductionSchedule.EndTime.Minute;

                        nMinuteRemainder = nStartMinute % 15;

                        if (nMinuteRemainder > 0)
                        {
                            oProductionSchedule.StartTime = oProductionSchedule.StartTime.AddMinutes(-nMinuteRemainder);
                            oProductionSchedule.StartTime = oProductionSchedule.StartTime.AddMinutes(15); //-----------
                        }


                        nMinuteRemainder = nEndMinute % 15;
                        if (nMinuteRemainder > 0)
                        {
                            if (i == 1)
                            {
                                oProductionSchedule.EndTime = oProductionSchedule.EndTime.AddMinutes(-nMinuteRemainder);
                                oProductionSchedule.EndTime = oProductionSchedule.EndTime.AddMinutes(15); //-----------
                            }
                            else
                            {
                                nEndMinute = 14 - nMinuteRemainder;
                                oProductionSchedule.EndTime = oProductionSchedule.EndTime.AddMinutes(nEndMinute);
                            }
                        }


                        // TimeDifference(out oProductionSchedule.StartTime, out oProductionSchedule.EndTime);

                        TimeSpan ts = oProductionSchedule.EndTime - oProductionSchedule.StartTime;



                        int nhours = ts.Hours;
                        int nminutes = ts.Minutes;

                        //if (nhours >=1)
                        //{
                        //    oProductionSchedule.EndTime=oProductionSchedule.EndTime.AddMinutes(-1);
                        //}

                        if (i == 1)
                        {
                            oProductionSchedule.EndTime = oProductionSchedule.EndTime.AddMinutes(-1);
                        }
                        else if ((nminutes % 15) == 0 && i > 1)
                        {
                            oProductionSchedule.EndTime = oProductionSchedule.EndTime.AddMinutes(-1);
                        }


                        TimeSpan timeDiff = oProductionSchedule.EndTime - oProductionSchedule.StartTime;

                        oProductionSchedule.BatchGroup = _oProductionSchedule.BatchGroup;
                        oProductionSchedule.BatchNo = (EnumNumericOrder)i;
                        startDate = oProductionSchedule.StartTime;
                        endDate = oProductionSchedule.EndTime;

                        ProductionScheduleDetail oProductionScheduleDetail = new ProductionScheduleDetail();
                        List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();


                        oProductionScheduleDetail.ProductionTracingUnitID = oProductionSchedule.ProductionScheduleDetails[0].ProductionTracingUnitID;
                        oProductionScheduleDetail.DODID = oProductionSchedule.ProductionScheduleDetails[0].DODID;
                        oProductionScheduleDetail.Remarks = sRemarks;
                        if (i == nBatch && ntotalQuantity > nYetToSchedule && bflag == true)
                        {
                            oProductionSchedule.ProductionQty = nQuantity;
                            oProductionScheduleDetail.ProductionQty = nQuantity;
                            oProductionSchedule.ScheduleStatus = (EnumProductionScheduleStatus)2;  // here, 2 is used for indicating incomplete cycle
                        }

                        else if (bflag == false)
                        {
                            oProductionSchedule.ProductionQty = nQuantity;
                            oProductionScheduleDetail.ProductionQty = nQuantity;
                            oProductionSchedule.ScheduleStatus = (EnumProductionScheduleStatus)2;  // here, 2 is used for indicating incomplete cycle
                        }

                        else
                        {
                            oProductionScheduleDetail.ProductionQty = oProductionSchedule.ProductionScheduleDetails[0].ProductionQty;
                            oProductionSchedule.ScheduleStatus = (EnumProductionScheduleStatus)1; // here, 1 is used for indicating complete cycle
                        }
                        oProductionSchedule.ProductionScheduleDetails.Clear();
                        oProductionSchedule.ProductionScheduleDetails.Add(oProductionScheduleDetail);
                        _oProductionSchedule = oProductionSchedule.Save(((User)Session[SessionInfo.CurrentUser]).UserID);

                        TimeSpan newspan = timeDiff.Add(TimeSpan.FromMinutes(1));
                        oProductionSchedule.StartTime = startDate.Add(newspan);
                        if (i == (nBatch - 1))
                        {
                            oProductionSchedule.EndTime = endDate.Add(newspan);
                        }
                        else
                        {
                            if (nhours >= 1 && nminutes == 0)
                            {

                                oProductionSchedule.EndTime = endDate.Add(timeDiff);
                            }
                            else
                            {
                                oProductionSchedule.EndTime = endDate.Add(newspan);
                            }
                        }

                    }

                    string sSQL = "Select * from View_ProductionSchedule where BatchGroup='" + _oProductionSchedule.BatchGroup + "' ";

                    _oProductionSchedule.ProductionScheduleList = ProductionSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (_oProductionSchedule.ProductionScheduleList.Count > 0)
                    {
                        string sPSDIDS = "";
                        foreach (ProductionSchedule ops in _oProductionSchedule.ProductionScheduleList)
                        {
                            sPSDIDS = sPSDIDS + ops.ProductionScheduleID + ",";
                        }
                        sPSDIDS = sPSDIDS.Substring(0, sPSDIDS.Length - 1);
                        sSQL = "Select * from View_ProductionScheduleDetail where ProductionScheduleID in(" + sPSDIDS + ") ";
                        _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }

                else
                {
                    _oProductionSchedule.ErrorMessage = sErrorMessage;
                }

            }
            catch (Exception ex)
            {
                _oProductionSchedule = new ProductionSchedule();
                _oProductionSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit Production schedule

        public JsonResult Save(ProductionSchedule oProductionSchedule)
        {
            _oProductionSchedule = new ProductionSchedule();
            _oProductionSchedule = ModifiedSchedule(oProductionSchedule);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ProductionSchedule ModifiedSchedule(ProductionSchedule oProductionSchedule)
        {
            if (oProductionSchedule.ProductionScheduleID > 0)
            {
                if (oProductionSchedule.IncDecMachineID > 0) // for undo redo
                {
                    oProductionSchedule.bEffectIncDec = false;
                }
                int nUnpublishProductionSchedule = 0;
                int nStartMinute = oProductionSchedule.StartTime.Minute;
                int nEndMinute = oProductionSchedule.EndTime.Minute;

                int nMinuteRemainder = nStartMinute % 15;

                if (nMinuteRemainder > 0)
                {
                    oProductionSchedule.StartTime = oProductionSchedule.StartTime.AddMinutes(-nMinuteRemainder);
                    oProductionSchedule.StartTime = oProductionSchedule.StartTime.AddMinutes(15); //----------
                }

                nMinuteRemainder = nEndMinute % 15;
                if (nMinuteRemainder >= 0)
                {
                    if (nMinuteRemainder > 0 && nMinuteRemainder != 14)
                    {
                        oProductionSchedule.EndTime = oProductionSchedule.EndTime.AddMinutes(-(nMinuteRemainder + 1));
                        oProductionSchedule.EndTime = oProductionSchedule.EndTime.AddMinutes(15); //-----------------
                    }
                    if (nMinuteRemainder == 0)
                    {
                        oProductionSchedule.EndTime = oProductionSchedule.EndTime.AddMinutes(-1);
                    }

                }


                string sSQL = "Select top(1)* from View_ProductionSchedule Where MachineID=" + oProductionSchedule.MachineID + " AND EndTime>='" + oProductionSchedule.StartTimeInPerfect + "' Order By StartTime ";
                List<ProductionSchedule> oPSs = new List<ProductionSchedule>();
                oPSs = ProductionSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


                if (oProductionSchedule.ScheduleStatusInString == "Publish")
                {

                    oProductionSchedule.ScheduleStatus = (EnumProductionScheduleStatus)3; // 3 is used to publish a schedule. 
                    if (nUnpublishProductionSchedule == 0)
                    {
                        try
                        {
                            _oProductionSchedule = oProductionSchedule.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                        catch (Exception ex)
                        {
                            _oProductionSchedule = new ProductionSchedule();
                            _oProductionSchedule.ErrorMessage = ex.Message;
                        }
                    }
                    else
                    {
                        _oProductionSchedule.ErrorMessage = "You have need to publish all schedule before this schedule.";
                    }
                }

                else
                {


                    try
                    {
                        _oProductionSchedule = oProductionSchedule.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                        sSQL = "Select * from View_ProductionScheduleDetail where ProductionScheduleID =" + _oProductionSchedule.ProductionScheduleID + "";
                        _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    }
                    catch (Exception ex)
                    {
                        _oProductionSchedule = new ProductionSchedule();
                        _oProductionSchedule.ErrorMessage = ex.Message;
                    }
                }
                if (oPSs.Count() > 0)
                {
                    if (_oProductionSchedule.EndTime > oPSs[0].StartTime)
                    {
                        _oProductionSchedule.CheckTime = oPSs[0].StartTime;
                        TimeSpan oTS = new TimeSpan();
                        oTS = (_oProductionSchedule.EndTime.AddMinutes(1) - _oProductionSchedule.CheckTime).Duration();
                        _oProductionSchedule.IncDecTime = new DateTime(1900, 1, (oTS.Days > 0 ? oTS.Days : 1), oTS.Hours, oTS.Minutes, 0);
                    }
                }
            }
            else
            {
                _oProductionSchedule = new ProductionSchedule();
                _oProductionSchedule.ErrorMessage = "Invalid Selection";
            }
            return _oProductionSchedule;
        }
        #endregion

        #endregion

        #region Check Number of Schedules In Given Time Period.
        [HttpPost]
        public JsonResult ScheduleInTimePeriod(ProductionSchedule oProductionSchedule, double nts)
        {

            int nScheduleCount = NumberofScheduleInTimePeriod(oProductionSchedule);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(nScheduleCount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public int NumberofScheduleInTimePeriod(ProductionSchedule oProductionSchedule)
        {
            int nScheduleCount = 0;
            DateTime dStartTime = oProductionSchedule.StartTime;
            DateTime dEndTime = oProductionSchedule.EndTime.AddMinutes(-1);
            int nLocationID = oProductionSchedule.LocationID;
            int nMachineID = oProductionSchedule.MachineID;
            string sSQL = "Select * from View_ProductionSchedule where EndTime >= '" + dStartTime + "' and StartTime <='" + dEndTime + "' and LocationID=" + nLocationID + " and MachineID=" + nMachineID + " and ProductionScheduleID NOT IN (" + oProductionSchedule.ProductionScheduleID + ")";
            if (nLocationID > 0 && nMachineID > 0)
            {
                List<ProductionSchedule> oPS = new List<ProductionSchedule>();
                oPS = ProductionSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oPS.Count() > 0)
                {
                    if (oProductionSchedule.StartTime <= oPS.Min(x => x.EndTime))
                    {
                        nScheduleCount = oPS.Count();
                    }
                }

                // nScheduleCount = ProductionSchedule.GetsMax(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return nScheduleCount;

        }

        #endregion

        #region Get of Schedules In Given Time Period.
        [HttpPost]
        public JsonResult GetSchedulesInTimePeriod(ProductionSchedule oProductionSchedule, double nts)
        {
            DateTime dStartTime = oProductionSchedule.StartTime;
            DateTime dEndTime = oProductionSchedule.EndTime;
            int nLocationID = oProductionSchedule.LocationID;
            int nMachineID = oProductionSchedule.MachineID;
            string sSQL = "Select top(1)* from View_ProductionSchedule where EndTime >= '" + dStartTime + "' AND LocationID=" + nLocationID + " AND MachineID=" + nMachineID + " AND ProductionScheduleID Not IN(" + oProductionSchedule.ProductionScheduleID + ") Order By StartTime";
            List<ProductionSchedule> oPSs = new List<ProductionSchedule>();
            if (nLocationID > 0 && nMachineID > 0)
            {
                oPSs = ProductionSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPSs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HTTP Delete

        [HttpPost]
        public JsonResult Delete(ProductionSchedule oProductionSchedule)
        {
            string sErrorMease = "";
            try
            {

                sErrorMease = oProductionSchedule.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (sErrorMease.Contains('~'))
                {
                    sErrorMease = Convert.ToString(sErrorMease.Split('~')[0]);
                }

            }
            catch (Exception ex)
            {

                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HTTP Refresh

        [HttpPost]
        public JsonResult Refresh(ProductionSchedule oProductionSchedule, bool bUptoLoadDyeMachine)
        {
            _oProductionSchedule = new ProductionSchedule();
            string gridas = "";
            DateTime today = DateTime.Now, startTime = DateTime.Now, endTime = DateTime.Now;
            if (oProductionSchedule.sDay == "Day")
            {
                gridas = "Day";
                today = oProductionSchedule.StartTime;
                startTime = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
                endTime = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59);
            }
            else if (oProductionSchedule.sWeek == "Week")
            {
                gridas = "Week";
                today = oProductionSchedule.StartTime;
                startTime = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
                DateTime newTime = today.AddDays(6);
                endTime = new DateTime(newTime.Year, newTime.Month, newTime.Day, 23, 59, 59);
            }
            else if (oProductionSchedule.sMonth == "Month")
            {
                gridas = "Month";
                today = oProductionSchedule.StartTime;
                startTime = new DateTime(today.Year, today.Month, 1);
                endTime = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month), 23, 59, 59);
            }
            _oProductionSchedule = RefreshValue(oProductionSchedule, bUptoLoadDyeMachine, startTime, endTime, gridas);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        ProductionSchedule RefreshValue(ProductionSchedule oProductionSchedule, bool bUptoLoadDyeMachine, DateTime startTime, DateTime endTime, string gridas)
        {
            _oProductionSchedule = new ProductionSchedule();
            _oProductionSchedule.ProductionScheduleList = new List<ProductionSchedule>();
            string sOrderNo = "";
            int x, y, z;
            if (oProductionSchedule.ProductionScheduleDetails.Count > 0)
            {
                sOrderNo = oProductionSchedule.ProductionScheduleDetails[0].OrderNo;
            }

            string sPSIDs = "";
            try
            {
                string sql = "";

                if (String.IsNullOrEmpty(oProductionSchedule.MachineIDs) && String.IsNullOrEmpty(sOrderNo))
                {

                    if (bUptoLoadDyeMachine == true)
                    {
                        sql = "Select * from View_ProductionSchedule where LocationID=" + oProductionSchedule.LocationID + " and ProductionScheduleID in ( Select distinct(ProductionScheduleID) from View_ProductionScheduleDetail Where ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "')) and  RSState<7 )  and Activity='1' order by MachineID,StartTime ";
                    }
                    else
                    {
                        sql = "Select * from View_ProductionSchedule where ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "')) and LocationID=" + oProductionSchedule.LocationID + " and Activity='1' order by MachineID,StartTime ";
                        // sql = "Select * from View_ProductionSchedule where StartTime>='" + startTime + "' and EndTime<='" + endTime + "' and LocationID=" + oProductionSchedule.LocationID + " order by MachineID,StartTime ";
                    }
                }
                else if (String.IsNullOrEmpty(oProductionSchedule.MachineIDs) && !String.IsNullOrEmpty(sOrderNo))
                {

                    if (bUptoLoadDyeMachine == true)
                    {
                        sql = "Select * from View_ProductionSchedule where LocationID=" + oProductionSchedule.LocationID + " and Activity='1' and ProductionScheduleID in (Select distinct(ProductionScheduleID) from View_ProductionScheduleDetail Where OrderNo LIKE '%" + sOrderNo + "%' and ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "')) and RSState<7) order by MachineID,StartTime ";
                    }
                    else
                    {
                        sql = "Select * from View_ProductionSchedule where ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "')) and LocationID=" + oProductionSchedule.LocationID + " and Activity='1' and ProductionScheduleID in (Select distinct(ProductionScheduleID) from View_ProductionScheduleDetail Where OrderNo LIKE '%" + sOrderNo + "'% ) order by MachineID,StartTime ";

                    }
                }
                else if (!String.IsNullOrEmpty(oProductionSchedule.MachineIDs) && !String.IsNullOrEmpty(sOrderNo))
                {
                    if (bUptoLoadDyeMachine == true)
                    {
                        sql = "Select * from View_ProductionSchedule where MachineID in (" + oProductionSchedule.MachineIDs + ") and LocationID=" + oProductionSchedule.LocationID + " and Activity='1' and ProductionScheduleID in (Select ProductionScheduleID from View_ProductionScheduleDetail Where OrderNo LIKE '%" + sOrderNo + "%' and ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "')) and  RSState<7 ) order by MachineID,StartTime ";
                    }
                    else
                    {
                        sql = "Select * from View_ProductionSchedule where ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "')) and MachineID in (" + oProductionSchedule.MachineIDs + ") and LocationID=" + oProductionSchedule.LocationID + " and Activity='1' and ProductionScheduleID in (Select ProductionScheduleID from View_ProductionScheduleDetail Where OrderNo LIKE '%" + sOrderNo + "'%) order by MachineID,StartTime ";

                    }
                }
                else
                {
                    if (bUptoLoadDyeMachine == true)
                    {

                        sql = "Select * from View_ProductionSchedule where  MachineID in (" + oProductionSchedule.MachineIDs + ") and LocationID=" + oProductionSchedule.LocationID + " and ProductionScheduleID in ( Select distinct(ProductionScheduleID) from View_ProductionScheduleDetail Where ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "'))  and RSState<7 ) and Activity='1' order by MachineID,StartTime ";
                    }
                    else
                    {
                        sql = "Select * from View_ProductionSchedule where ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "')) and MachineID in (" + oProductionSchedule.MachineIDs + ") and LocationID=" + oProductionSchedule.LocationID + " and Activity='1' order by MachineID,StartTime ";
                    }
                }
                _oProductionSchedule.ProductionScheduleList = ProductionSchedule.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oProductionSchedule.ProductionScheduleList != null)
                {
                    sPSIDs = ProductionSchedule.IDInString(_oProductionSchedule.ProductionScheduleList);
                }
                if (sPSIDs.Length > 0)
                {
                    sql = "SELECT * FROM View_ProductionScheduleDetail where ProductionScheduleID in (" + sPSIDs + ") and RSState<7";
                    if (bUptoLoadDyeMachine == true)
                    {
                        _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    else
                    {
                        _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.Gets(sPSIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                _oProductionSchedule = new ProductionSchedule();
                _oProductionSchedule.ErrorMessage = ex.Message;
            }

            if (gridas == "Day")
            {
                _oProductionSchedule.sDay = gridas;
            }
            else if (gridas == "Week")
            {
                _oProductionSchedule.sWeek = gridas;
            }
            else if (gridas == "Month")
            {
                _oProductionSchedule.sMonth = gridas;
            }
            return _oProductionSchedule;
        }

        #endregion

        #region Search By Job No
        [HttpPost]
        public JsonResult SearchByJobNo(ProductionSchedule oProductionSchedule)
        {
            _oProductionSchedule = new ProductionSchedule();
            _oProductionSchedule.ProductionScheduleList = new List<ProductionSchedule>();
            DateTime dateTime = DateTime.Now, startTime = DateTime.Now, endTime = DateTime.Now;
            string gridas = "";
            string sOrderNo = "";
            if (oProductionSchedule.ProductionScheduleDetails.Count > 0)
            {
                sOrderNo = oProductionSchedule.ProductionScheduleDetails[0].OrderNo;
            }

            string sPSIDs = "";
            try
            {
                string sql = "";
                if (sOrderNo != "" & oProductionSchedule.LocationID > 0)
                {
                    sql = "Select * from View_ProductionSchedule where LocationID=" + oProductionSchedule.LocationID + " and Activity='1' and ProductionScheduleID in (Select ProductionScheduleID from View_ProductionScheduleDetail Where OrderNo LIKE '%" + sOrderNo + "%') order by StartTime ";
                }

                if (sql != "")
                {
                    _oProductionSchedule.ProductionScheduleList = ProductionSchedule.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oProductionSchedule.ProductionScheduleList.Count > 0)
                    {
                        int nCount = _oProductionSchedule.ProductionScheduleList.Count;
                        _oProductionSchedule.TotalJobCount = nCount;

                        string sMinMonth = _oProductionSchedule.ProductionScheduleList[0].StartTimeInPerfect.Split(' ')[1];
                        int nMinYear = Convert.ToInt32(_oProductionSchedule.ProductionScheduleList[0].StartTimeInPerfect.Split(' ')[2]);
                        string sMinMonthYear = sMinMonth + " " + nMinYear.ToString();

                        string sMaxMonth = sMinMonth;
                        int nMaxYear = nMinYear;
                        string sMaxMonthYear = sMinMonthYear;

                        if (nCount > 1)
                        {
                            sMaxMonth = _oProductionSchedule.ProductionScheduleList[nCount - 1].StartTimeInPerfect.Split(' ')[1];
                            nMaxYear = Convert.ToInt32(_oProductionSchedule.ProductionScheduleList[nCount - 1].StartTimeInPerfect.Split(' ')[2]);
                            sMaxMonthYear = sMaxMonth + " " + nMaxYear.ToString();
                        }

                        List<ProductionSchedule> oPSs = new List<ProductionSchedule>();
                        oPSs = _oProductionSchedule.ProductionScheduleList;
                        List<JobPerMonth> oJPMS = new List<JobPerMonth>();
                        JobPerMonth oJPM = new JobPerMonth();
                        string[] Month = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

                        int nMinMonth = 0;
                        int nMaxMonth = 0;
                        for (int i = 0; i < Month.Length; i++)
                        {
                            if (Month[i] == sMinMonth)
                            {
                                nMinMonth = i;
                                i = Month.Length;
                            }
                        }
                        for (int i = 0; i < Month.Length; i++)
                        {
                            if (Month[i] == sMaxMonth)
                            {
                                nMaxMonth = i;
                                i = Month.Length;
                            }
                        }
                        if (sMinMonth != sMaxMonth && nMinYear == nMaxYear)
                        {
                            int tempTotal = 0;
                            for (int i = 0; i < nCount; i++)
                            {

                                if (oPSs[i].StartTimeInPerfect.Contains(sMinMonthYear))
                                {
                                    tempTotal = tempTotal + 1;
                                }
                                else
                                {
                                    oJPM = new JobPerMonth();
                                    oJPM.MonthOfYear = sMinMonthYear;
                                    oJPM.JobCount = tempTotal;
                                    oJPMS.Add(oJPM);
                                    tempTotal = 0;
                                    if (nMinMonth < nMaxMonth)
                                    {
                                        sMinMonthYear = Month[++nMinMonth] + " " + nMinYear.ToString();
                                        if (oPSs[i].StartTimeInPerfect.Contains(sMinMonthYear))
                                        {
                                            tempTotal = tempTotal + 1;
                                            if (i == nCount - 1)
                                            {
                                                oJPM = new JobPerMonth();
                                                oJPM.MonthOfYear = sMinMonthYear;
                                                oJPM.JobCount = tempTotal;
                                                oJPMS.Add(oJPM);
                                            }
                                        }
                                    }

                                }

                            }
                        }
                        else if (sMinMonth != sMaxMonth && nMinYear < nMaxYear)
                        {
                            int tempTotal = 0;
                            for (int i = 0; i < nCount; i++)
                            {

                                if (oPSs[i].StartTimeInPerfect.Contains(sMinMonthYear))
                                {
                                    tempTotal = tempTotal + 1;
                                }
                                else
                                {
                                    oJPM = new JobPerMonth();
                                    oJPM.MonthOfYear = sMinMonthYear;
                                    oJPM.JobCount = tempTotal;
                                    oJPMS.Add(oJPM);
                                    tempTotal = 0;
                                    if (nMinMonth == 11)
                                    {
                                        nMinYear = nMinYear + 1;
                                        nMinMonth = 0;
                                    }
                                    if (nMinMonth < nMaxMonth)
                                    {
                                        sMinMonthYear = Month[++nMinMonth] + " " + nMinYear.ToString();
                                        if (oPSs[i].StartTimeInPerfect.Contains(sMinMonthYear))
                                        {
                                            tempTotal = tempTotal + 1;
                                            if (i == nCount - 1)
                                            {
                                                oJPM = new JobPerMonth();
                                                oJPM.MonthOfYear = sMinMonthYear;
                                                oJPM.JobCount = tempTotal;
                                                oJPMS.Add(oJPM);
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        else if (sMinMonthYear == sMaxMonthYear)
                        {
                            int tempTotal = 0;
                            for (int i = 0; i < nCount; i++)
                            {

                                if (oPSs[i].StartTimeInPerfect.Contains(sMinMonthYear))
                                {
                                    tempTotal = tempTotal + 1;
                                }

                            }
                            oJPM = new JobPerMonth();
                            oJPM.MonthOfYear = sMinMonthYear;
                            oJPM.JobCount = tempTotal;
                            oJPMS.Add(oJPM);
                        }

                        _oProductionSchedule.JobPerMonths = oJPMS;

                    }
                    else
                    {
                        throw new Exception("Nothing found for this exection no.");
                    }
                    dateTime = _oProductionSchedule.ProductionScheduleList[0].StartTime;
                    startTime = new DateTime(dateTime.Year, dateTime.Month, 1);
                    endTime = new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month), 23, 59, 59);
                    sql = "Select * from View_ProductionSchedule where ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "')) and LocationID=" + oProductionSchedule.LocationID + " and Activity='1' and ProductionScheduleID in (Select ProductionScheduleID from View_ProductionScheduleDetail Where OrderNo LIKE '%" + sOrderNo + "%' ) order by MachineID,StartTime ";
                    _oProductionSchedule.ProductionScheduleList = ProductionSchedule.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oProductionSchedule.ProductionScheduleList.Count > 0)
                    {
                        sPSIDs = ProductionSchedule.IDInString(_oProductionSchedule.ProductionScheduleList);
                    }
                    if (sPSIDs.Length > 0)
                    {
                        _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.Gets(sPSIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        foreach (ProductionSchedule oItem in _oProductionSchedule.ProductionScheduleList)
                        {
                            this.AddOrderDetail(oItem, "View");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _oProductionSchedule = new ProductionSchedule();
                _oProductionSchedule.ErrorMessage = ex.Message;
            }


            _oProductionSchedule.sMonth = gridas;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Add order Detail

        private void AddOrderDetail(ProductionSchedule oProductionSchedule, string sValue)
        {
            if (sValue == "View")
            {

                #region New Version

                List<ProductionScheduleDetail> oTempPSDs = new List<ProductionScheduleDetail>();
                oTempPSDs = _oProductionSchedule.ProductionScheduleDetails;

                //  string sFactory = string.Join(", ", (from p in oTempPSDs where p.ProductionScheduleID == oProductionSchedule.ProductionScheduleID && p.FactoryName.Trim() != "" select p.FactoryName.Trim()).Distinct().ToList());
                // string sBuyerRef = string.Join(", ", (from p in oTempPSDs where p.ProductionScheduleID == oProductionSchedule.ProductionScheduleID && p.BuyerRef.Trim() != "" select p.BuyerRef.Trim()).Distinct().ToList());
                // string sOrderNo = string.Join(", ", (from p in oTempPSDs where p.ProductionScheduleID == oProductionSchedule.ProductionScheduleID && p.OrderNo.Trim() != "" select p.OrderNo.Trim()).Distinct().ToList());
                //  string sRSState = string.Join(", ", (from p in oTempPSDs where p.ProductionScheduleID == oProductionSchedule.ProductionScheduleID && p.RSStateInString.Trim() != "" select p.RSStateInString.Trim()).Distinct().ToList());
                string sYarnName = string.Join(", ", (from p in oTempPSDs where p.ProductionScheduleID == oProductionSchedule.ProductionScheduleID && p.ProductName.Trim() != "" select p.ProductName.Trim()).Distinct().ToList()).Trim(',');
                string sColorName = string.Join(", ", (from p in oTempPSDs where p.ProductionScheduleID == oProductionSchedule.ProductionScheduleID && p.ColorName.Trim() != "" select p.ColorName.Trim() + "/" + p.PSBatchNo).Distinct().ToList());
                // string sBatchCardNo = string.Join(", ", (from p in oTempPSDs where p.ProductionScheduleID == oProductionSchedule.ProductionScheduleID && p.BatchCardNo.Trim() != "" select p.BatchCardNo).Distinct().ToList());
                //  string sRemarks = string.Join(", ", (from p in oTempPSDs where p.ProductionScheduleID == oProductionSchedule.ProductionScheduleID && p.Remarks.Trim() != "" select p.Remarks.Trim()).Distinct().ToList());
                string sTotal = Global.MillionFormat(Convert.ToDouble((from p in oTempPSDs where p.ProductionScheduleID == oProductionSchedule.ProductionScheduleID select p.ProductionQty).Sum())) + " kg";

                //sFactory= (sFactory != "") ? "" + sFactory + " <br/>" : "";
                //  sBuyerRef = (sBuyerRef != "") ? "BYR- " + sBuyerRef + "<br/> " : "";
                //sOrderNo = (sOrderNo != "") ? "" + sOrderNo + " <br/>" : "";
                //sRSState = (sRSState != "") ? "" + sRSState + " <br/>" : "";
                sYarnName = (sYarnName != "") ? "" + sYarnName + " <br/>" : "";
                sColorName = (sColorName != "") ? "" + sColorName + " <br/>" : "";
                //sBatchCardNo = (sBatchCardNo != "") ? "" + sBatchCardNo + " <br/>" : "";
                //sRemarks = (sRemarks != "") ? "" + sRemarks + " <br/>" : "";
                sTotal = (sTotal != "") ? "" + sTotal + " <br/>" : "";

                oProductionSchedule.OrderDetail = sYarnName + sColorName + "Qty: " + sTotal;
                #endregion

                #region Old Version
                //bool bflag = false;
                //foreach (ProductionScheduleDetail oPSD in _oProductionSchedule.ProductionScheduleDetails)
                //{
                //    if (oPSD.ProductionScheduleID == oProductionSchedule.ProductionScheduleID)
                //    {
                //        if (oPSD.BuyerName == "" || (oPSD.BuyerName.Trim() == oPSD.FactoryName.Trim()) || oPSD.Remarks == "")
                //        {
                //            //oProductionSchedule.OrderDetail = oProductionSchedule.OrderDetail + "\n" + oPSD.OrderNo + "\n " + oPSD.FactoryName + "\n " + "Color: " + oPSD.ColorName + "\n " + oPSD.ProductName + "\n " + "Remark: " + oPSD.Remarks + " ";
                //            if (!bflag)
                //            {
                //                bflag = true;
                //                oProductionSchedule.OrderDetail = oPSD.FactoryName + "<br/>" + "BYR- " + oPSD.BuyerRef + "<br/> " + oPSD.OrderNo + "<br/>" + oPSD.RSStateInString + "<br/>" + oPSD.ProductName + "<br/> " + oPSD.ColorName + "/" + oPSD.PSBatchNo + " ";
                //            }
                //            else
                //            {
                //                oProductionSchedule.OrderDetail = oProductionSchedule.OrderDetail + "<br/>" + oPSD.FactoryName + "<br/>" + "BYR- " + oPSD.BuyerRef + "<br/> " + oPSD.OrderNo + "<br/>" + oPSD.RSStateInString + "<br/>" + oPSD.ProductName + "<br/> " + oPSD.ColorName + "/" + oPSD.PSBatchNo + " ";
                //            }
                //        }
                //        else if ((oPSD.BuyerName == "" || (oPSD.BuyerName.Trim() == oPSD.FactoryName.Trim())) && oPSD.Remarks != "")
                //        {
                //            //oProductionSchedule.OrderDetail = oProductionSchedule.OrderDetail + "\n" + oPSD.OrderNo + "\n " + oPSD.FactoryName + "\n " + "Color: " + oPSD.ColorName + "\n " + oPSD.ProductName + "\n " + "Remark: " + oPSD.Remarks + " ";
                //            if (!bflag)
                //            {
                //                bflag = true;
                //                oProductionSchedule.OrderDetail = oPSD.FactoryName + "<br/>" + "BYR- " + oPSD.BuyerRef + "<br/> " + oPSD.OrderNo + "<br/>" + oPSD.RSStateInString + "<br/>" + oPSD.ProductName + "<br/> " + oPSD.ColorName + "/" + oPSD.PSBatchNo + "<br/>" + oPSD.Remarks + " ";
                //            }
                //            else
                //            {
                //                oProductionSchedule.OrderDetail = oProductionSchedule.OrderDetail + "<br/>" + oPSD.FactoryName + "<br/>" + "BYR- " + oPSD.BuyerRef + "<br/> " + oPSD.OrderNo + "<br/>" + oPSD.RSStateInString + "<br/>" + oPSD.ProductName + "<br/> " + oPSD.ColorName + "/" + oPSD.PSBatchNo + "<br/>" + oPSD.Remarks + " ";
                //            }
                //        }
                //        else
                //        {
                //            //oProductionSchedule.OrderDetail = oProductionSchedule.OrderDetail + "\n" + oPSD.OrderNo + " \n" + oPSD.BuyerName + " \n" + oPSD.FactoryName + "\n " + "Color: " + oPSD.ColorName + " \n" + oPSD.ProductName + " \n" + "Remark: " + oPSD.Remarks + " ";
                //            if (!bflag)
                //            {
                //                bflag = true;
                //                if (oPSD.Remarks == "")
                //                {
                //                    oProductionSchedule.OrderDetail = oPSD.FactoryName + "<br/>" + "BYR- " + oPSD.BuyerRef + "<br/>" + oPSD.BuyerName + "<br/>" + oPSD.OrderNo + "<br/>" + oPSD.RSStateInString + "<br/>" + oPSD.ProductName + "<br/>" + oPSD.ColorName + "/" + oPSD.PSBatchNo + " ";
                //                }
                //                else
                //                {
                //                    oProductionSchedule.OrderDetail = oPSD.FactoryName + "<br/>" + "BYR- " + oPSD.BuyerRef + "<br/>" + oPSD.BuyerName + "<br/>" + oPSD.OrderNo + "<br/>" + oPSD.RSStateInString + "<br/>" + oPSD.ProductName + "<br/>" + oPSD.ColorName + "/" + oPSD.PSBatchNo + "<br/>" + oPSD.Remarks + " ";
                //                }
                //            }
                //            else
                //            {
                //                if (oPSD.Remarks == "")
                //                {
                //                    oProductionSchedule.OrderDetail = oProductionSchedule.OrderDetail + "<br/>" + oPSD.FactoryName + "<br/>" + "BYR- " + oPSD.BuyerRef + "<br/>" + oPSD.BuyerName + "<br/>" + oPSD.OrderNo + "<br/>" + oPSD.RSStateInString + "<br/>" + oPSD.ProductName + "<br/>" + oPSD.ColorName + "/" + oPSD.PSBatchNo + " ";
                //                }
                //                else
                //                {
                //                    oProductionSchedule.OrderDetail = oProductionSchedule.OrderDetail + "<br/>" + oPSD.FactoryName + "<br/>" + "BYR- " + oPSD.BuyerRef + "<br/>" + oPSD.BuyerName + "<br/>" + oPSD.OrderNo + "<br/>" + oPSD.RSStateInString + "<br/>" + oPSD.ProductName + "<br/>" + oPSD.ColorName + "/" + oPSD.PSBatchNo + "<br/>" + oPSD.Remarks + " ";
                //                }

                //            }
                //        }
                //    }
                //}
                #endregion
            }
            else if (sValue == "Print")
            {
                oProductionSchedule.ProductionScheduleDetails = new List<ProductionScheduleDetail>();
                foreach (ProductionScheduleDetail oPSD in _oProductionSchedule.ProductionScheduleDetails)
                {

                    if (oPSD.ProductionScheduleID == oProductionSchedule.ProductionScheduleID)
                    {
                        oProductionSchedule.ProductionScheduleDetails.Add(oPSD);
                    }

                }
            }

        }

        #endregion

        #region Order Change

        [HttpPost]
        public JsonResult Update(ProductionSchedule oProductionSchedule)
        {
            try
            {

                oProductionSchedule.Update(oProductionSchedule.PSID1, oProductionSchedule.PSID2, oProductionSchedule.ProductionQtyFirst, oProductionSchedule.ProductionQtySecond, ((User)Session[SessionInfo.CurrentUser]).UserID);

                _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.Gets(oProductionSchedule.PSID1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oProductionSchedule = new ProductionSchedule();
                _oProductionSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Machien Change



        #endregion

        #region View PDF
        public ActionResult PrintProductionSchedule(string newstring, bool bPortrait)
        {

            _oProductionSchedule = new ProductionSchedule();

            int cSpace = Convert.ToInt32(newstring.Split('~')[0]);
            DateTime StartSearchTime = Convert.ToDateTime(newstring.Split('~')[1]);
            DateTime EndSearchTime = Convert.ToDateTime(newstring.Split('~')[2]);
            int LocationId = Convert.ToInt32(newstring.Split('~')[3]);
            string DyeMachineIds = Convert.ToString(newstring.Split('~')[4]);
            string DateProductionScheduleOf = Convert.ToString(newstring.Split('~')[5]);
            string sPrintView = Convert.ToString(newstring.Split('~')[6]);
            string sProductionScheduleIDs = Convert.ToString(newstring.Split('~')[7]);
            bool bUptoLoadDyeMachine = Convert.ToBoolean(newstring.Split('~')[8]);
            int buid = Convert.ToInt32(newstring.Split('~')[9]);

            double nTotalPSQty = 0;

            DateTime startTime = DateTime.Now, endTime = DateTime.Now, today = DateTime.Now;
            if (sPrintView == "viewAsDateMonth")
            {
                if (cSpace == 1)
                {
                    startTime = new DateTime(StartSearchTime.Year, StartSearchTime.Month, 1);
                    endTime = new DateTime(StartSearchTime.Year, StartSearchTime.Month, DateTime.DaysInMonth(StartSearchTime.Year, StartSearchTime.Month));
                }

                else if (cSpace == 2)
                {
                    startTime = new DateTime(StartSearchTime.Year, StartSearchTime.Month, StartSearchTime.Day, 0, 0, 0);
                    endTime = new DateTime(StartSearchTime.Year, StartSearchTime.Month, StartSearchTime.Day, 23, 59, 59);
                }
            }
            else if (sPrintView == "viewAsWeek")
            {

                startTime = new DateTime(StartSearchTime.Year, StartSearchTime.Month, StartSearchTime.Day, 0, 0, 0);
                endTime = new DateTime(EndSearchTime.Year, EndSearchTime.Month, EndSearchTime.Day, 23, 59, 59);
            }


            try
            {
                string sql = "", sPSIDs = "";
                _oProductionSchedule.MachineIDs = DyeMachineIds;
                if (String.IsNullOrEmpty(_oProductionSchedule.MachineIDs) && String.IsNullOrEmpty(sProductionScheduleIDs))
                {
                    if (bUptoLoadDyeMachine == true)
                    {
                        sql = "Select * from View_ProductionSchedule where LocationID=" + LocationId + " and ProductionScheduleID in ( Select distinct(ProductionScheduleID) from View_ProductionScheduleDetail Where  ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "')) and RSState<7 ) and  Activity='1' order by MachineID,StartTime ";
                    }
                    else
                    {
                        sql = "Select * from View_ProductionSchedule where ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "')) and LocationID=" + LocationId + " and   and Activity='1' order by MachineID,StartTime ";
                    }
                }
                else if (!String.IsNullOrEmpty(_oProductionSchedule.MachineIDs) && !String.IsNullOrEmpty(sProductionScheduleIDs))
                {
                    if (bUptoLoadDyeMachine == true)
                    {
                        sql = "Select * from View_ProductionSchedule where ProductionScheduleID in ( Select distinct(ProductionScheduleID) from View_ProductionScheduleDetail Where RSState<7 and ProductionScheduleID in (" + sProductionScheduleIDs + ") ) AND ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "')) and LocationID=" + LocationId + " and Activity='1' order by MachineID,StartTime ";
                    }
                    else
                    {
                        sql = "Select * from View_ProductionSchedule where ProductionScheduleID in  (" + sProductionScheduleIDs + ") AND ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "')) and LocationID=" + LocationId + " and Activity='1' order by MachineID,StartTime ";
                    }
                }
                else
                {
                    if (bUptoLoadDyeMachine == true)
                    {
                        sql = "Select * from View_ProductionSchedule where LocationID=" + LocationId + " and ProductionScheduleID in ( Select distinct(ProductionScheduleID) from View_ProductionScheduleDetail Where  ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "')) and MachineID in (" + DyeMachineIds + ") and RSState<7 ) and Activity='1' order by MachineID,StartTime ";
                    }
                    else
                    {
                        sql = "Select * from View_ProductionSchedule where ((StartTime>='" + startTime + "' and StartTime<='" + endTime + "') or (EndTime>='" + startTime + "' and EndTime<='" + endTime + "')) and MachineID in (" + DyeMachineIds + ") and LocationID=" + LocationId + " and Activity='1' order by MachineID,StartTime ";
                    }
                }

                _oProductionSchedule.ProductionScheduleList = ProductionSchedule.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oProductionSchedule.ProductionScheduleList != null)
                {
                    sPSIDs = ProductionSchedule.IDInString(_oProductionSchedule.ProductionScheduleList);
                    List<int> UniqueDyeMachineId = new List<int>();
                    List<int> DMachineIds = new List<int>();
                    foreach (ProductionSchedule oItem in _oProductionSchedule.ProductionScheduleList)
                    {
                        DMachineIds.Add(oItem.MachineID);
                    }
                    _oProductionSchedule.UniqueDyeMachineId = DMachineIds.Distinct().ToList();
                }
                if (sPSIDs.Length > 0)
                {
                    sql = "SELECT * FROM View_ProductionScheduleDetail where ProductionScheduleID in (" + sPSIDs + ") and RSState<7";
                    if (bUptoLoadDyeMachine == true)
                    {
                        _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    else
                    {
                        _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.Gets(sPSIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }

                    foreach (ProductionSchedule oItem in _oProductionSchedule.ProductionScheduleList)
                    {
                        this.AddOrderDetail(oItem, "Print");
                        nTotalPSQty = nTotalPSQty + oItem.ProductionQty;
                    }
                }

                if (_oProductionSchedule.ProductionScheduleList.Count() > 0)
                {
                    _oProductionSchedule.MaxValue = _oProductionSchedule.ProductionScheduleList.GroupBy(x => x.MachineID).Max(t => t.Count());
                }

            }
            catch (Exception ex)
            {
                _oProductionSchedule = new ProductionSchedule();
                _oProductionSchedule.ErrorMessage = ex.Message;
            }

            _oProductionSchedule.ProductionScheduleOf = DateProductionScheduleOf;

            if (sPrintView == "viewAsDateMonth")
            {
                if (cSpace == 1)
                {
                    _oProductionSchedule.sMonth = "Month";
                }
                else if (cSpace == 2)
                {
                    _oProductionSchedule.sDay = "Day";
                }

            }
            else if (sPrintView == "viewAsWeek")
            {
                _oProductionSchedule.sWeek = "Week";
            }

            double nGrandTotal = 0, nTotalQtyInHouse = 0, nTotalQtyOutside = 0, nDoubeYarn = 0;
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            string sSQL = "Select * from View_ProductionScheduleDetail Where RSState<7";
            oProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oProductionScheduleDetails.Count() > 0)
            {
                nGrandTotal = oProductionScheduleDetails.Sum(x => x.ProductionQty);
                nTotalQtyInHouse = (from oPSD in oProductionScheduleDetails where oPSD.IsInHouse == true select oPSD.ProductionQty).Sum();
                nTotalQtyOutside = (from oPSD in oProductionScheduleDetails where oPSD.IsInHouse == false select oPSD.ProductionQty).Sum();
                nDoubeYarn = oProductionScheduleDetails.Where(x => x.OrderNo.Contains("55-")).Sum(o => o.ProductionQty);
            }

            Company oCompany = new Company();
            _oProductionSchedule.Company = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.Company.CompanyLogo = GetCompanyLogo(_oProductionSchedule.Company);
            string sSql = "SELECT * FROM VIEW_CapitalResource WHERE CRID IN (" + DyeMachineIds + ") AND BUID = " + buid + " AND IsActive=1";
            _oProductionSchedule.CapitalResources = CapitalResource.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            byte[] abytes;
            if (bPortrait)
            {
                abytes = PrintProductionScheduleInPortrait(_oProductionSchedule, _oProductionSchedule.Company, nTotalPSQty, nGrandTotal, nTotalQtyInHouse, nTotalQtyOutside, nDoubeYarn);
            }
            else
            {
                abytes = PrintProductionScheduleInLandScape(_oProductionSchedule, _oProductionSchedule.Company, nTotalPSQty, nGrandTotal, nTotalQtyInHouse, nTotalQtyOutside, nDoubeYarn);
            }
            return File(abytes, "application/pdf");
        }


        private byte[] PrintProductionScheduleInPortrait(ProductionSchedule oPS, Company oCompany, double nTotalPSQty, double nGrandTotal, double nTotalQtyInHouse, double nTotalQtyOutside, double nDoubeYarn)
        {
            byte[] abytes;
            rptProductionScheduleReportPortrait oReport = new rptProductionScheduleReportPortrait();
            abytes = oReport.PrepareReport(oPS, oCompany, nTotalPSQty, nGrandTotal, nTotalQtyInHouse, nTotalQtyOutside, nDoubeYarn);
            return abytes;

        }
        private byte[] PrintProductionScheduleInLandScape(ProductionSchedule oPS, Company oCompany, double nTotalPSQty, double nGrandTotal, double nTotalQtyInHouse, double nTotalQtyOutside, double nDoubeYarn)
        {
            byte[] abytes;
            rptProductionScheduleReport oReport = new rptProductionScheduleReport();
            abytes = oReport.PrepareReport(oPS, oCompany, nTotalPSQty, nGrandTotal, nTotalQtyInHouse, nTotalQtyOutside, nDoubeYarn);
            return abytes;

        }

        public ActionResult PrintProductionScheduleFromTracking(string sPSIDs, bool bPortrait, int buid) // sPSIDs => Indicates Production Schedule IDs
        {

            double nTotalPSQty = 0;
            try
            {
                string sql = "";
                sql = "Select * from View_ProductionSchedule where ProductionScheduleID in (" + sPSIDs + ") and Activity='1' order by MachineID,StartTime ";

                _oProductionSchedule.ProductionScheduleList = ProductionSchedule.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oProductionSchedule.ProductionScheduleList != null)
                {
                    sPSIDs = ProductionSchedule.IDInString(_oProductionSchedule.ProductionScheduleList);
                    List<int> UniqueDyeMachineId = new List<int>();
                    List<int> DMachineIds = new List<int>();
                    foreach (ProductionSchedule oItem in _oProductionSchedule.ProductionScheduleList)
                    { DMachineIds.Add(oItem.MachineID); }
                    _oProductionSchedule.UniqueDyeMachineId = DMachineIds.Distinct().ToList();
                }

                _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.Gets(sPSIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ProductionSchedule oItem in _oProductionSchedule.ProductionScheduleList)
                {
                    this.AddOrderDetail(oItem, "Print");
                    nTotalPSQty = nTotalPSQty + oItem.ProductionQty;
                }
                if (_oProductionSchedule.ProductionScheduleList.Count() > 0)
                {
                    _oProductionSchedule.MaxValue = _oProductionSchedule.ProductionScheduleList.GroupBy(x => x.MachineID).Max(t => t.Count());
                }

            }
            catch (Exception ex)
            {
                _oProductionSchedule = new ProductionSchedule();
                _oProductionSchedule.ErrorMessage = ex.Message;
            }

            _oProductionSchedule.ProductionScheduleOf = "";



            double nGrandTotal = 0, nTotalQtyInHouse = 0, nTotalQtyOutside = 0, nDoubeYarn = 0;
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            string sSQL = "Select * from View_ProductionScheduleDetail Where RSState<7";
            oProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oProductionScheduleDetails.Count() > 0)
            {
                nGrandTotal = oProductionScheduleDetails.Sum(x => x.ProductionQty);
                nTotalQtyInHouse = (from oPSD in oProductionScheduleDetails where oPSD.IsInHouse == true select oPSD.ProductionQty).Sum();
                nTotalQtyOutside = (from oPSD in oProductionScheduleDetails where oPSD.IsInHouse == false select oPSD.ProductionQty).Sum();
                nDoubeYarn = oProductionScheduleDetails.Where(x => x.OrderNo.Contains("55-")).Sum(o => o.ProductionQty);
            }

            Company oCompany = new Company();
            _oProductionSchedule.Company = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.Company.CompanyLogo = GetCompanyLogo(_oProductionSchedule.Company);
            string sSql = "SELECT * FROM VIEW_CapitalResource WHERE CRID IN (" + string.Join(",", _oProductionSchedule.ProductionScheduleList.Where(x => x.MachineID > 0).Select(x => x.MachineID).Distinct().ToList()) + ") AND BUID = " + buid + " AND IsActive=1";
            _oProductionSchedule.CapitalResources = CapitalResource.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            // return this.ViewPdf("", "rptProductionSchedule", _oProductionSchedule, PageSize.A4, 40, 40, 40, 40, true);

            byte[] abytes;
            if (bPortrait)
            {
                abytes = PrintProductionScheduleInPortrait(_oProductionSchedule, _oProductionSchedule.Company, nTotalPSQty, nGrandTotal, nTotalQtyInHouse, nTotalQtyOutside, nDoubeYarn);
            }
            else
            {
                abytes = PrintProductionScheduleInLandScape(_oProductionSchedule, _oProductionSchedule.Company, nTotalPSQty, nGrandTotal, nTotalQtyInHouse, nTotalQtyOutside, nDoubeYarn);
            }


            return File(abytes, "application/pdf");
        }

        #endregion

        #region Get Company Logo

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

        #region view production  schedule detail

        public ActionResult ViewProductionScheduleDetails(int menuid, string sActionlink)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            ProductionSchedule oProductionSchedule = new ProductionSchedule();
            ProductionScheduleDetail oProductionScheduleDetail = new ProductionScheduleDetail();
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            DateTime currentDate = DateTime.Now;

            List<AuthorizationUserOEDO> oAUOEDOS = new List<AuthorizationUserOEDO>();
            string sSQL = "SELECT * FROM View_AuthorizationUserOEDO where (DBObjectName='ProductionTracingUnit' OR DBObjectName='DyeingOrder')  and UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "";
            oAUOEDOS = AuthorizationUserOEDO.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            bool bApprove = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._View, "ProductionTracingUnit", oAUOEDOS);
            TempData["Details"] = bApprove;

            sSQL = "SELECT * FROM View_ProductionScheduleDetail where RSState<7";
            oProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oProductionScheduleDetails.Count() > 0)
            {
                oProductionScheduleDetails = oProductionScheduleDetails.OrderBy(x => x.MachineName).ThenBy(x => x.StartTime).ThenBy(x => x.ProductionScheduleNo).ToList();
            }
            oProductionSchedule.ProductionScheduleDetails = oProductionScheduleDetails;

            #region Data Collect
            //oProductionSchedule.sActionLink = sActionlink;
            //string sSQL = "";
            //if (oProductionSchedule.sActionLink == "Schedule Detail" || oProductionSchedule.sActionLink == null)
            //{
            //    sSQL = "select * from View_ProductionScheduleDetail where [EndTime]>='" + currentDate + "'";
            //    _oDataSet = ProductionSchedule.GetsGroupBy(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //    foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
            //    {
            //        oProductionScheduleDetail = new ProductionScheduleDetail();
            //        oProductionScheduleDetail.ProductionScheduleDetailID = (oRow["ProductionScheduleDetailID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductionScheduleDetailID"]);
            //        oProductionScheduleDetail.ProductionScheduleID = (oRow["ProductionScheduleID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductionScheduleID"]);
            //        oProductionScheduleDetail.ProductionTracingUnitID = (oRow["ProductionTracingUnitID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductionTracingUnitID"]);
            //        oProductionScheduleDetail.ProductionQty = (oRow["ProductionQty"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductionQty"]);
            //        oProductionScheduleDetail.DBUserID = (oRow["DBUserID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DBUserID"]);
            //        oProductionScheduleDetail.LocationName = (oRow["LocationName"] == DBNull.Value) ? "" : Convert.ToString(oRow["LocationName"]);
            //        oProductionScheduleDetail.MachineName = (oRow["MachineName"] == DBNull.Value) ? "" : Convert.ToString(oRow["MachineName"]);
            //        oProductionScheduleDetail.MachineNo = (oRow["MachineNo"] == DBNull.Value) ? "" : Convert.ToString(oRow["MachineNo"]);
            //        oProductionScheduleDetail.StartTime = (oRow["StartTime"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oRow["StartTime"]);
            //        oProductionScheduleDetail.EndTime = (oRow["EndTime"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oRow["EndTime"]);
            //        oProductionScheduleDetail.OrderNo = (oRow["OrderNo"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderNo"]);
            //        oProductionScheduleDetail.BuyerName = (oRow["BuyerName"] == DBNull.Value) ? "" : Convert.ToString(oRow["BuyerName"]);
            //        oProductionScheduleDetail.ColorName = (oRow["ColorName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ColorName"]);
            //        oProductionScheduleDetail.ProductName = (oRow["ProductName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductName"]);
            //        oProductionScheduleDetail.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
            //        oProductionScheduleDetail.FactoryName = (oRow["FactoryName"] == DBNull.Value) ? "" : Convert.ToString(oRow["FactoryName"]);
            //        oProductionScheduleDetail.WaitingForProductionQty = (oRow["WaitingForProductionQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["WaitingForProductionQty"]);
            //        oProductionScheduleDetail.YetToProductionQty = (oRow["YetToProductionQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YetToProductionQty"]);

            //        oProductionScheduleDetails.Add(oProductionScheduleDetail);
            //    }
            //}
            //else if(oProductionSchedule.sActionLink=="Party Wise")
            //{
            //    sSQL = "select BuyerName,ProductName,SUM(ProductionQty)as ProductionQty  from View_ProductionScheduleDetail group by BuyerName,ProductName ";
            //    _oDataSet = ProductionSchedule.GetsGroupBy(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //    foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
            //    {
            //        oProductionScheduleDetail = new ProductionScheduleDetail();

            //        oProductionScheduleDetail.BuyerName = (oRow["BuyerName"] == DBNull.Value) ? "" : Convert.ToString(oRow["BuyerName"]);
            //        oProductionScheduleDetail.ProductName = (oRow["ProductName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductName"]);
            //        oProductionScheduleDetail.ProductionQty = (oRow["ProductionQty"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductionQty"]);

            //        oProductionScheduleDetails.Add(oProductionScheduleDetail);
            //    }
            //}

            //else if (oProductionSchedule.sActionLink == "Order Wise")
            //{
            //    sSQL = "select OrderNo,BuyerName,SUM(ProductionQty)as ProductionQty  from View_ProductionScheduleDetail group by OrderNo,BuyerName ";
            //    _oDataSet = ProductionSchedule.GetsGroupBy(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //    foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
            //    {
            //        oProductionScheduleDetail = new ProductionScheduleDetail();
            //        oProductionScheduleDetail.BuyerName = (oRow["BuyerName"] == DBNull.Value) ? "" : Convert.ToString(oRow["BuyerName"]);
            //        oProductionScheduleDetail.OrderNo = (oRow["OrderNo"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderNo"]);
            //        oProductionScheduleDetail.ProductionQty = (oRow["ProductionQty"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductionQty"]);

            //        oProductionScheduleDetails.Add(oProductionScheduleDetail);
            //    }
            //}

            //else if (oProductionSchedule.sActionLink=="Product Wise")
            //{
            //    sSQL = "select ProductName, SUM(ProductionQty) as ProductionQty from View_ProductionScheduleDetail group by ProductName";
            //    _oDataSet = ProductionSchedule.GetsGroupBy(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //    foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
            //    {
            //        oProductionScheduleDetail = new ProductionScheduleDetail();
            //        oProductionScheduleDetail.ProductName = (oRow["ProductName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductName"]);
            //        oProductionScheduleDetail.ProductionQty = (oRow["ProductionQty"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductionQty"]);

            //        oProductionScheduleDetails.Add(oProductionScheduleDetail);
            //    }
            //}
            //oProductionSchedule.ProductionScheduleDetails = oProductionScheduleDetails;
            #endregion

            return View(oProductionSchedule);
        }

        [HttpPost]
        public JsonResult viewGroupSearch(ProductionSchedule oProductionSchedule)
        {
            oProductionSchedule.ProductionScheduleDetails = ProductionScheduleTracking(oProductionSchedule.sActionLink, oProductionSchedule.sProductionScheduleIds);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }

        private List<ProductionScheduleDetail> ProductionScheduleTracking(string sType, string sIDs)
        {
            ProductionScheduleDetail oProductionScheduleDetail = new ProductionScheduleDetail();
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            DateTime currentDate = DateTime.Now;

            string sSQL = "";
            try
            {
                if (sType == "Party Wise")
                {
                    sSQL = "select BuyerName,ProductName,SUM(ProductionQty)as ProductionQty  from View_ProductionScheduleDetail where ProductionScheduleDetailID in (" + sIDs + ") group by BuyerName,ProductName ";
                    _oDataSet = ProductionSchedule.GetsGroupBy(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
                    {
                        oProductionScheduleDetail = new ProductionScheduleDetail();
                        oProductionScheduleDetail.BuyerName = (oRow["BuyerName"] == DBNull.Value) ? "" : Convert.ToString(oRow["BuyerName"]);
                        oProductionScheduleDetail.ProductName = (oRow["ProductName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductName"]);
                        oProductionScheduleDetail.ProductionQty = (oRow["ProductionQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["ProductionQty"]);

                        oProductionScheduleDetails.Add(oProductionScheduleDetail);
                    }
                }

                else if (sType == "Order Wise")
                {
                    sSQL = "select OrderNo,BuyerName,SUM(ProductionQty)as ProductionQty  from View_ProductionScheduleDetail where ProductionScheduleDetailID in (" + sIDs + ") group by OrderNo,BuyerName ";
                    _oDataSet = ProductionSchedule.GetsGroupBy(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
                    {
                        oProductionScheduleDetail = new ProductionScheduleDetail();
                        oProductionScheduleDetail.BuyerName = (oRow["BuyerName"] == DBNull.Value) ? "" : Convert.ToString(oRow["BuyerName"]);
                        oProductionScheduleDetail.OrderNo = (oRow["OrderNo"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderNo"]);
                        oProductionScheduleDetail.ProductionQty = (oRow["ProductionQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["ProductionQty"]);

                        oProductionScheduleDetails.Add(oProductionScheduleDetail);
                    }
                }

                else if (sType == "Product Wise")
                {
                    sSQL = "select ProductName, SUM(ProductionQty) as ProductionQty from View_ProductionScheduleDetail where ProductionScheduleDetailID in (" + sIDs + ") group by ProductName";
                    _oDataSet = ProductionSchedule.GetsGroupBy(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
                    {
                        oProductionScheduleDetail = new ProductionScheduleDetail();
                        oProductionScheduleDetail.ProductName = (oRow["ProductName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductName"]);
                        oProductionScheduleDetail.ProductionQty = (oRow["ProductionQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["ProductionQty"]);

                        oProductionScheduleDetails.Add(oProductionScheduleDetail);
                    }
                }

                if (oProductionScheduleDetails.Count() <= 0) { throw new Exception("No information found."); }

            }

            catch (Exception ex)
            {
                oProductionScheduleDetail = new ProductionScheduleDetail();
                oProductionScheduleDetails = new List<ProductionScheduleDetail>();
                oProductionScheduleDetail.ErrorMessage = ex.Message;
                oProductionScheduleDetails.Add(oProductionScheduleDetail);

            }

            return oProductionScheduleDetails;
        }


        [HttpPost]
        public JsonResult GetProductionScheduleTrackings(string sIDs, double nts)
        {
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            string sSQL = "";
            if (sIDs.Trim() != "")
            {
                sSQL = "Select * from  View_ProductionScheduleDetail where ProductionScheduleDetailID IN (" + sIDs + ") Order By StartTime DESC";
                oProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oProductionScheduleDetails.Count() > 0)
                {
                    oProductionScheduleDetails = oProductionScheduleDetails.OrderBy(x => x.MachineName).ThenBy(x => x.StartTime).ThenBy(x => x.ProductionScheduleNo).ToList();
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionScheduleDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult RefreshScheduleTrackings(string sIDs, double nts)
        {
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            string sSQL = "";
            if (sIDs.Trim() != "")
            {
                sSQL = "Select * from  View_ProductionScheduleDetail where ProductionScheduleID IN (" + sIDs + ") Order By StartTime DESC";
                oProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oProductionScheduleDetails.Count() > 0)
                {
                    oProductionScheduleDetails = oProductionScheduleDetails.OrderBy(x => x.MachineName).ThenBy(x => x.StartTime).ThenBy(x => x.ProductionScheduleNo).ToList();
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionScheduleDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Show group data


        [HttpPost]
        public JsonResult ShowGroupSearch(ProductionScheduleDetail oProductionScheduleDetail)
        {

            ProductionSchedule oProductionSchedule = new ProductionSchedule();
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            string sSQL = "";
            try
            {
                if (oProductionScheduleDetail.sActionLink == "Party Wise")
                {
                    sSQL = "select * from View_ProductionScheduleDetail where BuyerName='" + oProductionScheduleDetail.BuyerName + "' and ProductName='" + oProductionScheduleDetail.ProductName + "' ";
                }

                else if (oProductionScheduleDetail.sActionLink == "Order Wise")
                {
                    sSQL = "select * from View_ProductionScheduleDetail where OrderNo='" + oProductionScheduleDetail.OrderNo + "' and BuyerName='" + oProductionScheduleDetail.BuyerName + "' ";

                }

                else if (oProductionScheduleDetail.sActionLink == "Product Wise")
                {
                    sSQL = "select * from View_ProductionScheduleDetail where ProductName='" + oProductionScheduleDetail.ProductName + "'";

                }

                _oDataSet = ProductionSchedule.GetsGroupBy(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
                {
                    oProductionScheduleDetail = new ProductionScheduleDetail();
                    oProductionScheduleDetail.ProductionScheduleDetailID = (oRow["ProductionScheduleDetailID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductionScheduleDetailID"]);
                    oProductionScheduleDetail.ProductionScheduleID = (oRow["ProductionScheduleID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductionScheduleID"]);
                    oProductionScheduleDetail.ProductionTracingUnitID = (oRow["ProductionTracingUnitID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductionTracingUnitID"]);
                    oProductionScheduleDetail.ProductionQty = (oRow["ProductionQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["ProductionQty"]);
                    oProductionScheduleDetail.DBUserID = (oRow["DBUserID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DBUserID"]);
                    oProductionScheduleDetail.LocationName = (oRow["LocationName"] == DBNull.Value) ? "" : Convert.ToString(oRow["LocationName"]);
                    oProductionScheduleDetail.MachineName = (oRow["MachineName"] == DBNull.Value) ? "" : Convert.ToString(oRow["MachineName"]);
                    oProductionScheduleDetail.MachineNo = (oRow["MachineNo"] == DBNull.Value) ? "" : Convert.ToString(oRow["MachineNo"]);
                    oProductionScheduleDetail.StartTime = (oRow["StartTime"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oRow["StartTime"]);
                    oProductionScheduleDetail.EndTime = (oRow["EndTime"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oRow["EndTime"]);
                    oProductionScheduleDetail.OrderNo = (oRow["OrderNo"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderNo"]);
                    oProductionScheduleDetail.BuyerName = (oRow["BuyerName"] == DBNull.Value) ? "" : Convert.ToString(oRow["BuyerName"]);
                    oProductionScheduleDetail.ColorName = (oRow["ColorName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ColorName"]);
                    oProductionScheduleDetail.ProductName = (oRow["ProductName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductName"]);
                    oProductionScheduleDetail.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                    oProductionScheduleDetail.FactoryName = (oRow["FactoryName"] == DBNull.Value) ? "" : Convert.ToString(oRow["FactoryName"]);
                    oProductionScheduleDetail.WaitingForProductionQty = (oRow["WaitingForProductionQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["WaitingForProductionQty"]);
                    oProductionScheduleDetail.YetToProductionQty = (oRow["YetToProductionQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YetToProductionQty"]);

                    oProductionScheduleDetails.Add(oProductionScheduleDetail);
                }

                oProductionSchedule.ProductionScheduleDetails = oProductionScheduleDetails;


            }

            catch (Exception ex)
            {
                _oProductionSchedule = new ProductionSchedule();
                _oProductionSchedule.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Advance Search For Production Schedule

        #region View Picker

        public ActionResult ViewAdvanceSearchProductionSchedule(ProductionSchedule oProductionSchedule)
        {

            _oProductionSchedule.LocationList = Location.Gets(_sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.CapitalResources = CapitalResource.BUWiseGets(oProductionSchedule.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            return PartialView(_oProductionSchedule);

        }

        #endregion


        #region Search

        [HttpGet]
        public JsonResult SearchProductionSchedule(string Temp)
        {
            _oProductionScheduleList = new List<ProductionSchedule>();
            try
            {
                string sSQL = "";

                sSQL = GetSQL(Temp);
                _oProductionScheduleList = ProductionSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oProductionSchedule = new ProductionSchedule();
                _oProductionSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionScheduleList);
            return Json(sjson, JsonRequestBehavior.AllowGet);


        }

        private string GetSQL(string sTemp)
        {
            int nCreateDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dSOStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dSOEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sPSNo = sTemp.Split('~')[3];
            string sMachineIDs = sTemp.Split('~')[4];
            string sReturn1 = "SELECT * FROM View_ProductionSchedule";
            string sReturn = "";

            #region ps No

            if (sPSNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductionScheduleNo LIKE '%" + sPSNo + "%'";
            }
            #endregion


            #region Machine Name
            if (sMachineIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MachineID IN (" + sMachineIDs + ")";
            }
            #endregion

            #region start Date Wise
            if (nCreateDateCom > 0)
            {
                if (nCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "[StartTime]>= '" + dSOStartDate.ToString("dd MMM yyyy") + "' and [Endtime]<'" + dSOStartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "[StartTime]!= '" + dSOStartDate.ToString("dd MMM yyyy") + "' and [Endtime]!='" + dSOStartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " StartTime > '" + dSOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " StartTime < '" + dSOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " StartTime>= '" + dSOStartDate.ToString("dd MMM yyyy") + "' AND StartTime < '" + dSOEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " StartTime< '" + dSOStartDate.ToString("dd MMM yyyy") + "' OR StartTime > '" + dSOEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }

            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY ProductionScheduleID";
            return sReturn;
        }
        #endregion

        #endregion

        #region advance search Production Schedule Detail


        #region View Picker

        public ActionResult ViewAdvanceSearchProductionScheduleDetails(int buid)
        {
            _oProductionSchedule.LocationList = Location.Gets(_sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.CapitalResources = CapitalResource.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            return PartialView(_oProductionSchedule);
        }

        #endregion


        #region Search
        [HttpPost]
        public JsonResult AdvanceSearch(string sValue, double nts)
        {
            ProductionSchedule oProductionSchedule = new ProductionSchedule();
            ProductionScheduleDetail oProductionScheduleDetail = new ProductionScheduleDetail();
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();

            try
            {
                if (sValue.Trim() == "") { throw new Exception("Invalid search query."); }

                string sPlanDateType = sValue.Split('~')[0];
                DateTime dStartTime = Convert.ToDateTime(sValue.Split('~')[1]);
                DateTime dEndTime = Convert.ToDateTime(sValue.Split('~')[2]);
                string sLoadUnloadDateType = sValue.Split('~')[3];
                DateTime dLoadUnloadStartTime = Convert.ToDateTime(sValue.Split('~')[4]);
                DateTime dLoadUnloadEndTime = Convert.ToDateTime(sValue.Split('~')[5]);
                string sMachineIDs = sValue.Split('~')[6];
                int nLocationID = Convert.ToInt32(sValue.Split('~')[7]);
                string sProductIDs = sValue.Split('~')[8];
                string sExeNo = sValue.Split('~')[9];
                string sScheduleNo = sValue.Split('~')[10];
                string sLoadUnload = sValue.Split('~')[11];
                string sStatus = sValue.Split('~')[12];
                int nInHouse = Convert.ToInt32(sValue.Split('~')[13]);
                int nOrderType = Convert.ToInt32(sValue.Split('~')[14]);

                string sSQL = "";
                string sReturn1 = "SELECT * FROM View_ProductionScheduleDetail Where ProductionScheduleDetailID<>0";
                string sReturn = "";

                #region SQL Make

                #region Date Range Search In PS Detail

                if (sPlanDateType == "EqualTo")
                {
                    sReturn = sReturn + " AND [StartTime]>= '" + dStartTime + "' AND [Endtime]<'" + dStartTime.AddDays(1) + "'";
                }
                else if (sPlanDateType == "NotEqualTo")
                {
                    sReturn = sReturn + " AND [EventTime] Not Between '" + dStartTime + "' AND '" + dStartTime.AddDays(1) + "'";
                }
                else if (sPlanDateType == "GreaterThen")
                {
                    sReturn = sReturn + " AND [StartTime]> '" + dStartTime + "'";
                }
                else if (sPlanDateType == "SmallerThen")
                {
                    sReturn = sReturn + " AND [StartTime]< '" + dStartTime + "'";
                }
                else if (sPlanDateType == "Between")
                {
                    sReturn = sReturn + " AND [StartTime] between '" + dStartTime + "' AND '" + dEndTime + "'";
                }
                else if (sPlanDateType == "NotBetween")
                {
                    sReturn = sReturn + " AND [StartTime] not between '" + dStartTime + "' AND '" + dEndTime + "'";
                }

                #endregion

                #region Search By ProductID in PSD
                if (sProductIDs.Trim() != "")
                {
                    sReturn = sReturn + " AND ProductID IN (" + sProductIDs + ")";
                }

                #endregion

                #region Search By Exe No In PSD
                if (sExeNo.Trim() != "")
                {
                    sReturn = sReturn + " AND OrderNo= '" + sExeNo + "'";
                }
                #endregion

                #region GetProduction Schedule
                string sTempSQL = "";
                if (nLocationID > 0 || sMachineIDs.Trim() != "" || sScheduleNo.Trim() != "")
                {
                    sTempSQL = "Select ProductionScheduleID from ProductionSchedule Where ProductionScheduleID<>0 ";
                    if (nLocationID > 0)
                    {
                        sTempSQL = sTempSQL + " AND LocationID=" + nLocationID;
                    }
                    if (sMachineIDs.Trim() != "")
                    {
                        sTempSQL = sTempSQL + " AND MachineID IN (" + sMachineIDs + ")";
                    }
                    if (sScheduleNo.Trim() != "")
                    {
                        sTempSQL = sTempSQL + " AND ProductionScheduleNo = '" + sScheduleNo + "'";
                    }
                }
                #endregion

                #region DyeMachine Load/ Unload Search
                string sRHESQL = "";
                if ((sLoadUnload.Trim() == "DyeLoad" || sLoadUnload.Trim() == "DyeUnLoad") && sLoadUnloadDateType.Trim() != "None")
                {
                    int nEvent = 0;
                    if (sLoadUnload.Trim() == "DyeLoad") { nEvent = (int)EnumRSState.LoadedInDyeMachine; }
                    else if (sLoadUnload.Trim() == "DyeUnLoad") { nEvent = (int)EnumRSState.UnloadedFromDyeMachine; }

                    if (sStatus.Trim() == "BeforeDyeLoad") { sRHESQL = "Select Distinct(RouteSheetID) from RouteSheetHistory Where [Event]<" + nEvent + " AND "; }
                    else if (sStatus.Trim() == "AfterDyeUnLoad") { sRHESQL = "Select Distinct(RouteSheetID) from RouteSheetHistory Where [Event]>" + nEvent + " AND "; }
                    else { sRHESQL = "Select Distinct(RouteSheetID) from RouteSheetHistory Where [Event]=" + nEvent + " AND "; }

                    #region Event Time Search In RHE
                    if (sLoadUnloadDateType == "EqualTo")
                    {
                        sRHESQL = sRHESQL + "[EventTime]>= '" + dLoadUnloadStartTime + "' and [EventTime]<'" + dLoadUnloadStartTime.AddDays(1) + "'";
                    }
                    else if (sLoadUnloadDateType == "NotEqualTo")
                    {
                        sRHESQL = sRHESQL + "[EventTime] Not Between '" + dLoadUnloadStartTime + "' AND '" + dLoadUnloadStartTime.AddDays(1) + "'";
                    }
                    else if (sLoadUnloadDateType == "GreaterThen")
                    {
                        sRHESQL = sRHESQL + "[EventTime]> '" + dLoadUnloadStartTime + "'";
                    }
                    else if (sLoadUnloadDateType == "SmallerThen")
                    {
                        sRHESQL = sRHESQL + "[EventTime]< '" + dLoadUnloadStartTime + "'";
                    }
                    else if (sLoadUnloadDateType == "Between")
                    {
                        sRHESQL = sRHESQL + "[EventTime] between '" + dLoadUnloadStartTime + "' and '" + dLoadUnloadEndTime + "'";
                    }
                    else if (sLoadUnloadDateType == "NotBetween")
                    {
                        sRHESQL = sRHESQL + "[EventTime] not between '" + dLoadUnloadStartTime + "' and '" + dLoadUnloadEndTime + "'";
                    }
                    #endregion

                }
                #endregion

                #region Inside/Outside

                if (nInHouse == 1) { sReturn1 = sReturn1 + " AND IsInHouse=1"; }
                else if (nInHouse == 0) { sReturn1 = sReturn1 + " AND IsInHouse=0"; }

                #endregion
                #region Order Type

                if (nOrderType == (int)EnumOrderType.BulkOrder) { sReturn1 = sReturn1 + " AND OrderType=" + (int)EnumOrderType.BulkOrder + ""; }
                else if (nOrderType == (int)EnumOrderType.SampleOrder) { sReturn1 = sReturn1 + " AND OrderType=" + (int)EnumOrderType.SampleOrder + ""; }

                #endregion


                sSQL = sReturn1 + sReturn;
                if (sTempSQL != "") { sSQL = sSQL + " AND ProductionScheduleID IN (" + sTempSQL + ")"; }
                if (sRHESQL != "") { sSQL = sSQL + " AND RouteSheetID IN (" + sRHESQL + ")"; }


                #endregion

                oProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oProductionScheduleDetails.Count <= 0) { throw new Exception("No infromation found."); }
                oProductionScheduleDetails = oProductionScheduleDetails.OrderBy(x => x.MachineName).ThenBy(x => x.ProductionScheduleNo).ToList();
                //_oDataSet = ProductionSchedule.GetsGroupBy(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                //foreach (DataRow oRow in _oDataSet.Tables[0].Rows)
                //{
                //    oProductionScheduleDetail = new ProductionScheduleDetail();
                //    oProductionScheduleDetail.ProductionScheduleDetailID = (oRow["ProductionScheduleDetailID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductionScheduleDetailID"]);
                //    oProductionScheduleDetail.ProductionScheduleID = (oRow["ProductionScheduleID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductionScheduleID"]);
                //    oProductionScheduleDetail.ProductionTracingUnitID = (oRow["ProductionTracingUnitID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductionTracingUnitID"]);
                //    oProductionScheduleDetail.ProductionQty = (oRow["ProductionQty"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductionQty"]);
                //    oProductionScheduleDetail.DBUserID = (oRow["DBUserID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["DBUserID"]);
                //    oProductionScheduleDetail.LocationName = (oRow["LocationName"] == DBNull.Value) ? "" : Convert.ToString(oRow["LocationName"]);
                //    oProductionScheduleDetail.MachineName = (oRow["MachineName"] == DBNull.Value) ? "" : Convert.ToString(oRow["MachineName"]);
                //    oProductionScheduleDetail.MachineNo = (oRow["MachineNo"] == DBNull.Value) ? "" : Convert.ToString(oRow["MachineNo"]);
                //    oProductionScheduleDetail.StartTime = (oRow["StartTime"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oRow["StartTime"]);
                //    oProductionScheduleDetail.EndTime = (oRow["EndTime"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oRow["EndTime"]);
                //    oProductionScheduleDetail.OrderNo = (oRow["OrderNo"] == DBNull.Value) ? "" : Convert.ToString(oRow["OrderNo"]);
                //    oProductionScheduleDetail.BuyerName = (oRow["BuyerName"] == DBNull.Value) ? "" : Convert.ToString(oRow["BuyerName"]);
                //    oProductionScheduleDetail.ColorName = (oRow["ColorName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ColorName"]);
                //    oProductionScheduleDetail.ProductName = (oRow["ProductName"] == DBNull.Value) ? "" : Convert.ToString(oRow["ProductName"]);
                //    oProductionScheduleDetail.ProductID = (oRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oRow["ProductID"]);
                //    oProductionScheduleDetail.FactoryName = (oRow["FactoryName"] == DBNull.Value) ? "" : Convert.ToString(oRow["FactoryName"]);
                //    oProductionScheduleDetail.WaitingForProductionQty = (oRow["WaitingForProductionQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["WaitingForProductionQty"]);
                //    oProductionScheduleDetail.YetToProductionQty = (oRow["YetToProductionQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oRow["YetToProductionQty"]);
                //    oProductionScheduleDetails.Add(oProductionScheduleDetail);
                //}


            }
            catch (Exception ex)
            {
                oProductionScheduleDetail = new ProductionScheduleDetail();
                oProductionScheduleDetail.ErrorMessage = ex.Message;
                oProductionScheduleDetails.Add(oProductionScheduleDetail);
            }
            oProductionSchedule.ProductionScheduleDetails = oProductionScheduleDetails;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion


        #region  Get Machines which is free in a particular time. --- Added By Sagor on 21 May 2014

        [HttpGet]
        public JsonResult GetMachineInFreeTime(string sData, int buid, double nts)
        {
            int nLocationID = Convert.ToInt32(sData.Split('~')[0]);
            DateTime startDate = Convert.ToDateTime(sData.Split('~')[1]);
            DateTime endDate = Convert.ToDateTime(sData.Split('~')[2]);
            List<CapitalResource> oDyeMachines = new List<CapitalResource>();
            List<ProductionSchedule> oProductionSchedules = new List<ProductionSchedule>();
            try
            {
                string sSql = "select * from View_ProductionSchedule where EndTime >= '" + startDate + "' and StartTime <='" + endDate + "' and LocationID=" + nLocationID + " ";
                oProductionSchedules = ProductionSchedule.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oProductionSchedules.Count > 0)
                {
                    int[] A = new int[3];
                    List<int> MachineIDS = oProductionSchedules.Select(e => e.MachineID).Distinct().ToList();
                    string sMachineIDS = "";
                    for (int i = 0; i < MachineIDS.Count; i++)
                    {
                        sMachineIDS = sMachineIDS + MachineIDS[i] + ",";
                    }
                    sMachineIDS = sMachineIDS.Substring(0, sMachineIDS.Length - 1);
                    sSql = "Select * from View_CapitalResource where MachineID not in(" + sMachineIDS + ") AND BUID =" + buid;
                }
                else
                {
                    sSql = "Select * from View_CapitalResource WHERE BUID = " + buid;
                }
                oDyeMachines = CapitalResource.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDyeMachines.Count <= 0)
                {
                    throw new Exception("No data Found.");
                }
            }
            catch (Exception ex)
            {
                CapitalResource oPSS = new CapitalResource();
                oPSS.ErrorMessage = ex.Message;
                oDyeMachines = new List<CapitalResource>();
                oDyeMachines.Add(oPSS);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HTTP Get for ProductionScheduleDetail For DyeLine Sheet
        [HttpPost]
        public JsonResult GetsByPTURS(int nPTUID, int nRSID)
        {
            string sSQL = "SELECT * FROM View_ProductionScheduleDetail"
                       + " WHERE ProductionTracingUnitID=" + nPTUID + " AND YetToProductionQty>0"
                       + " AND ProductionScheduleDetailID not in( Select ProductionScheduleDetailID"
                       + " From RouteSheet Where RouteSheetID!=" + nRSID + ") "; //PTUID=" + nPTUID + " AND
            List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
            try
            {
                oPSDs = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                ProductionScheduleDetail oPSD = new ProductionScheduleDetail();
                oPSD.ErrorMessage = ex.Message;
                oPSDs.Add(oPSD);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPSDs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Schedule By DEO added by Sagor 17 May 2014

        public ActionResult ViewScheduleByDEO(int nDEOID, double nts)
        {
            ///
            /// Job has been removed and Job refer to DyeingOrder
            ///


            List<PTU> oPTUs = new List<PTU>();
            _oProductionSchedule = new ProductionSchedule();

            try
            {
                //if (nPTUID <= 0)
                //{
                //    throw new Exception("Please select an item from list.");
                //}
                //string sSQL = "SELECT * FROM View_ProductionScheduleDetail where ProductionTracingUnitID=" + nPTUID + "";
                //oPTU = oPTU.Get(nPTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //_oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                //string sSQL = "where OrderType=3 and OrderID=(Select JobID from  View_Job where PIID =(Select PIID from VIEW_DyeingOrder where DEOID=" + nDEOID + "))";
                string sSQL = "where OrderID=" + nDEOID;
                // oPTUs = PTU.GetsforWeb(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                List<AuthorizationUserOEDO> oAUOEDOS = new List<AuthorizationUserOEDO>();
                bool bApprove;
                sSQL = "SELECT * FROM View_AuthorizationUserOEDO where DBObjectName='ProductionSchedule' and UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "";
                oAUOEDOS = AuthorizationUserOEDO.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                bApprove = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Add, "ProductionSchedule", oAUOEDOS);
                TempData["AddSchedule"] = bApprove;
                bApprove = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Edit, "ProductionSchedule", oAUOEDOS);
                TempData["EditSchedule"] = bApprove;
                bApprove = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Delete, "ProductionSchedule", oAUOEDOS);
                TempData["DeleteSchedule"] = bApprove;

                sSQL = "SELECT * FROM View_AuthorizationUserOEDO where DBObjectName='RouteSheet' and UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "";
                oAUOEDOS = AuthorizationUserOEDO.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                bApprove = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Add, "RouteSheet", oAUOEDOS);
                TempData["AddDyelineSheet"] = bApprove;


                ViewBag.Title = "Schedule Plan";
                ViewBag.oPTUs = oPTUs;

            }
            catch (Exception ex)
            {
                _oProductionSchedule.ErrorMessage = ex.Message;
            }
            return PartialView(_oProductionSchedule);
        }

        public ActionResult ViewEditProductionScheduleByDEO(int id, int nPSDID, int buid)
        {
            if (id > 0)
            {
                _oProductionSchedule = _oProductionSchedule.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                ViewBag.Count = _oProductionSchedule.ProductionScheduleDetails.Count;
                string sSQL = "Select * from View_ProductionScheduleDetail Where ProductionScheduleDetailID=" + nPSDID + " and ProductionScheduleID=" + id + "";
                _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            _oProductionSchedule.LocationList = Location.Gets(_sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.CapitalResources = CapitalResource.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(_oProductionSchedule);
        }

        public JsonResult SaveByDEO(ProductionSchedule oProductionSchedule)
        {

            List<ProductionScheduleDetail> oPSDS = new List<ProductionScheduleDetail>();

            int nUnpublishProductionSchedule = 0;

            int nStartMinute = oProductionSchedule.StartTime.Minute;
            int nEndMinute = oProductionSchedule.EndTime.Minute;

            int nMinuteRemainder = nStartMinute % 15;

            if (nMinuteRemainder > 0)
            {
                oProductionSchedule.StartTime = oProductionSchedule.StartTime.AddMinutes(-nMinuteRemainder);
            }

            nMinuteRemainder = nEndMinute % 15;
            if (nMinuteRemainder >= 0)
            {
                if (nMinuteRemainder > 0 && nMinuteRemainder != 14)
                {
                    oProductionSchedule.EndTime = oProductionSchedule.EndTime.AddMinutes(-(nMinuteRemainder + 1));
                }
                if (nMinuteRemainder == 0)
                {
                    oProductionSchedule.EndTime = oProductionSchedule.EndTime.AddMinutes(-1);
                }

            }

            if (oProductionSchedule.ProductionScheduleDetails.Count < 1)
            {
                _oProductionSchedule.ErrorMessage = "There is no production schedule detail.";
            }

            int nPSDID = oProductionSchedule.ProductionScheduleDetails[0].ProductionScheduleDetailID;
            int nPSID = oProductionSchedule.ProductionScheduleDetails[0].ProductionScheduleID;
            _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.Gets(nPSID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oProductionSchedule.ProductionScheduleDetails.Count > 1)
            {
                for (int i = 0; i < _oProductionSchedule.ProductionScheduleDetails.Count; i++)
                {
                    if (_oProductionSchedule.ProductionScheduleDetails[i].ProductionScheduleDetailID == nPSDID)
                    {
                        oPSDS.Add(oProductionSchedule.ProductionScheduleDetails[0]);
                    }
                    else
                    {
                        oPSDS.Add(_oProductionSchedule.ProductionScheduleDetails[i]);
                    }
                }
                oProductionSchedule.ProductionScheduleDetails = oPSDS;
            }


            if (oProductionSchedule.ScheduleStatusInString == "Publish")
            {
                //string sSql="select COUNT(*) from ProductionSchedule where LocationID="+ oProductionSchedule.LocationID +" and  DyeMachineID="+ oProductionSchedule.DyeMachineID +" and ScheduleStatus<3 and StartTime< '"+ oProductionSchedule.StartTime +"'";
                //nUnpublishProductionSchedule = oProductionSchedule.GetUnpublishProductionSchedule(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oProductionSchedule.ScheduleStatus = (EnumProductionScheduleStatus)3; // 3 is used to publish a schedule. 
                if (nUnpublishProductionSchedule == 0)
                {
                    try
                    {
                        _oProductionSchedule = oProductionSchedule.Save(((User)Session[SessionInfo.CurrentUser]).UserID);

                    }
                    catch (Exception ex)
                    {
                        _oProductionSchedule = new ProductionSchedule();
                        _oProductionSchedule.ErrorMessage = ex.Message;
                    }
                }
                else
                {
                    _oProductionSchedule.ErrorMessage = "You have need to publish all schedule before this schedule.";
                }
            }

            else
            {


                try
                {
                    _oProductionSchedule = oProductionSchedule.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                catch (Exception ex)
                {
                    _oProductionSchedule = new ProductionSchedule();
                    _oProductionSchedule.ErrorMessage = ex.Message;
                }
            }
            string sSQL = "Select * from View_ProductionScheduleDetail Where ProductionScheduleDetailID=" + nPSDID + " and ProductionScheduleID=" + nPSID + "";
            _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteByDEO(int nPSID, int nPSDID, double nts)
        {

            ProductionSchedule oPS = new ProductionSchedule();
            List<ProductionScheduleDetail> oPSDS = new List<ProductionScheduleDetail>();

            string sErrorMease = "";
            try
            {
                if (nPSID <= 0)
                {
                    throw new Exception("Please select an item from list.");
                }
                oPS = oPS.Get(nPSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oPSDS = ProductionScheduleDetail.Gets(oPS.ProductionScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPSDS.Count == 1)
                {
                    sErrorMease = oPS.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (sErrorMease.Contains('~'))
                    {
                        sErrorMease = Convert.ToString(sErrorMease.Split('~')[0]);
                    }
                }
                else if (oPSDS.Count > 1)
                {
                    string sSQL = "Delete from ProductionScheduleDetail Where ProductionScheduleDetailID=" + nPSDID + " and ProductionScheduleID=" + nPSID + "";
                    oPSDS = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oPSDS.Count <= 0)
                    {
                        sErrorMease = "Delete Successfully.";
                    }

                }



            }
            catch (Exception ex)
            {

                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetsByPTUForDEO(int nPTUID)
        {
            string sSQL = "SELECT * FROM View_ProductionScheduleDetail"
                       + " WHERE ProductionTracingUnitID=" + nPTUID + "";
            List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
            try
            {
                oPSDs = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                ProductionScheduleDetail oPSD = new ProductionScheduleDetail();
                oPSD.ErrorMessage = ex.Message;
                oPSDs.Add(oPSD);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPSDs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetPSDForDEO(int nPSDID, double nts)
        {

            string sSQL = "SELECT * FROM View_ProductionScheduleDetail WHERE ProductionScheduleDetailID=" + nPSDID + "";

            List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
            ProductionScheduleDetail oPSD = new ProductionScheduleDetail();
            try
            {
                oPSDs = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPSDs.Count > 0)
                {
                    oPSD = oPSDs[0];
                }
            }
            catch (Exception ex)
            {
                oPSD = new ProductionScheduleDetail();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPSD);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPSByMachine(int nMachineID, int nLocationID, double nts)
        {

            string sSQL = "Select top(1)* from View_ProductionSChedule Where MachineID=" + nMachineID + " and LocationID=" + nLocationID + " Order by StartTime  DESC";

            List<ProductionSchedule> oPSs = new List<ProductionSchedule>();
            ProductionSchedule oPS = new ProductionSchedule();
            string sStartTime = "";
            try
            {
                oPSs = ProductionSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPSs.Count > 0)
                {

                    sStartTime = oPSs[0].EndTime.AddMinutes(1).ToString("MM'/'dd'/'yyyy HH:mm");


                }
            }
            catch (Exception ex)
            {
                sStartTime = "";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sStartTime);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Machine Wise production Schedule

        public ActionResult ViewMachineWiseProductionSchedule(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            string sLocationIDs = "";
            string sPSIDs = "";

            _sQuery = "SELECT * FROM Location where LocationID in(SELECT Distinct LocationID FROM VIEW_CapitalResource where ISNULL(IsActive,1)=1 AND BUID = " + buid + " AND ResourcesType = " + (int)EnumResourcesType.Machine + " )";
            _oProductionSchedule.LocationList = Location.Gets(_sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.CapitalResources = CapitalResource.GetsByBUandResourceTypeWithName(buid, (int)EnumResourcesType.Machine, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            sLocationIDs = Location.IDInString(_oProductionSchedule.LocationList);
            _oProductionSchedule.ProductionScheduleList = new List<ProductionSchedule>();

            DateTime today = DateTime.Today;
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

            string sql = "Select * from View_ProductionSchedule where LocationID=" + sLocationIDs + " and ProductionScheduleID in ( Select distinct(ProductionScheduleID) from View_ProductionScheduleDetail Where ((StartTime>='" + startOfMonth + "' and StartTime<='" + endOfMonth + "') or (EndTime>='" + startOfMonth + "' and EndTime<='" + endOfMonth + "'))  and RSState<7 ) and Activity='1' order by MachineID,StartTime ";
            _oProductionSchedule.ProductionScheduleList = ProductionSchedule.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //if (_oProductionSchedule.ProductionScheduleList!=null)
            //{
            //    sPSIDs = ProductionSchedule.IDInString(_oProductionSchedule.ProductionScheduleList);
            //}
            //if (sPSIDs.Length > 0)
            //{
            //    sql = "SELECT * FROM View_ProductionScheduleDetail where ProductionScheduleID in (" + sPSIDs + ") and RSState<7";
            //    _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //    foreach (ProductionSchedule oItem in _oProductionSchedule.ProductionScheduleList)
            //    {
            //        this.AddOrderDetail(oItem,"View");
            //    }
            //}
            foreach (CapitalResource oItem in _oProductionSchedule.CapitalResources)
            {
                var nCount = (from oPS in _oProductionSchedule.ProductionScheduleList where oPS.MachineID == oItem.CRID && oPS.BUID == oItem.BUID select oPS).Count();
                oItem.MachineNoWithCapacityAndTotalSchedule = "[" + oItem.Code + "]" + oItem.Name + "[" + oItem.MachineCapacity + "]" + " (" + Convert.ToInt32(nCount) + ")";
            }

            List<MachineWiseProductionSchedule> oTempMPSs = new List<MachineWiseProductionSchedule>();
            oTempMPSs = MapScheduleByMachine(startOfMonth, endOfMonth, _oProductionSchedule.ProductionScheduleList, _oProductionSchedule.CapitalResources, "Month"); // last paramerter indicates initially Grid Style in Month View.
            ViewBag.ProductionSchedules = oTempMPSs;
            _oProductionSchedule.LocationList = Location.Gets(_sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LocationList = _oProductionSchedule.LocationList;
            ViewBag.BUID = buid;
            return View(_oProductionSchedule);
        }

        public List<MachineWiseProductionSchedule> MapScheduleByMachine(DateTime startOfMonth, DateTime endOfMonth, List<ProductionSchedule> oPSs, List<CapitalResource> oMachines, string sGridType)
        {
            List<MachineWiseProductionSchedule> oTempMPSs = new List<MachineWiseProductionSchedule>();
            MachineWiseProductionSchedule oTempMPS = new MachineWiseProductionSchedule();
            List<ProductionSchedule> oProductionSchedules = new List<ProductionSchedule>();
            int nCount;
            string sPropertyName = "";

            DateTime StartTime, EndTime;

            while (startOfMonth <= endOfMonth)
            {
                StartTime = startOfMonth;
                EndTime = startOfMonth.AddDays(1);
                oTempMPS = new MachineWiseProductionSchedule();
                nCount = 0;
                foreach (CapitalResource oMachine in oMachines)
                {
                    ++nCount;
                    string sValue = "";
                    oProductionSchedules = new List<ProductionSchedule>();
                    // Get Production Schedule by using Linq
                    var oSchedules = from p in oPSs where p.MachineID == oMachine.CRID && ((p.StartTime >= StartTime && p.StartTime <= EndTime) || (p.EndTime >= StartTime && p.EndTime <= EndTime) || (p.StartTime <= StartTime && p.EndTime >= EndTime)) && p.StartTime.Date <= StartTime.Date orderby p.MachineID select p;
                    if (oSchedules.Count() > 0)
                    {
                        foreach (var oItem in oSchedules)
                        {
                            oProductionSchedules.Add(oItem);
                        }
                    }
                    #region Set Schedule Information
                    sPropertyName = "Machine" + nCount.ToString(); // Make property Name
                    PropertyInfo prop = oTempMPS.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance); // Check Property name Exists or Not
                    if (null != prop && prop.CanWrite)
                    {
                        sValue = SpecificMachineSchedule(oProductionSchedules, oMachine, startOfMonth, sGridType);
                        prop.SetValue(oTempMPS, sValue, null); // Set Schedule Information to the property 
                    }
                    #endregion
                }
                oTempMPS.sDate = startOfMonth.ToString("ddd") + "<br/>" + startOfMonth.Day + " " + startOfMonth.ToString("MMM");
                oTempMPSs.Add(oTempMPS);
                startOfMonth = EndTime;

            }

            return oTempMPSs;
        }

        String SpecificMachineSchedule(List<ProductionSchedule> oPSs, CapitalResource oMachine, DateTime oStartTime, string sGridType)
        {
            DateTime oDateTime = DateTime.Now;
            string sScheduleInfo = "";
            string sParam = "";
            ProductionSchedule oProductionSchedule = new ProductionSchedule();
            if (oPSs.Count > 0)
            {
                bool bFalg = false;
                foreach (ProductionSchedule oItem in oPSs)
                {

                    if (oItem.MachineID == oMachine.CRID)
                    {
                        if (!bFalg)
                        {
                            bool bModificationIcon = true;
                            if (oStartTime.Date >= oDateTime.Date) // Add Schedule Button
                            {
                                bModificationIcon = false;
                                if (sGridType == "Day")
                                {
                                    sParam = "" + oMachine.LocationID + "," + oMachine.CRID + "," + oStartTime.ToString("dd") + "," + oStartTime.ToString("MM") + "," + oStartTime.ToString("yyyy") + "," + oStartTime.ToString("HH") + "," + oStartTime.ToString("mm") + "," + 0 + "";
                                    sScheduleInfo = "<center>" +
                                                    "<img src='../../Content/Images/Add.png' title='Add'  onClick='AddNewSchedule(" + sParam + ")'/>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Edit.png' title='Edit'  onClick='Edit()'/> </span>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Delete.png' title='Delete'  onClick='Delete()'/> </span>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Copy.png' title='Cpoy'  onClick='ScheduleCopy(" + sParam + ")'/> </span>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Cut.png' title='Cut'  onClick='ScheduleCut(" + sParam + ")'/> </span>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Paste.png' title='Paste'  onClick='SchedulePaste(" + sParam + ")'/> </span>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Swap.png' title='Swap' onClick='ScheduleSwap(" + sParam + ")'/> <span>" +
                                                    "</center>";
                                }
                                else if (sGridType == "Week")
                                {
                                    sParam = "" + oMachine.LocationID + "," + oMachine.CRID + "," + oStartTime.ToString("dd") + "," + oStartTime.ToString("MM") + "," + oStartTime.ToString("yyyy") + "," + oStartTime.ToString("HH") + "," + oStartTime.ToString("mm") + "," + 1 + "";
                                    sScheduleInfo = "<center>" +
                                                    "<img src='../../Content/Images/Add.png' title='Add'  onClick='AddNewSchedule(" + sParam + ")'/>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Edit.png' title='Edit'  onClick='Edit()'/> </span>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Delete.png' title='Delete'  onClick='Delete()'/> </span>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Copy.png' title='Cpoy'  onClick='ScheduleCopy(" + sParam + ")'/> </span>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Cut.png' title='Cut'  onClick='ScheduleCut(" + sParam + ")'/> </span>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Paste.png' title='Paste'  onClick='SchedulePaste(" + sParam + ")'/> </span>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Swap.png' title='Swap' onClick='ScheduleSwap(" + sParam + ")'/> <span>" +
                                                    "</center>";
                                }
                                else
                                {
                                    sParam = "" + oMachine.LocationID + "," + oMachine.CRID + "," + oStartTime.ToString("dd") + "," + oStartTime.ToString("MM") + "," + oStartTime.ToString("yyyy") + "," + oStartTime.ToString("HH") + "," + oStartTime.ToString("mm") + "," + 2 + "";
                                    sScheduleInfo = "<center>" +
                                                    "<img src='../../Content/Images/Add.png' title='Add'  onClick='AddNewSchedule(" + sParam + ")'/>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Edit.png' title='Edit'  onClick='Edit()'/> </span>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Delete.png' title='Delete'  onClick='Delete()'/> </span>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Copy.png' title='Cpoy'  onClick='ScheduleCopy(" + sParam + ")'/> </span>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Cut.png' title='Cut'  onClick='ScheduleCut(" + sParam + ")'/> </span>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Paste.png' title='Paste'  onClick='SchedulePaste(" + sParam + ")'/> </span>" +
                                                    "<span style='padding-left:2px;'> <img src='../../Content/Images/Swap.png' title='Swap' onClick='ScheduleSwap(" + sParam + ")'/> <span>" +
                                                    "</center>";
                                }


                            }

                            if (oItem.StartTime.Date < oItem.EndTime.Date)  // Schedule Continue
                            {
                                if (oStartTime.Date == oItem.StartTime.Date)
                                {
                                    if (bModificationIcon) { sScheduleInfo = sScheduleInfo + Modificationtype(sGridType, oMachine.LocationID, oMachine.CRID, oStartTime.ToString("dd"), oStartTime.ToString("MM"), oStartTime.ToString("yyyy"), oStartTime.ToString("HH"), oStartTime.ToString("mm")); }
                                    sScheduleInfo = sScheduleInfo + "<input id=" + oItem.ProductionScheduleID + "   type='checkbox'  onChange='CheckBoxSelectionForPrint(" + oItem.ProductionScheduleID + "," + oItem.MachineID + ")'/>" + "<a style='text-decoration:none;font-size:10px; color:Black;' href=" + "javascript:View(" + oItem.ProductionScheduleID + ")>" + oItem.StartTime.ToString("HH:mm") + " - " + "Continue" + "(" + oItem.ProductionScheduleNo + ")</a>";
                                }
                                else if (oItem.EndTime.Date > oStartTime.Date)
                                {
                                    sScheduleInfo = sScheduleInfo + "<a style='text-decoration:none; font-size:10px;color:Black;' href=" + "javascript:View(" + oItem.ProductionScheduleID + ")>" + "Continue" + "(" + oItem.ProductionScheduleNo + ")<br/>" + "Quantiry: " + Math.Round(oItem.ProductionQty, 2) + "</a>";
                                }
                                else
                                {
                                    int nCount = oPSs.Where(x => x.StartTime.Day == oItem.EndTime.Day).ToList().Count();
                                    if (nCount > 0 && bModificationIcon) { sScheduleInfo = sScheduleInfo + Modificationtype(sGridType, oMachine.LocationID, oMachine.CRID, oStartTime.ToString("dd"), oStartTime.ToString("MM"), oStartTime.ToString("yyyy"), oStartTime.ToString("HH"), oStartTime.ToString("mm")); }
                                    sScheduleInfo = sScheduleInfo + "<a style='text-decoration:none; font-size:10px;color:Black;' href=" + "javascript:View(" + oItem.ProductionScheduleID + ")>" + "Continue" + " - " + oItem.EndTime.ToString("HH:mm") + "(" + oItem.ProductionScheduleNo + ")</a>";
                                }

                            }
                            else
                            {
                                if (bModificationIcon) { sScheduleInfo = sScheduleInfo + Modificationtype(sGridType, oMachine.LocationID, oMachine.CRID, oStartTime.ToString("dd"), oStartTime.ToString("MM"), oStartTime.ToString("yyyy"), oStartTime.ToString("HH"), oStartTime.ToString("mm")); }
                                sScheduleInfo = sScheduleInfo + "<input id=" + oItem.ProductionScheduleID + "   type='checkbox'  onChange='CheckBoxSelectionForPrint(" + oItem.ProductionScheduleID + "," + oItem.MachineID + ")'/>" + "<a style='text-decoration:none;font-size:10px;color:Black;' href=" + "javascript:View(" + oItem.ProductionScheduleID + ")>" + oItem.StartTime.ToString("HH:mm") + " - " + oItem.EndTime.ToString("HH:mm") + "(" + oItem.ProductionScheduleNo + ")</a>";
                            }

                            bFalg = true;
                        }
                        else
                        {

                            if (oItem.StartTime.Date < oItem.EndTime.Date)  // Schedule Continue
                            {
                                if (oStartTime.Date == oItem.StartTime.Date)
                                {
                                    sScheduleInfo = sScheduleInfo + "<br/>" + "<input id=" + oItem.ProductionScheduleID + "   type='checkbox'  onChange='CheckBoxSelectionForPrint(" + oItem.ProductionScheduleID + "," + oItem.MachineID + ")'/>" + "<a style='text-decoration:none;font-size:10px; color:Black;' href=" + "javascript:View(" + oItem.ProductionScheduleID + ")>" + oItem.StartTime.ToString("HH:mm") + " - " + "Continue" + "(" + oItem.ProductionScheduleNo + ")" + "</a>";
                                }
                                else if (oItem.EndTime.Date > oStartTime.Date)
                                {
                                    sScheduleInfo = sScheduleInfo + "<br/>" + "<a style='text-decoration:none; font-size:10px;color:Black;' href=" + "javascript:View(" + oItem.ProductionScheduleID + ")>" + "Continue" + " (" + oItem.ProductionScheduleNo + ")</a>";
                                }
                                else
                                {
                                    sScheduleInfo = sScheduleInfo + "<br/>" + "<a style='text-decoration:none;font-size:10px; color:Black;' href=" + "javascript:View(" + oItem.ProductionScheduleID + ")>" + "Continue" + " - " + oItem.EndTime.ToString("HH:mm") + "(" + oItem.ProductionScheduleNo + ")</a>";
                                }

                            }
                            else
                            {
                                sScheduleInfo = sScheduleInfo + "<br/>" + "<input id=" + oItem.ProductionScheduleID + "   type='checkbox'  onChange='CheckBoxSelectionForPrint(" + oItem.ProductionScheduleID + "," + oItem.MachineID + ")'/>" + "<a style='text-decoration:none; font-size:10px; color:Black;' href=" + "javascript:View(" + oItem.ProductionScheduleID + ")>" + oItem.StartTime.ToString("HH:mm") + " - " + oItem.EndTime.ToString("HH:mm") + "(" + oItem.ProductionScheduleNo + ")</a>";
                            }

                        }

                    }
                }
            }

            else
            {
                if (oStartTime.Date >= oDateTime.Date) // Add Schedule Button
                {
                    if (sGridType == "Day")
                    {
                        sParam = "" + oMachine.LocationID + "," + oMachine.CRID + "," + oStartTime.ToString("dd") + "," + oStartTime.ToString("MM") + "," + oStartTime.ToString("yyyy") + "," + oStartTime.ToString("HH") + "," + oStartTime.ToString("mm") + "," + 0 + "";
                        sScheduleInfo = "<center>" +
                                        "<img src='../../Content/Images/Add.png' title='Add'  onClick='AddNewSchedule(" + sParam + ")'/>" +
                                        "<span style='padding-left:8px;'> <img src='../../Content/Images/Paste.png' title='Paste'   onClick='SchedulePaste(" + sParam + ")'/> </span>" +
                                        "</center>";

                    }
                    else if (sGridType == "Week")
                    {
                        sParam = "" + oMachine.LocationID + "," + oMachine.CRID + "," + oStartTime.ToString("dd") + "," + oStartTime.ToString("MM") + "," + oStartTime.ToString("yyyy") + "," + oStartTime.ToString("HH") + "," + oStartTime.ToString("mm") + "," + 1 + "";
                        sScheduleInfo = "<center>" +
                                        "<img src='../../Content/Images/Add.png' title='Add'  onClick='AddNewSchedule(" + sParam + ")'/>" +
                                        "<span style='padding-left:8px;'> <img src='../../Content/Images/Paste.png' title='Paste'   onClick='SchedulePaste(" + sParam + ")'/> </span>" +
                                        "</center>";
                    }
                    else
                    {
                        sParam = "" + oMachine.LocationID + "," + oMachine.CRID + "," + oStartTime.ToString("dd") + "," + oStartTime.ToString("MM") + "," + oStartTime.ToString("yyyy") + "," + oStartTime.ToString("HH") + "," + oStartTime.ToString("mm") + "," + 2 + "";
                        sScheduleInfo = "<center>" +
                                        "<img src='../../Content/Images/Add.png' title='Add'  onClick='AddNewSchedule(" + sParam + ")'/>" +
                                        "<span style='padding-left:8px;'> <img src='../../Content/Images/Paste.png' title='Paste'   onClick='SchedulePaste(" + sParam + ")'/> </span>" +
                                        "</center>";

                    }
                }
            }

            return sScheduleInfo;
        }


        private string Modificationtype(string sGridType, int nLocationID, int nMachineID, string sDD, string sMM, string sYear, string sHour, string sMin)
        {
            string sScheduleInfo = "", sParam = "";
            if (sGridType == "Day")
            {
                //Modificationtype(sGridType, oPSS.LocationID, oPSS.MachineID, (int)StartTime.ToString("dd"), (int)oStartTime.ToString("MM"), (int)oStartTime.ToString("yyyy"), (int)oStartTime.ToString("HH"), (int)oStartTime.ToString("mm"));
                sParam = "" + nLocationID + "," + nMachineID + "," + sDD + "," + sMM + "," + sYear + "," + sHour + "," + sMin + "," + 0 + "";
                sScheduleInfo = "<center>" +
                                "<span style='padding-left:2px;'> <img src='../../Content/Images/Copy.png' title='Cpoy'  onClick='ScheduleCopy(" + sParam + ")'/> </span>" +
                    //"<span style='padding-left:2px;'> <img src='../../Content/Images/Cut.gif' title='Cut'  onClick='ScheduleCut(" + sParam + ")'/> </span>" +
                    //"<span style='padding-left:2px;'> <img src='../../Content/Images/Paste.gif' title='Paste'  onClick='SchedulePaste(" + sParam + ")'/> </span>" +
                    //"<span style='padding-left:2px;'> <img src='../../Content/Images/Swap.gif' title='Swap' onClick='ScheduleSwap(" + sParam + ")'/> <span>" +
                                "</center>";
            }
            else if (sGridType == "Week")
            {
                sParam = "" + nLocationID + "," + nMachineID + "," + sDD + "," + sMM + "," + sYear + "," + sHour + "," + sMin + "," + 1 + "";
                sScheduleInfo = "<center>" +
                                "<span style='padding-left:2px;'> <img src='../../Content/Images/Copy.png' title='Cpoy'  onClick='ScheduleCopy(" + sParam + ")'/> </span>" +
                    //"<span style='padding-left:2px;'> <img src='../../Content/Images/Cut.gif' title='Cut'  onClick='ScheduleCut(" + sParam + ")'/> </span>" +
                    //"<span style='padding-left:2px;'> <img src='../../Content/Images/Paste.gif' title='Paste'  onClick='SchedulePaste(" + sParam + ")'/> </span>" +
                    //"<span style='padding-left:2px;'> <img src='../../Content/Images/Swap.gif' title='Swap' onClick='ScheduleSwap(" + sParam + ")'/> <span>" +
                                "</center>";
            }
            else
            {
                sParam = "" + nLocationID + "," + nMachineID + "," + sDD + "," + sMM + "," + sYear + "," + sHour + "," + sMin + "," + 2 + "";
                sScheduleInfo = "<center>" +
                                "<span style='padding-left:2px;'> <img src='../../Content/Images/Copy.png' title='Cpoy'  onClick='ScheduleCopy(" + sParam + ")'/> </span>" +
                    //"<span style='padding-left:2px;'> <img src='../../Content/Images/Cut.gif' title='Cut'  onClick='ScheduleCut(" + sParam + ")'/> </span>" +
                    //"<span style='padding-left:2px;'> <img src='../../Content/Images/Paste.gif' title='Paste'  onClick='SchedulePaste(" + sParam + ")'/> </span>" +
                    //"<span style='padding-left:2px;'> <img src='../../Content/Images/Swap.gif' title='Swap' onClick='ScheduleSwap(" + sParam + ")'/> <span>" +
                                "</center>";
            }
            return sScheduleInfo;
        }


        #region Get Production Schedule Information

        [HttpGet]
        public JsonResult GetProductionSchedule(int nID, double nts) // nID represent ProductionScheduleID
        {
            _oProductionSchedule = new ProductionSchedule();
            try
            {
                _oProductionSchedule = _oProductionSchedule.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.Gets(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oProductionSchedule.ProductionScheduleID <= 0)
                {
                    throw new Exception("Invalid request.");
                }

            }
            catch (Exception ex)
            {

                _oProductionSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get machine

        [HttpGet]
        public JsonResult GetMachine(int nMachineID, double nts)
        {
            CapitalResource oCapitalResource = new CapitalResource();
            List<CapitalResource> oCapitalResources = new List<CapitalResource>();

            try
            {
                string sSQL = "SELECT * FROM VIEW_CapitalResource Where CRID =" + nMachineID;
                oCapitalResources = CapitalResource.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oCapitalResources.Count > 0)
                {
                    oCapitalResource = oCapitalResources[0];
                }
                else
                {
                    throw new Exception("Invalid request.");
                }

            }
            catch (Exception ex)
            {

                oCapitalResource.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCapitalResource);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get Search Value
        [HttpPost]
        public JsonResult RefreshMachineWise(ProductionSchedule oProductionSchedule, bool bUptoLoadDyeMachine, string sScheduleDateType)
        {

            _oProductionSchedule = new ProductionSchedule();
            string gridas = "";
            DateTime today = DateTime.Now, startTime = DateTime.Now, endTime = DateTime.Now;
            if (sScheduleDateType == "")
            {
                if (oProductionSchedule.sDay == "Day")
                {
                    gridas = "Day";
                    today = oProductionSchedule.StartTime;
                    startTime = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
                    endTime = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59);
                }
                else if (oProductionSchedule.sWeek == "Week")
                {
                    gridas = "Week";
                    today = oProductionSchedule.StartTime;
                    startTime = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
                    DateTime newTime = today.AddDays(6);
                    endTime = new DateTime(newTime.Year, newTime.Month, newTime.Day, 23, 59, 59);
                }
                else if (oProductionSchedule.sMonth == "Month")
                {
                    gridas = "Month";
                    today = oProductionSchedule.StartTime;
                    startTime = new DateTime(today.Year, today.Month, 1);
                    endTime = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month), 23, 59, 59);
                }
            }
            else
            {
                gridas = "Day";
                today = oProductionSchedule.StartTime;
                startTime = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
                if (sScheduleDateType == "Between")
                {
                    endTime = new DateTime(oProductionSchedule.EndTime.Year, oProductionSchedule.EndTime.Month, oProductionSchedule.EndTime.Day, 23, 59, 59);
                }
                else
                {
                    endTime = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59);
                }

            }

            List<CapitalResource> oCapitalResources = new List<CapitalResource>();
            if (oProductionSchedule.MachineIDs.Trim() != "")
            {
                string sSQL = "SELECT * FROM VIEW_CapitalResource Where CRID in(" + oProductionSchedule.MachineIDs + ") and BUID=" + oProductionSchedule.BUID + "";
                oCapitalResources = CapitalResource.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {

                oCapitalResources = CapitalResource.GetsByBUandResourceTypeWithName(oProductionSchedule.BUID, (int)EnumResourcesType.Machine, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _oProductionSchedule = RefreshValue(oProductionSchedule, bUptoLoadDyeMachine, startTime, endTime, gridas);
            MachineWiseProductionSchedule oMPS = new MachineWiseProductionSchedule();
            List<MachineWiseProductionSchedule> oMPSs = new List<MachineWiseProductionSchedule>();
            oMPSs = MapScheduleByMachine(startTime, endTime, _oProductionSchedule.ProductionScheduleList, oCapitalResources, gridas);

            _oProductionSchedule.CapitalResources = CapitalResource.GetsByBUandResourceTypeWithName(oProductionSchedule.BUID, (int)EnumResourcesType.Machine, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            foreach (CapitalResource oItem in _oProductionSchedule.CapitalResources)
            {
                var nCount = (from oPS in _oProductionSchedule.ProductionScheduleList where oPS.MachineID == oItem.CRID && oPS.BUID == oItem.BUID select oPS).Count();
                oItem.MachineNoWithCapacityAndTotalSchedule = "[" + oItem.Code + "]" + oItem.Name + "[" + oItem.MachineCapacity + "]" + " (" + Convert.ToInt32(nCount) + ")";
            }
            oMPS.MachineWiseProductionSchedules = oMPSs;
            oMPS.CapitalResources = _oProductionSchedule.CapitalResources;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMPS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get InComplete Schedule

        [HttpPost]
        public JsonResult GetsInCompleteSchedule(string sMachineIDs, int nLocationID, int nBUID, string sViewType)
        {

            _oProductionSchedule = new ProductionSchedule();
            string gridas = "", sPSIDs = "";
            string sSQL = "";
            DateTime today = DateTime.Now, startTime = DateTime.Now, endTime = DateTime.Now;

            if (sViewType == "Day") { gridas = "Day"; }
            else if (sViewType == "Week") { gridas = "Week"; }
            else { gridas = "Month"; }

            List<CapitalResource> oCapitalResources = new List<CapitalResource>();
            if (sMachineIDs.Trim() != "")
            {
                sSQL = "SELECT * FROM VIEW_CapitalResource Where CRID in(" + sMachineIDs + ") and BUID=" + nBUID + " AND LocationID = " + nLocationID;
                oCapitalResources = CapitalResource.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = "Select * from View_ProductionSchedule Where ProductionScheduleID In (Select distinct(ProductionScheduleID) from View_ProductionScheduleDetail Where RSState<7)" +
                       " And MachineID in(" + sMachineIDs + ") And LocationID=" + nLocationID + " and Activity='1' order by MachineID,StartTime ";
            }
            else
            {
                oCapitalResources = CapitalResource.GetsByBUandResourceTypeWithName(nBUID, (int)EnumResourcesType.Machine, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = "Select * from View_ProductionSchedule Where ProductionScheduleID In (Select distinct(ProductionScheduleID) from View_ProductionScheduleDetail Where RSState<7)" +
                       " And LocationID=" + nLocationID + "  and Activity='1' order by MachineID,StartTime";
            }
            _oProductionSchedule.ProductionScheduleList = ProductionSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nNoOfSchedule = _oProductionSchedule.ProductionScheduleList.Count();
            if (nNoOfSchedule > 0)
            {
                sPSIDs = ProductionSchedule.IDInString(_oProductionSchedule.ProductionScheduleList);
                sSQL = "SELECT * FROM View_ProductionScheduleDetail where ProductionScheduleID in (" + sPSIDs + ") and RSState<7";
                _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ProductionSchedule oItem in _oProductionSchedule.ProductionScheduleList)
                {
                    this.AddOrderDetail(oItem, "View");
                }
                startTime = Convert.ToDateTime((from oPS in _oProductionSchedule.ProductionScheduleList select oPS.StartTime).Min().ToString("dd MMM yyyy"));
                endTime = Convert.ToDateTime(((from oPS in _oProductionSchedule.ProductionScheduleList select oPS.EndTime).Max()).ToString("dd MMM yyyy"));
            }

            MachineWiseProductionSchedule oMPS = new MachineWiseProductionSchedule();
            List<MachineWiseProductionSchedule> oMPSs = new List<MachineWiseProductionSchedule>();
            oMPSs = MapScheduleByMachine(startTime, endTime, _oProductionSchedule.ProductionScheduleList, oCapitalResources, gridas);

            _oProductionSchedule.CapitalResources = CapitalResource.GetsByBUandResourceTypeWithName(nBUID, (int)EnumResourcesType.Machine, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            foreach (CapitalResource oItem in _oProductionSchedule.CapitalResources)
            {
                var nCount = (from oPS in _oProductionSchedule.ProductionScheduleList where oPS.MachineID == oItem.CRID && oPS.BUID == oItem.BUID select oPS).Count();
                oItem.MachineCapacity = oItem.MachineNoWithCapacity + " (" + Convert.ToInt32(nCount) + ")";
            }
            oMPS.MachineWiseProductionSchedules = oMPSs;
            oMPS.CapitalResources = _oProductionSchedule.CapitalResources;
            oMPS.StartTimeInString = startTime.ToString("dd MMM yyyy");
            oMPS.EndTimeInString = endTime.ToString("dd MMM yyyy");

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMPS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region  To get Production Schedule Detail By ProductionScheduleNo for Combine Dyline

        [HttpGet]
        public JsonResult GetsPSDForCombineDyline(string sPSNo, double nts)
        {
            List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
            ProductionScheduleDetail oPSD = new ProductionScheduleDetail();
            try
            {
                if (sPSNo.Trim() == "")
                {
                    throw new Exception("Please enter valid porduction schedule no.");
                }
                string sSQL = "Select * from View_ProductionScheduleDetail Where ProductionScheduleNo='" + sPSNo + "'";
                oPSDs = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPSDs.Count <= 0)
                {
                    throw new Exception("Nothing found for this porduction schedule no.");
                }
            }
            catch (Exception ex)
            {

                oPSD.ErrorMessage = ex.Message;
                oPSDs.Add(oPSD);


            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPSDs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetsProductionScheduleDetail(int nPSID, double nts)
        {
            List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
            ProductionScheduleDetail oPSD = new ProductionScheduleDetail();
            try
            {
                if (nPSID <= 0)
                {
                    throw new Exception("Invalid production schedule.");
                }
                string sSQL = "Select * from View_ProductionScheduleDetail Where ProductionScheduleID='" + nPSID + "'";
                oPSDs = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPSDs.Count <= 0)
                {
                    throw new Exception("Nothing found for this porduction schedule.");
                }
            }
            catch (Exception ex)
            {

                oPSD.ErrorMessage = ex.Message;
                oPSDs.Add(oPSD);


            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPSDs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Schedule Count For Individual Machine In DEO
        [HttpGet]
        public JsonResult GetIndividualMachineSchedule(int nMachineID, int nLocationID, double nts)
        {
            ProductionSchedule oPS = new ProductionSchedule();
            List<ProductionSchedule> oPSs = new List<ProductionSchedule>();

            string Ssql = "Select * From View_ProductionSchedule Where ProductionScheduleID IN (Select Distinct(ProductionScheduleID) from View_ProductionScheduleDetail Where RSState<7) AND Activity=1 AND MachineID=" + nMachineID + " AND LocationID=" + nLocationID + " ";
            try
            {
                oPSs = ProductionSchedule.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPSs.Count <= 0) { throw new Exception("Data Not Found !"); }
            }
            catch (Exception ex)
            {
                oPS.ErrorMessage = ex.Message;
                oPSs.Add(oPS);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPSs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Production Schedule Tracking Report Added By Sagor on 13 Oct 2014

        public ActionResult PrintProductionScheduleTrackingReportDefault(string sIDs, double nts)
        {
            List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
            string sSQL = "", sDateRange = "";
            try
            {
                if (sIDs.Trim() != "")
                {
                    sSQL = "Select * from  View_ProductionScheduleDetail where ProductionScheduleDetailID IN (" + sIDs + ") Order By StartTime DESC";
                    oPSDs = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oPSDs.Count() > 0)
                    {
                        oPSDs = oPSDs.OrderBy(x => x.MachineName).ThenBy(x => x.ProductionScheduleNo).ToList();
                        if (oPSDs.Min(x => x.StartTime).ToString("dd MMM yyyy") == oPSDs.Max(x => x.StartTime).ToString("dd MMM yyyy")) { sDateRange = oPSDs.Min(x => x.StartTime).ToString("dd MMM yyyy"); }
                        else { sDateRange = oPSDs.Min(x => x.StartTime).ToString("dd MMM yyyy") + " to " + oPSDs.Max(x => x.StartTime).ToString("dd MMM yyyy"); }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptProductionScheduleTrackingDefault oReport = new rptProductionScheduleTrackingDefault();
            byte[] abytes = oReport.PrepareReport(oPSDs, oCompany, sDateRange);
            return File(abytes, "application/pdf");

        }

        public ActionResult PrintProductionScheduleTrackingReport(string sType, string sIDs, double nts)
        {
            List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
            string sSQL = "", sDateRange = "";
            try
            {

                sSQL = "Select * from  View_ProductionScheduleDetail where ProductionScheduleDetailID IN (" + sIDs + ") Order By StartTime DESC";
                oPSDs = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPSDs.Count() > 0)
                {
                    if (oPSDs.Min(x => x.StartTime).ToString("dd MMM yyyy") == oPSDs.Max(x => x.StartTime).ToString("dd MMM yyyy")) { sDateRange = oPSDs.Min(x => x.StartTime).ToString("dd MMM yyyy"); }
                    else { sDateRange = oPSDs.Min(x => x.StartTime).ToString("dd MMM yyyy") + " to " + oPSDs.Max(x => x.StartTime).ToString("dd MMM yyyy"); }
                    if (sType != "Schedule Detail")
                    {
                        oPSDs = ProductionScheduleTrackingForReport(sType, oPSDs);
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptProductionScheduleTracking oReport = new rptProductionScheduleTracking();
            byte[] abytes = oReport.PrepareReport(oPSDs, oCompany, sType, sDateRange);
            return File(abytes, "application/pdf");

        }

        private List<ProductionScheduleDetail> ProductionScheduleTrackingForReport(string sType, List<ProductionScheduleDetail> oProductionScheduleDetails)
        {
            ProductionScheduleDetail oPSD = new ProductionScheduleDetail();
            List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
            if (sType == "Party Wise")
            {
                var oTempPSDs = from oTPSD in oProductionScheduleDetails
                                group oTPSD by new { oTPSD.ProductName, oTPSD.BuyerName } into oTemp
                                select new
                                {
                                    ProductName = oTemp.Key.ProductName,
                                    BuyerName = oTemp.Key.BuyerName,
                                    Qty = oTemp.Sum(oTPSD => oTPSD.ProductionQty)
                                };
                foreach (var oItem in oTempPSDs)
                {
                    oPSD = new ProductionScheduleDetail();
                    oPSD.ProductName = oItem.ProductName;
                    oPSD.BuyerName = oItem.BuyerName;
                    oPSD.ProductionQty = oItem.Qty;
                    oPSDs.Add(oPSD);
                }
            }
            else if (sType == "Order Wise")
            {
                var oTempPSDs = from oTPSD in oProductionScheduleDetails
                                group oTPSD by new { oTPSD.OrderNo, oTPSD.BuyerName } into oTemp
                                select new
                                {
                                    OrderNo = oTemp.Key.OrderNo,
                                    BuyerName = oTemp.Key.BuyerName,
                                    Qty = oTemp.Sum(oTPSD => oTPSD.ProductionQty)
                                };
                foreach (var oItem in oTempPSDs)
                {
                    oPSD = new ProductionScheduleDetail();
                    oPSD.OrderNo = oItem.OrderNo;
                    oPSD.BuyerName = oItem.BuyerName;
                    oPSD.ProductionQty = oItem.Qty;
                    oPSDs.Add(oPSD);
                }
            }
            else if (sType == "Product Wise")
            {
                var oTempPSDs = from oTPSD in oProductionScheduleDetails
                                group oTPSD by new { oTPSD.ProductName } into oTemp
                                select new
                                {
                                    ProductName = oTemp.Key.ProductName,
                                    Qty = oTemp.Sum(oTPSD => oTPSD.ProductionQty)
                                };
                foreach (var oItem in oTempPSDs)
                {
                    oPSD = new ProductionScheduleDetail();
                    oPSD.ProductName = oItem.ProductName;
                    oPSD.ProductionQty = oItem.Qty;
                    oPSDs.Add(oPSD);
                }
            }
            return oPSDs;
        }
        #endregion

        #region Schedule (Swap, Undo,Redo) // Added By Sagor on 14 Oct 2014

        [HttpPost]
        public JsonResult SwapProductionSchedule(string sPSIDs, double nts)
        {
            ProductionSchedule oPS = new ProductionSchedule();
            List<ProductionSchedule> oPSs = new List<ProductionSchedule>();
            List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
            List<ProductionSchedule> oTempPSs = new List<ProductionSchedule>();
            TimeSpan oTimeSpan = new TimeSpan();
            string sMessage = "";
            try
            {
                if (sPSIDs.Trim() == "") { throw new Exception("Invalid request;"); }
                string sSQL = "Select * from View_ProductionSchedule Where ProductionScheduleID IN (" + sPSIDs + ")";
                oPSs = ProductionSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oTempPSs = ProductionSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


                if (oPSs.Count() != 2) { throw new Exception("Invalid request;"); }
                else
                {
                    sSQL = "Select * from View_ProductionScheduleDetail Where ProductionScheduleID IN (" + sPSIDs + ") Order By ProductionScheduleID";
                    oPSDs = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if ((int)oPSDs.Max(x => x.RSState) >= 6) { throw new Exception("Unable to swap. Because one of Dyeline state is in " + oPSDs.Max(x => x.RSState).ToString() + ""); }; // RS State Greater or Equal Loades in Dye Machine

                    bool bFirst = true;
                    foreach (ProductionSchedule oItem in oPSs)
                    {
                        oTimeSpan = new TimeSpan();
                        oItem.ProductionScheduleDetails = oPSDs.Where(x => x.ProductionScheduleID == oItem.ProductionScheduleID).ToList();
                        if (bFirst)
                        {
                            oItem.MachineID = oTempPSs[1].MachineID;
                            oItem.LocationID = oTempPSs[1].LocationID;
                            oItem.SwapScheduleID = oTempPSs[1].ProductionScheduleID;

                            oItem.EndTime = oItem.EndTime.AddMinutes(1);
                            oTimeSpan = oItem.EndTime - oItem.StartTime;
                            //  oItem.StartTime = new DateTime(oTempPSs[1].StartTime.Year, oTempPSs[1].StartTime.Month, oTempPSs[1].StartTime.Day, oItem.StartTime.Hour, oItem.StartTime.Minute, 0);
                            oItem.StartTime = oTempPSs[1].StartTime;
                            oItem.EndTime = oItem.StartTime + oTimeSpan;
                            bFirst = false;
                        }
                        else
                        {
                            oItem.MachineID = oTempPSs[0].MachineID;
                            oItem.LocationID = oTempPSs[0].LocationID;

                            oItem.EndTime = oItem.EndTime.AddMinutes(1);
                            oTimeSpan = oItem.EndTime - oItem.StartTime;
                            // oItem.StartTime = new DateTime(oTempPSs[0].StartTime.Year, oTempPSs[0].StartTime.Month, oTempPSs[0].StartTime.Day, oItem.StartTime.Hour, oItem.StartTime.Minute, 0);
                            oItem.StartTime = oTempPSs[0].StartTime;
                            oItem.EndTime = oItem.StartTime + oTimeSpan;
                        }

                        int nScheduleCount = NumberofScheduleInTimePeriod(oItem);
                        if (nScheduleCount > 0) { oItem.bIncreaseTime = true; }
                        else { oItem.bIncreaseTime = false; }
                        oPS = ModifiedSchedule(oItem);


                        if (oPS.ErrorMessage != "")
                        {
                            foreach (ProductionSchedule oTemp in oTempPSs)
                            {
                                oTemp.ProductionScheduleDetails = oPSDs.Where(x => x.ProductionScheduleID == oTemp.ProductionScheduleID).ToList();
                                oTemp.EndTime = oTemp.EndTime.AddMinutes(1);
                                oPS = ModifiedSchedule(oTemp);
                            }
                            throw new Exception(oPS.ErrorMessage);
                        }
                    }
                    sMessage = "Swap Successfully";
                }
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

        #region Schedule (Copy,Undo,Redo) // Added By Sagor on 14 Oct 2014
        public JsonResult CopyProductionSchedule(ProductionSchedule oPS, bool IsCreate, double nts)
        {
            ProductionSchedule oProductionSchedule = new ProductionSchedule();
            if (IsCreate)
            {
                ProductionScheduleDetail oPSD = new ProductionScheduleDetail();
                List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
                List<ProductionSchedule> oPSs = new List<ProductionSchedule>();
                try
                {
                    if (oPS.ProductionScheduleID <= 0 || oPS.ProductionScheduleDetails.Count() <= 0) { throw new Exception("Unable to copy. There is no information avaiable."); }
                    if (oPS.ProductionScheduleDetails.Where(x => x.ProductionQty <= 0).Count() > 0) { throw new Exception("Unable to copy. Because one of it's detail Quantity is zero."); }

                    if (oPS.bIncreaseTime)
                    {
                        string sSQL = "Select top(1)* from View_ProductionSchedule Where MachineID=" + oPS.MachineID + " AND EndTime>='" + oPS.StartTimeInPerfect + "' Order By StartTime ";
                        oPSs = ProductionSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }

                    oPS.ProductionScheduleID = 0;
                    oPS.ProductionScheduleNo = "";
                    oPS.ScheduleStability = "";
                    oPS.BatchNo = EnumNumericOrder.First;
                    oPS.ScheduleType = true;
                    oPS.EndTime = oPS.EndTime.AddMinutes(-1);
                    oPS.BatchGroup = 0;
                    oPS.ScheduleStatus = EnumProductionScheduleStatus.Hold;
                    foreach (ProductionScheduleDetail oItem in oPS.ProductionScheduleDetails)
                    {
                        oPSD = new ProductionScheduleDetail();
                        oPSD.ProductionTracingUnitID = oItem.ProductionTracingUnitID;
                        oPSD.DODID = oItem.DODID;
                        oPSD.ProductionQty = oItem.ProductionQty;
                        oPSD.Remarks = oItem.Remarks;
                        oPSDs.Add(oPSD);
                    }
                    oPS.ProductionScheduleDetails = oPSDs;
                    oProductionSchedule = oPS.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oPSs.Count() > 0)
                    {
                        if (oProductionSchedule.EndTime > oPSs[0].StartTime)
                        {
                            oProductionSchedule.CheckTime = oPSs[0].StartTime;
                            TimeSpan oTS = new TimeSpan();
                            oTS = (oProductionSchedule.EndTime.AddMinutes(1) - oProductionSchedule.CheckTime).Duration();
                            oProductionSchedule.IncDecTime = new DateTime(1900, 1, (oTS.Days > 0 ? oTS.Days : 1), oTS.Hours, oTS.Minutes, 0);
                        }

                    }
                    if (oProductionSchedule.ProductionScheduleID <= 0) { throw new Exception("Unable to copy."); }
                }
                catch (Exception ex)
                {
                    oProductionSchedule = new ProductionSchedule();
                    oProductionSchedule.ErrorMessage = ex.Message;
                }
            }
            else if (!IsCreate)
            {
                if (oPS.ProductionScheduleID <= 0) { throw new Exception("Unable to redo. There is no information avaiable."); }

                string sErrorMessage = "";
                if (oPS.CheckTime > new DateTime(1900, 1, 1, 0, 0, 0)) { oPS.IncDecMachineID = oPS.MachineID; }
                sErrorMessage = oPS.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (sErrorMessage.Contains('~')) { sErrorMessage = Convert.ToString(sErrorMessage.Split('~')[0]); }
                if (sErrorMessage == "Delete successfully") { oProductionSchedule.ErrorMessage = ""; }
                else { oProductionSchedule.ErrorMessage = sErrorMessage; }

            }
            else
            {
                oProductionSchedule.ErrorMessage = "Invalid Operation.";
            }


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Schedule Search By Order Ref & Buyer Ref  Added By Sagor On 30 Nov 2014

        [HttpPost]
        public JsonResult PSSeachByOrderBuyerRef(string sValue, double nts)
        {
            ProductionSchedule oProductionSchedule = new ProductionSchedule();
            ProductionScheduleDetail oProductionScheduleDetail = new ProductionScheduleDetail();
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            string sBuyerRef = sValue.Split('~')[0].Trim();
            string sOrderRef = sValue.Split('~')[1].Trim();

            try
            {
                if (sOrderRef == "" && sBuyerRef == "")
                {
                    throw new Exception("Please enter buyer or order reference.");
                }
                string sSQL = "SELECT * FROM View_ProductionScheduleDetail where RSState<7  ";
                if (sOrderRef != "") { sSQL = sSQL + " and  OrderRef Like '%" + sBuyerRef + "%'"; }
                if (sBuyerRef != "") { sSQL = sSQL + " and BuyerRef Like '%" + sBuyerRef + "%'"; }
                oProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oProductionScheduleDetails.Count() > 0)
                {
                    oProductionScheduleDetails = oProductionScheduleDetails.OrderBy(x => x.MachineName).ThenBy(x => x.ProductionScheduleNo).ToList();
                }
                else
                {
                    throw new Exception("No Information found.");
                }
            }
            catch (Exception ex)
            {
                oProductionScheduleDetails = new List<ProductionScheduleDetail>();
                oProductionScheduleDetail.ErrorMessage = ex.Message;
                oProductionScheduleDetails.Add(oProductionScheduleDetail);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionScheduleDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PSSeachByProductName(string sProductName, double nts)
        {
            ProductionSchedule oProductionSchedule = new ProductionSchedule();
            ProductionScheduleDetail oProductionScheduleDetail = new ProductionScheduleDetail();
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            try
            {
                if (sProductName.Trim() == "")
                {
                    throw new Exception("Please enter product name.");
                }
                string sSQL = "SELECT * FROM View_ProductionScheduleDetail where RSState<7 and ProductName Like '%" + sProductName + "%'  ";
                oProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oProductionScheduleDetails.Count() > 0)
                {
                    oProductionScheduleDetails = oProductionScheduleDetails.OrderBy(x => x.MachineName).ThenBy(x => x.ProductionScheduleNo).ToList();
                }
                else
                {
                    throw new Exception("No Information found.");
                }
            }
            catch (Exception ex)
            {
                oProductionScheduleDetails = new List<ProductionScheduleDetail>();
                oProductionScheduleDetail.ErrorMessage = ex.Message;
                oProductionScheduleDetails.Add(oProductionScheduleDetail);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionScheduleDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PSSeachByExecution(string sExecution, double nts)
        {
            ProductionSchedule oProductionSchedule = new ProductionSchedule();
            ProductionScheduleDetail oProductionScheduleDetail = new ProductionScheduleDetail();
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            try
            {
                if (sExecution.Trim() == "")
                {
                    throw new Exception("Please enter a valid execution no.");
                }
                string sSQL = "SELECT * FROM View_ProductionScheduleDetail where RSState<7 and OrderNo Like '%" + sExecution.Trim() + "%'  ";
                oProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oProductionScheduleDetails.Count() > 0)
                {
                    oProductionScheduleDetails = oProductionScheduleDetails.OrderBy(x => x.MachineName).ThenBy(x => x.ProductionScheduleNo).ToList();
                }
                else
                {
                    throw new Exception("No Information found.");
                }
            }
            catch (Exception ex)
            {
                oProductionScheduleDetails = new List<ProductionScheduleDetail>();
                oProductionScheduleDetail.ErrorMessage = ex.Message;
                oProductionScheduleDetails.Add(oProductionScheduleDetail);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionScheduleDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PSRefresh(double nts)
        {
            ProductionScheduleDetail oProductionScheduleDetail = new ProductionScheduleDetail();
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            try
            {
                string sSQL = "SELECT * FROM View_ProductionScheduleDetail where RSState<7";
                oProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oProductionScheduleDetails.Count() > 0)
                {
                    oProductionScheduleDetails = oProductionScheduleDetails.OrderBy(x => x.MachineName).ThenBy(x => x.ProductionScheduleNo).ToList();
                }
                else
                {
                    throw new Exception("No Information found.");
                }
            }
            catch (Exception ex)
            {
                oProductionScheduleDetails = new List<ProductionScheduleDetail>();
                oProductionScheduleDetail.ErrorMessage = ex.Message;
                oProductionScheduleDetails.Add(oProductionScheduleDetail);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionScheduleDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Production Report

        public ActionResult ViewProductionReport(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();

            List<AuthorizationUserOEDO> oAUOEDOs = new List<AuthorizationUserOEDO>();
            oAUOEDOs = AuthorizationUserOEDO.GetsByUser(((User)(Session[SessionInfo.CurrentUser])).UserID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Summary = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._ReportView, "ProductionSchedule", oAUOEDOs);

            return View(oPSDs);
        }

        [HttpGet]
        public JsonResult SearchForProductionReport(string sValue, double nts)
        {
            string eCompareOperator = sValue.Split('~')[0];
            DateTime StartDate = Convert.ToDateTime(sValue.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sValue.Split('~')[2]);
            int nMachineID = Convert.ToInt32(sValue.Split('~')[3]);
            int nLocationID = Convert.ToInt32(sValue.Split('~')[4]);
            StartDate = StartDate.AddHours(09);
            EndDate = EndDate.AddDays(1).AddHours(09);
            string sReturn = "";
            if (nMachineID > 0 && nLocationID > 0)
            {
                sReturn = "Select * from View_ProductionScheduleDetail Where RouteSheetID Is Not Null And RSState>=7 And ProductionScheduleID IN (Select ProductionScheduleID from ProductionSchedule Where MachineID=" + nMachineID + " And LocationID=" + nLocationID + ")";  // RSState=7  UnloadFromDyemachine
            }
            else
            {
                sReturn = "Select * from View_ProductionScheduleDetail Where RouteSheetID Is Not Null And RSState>=7";  // RSState=7  UnloadFromDyemachine
            }
            string sSQL = "";
            if (eCompareOperator != EnumCompareOperator.None.ToString())
            {
                if (eCompareOperator == EnumCompareOperator.Between.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.ToString("dd MMM yyyy") + "' AND StartTime< '" + EndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND EventTime< '" + EndDate.ToString("dd MMM yyyy HH:mm") + "'";
                }
                else if (eCompareOperator == EnumCompareOperator.NotBetween.ToString())
                {
                    //sSQL = sSQL + " And StartTime Not Between '" + StartDate.ToString("dd MMM yyyy") + "' AND '" + EndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime Not Between '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND '" + EndDate.ToString("dd MMM yyyy HH:mm") + "'";

                }
                else if (eCompareOperator == EnumCompareOperator.EqualTo.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.ToString("dd MMM yyyy") + "' AND StartTime< '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND EventTime< '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                }

                else if (eCompareOperator == EnumCompareOperator.NotEqualTo.ToString())
                {
                    //sSQL = sSQL + " And StartTime Not Between '" + StartDate.ToString("dd MMM yyyy") + "' AND '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime Not Between '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                }
                else if (eCompareOperator == EnumCompareOperator.GreaterThan.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                }
                else if (eCompareOperator == EnumCompareOperator.SmallerThan.ToString())
                {
                    //sSQL = sSQL + " And StartTime< '" + StartDate.ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime< '" + StartDate.ToString("dd MMM yyyy HH:mm") + "'";
                }

            }

            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            oProductionScheduleDetails = MapProductionScheduleDetailWithRouteSheetDetail(sReturn, sSQL);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionScheduleDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PrintProductionScheduleReport(string sValue, double nts)
        {

            string eCompareOperator = sValue.Split('~')[0];
            DateTime StartDate = Convert.ToDateTime(sValue.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sValue.Split('~')[2]);
            int nMachineID = Convert.ToInt32(sValue.Split('~')[3]);
            int nLocationID = Convert.ToInt32(sValue.Split('~')[4]);
            StartDate = StartDate.AddHours(09);
            EndDate = EndDate.AddDays(1).AddHours(09);
            string sReturn = "";
            if (nMachineID > 0 && nLocationID > 0)
            {
                sReturn = "Select * from View_ProductionScheduleDetail Where RouteSheetID Is Not Null And RSState>=7 And ProductionScheduleID IN (Select ProductionScheduleID from ProductionSchedule Where MachineID=" + nMachineID + " And LocationID=" + nLocationID + ")";  // RSState=7  UnloadFromDyemachine
            }
            else
            {
                sReturn = "Select * from View_ProductionScheduleDetail Where RouteSheetID Is Not Null And RSState>=7";  // RSState=7  UnloadFromDyemachine
            }
            string sSQL = "";
            string sDateRange = "";
            if (eCompareOperator != EnumCompareOperator.None.ToString())
            {
                if (eCompareOperator == EnumCompareOperator.Between.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.ToString("dd MMM yyyy") + "' AND StartTime< '" + EndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND EventTime< '" + EndDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    sDateRange = "Date: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + EndDate.ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.NotBetween.ToString())
                {
                    //sSQL = sSQL + " And StartTime Not Between '" + StartDate.ToString("dd MMM yyyy") + "' AND '" + EndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime Not Between '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND '" + EndDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    sDateRange = "Date except: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + EndDate.ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.EqualTo.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.ToString("dd MMM yyyy") + "' AND StartTime< '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND EventTime< '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    sDateRange = "Date: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }

                else if (eCompareOperator == EnumCompareOperator.NotEqualTo.ToString())
                {
                    //sSQL = sSQL + " And StartTime Not Between '" + StartDate.ToString("dd MMM yyyy") + "' AND '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime Not Between '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    //sDateRange = "Date except: " + StartDate.ToString("dd MMM yyyy");
                    sDateRange = "Date except: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.GreaterThan.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    //sDateRange = "Date greater than: " + StartDate.ToString("dd MMM yyyy");
                    sDateRange = "Date greater: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.SmallerThan.ToString())
                {
                    //sSQL = sSQL + " And StartTime< '" + StartDate.ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime< '" + StartDate.ToString("dd MMM yyyy HH:mm") + "'";
                    //sDateRange = "Date smaller than: " + StartDate.ToString("dd MMM yyyy");
                    sDateRange = "Date smaller than: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }

            }

            Company oCompany = new Company();
            _oProductionSchedule.Company = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.Company.CompanyLogo = GetCompanyLogo(_oProductionSchedule.Company);
            _oProductionSchedule.ProductionScheduleDetails = MapProductionScheduleDetailWithRouteSheetDetail(sReturn, sSQL);


            if (_oProductionSchedule.ProductionScheduleDetails.Count > 0 && sDateRange == "")
            {
                sDateRange = "Date: " + _oProductionSchedule.ProductionScheduleDetails[0].StartTime.ToString("dd MMM yyy") + " - " + _oProductionSchedule.ProductionScheduleDetails[_oProductionSchedule.ProductionScheduleDetails.Count - 1].EndTime.ToString("dd MMM yyy");
            }
            rptProduction oReport = new rptProduction();
            byte[] abytes = oReport.PrepareReport(_oProductionSchedule.ProductionScheduleDetails, _oProductionSchedule.Company, sDateRange);
            return File(abytes, "application/pdf");
        }


        List<ProductionScheduleDetail> MapProductionScheduleDetailWithRouteSheetDetail(string sReturn, string sSQL)
        {
            string sTempSQL = sReturn;
            string sRHESQL = " AND RouteSheetID IN (SELECT RouteSheetID FROM RouteSheetHistory WHERE [EVENT]=7" + sSQL + ")";//Event(7) =Unload from Dye Machine
            sReturn = sReturn + sRHESQL + " Order By StartTime";

            List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
            oPSDs = ProductionScheduleDetail.GetsSqL(sReturn, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<RouteSheetDetail> oRSDs = new List<RouteSheetDetail>();

            sReturn = "Select * from View_RouteSheetDetail Where RouteSheetID In ( " + sTempSQL.Replace("*", "RouteSheetID") + " " + sRHESQL + ")";
            oRSDs = RouteSheetDetail.Gets(sReturn, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = " WHERE RouteSheetID IN(" + string.Join(",", (from RD in oRSDs select RD.RouteSheetID).Distinct().ToList()) + ") AND ( [EVENT]=6 Or [EVENT]=7 )";
            List<RouteSheetHistory> oRHEs = new List<RouteSheetHistory>();
            oRHEs = RouteSheetHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var oRSDValues = (from oRSD in oRSDs
                              where oRSD.RouteSheetID > 0 && oRSD.ProductCategoryName != ""           //     where oRSD.ProductCategoryName == "Dyes"
                              group oRSD by new { oRSD.RouteSheetID, oRSD.ProductCategoryName } into otempRSD
                              //where oRSD.ProductCategoryName == "Dyes"
                              //group oRSD by oRSD.RouteSheetID into otempRSD
                              select new
                              {
                                  RouteSheetID = otempRSD.Key.RouteSheetID,
                                  ProductCategoryName = otempRSD.Key.ProductCategoryName,
                                  Duration = (from oRHE in oRHEs
                                              where oRHE.RouteSheetID == otempRSD.Key.RouteSheetID && (int)oRHE.EventEmpID == 6
                                              select
                                                  (from oRHE2 in oRHEs
                                                   where oRHE2.RouteSheetID == otempRSD.Key.RouteSheetID && (int)oRHE2.EventEmpID == 7
                                                   select
                                                       oRHE2.EventTime.Subtract(oRHE.EventTime)).ElementAtOrDefault(0).ToString("dd'd 'hh'h 'mm'm'")).ElementAt(0),

                                  ShadePercentage = Math.Round(CalculateShadePercentage(oRSDs.Where(x => x.RouteSheetID == otempRSD.Key.RouteSheetID).ToList()), 2),
                                  Remarks = otempRSD.Sum(oRSD => oRSD.AddThreeLotID) > 0 ? "Ok, ADD-03" :
                                            otempRSD.Sum(oRSD => oRSD.AddTwoLotID) > 0 ? "Ok, ADD-02" :
                                            otempRSD.Sum(oRSD => oRSD.AddOneLotID) > 0 ? "Ok, ADD-01" : "Ok"

                              }).OrderBy(oOB => oOB.RouteSheetID).ToList();

            var oTempoRSDValues = oRSDValues;

            for (int i = 0; i < oTempoRSDValues.Count(); i++)
            {
                int nCount = oRSDValues.Count(x => x.RouteSheetID == oTempoRSDValues[i].RouteSheetID);
                if (nCount > 1)
                {
                    oRSDValues.RemoveAll(x => x.RouteSheetID == oTempoRSDValues[i].RouteSheetID && x.ProductCategoryName == "Chemical");
                }

            }


            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            foreach (ProductionScheduleDetail oItem in oPSDs)
            {
                foreach (var oTemp in oRSDValues)
                {
                    if (oTemp.RouteSheetID == oItem.RouteSheetID)
                    {
                        //if (oItem.UsesWeight > 0)
                        //{
                        //    if ((oItem.ProductionQty * 100) / oItem.UsesWeight > 100)
                        //    {
                        //        oItem.Loading = 100;
                        //    }
                        //    else
                        //    {
                        //        oItem.Loading = Math.Round((oItem.ProductionQty * 100) / oItem.UsesWeight);

                        //    }
                        //}
                        if (oItem.RedyingForRSNo != "")
                        {
                            oItem.PRRemarks = oTemp.Remarks + ", R-" + oItem.RedyingForRSNo;
                        }
                        else
                        {
                            oItem.PRRemarks = oTemp.Remarks;
                        }
                        oItem.DurationInString = oTemp.Duration;
                        oItem.ShadePercentage = (oTemp.ShadePercentage <= 0) ? 0 : oTemp.ShadePercentage;
                        oProductionScheduleDetails.Add(oItem);
                    }
                }
            }
            return oProductionScheduleDetails;
        }

        private double CalculateShadePercentage(List<RouteSheetDetail> oRSDs)
        {
            double nCDShadePercentagre = 0;
            double nAdditionalPercentage = 0;
            if (oRSDs.Count > 0)
            {
                List<RSDTree> oRSDTrees = new List<RSDTree>();
                RSDTree oRSDTree = this.MakeRSDTree(oRSDs);
                _oRSDTs = new List<RSDTree>();
                this.GetRouteSheetDetails(oRSDTree);
                oRSDTrees = _oRSDTs;
                bool bValue = false;
                int nChild = 0;
                foreach (RSDTree oItem in oRSDTrees)
                {
                    if (oItem.children.Count > 0)
                    {
                        if (oItem.text.Trim() == "COTTON DYEING")
                        {
                            nChild = oItem.children.Count;
                            bValue = true;
                        }
                        else
                        {
                            bValue = false;
                        }
                    }
                    else
                    {

                        if (bValue == true && oItem.Percentage > 0)
                        {
                            nCDShadePercentagre = nCDShadePercentagre + oItem.Percentage + ((oItem.Percentage * oItem.DeriveGL) / 100);
                            nAdditionalPercentage = nAdditionalPercentage + oItem.AddOnePercentage + oItem.AddTwoPercentage + oItem.AddThreePercentage;

                        }
                    }
                }

            }
            _oRSDTs = new List<RSDTree>();
            return (nCDShadePercentagre + nAdditionalPercentage);
        }

        private void GetRouteSheetDetails(RSDTree oRSDT)
        {
            if (oRSDT.parentid != 0)
            {
                _oRSDTs.Add(oRSDT);
            }
            foreach (RSDTree oItem in oRSDT.children)
            {
                RSDTree oTemp = oItem;
                GetRouteSheetDetails(oTemp);
            }
        }

        #region Production Report With Cost  Added By Sagor


        public ActionResult PrintProductionWithCost(string sValue, double nts)
        {

            string eCompareOperator = sValue.Split('~')[0];
            DateTime StartDate = Convert.ToDateTime(sValue.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sValue.Split('~')[2]);
            int nMachineID = Convert.ToInt32(sValue.Split('~')[3]);
            int nLocationID = Convert.ToInt32(sValue.Split('~')[4]);
            StartDate = StartDate.AddHours(09);
            EndDate = EndDate.AddDays(1).AddHours(09);
            string sReturn = "";
            if (nMachineID > 0 && nLocationID > 0)
            {
                sReturn = "Select * from View_ProductionScheduleDetail Where RouteSheetID Is Not Null And RSState>=7 And ProductionScheduleID IN (Select ProductionScheduleID from ProductionSchedule Where MachineID=" + nMachineID + " And LocationID=" + nLocationID + ")";  // RSState=7  UnloadFromDyemachine
            }
            else
            {
                sReturn = "Select * from View_ProductionScheduleDetail Where RouteSheetID Is Not Null And RSState>=7";  // RSState=7  UnloadFromDyemachine
            }
            string sSQL = "";
            string sDateRange = "";
            if (eCompareOperator != EnumCompareOperator.None.ToString())
            {
                if (eCompareOperator == EnumCompareOperator.Between.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.ToString("dd MMM yyyy") + "' AND StartTime< '" + EndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND EventTime< '" + EndDate.ToString("dd MMM yyyy HH:mm") + "'";
                    sDateRange = "Date: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + EndDate.ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.NotBetween.ToString())
                {
                    //sSQL = sSQL + " And StartTime Not Between '" + StartDate.ToString("dd MMM yyyy") + "' AND '" + EndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime Not Between '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND '" + EndDate.ToString("dd MMM yyyy HH:mm") + "'";
                    sDateRange = "Date except: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + EndDate.ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.EqualTo.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.ToString("dd MMM yyyy") + "' AND StartTime< '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND EventTime< '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    sDateRange = "Date: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }

                else if (eCompareOperator == EnumCompareOperator.NotEqualTo.ToString())
                {
                    //sSQL = sSQL + " And StartTime Not Between '" + StartDate.ToString("dd MMM yyyy") + "' AND '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime Not Between '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    //sDateRange = "Date except: " + StartDate.ToString("dd MMM yyyy");
                    sDateRange = "Date except: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.GreaterThan.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    //sDateRange = "Date greater than: " + StartDate.ToString("dd MMM yyyy");
                    sDateRange = "Date greater: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.SmallerThan.ToString())
                {
                    //sSQL = sSQL + " And StartTime< '" + StartDate.ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime< '" + StartDate.ToString("dd MMM yyyy HH:mm") + "'";
                    //sDateRange = "Date smaller than: " + StartDate.ToString("dd MMM yyyy");
                    sDateRange = "Date smaller than: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }

            }

            Company oCompany = new Company();
            _oProductionSchedule.Company = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.Company.CompanyLogo = GetCompanyLogo(_oProductionSchedule.Company);
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            oProductionScheduleDetails = MapProductionScheduleDetailWithRouteSheetDetailWithCost(sReturn, sSQL);

            if (oProductionScheduleDetails.Count > 0 && sDateRange == "")
            {
                sDateRange = "Date: " + oProductionScheduleDetails.Min(x => x.DyeUnLoadTime).ToString("dd MMM yyyy HH:mm tt") + " - " + oProductionScheduleDetails.Max(x => x.DyeUnLoadTime).ToString("dd MMM yyyy HH:mm tt");
            }
            rptProductionWithCost oReport = new rptProductionWithCost();
            byte[] abytes = oReport.PrepareReport(oProductionScheduleDetails, _oProductionSchedule.Company, sDateRange);
            return File(abytes, "application/pdf");
        }

        List<ProductionScheduleDetail> MapProductionScheduleDetailWithRouteSheetDetailWithCost(string sReturn, string sSQL)
        {

            string sTempSQL = sReturn;
            string sRHESQL = " AND RouteSheetID IN (SELECT RouteSheetID FROM RouteSheetHistory WHERE [EVENT]=7" + sSQL + ")";//Event(7) =Unload from Dye Machine
            sReturn = sReturn + sRHESQL + " Order By StartTime";

            List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
            oPSDs = ProductionScheduleDetail.GetsSqL(sReturn, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<RouteSheetDetail> oRSDs = new List<RouteSheetDetail>();

            sReturn = "Select * from View_RouteSheetDetail Where RouteSheetID In ( " + sTempSQL.Replace("*", "RouteSheetID") + " " + sRHESQL + ")";
            oRSDs = RouteSheetDetail.Gets(sReturn, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = " WHERE RouteSheetID IN(" + string.Join(",", (from RD in oRSDs select RD.RouteSheetID).Distinct().ToList()) + ") AND ( [EVENT]=6 Or [EVENT]=7 )";
            List<RouteSheetHistory> oRHEs = new List<RouteSheetHistory>();
            oRHEs = RouteSheetHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            InventoryTransaction oIT = new InventoryTransaction();
            List<InventoryTransaction> oITs = new List<InventoryTransaction>();
            sSQL = "Select IT.*, LT.LotNo, Product.ProductCode,Product.ProductName,Location.Name as LocationName,OperationUnit.OperationUnitName" +
                   " from ITransactionA as IT ,WorkingUnit,Location,OperationUnit,LotA AS LT,Product where" +
                   " IT.ProductID=Product.ProductID and IT.WorkingUnitID=WorkingUnit.WorkingUnitID and Location.LocationID=WorkingUnit.LocationID" +
                   " and OperationUnit.OperationUnitID=WorkingUnit.OperationUnitID and LT.LotID=IT.LotID and Product.ProductID=IT.ProductID" +
                   " and TriggerParentType=107 and TriggerParentID IN (" + string.Join(",", (from x in oRSDs select x.RouteSheetDetailID).ToList()) + ")";
            oITs = InventoryTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var oRSDValues = (from oRSD in oRSDs
                              where oRSD.RouteSheetID > 0 && oRSD.ProductCategoryName != ""
                              group oRSD by new { oRSD.RouteSheetID, oRSD.ProductCategoryName } into otempRSD
                              select new
                              {
                                  RouteSheetID = otempRSD.Key.RouteSheetID,
                                  ProductCategoryName = otempRSD.Key.ProductCategoryName,
                                  DyeChemicalCost = (from oT in oITs
                                                     where (from oRSD in oRSDs
                                                            where oRSD.RouteSheetID == otempRSD.Key.RouteSheetID
                                                                && oRSD.ProductCategoryName == otempRSD.Key.ProductCategoryName
                                                            select oRSD.RouteSheetDetailID).Contains(oT.TriggerParentID)
                                                     select oT).Sum(oT => oT.Qty * oT.UnitPrice),

                                  Duration = (from oRHE in oRHEs
                                              where oRHE.RouteSheetID == otempRSD.Key.RouteSheetID && (int)oRHE.EventEmpID == 6
                                              select
                                                  (from oRHE2 in oRHEs
                                                   where oRHE2.RouteSheetID == otempRSD.Key.RouteSheetID && (int)oRHE2.EventEmpID == 7
                                                   select
                                                       oRHE2.EventTime.Subtract(oRHE.EventTime)).ElementAtOrDefault(0).ToString("dd'd 'hh'h 'mm'm'")).ElementAt(0),

                                  ShadePercentage = Math.Round(CalculateShadePercentage(oRSDs.Where(x => x.RouteSheetID == otempRSD.Key.RouteSheetID).ToList()), 2),
                                  Remarks = otempRSD.Sum(oRSD => oRSD.AddThreeLotID) > 0 ? "Ok, ADD-03" :
                                            otempRSD.Sum(oRSD => oRSD.AddTwoLotID) > 0 ? "Ok, ADD-02" :
                                            otempRSD.Sum(oRSD => oRSD.AddOneLotID) > 0 ? "Ok, ADD-01" : "Ok",
                                  DyePerCentage = otempRSD.Sum(oRSD => oRSD.AddThreeLotID) > 0 ? (otempRSD.Where(x => x.AddThreeLotID > 0).ToList().Sum(p => p.AddThreePercentage)) + (otempRSD.Where(x => x.AddTwoLotID > 0).ToList().Sum(p => p.AddTwoPercentage)) + (otempRSD.Where(x => x.AddOneLotID > 0).ToList().Sum(p => p.AddOnePercentage)) :
                                            otempRSD.Sum(oRSD => oRSD.AddTwoLotID) > 0 ? (otempRSD.Where(x => x.AddTwoLotID > 0).ToList().Sum(p => p.AddTwoPercentage)) + (otempRSD.Where(x => x.AddOneLotID > 0).ToList().Sum(p => p.AddOnePercentage)) :
                                            otempRSD.Sum(oRSD => oRSD.AddOneLotID) > 0 ? (otempRSD.Where(x => x.AddOneLotID > 0).ToList().Sum(p => p.AddOnePercentage)) : -1

                              }).OrderBy(oOB => oOB.RouteSheetID).ThenByDescending(oOB => oOB.ProductCategoryName).ToList();



            var oTempoRSDValues = (from oRSDValue in oRSDValues
                                   where oRSDValue.RouteSheetID > 0 && oRSDValue.ProductCategoryName != ""
                                   group oRSDValue by new { oRSDValue.RouteSheetID } into otempRSD
                                   select new
                                   {
                                       RouteSheetID = otempRSD.Key.RouteSheetID,
                                       DyesCost = otempRSD.Where(x => x.ProductCategoryName == "Dyes").Sum(x => x.DyeChemicalCost),
                                       ChemicalCost = otempRSD.Where(x => x.ProductCategoryName == "Chemical").Sum(x => x.DyeChemicalCost),
                                       DyeChemicalCost = otempRSD.Sum(x => x.DyeChemicalCost),
                                       ShadePercentage = otempRSD.Select(x => x.ShadePercentage).ElementAt(0),
                                       Remarks = otempRSD.Select(x => x.Remarks).ElementAt(0),
                                       Duration = otempRSD.Select(x => x.Duration).ElementAt(0),
                                       DyePerCentage = otempRSD.Select(x => x.DyePerCentage).ElementAt(0)
                                   }).OrderBy(oOB => oOB.RouteSheetID).ToList();


            //for (int i = 0; i < oTempoRSDValues.Count(); i++)
            //{
            //    int nCount = oRSDValues.Count(x => x.RouteSheetID == oTempoRSDValues[i].RouteSheetID);


            //    if (nCount > 1)
            //    {
            //        oRSDValues.RemoveAll(x => x.RouteSheetID == oTempoRSDValues[i].RouteSheetID && x.ProductCategoryName == "Chemical");
            //    }

            //}
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            foreach (ProductionScheduleDetail oItem in oPSDs)
            {
                foreach (var oTemp in oTempoRSDValues)
                {
                    if (oTemp.RouteSheetID == oItem.RouteSheetID)
                    {
                        //if (oItem.UsesWeight > 0)
                        //{
                        //    if ((oItem.ProductionQty * 100) / oItem.UsesWeight > 100)
                        //    {
                        //        oItem.Loading = 100;
                        //    }
                        //    else
                        //    {
                        //        oItem.Loading = Math.Round((oItem.ProductionQty * 100) / oItem.UsesWeight);

                        //    }
                        //}
                        if (oItem.RedyingForRSNo != "")
                        {
                            oItem.PRRemarks = oTemp.Remarks + ", R-" + oItem.RedyingForRSNo;
                        }
                        else
                        {
                            oItem.PRRemarks = oTemp.Remarks;
                        }
                        oItem.DyesCost = oTemp.DyesCost;
                        oItem.ChemicalCost = oTemp.ChemicalCost;
                        oItem.DurationInString = oTemp.Duration;
                        oItem.ShadePercentage = (oTemp.ShadePercentage <= 0) ? 0 : oTemp.ShadePercentage;
                        oItem.DyeChemicalCost = oTemp.DyeChemicalCost;
                        oItem.DyePerCentage = oTemp.DyePerCentage;
                        oProductionScheduleDetails.Add(oItem);
                    }
                }
            }
            return oProductionScheduleDetails;
        }

        #endregion

        #region Print Production Summary
        public ActionResult PrintProductionSummary(string sValue, double nts)
        {

            string eCompareOperator = sValue.Split('~')[0];
            DateTime StartDate = Convert.ToDateTime(sValue.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sValue.Split('~')[2]);
            int nMachineID = Convert.ToInt32(sValue.Split('~')[3]);
            int nLocationID = Convert.ToInt32(sValue.Split('~')[4]);
            StartDate = StartDate.AddHours(09);
            EndDate = EndDate.AddDays(1).AddHours(09);
            string sReturn = "";
            if (nMachineID > 0 && nLocationID > 0)
            {
                sReturn = "Select * from View_ProductionScheduleDetail Where RouteSheetID Is Not Null And RSState>=7 And ProductionScheduleID IN (Select ProductionScheduleID from ProductionSchedule Where MachineID=" + nMachineID + " And LocationID=" + nLocationID + ")";  // RSState=7  UnloadFromDyemachine
            }
            else
            {
                sReturn = "Select * from View_ProductionScheduleDetail Where RouteSheetID Is Not Null And RSState>=7";  // RSState=7  UnloadFromDyemachine
            }
            string sSQL = "";
            string sDateRange = "";
            if (eCompareOperator != EnumCompareOperator.None.ToString())
            {
                if (eCompareOperator == EnumCompareOperator.Between.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.ToString("dd MMM yyyy") + "' AND StartTime< '" + EndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND EventTime< '" + EndDate.ToString("dd MMM yyyy HH:mm") + "'";
                    sDateRange = "Date: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + EndDate.ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.NotBetween.ToString())
                {
                    //sSQL = sSQL + " And StartTime Not Between '" + StartDate.ToString("dd MMM yyyy") + "' AND '" + EndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime Not Between '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND '" + EndDate.ToString("dd MMM yyyy HH:mm") + "'";
                    sDateRange = "Date except: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + EndDate.ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.EqualTo.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.ToString("dd MMM yyyy") + "' AND StartTime< '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND EventTime< '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    sDateRange = "Date: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }

                else if (eCompareOperator == EnumCompareOperator.NotEqualTo.ToString())
                {
                    //sSQL = sSQL + " And StartTime Not Between '" + StartDate.ToString("dd MMM yyyy") + "' AND '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime Not Between '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    //sDateRange = "Date except: " + StartDate.ToString("dd MMM yyyy");
                    sDateRange = "Date except: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.GreaterThan.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    //sDateRange = "Date greater than: " + StartDate.ToString("dd MMM yyyy");
                    sDateRange = "Date greater: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.SmallerThan.ToString())
                {
                    //sSQL = sSQL + " And StartTime< '" + StartDate.ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime< '" + StartDate.ToString("dd MMM yyyy HH:mm") + "'";
                    //sDateRange = "Date smaller than: " + StartDate.ToString("dd MMM yyyy");
                    sDateRange = "Date smaller than: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }

            }

            Company oCompany = new Company();
            _oProductionSchedule.Company = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.Company.CompanyLogo = GetCompanyLogo(_oProductionSchedule.Company);
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            oProductionScheduleDetails = MapProductionScheduleDetailWithRouteSheetDetailWithCost(sReturn, sSQL);

            if (oProductionScheduleDetails.Count > 0 && sDateRange == "")
            {
                sDateRange = "Date: " + oProductionScheduleDetails.Min(x => x.DyeUnLoadTime).ToString("dd MMM yyyy HH:mm tt") + " - " + oProductionScheduleDetails.Max(x => x.DyeUnLoadTime).ToString("dd MMM yyyy HH:mm tt");
            }
            rptProductionSummary oReport = new rptProductionSummary();
            byte[] abytes = oReport.PrepareReport(oProductionScheduleDetails, _oProductionSchedule.Company, sDateRange);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintProductionSummaryWithShadeGroup(string sValue, double nts)
        {

            string eCompareOperator = sValue.Split('~')[0];
            DateTime StartDate = Convert.ToDateTime(sValue.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sValue.Split('~')[2]);
            int nMachineID = Convert.ToInt32(sValue.Split('~')[3]);
            int nLocationID = Convert.ToInt32(sValue.Split('~')[4]);
            StartDate = StartDate.AddHours(09);
            EndDate = EndDate.AddDays(1).AddHours(09);
            string sReturn = "";
            if (nMachineID > 0 && nLocationID > 0)
            {
                sReturn = "Select * from View_ProductionScheduleDetail Where RouteSheetID Is Not Null And RSState>=7 And ProductionScheduleID IN (Select ProductionScheduleID from ProductionSchedule Where MachineID=" + nMachineID + " And LocationID=" + nLocationID + ")";  // RSState=7  UnloadFromDyemachine
            }
            else
            {
                sReturn = "Select * from View_ProductionScheduleDetail Where RouteSheetID Is Not Null And RSState>=7";  // RSState=7  UnloadFromDyemachine
            }

            string sSQL = "";
            string sDateRange = "";
            if (eCompareOperator != EnumCompareOperator.None.ToString())
            {
                if (eCompareOperator == EnumCompareOperator.Between.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.ToString("dd MMM yyyy") + "' AND StartTime< '" + EndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND EventTime< '" + EndDate.ToString("dd MMM yyyy HH:mm") + "'";
                    sDateRange = "Date: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + EndDate.ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.NotBetween.ToString())
                {
                    //sSQL = sSQL + " And StartTime Not Between '" + StartDate.ToString("dd MMM yyyy") + "' AND '" + EndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime Not Between '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND '" + EndDate.ToString("dd MMM yyyy HH:mm") + "'";
                    sDateRange = "Date except: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + EndDate.ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.EqualTo.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.ToString("dd MMM yyyy") + "' AND StartTime< '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND EventTime< '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    sDateRange = "Date: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }

                else if (eCompareOperator == EnumCompareOperator.NotEqualTo.ToString())
                {
                    //sSQL = sSQL + " And StartTime Not Between '" + StartDate.ToString("dd MMM yyyy") + "' AND '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime Not Between '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    //sDateRange = "Date except: " + StartDate.ToString("dd MMM yyyy");
                    sDateRange = "Date except: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.GreaterThan.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    //sDateRange = "Date greater than: " + StartDate.ToString("dd MMM yyyy");
                    sDateRange = "Date greater: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.SmallerThan.ToString())
                {
                    //sSQL = sSQL + " And StartTime< '" + StartDate.ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime< '" + StartDate.ToString("dd MMM yyyy HH:mm") + "'";
                    //sDateRange = "Date smaller than: " + StartDate.ToString("dd MMM yyyy");
                    sDateRange = "Date smaller than: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }

            }

            Company oCompany = new Company();
            _oProductionSchedule.Company = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.Company.CompanyLogo = GetCompanyLogo(_oProductionSchedule.Company);
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            oProductionScheduleDetails = MapProductionScheduleDetailWithRouteSheetDetailWithCost(sReturn, sSQL);

            if (oProductionScheduleDetails.Count > 0 && sDateRange == "")
            {
                sDateRange = "Date: " + oProductionScheduleDetails.Min(x => x.DyeUnLoadTime).ToString("dd MMM yyyy HH:mm tt") + " - " + oProductionScheduleDetails.Max(x => x.DyeUnLoadTime).ToString("dd MMM yyyy HH:mm tt");
            }
            rptProductionSummaryWithShadeGroup oReport = new rptProductionSummaryWithShadeGroup();
            byte[] abytes = oReport.PrepareReport(oProductionScheduleDetails, _oProductionSchedule.Company, sDateRange);
            return File(abytes, "application/pdf");
        }

        #endregion

        #region Production Report With Duration Cost   Added By Sagor


        public ActionResult PrintProductionWithDuration(string sValue, double nts)
        {

            string eCompareOperator = sValue.Split('~')[0];
            DateTime StartDate = Convert.ToDateTime(sValue.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sValue.Split('~')[2]);
            int nMachineID = Convert.ToInt32(sValue.Split('~')[3]);
            int nLocationID = Convert.ToInt32(sValue.Split('~')[4]);
            StartDate = StartDate.AddHours(09);
            EndDate = EndDate.AddDays(1).AddHours(09);
            string sReturn = "";
            if (nMachineID > 0 && nLocationID > 0)
            {
                sReturn = "Select * from View_ProductionScheduleDetail Where RouteSheetID Is Not Null And RSState>=7 And ProductionScheduleID IN (Select ProductionScheduleID from ProductionSchedule Where MachineID=" + nMachineID + " And LocationID=" + nLocationID + ")";  // RSState=7  UnloadFromDyemachine
            }
            else
            {
                sReturn = "Select * from View_ProductionScheduleDetail Where RouteSheetID Is Not Null And RSState>=7";  // RSState=7  UnloadFromDyemachine
            }
            string sSQL = "";
            string sDateRange = "";
            if (eCompareOperator != EnumCompareOperator.None.ToString())
            {
                if (eCompareOperator == EnumCompareOperator.Between.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.ToString("dd MMM yyyy") + "' AND StartTime< '" + EndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND EventTime< '" + EndDate.ToString("dd MMM yyyy HH:mm") + "'";
                    sDateRange = "Date: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + EndDate.ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.NotBetween.ToString())
                {
                    //sSQL = sSQL + " And StartTime Not Between '" + StartDate.ToString("dd MMM yyyy") + "' AND '" + EndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime Not Between '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND '" + EndDate.ToString("dd MMM yyyy HH:mm") + "'";
                    sDateRange = "Date except: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + EndDate.ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.EqualTo.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.ToString("dd MMM yyyy") + "' AND StartTime< '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND EventTime< '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    sDateRange = "Date: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }

                else if (eCompareOperator == EnumCompareOperator.NotEqualTo.ToString())
                {
                    //sSQL = sSQL + " And StartTime Not Between '" + StartDate.ToString("dd MMM yyyy") + "' AND '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime Not Between '" + StartDate.ToString("dd MMM yyyy HH:mm") + "' AND '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    //sDateRange = "Date except: " + StartDate.ToString("dd MMM yyyy");
                    sDateRange = "Date except: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.GreaterThan.ToString())
                {
                    //sSQL = sSQL + " And StartTime>= '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime>= '" + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm") + "'";
                    //sDateRange = "Date greater than: " + StartDate.ToString("dd MMM yyyy");
                    sDateRange = "Date greater: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }
                else if (eCompareOperator == EnumCompareOperator.SmallerThan.ToString())
                {
                    //sSQL = sSQL + " And StartTime< '" + StartDate.ToString("dd MMM yyyy") + "'";
                    sSQL = sSQL + " And EventTime< '" + StartDate.ToString("dd MMM yyyy HH:mm") + "'";
                    //sDateRange = "Date smaller than: " + StartDate.ToString("dd MMM yyyy");
                    sDateRange = "Date smaller than: " + StartDate.ToString("dd MMM yyyy HH:mm tt") + " - " + StartDate.AddDays(1).ToString("dd MMM yyyy HH:mm tt");
                }

            }
            Company oCompany = new Company();
            _oProductionSchedule.Company = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.Company.CompanyLogo = GetCompanyLogo(_oProductionSchedule.Company);
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            oProductionScheduleDetails = MapProductionScheduleDetailWithRouteSheetDetail(sReturn, sSQL);

            if (oProductionScheduleDetails.Count > 0 && sDateRange == "")
            {
                sDateRange = "Date: " + oProductionScheduleDetails.Min(x => x.DyeUnLoadTime).ToString("dd MMM yyyy HH:mm tt") + " - " + oProductionScheduleDetails.Max(x => x.DyeUnLoadTime).ToString("dd MMM yyyy HH:mm tt");
            }
            rptProductionWithDuration oReport = new rptProductionWithDuration();
            byte[] abytes = oReport.PrepareReport(oProductionScheduleDetails, _oProductionSchedule.Company, sDateRange);
            return File(abytes, "application/pdf");
        }

        #endregion

        #endregion

        #region Tree
        private RSDTree GetRoot()
        {
            RSDTree oRSDT = new RSDTree();
            foreach (RSDTree oItem in _oRSDTs)
            {
                if (oItem.parentid == 0)
                {
                    return oItem;
                }
            }
            return oRSDT;
        }

        private List<RSDTree> GetChild(int nRSDID)
        {
            List<RSDTree> oRSDTs = new List<RSDTree>();
            foreach (RSDTree oItem in _oRSDTs)
            {
                if (oItem.parentid == nRSDID)
                {
                    oRSDTs.Add(oItem);
                }
            }
            return oRSDTs;
        }

        private void AddTreeNodes(ref RSDTree oRSDT)
        {
            List<RSDTree> oChildNodes;
            oChildNodes = GetChild(oRSDT.id);
            oRSDT.children = oChildNodes;

            foreach (RSDTree oItem in oChildNodes)
            {
                RSDTree oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }

        private RSDTree MakeRSDTree(List<RouteSheetDetail> oRSDs)
        {
            foreach (RouteSheetDetail oItem in oRSDs)
            {

                _oRSDT = new RSDTree();
                _oRSDT.id = oItem.RouteSheetDetailID;
                _oRSDT.RouteSheetID = oItem.RouteSheetID;
                _oRSDT.parentid = oItem.ParentID;
                _oRSDT.text = oItem.ProcessName;
                _oRSDT.attributes = "";
                _oRSDT.code = "";
                _oRSDT.Description = oItem.Note;
                _oRSDT.IsDyesChemical = oItem.IsDyesChemical;
                _oRSDT.ProcessID = oItem.ProcessID;
                _oRSDT.TempTime = oItem.TempTime;
                _oRSDT.GL = oItem.GL;
                _oRSDT.Percentage = oItem.Percentage;
                _oRSDT.TotalQty = oItem.TotalQty;
                //_oRSDT.AddOne = oItem.AddOne;
                //_oRSDT.AddTwo = oItem.AddTwo;
                //_oRSDT.AddThree = oItem.AddThree;
                //_oRSDT.Return = oItem.Return;
                _oRSDT.ForCotton = oItem.ForCotton;
                _oRSDT.DeriveGL = oItem.DeriveGL;
                //_oRSDT.TotalQtyInString = oItem.TotalQtyInString;
                //_oRSDT.ProductNameCode = oItem.ProductNameCode;

                //_oRSDT.AddOneInString = oItem.AddOneInString;
                //_oRSDT.AddTwoInString = oItem.AddTwoInString;
                //_oRSDT.AddThreeInString = oItem.AddThreeInString;
                //_oRSDT.ReturnInString = oItem.ReturnInString;
                _oRSDT.GTotalInString = oItem.GTotalInString;

                _oRSDT.TotalQtyLotNo = oItem.TotalQtyLotNo;
                _oRSDT.AddOneLotNo = oItem.AddOneLotNo;
                _oRSDT.AddTwoLotNo = oItem.AddTwoLotNo;
                _oRSDT.AddThreeLotNo = oItem.AddThreeLotNo;
                _oRSDT.ReturnLotNo = oItem.ReturnLotNo;

                _oRSDT.AddOnePercentage = oItem.AddOnePercentage;
                _oRSDT.AddTwoPercentage = oItem.AddTwoPercentage;
                _oRSDT.AddThreePercentage = oItem.AddThreePercentage;

                _oRSDT.ErrorMessage = "";
                _oRSDTs.Add(_oRSDT);
            }
            _oRSDT = new RSDTree();
            _oRSDT = GetRoot();
            this.AddTreeNodes(ref _oRSDT);
            return _oRSDT;
        }
        #endregion

        #region Mail Production Report With Shade Group

        [HttpPost]
        public JsonResult SummaryReportForMail(double nts)
        {
            string sMessage = "", sDateRange = "";
            DateTime sysDate = DateTime.Now;
            //sysDate = new DateTime(2014,12, 31, 09, 00, 00);
            //DateTime sysStartTime = new DateTime(2014, 12, 1, 09, 00, 00);
            sysDate = new DateTime(sysDate.Year, sysDate.Month, sysDate.Day, 09, 00, 00);
            DateTime sysStartTime = new DateTime(sysDate.Year, sysDate.Month, 21, 09, 00, 00);
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            try
            {
                string sReturn = "Select * from View_ProductionScheduleDetail Where RouteSheetID Is Not Null And RSState>=7 ";
                string sSQL = " And EventTime>= '" + sysStartTime.ToString("dd MMM yyyy HH:mm") + "' AND EventTime< '" + sysDate.ToString("dd MMM yyyy HH:mm") + "'";
                sDateRange = sysStartTime.ToString("dd MMM yyyy HH:mm") + " To " + sysDate.ToString("dd MMM yyyy HH:mm");
                oProductionScheduleDetails = MapProductionScheduleDetailWithRouteSheetDetailWithCost(sReturn, sSQL);

                if (oProductionScheduleDetails.Count > 0)
                {
                    ReportProductionSummaryWithShadeGroup(sDateRange, oProductionScheduleDetails);
                }
                else
                {
                    throw new Exception("An error occured..");
                }
            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string ReportProductionSummaryWithShadeGroup(string sDateRange, List<ProductionScheduleDetail> oProductionScheduleDetails)
        {
            string sSubject = "ATML Dyeing Production Summary Report() @" + DateTime.Now.ToString("dd MMM yyyy HH:mm") + " [Auto Generated From ESimSOl]";
            string sBodyInformation = "";

            sBodyInformation = "<div>" + sDateRange + "</div>";
            sBodyInformation = sBodyInformation + "<table cellspacing='0' border='1' style='font-size:11px; border:1px solid gray'>";

            #region Table Header
            sBodyInformation = sBodyInformation +
                               "<thead>" +
                                    "<tr>" +
                                        "<td rowspan='2' align='center' style=' min-width:30px;'>SL No</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:65px;'>Date</td>" +
                                        "<td colspan='5' align='center' style=' min-width:192px;'>Production(kg)</td>" +
                                        "<td colspan='5' align='center' style=' min-width:162px;'>Duration</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Avg Shade (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Add. Shade (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Avg Batch</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Avg Batch Load (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Net RFT (%)</td>" +
                                        "<td rowspan='2' align='center' style=' min-width:25px;'>Gross RFT (%)</td>" +
                                        "<td colspan='3' align='center' style=' min-width:108px;'>Cost/Kg(BDT)</td>" +
                                        "<td colspan='3' align='center' style=' min-width:150px;'>Total Cost(BDT)</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td align='center' style=' min-width:40px;'>Total</td>" +
                                        "<td align='center' style=' min-width:38px;'>White (0%)</td>" +
                                        "<td align='center' style=' min-width:38px;'>Light (0.01-.5%)</td>" +
                                        "<td align='center' style=' min-width:38px;'>Medium (0.51-2%)</td>" +
                                        "<td align='center' style=' min-width:38px;'>Deep (>2%)</td>" +

                                        "<td align='center' style=' min-width:42px;'>Average</td>" +
                                        "<td align='center' style=' min-width:40px;'>White (0%)</td>" +
                                        "<td align='center' style=' min-width:40px;'>Light (0.01-.5%)</td>" +
                                        "<td align='center' style=' min-width:40px;'>Medium (0.51-2%)</td>" +
                                        "<td align='center' style=' min-width:40px;'>Deep (>2%)</td>" +

                                        "<td align='center' style=' min-width:38px;'>Total</td>" +
                                        "<td align='center' style=' min-width:32px;'>Dyes</td>" +
                                        "<td align='center' style=' min-width:38px;'>Chemical</td>" +

                                        "<td align='center' style=' min-width:50px;'>Total</td>" +
                                        "<td align='center' style=' min-width:50px;'>Dyes</td>" +
                                        "<td align='center' style=' min-width:50px;'>Chemical</td>" +
                                    "</tr>" +
                                "</thead>";

            #endregion

            if (oProductionScheduleDetails.Count() > 0)
            {
                #region Table Content
                int nCount = 0;
                int nTotalDay = 0;
                int nTotalHour = 0;
                int nTotalMin = 0;

                oProductionScheduleDetails = oProductionScheduleDetails.OrderBy(x => x.DyeUnLoadTime).ToList();
                List<ProductionScheduleDetail> oPSDs = new List<ProductionScheduleDetail>();
                while (oProductionScheduleDetails.Count() > 0)
                {

                    oPSDs = new List<ProductionScheduleDetail>();
                    DateTime StartTime = oProductionScheduleDetails[0].DyeUnLoadTime;
                    StartTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, 9, 0, 0);
                    DateTime EndTime = StartTime.AddDays(1);
                    oPSDs = oProductionScheduleDetails.Where(x => x.DyeUnLoadTime >= StartTime).Where(x => x.DyeUnLoadTime < EndTime).ToList();

                    foreach (ProductionScheduleDetail oItem in oPSDs)
                    {
                        oProductionScheduleDetails.RemoveAll(x => x.ProductionScheduleDetailID == oItem.ProductionScheduleDetailID);
                    }

                    nCount++;

                    int nDay = 0;
                    int nHour = 0;
                    int nMin = 0;
                    nDay = oPSDs.Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                    nHour = oPSDs.Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                    nMin = oPSDs.Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));

                    nTotalDay = nTotalDay + nDay; nTotalHour = nTotalHour + nHour; nTotalMin = nTotalMin + nMin;


                    if (oPSDs[0].DyeUnLoadTime.Hour < 9) { oPSDs[0].DyeUnLoadTime = oPSDs[0].DyeUnLoadTime.AddDays(-1); }

                    sBodyInformation = sBodyInformation +
                    "<tr>" +
                        "<td align='center' style=' width:30px;'>" + nCount + "</td>" +
                        "<td align='center' style=' width:45px;'>" + oPSDs[0].DyeUnLoadTime.ToString("dd MMM yyyy") + "</td>";

                    #region Production
                    sBodyInformation = sBodyInformation +
                    "<td align='right' style=' width:40px;'>" + Global.MillionFormat(oPSDs.Sum(x => x.ProductionQty)) + "</td>" +
                    "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oPSDs.Where(x => x.ShadePercentage == 0).Sum(x => x.ProductionQty)) + "</td>" +
                    "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oPSDs.Where(x => x.ShadePercentage > 0).Where(x => x.ShadePercentage <= .5).Sum(x => x.ProductionQty)) + "</td>" +
                    "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oPSDs.Where(x => x.ShadePercentage > .5).Where(x => x.ShadePercentage <= 2).Sum(x => x.ProductionQty)) + "</td>" +
                    "<td align='right' style=' width:38px;'>" + Global.MillionFormat(oPSDs.Where(x => x.ShadePercentage > 2).Sum(x => x.ProductionQty)) + "</td>";

                    #endregion

                    #region Duration
                    sBodyInformation = sBodyInformation +
                    "<td align='right' style=' width:40px;'>" + TimeStampCoversion(nDay, nHour, nMin, oPSDs.Count()) + "</td>";

                    if (oPSDs.Where(x => x.ShadePercentage == 0).Count() > 0)
                    {
                        nDay = oPSDs.Where(x => x.ShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                        nHour = oPSDs.Where(x => x.ShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                        nMin = oPSDs.Where(x => x.ShadePercentage == 0).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                    }
                    else { nDay = 0; nHour = 0; nMin = 0; }
                    sBodyInformation = sBodyInformation +
                    "<td align='right' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oPSDs.Where(x => x.ShadePercentage == 0).Count()) + "</td>";


                    if (oPSDs.Where(x => x.ShadePercentage > 0).Where(x => x.ShadePercentage <= .5).Count() > 0)
                    {
                        nDay = oPSDs.Where(x => x.ShadePercentage > 0).Where(x => x.ShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                        nHour = oPSDs.Where(x => x.ShadePercentage > 0).Where(x => x.ShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                        nMin = oPSDs.Where(x => x.ShadePercentage > 0).Where(x => x.ShadePercentage <= .5).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                    }
                    else { nDay = 0; nHour = 0; nMin = 0; }
                    sBodyInformation = sBodyInformation +
                    "<td align='right' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oPSDs.Where(x => x.ShadePercentage > 0).Where(x => x.ShadePercentage <= .5).Count()) + "</td>";


                    if (oPSDs.Where(x => x.ShadePercentage > .5).Where(x => x.ShadePercentage <= 2).Count() > 0)
                    {
                        nDay = oPSDs.Where(x => x.ShadePercentage > .5).Where(x => x.ShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                        nHour = oPSDs.Where(x => x.ShadePercentage > .5).Where(x => x.ShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                        nMin = oPSDs.Where(x => x.ShadePercentage > .5).Where(x => x.ShadePercentage <= 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                    }
                    else { nDay = 0; nHour = 0; nMin = 0; }
                    sBodyInformation = sBodyInformation +
                    "<td align='right' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oPSDs.Where(x => x.ShadePercentage > .5).Where(x => x.ShadePercentage <= 2).Count()) + "</td>";


                    if (oPSDs.Where(x => x.ShadePercentage > 2).Count() > 0)
                    {
                        nDay = oPSDs.Where(x => x.ShadePercentage > 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[0].TrimEnd(new char[] { 'd' })));
                        nHour = oPSDs.Where(x => x.ShadePercentage > 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[1].TrimEnd(new char[] { 'h' })));
                        nMin = oPSDs.Where(x => x.ShadePercentage > 2).Sum(x => int.Parse(x.DurationInString.Split(' ')[2].TrimEnd(new char[] { 'm' })));
                    }
                    else { nDay = 0; nHour = 0; nMin = 0; }
                    sBodyInformation = sBodyInformation +
                    "<td align='right' style=' width:38px;'>" + TimeStampCoversion(nDay, nHour, nMin, oPSDs.Where(x => x.ShadePercentage > 2).Count()) + "</td>";
                    #endregion

                    #region Shade Percentage, Additional, Batch, Loading, Net RFT, Gross RFT

                    sBodyInformation = sBodyInformation +
                    "<td align='right'  style=' width:25px;'>" + Math.Round(Convert.ToDouble((oPSDs.Count(x => x.ShadePercentage > 0) > 0) ? oPSDs.Sum(x => x.ShadePercentage) / oPSDs.Count(x => x.ShadePercentage > 0) : 0), 2).ToString() + "</td>";

                    double nAvgAddition = 0;
                    int nAddition = oPSDs.Count(x => x.PRRemarks != "Ok");
                    if (nAddition > 0) { nAvgAddition = oPSDs.Where(x => x.PRRemarks != "Ok").Sum(x => x.DyePerCentage) / Convert.ToDouble(nAddition); }
                    sBodyInformation = sBodyInformation +
                    "<td align='right'  style=' width:25px;'>" + nAvgAddition.ToString("0.00") + "</td>" +
                    "<td align='right'  style=' width:25px;'>" + (Convert.ToDouble(oPSDs.Count()) / Convert.ToDouble(oPSDs.Select(x => x.MachineName).Distinct().Count())).ToString("0.00") + "</td>" +
                    "<td align='right'  style=' width:25px;'>" + Convert.ToDouble(oPSDs.Sum(x => x.Loading) / Convert.ToDouble(oPSDs.Count())).ToString("0.00") + "</td>";

                    double nNetRFT = 0;
                    nNetRFT = oPSDs.Count(x => x.PRRemarks == "Ok") > 0 ? (oPSDs.Count(x => x.PRRemarks == "Ok") * 100) / Convert.ToDouble(oPSDs.Count()) : 0;
                    sBodyInformation = sBodyInformation +
                    "<td align='right'  style=' width:25px;'>" + Math.Round(nNetRFT, 2).ToString() + "</td>";

                    int nAddOneOkCount = oPSDs.Count(x => x.PRRemarks == "Ok, ADD-01"); int nAddTwoOkCount = oPSDs.Count(x => x.PRRemarks == "Ok, ADD-02"); int nAddThreeOkCount = oPSDs.Count(x => x.PRRemarks == "Ok, ADD-03");
                    double nGrossRFT = 0;
                    if ((oPSDs.Count(x => x.PRRemarks == "Ok") + nAddOneOkCount + nAddTwoOkCount + nAddThreeOkCount) > 0)
                    {
                        nGrossRFT = nNetRFT + ((nAddOneOkCount * 65) + (nAddTwoOkCount * 35) + (nAddThreeOkCount * 0)) / Convert.ToDouble(oPSDs.Count());
                    }
                    sBodyInformation = sBodyInformation +
                    "<td align='right'  style=' width:25px;'>" + Math.Round(nGrossRFT, 2).ToString() + "</td>";
                    #endregion

                    #region Cost Calculation Per Kg
                    sBodyInformation = sBodyInformation +
                    "<td align='center' style=' width:38px;'>" + Global.MillionFormat(oPSDs.Sum(x => x.DyeChemicalCost) / oPSDs.Sum(x => x.ProductionQty)) + "</td>" +
                    "<td align='center' style=' width:32px;'>" + Global.MillionFormat(oPSDs.Sum(x => x.DyesCost) / oPSDs.Sum(x => x.ProductionQty)) + "</td>" +
                    "<td align='center' style=' width:38px;'>" + Global.MillionFormat(oPSDs.Sum(x => x.ChemicalCost) / oPSDs.Sum(x => x.ProductionQty)) + "</td>";
                    #endregion

                    #region Total Cost Calculation
                    sBodyInformation = sBodyInformation +
                    "<td align='center' style=' width:50px;'>" + Global.MillionFormat(oPSDs.Sum(x => x.DyeChemicalCost)) + "</td>" +
                    "<td align='center' style=' width:50px;'>" + Global.MillionFormat(oPSDs.Sum(x => x.DyesCost)) + "</td>" +
                    "<td align='center' style=' width:50px;'>" + Global.MillionFormat(oPSDs.Sum(x => x.ChemicalCost)) + "</td>";
                    #endregion

                    sBodyInformation = sBodyInformation + "</tr>";
                }

                #endregion
            }

            #region Table Footer
            sBodyInformation = sBodyInformation + "</table>";
            #endregion

            return sBodyInformation;
        }

        #region Day Hour Min Conversion

        public String TimeStampCoversion(int nDay, int nHour, int nMin, int nCount)
        {
            string sAvgTime = "";
            if (nMin > 0)
            {
                nHour = nHour + (nMin / 60);
                nMin = nMin % 60;
            }
            if (nHour >= 24)
            {
                nDay = nDay + (nHour / 24);
                nHour = nHour % 24;
            }

            if (nMin > 0 || nHour > 0 || nDay > 0)
            {
                int nRemainDay = nDay % nCount;
                if (nRemainDay != 0) { nHour += nRemainDay * 24; }
                int nReminHour = nHour % nCount;
                if (nReminHour != 0) { nMin += nReminHour * 60; }

                int nAvgDay = nDay / nCount;
                int nAvgHour = nHour / nCount;
                double nAvgMin = nMin / nCount;

                string sAvgDay = "", sAvgHour = "", sAvgMin = "";
                if (nAvgDay < 10) { sAvgDay = "0" + nAvgDay.ToString(); } else { sAvgDay = nAvgDay.ToString(); }
                if (nAvgHour < 10) { sAvgHour = "0" + nAvgHour.ToString(); } else { sAvgHour = nAvgHour.ToString(); }
                if (nAvgMin < 10)
                {
                    nAvgMin = Math.Round(nAvgMin, 2);
                    if (nAvgMin.ToString().Contains('.'))
                    {
                        sAvgMin = "0" + nAvgMin.ToString().Split('.')[0] + "." + nAvgMin.ToString().Split('.')[1];
                    }
                    else
                    {
                        sAvgMin = "0" + nAvgMin.ToString();
                    }
                }
                else
                {
                    sAvgMin = nAvgMin.ToString();
                }
                if (Convert.ToInt32(sAvgDay) <= 0) { sAvgTime = sAvgHour + "h " + sAvgMin + "m"; }
                else { sAvgTime = sAvgDay + "d " + sAvgHour + "h " + sAvgMin + "m"; }

            }
            return sAvgTime;
        }

        #endregion
        #endregion

        #region MAMUN
        public ActionResult ViewDUProductionSchedule(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            string sLocationIDs = "";
            List<Location> oLocations = new List<Location>();
            List<CapitalResource> oCapitalResources = new List<CapitalResource>();

            _sQuery = "SELECT * FROM Location where LocationID in(SELECT Distinct LocationID FROM VIEW_CapitalResource where ISNULL(IsActive,1)=1 AND BUID = " + buid + " AND ResourcesType = " + (int)EnumResourcesType.Machine + " )";
            oLocations = Location.Gets(_sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCapitalResources = CapitalResource.GetsByBUandResourceTypeWithName(buid, (int)EnumResourcesType.Machine, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            sLocationIDs = Location.IDInString(oLocations);

            DateTime today = DateTime.Today;
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            string sql = "Select * from View_ProductionSchedule where LocationID=" + sLocationIDs + " and ProductionScheduleID in ( Select distinct(ProductionScheduleID) from View_ProductionScheduleDetail Where ((StartTime>='" + startOfMonth + "' and StartTime<='" + endOfMonth + "') or (EndTime>='" + startOfMonth + "' and EndTime<='" + endOfMonth + "'))  and RSState<7 ) and Activity='1' order by MachineID,StartTime ";
            _oProductionSchedule.ProductionScheduleList = ProductionSchedule.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oProductionSchedule.ProductionScheduleList.Count>0)
            {
                sql = "Select * from View_ProductionScheduleDetail Where ProductionScheduleID IN (Select ProductionScheduleID from View_ProductionSchedule where LocationID=" + sLocationIDs + " and ProductionScheduleID in ( Select distinct(ProductionScheduleID) from View_ProductionScheduleDetail Where ((StartTime>='" + startOfMonth + "' and StartTime<='" + endOfMonth + "') or (EndTime>='" + startOfMonth + "' and EndTime<='" + endOfMonth + "'))  and RSState<7 ) and Activity='1' )";  // RSState=7  UnloadFromDyemachine
                _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.GetsSqL(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
          

            foreach (CapitalResource oItem in oCapitalResources)
            {
                var nCount = (from oPS in _oProductionSchedule.ProductionScheduleList where oPS.MachineID == oItem.CRID && oPS.BUID == oItem.BUID select oPS).Count();
                oItem.MachineNoWithCapacityAndTotalSchedule = "[" + oItem.CRID + "]" + "[" + oItem.Code + "]" + oItem.Name + "[" + oItem.MachineCapacity + "]" + " (" + Convert.ToInt32(nCount) + ")";
            }

            //oTWGLDDetails = oLabDipDetails.Where(x => x.TwistedGroup > 0).ToList();
            foreach (ProductionSchedule oItem in _oProductionSchedule.ProductionScheduleList)
            {
                oItem.ProductionScheduleDetails = new List<ProductionScheduleDetail>();
                oItem.ProductionScheduleDetails = _oProductionSchedule.ProductionScheduleDetails.Where(x => x.ProductionScheduleID == oItem.ProductionScheduleID).ToList();
                foreach (ProductionScheduleDetail oItem2 in oItem.ProductionScheduleDetails)
                {
                    oItem.ProductionScheduleNo = "Time:" +oItem.StartTime.ToString("HH:MM")+"-"+oItem.EndTime.ToString("HH:MM")+"   </br> Batch:" + oItem.ProductionScheduleNo + "</br> Order No:" + oItem2.OrderNo + "" + "</br>" + oItem2.BuyerName;
                }

            }

            //List<MachineWiseProductionSchedule> oTempMPSs = new List<MachineWiseProductionSchedule>();
            //oTempMPSs = MapScheduleByMachine(startOfMonth, endOfMonth, _oProductionSchedule.ProductionScheduleList, _oProductionSchedule.CapitalResources, "Month"); // last paramerter indicates initially Grid Style in Month View.
            ViewBag.ProductionSchedules = _oProductionSchedule.ProductionScheduleList;
            ViewBag.CapitalResources = oCapitalResources;
            ViewBag.Locations = oLocations;
            ViewBag.BUID = buid;
            return View(_oProductionSchedule);
        }
        [HttpPost]
        public JsonResult GetPS(ProductionSchedule oProductionSchedule)
        {
             string sSQL="";
             _oProductionSchedule = new ProductionSchedule();
            _oProductionSchedule.ProductionScheduleDetails=new List<ProductionScheduleDetail>();
            if(oProductionSchedule.ProductionScheduleID>0)
            {
              _oProductionSchedule = _oProductionSchedule.Get(oProductionSchedule.ProductionScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oProductionSchedule.ProductionScheduleDetails = ProductionScheduleDetail.Gets(oProductionSchedule.ProductionScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else{
                 List<ProductionSchedule> oPSs = new List<ProductionSchedule>();
                  sSQL = "Select * from View_ProductionSchedule Where MachineID="+ oProductionSchedule.MachineID +" and "+" EndTime = (Select MAX(EndTime) from ProductionSchedule Where MachineID=" + oProductionSchedule.MachineID + ")";
                oPSs = ProductionSchedule.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPSs.Count > 0)
                {
                    _oProductionSchedule.StartTime=oPSs[0].StartTime;
                }
            }
              sSQL = "SELECT * FROM Location where LocationID in(SELECT Distinct LocationID FROM VIEW_CapitalResource where ISNULL(IsActive,1)=1 AND BUID = " + oProductionSchedule.BUID + " AND ResourcesType = " + (int)EnumResourcesType.Machine + " )";
            _oProductionSchedule.LocationList = Location.Gets(_sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oProductionSchedule.CapitalResources = CapitalResource.GetsByBUandResourceTypeWithName(oProductionSchedule.BUID, (int)EnumResourcesType.Machine, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oProductionSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

      
        [HttpPost]
        public JsonResult GetsDyeingOrder(ProductionScheduleDetail oPSDetail)// For Delivery Order
        {
            string sSQL = "";
            List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            List<DUClaimOrderDetail> oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
            List<RouteSheetDO> oRouteSheetDO = new List<RouteSheetDO>();
            ProductionScheduleDetail oPSD = new ProductionScheduleDetail();
            try
            {
                if (oPSDetail.OrderType == (int)EnumOrderType.ClaimOrder)
                {
                    sSQL = "Select * from View_DUClaimOrderDetail";
                    string sReturn = "";
                    if (!String.IsNullOrEmpty(oPSDetail.OrderNo))
                    {
                        oPSDetail.OrderNo = oPSDetail.OrderNo.Trim();
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "ClaimOrderNo Like'%" + oPSDetail.OrderNo + "'";
                    }
                    //if (oPSDetail.DyeingOrderDetailID > 0)
                    //{
                    //    Global.TagSQL(ref sReturn);
                    //    sReturn = sReturn + "DyeingOrderDetailID=" + oPSDetail.DyeingOrderDetailID + "";
                    //}
                    if (oPSDetail.LabDipDetailID > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "LabDipDetailID=" + oPSDetail.LabDipDetailID + "";
                    }

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "(Qty+30)>Qty_RS";


                    sSQL = sSQL + "" + sReturn;

                    oDUClaimOrderDetails = DUClaimOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                   
                    foreach (DUClaimOrderDetail oItem in oDUClaimOrderDetails)
                    {
                        oPSD = new ProductionScheduleDetail();
                        oPSD.DODID = oItem.DyeingOrderDetailID;
                        oPSD.DUClaimOrderDetailID = oItem.DUClaimOrderDetailID;
                        oPSD.OrderNo = oItem.ClaimOrderNo;
                        oPSD.OrderType = (int)EnumOrderType.ClaimOrder;
                        oPSD.OrderNo = oItem.OrderNo;
                        oPSD.ColorName = oItem.ColorName;
                        oPSD.ColorNo = oItem.ColorNo;
                        oPSD.ProductID = oItem.ProductID;
                        oPSD.ProductName = oItem.ProductName;
                        //oPSD.DEOScheduleQty = oItem.Qty - oItem.Qty_RS;
                        //if (oPSD.Qty > 0)
                        //{
                            oProductionScheduleDetails.Add(oPSD);
                        //}
                    }
                }
                else
                {
                    sSQL = "Select * from View_DyeingOrderDetailWaitingForRS ";
                    string sReturn = "";
                    if (!String.IsNullOrEmpty(oPSDetail.OrderNo))
                    {
                        oPSDetail.OrderNo = oPSDetail.OrderNo.Trim();
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "OrderNo Like'%" + oPSDetail.OrderNo + "'";
                    }
                    if (oPSDetail.OrderType > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "OrderType=" + oPSDetail.OrderType + "";
                    }
                    if (oPSDetail.LabDipDetailID > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "LabDipDetailID=" + oPSDetail.LabDipDetailID + "";
                    }

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "(OrderQty+30)>Qty_Pro";

                    sSQL = sSQL + "" + sReturn;
                    oRouteSheetDO = RouteSheetDO.GetsDOYetTORS(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach(RouteSheetDO oItem in oRouteSheetDO)
                    {
                        oPSD = new ProductionScheduleDetail();
                        oPSD.DODID = oItem.DyeingOrderDetailID;
                       // oPSD.DUClaimOrderDetailID = oItem.DUClaimOrderDetailID;
                        //oPSD.OrderNo = oItem.ClaimOrderNo;
                        oPSD.OrderType = (int)EnumOrderType.ClaimOrder;
                        oPSD.OrderNo = oItem.OrderNo;
                        oPSD.ColorName = oItem.ColorName;
                        oPSD.ColorNo = oItem.ColorNo;
                        oPSD.ProductID = oItem.ProductID;
                        oPSD.ProductName = oItem.ProductName;
                        //oPSD.DEOScheduleQty = oItem.Qty - oItem.Qty_RS;
                        //if (oPSD.Qty > 0)
                        //{
                            oProductionScheduleDetails.Add(oPSD);
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                oProductionScheduleDetails = new List<ProductionScheduleDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionScheduleDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
