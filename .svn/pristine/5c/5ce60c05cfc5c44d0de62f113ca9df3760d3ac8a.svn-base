using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
 
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;

namespace ESimSolFinancial.Controllers
{
    public class PackageTemplateController : Controller
    {



        #region Declartion
        PackageTemplate _oPackageTemplate = new PackageTemplate();
        List<PackageTemplate> _oPackageTemplates = new List<PackageTemplate>();
        PackageTemplateDetail _oPackageTemplateDetail = new PackageTemplateDetail();
        List<PackageTemplateDetail> _oPackageTemplateDetails = new List<PackageTemplateDetail>();
    

        #endregion

        #region Package Template Issue
        public ActionResult ViewPackageTemplates(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.PackageTemplate).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oPackageTemplates = new List<PackageTemplate>();            
            _oPackageTemplates = PackageTemplate.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oPackageTemplates);
        }
        #endregion



        #region Add, Edit, Delete

        #region Package Template
        public ActionResult ViewPackageTemplate(int id)
        {
            _oPackageTemplate = new PackageTemplate();
            if (id > 0)
            {
                _oPackageTemplate = _oPackageTemplate.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oPackageTemplate.PackageTemplateDetails = PackageTemplateDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oPackageTemplate);
        }
        #endregion

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(PackageTemplate oPackageTemplate)
        {
            _oPackageTemplate = new PackageTemplate();
            try
            {
                _oPackageTemplate = oPackageTemplate.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPackageTemplate = new PackageTemplate();
                _oPackageTemplate.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPackageTemplate);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HTTP GET Delete
        [HttpPost]
        public JsonResult Delete(PackageTemplate oPackageTemplate)
        {
            string smessage = "";
            try
            {
                smessage = oPackageTemplate.Delete(oPackageTemplate.PackageTemplateID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                smessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(smessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #endregion

        #region Package Template Picker
        public ActionResult PackageTemplatePicker()
        {
            return PartialView();
        }


        [HttpPost]
        public JsonResult GetPackageTemplates(PackageTemplate oPackageTemplate)
        {
            _oPackageTemplate = new PackageTemplate();
            try
            {
                _oPackageTemplate.PackageTemplateList = PackageTemplate.Gets((int)Session[SessionInfo.currentUserID]);
                _oPackageTemplate.PackageTemplateDetails = PackageTemplateDetail.Gets((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPackageTemplate = new PackageTemplate();
                _oPackageTemplate.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPackageTemplate);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion 


        //#region Print and Preview
        //public ActionResult PackageTemplatePrintList()
        //{
        //    _oPackageTemplate = new PackageTemplate();
        //    string sSQL = "";
        //    sSQL = "Select * from View_PackageTemplate WHERE OrderType =" + (int)EnumPackageTemplateType.PackageTemplate;
        //    _oPackageTemplate.PackageTemplates = PackageTemplate.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    _oPackageTemplate.Company = oCompany;

        //    rptPackageTemplateList oReport = new rptPackageTemplateList();
        //    if (_oPackageTemplate.PackageTemplates.Count > 0)
        //    {
        //        string sMessage = "Supply Order List";
        //        byte[] abytes = oReport.PrepareReport(_oPackageTemplate, sMessage);
        //        return File(abytes, "application/pdf");
        //    }
        //    else
        //    {

        //        string sMessage = "There is no data for print";
        //        return RedirectToAction("MessageHelper", "User", new { message = sMessage });
        //    }
        //}

        //public ActionResult PackageTemplatePreview(int id)
        //{
        //    _oPackageTemplate = new PackageTemplate();
        //    Contractor oContractor = new Contractor();
        //    ContactPersonnel oContactPersonnel = new ContactPersonnel();
        //    Company oCompany = new Company();
        //    _oPackageTemplate = _oPackageTemplate.Get(id, (int)Session[SessionInfo.currentUserID]);
        //    _oPackageTemplate.PackageTemplateDetails = PackageTemplateDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
        //    _oPackageTemplate.Contractor = oContractor.Get(_oPackageTemplate.ProductionFactoryID, (int)Session[SessionInfo.currentUserID]);
        //    _oPackageTemplate.ContactPersonnel = oContactPersonnel.Get(_oPackageTemplate.SuppplierContactPersonID, (int)Session[SessionInfo.currentUserID]); // need to change 5 temporary value
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    _oPackageTemplate.Company = oCompany;

        //    rptPackageTemplateIssue oReport = new rptPackageTemplateIssue();
        //    string sMessage = "Supply Order Preivew";
        //    byte[] abytes = oReport.PrepareReportPreview(_oPackageTemplate, sMessage);
        //    return File(abytes, "application/pdf");
        //}

        //#endregion Print

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


    }
}
