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

namespace ESimSolFinancial.Controllers
{
	public class JobController : Controller
	{
		#region Declaration

		Job _oJob = new Job();
		List<Job> _oJobs = new  List<Job>();
        List<TAPDetail> _oTAPDetails = new List<TAPDetail>();
		#endregion

		#region Functions
        private List<TAPDetail> ManageSequence(List<TAPDetail> oTAPDetails)
        {
            List<TAPDetail> oNewTAPDetails = new List<TAPDetail>();
            List<TAPDetail> oParentStepList = new List<TAPDetail>();
            oParentStepList = oTAPDetails.Where(p => p.OrderStepParentID == 1).OrderBy(x => x.Sequence).ToList();//find parent steps
            foreach (TAPDetail oItem in oParentStepList)
            {
                oNewTAPDetails.Add(oItem);
                _oTAPDetails = new List<TAPDetail>();
                _oTAPDetails = oTAPDetails.Where(x => x.OrderStepParentID == oItem.OrderStepID).OrderBy(x => x.Sequence).ToList();//find list for a single parent
                int nChildSteps = _oTAPDetails.Count;
                foreach (TAPDetail oChildItem in _oTAPDetails)
                {
                    oNewTAPDetails.Add(oChildItem);
                }
            }

            return oNewTAPDetails;
        }

     
		#endregion

		#region Actions

		public ActionResult ViewJobList(int menuid, int buid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Job).ToString() + "," + ((int)EnumModuleName.TAP).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oJobs = new List<Job>(); 
			_oJobs = Job.Gets(buid, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BusinessSessions = BusinessSession.Gets((int)Session[SessionInfo.currentUserID]);  
			return View(_oJobs);
		}

        public ActionResult ViewJob(int id, int buid)
		{
			_oJob = new Job();
			if (id > 0)
			{
				_oJob = _oJob.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oJob.JobDetails = JobDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
			}
            _oJob.BUID = buid;
			return View(_oJob);
		}
        public ActionResult ViewTAPCreation(int id)
		{
			_oJob = new Job();
			if (id > 0)
			{
				_oJob = _oJob.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oJob.JobDetails = JobDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                string sSQL = "SELECT * FROM View_TAPDetail WHERE TAPID = (SELECT top 1 TAPID FROM TAP WHERE OrderRecapID = "+_oJob.JobDetails[0].OrderRecapID+")";
                _oJob.TAPDetails = TAPDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

			}
			return View(_oJob);
		}

		[HttpPost]
		public JsonResult Save(Job oJob)
		{
			_oJob = new Job();
			try
			{
				_oJob = oJob;
				_oJob = _oJob.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oJob = new Job();
				_oJob.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oJob);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult Approve(Job oJob)
        {
            _oJob = new Job();
            try
            {
                _oJob = oJob;
                _oJob = _oJob.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oJob = new Job();
                _oJob.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oJob);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UndoApprove(Job oJob)
        {
            _oJob = new Job();
            try
            {
                _oJob = oJob;
                _oJob = _oJob.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oJob = new Job();
                _oJob.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oJob);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

		[HttpPost]
		public JsonResult Delete(Job oJB)
		{
			string sFeedBackMessage = "";
			try
			{
				Job oJob = new Job();
                sFeedBackMessage = oJob.Delete(oJB.JobID, (int)Session[SessionInfo.currentUserID]);
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

        #region PrintList
        public ActionResult PrintList(string sIDs)
        {
            _oJob = new Job();
            string sSQL = "SELECT * FROM View_Job WHERE JobID IN (" + sIDs + ") ORDER BY JobID  ASC";
            _oJobs = Job.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptJobList oReport = new rptJobList();
            byte[] abytes = oReport.PrepareReport(_oJobs, oCompany);
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

        public ActionResult PrintJobPreview(int id)
        {
            _oJob = new Job();
            _oJob = _oJob.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oJob.JobDetails = JobDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);


            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptJob oReport = new rptJob();
            byte[] abytes = oReport.PrepareReport(_oJob, oCompany);
            return File(abytes, "application/pdf");
        }

        #endregion

        #region Advance Search

        [HttpPost]
        public JsonResult AdvSearch(Job oJob)
        {
            List<Job> oJobs = new List<Job>();
            try
            {
                string sSQL = GetSQL(oJob.ErrorMessage);
                oJobs = Job.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oJob.ErrorMessage = ex.Message;
                oJobs.Add(oJob);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oJobs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();

            int nCount = 0;
            string sTechnicalSheetIDs = Convert.ToString(sTemp.Split('~')[nCount++]);
            int nDateCriteria_Issue = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dStart_Issue = Convert.ToDateTime(sTemp.Split('~')[nCount++]),
                    dEnd_Issue = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            string sMerchandiserIDs = Convert.ToString(sTemp.Split('~')[nCount++]);
            string sBuyerIDs = Convert.ToString(sTemp.Split('~')[nCount++]);
            int nSessionID = Convert.ToInt32(sTemp.Split('~')[nCount++]);

            
            string sReturn1 = "SELECT * FROM View_Job ";
            string sReturn = "";

            #region Order No
            if (!string.IsNullOrEmpty(sTechnicalSheetIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " TechnicalSheetID IN (" + sTechnicalSheetIDs + ")";
            }
            #endregion

            #region Date Wise
            DateObject.CompareDateQuery(ref sReturn, " IssueDate", nDateCriteria_Issue, dStart_Issue, dEnd_Issue);

            #endregion

            #region MerchandiserID
            if (!string.IsNullOrEmpty(sMerchandiserIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MerchandiserID IN (" + sMerchandiserIDs + ")";
            }
            #endregion

            #region BuyerID
            if (!string.IsNullOrEmpty(sBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region SessionID
            if (nSessionID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BusinessSessionID = " + nSessionID + " ";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }


        #endregion

        #region Search

        #endregion


    }

}
