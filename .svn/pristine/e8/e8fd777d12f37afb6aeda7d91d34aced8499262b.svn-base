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
	public class FGQCController : Controller
	{
		#region Declaration
		FGQC _oFGQC = new FGQC();
		List<FGQC> _oFGQCs = new  List<FGQC>();
		#endregion

		#region Actions
		public ActionResult ViewFGQCs(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FGQC).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oFGQCs = new List<FGQC>();
            _oFGQCs = FGQC.Gets("SELECT * FROM View_FGQC AS HH WHERE ISNULL(HH.ApprovedBy,0) = 0 ORDER BY FGQCID ASC", (int)Session[SessionInfo.currentUserID]);

            List<ESimSol.BusinessObjects.User> oApprovedUsers = new List<User>();
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT ApprovedBy FROM FGQC) ORDER BY UserName ASC";
            oApprovedUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ApprovedUsers = oApprovedUsers;
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
			return View(_oFGQCs);
		}

		public ActionResult ViewFGQC(int id)
		{
			_oFGQC = new FGQC();
			if (id > 0)
			{
				_oFGQC = _oFGQC.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oFGQC.FGQCDetails = FGQCDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
			}            
            
            #region Trigger Type
            EnumObject oEnumObject = new EnumObject();
            List<EnumObject> oEnumObjects = new List<EnumObject>();
            
            oEnumObject = new EnumObject();
            oEnumObject.id = (int)EnumTriggerParentsType.RouteSheet;
            oEnumObject.Value = "Yarn";
            oEnumObjects.Add(oEnumObject);

            oEnumObject = new EnumObject();
            oEnumObject.id = (int)EnumTriggerParentsType.RouteSheetDetail;
            oEnumObject.Value = "Dyes/Chemical";
            oEnumObjects.Add(oEnumObject);

            oEnumObject = new EnumObject();
            oEnumObject.id = (int)EnumTriggerParentsType.ProductionRecipe;
            oEnumObject.Value = "Raw Material";
            oEnumObjects.Add(oEnumObject);
            #endregion

            ViewBag.TriggerTypes = oEnumObjects;
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oFGQC);
		}

        [HttpPost]
        public JsonResult GetsFinishGoods(FGQC oFGQC)
        {
            FGQCDetail oFGQCDetail = new FGQCDetail();
            List<FGQCDetail> oFGQCDetails = new List<FGQCDetail>();
            try
            {
                oFGQCDetails = FGQCDetail.FGQCProcess(oFGQC, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFGQCDetail = new FGQCDetail();
                oFGQCDetail.ErrorMessage = ex.Message;
                oFGQCDetails = new List<FGQCDetail>();
                oFGQCDetails.Add(oFGQCDetail);
            }

            var jsonResult = Json(oFGQCDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

		[HttpPost]
		public JsonResult Save(FGQC oFGQC)
		{
			_oFGQC = new FGQC();
			try
			{
				_oFGQC = oFGQC;
				_oFGQC = _oFGQC.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oFGQC = new FGQC();
				_oFGQC.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oFGQC);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult GetSuggestFGQCDate(FGQC oFGQC)
        {
            _oFGQC = new FGQC();
            try
            {                
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(oFGQC.BUID, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Dyeing)
                {
                    string sSQL = "SELECT MIN(RSH.EventTime) AS SuggestDate FROM RouteSheetHistory AS RSH WHERE RSH.CurrentStatus=13 AND RSH.RouteSheetID IN(SELECT HH.RouteSheetID FROM View_RouteSheet AS HH WHERE HH.BUID=" + oFGQC.BUID.ToString() + " AND HH.RouteSheetID NOT IN(SELECT FGQCD.RefID FROM FGQCDetail AS FGQCD WHERE FGQCD.RefType=2))";
                    _oFGQC = _oFGQC.GetSuggestFGQCDate(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    string sSQL = "SELECT MIN(HH.OperationTime) AS SuggestDate FROM QC AS HH WHERE HH.ProductionSheetID IN(SELECT MM.ProductionSheetID FROM ProductionSheet AS MM WHERE MM.BUID=" + oFGQC.BUID.ToString() + ") AND ISNULL(HH.LotID,0)!=0 AND HH.QCID NOT IN (SELECT FGQCD.RefID FROM FGQCDetail AS FGQCD WHERE FGQCD.RefType=1)";
                    _oFGQC = _oFGQC.GetSuggestFGQCDate(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oFGQC = new FGQC();
                _oFGQC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFGQC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 


		[HttpPost]
        public JsonResult Delete(FGQC oFGQC)
		{
			string sFeedBackMessage = "";
			try
			{				
				sFeedBackMessage = oFGQC.Delete(oFGQC.FGQCID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Approved(FGQC oFGQC)
        {            
            try
            {
                oFGQC = oFGQC.Approved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFGQC = new FGQC();
                oFGQC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFGQC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 
		#endregion

        #region Advance Search
        [HttpPost]
        public JsonResult GetsByFGQCNo(FGQC oFGQC)
        {
            List<FGQC> oFGQCs = new List<FGQC>();            
            try
            {
                string sSQL = "SELECT * FROM View_FGQC AS HH WHERE HH.FGQCNo LIKE '%" + oFGQC.FGQCNo + "%' ORDER BY HH.FGQCID ASC";                
                oFGQCs = FGQC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFGQCs = new List<FGQC>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFGQCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AdvanceSearch(FGQC oFGQC)
        {
            _oFGQCs = new List<FGQC>();
            try
            {
                string sSQL = this.GetSQL(oFGQC);
                _oFGQCs = FGQC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFGQC = new FGQC();
                _oFGQCs = new List<FGQC>();
                _oFGQC.ErrorMessage = ex.Message;
                _oFGQCs.Add(_oFGQC);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFGQCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(FGQC oFGQC)
        {
            EnumCompareOperator eFGQCDate = (EnumCompareOperator)Convert.ToInt32(oFGQC.Remarks.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(oFGQC.Remarks.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(oFGQC.Remarks.Split('~')[2]);
            string sRemarks = oFGQC.Remarks.Split('~')[3];
            
            string sReturn1 = "SELECT * FROM View_FGQC";
            string sReturn = "";

            #region BUID
            if (oFGQC.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + oFGQC.BUID.ToString();
            }
            #endregion

            #region FGQCNo
            oFGQC.FGQCNo = oFGQC.FGQCNo == null ? "" : oFGQC.FGQCNo;
            if (oFGQC.FGQCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FGQCNo LIKE '%" + oFGQC.FGQCNo + "%'";
            }
            #endregion
            
            #region FGQC Date
            if (eFGQCDate != EnumCompareOperator.None)
            {
                if (eFGQCDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FGQCDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eFGQCDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FGQCDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eFGQCDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FGQCDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eFGQCDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FGQCDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eFGQCDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FGQCDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eFGQCDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FGQCDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region ApprovedBy
            if (oFGQC.ApprovedBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApprovedBy,0) = " + oFGQC.ApprovedBy.ToString();
            }
            #endregion

            #region Remarks
            if (sRemarks != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Remarks LIKE '%" + sRemarks + "%'";
            }
            #endregion

            sReturn = sReturn1 + sReturn+ " ORDER BY FGQCID ASC";
            return sReturn;
        }
        #endregion

        #region Print FGQCs
        public ActionResult PrintFGQCs(string Param)
        {
            _oFGQCs = new List<FGQC>();
            string sSQLQuery = "SELECT * FROM View_FGQC AS HH WHERE HH.FGQCID IN (" + Param + ") ORDER BY HH.FGQCID ASC";
            _oFGQCs = FGQC.Gets(sSQLQuery, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptFGQCs oReport = new rptFGQCs();
            byte[] abytes = oReport.PrepareReport(_oFGQCs, oCompany);
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
    }

}
