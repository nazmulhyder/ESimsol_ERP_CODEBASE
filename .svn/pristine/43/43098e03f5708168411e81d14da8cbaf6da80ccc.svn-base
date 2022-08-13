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
	public class SampleAdjustmentController : Controller
	{
		#region Declaration

		SampleAdjustment _oSampleAdjustment = new SampleAdjustment();
		List<SampleAdjustment> _oSampleAdjustments = new  List<SampleAdjustment>();
        SampleAdjustmentDetail _oSampleAdjustmentDetail = new SampleAdjustmentDetail();
        List<SampleAdjustmentDetail> _oSampleAdjustmentDetails = new List<SampleAdjustmentDetail>();

		#endregion

		#region Functions

		#endregion

		#region Actions
		public ActionResult ViewSampleAdjustments(int buid, int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.SampleAdjustment).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oSampleAdjustments = new List<SampleAdjustment>(); 
			_oSampleAdjustments = SampleAdjustment.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
			return View(_oSampleAdjustments);
		}

		public ActionResult ViewSampleAdjustment(int id)
		{
			_oSampleAdjustment = new SampleAdjustment();
			if (id > 0)
			{
                _oSampleAdjustment = _oSampleAdjustment.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oSampleAdjustment.SampleAdjustmentDetails = SampleAdjustmentDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
			}
			return View(_oSampleAdjustment);
		}

        [HttpPost]
        public JsonResult GetSampleInvoiceList(SampleInvoice oSampleInvoice)
        {
            List<SampleInvoice> oSampleInvoices = new List<SampleInvoice>();
            _oSampleAdjustmentDetails = new List<SampleAdjustmentDetail>();
            string sSQL = "SELECT * FROM View_SampleInvoice WHERE PaymentType IN (3) AND BUID = " + oSampleInvoice.BUID + " AND InvoiceType IN (" + (int)EnumSampleInvoiceType.None + "," + (int)EnumSampleInvoiceType.SampleInvoice + "," + (int)EnumSampleInvoiceType.SalesContract + ") AND ContractorID = " + oSampleInvoice.ContractorID + " AND WaitForAdjust>0 AND ISNULL(ExportPIID,0)=0 AND SampleInvoiceID IN (SELECT HH.PKValue FROM VoucherMapping AS HH WHERE HH.TableName='SampleInvoice' AND HH.PKColumnName='SampleInvoiceID' AND HH.VoucherSetup=14) ORDER BY SampleInvoiceDate ASC";
            try
            {
                oSampleInvoices = SampleInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach(SampleInvoice oItem in oSampleInvoices)
                {
                    _oSampleAdjustmentDetail = new SampleAdjustmentDetail();
                    _oSampleAdjustmentDetail.SampleInvoiceID = oItem.SampleInvoiceID;
                    _oSampleAdjustmentDetail.RefAmount = oItem.Amount;
                    _oSampleAdjustmentDetail.AdjustAmount = oItem.WaitForAdjust;
                    _oSampleAdjustmentDetail.InvoiceNo = oItem.SampleInvoiceNo;
                    _oSampleAdjustmentDetail.SampleInvoiceDate = oItem.SampleInvoiceDate;
                    _oSampleAdjustmentDetails.Add(_oSampleAdjustmentDetail);
                }
            }
            catch (Exception ex)
            {
                _oSampleAdjustmentDetail = new SampleAdjustmentDetail();
                _oSampleAdjustmentDetail.ErrorMessage = ex.Message;
                _oSampleAdjustmentDetails.Add(_oSampleAdjustmentDetail);
            }
            var jsonResult = Json(_oSampleAdjustmentDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult Process(SampleAdjustment oSampleAdjustment)
        {
            _oSampleAdjustments = new List<SampleAdjustment>();
            try
            {
                _oSampleAdjustments =SampleAdjustment.Process(oSampleAdjustment.BUID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSampleAdjustment = new SampleAdjustment();
                _oSampleAdjustment.ErrorMessage = ex.Message;
            }

            var jsonResult = Json(_oSampleAdjustments, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

           
        } 

		[HttpPost]
		public JsonResult Save(SampleAdjustment oSampleAdjustment)
		{
			_oSampleAdjustment = new SampleAdjustment();
			try
			{
				_oSampleAdjustment = oSampleAdjustment;
				_oSampleAdjustment = _oSampleAdjustment.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oSampleAdjustment = new SampleAdjustment();
				_oSampleAdjustment.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oSampleAdjustment);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult Approve(SampleAdjustment oSampleAdjustment)
        {
            _oSampleAdjustment = new SampleAdjustment();
            try
            {
                _oSampleAdjustment = oSampleAdjustment.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSampleAdjustment = new SampleAdjustment();
                _oSampleAdjustment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleAdjustment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UnApprove(SampleAdjustment oSampleAdjustment)
        {
            _oSampleAdjustment = new SampleAdjustment();
            try
            {
                _oSampleAdjustment = oSampleAdjustment.UnApprove((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSampleAdjustment = new SampleAdjustment();
                _oSampleAdjustment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleAdjustment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 

		[HttpGet]
		public JsonResult Delete(int id)
		{
			string sFeedBackMessage = "";
			try
			{
				SampleAdjustment oSampleAdjustment = new SampleAdjustment();
				sFeedBackMessage = oSampleAdjustment.Delete(id, (int)Session[SessionInfo.currentUserID]);
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

        #region HttpGet For Search
        [HttpPost]
        public JsonResult Search(SampleAdjustment oSampleAdjustment)
        {
            List<SampleAdjustment> oSampleAdjustments = new List<SampleAdjustment>();
            try
            {
                string sSQL = GetSQL(oSampleAdjustment);
                oSampleAdjustments = SampleAdjustment.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSampleAdjustment = new SampleAdjustment();
                _oSampleAdjustment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSampleAdjustments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region GetSQL
        private string GetSQL(SampleAdjustment oSampleAdjustment)
        {
            string sParams = oSampleAdjustment.Params;
            int nIssueDateComboValue = 0;
            int nSANDateComboValue = 0;

            string sContractorIDs = "";
            DateTime dStartIssueDate = DateTime.Now;
            DateTime dEndIssueDate = DateTime.Now;
            DateTime dStartSANDate = DateTime.Now;
            DateTime dEndSANDate = DateTime.Now;
            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                oSampleAdjustment.AdjustmentNo = sParams.Split('~')[nCount++];
                oSampleAdjustment.SANNo = sParams.Split('~')[nCount++];
                nIssueDateComboValue = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStartIssueDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEndIssueDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);

                nSANDateComboValue = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStartSANDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEndSANDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                sContractorIDs = sParams.Split('~')[nCount++];
            }
            string sReturn1 = "SELECT * FROM View_SampleAdjustment";
            string sReturn = "";

            #region AdjustmentNo
            if (!string.IsNullOrEmpty(oSampleAdjustment.AdjustmentNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " AdjustmentNo LIKE '%" + oSampleAdjustment.AdjustmentNo + "%'";
            }
            #endregion

            #region SANNo
            if (!string.IsNullOrEmpty(oSampleAdjustment.SANNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SANNo LIKE '%" + oSampleAdjustment.SANNo + "%'";
            }

            #endregion

            #region Issue  Date Wise
            if (nIssueDateComboValue > 0)
            {
                if (nIssueDateComboValue == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate = '" + dStartIssueDate.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDateComboValue == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate != '" + dStartIssueDate.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDateComboValue == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate > '" + dStartIssueDate.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDateComboValue == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate < '" + dStartIssueDate.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDateComboValue == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate>= '" + dStartIssueDate.ToString("dd MMM yyyy") + "' AND IssueDate < '" + dEndIssueDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDateComboValue == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate< '" + dStartIssueDate.ToString("dd MMM yyyy") + "' OR IssueDate > '" + dEndIssueDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion

            #region SAN  Date Wise
            if (nSANDateComboValue > 0)
            {
                if (nSANDateComboValue == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SANDate = '" + dStartSANDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSANDateComboValue == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SANDate != '" + dStartSANDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSANDateComboValue == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SANDate > '" + dStartSANDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSANDateComboValue == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SANDate < '" + dStartSANDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSANDateComboValue == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SANDate>= '" + dStartSANDate.ToString("dd MMM yyyy") + "' AND SANDate < '" + dEndSANDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nSANDateComboValue == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SANDate< '" + dStartSANDate.ToString("dd MMM yyyy") + "' OR SANDate > '" + dEndSANDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion

            #region ContractorID
            if (!string.IsNullOrEmpty(sContractorIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sContractorIDs + ")";
            }
            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY SampleAdjustmentID";
            return sReturn;
        }
        #endregion


        #endregion

        #region PrintList
    
        public ActionResult PrintSampleAdjustmentPreview(int id)
        {
            _oSampleAdjustment = new SampleAdjustment();
            _oSampleAdjustment = _oSampleAdjustment.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oSampleAdjustment.SampleAdjustmentDetails = SampleAdjustmentDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);


            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptSampleAdjustment oReport = new rptSampleAdjustment();
            byte[] abytes = oReport.PrepareReport(_oSampleAdjustment, oCompany);
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
