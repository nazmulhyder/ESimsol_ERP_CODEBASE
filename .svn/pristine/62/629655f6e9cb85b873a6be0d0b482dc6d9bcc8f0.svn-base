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
	public class KnitDyeingGrayChallanController : Controller
	{
		#region Declaration

        KnitDyeingBatch _oKnitDyeingBatch = new KnitDyeingBatch();
		KnitDyeingGrayChallan _oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
		List<KnitDyeingGrayChallan> _oKnitDyeingGrayChallans = new  List<KnitDyeingGrayChallan>();
		#endregion

		#region Actions
        public ActionResult ViewKnitDyeingGrayChallan(int nId, int nBUID, double ts)
        {
            _oKnitDyeingBatch = new KnitDyeingBatch();
            _oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
            KnitDyeingBatchDetail oKnitDyeingBatchDetail = new KnitDyeingBatchDetail();
            List<KnitDyeingBatchDetail> oKnitDyeingBatchDetails = new List<KnitDyeingBatchDetail>();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            if (nId > 0)
            {
                _oKnitDyeingBatch = _oKnitDyeingBatch.Get(nId, (int)Session[SessionInfo.currentUserID]);
                _oKnitDyeingBatch.KnitDyeingBatchDetails = KnitDyeingBatchDetail.Gets("SELECT * FROM View_KnitDyeingBatchDetail WHERE KnitDyeingBatchID = " + nId, (int)Session[SessionInfo.currentUserID]);
                _oKnitDyeingGrayChallans = KnitDyeingGrayChallan.Gets("SELECT * FROM View_KnitDyeingGrayChallan WHERE KnitDyeingBatchID = " + nId, (int)Session[SessionInfo.currentUserID]);
                oWorkingUnits = WorkingUnit.GetsPermittedStoreByStoreName(nBUID, EnumModuleName.KnitDyeingBatch, EnumStoreType.IssueStore, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oKnitDyeingGrayChallans.Count > 0)
                {
                    _oKnitDyeingGrayChallan = _oKnitDyeingGrayChallans[0];
                    _oKnitDyeingGrayChallan.KnitDyeingGrayChallanDetails = KnitDyeingGrayChallanDetail.Gets("SELECT * FROM View_KnitDyeingGrayChallanDetail WHERE KnitDyeingGrayChallanID = " + _oKnitDyeingGrayChallan.KnitDyeingGrayChallanID, ((int)Session[SessionInfo.currentUserID]));
                }
            }
            ViewBag.KnitDyeingBatch = _oKnitDyeingBatch;
            ViewBag.WorkingUnits = oWorkingUnits;
            return View(_oKnitDyeingGrayChallan);
        }

		[HttpPost]
		public JsonResult Save(KnitDyeingGrayChallan oKnitDyeingGrayChallan)
		{
			_oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
			try
			{
				_oKnitDyeingGrayChallan = oKnitDyeingGrayChallan;
				_oKnitDyeingGrayChallan = _oKnitDyeingGrayChallan.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
				_oKnitDyeingGrayChallan.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oKnitDyeingGrayChallan);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		} 

		[HttpGet]
		public JsonResult Delete(int id)
		{
			string sFeedBackMessage = "";
			try
			{
				KnitDyeingGrayChallan oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
				sFeedBackMessage = oKnitDyeingGrayChallan.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Disburse(KnitDyeingGrayChallan oKnitDyeingGrayChallan)
        {
            _oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
            try
            {
                if (oKnitDyeingGrayChallan.KnitDyeingGrayChallanID > 0)
                {
                    _oKnitDyeingGrayChallan = oKnitDyeingGrayChallan.Disburse((int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
                _oKnitDyeingGrayChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingGrayChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 
            
        public ActionResult PrintKnitDyeingChallan(int id)
        {
            KnitDyeingBatch _oKnitDyeingBatch = new KnitDyeingBatch();
            KnitDyeingGrayChallan _oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
            List<KnitDyeingGrayChallan> _oKnitDyeingGrayChallans = new List<KnitDyeingGrayChallan>();

            _oKnitDyeingBatch = _oKnitDyeingBatch.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oKnitDyeingGrayChallans = KnitDyeingGrayChallan.Gets("SELECT * FROM View_KnitDyeingGrayChallan WHERE KnitDyeingBatchID = " + _oKnitDyeingBatch.KnitDyeingBatchID, (int)Session[SessionInfo.currentUserID]);
            if (_oKnitDyeingGrayChallans.Count() > 0)
            {
                _oKnitDyeingGrayChallan = _oKnitDyeingGrayChallans[0];
                _oKnitDyeingGrayChallan.KnitDyeingGrayChallanDetails = KnitDyeingGrayChallanDetail.Gets("SELECT * FROM View_KnitDyeingGrayChallanDetail WHERE KnitDyeingGrayChallanID = " + _oKnitDyeingGrayChallan.KnitDyeingGrayChallanID, ((int)Session[SessionInfo.currentUserID]));
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oKnitDyeingBatch.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.KnitDyeingGrayChallan, (int)Session[SessionInfo.currentUserID]);

            rptKnitDyeingGrayChallan oReport = new rptKnitDyeingGrayChallan();
            byte[] abytes = oReport.PrepareReport(_oKnitDyeingBatch, _oKnitDyeingGrayChallan, oCompany, oBusinessUnit, oSignatureSetups);
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
