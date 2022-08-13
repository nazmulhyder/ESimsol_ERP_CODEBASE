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
    public class KnitDyeingBatchController : Controller
    {
        #region Declaration

        KnitDyeingBatch _oKnitDyeingBatch = new KnitDyeingBatch();
        List<KnitDyeingBatch> _oKnitDyeingBatchs = new List<KnitDyeingBatch>();
        #endregion

        #region Functions

        #endregion

        #region Actions

        public ActionResult ViewKnitDyeingBatchs(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oKnitDyeingBatchs = new List<KnitDyeingBatch>();
            _oKnitDyeingBatchs = KnitDyeingBatch.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.SourcingConfigHeadType = EnumObject.jGets(typeof(EnumSourcingConfigHeadType));
            ViewBag.BUID = buid;
            return View(_oKnitDyeingBatchs);
        }

        public ActionResult ViewKnitDyeingBatch(int id, double ts)
        {
            _oKnitDyeingBatch = new KnitDyeingBatch();
            if (id > 0)
            {
                _oKnitDyeingBatch = _oKnitDyeingBatch.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oKnitDyeingBatch.KnitDyeingBatchDetails = KnitDyeingBatchDetail.Gets("SELECT * FROM View_KnitDyeingBatchDetail WHERE KnitDyeingBatchID = " + id, ((int)Session[SessionInfo.currentUserID]));
                _oKnitDyeingBatch.KnitDyeingBatchYarns = KnitDyeingBatchYarn.Gets("SELECT * FROM View_KnitDyeingBatchYarn WHERE KnitDyeingBatchID = " + id, ((int)Session[SessionInfo.currentUserID]));
            }
            return View(_oKnitDyeingBatch);
        }
        public ActionResult PrintKnitDyeingBatch(int id)
        {
            KnitDyeingBatch _oKnitDyeingBatch = new KnitDyeingBatch();

            _oKnitDyeingBatch = _oKnitDyeingBatch.Get(id, (int)Session[SessionInfo.currentUserID]);
            List<KnitDyeingBatchDetail> oKnitDyeingBatchDetails = new List<KnitDyeingBatchDetail>();
            oKnitDyeingBatchDetails = KnitDyeingBatchDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oKnitDyeingBatch.KnitDyeingBatchDetails = oKnitDyeingBatchDetails;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oKnitDyeingBatch.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptKnitDyeingBatch oReport = new rptKnitDyeingBatch();
            byte[] abytes = oReport.PrepareReport(_oKnitDyeingBatch, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        [HttpPost]
        public JsonResult Save(KnitDyeingBatch oKnitDyeingBatch)
        {
            _oKnitDyeingBatch = new KnitDyeingBatch();
            try
            {
                _oKnitDyeingBatch = oKnitDyeingBatch;
                _oKnitDyeingBatch = _oKnitDyeingBatch.SaveAll((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnitDyeingBatch = new KnitDyeingBatch();
                _oKnitDyeingBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approved(KnitDyeingBatch oKnitDyeingBatch)
        {
            _oKnitDyeingBatch = new KnitDyeingBatch();
            try
            {
                _oKnitDyeingBatch = oKnitDyeingBatch;
                _oKnitDyeingBatch = _oKnitDyeingBatch.Approved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnitDyeingBatch = new KnitDyeingBatch();
                _oKnitDyeingBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveKnitDyeingBatchGrayChallans(KnitDyeingBatch oKnitDyeingBatch)
        {
            _oKnitDyeingBatch = new KnitDyeingBatch();
            try
            {
                if (oKnitDyeingBatch.KnitDyeingBatchID > 0 && oKnitDyeingBatch.KnitDyeingBatchGrayChallans.Count > 0)
                {
                    _oKnitDyeingBatch = oKnitDyeingBatch.SaveKnitDyeingBatchGrayChallans((int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oKnitDyeingBatch = new KnitDyeingBatch();
                _oKnitDyeingBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                KnitDyeingBatch oKnitDyeingBatch = new KnitDyeingBatch();
                sFeedBackMessage = oKnitDyeingBatch.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetKnitDyeingBatchDetails(KnitDyeingBatch oKnitDyeingBatch)
        {
            KnitDyeingBatchDetail oKnitDyeingBatchDetail = new KnitDyeingBatchDetail();
            List<KnitDyeingBatchDetail> oKnitDyeingBatchDetails = new List<KnitDyeingBatchDetail>();
            try
            {
                if (oKnitDyeingBatch.KnitDyeingBatchID > 0)
                {
                    oKnitDyeingBatchDetails = KnitDyeingBatchDetail.Gets("SELECT * FROM View_KnitDyeingBatchDetail WHERE KnitDyeingBatchID = " + oKnitDyeingBatch.KnitDyeingBatchID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oKnitDyeingBatchDetail = new KnitDyeingBatchDetail();
                oKnitDyeingBatchDetail.ErrorMessage = ex.Message;
                oKnitDyeingBatchDetails.Add(oKnitDyeingBatchDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnitDyeingBatchDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetKnitDyeingBatchGrayChallans(KnitDyeingBatch oKnitDyeingBatch)
        {
            KnitDyeingBatchGrayChallan oKnitDyeingBatchGrayChallan = new KnitDyeingBatchGrayChallan();
            List<KnitDyeingBatchGrayChallan> oKnitDyeingBatchGrayChallans = new List<KnitDyeingBatchGrayChallan>();
            try
            {
                if (oKnitDyeingBatch.KnitDyeingBatchID > 0)
                {
                    oKnitDyeingBatchGrayChallans = KnitDyeingBatchGrayChallan.Gets("SELECT * FROM View_KnitDyeingBatchGrayChallan WHERE KnitDyeingBatchDetailID IN (SELECT KnitDyeingBatchDetailID FROM KnitDyeingBatchDetail WHERE KnitDyeingBatchID =" + oKnitDyeingBatch.KnitDyeingBatchID + ")", (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oKnitDyeingBatchGrayChallan = new KnitDyeingBatchGrayChallan();
                oKnitDyeingBatchGrayChallan.ErrorMessage = ex.Message;
                oKnitDyeingBatchGrayChallans.Add(oKnitDyeingBatchGrayChallan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnitDyeingBatchGrayChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
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

        #region SEARCH
        [HttpPost]
        public JsonResult GetsOrderRecapByProgramID(KnitDyeingProgram oKnitDyeingProgram)
        {
            List<OrderRecap> oOrderRecaps = new List<OrderRecap>();
            OrderRecap oOrderRecap = new OrderRecap();
            try
            {
                string sSQL = "SELECT * FROM View_OrderRecap WHERE OrderRecapID IN (SELECT OrderRecapID FROM KnitDyeingProgramDetail WHERE KnitDyeingProgramID = " + oKnitDyeingProgram.KnitDyeingProgramID + ")";
                oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oOrderRecap = new OrderRecap();
                oOrderRecap.ErrorMessage = ex.Message;
                oOrderRecaps.Add(oOrderRecap);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsColorByOrderRecapAndProgramDetailID(KnitDyeingProgramDetail oKnitDyeingProgramDetail)
        {
            List<KnitDyeingProgramDetail> objKnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
            KnitDyeingProgramDetail objKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
            try
            {
                string sSQL = "SELECT HH.ColorID, HH.ColorName, SUM(HH.ReqFabricQty) AS ReqFabricQty FROM View_KnitDyeingProgramDetail AS HH WHERE HH.KnitDyeingProgramID =" + oKnitDyeingProgramDetail.KnitDyeingProgramID + " AND HH.RefObjectNo LIKE '%" + oKnitDyeingProgramDetail.RefObjectNo + "%' GROUP BY HH.ColorID, HH.ColorName";
                objKnitDyeingProgramDetails = KnitDyeingProgramDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                objKnitDyeingProgramDetail = new KnitDyeingProgramDetail();
                objKnitDyeingProgramDetail.ErrorMessage = ex.Message;
                objKnitDyeingProgramDetails.Add(objKnitDyeingProgramDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(objKnitDyeingProgramDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AdvanceSearch(KnitDyeingBatch oKnitDyeingBatch)
        {
            List<KnitDyeingBatch> oKnitDyeingBatchs = new List<KnitDyeingBatch>();
            string sSQL = GetSQL(oKnitDyeingBatch);
            oKnitDyeingBatchs = KnitDyeingBatch.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnitDyeingBatchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL(KnitDyeingBatch oKnitDyeingBatch)
        {
            int nCount = 0;
            int nDiaType = Convert.ToInt32(oKnitDyeingBatch.ErrorMessage.Split('~')[nCount++]);
            string sDiaName = oKnitDyeingBatch.ErrorMessage.Split('~')[nCount++];
            int nIssueDate = Convert.ToInt32(oKnitDyeingBatch.ErrorMessage.Split('~')[nCount++]);
            DateTime dIssueDateStart = Convert.ToDateTime(oKnitDyeingBatch.ErrorMessage.Split('~')[nCount++]);
            DateTime dIssueDateEnd = Convert.ToDateTime(oKnitDyeingBatch.ErrorMessage.Split('~')[nCount++]);

            string sReturn1 = "SELECT * FROM View_KnitDyeingBatch ";
            string sReturn = "";

            #region Style
            if (!string.IsNullOrEmpty(oKnitDyeingBatch.StyleNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " TechnicalSheetID IN (SELECT TechnicalSheetID FROM TechnicalSheet WHERE TechnicalSheetID IN (" + oKnitDyeingBatch.StyleNo + "))";
            }
            #endregion

            #region Order Recap
            if (!string.IsNullOrEmpty(oKnitDyeingBatch.OrderRecapNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderRecapID IN (" + oKnitDyeingBatch.OrderRecapNo + ")";
            }
            #endregion

            #region Color
            if (!string.IsNullOrEmpty(oKnitDyeingBatch.ColorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ColorName LIKE " + "'%" + oKnitDyeingBatch.ColorName + "%'";
            }
            #endregion

            #region Batch No
            if (!string.IsNullOrEmpty(oKnitDyeingBatch.BatchNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BatchNo LIKE " + "'%" + oKnitDyeingBatch.BatchNo + "%'";
            }
            #endregion

            #region Dyed Type
            if (nDiaType > 0)
            {
                if (nDiaType == (int)EnumSourcingConfigHeadType.Wash){
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WashName LIKE " + "'%" + sDiaName + "%'";
                }
                else if (nDiaType == (int)EnumSourcingConfigHeadType.GSM){
                    
                }
                else if (nDiaType == (int)EnumSourcingConfigHeadType.FinishDIA){
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FinishedGSMName LIKE " + "'%" + sDiaName + "%'";
                }
                else if (nDiaType == (int)EnumSourcingConfigHeadType.MachineDIA){
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MachineName LIKE " + "'%" + sDiaName + "%'";
                }
                else if (nDiaType == (int)EnumSourcingConfigHeadType.YarnType){
                    
                }
                else if (nDiaType == (int)EnumSourcingConfigHeadType.FabricType){
                    
                } 
                else if (nDiaType == (int)EnumSourcingConfigHeadType.GrayDIA){

                }
            }
            #endregion

            #region Issue Date Wise
            if (nIssueDate > 0)
            {
                if (nIssueDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BatchIssueDate = '" + dIssueDateStart.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (BatchIssueDate>= '" + dIssueDateStart.ToString("dd MMM yyyy") + "' AND BatchIssueDate < '" + dIssueDateStart.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nIssueDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BatchIssueDate > '" + dIssueDateStart.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BatchIssueDate < '" + dIssueDateStart.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (BatchIssueDate>= '" + dIssueDateStart.ToString("dd MMM yyyy") + "' AND BatchIssueDate < '" + dIssueDateEnd.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nIssueDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (BatchIssueDate< '" + dIssueDateStart.ToString("dd MMM yyyy") + "' OR BatchIssueDate > '" + dIssueDateEnd.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        public ActionResult PrintList(string tvs, int buid)
        {
            List<KnitDyeingBatch> oKnitDyeingBatchs = new List<KnitDyeingBatch>();
            KnitDyeingBatch _oKnitDyeingBatch = new KnitDyeingBatch();
            oKnitDyeingBatchs = KnitDyeingBatch.Gets("SELECT * FROM View_KnitDyeingBatch  AS HH ORDER BY KnitDyeingBatchID ASC", (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);

            rptKnitDyeingBatchs oReport = new rptKnitDyeingBatchs();
            byte[] abytes = oReport.PrepareReport(oKnitDyeingBatchs, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        #endregion
    }
}
