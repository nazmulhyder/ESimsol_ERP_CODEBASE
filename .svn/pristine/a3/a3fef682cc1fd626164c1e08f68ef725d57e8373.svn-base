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
	public class FNProductionBatchQualityController : Controller
	{
		#region Declaration

		FNProductionBatchQuality _oFNProductionBatchQuality = new FNProductionBatchQuality();
		List<FNProductionBatchQuality> _oFNProductionBatchQualitys = new  List<FNProductionBatchQuality>();
        List<FNProductionBatch> _oFNProductionBatchs = new List<FNProductionBatch>();
		#endregion

		#region Functions

		#endregion

		#region Actions

		public ActionResult ViewFNProductionBatchQualitys(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
            _oFNProductionBatchs = new List<FNProductionBatch>();
            string sSQL = "SELECT top(100)* FROM View_FNProductionBatch AS HH WHERE HH.FNPBatchID NOT IN (SELECT FNPBQ.FNPBatchID FROM FNProductionBatchQuality FNPBQ) AND ISNULL(HH.EndDateTime,'')!='' AND HH.FNBatchCardID IN(SELECT FBC.FNBatchCardID FROM FNBatchCard FBC WHERE FBC.FNTreatmentProcessID IN( SELECT FNTP.FNTPID FROM FNTreatmentProcess FNTP WHERE FNTP.FNTreatment= " + (int)EnumFNTreatment.Pre_Treatment + ")) order by EndDateTime DESC,BatchNo ASC";
            _oFNProductionBatchs = FNProductionBatch.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);//consider as Pre-treatmentBatch
            ViewBag.DyeingBatchs = FNProductionBatch.Gets("SELECT top(100)* FROM View_FNProductionBatch AS HH WHERE FNProductionID>0 and HH.FNPBatchID NOT IN (SELECT FNPBQ.FNPBatchID FROM FNProductionBatchQuality FNPBQ) AND ISNULL(HH.EndDateTime,'')!='' AND HH.FNBatchCardID IN(SELECT FBC.FNBatchCardID FROM FNBatchCard FBC WHERE FBC.FNTreatmentProcessID IN( SELECT FNTP.FNTPID FROM FNTreatmentProcess FNTP WHERE FNTP.FNTreatment= " + (int)EnumFNTreatment.Dyeing + ")) order by EndDateTime DESC,BatchNo ASC", (int)Session[SessionInfo.currentUserID]);
            ViewBag.FinishingBatchs = FNProductionBatch.Gets("SELECT top(100)* FROM View_FNProductionBatch AS HH WHERE HH.FNPBatchID NOT IN (SELECT FNPBQ.FNPBatchID FROM FNProductionBatchQuality FNPBQ) AND ISNULL(HH.EndDateTime,'')!='' AND HH.FNBatchCardID IN(SELECT FBC.FNBatchCardID FROM FNBatchCard FBC WHERE FBC.FNTreatmentProcessID IN( SELECT FNTP.FNTPID FROM FNTreatmentProcess FNTP WHERE FNTP.FNTreatment= " + (int)EnumFNTreatment.Finishing + ")) order by EndDateTime DESC,BatchNo ASC", (int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oFNProductionBatchs);
		}

		public ActionResult ViewFNProductionBatchQuality(int id)
		{
			_oFNProductionBatchQuality = new FNProductionBatchQuality();
			if (id > 0)
			{
				_oFNProductionBatchQuality = _oFNProductionBatchQuality.Get(id, (int)Session[SessionInfo.currentUserID]);
			}
			return View(_oFNProductionBatchQuality);
		}

		[HttpPost]
		public JsonResult Save(FNProductionBatchQuality oFNProductionBatchQuality)
		{
			_oFNProductionBatchQuality = new FNProductionBatchQuality();
			try
			{
				_oFNProductionBatchQuality = oFNProductionBatchQuality;
				_oFNProductionBatchQuality = _oFNProductionBatchQuality.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oFNProductionBatchQuality = new FNProductionBatchQuality();
				_oFNProductionBatchQuality.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oFNProductionBatchQuality);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}
        [HttpPost]
        public JsonResult Reprocess(FNProductionBatchQuality oFNProductionBatchQuality)
        {    
            FNProductionBatch oFNProductionBatch = new FNProductionBatch();
            _oFNProductionBatchQuality = new FNProductionBatchQuality();
            try
            {
                _oFNProductionBatchQuality = oFNProductionBatchQuality;
                _oFNProductionBatchQuality = _oFNProductionBatchQuality.Reprocess((int)Session[SessionInfo.currentUserID]);
                _oFNProductionBatchQuality.FNProductionBatch = oFNProductionBatch.Get(_oFNProductionBatchQuality.FNPBatchID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNProductionBatchQuality = new FNProductionBatchQuality();
                _oFNProductionBatchQuality.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNProductionBatchQuality);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsFNBatch(FNBatch oFNBatch)
        {
            List<FNBatch> oFNBatchs = new List<FNBatch>();
        
            try
            {
                string sSQL = " SELECT * FROM View_FNBatch WHERE BatchNo LIKE '%" + oFNBatch.BatchNo + "%'";
                oFNBatchs = FNBatch.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFNBatchs = new List<FNBatch>();
                oFNBatch = new FNBatch();
                oFNBatch.ErrorMessage = ex.Message;
                oFNBatchs.Add(oFNBatch);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNBatchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult GetsFNExeOrders(FNExecutionOrder oFNExecutionOrder)
        //{
        //    List<FNExecutionOrder> oFNExecutionOrders = new List<FNExecutionOrder>();

        //    try
        //    {
        //        string sSQL = " SELECT * FROM view_FNExecutionOrder WHERE FNExONo LIKE '%" + oFNExecutionOrder.FNExONo + "%'";
        //        oFNExecutionOrders = FNExecutionOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        oFNExecutionOrders = new List<FNExecutionOrder>();
        //        oFNExecutionOrder = new FNExecutionOrder();
        //        oFNExecutionOrder.ErrorMessage = ex.Message;
        //        oFNExecutionOrders.Add(oFNExecutionOrder);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oFNExecutionOrders);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult FNProductionBatchQualitysAdvSearch(FNProductionBatch oFNProductionBatch)
        {
            _oFNProductionBatchs = new List<FNProductionBatch>();
            try
            {
              
                string orderIDs = oFNProductionBatch.Params.Split('~')[0];
                string  batchIDs= oFNProductionBatch.Params.Split('~')[1];
                bool chkQCOk = Convert.ToBoolean(oFNProductionBatch.Params.Split('~')[2]);
                bool chkQCNotOk = Convert.ToBoolean(oFNProductionBatch.Params.Split('~')[3]);
                bool chkWaitingForQCOk = Convert.ToBoolean(oFNProductionBatch.Params.Split('~')[4]);
                int TreatmentProcess = Convert.ToInt32(oFNProductionBatch.Params.Split('~')[5]);
                bool chkWaitingForTransfer = Convert.ToBoolean(oFNProductionBatch.Params.Split('~')[6]);
                int ncboDate = Convert.ToInt32(oFNProductionBatch.Params.Split('~')[7]);
                DateTime dStartDate = DateTime.MinValue; DateTime dEndDate = DateTime.MinValue;
                if(ncboDate!=0)
                {
                    dStartDate = Convert.ToDateTime(oFNProductionBatch.Params.Split('~')[8]);
                    dEndDate = Convert.ToDateTime(oFNProductionBatch.Params.Split('~')[9]);
                }
                string sSQL = " SELECT * FROM View_FNProductionBatch AS HH WHERE HH.FNPBatchID  IN (SELECT FNPBQ.FNPBatchID FROM FNProductionBatchQuality FNPBQ) AND HH.FNBatchCardID IN(SELECT FBC.FNBatchCardID FROM FNBatchCard FBC WHERE FBC.FNTreatmentProcessID IN( SELECT FNTP.FNTPID FROM FNTreatmentProcess FNTP WHERE FNTP.FNTreatment=" + TreatmentProcess + "))";
                #region WaitingForQCOk

                if (chkWaitingForQCOk)
                {
                    sSQL += " AND FNPBatchID NOT IN (SELECT FNPBatchID FROM FNProductionBatchQuality) AND ISNULL(EndDateTime,'')!=''";

                }
                #endregion
                #region QC Not Ok

                if (chkQCNotOk)
                {
                    sSQL += " AND FNPBatchID  IN (SELECT FNPBatchID FROM FNProductionBatchQuality WHERE IsOK =0)";

                }
                #endregion
                #region QC  Ok
                if (chkQCOk)
                {
                    sSQL += " AND FNPBatchID  IN (SELECT FNPBatchID FROM FNProductionBatchQuality WHERE IsOK =1)";
                }
                #endregion
                #region FNExeOrders
                if (!string.IsNullOrEmpty(orderIDs))
                    sSQL += " AND FNExOID  IN (" + orderIDs + ")";
                #endregion
                #region batchIDs
                if (!string.IsNullOrEmpty(batchIDs))
                    sSQL += " AND FNBatchID  IN (" + batchIDs + ")";
                #endregion
                #region   Date Wise
                if (ncboDate > 0)
                {
                    if (ncboDate == 1)
                    {
                         
                        sSQL += " AND StartDateTime = '" + dStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (ncboDate == 2)
                    {
                         
                        sSQL += " AND StartDateTime != '" + dStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (ncboDate == 3)
                    {
                         
                        sSQL += " AND StartDateTime > '" + dStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (ncboDate == 4)
                    {
                         
                        sSQL += " AND StartDateTime < '" + dStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (ncboDate == 5)
                    {
                         
                        sSQL += " AND StartDateTime>= '" + dStartDate.ToString("dd MMM yyyy") + "' AND StartDateTime < '" + dEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    }
                    if (ncboDate == 6)
                    {
                         
                        sSQL += " AND StartDateTime< '" + dStartDate.ToString("dd MMM yyyy") + "' OR StartDateTime > '" + dEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    }
                }
                #endregion

                _oFNProductionBatchs = FNProductionBatch.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNProductionBatchs = new List<FNProductionBatch>();
                oFNProductionBatch = new FNProductionBatch();
                oFNProductionBatch.ErrorMessage = ex.Message;
                _oFNProductionBatchs.Add(oFNProductionBatch);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNProductionBatchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PrintQCParameter(int nFNPBatchID)
        {
            FNProductionBatch oFNProductionBatch = new FNProductionBatch();
            List<FNProductionBatch> oFNProductionBatchs = new List<FNProductionBatch>();
            FNBatch oFNBatch = new FNBatch();
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
            List<FNQCResult> oFNQCResults = new List<FNQCResult>();
            
            if(nFNPBatchID>0)
            {
                oFNProductionBatch = oFNProductionBatch.Get(nFNPBatchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFNBatch = FNBatch.Get(oFNProductionBatch.FNBatchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSalesContractDetail = oFabricSalesContractDetail.Get(oFNBatch.FNExOID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oFNQCResults = oFNQCResults = FNQCResult.Gets("SELECT * FROM View_FNQCResult WHERE FNPBatchID = '" + oFNProductionBatch.FNPBatchID + "' AND FNTPID = " + oFNProductionBatch.FNTPID + " Order By FNQCParameterID, TestMethod", (int)Session[SessionInfo.currentUserID]);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptFNQCTestReport oReport = new rptFNQCTestReport();
            byte[] abytes = oReport.PrepareReport(oFabricSalesContractDetail, oFNQCResults, oCompany, oBusinessUnit);
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


		#endregion

	}

}
