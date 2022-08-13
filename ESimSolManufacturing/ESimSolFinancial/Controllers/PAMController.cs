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
using System.Text;

namespace ESimSolFinancial.Controllers
{
	public class PAMController : Controller
	{
		#region Declaration

		PAM _oPAM = new PAM();
		List<PAM> _oPAMs = new  List<PAM>();
        List<PAMDetail> _oPAMDetails = new List<PAMDetail>();

		#endregion

		#region Functions

		#endregion

		#region Actions

        public ActionResult ViewPAMs(int id, double ts)
		{

			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.PAM).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            TechnicalSheet _oTechnicalSheet = new TechnicalSheet();
			_oPAMs = new List<PAM>(); 
			_oPAMs = PAM.Gets(id, (int)Session[SessionInfo.currentUserID]);
            @ViewBag.TechnicalSheet = _oTechnicalSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
			return View(_oPAMs);
		}
        public ActionResult ViewAddMultiPAMs(int id)
        {

            _oPAM = new PAM();

            return View(_oPAM);

        }
            [HttpPost]
        public JsonResult GetPAM(PAM oPAM)
        {
            _oPAM = new PAM();
             try
			{
                if (oPAM.PAMID > 0)
                {
                    _oPAM = _oPAM.Get(oPAM.PAMID, (int)Session[SessionInfo.currentUserID]);
                    _oPAM.PAMDetailLst = PAMDetail.Gets(oPAM.PAMID, (int)Session[SessionInfo.currentUserID]);
                }
            }
             catch (Exception ex)
             {
                 _oPAM = new PAM();
                 _oPAM.ErrorMessage = ex.Message;
             }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPAM);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPAMs(PAM oPAM)
        {
            _oPAMs = new List<PAM>();
             try
			{
                if (oPAM.StyleID > 0)
                {
                    _oPAMs = PAM.Gets(oPAM.StyleID, (int)Session[SessionInfo.currentUserID]);
                }
            }
             catch (Exception ex)
             {
                 _oPAM = new PAM();
                 _oPAM.ErrorMessage = ex.Message;
             }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPAMs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

		[HttpPost]
		public JsonResult Save(PAM oPAM)
		{
			_oPAM = new PAM();
			try
			{
				_oPAM = oPAM;
				_oPAM = _oPAM.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oPAM = new PAM();
				_oPAM.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oPAM);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult SaveMultiPAM(PAM oPAM)
        {
            string sFeedBackMessage = "";
            _oPAM = new PAM();
            try
            {
                _oPAM.PAMDetailLst = oPAM.PAMDetailLst.OrderBy(x => x.ForwardWeek).ToList();
                sFeedBackMessage = _oPAM.SaveMultiPAM((int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Revise(PAM oPAM)
		{
			_oPAM = new PAM();
			try
			{
				_oPAM = oPAM;
                _oPAM = _oPAM.Revise((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oPAM = new PAM();
				_oPAM.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oPAM);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}
        
        [HttpPost]
        public JsonResult Approve(PAM oPAM)
        {
            _oPAM = new PAM();
            try
            {
                _oPAM = oPAM;
                _oPAM = _oPAM.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPAM = new PAM();
                _oPAM.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPAM);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Delete(PAM oPAM)
		{
			string sFeedBackMessage = "";
			try
			{
				
                sFeedBackMessage = oPAM.Delete(oPAM.PAMID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetColorByStyle(ColorCategory oColor)
        {
            List<ColorCategory> oColors = new List<ColorCategory>();
            _oPAMDetails = new List<PAMDetail>();
            try
            {
                StringBuilder sSQL = new StringBuilder("SELECT * FROM ColorCategory WHERE ColorCategoryID IN (SELECT ColorCategoryID FROM TechnicalSheetColor WHERE TechnicalSheetID = " + oColor .ObjectID+ ")");
                if (!string.IsNullOrEmpty(oColor.ColorName)) { sSQL.Append(" AND ColorName LIKE '%"+oColor.ColorName+"%'"); }
                oColors = ColorCategory.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);
                foreach(ColorCategory oItem in oColors)
                {
                    PAMDetail oPAMDetail = new PAMDetail();
                    oPAMDetail.ColorID = oItem.ColorCategoryID;
                    oPAMDetail.ColorName = oItem.ColorName;
                    _oPAMDetails.Add(oPAMDetail);
                }
            }
            catch (Exception ex)
            {
                PAMDetail oPAMDetail = new PAMDetail();
                oPAMDetail.ErrorMessage = ex.Message;
                _oPAMDetails.Add(oPAMDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPAMDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 
		#endregion

        #region Print
        public ActionResult PAMPrint(int StyleID, int PAMID)
        {
            StringBuilder sSQL = new StringBuilder();
            if (PAMID > 0)
            {
                sSQL.Append("SELECT * FROM View_PAMDetail WHERE PAMID=" + PAMID + "");
            }
            else
            {
                sSQL.Append("SELECT * FROM View_PAMDetail WHERE PAMID IN (SELECT PAMID FROM PAM WHERE StyleID= " + StyleID + ")");
            }
            List<PAMDetail> oPAMDetails = new List<PAMDetail>();
            oPAMDetails = PAMDetail.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            rptPAM oReport = new rptPAM();
            byte[] abytes = oReport.PrepareReport(oPAMDetails, oCompany);
            return File(abytes, "application/pdf");
        }
        #endregion

        [HttpPost]
        public JsonResult GetsColor(TechnicalSheetColor oTechnicalSheetColor)//For MultiPAMs
        {
            List<TechnicalSheetColor> _oTechnicalSheetColors=new List<TechnicalSheetColor>();
            try
            {
                 
                    _oTechnicalSheetColors = TechnicalSheetColor.Gets(oTechnicalSheetColor.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {

                TechnicalSheetColor _oTechnicalSheetColor = new TechnicalSheetColor();
                _oTechnicalSheetColor.ErrorMessage = ex.Message;
                _oTechnicalSheetColors.Add(_oTechnicalSheetColor);

            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTechnicalSheetColors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


    }

}
