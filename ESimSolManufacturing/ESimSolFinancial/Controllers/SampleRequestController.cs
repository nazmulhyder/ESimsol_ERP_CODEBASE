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
    public class SampleRequestController : Controller
    {
        #region Declaration

        SampleRequest _oSampleRequest = new SampleRequest();
        List<SampleRequest> _oSampleRequests = new List<SampleRequest>();
        SampleRequestDetail _oSampleRequestDetail = new SampleRequestDetail();
        List<SampleRequestDetail> _oSampleRequestDetails = new List<SampleRequestDetail>();
        #endregion

        #region Functions
        #endregion

        #region Actions

        public ActionResult ViewSampleRequestList(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.SampleRequest).ToString() + "," + ((int)EnumModuleName.TAP).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oSampleRequests = new List<SampleRequest>();
           string sSQL = "SELECT * FROM View_SampleRequest AS HH WHERE ISNULL(HH.DisbursedBy,0)=0 AND HH.BUID = "+buid.ToString()+ " Order BY SampleRequestID ASC";
            _oSampleRequests = SampleRequest.Gets( sSQL,(int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApprovedBy FROM SampleRequest AS MM WHERE ISNULL(MM.ApprovedBy,0)!=0) ORDER BY HH.UserName";
            List<User> oApprovalUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovalUser = new ESimSol.BusinessObjects.User();
            oApprovalUser.UserID = 0; oApprovalUser.UserName = "--Select Approval User--";
            oApprovalUsers.Add(oApprovalUser);
            oApprovalUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));

            sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.DisbursedBy FROM SampleRequest AS MM WHERE ISNULL(MM.DisbursedBy,0)!=0) ORDER BY HH.UserName";
            List<User> oDisburseUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oDisburseUser = new ESimSol.BusinessObjects.User();
            oDisburseUser.UserID = 0; oDisburseUser.UserName = "--Select Disburse User--";
            oDisburseUsers.Add(oDisburseUser);
            oDisburseUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));


            sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.RequestBy FROM SampleRequest AS MM WHERE ISNULL(MM.RequestBy,0)!=0) ORDER BY HH.UserName";
            List<User> oRequestUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oRequestUser = new ESimSol.BusinessObjects.User();
            oRequestUser.UserID = 0; oRequestUser.UserName = "--Select Request User--";
            oRequestUsers.Add(oRequestUser);
            oRequestUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));

            ViewBag.RequestUsers = oRequestUsers;
            ViewBag.DisburseUsers = oDisburseUsers;
            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.AmountCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oSampleRequests);
        }

        public ActionResult ViewSampleRequest(int id, int buid)
        {
            _oSampleRequest = new SampleRequest();
            if (id > 0)
            {
                _oSampleRequest = _oSampleRequest.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oSampleRequest.SampleRequestDetails = SampleRequestDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oSampleRequest.BUID = buid;

            List<EnumObject> oTemp = EnumObject.jGets(typeof(EnumProductNature)).Where(x => x.id == (int)EnumProductNature.Hanger || x.id == (int)EnumProductNature.Poly || x.id == (int)EnumProductNature.Cone || x.id == (int)EnumProductNature.Sizer).ToList();
            ViewBag.EnumType = oTemp;
            ViewBag.Units = MeasurementUnit.Gets("SELECT * FROM MeasurementUnit ORDER BY UnitType ASC", (int)Session[SessionInfo.currentUserID]);
            return View(_oSampleRequest);
        }
        public ActionResult ViewChallanRequest(int id, int buid)
        {
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            _oSampleRequest = new SampleRequest();
            if (id > 0)
            {
                _oSampleRequest = _oSampleRequest.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oSampleRequest.SampleRequestDetails = SampleRequestDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oSampleRequest.BUID = buid;

            List<EnumObject> oTemp = EnumObject.jGets(typeof(EnumProductNature)).Where(x => x.id == (int)EnumProductNature.Hanger || x.id == (int)EnumProductNature.Poly || x.id == (int)EnumProductNature.Cone || x.id == (int)EnumProductNature.Sizer).ToList();
            ViewBag.EnumType = oTemp;
            ViewBag.Units = MeasurementUnit.Gets("SELECT * FROM MeasurementUnit ORDER BY UnitType ASC", (int)Session[SessionInfo.currentUserID]);
            oWorkingUnits = WorkingUnit.GetsPermittedStore(_oSampleRequest.BUID, EnumModuleName.SampleRequest, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            _oSampleRequest.WorkingUnits = oWorkingUnits;
            return View(_oSampleRequest);
        }
        [HttpPost]
        public JsonResult Approved(SampleRequest oSampleRequest)
        {
            _oSampleRequest = new SampleRequest();
            try
            {
                _oSampleRequest = oSampleRequest;
           
                _oSampleRequest = _oSampleRequest.Approved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSampleRequest.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleRequest);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region HTTP UndoApproved
        [HttpPost]
        public JsonResult UndoApproved(SampleRequest oSampleRequest)
        {
            _oSampleRequest = new SampleRequest();
            try
            {

                _oSampleRequest = oSampleRequest;


                _oSampleRequest = _oSampleRequest.UndoApproved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSampleRequest.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleRequest);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult Save(SampleRequest oSampleRequest)
        {
            _oSampleRequest = new SampleRequest();
            try
            {
                _oSampleRequest = oSampleRequest;
                _oSampleRequest = oSampleRequest.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSampleRequest = new SampleRequest();
                _oSampleRequest.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleRequest);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Commit(SampleRequest oSampleRequest)
        {
            _oSampleRequest = new SampleRequest();
            try
            {
                _oSampleRequest = oSampleRequest;
                _oSampleRequest = oSampleRequest.Save((int)Session[SessionInfo.currentUserID]);
                _oSampleRequest = oSampleRequest.Commit((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSampleRequest = new SampleRequest();
                _oSampleRequest.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleRequest);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(SampleRequest oJB)
        {
            string sFeedBackMessage = "";
            try
            {
                SampleRequest oSampleRequest = new SampleRequest();
                sFeedBackMessage = oSampleRequest.Delete(oJB.SampleRequestID, (int)Session[SessionInfo.currentUserID]);
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
        #region Advance Search
        [HttpPost]
        public JsonResult AdvanceSearch(SampleRequest oSampleRequest)
        {
            _oSampleRequests = new List<SampleRequest>();
            try
            {
                string sSQL = this.GetSQL(oSampleRequest.Note, oSampleRequest.BUID);
                _oSampleRequests = SampleRequest.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSampleRequest = new SampleRequest();
                _oSampleRequests = new List<SampleRequest>();
                _oSampleRequest.ErrorMessage = ex.Message;
                _oSampleRequests.Add(_oSampleRequest);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleRequests);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sSearchingData, int nBUID)
        {
            string sRequestNo = Convert.ToString(sSearchingData.Split('~')[0]);
         
            EnumCompareOperator eRequestDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[1]);
            DateTime dStartDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            DateTime dEndDate = Convert.ToDateTime(sSearchingData.Split('~')[3]);
            EnumCompareOperator eRequestAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[4]);
            double nStartAmount = Convert.ToDouble(sSearchingData.Split('~')[5]);
            double nEndAmount = Convert.ToDouble(sSearchingData.Split('~')[6]);
            int nApprovedBy = Convert.ToInt32(sSearchingData.Split('~')[7]);
            int nDisbursedBy = Convert.ToInt32(sSearchingData.Split('~')[8]);
            int nRequestBy = Convert.ToInt32(sSearchingData.Split('~')[9]);
            string sBuyerIDs = Convert.ToString(sSearchingData.Split('~')[10]);
            string sProductIDs = Convert.ToString(sSearchingData.Split('~')[11]);

            string sReturn1 = "SELECT * FROM View_SampleRequest";
            string sReturn = "";

            #region BUID
            if (nBUID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID.ToString();
            }
            #endregion

            #region RequestNo
            if (sRequestNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RequestNo LIKE '%" + sRequestNo + "%'";
            }
            #endregion

            #region Request Date
            if (eRequestDate != EnumCompareOperator.None)
            {
                if (eRequestDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eRequestDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eRequestDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eRequestDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eRequestDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eRequestDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region SampleRequestAmount
            if (eRequestAmount != EnumCompareOperator.None)
            {
                if (eRequestAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalQty = " + nStartAmount.ToString("0.00");
                }
                else if (eRequestAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalQty != " + nStartAmount.ToString("0.00");
                }
                else if (eRequestAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalQty < " + nStartAmount.ToString("0.00");
                }
                else if (eRequestAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalQty > " + nStartAmount.ToString("0.00");
                }
                else if (eRequestAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalQty BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
                }
                else if (eRequestAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalQty NOT BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
                }
            }
            #endregion

            //#region Delivery By
            //if (nDeliveryBy != 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " ISNULL(DeliveryBy,0) = " + nDeliveryBy.ToString();
            //}
            //#endregion
            #region ApprovedBy
            if (nApprovedBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApprovedBy,0) = " + nApprovedBy.ToString();
            }
            #endregion
            #region DisbursedBy
            if (nDisbursedBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(DisbursedBy,0) = " + nDisbursedBy.ToString();
            }
            #endregion
            #region RequestBy
            if (nRequestBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(RequestBy,0) = " + nRequestBy.ToString();
            }
            #endregion
            #region BuyerIDs
            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region Products
            if (sProductIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SampleRequestID IN (SELECT TT.SampleRequestID FROM SampleRequestDetail AS TT WHERE TT.ProductID IN(" + sProductIDs + "))";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region Get

        [HttpPost]
        public JsonResult GetsDetailsByID(SampleRequestDetail oSampleRequestDetail)//Id=ContractorID
        {
            try
            {
                string Ssql = "SELECT*FROM View_SampleRequestDetail WHERE SampleRequestID=" + oSampleRequestDetail.SampleRequestID + " ";
                _oSampleRequestDetails = new List<SampleRequestDetail>();
                _oSampleRequestDetail.SampleRequestDetails = SampleRequestDetail.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSampleRequestDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleRequestDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region print
        [HttpPost]
        public ActionResult SetSampleRequestListData(SampleRequest oSampleRequest)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oSampleRequest);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintSampleRequests()
        {
            _oSampleRequest = new SampleRequest();
            try
            {
                _oSampleRequest = (SampleRequest)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_SampleRequest WHERE SampleRequestID IN (" + _oSampleRequest.ErrorMessage + ") Order By SampleRequestID";
                _oSampleRequests = SampleRequest.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSampleRequest = new SampleRequest();
                _oSampleRequests = new List<SampleRequest>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            //_oSampleRequest.Company = oCompany;

            rptSampleRequests oReport = new rptSampleRequests();
            byte[] abytes = oReport.PrepareReport(_oSampleRequests, oCompany);
            return File(abytes, "application/pdf");
        }

        public ActionResult SampleRequestPrintPreview(int id,bool PrintGatePass)
        {
            _oSampleRequest = new SampleRequest();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oSampleRequest = _oSampleRequest.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oSampleRequest.SampleRequestDetails = SampleRequestDetail.Gets(_oSampleRequest.SampleRequestID, (int)Session[SessionInfo.currentUserID]);
                //_oSampleRequest.BusinessUnit = oBusinessUnit.Get(_oSampleRequest.BUID, (int)Session[SessionInfo.currentUserID]);
            }
            //else
            //{
            //    _oSampleRequest.BusinessUnit = new BusinessUnit();
            //}
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            //_oSampleRequest.Company = oCompany;
            byte[] abytes;
            if (!PrintGatePass)
            {
                rptSampleRequest1 oReport = new rptSampleRequest1();
                abytes = oReport.PrepareReport(_oSampleRequest, oCompany);
            }
            else
            {
                rptSampleRequestGatePass oReport = new rptSampleRequestGatePass();               
                abytes = oReport.PrepareReport(_oSampleRequest, oCompany);
            }
            return File(abytes, "application/pdf");
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
        #endregion

    }

}
