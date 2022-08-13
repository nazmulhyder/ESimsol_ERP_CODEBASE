using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;


namespace ESimSolFinancial.Controllers
{
    public class ProductionStepController : Controller
    {
        #region Declaration
        ProductionStep _oProductionStep = new ProductionStep();
        List<ProductionStep> _oProductionSteps = new List<ProductionStep>();
        string _sErrorMessage = "";
        #endregion

        #region Functions

        #endregion

        public ActionResult ViewProductionSteps(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ProductionStep).ToString() , (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oProductionSteps = new List<ProductionStep>();
            _oProductionSteps = ProductionStep.Gets((int)Session[SessionInfo.currentUserID]);            
            return View(_oProductionSteps);
        }

        public ActionResult ViewProductionStep(int id)
        {
            _oProductionStep = new ProductionStep();
            if (id > 0)
            {
                _oProductionStep = _oProductionStep.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oProductionStep.ProductionStepTypeObjs = EnumObject.jGets(typeof(EnumProductionStepType));//ProductionStepTypeObj.Gets();
            return View(_oProductionStep);
        }

        [HttpPost]
        public JsonResult Save(ProductionStep oProductionStep)
        {
            _oProductionStep = new ProductionStep();
            try
            {
                _oProductionStep = oProductionStep;
                _oProductionStep = _oProductionStep.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionStep = new ProductionStep();
                _oProductionStep.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionStep);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                ProductionStep oProductionStep = new ProductionStep();
                sFeedBackMessage = oProductionStep.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region PrintList
        public ActionResult PrintList(string sIDs)
        {
            _oProductionStep = new ProductionStep();
            string sSQL = "SELECT * FROM ProductionStep WHERE ProductionStepID IN (" + sIDs + ")";
            _oProductionStep.ProductionStepList = ProductionStep.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptProductionStepList oReport = new rptProductionStepList();
            byte[] abytes = oReport.PrepareReport(_oProductionStep, oCompany);
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
