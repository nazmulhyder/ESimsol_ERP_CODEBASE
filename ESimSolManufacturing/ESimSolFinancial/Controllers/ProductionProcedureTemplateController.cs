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
    public class ProductionProcedureTemplateController : Controller
    {
        #region Declartion
        ProductionProcedureTemplate _oProductionProcedureTemplate = new ProductionProcedureTemplate();
        List<ProductionProcedureTemplate> _oProductionProcedureTemplates = new List<ProductionProcedureTemplate>();
        ProductionProcedureTemplateDetail _oProductionProcedureTemplateDetail = new ProductionProcedureTemplateDetail();
        List<ProductionProcedureTemplateDetail> _oProductionProcedureTemplateDetails = new List<ProductionProcedureTemplateDetail>();
        #endregion

        #region ProductionProcedureTemplate Issue
        public ActionResult ViewProductionProcedureTemplates(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ProductionProcedureTemplate).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oProductionProcedureTemplates = new List<ProductionProcedureTemplate>();            
            _oProductionProcedureTemplates = ProductionProcedureTemplate.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oProductionProcedureTemplates);
        }
        #endregion



        #region Add, Edit, Delete

        #region ProductionProcedureTemplate
        public ActionResult ViewProductionProcedureTemplate(int id)
        {
            _oProductionProcedureTemplate = new ProductionProcedureTemplate();
            if (id > 0)
            {
                _oProductionProcedureTemplate = _oProductionProcedureTemplate.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionProcedureTemplate.ProductionProcedureTemplateDetails = ProductionProcedureTemplateDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oProductionProcedureTemplate.ProductionSteps = ProductionStep.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oProductionProcedureTemplate);
        }
        #endregion

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(ProductionProcedureTemplate oProductionProcedureTemplate)
        {
            _oProductionProcedureTemplate = new ProductionProcedureTemplate();
            try
            {
                _oProductionProcedureTemplate = oProductionProcedureTemplate.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionProcedureTemplate = new ProductionProcedureTemplate();
                _oProductionProcedureTemplate.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionProcedureTemplate);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HTTP GET Delete
        [HttpGet]
        public JsonResult Delete(int nProductionProcedureTemplateID)
        {
            string smessage = "";
            try
            {
                ProductionProcedureTemplate oProductionProcedureTemplate = new ProductionProcedureTemplate();
                smessage = oProductionProcedureTemplate.Delete(nProductionProcedureTemplateID, (int)Session[SessionInfo.currentUserID]);

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

        #region ProductionProcedureTemplate Picker
        public ActionResult ProductionProcedureTemplatePicker()
        {
            _oProductionProcedureTemplate = new ProductionProcedureTemplate();
            _oProductionProcedureTemplate.ProductionProcedureTemplates = ProductionProcedureTemplate.Gets((int)Session[SessionInfo.currentUserID]);
            _oProductionProcedureTemplate.ProductionProcedureTemplateDetails = ProductionProcedureTemplateDetail.Gets((int)Session[SessionInfo.currentUserID]);
            return PartialView(_oProductionProcedureTemplate);
        }

        #endregion


        #region PrintList
        public ActionResult PrintList(string sIDs)
        {
            _oProductionProcedureTemplate = new ProductionProcedureTemplate();
            string sSQL = "SELECT * FROM ProductionProcedureTemplate WHERE ProductionProcedureTemplateID IN (" + sIDs + ")";
            _oProductionProcedureTemplate.ProductionProcedureTemplates = ProductionProcedureTemplate.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptProductionProcedureTemplateList oReport = new rptProductionProcedureTemplateList();
            byte[] abytes = oReport.PrepareReport(_oProductionProcedureTemplate, oCompany);
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
