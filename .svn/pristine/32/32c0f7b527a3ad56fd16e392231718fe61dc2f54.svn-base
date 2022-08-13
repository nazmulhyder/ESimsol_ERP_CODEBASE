using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
 
using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Web.Services;

namespace ESimSolFinancial.Controllers
{
    public class FeatureController : Controller
    {
        #region Declaration
        Feature _oFeature = new Feature();
        List<Feature> _oFeatures = new List<Feature>();
        string _sErrorMessage = "";
        #endregion

        #region Functions

        #endregion

        public ActionResult ViewFeatures(int menuid)
        {
            
	        this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Feature).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oFeatures = new List<Feature>();
            _oFeatures = Feature.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oFeatures);
        }

        public ActionResult ViewFeature(int id)
        {
            _oFeature = new Feature();
            if (id > 0)
            {
                _oFeature = _oFeature.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM View_VehicleModel";
            ViewBag.VehicleModels = VehicleModel.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.FeatureTypes = EnumObject.jGets(typeof(EnumFeatureType));
            return View(_oFeature);
        }

        [HttpPost]
        public JsonResult Save(Feature oFeature)
        {
            _oFeature = new Feature();
            try
            {
                _oFeature = oFeature;                
                _oFeature = _oFeature.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFeature = new Feature();
                _oFeature.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFeature);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                Feature oFeature = new Feature();
                sFeedBackMessage = oFeature.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchColor(string sTemp, double ts)
        {
            List<Feature> oFeatures = new List<Feature>();
            string sFTypes = (int)EnumFeatureType.StandardFeature + "," + (int)EnumFeatureType.SafetyFeature + "," + (int)EnumFeatureType.InteriorFeature + "," + (int)EnumFeatureType.ExteriorFeature + "," + (int)EnumFeatureType.CountrySettingFeature + "," + (int)EnumFeatureType.OptionalFeature;
            oFeatures = Feature.GetsbyFeatureNameWithType(sTemp,sFTypes, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oFeatures);
        }

        public ActionResult FeatureSearch()
        {
            List<Feature> oFeatures = new List<Feature>();            
            return PartialView(oFeatures);
        }
        #region Searching

             
     
        [HttpPost]
        public JsonResult Gets()
        {
            List<Feature> oFeatures = new List<Feature>();
            oFeatures = Feature.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oFeatures);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchColor(Feature oFeature)
        {
            List<Feature> oFeatures = new List<Feature>();
            if (oFeature.Param == null || oFeature.Param == "")
            {
                oFeatures = Feature.Gets((int)Session[SessionInfo.currentUserID]);
            }
            {
                string sFTypes = (int)EnumFeatureType.StandardFeature + "," + (int)EnumFeatureType.SafetyFeature + "," + (int)EnumFeatureType.InteriorFeature + "," + (int)EnumFeatureType.ExteriorFeature + "," + (int)EnumFeatureType.CountrySettingFeature + "," + (int)EnumFeatureType.OptionalFeature;
                oFeatures = Feature.GetsbyFeatureNameWithType(oFeature.Param,sFTypes, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFeatures);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print List
        public ActionResult PrintList(string sIDs)
        {

            _oFeature = new Feature();
            string sSQL = "SELECT * FROM View_Feature WHERE FeatureID IN (" + sIDs + ")";
            _oFeature.ColorCategories = Feature.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptFeatureList oReport = new rptFeatureList();
            byte[] abytes = oReport.PrepareReport(_oFeature, oCompany);
            return File(abytes, "application/pdf");
        }

        public void FeatureExportToExcel(string sIDs)
        {

            _oFeature = new Feature();
            string sSQL = "SELECT * FROM Feature WHERE FeatureID IN (" + sIDs + ")";
            _oFeature.ColorCategories = Feature.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = null;
            rptFeatureList oReport = new rptFeatureList();
            PdfPTable oPdfPTable = oReport.PrepareExcel(_oFeature, oCompany);

            ExportToExcel.WorksheetName = "Model Category";
            byte[] abytes = ExportToExcel.ConvertToExcel(oPdfPTable);

            Response.ClearContent();
            Response.BinaryWrite(abytes);
            Response.AddHeader("content-disposition", "attachment; filename=Feature.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
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
