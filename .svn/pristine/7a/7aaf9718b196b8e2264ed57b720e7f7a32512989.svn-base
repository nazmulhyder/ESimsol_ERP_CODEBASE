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
    public class FNReProRequestController : Controller
    {
        #region Declaration

        FNReProRequest _oFNReProRequest = new FNReProRequest();
        List<FNReProRequest> _oFNReProRequests = new List<FNReProRequest>();
        FNReProRequestDetail _oFNReProRequestDetail = new FNReProRequestDetail();
        List<FNReProRequestDetail> _oFNReProRequestDetails = new List<FNReProRequestDetail>();
        #endregion

        #region Functions
        #endregion

        #region Actions

        public ActionResult ViewFNReProRequests(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FNReProRequest).ToString() + "," + ((int)EnumModuleName.FNReProRequest).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oFNReProRequests = new List<FNReProRequest>();
            _oFNReProRequests = FNReProRequest.Gets("SELECT * FROM View_FNReProRequest WHERE Status=0", (int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.StatusList = EnumObject.jGets(typeof(EnumFNReProRequestStatus));
            return View(_oFNReProRequests);
        }

        public ActionResult ViewFNReProRequest(int id)
        {
            _oFNReProRequest = new FNReProRequest();
            if (id > 0)
            {
                _oFNReProRequest = _oFNReProRequest.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oFNReProRequest.FNReProRequestDetails = FNReProRequestDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }

            List<EnumObject> oTemp = EnumObject.jGets(typeof(EnumProductNature)).Where(x => x.id == (int)EnumProductNature.Hanger || x.id == (int)EnumProductNature.Poly || x.id == (int)EnumProductNature.Cone || x.id == (int)EnumProductNature.Sizer).ToList();
            ViewBag.EnumType = oTemp;
            ViewBag.Units = MeasurementUnit.Gets("SELECT * FROM MeasurementUnit ORDER BY UnitType ASC", (int)Session[SessionInfo.currentUserID]);
            ViewBag.FNTreatments = EnumObject.jGets(typeof(EnumFNTreatment));            
            return View(_oFNReProRequest);
        }

        [HttpPost]
        public JsonResult Save(FNReProRequest oFNReProRequest)
        {
            _oFNReProRequest = new FNReProRequest();
            try
            {
                _oFNReProRequest = oFNReProRequest;
                _oFNReProRequest = oFNReProRequest.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNReProRequest = new FNReProRequest();
                _oFNReProRequest.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNReProRequest);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Approve(FNReProRequest oFNReProRequest)
        {
            _oFNReProRequest = new FNReProRequest();
            try
            {
                _oFNReProRequest = oFNReProRequest.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNReProRequest = new FNReProRequest();
                _oFNReProRequest.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNReProRequest);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FNReProRequest oJB)
        {
            string sFeedBackMessage = "";
            try
            {
                FNReProRequest oFNReProRequest = new FNReProRequest();
                sFeedBackMessage = oFNReProRequest.Delete(oJB.FNReProRequestID, (int)Session[SessionInfo.currentUserID]);
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

        #region Adv Search

        [HttpPost]
        public JsonResult GetsData(FNReProRequest objFNReProRequest)
        {
            List<FNReProRequest> oFNReProRequestList = new List<FNReProRequest>();
            FNReProRequest oFNReProRequest = new FNReProRequest();
            //string sTemp = objFNReProRequest.ErrorMessage;
            try
            {
                string sSQL = GetSQL(objFNReProRequest);
                oFNReProRequestList = FNReProRequest.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNReProRequestList = new List<FNReProRequest>();
                oFNReProRequest.ErrorMessage = ex.Message;
                oFNReProRequestList.Add(oFNReProRequest);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNReProRequestList);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(FNReProRequest oFNReProRequest)
        {
            string sWhereCluse="", sSQL="";
            if (!String.IsNullOrEmpty(oFNReProRequest.ErrorMessage))
            {
                int nCount = 0;
                int nDate = Convert.ToInt16(oFNReProRequest.ErrorMessage.Split('~')[nCount++]);
                DateTime dRequestDateStart = Convert.ToDateTime(oFNReProRequest.ErrorMessage.Split('~')[nCount++]);
                DateTime dRequestDateEnd = Convert.ToDateTime(oFNReProRequest.ErrorMessage.Split('~')[nCount++]);

                string sBatchNo = oFNReProRequest.ErrorMessage.Split('~')[nCount++];
                int nStatus = Convert.ToInt16(oFNReProRequest.ErrorMessage.Split('~')[nCount++]);

                int nApproveDate = Convert.ToInt16(oFNReProRequest.ErrorMessage.Split('~')[nCount++]);
                DateTime dApproveDateStart = Convert.ToDateTime(oFNReProRequest.ErrorMessage.Split('~')[nCount++]);
                DateTime dApproveDateEnd = Convert.ToDateTime(oFNReProRequest.ErrorMessage.Split('~')[nCount++]);

                string sRequestNo = oFNReProRequest.ErrorMessage.Split('~')[nCount++];
                string sDispoNo = oFNReProRequest.ErrorMessage.Split('~')[nCount++];

                sWhereCluse = "";
                sSQL = "SELECT * FROM View_FNReProRequest ";

                #region Batch No
                if (!string.IsNullOrEmpty(sBatchNo))
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " FNReProRequestID IN (SELECT FNReProRequestID FROM View_FNReProRequestDetail WHERE BatchNo LIKE '%" + sBatchNo + "%') ";
                }
                #endregion

                #region RequestNo
                if (!string.IsNullOrEmpty(sRequestNo))
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " ReqNo LIKE '%" + sRequestNo + "%' ";
                }
                #endregion

                #region DispoNo
                if (!string.IsNullOrEmpty(sDispoNo))
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " FNReProRequestID IN (SELECT FNReProRequestID FROM View_FNReProRequestDetail WHERE FNExeNo LIKE '%" + sDispoNo + "%') ";
                }
                #endregion

                #region Status
                if (nStatus > -1)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Status = " + nStatus;
                }
                #endregion

                #region Request Date
                if (nDate != (int)EnumCompareOperator.None)
                {
                    if (nDate == (int)EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nDate == (int)EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestDateEnd.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nDate == (int)EnumCompareOperator.NotBetween)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestDateEnd.ToString("dd MMM yyyy") + "', 106))";
                    }
                }
                #endregion

                #region Approve Date
                if (nApproveDate != (int)EnumCompareOperator.None)
                {
                    if (nDate == (int)EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApproveDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApproveDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApproveDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApproveDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nDate == (int)EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApproveDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApproveDateEnd.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nDate == (int)EnumCompareOperator.NotBetween)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApproveDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApproveDateEnd.ToString("dd MMM yyyy") + "', 106))";
                    }
                }
                #endregion

                sSQL += sWhereCluse;
            }
            return sSQL;
        }
        #endregion

        #region Get

        [HttpPost]
        public JsonResult GetsFNBatchCardByNo(FNBatchCard oFNBatchCard)
        {
            List<FNBatchCard> oFNBatchCards = new List<FNBatchCard>();
            try
            {
                string sSQL = "SELECT TOP 250 * FROM View_FNBatchCard WHERE FNBatchID > 0";

                if (!string.IsNullOrEmpty(oFNBatchCard.FNBatchNo))
                    sSQL += " AND FNBatchNo LIKE '%" + oFNBatchCard.FNBatchNo + "%'";
                if (oFNBatchCard.FNTreatmentProcessID > 0)
                    sSQL += " AND FNTreatmentProcessID IN (" + oFNBatchCard.FNTreatmentProcessID + ")";
                if (oFNBatchCard.FNTreatment > 0)
                    sSQL += " AND FNTreatment =" + (int)oFNBatchCard.FNTreatment;

                oFNBatchCards = FNBatchCard.Gets(sSQL + " AND ISNULL(Qty_Prod,0)>0 ORDER BY PlannedDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNBatchCards = new List<FNBatchCard>();
                oFNBatchCards.Add(new FNBatchCard() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchCards);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region print
        //[HttpPost]
        //public ActionResult SetFNReProRequestListData(FNReProRequest oFNReProRequest)
        //{
        //    this.Session.Remove(SessionInfo.ParamObj);
        //    this.Session.Add(SessionInfo.ParamObj, oFNReProRequest);

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(Global.SessionParamSetMessage);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult PrintFNReProRequests()
        //{
        //    _oFNReProRequest = new FNReProRequest();
        //    try
        //    {
        //        _oFNReProRequest = (FNReProRequest)Session[SessionInfo.ParamObj];
        //        string sSQL = "SELECT * FROM View_FNReProRequest WHERE FNReProRequestID IN (" + _oFNReProRequest.ErrorMessage + ") Order By FNReProRequestID";
        //        _oFNReProRequests = FNReProRequest.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oFNReProRequest = new FNReProRequest();
        //        _oFNReProRequests = new List<FNReProRequest>();
        //    }
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //_oFNReProRequest.Company = oCompany;

        //    rptFNReProRequests oReport = new rptFNReProRequests();
        //    byte[] abytes = oReport.PrepareReport(_oFNReProRequests, oCompany);
        //    return File(abytes, "application/pdf");
        //}

        //public ActionResult FNReProRequestPrintPreview(int id)
        //{
        //    _oFNReProRequest = new FNReProRequest();
        //    Company oCompany = new Company();
        //    Contractor oContractor = new Contractor();
        //    BusinessUnit oBusinessUnit = new BusinessUnit();
        //    if (id > 0)
        //    {
        //        _oFNReProRequest = _oFNReProRequest.Get(id, (int)Session[SessionInfo.currentUserID]);
        //        _oFNReProRequest.FNReProRequestDetails = FNReProRequestDetail.Gets(_oFNReProRequest.FNReProRequestID, (int)Session[SessionInfo.currentUserID]);
        //        //_oFNReProRequest.BusinessUnit = oBusinessUnit.Get(_oFNReProRequest.BUID, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    //else
        //    //{
        //    //    _oFNReProRequest.BusinessUnit = new BusinessUnit();
        //    //}
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //_oFNReProRequest.Company = oCompany;
        //    byte[] abytes;
        //    rptFNReProRequest oReport = new rptFNReProRequest();
        //    abytes = oReport.PrepareReport(_oFNReProRequest, oCompany);
        //    return File(abytes, "application/pdf");
        //}
        //public Image GetCompanyLogo(Company oCompany)
        //{
        //    if (oCompany.OrganizationLogo != null)
        //    {
        //        string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
        //        if (System.IO.File.Exists(fileDirectory))
        //        {
        //            System.IO.File.Delete(fileDirectory);
        //        }

        //        MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
        //        System.Drawing.Image img = System.Drawing.Image.FromStream(m);
        //        img.Save(fileDirectory, ImageFormat.Jpeg);
        //        return img;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        #endregion

    }

}
