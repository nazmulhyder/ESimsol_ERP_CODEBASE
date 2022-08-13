using System;
using System.Collections.Generic;
using System.Dynamic;
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
using System.Collections;

namespace ESimSolFinancial.Controllers
{
	public class FNProductionPlanController : Controller
	{
        #region Declaration
        FNProductionPlan _oFNProductionPlan = new FNProductionPlan();
        List<FNProductionPlan> _oFNProductionPlans = new List<FNProductionPlan>();
        #endregion

        #region Functions
        #endregion

        #region Actions
        public ActionResult ViewFNProductionPlans(int TreatmentProcess, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFNProductionPlans = new List<FNProductionPlan>();
            _oFNProductionPlans = FNProductionPlan.Gets("SELECT * FROM View_FNProductionPlan WHERE BUID = "+buid+" AND  CONVERT(DATE,PlanDate)='" + DateTime.Now.ToString("dd MMM yyyy") + "'AND FNTreatment=" + TreatmentProcess + " ORDER BY FNPPID DESC ", (int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.FNTreatments = EnumObject.jGets(typeof(EnumFNTreatment));
            ViewBag.TreatmentProcess = TreatmentProcess;
            ViewBag.BUID = buid;
            return View(_oFNProductionPlans);
        }

        public ActionResult ViewFNProductionPlansV2(int TreatmentProcess, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFNProductionPlans = new List<FNProductionPlan>();
            _oFNProductionPlans = FNProductionPlan.Gets("SELECT * FROM View_FNProductionPlan WHERE BUID = " + buid + " AND FNTreatment=" + TreatmentProcess + " ORDER BY FNPPID DESC ", (int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.FNTreatments = EnumObject.jGets(typeof(EnumFNTreatment));
            ViewBag.PlanStatusList= EnumObject.jGets(typeof(EnumFabricPlanStatus));
            ViewBag.TreatmentProcess = TreatmentProcess;
            ViewBag.BUID = buid;
            return View(_oFNProductionPlans);
        }

        #region Check Number of Schedules In Given Time Period.
        [HttpPost]
        public JsonResult ScheduleInTimePeriod(FNProductionPlan oFNProductionPlan, double nts)
        {

            int nScheduleCount = NumberofScheduleInTimePeriod(oFNProductionPlan);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(nScheduleCount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public int NumberofScheduleInTimePeriod(FNProductionPlan oFNProductionPlan)
        {
            int nScheduleCount = 0;
            DateTime dStartTime = oFNProductionPlan.StartTime;
            DateTime dEndTime = oFNProductionPlan.EndTime.AddMinutes(-1);
            //int nLocationID = oFNProductionPlan.LocationID;
            int nMachineID = oFNProductionPlan.FNMachineID;
            string sSQL = "Select * from View_FNProductionPlan where EndTime >= '" + dStartTime.ToString("dd MMM yyyy HH:mm") + "' and StartTime <='" + dEndTime.ToString("dd MMM yyyy HH:mm") + "'  and FNMachineID=" + nMachineID + " and FNPPID NOT IN (" + oFNProductionPlan.FNPPID + ")";
            if (nMachineID > 0)
            {
                List<FNProductionPlan> oPS = new List<FNProductionPlan>();
                oPS = FNProductionPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oPS.Count() > 0)
                {
                    if (oFNProductionPlan.StartTime <= oPS.Min(x => x.EndTime))
                    {
                        nScheduleCount = oPS.Count();
                    }
                }

            }
            return nScheduleCount;
        }
        #endregion

        [HttpPost]
        public JsonResult Save(FNProductionPlan oFNProductionPlan)
        {
            _oFNProductionPlan = new FNProductionPlan();
            try
            {
                _oFNProductionPlan = oFNProductionPlan.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNProductionPlan = new FNProductionPlan();
                _oFNProductionPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNProductionPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Delete(FNProductionPlan oFNProductionPlan)
        {
            string sFeedBackMessage = "";
            try
            {

                sFeedBackMessage = oFNProductionPlan.Delete(oFNProductionPlan.FNPPID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetFNProductionPlans(FNProductionPlan oFNProductionPlan)
        {
            _oFNProductionPlans = new List<FNProductionPlan>();
            try
            {
                string sSQL = "SELECT top(100)* FROM View_FNProductionPlan WHERE FNPPID>0  ";
                if (!string.IsNullOrEmpty(oFNProductionPlan.FNExONo))
                    sSQL += " AND FNExONo LIKE '%" + oFNProductionPlan.FNExONo + "%'";
                if (oFNProductionPlan.BUID>0)
                    sSQL += " AND BUID =" + oFNProductionPlan.BUID;
                if (oFNProductionPlan.FNTreatment > 0)
                    sSQL += " AND  ISNULL(FNTreatment,0) =" + oFNProductionPlan.FNTreatment;
                if (!string.IsNullOrEmpty(oFNProductionPlan.BatchNo))
                    sSQL += " AND FNPPID IN (SELECT FNPPID FROM FNBatch WHERE BatchNo LIKE'%" + oFNProductionPlan.BatchNo + "%')";
                if (!string.IsNullOrEmpty(oFNProductionPlan.ErrorMessage))
                    sSQL += " AND FnMachineID IN (" + oFNProductionPlan.ErrorMessage + ")";
                if (!string.IsNullOrEmpty(oFNProductionPlan.Params))
                {
                    bool chkDate = Convert.ToBoolean(oFNProductionPlan.Params.Split('~')[0]);
                    DateTime startDate = Convert.ToDateTime(oFNProductionPlan.Params.Split('~')[1]);
                    DateTime endDate = Convert.ToDateTime(oFNProductionPlan.Params.Split('~')[2]);
                    if (chkDate)
                    {
                        sSQL += " AND CONVERT(Date,PlanDate) BETWEEN '" + startDate.ToString("dd MMM yyyy") + "' AND '" + endDate.ToString("dd MMM yyyy") + "'";
                    }
                }
                sSQL += " ORDER BY FNPPID DESC";
                _oFNProductionPlans = FNProductionPlan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNProductionPlan = new FNProductionPlan();
                _oFNProductionPlan.ErrorMessage = ex.Message;
                _oFNProductionPlans.Add(_oFNProductionPlan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNProductionPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Search


        public ActionResult AdvanceSearch()
        {
            return PartialView();
        }
        #region HttpGet For Search
        [HttpGet]
        public JsonResult Search(string sTemp)
        {
            List<FNProductionPlan> oFNProductionPlans = new List<FNProductionPlan>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oFNProductionPlans = FNProductionPlan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNProductionPlan = new FNProductionPlan();
                _oFNProductionPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNProductionPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL(string sTemp)
        {
            //Issue Date
            int nStartDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dIssueStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dIssueEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            //EndTime
            int nEndDateCom = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dPPEndStartDate = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dPPEndEndDate = Convert.ToDateTime(sTemp.Split('~')[5]);

            string sPlanNo = sTemp.Split('~')[6];
            string sFNMachineIDs = sTemp.Split('~')[7];
            int nBUID = Convert.ToInt32(sTemp.Split('~')[8]);

            string sReturn1 = "SELECT * FROM View_FNProductionPlan";
            string sReturn = "";

            #region BU
            if (nBUID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID =" + nBUID;
            }
            #endregion


            #region Plan No
            if (sPlanNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PlanNo LIKE '%" + sPlanNo + "%'";

            }
            #endregion


            #region FNMachine wise

            if (sFNMachineIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FNMachineID IN (" + sFNMachineIDs + ")";
            }
            #endregion



            #region start Date Wise
            if (nStartDateCom > 0)
            {
                if (nStartDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),StartTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),StartTime,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),StartTime,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),StartTime,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),StartTime,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),StartTime,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region End Date Wise
            if (nEndDateCom > 0)
            {
                if (nEndDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EndDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPPEndStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nEndDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EndTime,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPPEndStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nEndDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EndTime,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPPEndStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nEndDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EndTime,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPPEndStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nEndDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EndTime,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPPEndStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPPEndEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nEndDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EndTime,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPPEndStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPPEndEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion
        #endregion


        #region Get Order
        [HttpPost]
        public JsonResult GetsFNExODetails(FabricSCReport oFabricSCReport)
        {
            List<FabricSCReport> oFabricSalesContractDetails = new List<FabricSCReport>();
            string sSQL = "";
            try
            {

                sSQL = "SELECT top(200)* FROM View_FabricSalesContractReport WHERE OrderType in (2,3,9) and  ExeNo !=''";
                if (!string.IsNullOrEmpty(oFabricSCReport.SCNoFull))
                {
                    sSQL += " AND SCNoFull LIKE '%" + oFabricSCReport.SCNoFull + "%'";
                }
                if (!string.IsNullOrEmpty(oFabricSCReport.ExeNo))
                {
                    sSQL += " AND ExeNo LIKE '%" + oFabricSCReport.ExeNo + "%'";
                }
                //sSQL += " AND FabricNo !='' AND ExeNoFull !=''";
                oFabricSalesContractDetails = FabricSCReport.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                FabricSCReport oFabricSalesContractDetail = new FabricSCReport();
                oFabricSalesContractDetail.ErrorMessage = ex.Message;
                oFabricSalesContractDetails.Add(oFabricSalesContractDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSalesContractDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetPSByMachine(int nFMID, double nts)
        {
            string sSQL = "Select top(1)* from View_FNProductionPlan Where FMID=" + nFMID + "  Order by StartTime  DESC";

            List<FNProductionPlan> oPSs = new List<FNProductionPlan>();
            FNProductionPlan oPS = new FNProductionPlan();
            string sStartTime = "";
            try
            {
                oPSs = FNProductionPlan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

        [HttpPost]
        public JsonResult GetFNP(FNProductionPlan oFNProductionPlan)
        {
            _oFNProductionPlan = new FNProductionPlan();
            try
            {
                _oFNProductionPlan = oFNProductionPlan.Get(oFNProductionPlan.FNPPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFNProductionPlan = new FNProductionPlan();
                _oFNProductionPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNProductionPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Print-FNProductionPlan
        public ActionResult SetSessionSearchCriterias(FNProductionPlan oFNProductionPlan)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFNProductionPlan);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintFNProductionPlans()
        {
            _oFNProductionPlan = new FNProductionPlan();
            _oFNProductionPlans = new List<FNProductionPlan>();
            _oFNProductionPlan = (FNProductionPlan)Session[SessionInfo.ParamObj];
            try
            {
                string sSQL = "SELECT * FROM View_FNProductionPlan AS HH WHERE HH.FNPPID IN (" + _oFNProductionPlan.ErrorMessage + ") ORDER BY HH.FNPPID ASC";
                _oFNProductionPlans = FNProductionPlan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            catch(Exception ex){
                _oFNProductionPlans = new List<FNProductionPlan>();
                _oFNProductionPlan = new FNProductionPlan();
                _oFNProductionPlan.ErrorMessage = ex.Message;
                _oFNProductionPlans.Add(_oFNProductionPlan);
            }

            if (_oFNProductionPlans.Count > 0) {
                Company oCompany = new Company();
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(_oFNProductionPlan.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(_oFNProductionPlan.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptFNProductionPlans oReport = new rptFNProductionPlans();
                byte[] abytes = oReport.PrepareReport(_oFNProductionPlans,_oFNProductionPlan,oBusinessUnit, oCompany, "");
                return File(abytes, "application/pdf");
            
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }          
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

        [HttpPost]
        public JsonResult AdvSearch(FNProductionPlan oFNProductionPlan)
        {
            _oFNProductionPlans = new List<FNProductionPlan>();
            try
            {
                string sSQL = this.GetSQL(oFNProductionPlan);
                _oFNProductionPlans = FNProductionPlan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNProductionPlans = new List<FNProductionPlan>();
                _oFNProductionPlan = new FNProductionPlan();
                _oFNProductionPlan.ErrorMessage = ex.Message;
                _oFNProductionPlans.Add(_oFNProductionPlan);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNProductionPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(FNProductionPlan oFNProductionPlan)
        {
            string sValue = oFNProductionPlan.ErrorMessage;
            EnumCompareOperator ePlanDate = (EnumCompareOperator)Convert.ToInt32(sValue.Split('~')[0]);
            DateTime dPlanStartDate = Convert.ToDateTime(sValue.Split('~')[1]);
            DateTime dPlanEndDate = Convert.ToDateTime(sValue.Split('~')[2]);

            string sDispoNo = Convert.ToString(sValue.Split('~')[3]);
            string sConstruction = Convert.ToString(sValue.Split('~')[4]);
            string sMachineIDs = Convert.ToString(sValue.Split('~')[5]);
            string sCustomerIDs = Convert.ToString(sValue.Split('~')[6]);

            int nPlanStatus = Convert.ToInt32(sValue.Split('~')[7]);
            bool IsPending = Convert.ToBoolean(sValue.Split('~')[8]);
            string sReturn1 = "SELECT * FROM View_FNProductionPlan AS HH";
            string sReturn = "";

            #region Dispo No
            if (sDispoNo != null && sDispoNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.ExeNo LIKE '%" + sDispoNo + "%'";
            }
            #endregion
            #region Construction
            if (sConstruction != null && sConstruction != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.Construction LIKE '%" + sConstruction + "%'";
            }
            #endregion

            #region Machine Ids
            if (sMachineIDs != null && sMachineIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.FNMachineID IN(" + sMachineIDs + ")";
            }
            #endregion

            #region Customer Ids
            if (sCustomerIDs != null && sCustomerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.ContractorID IN(" + sCustomerIDs + ")";
            }
            #endregion

            #region Plan Status
            if (nPlanStatus != null)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.PlanStatus = " + ((int)nPlanStatus).ToString();
            }
            #endregion

            #region PlanDate
            if (ePlanDate != EnumCompareOperator.None)
            {
                if (ePlanDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.StartTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPlanStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePlanDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.StartTime,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPlanStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePlanDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.StartTime,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPlanStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePlanDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.StartTime,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPlanStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePlanDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.StartTime,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPlanStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPlanEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePlanDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.StartTime,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPlanStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPlanEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY FNPPID ASC";
            return sReturn;
        }

        #endregion



    }

}
