using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using System.Drawing.Printing;
using System.Reflection;


namespace ESimSolFinancial.Controllers
{
    public class FNRecipeLabController : Controller
    {
        #region Declaration
        FNRecipeLab _oFNRecipeLab = new FNRecipeLab();
        List<FNRecipeLab> _oFNRecipeLabs = new List<FNRecipeLab>();
        string _sErrorMessage = "";

        #endregion

        #region FNRecipeLab Class
        public ActionResult ViewFNRecipeLabs(int nId, int buid)
        {
            FNLabDipDetail oFNLabDipDetail = new FNLabDipDetail();
            oFNLabDipDetail = FNLabDipDetail.Get(nId, (int)Session[SessionInfo.currentUserID]);

            FNRecipeLab oFNRecipeLab = new FNRecipeLab();
            List<FNRecipeLab> oFNRecipeLabs = new List<FNRecipeLab>();
            //oFNRecipeLabs = FNRecipeLab.Gets("SELECT * FROM View_FNRecipeLab WHERE FNLDDID =" + oFNLabDipDetail.FNLabDipDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FNLabDipDetail = oFNLabDipDetail;
            ViewBag.BUID = buid;
            ViewBag.Shades = EnumObject.jGets(typeof(EnumShade));
            ViewBag.FNTreatmentProcessList = oFNRecipeLabs;
            //ViewBag.FNTreatmentProcessList = FNRecipeLab.Gets("select * from View_FNRecipeLab where FNLDDID = '" + nId + "' AND IsProcess = 'true' ORDER BY FNTreatment, Code", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(oFNRecipeLabs);
        }
        #endregion

        #region Functions
        [HttpPost]
        public JsonResult GetsFNRecipeLabByTreatmentProcessAndFNLDDID(FNRecipeLab oFNRecipeLab)
        {
            _oFNRecipeLab = new FNRecipeLab();
            List<FNRecipeLab> oFNRecipeLabs = new List<FNRecipeLab>();
            try
            {
                oFNRecipeLabs = FNRecipeLab.Gets("SELECT * FROM View_FNRecipeLab WHERE FNLDDID = '" + oFNRecipeLab.FNLDDID + "' AND FNTPID = '" + oFNRecipeLab.FNTPID + "' AND ShadeID = '" + (int)oFNRecipeLab.ShadeID + "' AND IsProcess = 'false'", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRecipeLab = new FNRecipeLab();
                _oFNRecipeLab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNRecipeLabs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsProcessByShade(FNRecipeLab oFNRecipeLab)
        {
            List<FNRecipeLab> oFNRecipeLabs = new List<FNRecipeLab>();
            try
            {
                oFNRecipeLabs = FNRecipeLab.Gets("select * from View_FNRecipeLab where FNLDDID = '" + oFNRecipeLab.FNLDDID + "'AND ShadeID = " + (int)oFNRecipeLab.ShadeID + " AND IsProcess = 'true' ORDER BY FNTreatment, Code", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oFNRecipeLab = new FNRecipeLab();
                _oFNRecipeLab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNRecipeLabs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CopyShadeSave(FNRecipeLab oFNRecipeLab)
        {
            List<FNRecipeLab> objFNRecipeLabs = new List<FNRecipeLab>();
            try
            {

                if (!string.IsNullOrEmpty(oFNRecipeLab.ErrorMessage))
                {
                    int nFNLDDID = Convert.ToInt32(oFNRecipeLab.ErrorMessage.Split('~')[0]);
                    int nShadeID = Convert.ToInt32(oFNRecipeLab.ErrorMessage.Split('~')[1]);
                    int nShadeIDCopy = Convert.ToInt32(oFNRecipeLab.ErrorMessage.Split('~')[2]);
                    int nFNLabDipDetailID = Convert.ToInt32(oFNRecipeLab.ErrorMessage.Split('~')[3]);
                    objFNRecipeLabs = oFNRecipeLab.CopyShadeSave(nFNLDDID, nShadeID, nShadeIDCopy, nFNLabDipDetailID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oFNRecipeLab = new FNRecipeLab();
                _oFNRecipeLab.ErrorMessage = ex.Message;
                objFNRecipeLabs.Add(_oFNRecipeLab);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(objFNRecipeLabs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save(List<FNRecipeLab> oFNRecipeLabs)
        {
            _oFNRecipeLab = new FNRecipeLab();
            List<FNRecipeLab> _oFNRecipeLabs = new List<FNRecipeLab>();
            _oFNRecipeLabs = oFNRecipeLabs;
            try
            {
                foreach (FNRecipeLab oItem in _oFNRecipeLabs)
                {
                    _oFNRecipeLab = oItem;
                    _oFNRecipeLab = _oFNRecipeLab.Save((int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oFNRecipeLab = new FNRecipeLab();
                _oFNRecipeLab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNRecipeLabs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveProcess(FNRecipeLab oFNRecipeLab)
        {
            _oFNRecipeLab = oFNRecipeLab;
            try
            {
                _oFNRecipeLab = _oFNRecipeLab.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRecipeLab = new FNRecipeLab();
                _oFNRecipeLab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNRecipeLab);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDyes(FNRecipeLab oFNRecipeLab)
        {
            _oFNRecipeLab = oFNRecipeLab;
            try
            {
                _oFNRecipeLab.ErrorMessage = _oFNRecipeLab.Delete(oFNRecipeLab.FNRecipeLabID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRecipeLab = new FNRecipeLab();
                _oFNRecipeLab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNRecipeLab);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteProcess(FNRecipeLab oFNRecipeLab)
        {
            _oFNRecipeLab = oFNRecipeLab;
            try
            {
                _oFNRecipeLab.ErrorMessage = _oFNRecipeLab.DeleteProcess(oFNRecipeLab.FNRecipeLabID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRecipeLab = new FNRecipeLab();
                _oFNRecipeLab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNRecipeLab);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Print(int nFNLDDID, int nBUID, int nShadeID)
        {
            FNLabDipDetail oFNLabDipDetail = new FNLabDipDetail();
            oFNLabDipDetail = FNLabDipDetail.Get(nFNLDDID, (int)Session[SessionInfo.currentUserID]);

            List<FNRecipeLab> oFNRecipeLabs = new List<FNRecipeLab>();
            oFNRecipeLabs = FNRecipeLab.Gets("select * from View_FNRecipeLab where FNLDDID = '" + nFNLDDID + "' AND ShadeID = '" + nShadeID + "' Order by FNTreatment, FNProcess", (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);

            rptFNRecipeLab oReport = new rptFNRecipeLab();
            rptErrorMessage oErrorReport = new rptErrorMessage();
            string sName = "";
            if(oFNRecipeLabs.Count>0)
            {
                sName = oFNRecipeLabs[0].PrepareByName;
            }
            byte[] abytes = new byte[1];
            abytes = oReport.PrepareReport(oCompany, oFNLabDipDetail, oFNRecipeLabs, oBusinessUnit, sName);
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
