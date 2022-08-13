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


namespace ESimSolFinancial.Controllers
{
    public class DiagramIdentificationController : Controller
    {
        #region Declaration
        DiagramIdentification _oDiagramIdentification = new DiagramIdentification();
        List<DiagramIdentification> _oDiagramIdentifications = new List<DiagramIdentification>();
        string _sErrorMessage = "";

        #endregion

        public ActionResult ViewDiagramIdentifications(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DiagramIdentification).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oDiagramIdentifications = new List<DiagramIdentification>();
            _oDiagramIdentifications = DiagramIdentification.Gets((int)Session[SessionInfo.currentUserID]);            
            return View(_oDiagramIdentifications);
        }

        public ActionResult ViewDiagramIdentification(int id)
        {
            _oDiagramIdentification = new DiagramIdentification();
            if (id > 0)
            {
                _oDiagramIdentification = _oDiagramIdentification.Get(id, (int)Session[SessionInfo.currentUserID]);
            }

            return View(_oDiagramIdentification);
        }


        #region Searching
        public ActionResult DiagramIdentificationSearch()
        {
            List<DiagramIdentification> oDiagramIdentifications = new List<DiagramIdentification>();
            oDiagramIdentifications = DiagramIdentification.Gets((int)Session[SessionInfo.currentUserID]);
            return PartialView(oDiagramIdentifications);
        }

        [HttpPost]
        public JsonResult Gets()
        {
            List<DiagramIdentification> oDiagramIdentifications = new List<DiagramIdentification>();
            oDiagramIdentifications = DiagramIdentification.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDiagramIdentifications);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DiagramIdentificationSearch(DiagramIdentification oDiagramIdentification)
        {
            List<DiagramIdentification> oDiagramIdentifications = new List<DiagramIdentification>();
            if (oDiagramIdentification.Param == "" || oDiagramIdentification.Param==null)
            {
                oDiagramIdentifications = DiagramIdentification.Gets((int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oDiagramIdentifications = DiagramIdentification.GetsByName(oDiagramIdentification.Param, (int)Session[SessionInfo.currentUserID]);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDiagramIdentifications);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Report Study
        public ActionResult PrintDiagramIdentification()
        {

            _oDiagramIdentification = new DiagramIdentification();
            string sSQL = "SELECT * FROM DiagramIdentification";
            _oDiagramIdentification.DiagramIdentificationList = DiagramIdentification.Gets_print(sSQL, (int)Session[SessionInfo.currentUserID]);
            ////return this.ViewPdf("DiagramIdentification List", "rptDiagramIdentification", _oDiagramIdentification);
            //return this.ViewPdf("DiagramIdentification List", "rptDiagramIdentification", _oDiagramIdentification, 40, 40, 40, 40, false);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptDiagramIdentificationList oReport = new rptDiagramIdentificationList();
            byte[] abytes = oReport.PrepareReport(_oDiagramIdentification, oCompany);
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

      


        [HttpPost]
        public JsonResult Save(DiagramIdentification oDiagramIdentification)
        {
            _oDiagramIdentification = new DiagramIdentification();
            try
            {
                _oDiagramIdentification = oDiagramIdentification;
                _oDiagramIdentification = _oDiagramIdentification.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDiagramIdentification = new DiagramIdentification();
                _oDiagramIdentification.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDiagramIdentification);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(DiagramIdentification oDiagramIdentification)
        {
            string sFeedBackMessage = "";
            try
            {

                sFeedBackMessage = oDiagramIdentification.Delete(oDiagramIdentification.DiagramIdentificationID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
