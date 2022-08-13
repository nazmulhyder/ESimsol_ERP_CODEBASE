using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSolFinancial.Controllers;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;

namespace ESimSolFinancial.Controllers
{

    public class TAPController : Controller
    {
        #region Declaration
        TAP _oTAP = new TAP();
        List<TAP> _oTAPs = new List<TAP>();
        TAPDetail _oTAPDetail = new TAPDetail();
        List<TAPDetail> _oTAPDetails = new List<TAPDetail>();
        TechnicalSheetImage _oTechnicalSheetImage = new TechnicalSheetImage();
        List<OrderRecapDetail> _oOrderRecapDetails = new List<OrderRecapDetail>();
        ApprovalRequest _oApprovalRequest = new ApprovalRequest();
        List<JobDetail> _oJobDetails = new List<JobDetail>();
        Job _oJob = new Job();
        #endregion

       

        #region Actions
        public ActionResult ViewTAPs(int menuid,int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TAP).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oTAPs = new List<TAP>();
            //_oTAPs = TAP.Gets((int)Session[SessionInfo.currentUserID]);
            string sSql = "SELECT * FROM View_User WHERE UserID IN (SELECT Distinct  ApprovedBy FROM View_TAP WHERE ISNULL(ApprovedBy,0)!=0)";
            ViewBag.Users = ESimSol.BusinessObjects.User.GetsBySql(sSql, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Brands = Brand.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Merchandisers = Employee.Gets(EnumEmployeeDesignationType.Merchandiser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessSessions = BusinessSession.Gets(true,(int)Session[SessionInfo.currentUserID]);
            
            return View(_oTAPs);
        }
        public ActionResult ViewTAP(int id)
        {
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TAPDetail).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oTAP = new TAP();
            _oTAPDetails = new List<TAPDetail>();
            if (id > 0)
            {
                _oTAP = _oTAP.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oTAPDetails = TAPDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oTAP.TAPApprovalProcesList = _oTAPDetails.Where(x => x.StepType == EnumStepType.Approval).OrderBy(x => x.OrderStepSequence).ThenBy(z => z.Sequence).ToList();
                _oTAP.TAPProductionProcesList = _oTAPDetails.Where(x => x.StepType == EnumStepType.Production).OrderBy(x => x.OrderStepSequence).ThenBy(z => z.Sequence).ToList();
            }
            
           return  View(_oTAP);
        }
        public ActionResult ViewTAPAcceptRevise(int id, double ts)
        {
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TAPDetail).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oTAP = new TAP();
            _oTAPDetails = new List<TAPDetail>();
            if (id > 0)
            {
                _oTAP = _oTAP.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oTAPDetails = TAPDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oTAP.TAPApprovalProcesList = _oTAPDetails.Where(x => x.StepType == EnumStepType.Approval).OrderBy(x => x.OrderStepSequence).ThenBy(z => z.Sequence).ToList();
                _oTAP.TAPProductionProcesList = _oTAPDetails.Where(x => x.StepType == EnumStepType.Production).OrderBy(x => x.OrderStepSequence).ThenBy(z => z.Sequence).ToList();
            }
            return View(_oTAP);
        }

      

        [HttpPost]
        public JsonResult Save(TAP oTAP)
        {
            _oTAP = new TAP();
            try
            {
                _oTAP = oTAP;
                _oTAP.PlanBy = (int)Session[SessionInfo.currentUserID];
                _oTAP = _oTAP.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTAP = new TAP();
                _oTAP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AcceptRevise(TAP oTAP)
        {
            _oTAP = new TAP();
            try
            {
                _oTAP = oTAP;
                _oTAP.PlanBy = (int)Session[SessionInfo.currentUserID];
                _oTAP.PlanDate = DateTime.Today;
                _oTAP = _oTAP.AcceptRevise((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oTAP = new TAP();
                _oTAP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                TAP oTAP = new TAP();
                sFeedBackMessage = oTAP.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        #region New Functions 
        [HttpPost]
        public JsonResult StyleWiseOrderRecaps(TAP oTAP)
        {
            List<OrderRecap> oOrderRecaps = new List<OrderRecap>();
            string sSQL = "SELECT * FROM View_OrderRecap  WHERE TechnicalSheetID = " + oTAP.TechnicalSheetID + " AND ISNULL(ApproveBy,0)!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + DateTime.Today.ToString("dd MMM yyyy") + "',106)) AND OrderRecapID NOT IN (SELECT OrderRecapID FROM TAP) AND ISNULL(IsActive,0)= 1";

            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                sSQL += " AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
            }
            #endregion

            #region Avoid Hidden Style
            sSQL += " AND TechnicalSheetID NOT IN (SELECT TT.TechnicalSheetID FROM UserWiseStyleConfigure AS TT  WHERE TT.UserID=" + ((int)Session[SessionInfo.currentUserID]).ToString() + ")";
            #endregion

            if(oTAP.OrderRecapID>0)
            {
                sSQL += " OR OrderRecapID = " + oTAP.OrderRecapID;
            }
            oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

         [HttpPost]
        public JsonResult GetProcesList(TAP oTAP)
        {
            List<TAPDetail> oApprovalList = new List<TAPDetail>();
            List<TAPDetail> oProductionList = new List<TAPDetail>();
            List<OrderStep> oOrderSteps = new List<OrderStep>();
            List<OrderStep> oApprovalSteps = new List<OrderStep>();
            List<OrderStep> oProductionSteps = new List<OrderStep>();
            List<TechnicalSheetColor> oTechnicalSheetColors = new List<TechnicalSheetColor>();
             //get All step of Knit or Woven
            oOrderSteps = OrderStep.Gets((int)oTAP.TSType, (int)Session[SessionInfo.currentUserID]);
            oApprovalSteps = oOrderSteps.Where(x => x.StepType == EnumStepType.Approval).OrderBy(x => x.Sequence).ToList();
            oProductionSteps = oOrderSteps.Where(x => x.StepType == EnumStepType.Production).OrderBy(x => x.Sequence).ToList();
            oTechnicalSheetColors = TechnicalSheetColor.Gets(oTAP.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            foreach (OrderStep oItem in oApprovalSteps)
            {
                if (oItem.TnAStep == EnumTnAStep.Labdips)
                {
                    int nSequence = 0;
                    foreach(TechnicalSheetColor oColor in oTechnicalSheetColors)
                    {
                        nSequence++;
                        _oTAPDetail = new TAPDetail();
                        _oTAPDetail.OrderStepID = oItem.OrderStepID;
                        _oTAPDetail.OrderStepName = oItem.OrderStepName;
                        _oTAPDetail.TnAStep = oItem.TnAStep;
                        _oTAPDetail.TnAStepInt =(int)oItem.TnAStep;
                        _oTAPDetail.SubStepName = oColor.ColorName;
                        _oTAPDetail.Sequence = nSequence;//multiple
                        _oTAPDetail.OrderStepSequence = oItem.Sequence;
                        oApprovalList.Add(_oTAPDetail);
                    }
                }
                else
                {
                    _oTAPDetail = new TAPDetail();
                    _oTAPDetail.OrderStepID = oItem.OrderStepID;
                    _oTAPDetail.OrderStepName = oItem.OrderStepName;
                    _oTAPDetail.SubStepName = oItem.SubStepName;
                    _oTAPDetail.TnAStep = oItem.TnAStep;
                    _oTAPDetail.TnAStepInt = (int)oItem.TnAStep;
                    _oTAPDetail.Sequence = 1;//default 1, because only one
                    _oTAPDetail.OrderStepSequence = oItem.Sequence;
                    oApprovalList.Add(_oTAPDetail);
                }
            }
            foreach (OrderStep oItem in oProductionSteps)
            {
                _oTAPDetail = new TAPDetail();
                _oTAPDetail.OrderStepID = oItem.OrderStepID;
                _oTAPDetail.OrderStepName = oItem.OrderStepName;
                _oTAPDetail.SubStepName = oItem.SubStepName;
                _oTAPDetail.TnAStep = oItem.TnAStep;
                _oTAPDetail.TnAStepInt = (int)oItem.TnAStep;
                _oTAPDetail.Sequence = 1;//default 1, because only one
                _oTAPDetail.OrderStepSequence = oItem.Sequence;
                if (_oTAPDetail.TnAStepInt >= (int)EnumTnAStep.PossibleInputDate && _oTAPDetail.TnAStepInt <= (int)EnumTnAStep.FinalInspection)
                {
                    _oTAPDetail.SubmissionDate = DateTime.MinValue;//default set
                }
                oProductionList.Add(_oTAPDetail);
            }
            oTAP.TAPApprovalProcesList = oApprovalList;
            oTAP.TAPProductionProcesList = oProductionList;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTAP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


         [HttpPost]
         public JsonResult GetRearagngeList(TAP oTAP)
         {
             List<TAPDetail> oTAPDetails = new List<TAPDetail>();
             List<TAPDetail> oNewTAPDetails = new List<TAPDetail>();
             oTAPDetails = oTAP.TAPDetails;
             oNewTAPDetails = oTAPDetails.OrderBy(x => x.OrderStepSequence).ThenBy(x => x.Sequence).ToList();
             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(oNewTAPDetails);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }
        #endregion




        #endregion




        #region Job wise TAP Actions
        [HttpPost]
        public JsonResult JobWiseTAPSave(TAP oTAP)
        {
            _oJob = new Job();
            _oJobDetails = new List<JobDetail>();
            _oTAPs = new List<TAP>();
            _oTAPDetails = new List<TAPDetail>();
            List<TAPDetail> oCommonTAPDetails = new List<TAPDetail>();
            List<TAPDetail> oTempTAPDetails = new List<TAPDetail>();
            string sSQL = "";
            try
            {
                _oJob.JobID = oTAP.JobID;
               oCommonTAPDetails = oTAP.TAPDetails;
               oCommonTAPDetails.All(x => { x.TAPDetailID = 0; return true; });
             _oJobDetails = JobDetail.Gets(oTAP.JobID, (int)Session[SessionInfo.currentUserID]);
             sSQL = "SELECT * FROM View_TAP WHERE OrderRecapID IN (SELECT OrderRecapID FROM JobDetail WHERE JobID = "+oTAP.JobID+")";
             _oTAPs = TAP.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
             sSQL = "SELECT * FROM View_TAPDetail  WHERE TAPID IN (SELECT TAPID FROM TAP   WHERE OrderRecapID IN (SELECT OrderRecapID FROM JobDetail WHERE JobID = "+oTAP.JobID+"))";
            _oTAPDetails = TAPDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            foreach(JobDetail oItem in _oJobDetails)
            {
                _oTAP = new TAP();
                oTempTAPDetails = new List<TAPDetail>();
                _oTAP = _oTAPs.Where(x => x.OrderRecapID == oItem.OrderRecapID).FirstOrDefault();
                if(_oTAP==null)
                {
                    _oTAP = new TAP();
                }
                _oTAP.PlanBy = (int)Session[SessionInfo.currentUserID];
                _oTAP.PlanDate = DateTime.Today;
                _oTAP.ShipmentDate = oItem.ShipmentDate;
                _oTAP.TampleteName = oTAP.TampleteName;
                _oTAP.Remarks = oTAP.Remarks;

                _oTAP.BUID = oTAP.BUID;
                _oTAP.OrderRecapID = oItem.OrderRecapID;
                //oTempTAPDetails = _oTAPDetails.Where(x => x.TAPID == _oTAP.TAPID).ToList();
                if(_oTAP.TAPID>0)
                {
                    foreach(TAPDetail oDetailItem in oCommonTAPDetails)
                    {
                        _oTAPDetail = new TAPDetail();
                        _oTAPDetail = oDetailItem;
                        _oTAPDetail.TAPID = _oTAP.TAPID;
                        TAPDetail oNewTAPDetail = new TAPDetail();
                        oNewTAPDetail = _oTAPDetails.Where(x => x.TAPID == _oTAP.TAPID & x.OrderStepID == oDetailItem.OrderStepID).FirstOrDefault();
                        _oTAPDetail.TAPDetailID = oNewTAPDetail!=null?oNewTAPDetail.TAPDetailID:0;
                        oTempTAPDetails.Add(_oTAPDetail);
                    }
                }
                else
                {
                    oTempTAPDetails = oCommonTAPDetails;
                }
                _oTAP.TAPDetails = oTempTAPDetails;
                _oTAP = _oTAP.Save((int)Session[SessionInfo.currentUserID]);
            }

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

        [HttpGet]
        public JsonResult DeleteJobWise(int id)
        {
            string sFeedBackMessage = "";
            try
            {
               string  sSQL = "SELECT * FROM View_TAP WHERE OrderRecapID IN (SELECT OrderRecapID FROM JobDetail WHERE JobID = " + id + ")";
                _oTAPs = TAP.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (TAP oItem in _oTAPs)
                {
                    sFeedBackMessage = oItem.Delete(oItem.TAPID, (int)Session[SessionInfo.currentUserID]);
                }
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

        public ActionResult PrintTAPs(string sParam)
        {
            _oTAP = new TAP();
            string sSQL = "SELECT * FROM View_TAP WHERE TAPID IN (" + sParam + ")";
            _oTAP.TAPs = TAP.Gets((int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oTAP.Company = oCompany;

            string Messge = "Time Action Plan List";
            rptTAPs oReport = new rptTAPs();
            byte[] abytes = oReport.PrepareReport(_oTAP, Messge);
            return File(abytes, "application/pdf");

        }

        public ActionResult TAPPreview(int id)
        {
            _oTAP = new TAP();
            Company oCompany = new Company();
            _oTAPDetails = new List<TAPDetail>();
            _oTAP = _oTAP.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oTAPDetails = TAPDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oTAP.TAPApprovalProcesList = _oTAPDetails.Where(x => x.StepType == EnumStepType.Approval).OrderBy(x => x.OrderStepSequence).ThenBy(z => z.Sequence).ToList();
            _oTAP.TAPProductionProcesList = _oTAPDetails.Where(x => x.StepType == EnumStepType.Production).OrderBy(x => x.OrderStepSequence).ThenBy(z => z.Sequence).ToList();
            _oTAP.TechnicalSheetImage = _oTechnicalSheetImage.GetFrontImage(_oTAP.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oTAP.Company = oCompany;
            rptTAP oReport = new rptTAP();
            byte[] abytes = oReport.PrepareReport(_oTAP);
            return File(abytes, "application/pdf");
        }
        public ActionResult OrderFollowUpPreview(int id)
        {
            _oTAP = new TAP();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            Company oCompany = new Company();
            List<DevelopmentRecapSizeColorRatio> oDevelopmentRecapSizeColorRatios = new List<DevelopmentRecapSizeColorRatio>();
            _oTAP = _oTAP.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oTAP.TAPDetails = TAPDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oTAP.BusinessUnit = oBusinessUnit.Get(_oTAP.BUID, (int)Session[SessionInfo.currentUserID]);
                       
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oTAP.Company = oCompany;
            rptTAPOrderFollowUp oReport = new rptTAPOrderFollowUp();
            byte[] abytes = oReport.PrepareReport(_oTAP);
            return File(abytes, "application/pdf");
        }

        public ActionResult OrderFollowUpPreviewJobWise(int id)//id : JobID
        {
            _oJob = new Job();
            _oTAP = new TAP();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            Company oCompany = new Company();
            List<DevelopmentRecapSizeColorRatio> oDevelopmentRecapSizeColorRatios = new List<DevelopmentRecapSizeColorRatio>();
            _oJob = _oJob.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oJobDetails = JobDetail.Gets(_oJob.JobID, (int)Session[SessionInfo.currentUserID]);
           string sSQL = "SELECT * FROM View_TAP WHERE OrderRecapID IN (SELECT OrderRecapID FROM JobDetail WHERE JobID = " + _oJob.JobID + ")";
            _oTAPs = TAP.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            _oTAP = _oTAPs[0];//_oTAP.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oTAP.TAPDetails = TAPDetail.Gets(_oTAP.TAPID, (int)Session[SessionInfo.currentUserID]);
            _oTAP.BusinessUnit = oBusinessUnit.Get(_oTAP.BUID, (int)Session[SessionInfo.currentUserID]);

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oTAP.Company = oCompany;
            rptTAPOrderFollowUpJobWise oReport = new rptTAPOrderFollowUpJobWise();
            byte[] abytes = oReport.PrepareReport(_oTAP, _oJobDetails);
            return File(abytes, "application/pdf");
        }

        public ActionResult PendingTask(int id)
        {
            _oTAP = new TAP();
            Company oCompany = new Company();
            List<DevelopmentRecapSizeColorRatio> oDevelopmentRecapSizeColorRatios = new List<DevelopmentRecapSizeColorRatio>();
            _oTAP = _oTAP.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oTAP.TAPDetails = TAPDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
          
            _oTAP.TechnicalSheetImage = _oTechnicalSheetImage.GetFrontImage(_oTAP.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
           
                _oTAP.OrderRecapDetails = OrderRecapDetail.Gets(_oTAP.OrderRecapID, (int)Session[SessionInfo.currentUserID]);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oTAP.Company = oCompany;

            rptTAPPendingTask oReport = new rptTAPPendingTask();
            byte[] abytes = oReport.PrepareReport(_oTAP,true);
            return File(abytes, "application/pdf");
        }


        #region Waiting Search
        [HttpGet]
        public JsonResult WaitingSearch()
        {
            _oTAPs = new List<TAP>();
            string sSQL = "SELECT * FROM View_TAP WHERE TAPStatus = " + (int)EnumTAPStatus.Initialize;
            try
            {
                _oTAPs = TAP.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTAP = new TAP();
                _oTAP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAPs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        #region Advance Search
       
        #region HttpGet For Search
        [HttpGet]
        public JsonResult Gets(string sTemp)
        {
            List<TAP> oTAPs = new List<TAP>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oTAPs = TAP.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTAP = new TAP();
                _oTAP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTAPs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            int nCboPlanDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dIssueStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dSendingEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            string sPlanNo = sTemp.Split('~')[3];
            int nTechnicalSheetID = Convert.ToInt32(sTemp.Split('~')[4]);
            int nApproveByID = Convert.ToInt32(sTemp.Split('~')[5]);
            string sBuyerIDs = sTemp.Split('~')[6];
            int nOrderRecapID = Convert.ToInt32(sTemp.Split('~')[7]);
            int nBrandID = Convert.ToInt32(sTemp.Split('~')[8]);
            string sFactoryIDs = sTemp.Split('~')[9];
            int nMerchandiserID = Convert.ToInt32(sTemp.Split('~')[10]);
            int nSessionID = Convert.ToInt32(sTemp.Split('~')[11]);
            bool bIsRunningShipmentWise = Convert.ToBoolean(sTemp.Split('~')[12]);
            bool bIsOnlyforApprove = Convert.ToBoolean(sTemp.Split('~')[13]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[14]);

            string sReturn1 = "SELECT * FROM View_TAP";
            string sReturn = "";



            #region REf No

            if (sPlanNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PlanNo ='" + sPlanNo + "'";
            }
            #endregion

            #region nOrderRecapID

            if (nOrderRecapID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderRecapID =" + nOrderRecapID;
            }
            #endregion

            #region Buyer Name
            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region Brand
            if (nBrandID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BrandID = " +nBrandID ;
            }
            #endregion

            #region BU
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = "+ nBUID;
            }
            #endregion

            #region Merchandiser
            if (nMerchandiserID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MerchandiserID = " + nMerchandiserID;
            }
            #endregion

            #region Session
            if (nSessionID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderRecapID IN (SELECt OrderRecapID FROm OrderRecap  WHERE BusinessSessionID  = " + nSessionID+")";
            }
            #endregion

            #region Factory Name
            if (sFactoryIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductionFactoryID IN (" + sFactoryIDs + ")";
            }
            #endregion

            #region Style Wise
            if (nTechnicalSheetID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " TechnicalSheetID  =" + nTechnicalSheetID;
            }
            #endregion

            #region Approve By
            if (nApproveByID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ApprovedBy  =" + nApproveByID;
            }
            #endregion

            #region Is running
            if (bIsRunningShipmentWise == true)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " IsShippedOut=0";
            }
            #endregion

            #region Date Wise
            #region Sending Date
            if (nCboPlanDate > 0)
            {
                if (nCboPlanDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate = '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboPlanDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate != '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboPlanDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate > '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboPlanDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate < '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboPlanDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate>= '" + dIssueStartDate.ToString("dd MMM yyyy") + "' AND PlanDate < '" + dSendingEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCboPlanDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate< '" + dIssueStartDate.ToString("dd MMM yyyy") + "' OR PlanDate > '" + dSendingEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion


            #endregion

            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " BuyerID IN (SELECT ContractorID FROM UserWiseContractorConfigure WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ")";
            }
            #endregion

            //#region Active
            //Global.TagSQL(ref sReturn);
            //sReturn = sReturn + " ISNULL(IsActive,0)= 1";
            //#endregion

            if (bIsOnlyforApprove)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(TAPStatus,0)=2 AND ISNULL(ApprovedBy,0)!=0";
            }

            sReturn = sReturn1 + sReturn + " ORDER BY TAPID";
            return sReturn;
        }
        #endregion

        #endregion

        #region View PI Approval Request
        public ActionResult ViewTAPApprovalRequest()
        {
            _oApprovalRequest = new ApprovalRequest();
            string sSql = "SELECT * FROM View_User WHERE EmployeeID IN (SELECT EmployeeID FROM Employee WHERE EmployeeDesignationType IN (" + ((int)EnumEmployeeDesignationType.Management).ToString() + "," + ((int)EnumEmployeeDesignationType.Merchandiser).ToString() + "))";
            _oApprovalRequest.UserList = ESimSol.BusinessObjects.User.GetsBySql(sSql, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oApprovalRequest);
        }

        #endregion

        #region HTTP ChangeStatus
        [HttpPost]
        public JsonResult ChangeStatus(TAP oTAP)
        {
            _oTAP = new TAP();
            _oTAP = oTAP;
            _oJob = new Job();
            _oJob.JobID = oTAP.JobID;//don't Remove use in Job Moduele for Multiple TAP Approved
            try
            {
                
                if (oTAP.ActionTypeExtra == "Approve")
                {
                    _oTAP.TAPActionType = EnumTAPActionType.Approve;
                }
                else if (oTAP.ActionTypeExtra == "UndoApprove")
                {
                    _oTAP.TAPActionType = EnumTAPActionType.UndoApprove;
                }
                else if (oTAP.ActionTypeExtra == "RequestForRevise")
                {
                    _oTAP.TAPActionType = EnumTAPActionType.RequestForRevise;
                }
                _oTAP.Remarks = oTAP.Remarks;
                oTAP = SetTAPStatus(_oTAP);
            
                if (oTAP.ActionTypeExtra == "RequestForApproved") // for SEt Approval Request Value
                {
                    oTAP.ApprovalRequest.RequestBy = ((User)Session[SessionInfo.CurrentUser]).UserID;
                    oTAP.ApprovalRequest.OperationType = EnumApprovalRequestOperationType.TAP;
                }
                else
                {
                    oTAP.ApprovalRequest = new ApprovalRequest();
                }
                if (_oJob.JobID != 0)
                {
                    _oJobDetails = new List<JobDetail>();
                    _oJobDetails = JobDetail.Gets(oTAP.JobID, (int)Session[SessionInfo.currentUserID]);
                    string sSQL = "SELECT * FROM View_TAP WHERE OrderRecapID IN (SELECT OrderRecapID FROM JobDetail WHERE JobID = " + _oJob.JobID + ")";
                    _oTAPs = TAP.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    foreach(TAP oItem in _oTAPs)
                    {

                        oItem.TAPStatus = oTAP.TAPStatus;
                        oItem.TAPActionType = oTAP.TAPActionType;
                        oItem.ActionTypeExtra = oTAP.ActionTypeExtra;
                        oItem.ApprovalRequest = new ApprovalRequest();
                        oItem.Remarks = oTAP.Remarks;
                        _oTAP = oItem.ChangeStatus(oItem.ApprovalRequest, (int)Session[SessionInfo.currentUserID]);
                    }
                }
                else { 
                    //for single TAP Approved
                    _oTAP = oTAP.ChangeStatus(oTAP.ApprovalRequest, (int)Session[SessionInfo.currentUserID]);
                }
                if (_oJob.JobID != 0)//only for job , don;t Remove
                {
                    _oJob = _oJob.Get(_oJob.JobID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oTAP = new TAP();
                _oTAP.ErrorMessage = ex.Message;
            }
            if (_oJob.JobID != 0)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string sjson = serializer.Serialize(_oJob);
                return Json(sjson, JsonRequestBehavior.AllowGet);
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string sjson = serializer.Serialize(_oTAP);
                return Json(sjson, JsonRequestBehavior.AllowGet);
            }
        }

        #region Set Status
        private TAP SetTAPStatus(TAP oTAP)//Set EnumOrderStatus Value
        {
            switch (oTAP.TAPActionType)
            {
                case EnumTAPActionType.Approve:
                    {
                        oTAP.TAPStatus = EnumTAPStatus.Approved;
                        break;
                    }
                case EnumTAPActionType.UndoApprove:
                    {
                        oTAP.TAPStatus = EnumTAPStatus.Initialize;
                        break;
                    }
                case EnumTAPActionType.RequestForRevise:
                    {
                        oTAP.TAPStatus = EnumTAPStatus.Request_for_Revise;
                        break;
                    }
            }

            return oTAP;
        }
        #endregion
        #endregion

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

        #region Search Style OR Buyer by Press Enter
        [HttpGet]
        public JsonResult SearchStyleAndBuyer(string sTempData, bool bIsStyle, int buid, double ts)
        {
            _oTAPs = new List<TAP>();
            string sSQL = "";
            if (bIsStyle == true)
            {
                sSQL = "SELECT * FROM View_TAP WHERE StyleNo LIKE ('%" + sTempData + "%')";
            }
            else
            {
                sSQL = "SELECT * FROM View_TAP WHERE BuyerName LIKE ('%" + sTempData + "%')";
            }
            sSQL += " AND BUID = "+buid;
            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {

                sSQL += " AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
            }
            #endregion
            try
            {
                TAP oTAP = new TAP();
                _oTAPs = TAP.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTAP = new TAP();
                _oTAP.ErrorMessage = ex.Message;
                _oTAPs.Add(_oTAP);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAPs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }

}