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
	public class FADepreciationController : Controller
	{
		#region Declaration

		FADepreciation _oFADepreciation = new FADepreciation();
		List<FADepreciation> _oFADepreciations = new  List<FADepreciation>();
        List<FADepreciationDetail> _oFADepreciationDetails = new List<FADepreciationDetail>();
		#endregion

		#region Functions

		#endregion

		#region Actions
		public ActionResult ViewFADepreciations(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FADepreciation).ToString() , (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oFADepreciations = new List<FADepreciation>(); 
			
            @ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
			return View(_oFADepreciations);
		}

        public ActionResult ViewFADepreciation(int id, double ts)
		{
			_oFADepreciation = new FADepreciation();
			if (id > 0)
			{
				_oFADepreciation = _oFADepreciation.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oFADepreciation.FADepreciationDetails = FADepreciationDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
			}
			return View(_oFADepreciation);
		}

        [HttpPost]
        public JsonResult Search(FADepreciation oFADepreciation)
        {
            _oFADepreciations = new List<FADepreciation>();
            try
            {
                DateTime dStartDate = Convert.ToDateTime(oFADepreciation.Params.Split('~')[0]);
                DateTime dEndDate = Convert.ToDateTime(oFADepreciation.Params.Split('~')[1]);
                int BUID = Convert.ToInt32(oFADepreciation.Params.Split('~')[2]);
                string sSQL = "SELECT * FROM View_FADepreciation WHERE FADepreciationID!=0 AND DepreciationDate Between '" + dStartDate.ToString("dd MMM yyyy") + "' AND '" + dEndDate.ToString("dd MMM yyyy") + "'";
                if (BUID != 0) { sSQL += " AND BUID = " + BUID; }
                _oFADepreciations = FADepreciation.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFADepreciation = new FADepreciation();
                _oFADepreciation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFADepreciations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 

		[HttpPost]
		public JsonResult Save(FADepreciation oFADepreciation)
		{
			_oFADepreciation = new FADepreciation();
			try
			{
				_oFADepreciation = oFADepreciation;
				_oFADepreciation = _oFADepreciation.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oFADepreciation = new FADepreciation();
				_oFADepreciation.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oFADepreciation);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult Approval(FADepreciation oFADepreciation)
        {
            _oFADepreciation = new FADepreciation();
            try
            {
                _oFADepreciation = oFADepreciation;
                _oFADepreciation = _oFADepreciation.Approval((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFADepreciation = new FADepreciation();
                _oFADepreciation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFADepreciation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 

		[HttpPost]
        public JsonResult Delete(FADepreciation oFADepreciation)
		{
			string sFeedBackMessage = "";
			try
			{
                sFeedBackMessage = oFADepreciation.Delete(oFADepreciation.FADepreciationID, (int)Session[SessionInfo.currentUserID]);
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

        #region Print
        public ActionResult PrintFADepreciation(int id)
        {
            _oFADepreciation = new FADepreciation();
            _oFADepreciation = _oFADepreciation.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFADepreciation.FADepreciationDetails = FADepreciationDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oFADepreciation.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptFADepreciation oReport = new rptFADepreciation();
            byte[] abytes = oReport.PrepareReport(_oFADepreciation, oBusinessUnit, oCompany);
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
